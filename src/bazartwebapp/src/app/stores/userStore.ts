import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { UserDetails } from "../models/User/userDetails";
import { store } from "./store";
import { EditUser } from "../models/User/editUser";
import { router } from "../router/Routes";
import { toast } from "react-toastify";

export default class UserStore {
    userDetails: UserDetails | null = null;
    currentUserDetails: UserDetails | null = null;
    loadingInitial: boolean = false;
    usersRegistry = new Map<string, UserDetails>();
    pageNumber: number = 1;
    totalPages: number = 0;

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
            toast.info('Details informations updated successfully!');
        } catch (error) {
            throw error;
        }
    }

    loadUserFavoriteUsers = async (pageNumber: number = 1) => {
        this.setLoadingInitial(true);
        try {
            const users = await agent.FavoriteUsers.list(pageNumber);
            runInAction(() => {
                this.usersRegistry.clear();
                users.items.forEach(user => {
                    this.usersRegistry.set(user.id, user);
                })
                this.pageNumber = users.pageNumber;
                this.totalPages = users.totalPages;
            })
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    addFavoriteUser = async (id: string) => {
        try {
            await agent.FavoriteUsers.add(id);
            toast.success('User added to favorite.');
        } catch (error) {
            console.log(error);
        }
    }

    deleteFavoriteUser = async (id: string) => {
        try {
            await agent.FavoriteUsers.delete(id);
            runInAction(() => {
                this.usersRegistry.delete(id);
            })
            toast.info('User removed from favorite.');
        } catch (error) {
            console.log(error);
        }
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }
}