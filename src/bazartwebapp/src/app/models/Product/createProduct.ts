export interface CreateProduct {
    id: string;
    name: string;
    description: string;
    price: number;
    isForSell: boolean;
    quantity: number;
    imageUrl: string;
    categoryName: string;
}