import { Product } from "./product.model";

export interface ProductResponse extends Product{
    id: string;    
    stock: number;
  }