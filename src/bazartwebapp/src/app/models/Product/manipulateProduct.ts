export interface ManipulateProduct {
    id: string;
    name: string;
    description: string;
    price: number | null;
    isForSell: boolean;
    quantity: number | null;
    imageUrl: string | null;
    category: number;
}