import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Product } from "../models/Product/product";
import { ProductDetails } from "../models/Product/productDetails";
import { v4 as uuid } from 'uuid';
import { router } from "../router/Routes";
import { ManipulateProduct } from "../models/Product/manipulateProduct";
import { store } from "./store";
import { toast } from "react-toastify";

export default class ProductStore {
    products: Product[] = [];
    productsRegistry = new Map<string, Product>();
    latestProductRegistry = new Map<string, Product>();
    userCartProductsRegistry = new Map<string, Product>();
    loadingInitial: boolean = false;
    selectedProduct?: ProductDetails = undefined;
    pageNumber: number = 1;
    totalPages: number = 0;
    cartTotalPrice: number = 0;
    userCartQuantity: number = 0;

    constructor() {
        makeAutoObservable(this);
    }

    loadProducts = async (category: string, pageNumber: number = 1) => {
        this.setLoadingInitial(true);
        try {
            const products = await agent.Products.list(category, pageNumber);
            runInAction(() => {
                this.productsRegistry.clear();
                products.items.forEach(product => {
                    this.productsRegistry.set(product.id, product);
                })
                this.pageNumber = products.pageNumber;
                this.totalPages = products.totalPages;
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

    loadUserProducts = async (id: string, pageNumber: number = 1) => {
        this.setLoadingInitial(true);
        try {
            const products = await agent.Products.userProducts(id, pageNumber);
            runInAction(() => {
                this.productsRegistry.clear();
                products.items.forEach(product => {
                    this.productsRegistry.set(product.id, product);
                })
                this.pageNumber = products.pageNumber;
                this.totalPages = products.totalPages;
            })
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    createProduct = async (product: ManipulateProduct) => {
        product.id = uuid();
        try {
            const createdProduct = await agent.Products.create(product);
            runInAction(() => this.selectedProduct = createdProduct);
            router.navigate(`/product/${product.id}`);
            store.modalStore.closeModal();
            toast.success('Product created successfully!');
        } catch (error) {
            throw error;
        }
    }

    editProduct = async (product: ManipulateProduct) => {
        try {
            const editedProduct = await agent.Products.update(product, product.id);
            runInAction(() => this.selectedProduct = editedProduct);
            router.navigate(`/product/${product.id}`);
            store.modalStore.closeModal();
            toast.info('Product edited successfully!');
        } catch (error) {
            throw error;
        }
    }

    deleteProduct = async (id: string) => {
        try {
            await agent.Products.delete(id);
            router.navigate('/');
            toast.info('Product deleted successfully!');
        } catch (error) {
            console.log(error);
        }
    }

    loadUserFavoriteProducts = async (pageNumber: number = 1) => {
        this.setLoadingInitial(true);
        try {
            const products = await agent.FavoriteProducts.list(pageNumber);
            runInAction(() => {
                this.productsRegistry.clear();
                products.items.forEach(product => {
                    this.productsRegistry.set(product.id, product);
                })
                this.pageNumber = products.pageNumber;
                this.totalPages = products.totalPages;
            })
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    addFavoriteProduct = async (id: string) => {
        try {
            await agent.FavoriteProducts.add(id);
            toast.success('Product added to favorite.');
        } catch (error) {
            console.log(error);
        }
    }

    deleteFavoriteProduct = async (id: string) => {
        try {
            await agent.FavoriteProducts.delete(id);
            runInAction(() => {
                this.productsRegistry.delete(id);
            })
            toast.info('Product removed from favorites.');
        } catch (error) {
            console.log(error);
        }
    }

    loadUserCartProducts = async () => {
        this.setLoadingInitial(true);
        this.cartTotalPrice = 0;
        try {
            const products = await agent.Cart.list();
            runInAction(() => {
                this.userCartProductsRegistry.clear();
                this.userCartQuantity = products.length;
                products.forEach(product => {
                    this.userCartProductsRegistry.set(product.id, product);
                    this.cartTotalPrice += product.price;
                })
            })
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    addCartProduct = async (id: string) => {
        try {
            await agent.Cart.add(id);
            runInAction(() => {
                this.loadUserCartProducts();
            })
            toast.success('Product added to cart.');
        } catch (error) {
            console.log(error);
        }
    }

    deleteCartProduct = async (id: string) => {
        try {
            await agent.Cart.delete(id);
            runInAction(() => {
                this.userCartProductsRegistry.delete(id);
            })
            toast.info('Product removed from cart.');
        } catch (error) {
            console.log(error);
        }
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }
}