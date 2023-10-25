import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Category } from "../models/Category/category";

export default class CategoryStore {
    categories: Category[] = [];
    categoriesRegistry = new Map<string, Category>();
    loadingInitial: boolean = false;
    selectedCategory: Category | undefined = undefined;

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
            runInAction(() => this.categories = categories);
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    loadCategory = async (name: string) => {
        try {
            const category = await agent.Categories.details(name);
            runInAction(() => this.selectedCategory = category);
        } catch (error) {
            console.log(error);
        }
    }

    setLoadingInitial(state: boolean) {
        return this.loadingInitial = state;
    }
}