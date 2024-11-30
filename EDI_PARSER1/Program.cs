using System.Text.Json.Serialization;
using EdiClasses;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using log4net;
using log4net.Config;
using System.IO;
using Microsoft.Azure.Cosmos;

public class Program
{
    private static readonly ILog _log = LogManager.GetLogger(typeof(Program));
    // Declaring CosmosDb settings as variables
    private static string EndpointUri;
    private static string PrimaryKey;
    private static string DatabaseId;
    private static string ContainerId;

    private static async Task Main(string[] args)
    {
        // Load configuration
        var config = LoadConfiguration();
        EndpointUri = config["CosmosDb:EndpointUri"];
        PrimaryKey = config["CosmosDb:PrimaryKey"];
        DatabaseId = config["CosmosDb:DatabaseId"];
        ContainerId = config["CosmosDb:ContainerId"];
        // Configure logging
        XmlConfigurator.Configure(new FileInfo("log4net.config"));
        _log.Info("Starting Parsing");
        CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
        Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId);
        Container container = await database.CreateContainerIfNotExistsAsync(ContainerId, "/ContainerNumber");

        // Define input and output folder paths
        string inputFolderPath = @"D:\In";
        string outputFolderPath = @"D:\Out";

        // Create output directory if it doesn't exist
        Directory.CreateDirectory(outputFolderPath);

        // JSON serializer options
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        // Get all file paths in the input folder
        string[] filePaths = Directory.GetFiles(inputFolderPath, "*.txt");
        foreach (var filePath in filePaths)
        {
            System.Console.WriteLine(filePath);
        }

        // Check if files are present in the input folder
        if (!filePaths.Any())
        {
            System.Console.WriteLine("No files found in the input folder.");
            _log.Error("No files found in the input folder.");
            return;  // Exit if no files are found
        }

        // // Process each file
        foreach (string filePath in filePaths)
        {
            _log.Info($"Processing file: {filePaths}");

            // Get the output file path
            string outputFilePath = Path.Combine(outputFolderPath, Path.GetFileNameWithoutExtension(filePath) + ".json");
            File.WriteAllText(outputFilePath, "");

            // Read the file data into an array of lines
            string[] fileData = File.ReadAllLines(filePath);

            // Ensure the file is not empty
            if (!fileData.Any())
            {
                System.Console.WriteLine($"File is empty: {filePath}. Cannot proceed with empty files.");
                _log.Error($"File is empty: {filePath}. Cannot proceed with empty files.");
                continue;
            }

            // List of required segments
            List<string> requiredSegments = new List<string>
            {
                "ISA", "GS", "ST", "B4", "N9", "Q2", "SG", "R4", "SE", "GE", "IEA"
            };

            // List to track found segments
            List<string> foundSegments = new List<string>();

            _log.Info("Started reading the data from the input file.");

            // Create a new message data object
            MsgData msgData = new MsgData
            {
                st_to_se = new List<St_to_SeSegment>()
            };
            St_to_SeSegment st_to_se = null;
            B4 b4 = new B4();

            // Loop through each line in the file
            foreach (string line in fileData)
            {


                string[] lineData = line.Split('*');

                // // Track the segments found in the file
                if (requiredSegments.Contains(lineData[0]) && !foundSegments.Contains(lineData[0]))
                {
                    foundSegments.Add(lineData[0]);
                }

                // Process the segment data based on its type
                switch (lineData[0])
                {
                    case "ISA":
                        msgData.IsaSegment = ISASegment.ProcessISA(lineData, msgData, _log);
                        break;
                    case "GS":
                        msgData.GsSegment = GSSegment.ProcessGS(lineData, msgData, _log);
                        break;
                    case "ST":
                        st_to_se = new St_to_SeSegment();

                        st_to_se.StSegment = STSegment.ProcessST(lineData, msgData, st_to_se, _log);

                        break;
                    case "B4":

                        b4 = B4Segment.ProcessB4(lineData, msgData, st_to_se, _log);
                        st_to_se.ContainerNumber = b4.ContainerNumber;
                        st_to_se.B4Segment.Add(b4);
                        break;
                    case "N9":
                        st_to_se.N9Segment.Add(N9Segment.ProcessN9(lineData, msgData, st_to_se, _log));
                        break;
                    case "Q2":
                        st_to_se.Q2Segment = (Q2Segment.ProcessQ2(lineData, msgData, st_to_se, _log));
                        break;
                    case "SG":
                        st_to_se.SgSegment.Add(SGSegment.ProcessSG(lineData, msgData, st_to_se, _log));
                        break;
                    case "R4":
                        st_to_se.R4Segment.Add(R4Segment.ProcessR4(lineData, msgData, st_to_se, _log));
                        break;
                    case "SE":
                        st_to_se.SeSegment = SESegment.ProcessSE(lineData, msgData, st_to_se, _log);
                        msgData.st_to_se.Add(st_to_se);
                        break;
                    case "GE":
                        GESegment.ProcessGE(lineData, msgData, _log);
                        break;
                    case "IEA":
                        IEASegment.ProcessIEA(lineData, msgData, _log);
                        break;
                }


            }
            foreach (var segment in requiredSegments)
            {
                if (!foundSegments.Contains(segment))
                {
                    throw new Exception($"Missing required segment: {segment} in file: {filePath}");
                }
            }
            foreach (var segment in msgData.st_to_se)
            {
                System.Console.WriteLine(segment.ContainerNumber);
                // Calculate FeesDue (demurrage fees)
                decimal feesDue = duedees(segment);

                // Initialize FeesPaid to 0 (or fetch from a payment system if available)
                decimal feesPaid = 0m;  // Initially set to 0, update once payment is made
                var containerDoc = new
                {
                    id = Guid.NewGuid().ToString(),
                    ContainerNumber = segment.ContainerNumber,
                    TradeType = segment.B4Segment.First().SpecialHandlingCode,
                    Status = segment.B4Segment.First().ShipmentStatusCode,
                    Origin = segment.R4Segment.First().PortOrTerminalFunctionCode,
                    VesselCode = segment.Q2Segment.VesselCode,
                    VesselName = segment.Q2Segment.VesselName,
                    FlightNumber = segment.Q2Segment.FlightNumber,
                    TransactionSetControlNumber = segment.SeSegment.TransactionSetControlNumber,
                    // Add Demurrage_fees with FeesDue and FeesPaid
                    Demurrage_fees = new
                    {
                        FeesDue = feesDue,      // The amount that is due for payment (demurrage fees)
                        FeesPaid = 0     // The amount that has been paid (will be updated as payment occurs)
                    }
                };
                await container.CreateItemAsync(containerDoc);



                // Check for any missing required segments


                // Serialize and write the output file
                string json = JsonSerializer.Serialize(msgData, options);
                File.WriteAllText(outputFilePath, json);
                System.Console.WriteLine($"The file is parsed successfully: {outputFilePath}");
                _log.Info($"Successfully saved processed data to {outputFilePath}");
            }

            // Save data to Cosmos DB
            // dbContext.MsgData.Add(msgData);
            // await dbContext.SaveChangesAsync();
            // _log.Info($"Data saved to Cosmos DB for file: {filePath}");

        }
        _log.Info("Parsing completed successfully.");
    }

    // Load configuration settings from appsettings.json
    public static IConfiguration LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json", optional: false, reloadOnChange: true)
            .Build();
    }
    // 5. Example method to calculate FeesDue (demurrage logic)
    // This method will calculate the demurrage fees based on your specific business logic.
    public static decimal duedees(dynamic segment)
    {
        // Ensure N9Segment is a List<EdiClasses.N9> or is convertible
        var n9SegmentList = segment.N9Segment as List<EdiClasses.N9>;

        if (n9SegmentList != null && n9SegmentList.Count > 4)
        {
            // Access the 5th element (index 4)
            var referenceIdentification = n9SegmentList[4].ReferenceIdentification;
            return decimal.TryParse(referenceIdentification, out var feeDue) ? feeDue : 0m;
        }

        return 0m;  // Return 0 if N9Segment doesn't have the expected data
    }
}
