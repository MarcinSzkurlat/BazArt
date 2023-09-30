import { createContext, useContext } from "react"
import CategoryStore from "./categoryStore";
import EventStore from "./eventStore";
import ProductStore from "./productStore";

interface Store {
    eventStore: EventStore
    productStore: ProductStore
    categoryStore: CategoryStore
}

export const store: Store = {
    eventStore: new EventStore,
    productStore: new ProductStore,
    categoryStore: new CategoryStore
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}