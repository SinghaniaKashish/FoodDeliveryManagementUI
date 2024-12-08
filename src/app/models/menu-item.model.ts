export interface IngredientUsage {
  ingredientUsageId: number;
  itemId: number;
  ingredientName: string;
  quantity: number;
}

export interface MenuItem {
  itemId: number;
  restaurantId: number;
  name: string;
  category: string;
  price: number;
  cuisineType: string;
  availability: boolean;
  isVeg: boolean;
  imagePath: string;
  ingredients: IngredientUsage[];
}
