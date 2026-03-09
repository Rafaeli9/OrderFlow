import { Component, OnInit, inject } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';

import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    CurrencyPipe,
    MatTableModule,
    MatButtonModule,
    MatSelectModule,
    MatInputModule,
    MatFormFieldModule,
    MatSnackBarModule,
    MatDialogModule
  ],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent implements OnInit {
  private readonly productService = inject(ProductService);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);

  displayedColumns: string[] = ['name', 'sku', 'price', 'status', 'actions'];
  products: Product[] = [];
  loading = false;
  error = '';

  search = '';
  activeFilter: 'all' | 'true' | 'false' = 'true';

  page = 1;
  pageSize = 5;
  totalItems = 0;
  totalPages = 0;

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this.error = '';

    const active =
      this.activeFilter === 'all'
        ? null
        : this.activeFilter === 'true';

    this.productService.getProducts(this.search, active, this.page, this.pageSize).subscribe({
      next: (response) => {
        this.products = response.items;
        this.page = response.page;
        this.pageSize = response.pageSize;
        this.totalItems = response.totalItems;
        this.totalPages = response.totalPages;
        this.loading = false;
      },
      error: () => {
        this.error = 'Error load products.';
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    this.page = 1;
    this.loadProducts();
  }

  clearFilters(): void {
    this.search = '';
    this.activeFilter = 'true';
    this.page = 1;
    this.loadProducts();
  }

  previousPage(): void {
    if (this.page > 1) {
      this.page--;
      this.loadProducts();
    }
  }

  nextPage(): void {
    if (this.page < this.totalPages) {
      this.page++;
      this.loadProducts();
    }
  }

  goToCreate(): void {
    this.router.navigate(['/products/new']);
  }

  editProduct(id: string): void {
    this.router.navigate(['/products', id, 'edit']);
  }

  deactivateProduct(product: Product): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Deactivate product',
        message: `Do you want to deactivate "${product.name}"?`,
        confirmText: 'Deactivate',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe((confirmed) => {
      if (!confirmed) {
        return;
      }

      this.loading = true;
      this.error = '';

      this.productService.deleteProduct(product.id).subscribe({
        next: () => {
          this.snackBar.open('Product deactivated successfully.', 'Close', {
            duration: 3000
          });
          this.loadProducts();
        },
        error: () => {
          this.error = 'Error deactivating product.';
          this.loading = false;
        }
      });
    });
  }

  activateProduct(product: Product): void {
  const dialogRef = this.dialog.open(ConfirmDialogComponent, {
    data: {
      title: 'Activate product',
      message: `Do you want to activate "${product.name}"?`,
      confirmText: 'Activate',
      cancelText: 'Cancel'
    }
  });

  dialogRef.afterClosed().subscribe((confirmed) => {
      if (!confirmed) {
        return;
      }

      this.loading = true;
      this.error = '';

      this.productService.activateProduct(product.id).subscribe({
        next: () => {
          this.snackBar.open('Product activated successfully.', 'Close', {
            duration: 3000
          });
          this.loadProducts();
        },
        error: () => {
          this.error = 'Error activating product.';
          this.loading = false;
        }
      });
    });
  }

}
