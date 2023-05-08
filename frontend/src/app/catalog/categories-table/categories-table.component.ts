import { Component, Input, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Store } from '@ngrx/store';
import { Observable, combineLatest } from 'rxjs';
import { getAllCategories } from 'src/app/catalog.actions';
import { selectAllCategories } from 'src/app/catalog.selectors';
import { CategoryResponse } from 'src/app/models/categoryResponse.model';

@Component({
  selector: 'app-categories-table',
  templateUrl: './categories-table.component.html',
  styleUrls: ['./categories-table.component.less']
})
export class CategoriesTableComponent {
  @Input() products$: Observable<CategoryResponse[]>;
  dataSource: MatTableDataSource<CategoryResponse>;
  categories$: Observable<CategoryResponse[]>;
  displayedColumns: string[] = ['id', 'name'];
  contextMenuVisible = false;
  contextMenuPosition = { x: '0px', y: '0px' };
  selectedCategoryId: string; 
  displayData$: Observable<any[]>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private store: Store<{ categories: CategoryResponse[] }>,
    public dialog: MatDialog
  ) { 
  }

  ngOnInit() {
    this.categories$ = this.store.select(selectAllCategories);

    this.categories$.subscribe(categories => {
      this.dataSource = new MatTableDataSource<CategoryResponse>(categories);
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
    this.selectedCategoryId = row.id;
  }

  onOutsideClick() {
    this.contextMenuVisible = false;
  }

  onContextMenuClosed() {
    this.contextMenuVisible = false;
  }  
}
