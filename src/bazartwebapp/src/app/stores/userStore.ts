import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Product } from "../models/Product/product";
import { Event } from "../models/Event/event";
import { UserDetails } from "../models/User/userDetails";

export default class UserStore {
    userDetails: UserDetails | null = null;
    loadingInitial: boolean = false;
    userProductsRegistry = new Map<string, Product>();
    userEventsRegistry = new Map<string, Event>();

    constructor() {
        makeAutoObservable(this);
    }

    loadUserDetails = async (id: string) => {
        this.setLoadingInitial(true);
        try {
            const userDetails = await agent.Users.details(id);
            runInAction(() => this.userDetails = userDetails);
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    loadUserProducts = async (id: string) => {
        try {
            const products = await agent.Users.userProducts(id);
            this.userProductsRegistry.clear();
            products.forEach(product => {
                this.userProductsRegistry.set(product.id, product);
            })
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    loadUserEvents = async (id: string) => {
        try {
            const events = await agent.Users.userEvents(id);
            this.userEventsRegistry.clear();
            events.forEach(event => {
                this.userEventsRegistry.set(event.id, event);
            })
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }
}