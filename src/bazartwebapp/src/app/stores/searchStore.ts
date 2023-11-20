import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Searching } from "../models/Search/Searching";

export default class SearchStore {
    visibleSearchBar: boolean = false;
    results: Searching = {
        items: {},
        value: ''
    };

    constructor() {
        makeAutoObservable(this);
    }

    search = async (searchQuery: string) => {
        try {
            const results = await agent.Search.search(searchQuery);
            runInAction(() => {
                this.results.value = searchQuery;
                this.results.items = results.items;
            })
        } catch (error) {
            console.log(error);
        }
    }

    setVisibleSearchBar = (state: boolean) => {
        this.visibleSearchBar = state;
    }
}