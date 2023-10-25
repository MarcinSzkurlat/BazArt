import { createContext, useContext } from "react"
import AccountStore from "./accountStore";
import CategoryStore from "./categoryStore";
import EventStore from "./eventStore";
import ModalStore from "./modalStore";
import ProductStore from "./productStore";
import UserStore from "./userStore";

interface Store {
    eventStore: EventStore
    productStore: ProductStore
    categoryStore: CategoryStore
    accountStore: AccountStore
    modalStore: ModalStore
    userStore: UserStore
}

export const store: Store = {
    eventStore: new EventStore(),
    productStore: new ProductStore(),
    categoryStore: new CategoryStore(),
    accountStore: new AccountStore(),
    modalStore: new ModalStore(),
    userStore: new UserStore()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}