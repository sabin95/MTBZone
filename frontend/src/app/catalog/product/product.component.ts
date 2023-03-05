import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.less']
})
export class ProductComponent {
  @Input() price: number;
  @Input() title: string;
  @Input() description: string;
  @Input() stock: number;
}
