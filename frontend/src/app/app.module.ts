import { HttpClientModule } from '@angular/common/http';
import { NgModule, isDevMode } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CatalogEffects } from './catalog.effects';
import { catalogReducer } from './catalog.reducer';

import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ProductsTableComponent } from './catalog/products-table/products-table.component';
import { ProductDialogBoxComponent } from './catalog/product-dialog-box/product-dialog-box.component';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { ContextMenuProductsComponent } from './catalog/context-menu-products/context-menu-products.component';
import { MatMenuModule } from '@angular/material/menu';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { CategoryDialogBoxComponent } from './catalog/category-dialog-box/category-dialog-box.component';
import { CategoriesTableComponent } from './catalog/categories-table/categories-table.component';
import { ContextMenuCategoriesComponent } from './catalog/context-menu-categories/context-menu-categories.component';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox'; 
import { MatIconModule } from '@angular/material/icon';
import { IdentityEffects } from './Auth/authentication.effects';
import { identityReducer } from './Auth/authentication.reducer';
import { AuthenticationComponent } from './Auth/authentication/authentication.component';

@NgModule({
  declarations: [
    AppComponent,
    ProductsTableComponent,
    ProductDialogBoxComponent,
    ProductDialogBoxComponent,
    ContextMenuProductsComponent,
    CategoryDialogBoxComponent,
    CategoriesTableComponent,
    ContextMenuCategoriesComponent,
    AuthenticationComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    StoreModule.forRoot({
      catalogState: catalogReducer,
      identityState: identityReducer
    }),
    EffectsModule.forRoot([
      CatalogEffects, 
      IdentityEffects
    ]),
    StoreDevtoolsModule.instrument({ maxAge: 25, logOnly: !isDevMode() }),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    BrowserAnimationsModule,
    MatFormFieldModule,
    MatDialogModule,
    MatInputModule,
    MatButtonModule,
    MatMenuModule,
    MatSelectModule,
    MatSnackBarModule,
    MatCardModule,
    MatCheckboxModule,
    MatIconModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
