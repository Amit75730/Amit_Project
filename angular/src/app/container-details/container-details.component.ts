import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { containerdetailsService } from '../container-details.service';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms'; // Import necessary modules
import { containerdetails } from '../models/container-details';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-required-fields',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './container-details.component.html',
  styleUrls: ['./container-details.component.css'],
  //styleUrls: ['./required-fields.component.css'],
})
export class containerDetailsComponent implements OnInit {
  containerdetails: containerdetails[] = [];
  watchlist: containerdetails[] = [];
  cart: containerdetails[] = [];
  //cart: RequiredFields[] = [];

  selectedField: containerdetails | null = null;
  //isCreatePage = false;
  //newFieldForm!: FormGroup;

  // tradeTypes = ['IP', 'XP']; // Dropdown values for Trade Type
  // statuses = ['AV', 'DN']; // Dropdown values for Status
  // origins = ['D', 'L', 'E', 'Y']; // Dropdown values for Origin

  constructor(
    private containerdetailsService: containerdetailsService,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    //private fb: FormBuilder // Inject FormBuilder
  ) { }

  ngOnInit(): void {
    this.getRequiredFields();
    // ngOnInit(): void {
    //   this.route.url.subscribe((urlSegments) => {
    //     this.isCreatePage = urlSegments.some((segment) => segment.path === 'create');
    //     if (!this.isCreatePage) {
    //       this.getRequiredFields();
    //     } else {
    //       this.selectedField = null;
    //     }
    //   });

    // // Initialize the form
    // this.newFieldForm = this.fb.group({
    //   containerNumber: [
    //     '',
    //     [
    //       Validators.required,
    //       Validators.pattern(/^[A-Za-z]{4}[0-9]{7}$/), // 4 letters followed by 7 digits
    //     ],
    //   ],
    //   tradeType: ['', [Validators.required]],
    //   status: ['', [Validators.required]],
    //   origin: ['', [Validators.required]],
    //   vesselCode: ['', [Validators.required]],
    //   vesselName: ['', [Validators.required]],
    //   flightNumber: ['', [Validators.required]],
    //   transactionSetControlNumber: [
    //     '',
    //     [Validators.required, Validators.pattern('^[0-9]*$')], // Only digits
    //   ],
    //   demurrage_fees: [''], // Optional field
    // });

    // Check if route contains 'containerNumber' and fetch details if present
    // const containerNumber = this.route.snapshot.paramMap.get('containerNumber');
    // if (containerNumber) {
    //   this.getFieldDetails(containerNumber);
    // }
  }

  getRequiredFields(): void {
    this.containerdetailsService.getRequiredFields().subscribe(
      (data) => {
        this.containerdetails = data;
      },
      (error) => {
        console.error('Error fetching data', error);
      }
    );
  }

  // createRequiredField(): void {
  //   if (this.newFieldForm.invalid) {
  //     // Mark all controls as touched to display validation errors
  //     this.newFieldForm.markAllAsTouched();
  //     return;
  //   }

  //   const newField: RequiredFields = this.newFieldForm.value;

  //   this.requiredFieldsService.addRequiredField(newField).subscribe(
  //     () => {
  //       this.router.navigate(['/']); // Redirect to list page after creation
  //     },
  //     (error) => {
  //       console.error('Error creating field', error);
  //     }
  //   );
  // }

  // viewRequiredField(containerNumber: string): void {
  //   this.router.navigate(['/view', containerNumber]); // Navigate to detail view
  // }

  // getFieldDetails(containerNumber: string): void {
  //   this.requiredFieldsService.getRequiredField(containerNumber).subscribe(
  //     (data) => {
  //       this.selectedField = data;
  //     },
  //     (error) => {
  //       console.error('Error fetching field details', error);
  //     }
  //   );
  // }

  // updateRequiredField(): void {
  //   if (this.selectedField) {
  //     this.requiredFieldsService
  //       .updateRequiredField(this.selectedField.containerNumber, this.selectedField)
  //       .subscribe(
  //         () => {
  //           this.router.navigate(['/']); // Redirect to list page after updating
  //         },
  //         (error) => {
  //           console.error('Error updating field', error);
  //         }
  //       );
  //   }
  // }

  deleteRequiredField(containerNumber: string): void {
    if (confirm('Are you sure you want to delete this container?')) {
      this.containerdetailsService.deleteRequiredField(containerNumber).subscribe(
        () => {
          this.getRequiredFields(); // Refresh the list after deleting
        },
        (error) => {
          console.error('Error deleting field', error);
        }
      );
    }
  }

  addToWatchlist() {
    this.router.navigate(['/watchlist']); // Navigate to watchlist page
  }
  // Function to determine the status based on the demurrage fees due
  getDemurrageStatus(demurrageFees: any): string {
    return demurrageFees?.feesDue > 0 ? 'Pending' : 'Completed';
  }

  // Logout function
  logout(): void {
    this.authService.logout(); // Clear the token
    this.router.navigate(['/login']); // Redirect to login page
  }

  // // Utility function to check if the form control is invalid
  // get formControl() {
  //   return this.newFieldForm.controls;
  // }
}
