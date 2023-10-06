import { makeAutoObservable } from "mobx";
import agent from "../api/agent";
import { Product } from "../models/Product/product";
import { ProductDetails } from "../models/Product/productDetails";

export default class ProductStore {
    products: Product[] = [];
    productsRegistry = new Map<string, Product>();
    latestProductRegistry = new Map<string, Product>();
    selectedProduct: ProductDetails | undefined = undefined;
    loadingInitial: boolean = false;

    constructor() {
        makeAutoObservable(this);
    }

    loadProducts = async (category: string) => {
        try {
            const products = await agent.Products.list(category);
            products.forEach(product => {
                this.productsRegistry.set(product.id, product);
            })
        } catch (error) {
            console.log(error);
        }
    }

    loadProduct = async (id: string) => {
        try {
            const product = await agent.Products.details(id);
            this.selectedProduct = product;
        } catch (error) {
            console.log(error);
        }
    }

    loadLatestProducts = async () => {
        this.setLoadingInitial(true);
        try {
            const products = await agent.Products.latest();
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