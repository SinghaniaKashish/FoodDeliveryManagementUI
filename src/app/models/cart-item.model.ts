import { MenuItem } from './menu-item.model';

// export interface CartItem {
//   UserId: number;
//   MenuItemId: number;
//   MenuItem: MenuItem;
//   Quantity: number;
//   Price: number;
//   TotalPrice: number;
// }

export interface CartItem{
  // UserId:number;
  menuItemId:number;
  itemName:string;
  quantity:number;
  price:number;
  totalPrice:number;
}