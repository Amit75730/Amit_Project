using EdiWebAPI.Models;
using EdiWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EdiWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequiredFieldsController : ControllerBase
    {
        private readonly CosmosDbContext _context;
        private readonly ServiceBusSenderService _serviceBusSenderService;

        public RequiredFieldsController(CosmosDbContext context, ServiceBusSenderService serviceBusSenderService)
        {
            _context = context;
            _serviceBusSenderService = serviceBusSenderService;
        }

        // GET: api/RequiredFields
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequiredFields>>> GetRequiredFields()
        {
            try
            {
                var requiredFields = await _context.RequiredFields.ToListAsync();
                return Ok(requiredFields);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/RequiredFields/{ContainerNumber}
        [HttpGet("{ContainerNumber}")]
        public async Task<ActionResult<RequiredFields>> GetRequiredField(string ContainerNumber)
        {
            try
            {
                var requiredField = await _context.RequiredFields
                    .FirstOrDefaultAsync(r => r.ContainerNumber == ContainerNumber);

                if (requiredField == null)
                {
                    return NotFound(new { Message = $"Record with ContainerNumber {ContainerNumber} not found." });
                }

                return Ok(requiredField);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // [HttpPost]
        // public async Task<ActionResult<RequiredFields>> PostRequiredField([FromBody] RequiredFields requiredField)
        // {
        //     try
        //     {
        //         if (requiredField == null)
        //         {
        //             return BadRequest(new { Message = "Invalid data provided." });
        //         }

        //         // Ensure the partition key field is populated
        //         if (string.IsNullOrEmpty(requiredField.ContainerNumber))
        //         {
        //             return BadRequest(new { Message = "ContainerNumber (partition key) is required." });
        //         }

        //         // Check for an existing document with the same ContainerNumber
        //         var existingField = await _context.RequiredFields
        //             .FirstOrDefaultAsync(r => r.ContainerNumber == requiredField.ContainerNumber);

        //         if (existingField != null)
        //         {
        //             return Conflict(new { Message = $"Record with ContainerNumber {requiredField.ContainerNumber} already exists." });
        //         }

        //         // Assign a new ID if not provided
        //         if (string.IsNullOrEmpty(requiredField.Id))
        //         {
        //             requiredField.Id = Guid.NewGuid().ToString();
        //         }

        //         // Add the document
        //         _context.RequiredFields.Add(requiredField);
        //         await _context.SaveChangesAsync();

        //         return CreatedAtAction(nameof(GetRequiredField), new { ContainerNumber = requiredField.ContainerNumber }, requiredField);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }
        // }


        // // PUT: api/RequiredFields/{ContainerNumber}
        // [HttpPut("{ContainerNumber}")]
        // public async Task<IActionResult> PutRequiredField(string ContainerNumber, [FromBody] RequiredFields requiredField)
        // {
        //     if (ContainerNumber != requiredField.ContainerNumber)
        //     {
        //         return BadRequest(new { Message = "ContainerNumber in URL does not match ContainerNumber in payload." });
        //     }

        //     try
        //     {
        //         // Update the record
        //         _context.Entry(requiredField).State = EntityState.Modified;

        //         await _context.SaveChangesAsync();
        //         return NoContent();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!RequiredFieldsExists(ContainerNumber))
        //         {
        //             return NotFound(new { Message = $"Record with ContainerNumber {ContainerNumber} not found." });
        //         }
        //         else
        //         {
        //             return StatusCode(500, "A concurrency error occurred.");
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }
        // }

        // DELETE: api/RequiredFields/{ContainerNumber}
        [HttpDelete("{ContainerNumber}")]
        public async Task<IActionResult> DeleteRequiredField(string ContainerNumber)
        {
            try
            {
                var requiredField = await _context.RequiredFields
                    .FirstOrDefaultAsync(r => r.ContainerNumber == ContainerNumber);

                if (requiredField == null)
                {
                    return NotFound(new { Message = $"Record with ContainerNumber {ContainerNumber} not found." });
                }

                _context.RequiredFields.Remove(requiredField);
                await _context.SaveChangesAsync();

                return NoContent();//http 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/RequiredFields/demurrage/{ContainerNumber}
        [HttpGet("demurrage/{ContainerNumber}")]
        public async Task<IActionResult> GetDemurrageFees(string ContainerNumber)
        {
            try
            {
                var field = await _context.RequiredFields
                    .FirstOrDefaultAsync(r => r.ContainerNumber == ContainerNumber);

                if (field == null)
                {
                    return NotFound(new { Message = $"Container number {ContainerNumber} not found." });
                }

                return Ok(new { containerNumber = field.ContainerNumber, demurrageFees = field.Demurrage_fees });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // PUT: api/RequiredFields/demurrage/{ContainerNumber}
        [HttpPut("demurrage/{ContainerNumber}")]
        public async Task<IActionResult> PutDemurrageFees(string ContainerNumber, [FromBody] DemurrageFees paymentDetails)
        {
            try
            {
                var field = await _context.RequiredFields
                    .FirstOrDefaultAsync(r => r.ContainerNumber == ContainerNumber);

                if (field == null)
                {
                    return NotFound(new { Message = $"Container number {ContainerNumber} not found." });
                }

                // Update the demurrage fees
                field.Demurrage_fees.FeesDue = paymentDetails.FeesDue;
                field.Demurrage_fees.FeesPaid = paymentDetails.FeesPaid;

                // Mark the record as modified
                _context.Entry(field).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                // Send the updated demurrage fees to the Azure Service Bus
                await _serviceBusSenderService.SendMessageAsync(field.Demurrage_fees, ContainerNumber);
                
                return NoContent(); // No content response after successful update
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // // Helper method to check if a record exists by ContainerNumber
        // private bool RequiredFieldsExists(string ContainerNumber)
        // {
        //     return _context.RequiredFields.Any(e => e.ContainerNumber == ContainerNumber);
        // }
    }
}
