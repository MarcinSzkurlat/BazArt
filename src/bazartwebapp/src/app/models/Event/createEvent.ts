export interface CreateEvent {
    id: string;
    name: string;
    description: string;
    imageUrl: string;
    categoryName: string;
    organizerName: string;
    organizerId: string;
    country: string;
    city: string;
    street: string;
    houseNumber: number;
    postalCode: string;
    startingDate: string;
    endingDate: string;
}