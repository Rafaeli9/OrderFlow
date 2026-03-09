import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule
  ],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.scss'
})
export class ProductFormComponent implements OnInit {
  private readonly productService = inject(ProductService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly snackBar = inject(MatSnackBar);

  id: string | null = null;
  isEditMode = false;

  name = '';
  sku = '';
  price: number | null = null;
  active = true;

  loading = false;
  error = '';

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.id;

    if (this.isEditMode && this.id) {
      this.loadProduct(this.id);
    }
  }

  loadProduct(id: string): void {
    this.loading = true;
    this.error = '';

    this.productService.getProductById(id).subscribe({
      next: (product) => {
        this.name = product.name;
        this.sku = product.sku;
        this.price = product.price;
        this.active = product.active;
        this.loading = false;
      },
      error: () => {
        this.error = 'Error loading product.';
        this.loading = false;
      }
    });
  }

  save(): void {
    if (!this.name.trim() || !this.sku.trim() || !this.price || this.price <= 0) {
      this.error = 'Please fill in all fields correctly.';
      return;
    }

    this.loading = true;
    this.error = '';

    if (this.isEditMode && this.id) {
      this.productService.updateProduct(this.id, {
        name: this.name.trim(),
        sku: this.sku.trim(),
        price: this.price,
        active: this.active
      }).subscribe({
       next: () => {
          this.loading = false;
          this.snackBar.open('Product updated successfully.', 'Close', {
            duration: 3000
          });
          this.router.navigate(['/products']);
        },
        error: (err) => {
          this.loading = false;
          this.error = err?.error?.error || 'Error updating product.';
        }
      });

      return;
    }

    this.productService.createProduct({
      name: this.name.trim(),
      sku: this.sku.trim(),
      price: this.price
    }).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/products']);
      },
      error: (err) => {
        this.loading = false;
        this.error = err?.error?.error || 'Error creating product.';
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/products']);
  }
}
