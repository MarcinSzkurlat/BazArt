import { makeAutoObservable } from "mobx";
import agent from "../api/agent";
import { Category } from "../models/Category/category";

export default class CategoryStore {
    categories: Category[] = [];
    categoriesRegistry = new Map<string, Category>();
    loadingInitial: boolean = false;

    constructor() {
        makeAutoObservable(this);
    }

    loadCategories = async () => {
        this.setLoadingInitial(true);
        try {
            const categories = await agent.Categories.list();
            categories.forEach(category => {
                this.categoriesRegistry.set(category.id, category);
            })
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    setLoadingInitial(state: boolean) {
        return this.loadingInitial = state;
    }
}