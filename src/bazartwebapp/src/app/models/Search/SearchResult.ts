import { SearchItem } from "./SearchItem";

export interface SearchResult {
    [key: string]: {
        name: string;
        results: SearchItem[];
    }
}