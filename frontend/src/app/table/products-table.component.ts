import { Component, Input, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProductResponse } from '../models/productResponse.model';
import { Store } from '@ngrx/store';
import { selectAllProducts } from '../catalog.selectors';
import { Observable } from 'rxjs';
import { getAllProducts } from '../catalog.actions';

@Component({
  selector: 'app-products-table',
  templateUrl: './products-table.component.html',
  styleUrls: ['./products-table.component.less']
})
export class ProductsTableComponent {
  @Input() products$: Observable<ProductResponse[]>;
  dataSource: MatTableDataSource<ProductResponse>;
  displayedColumns: string[] = ['id', 'title', 'price', 'description', 'stock', 'categoryId'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private store: Store<{ products: ProductResponse[] }>,
  ) { }

  ngOnInit() {
    this.store.dispatch(getAllProducts());
    this.store.select(selectAllProducts).subscribe(products => {
      this.dataSource = new MatTableDataSource<ProductResponse>(products);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  onRowClicked(row: any) {
    console.log('Row clicked: ', row);
  }
}
