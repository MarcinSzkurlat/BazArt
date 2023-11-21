import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { UserDetails } from "../models/User/userDetails";
import { store } from "./store";
import { EditUser } from "../models/User/editUser";
import { router } from "../router/Routes";

export default class UserStore {
    userDetails: UserDetails | null = null;
    currentUserDetails: UserDetails | null = null;
    loadingInitial: boolean = false;

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

    loadCurrentUserDetails = async () => {
        const id = store.accountStore.user?.id;
        try {
            const userDetails = await agent.Users.details(id!);
            runInAction(() => this.currentUserDetails = userDetails);
        } catch (error) {
            console.log(error);
        }
    }

    editCurrentUserDetails = async (data: EditUser) => {
        if (data.stageName === '') data.stageName = null;
        if (data.description === '') data.description = null;
        if (data.country === '') data.country = null;
        if (data.city === '') data.city = null;
        if (data.street === '') data.street = null;
        if (data.postalCode === '') data.postalCode = null;
        if (data.categoryId === 0) data.categoryId = null;

        try {
            const userDetails = await agent.Users.editUser(data);
            runInAction(() => this.currentUserDetails = userDetails);
            router.navigate(`/user/${store.accountStore.user?.id}`);
            store.modalStore.closeModal();
        } catch (error) {
            throw error;
        }
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }
}