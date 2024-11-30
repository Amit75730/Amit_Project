import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { containerdetailsService } from '../container-details.service';
import { containerdetails } from '../models/container-details';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-watchlist',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './watchlist.component.html',
  styleUrls: ['./watchlist.component.css']
})
export class WatchlistComponent implements OnInit {
  containerNumber: string = '';
  containerDetails: containerdetails | null = null;
  watchlist: containerdetails[] = [];

  constructor(
    private router: Router,
    private containerdetailsService: containerdetailsService
  ) { }

  ngOnInit(): void {
    this.loadWatchlistFromStorage(); // Load watchlist from localStorage
  }

  // Load watchlist from localStorage
  loadWatchlistFromStorage() {
    const savedWatchlist = localStorage.getItem('watchlist');
    this.watchlist = savedWatchlist ? JSON.parse(savedWatchlist) : [];
  }

  // Search for a container by number
  searchContainer() {
    if (this.containerNumber) {
      this.containerdetailsService.getRequiredField(this.containerNumber).subscribe(
        (data) => {
          this.containerDetails = data;
        },
        (error) => {
          console.error('Error fetching container details', error);
        }
      );
    }
  }

  // Add container to watchlist and navigate to cart
  addToCart() {
    if (this.containerDetails) {
      if (!this.isInWatchlist(this.containerDetails.containerNumber)) {
        this.watchlist.push(this.containerDetails);
        localStorage.setItem('watchlist', JSON.stringify(this.watchlist)); // Save to localStorage
      }
      this.router.navigate(['/cart', this.containerDetails.containerNumber]); // Navigate to cart
    }
  }

  // Check if the container is in the watchlist
  isInWatchlist(containerNumber: string): boolean {
    return this.watchlist.some(c => c.containerNumber === containerNumber);
  }

  // Remove container from the watchlist
  removeFromWatchlist(containerNumber: string) {
    this.watchlist = this.watchlist.filter(c => c.containerNumber !== containerNumber);
    localStorage.setItem('watchlist', JSON.stringify(this.watchlist)); // Update localStorage
  }

  // Navigate to cart for a specific container
  goToCart(containerNumber: string) {
    this.router.navigate(['/cart', containerNumber]); // Navigate to cart page
  }

  // Navigate back to the list
  navigateBack() {
    this.router.navigate(['/required-fields']);
  }
}
