export interface Card {
    name: string;
    expansion: string;
    cost: {
        treasure: number;
    };
    types: string[];
    imageUrl: string;
}