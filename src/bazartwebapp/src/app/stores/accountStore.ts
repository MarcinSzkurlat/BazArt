import { makeAutoObservable, reaction, runInAction } from "mobx";
import agent from "../api/agent";
import { User } from "../models/User/user";
import { AccountLogin } from "../models/Account/accountLogin";
import { router } from "../router/Routes";
import { AccountRegistration } from "../models/Account/accountRegistration";
import { store } from "./store";
import { AccountChangePassword } from "../models/Account/accountChangePassword";
import { toast } from "react-toastify";

export default class AccountStore {
    user: User | null = null;
    token: string | null = localStorage.getItem('jwt');

    constructor() {
        makeAutoObservable(this);

        reaction(
            () => this.token,
            (token) => {
                if (token) {
                    localStorage.setItem('jwt', token);
                } else {
                    localStorage.removeItem('jwt');
                }
            }
        )
    }

    get isLoggedIn() {
        return !!this.user;
    }

    login = async (data: AccountLogin) => {
        try {
            const user = await agent.Account.login(data);
            runInAction(() => {
                this.setToken(user.token);
                this.setUser(user);
                store.userStore.loadCurrentUserDetails();
            })
            router.navigate(`/user/${user.id}`);
            store.modalStore.closeModal();
            toast.success('Logged in successfully!');
        } catch (error) {
            throw error;
        }
    }

    registration = async (data: AccountRegistration) => {
        try {
            const user = await agent.Account.registration(data);
            this.setToken(user.token);
            runInAction(() => this.setUser(user));
            router.navigate(`/user/${user.id}`);
            store.modalStore.closeModal();
            toast.success('Registration successful!');
        } catch (error) {
            throw error;
        }
    }

    logout = () => {
        this.setToken(null);
        this.setUser(null);
        router.navigate('/');
        toast.info('Logged out successfully!');
    }

    getCurrentUser = async () => {
        try {
            const user = await agent.Account.currentUser();
            runInAction(() => {
                this.setToken(user.token);
                this.setUser(user);
            })
        } catch (error) {
            console.log(error);
            this.logout();
        }
    }

    changePassword = async (data: AccountChangePassword) => {
        try {
            const user = await agent.Account.changePassword(data);
            runInAction(() => {
                this.setToken(user.token);
                this.setUser(user);
            })
            router.navigate(`/user/${user.id}`);
            store.modalStore.closeModal();
            toast.info('Changed password successfully!')
        } catch (error) {
            throw error;
        }
    }

    deleteAccount = async (id: string) => {
        try {
            await agent.Users.deleteUser(id);
            if (this.user?.id === id) {
                this.logout();
                store.modalStore.closeModal();
            }
            router.navigate('/');
            toast.info('Account has been deleted successfully!');
        } catch (error) {
            console.log(error);
        }
    }

    setToken = (token: string | null) => {
        this.token = token;
    }

    setUser = (user: User | null) => {
        this.user = user;
    }
}