import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { StoreModule } from '@ngrx/store';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { catalogReducer } from './catalog.reducer';
import { CategoryComponent } from './catalog/category/category.component';
import { ProductComponent } from './catalog/product/product.component';

@NgModule({
  declarations: [
    AppComponent,
    CategoryComponent,
    ProductComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    StoreModule.forRoot({ catalog: catalogReducer })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
