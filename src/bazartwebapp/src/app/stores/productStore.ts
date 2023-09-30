import { makeAutoObservable } from "mobx";
import agent from "../api/agent";
import { Product } from "../models/Product/product";
import { ProductDetails } from "../models/Product/productDetails";

export default class ProductStore {
    products: Product[] = [];
    productsRegistry = new Map<string, Product>();
    latestProductRegistry = new Map<string, Product>();
    selectedProduct: ProductDetails | undefined = undefined;

    constructor() {
        makeAutoObservable(this);
    }

    loadProducts = async (category: number) => {
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
        try {
            const products = await agent.Products.latest();
            products.forEach(product => {
                this.latestProductRegistry.set(product.id, product);
            })
        } catch (error) {
            console.log(error);
        }
    }
}