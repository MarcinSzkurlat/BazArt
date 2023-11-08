export interface ManipulateEvent {
    id: string;
    name: string;
    description: string;
    imageUrl: string | null;
    category: number;
    country: string | null;
    city: string | null;
    street: string | null;
    houseNumber: number | null;
    postalCode: string | null;
    startingDate: Date | null;
    endingDate: Date | null;
}