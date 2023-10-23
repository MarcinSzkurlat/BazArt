import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Product } from "../models/Product/product";
import { ProductDetails } from "../models/Product/productDetails";

export default class ProductStore {
    products: Product[] = [];
    productsRegistry = new Map<string, Product>();
    latestProductRegistry = new Map<string, Product>();
    loadingInitial: boolean = false;
    selectedProduct?: ProductDetails = undefined;

    constructor() {
        makeAutoObservable(this);
    }

    loadProducts = async (category: string) => {
        this.setLoadingInitial(true);
        try {
            const products = await agent.Products.list(category);
            this.productsRegistry.clear();
            products.forEach(product => {
                this.productsRegistry.set(product.id, product);
            })
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    loadProduct = async (id: string) => {
        this.setLoadingInitial(true);
        try {
            const product = await agent.Products.details(id);
            runInAction(() => this.selectedProduct = product);
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    loadLatestProducts = async () => {
        this.setLoadingInitial(true);
        try {
            const products = await agent.Products.latest();
            this.latestProductRegistry.clear();
            products.forEach(product => {
                this.latestProductRegistry.set(product.id, product);
            })
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }
}