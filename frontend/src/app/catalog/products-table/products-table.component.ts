import { Component, Input, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProductResponse } from '../../models/productResponse.model';
import { Store } from '@ngrx/store';
import { selectAllCategories, selectAllProducts } from '../../catalog.selectors';
import { Observable, combineLatest, map } from 'rxjs';
import { getAllCategories, getAllProducts } from '../../catalog.actions';
import { MatDialog } from '@angular/material/dialog';
import { ProductDialogBoxComponent } from '../product-dialog-box/product-dialog-box.component';
import { CategoryResponse } from 'src/app/models/categoryResponse.model';

@Component({
  selector: 'app-products-table',
  templateUrl: './products-table.component.html',
  styleUrls: ['./products-table.component.less']
})
export class ProductsTableComponent {
  @Input() products$: Observable<ProductResponse[]>;
  dataSource: MatTableDataSource<ProductResponse>;
  displayedColumns: string[] = ['id', 'title', 'price', 'description', 'stock', 'categoryName'];
  contextMenuVisible = false;
  contextMenuPosition = { x: '0px', y: '0px' };
  selectedProductId: string; 
  categories$: Observable<CategoryResponse[]>;
  displayData$: Observable<any[]>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private store: Store<{ products: ProductResponse[] }>,
    public dialog: MatDialog
  ) { 
    this.store.dispatch(getAllCategories());
  }

  ngOnInit() {
  this.store.dispatch(getAllProducts());
  this.categories$ = this.store.select(selectAllCategories);

  combineLatest([this.store.select(selectAllProducts), this.categories$]).subscribe(([products, categories]) => {
    const productsWithCategoryName = products.map(product => {
      const category = categories.find(cat => cat.id === product.categoryId);
      return {
        ...product,
        categoryName: category ? category.name : 'N/A',
      };
    });
    this.dataSource = new MatTableDataSource<ProductResponse>(productsWithCategoryName);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  });
  }

  onRowClicked(row: any) {
    console.log('Row clicked: ', row);
  }

  onRightClick(event: MouseEvent, row: any) {
    event.preventDefault();
    this.contextMenuPosition.x = event.clientX + 'px';
    this.contextMenuPosition.y = event.clientY + 'px';
    this.contextMenuVisible = true;
    this.selectedProductId = row.id;
  }

  onOutsideClick() {
    this.contextMenuVisible = false;
  }

  onContextMenuClosed() {
    this.contextMenuVisible = false;
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(ProductDialogBoxComponent, {
      width: '400px',
      data: { editProduct: null }
    });
  }  
}
