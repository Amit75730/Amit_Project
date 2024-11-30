import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { containerdetailsService } from '../container-details.service';
import { containerdetails } from '../models/container-details';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  containerDetails: containerdetails | null = null;
  containerNumber: string | null = null;

  feesDue: number = 0;
  feesPaid: number = 0;
  paymentAmount: number = 0;
  errorMessage: string = '';
  showPaymentBox: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private containerdetailsService: containerdetailsService,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Get the containerNumber from the route parameters
    this.containerNumber = this.route.snapshot.paramMap.get('containerNumber');

    // Check if containerNumber is not null before proceeding
    if (this.containerNumber !== null) {
      this.getContainerDetails(this.containerNumber);
      this.fetchDemurrageFees(this.containerNumber);
    } else {
      // Redirect to the watchlist if containerNumber is null
      this.router.navigate(['/watchlist']);
    }
  }

  getContainerDetails(containerNumber: string): void {
    this.containerdetailsService.getRequiredField(containerNumber).subscribe(
      (data) => {
        this.containerDetails = data;
      },
      (error) => {
        console.error('Error fetching container details', error);
      }
    );
  }

  fetchDemurrageFees(containerNumber: string): void {
    this.containerdetailsService.getDemurrageFees(containerNumber).subscribe({
      next: (data) => {
        this.feesDue = data.demurrageFees?.feesDue || 0;
        this.feesPaid = data.demurrageFees?.feesPaid || 0;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to load demurrage fees';
      }
    });
  }

  openPaymentBox(): void {
    this.showPaymentBox = true;
  }

  // Validates that the payment amount is within valid limits
  payFees(): void {
    if (this.paymentAmount <= 0 || this.paymentAmount > this.feesDue) {
      alert('Invalid payment amount. Please enter a valid value.');
      return;
    }

    // Update local state
    this.feesPaid += this.paymentAmount;
    this.feesDue -= this.paymentAmount;

    // Only call update if containerNumber is not null
    if (this.containerNumber !== null) {
      const paymentDetails = {
        feesDue: this.feesDue,
        feesPaid: this.feesPaid
      };

      this.containerdetailsService.updateDemurrageFees(this.containerNumber, paymentDetails).subscribe({
        next: () => {
          // Reset payment box after successful payment
          this.paymentAmount = 0;
          this.showPaymentBox = false;

          // Refetch the updated demurrage fees after successful payment
          this.fetchDemurrageFees(this.containerNumber!);
        },
        error: (err) => {
          this.errorMessage = err.error?.message || 'Failed to process payment.';
        }
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/watchlist']);
  }
}
