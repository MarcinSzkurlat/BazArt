import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Event } from "../models/Event/event";
import { EventDetails } from "../models/Event/eventDetails";
import { v4 as uuid } from 'uuid';
import { router } from "../router/Routes";
import { ManipulateEvent } from "../models/Event/manupulateEvent";
import { store } from "./store";
import { toast } from "react-toastify";

export default class EventStore {
    events: Event[] = [];
    eventsRegistry = new Map<string, Event>();
    latestEventsRegistry = new Map<string, Event>();
    loadingInitial: boolean = false;
    selectedEvent?: EventDetails = undefined;
    pageNumber: number = 1;
    totalPages: number = 0;

    constructor() {
        makeAutoObservable(this);
    }

    loadEvents = async (category: string, pageNumber: number = 1) => {
        this.setLoadingInitial(true);
        try {
            const events = await agent.Events.list(category, pageNumber);
            runInAction(() => {
                this.eventsRegistry.clear();
                events.items.forEach(event => {
                    this.eventsRegistry.set(event.id, event);
                })
                this.pageNumber = events.pageNumber;
                this.totalPages = events.totalPages;
            })
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    loadEvent = async (id: string) => {
        this.setLoadingInitial(true);
        try {
            const event = await agent.Events.details(id);
            runInAction(() => { this.selectedEvent = event })
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    loadLatestEvents = async () => {
        this.setLoadingInitial(true);
        try {
            const events = await agent.Events.latest();
            this.latestEventsRegistry.clear();
            events.forEach(event => {
                this.latestEventsRegistry.set(event.id, event);
            })
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    loadUserEvents = async (id: string, pageNumber: number = 1) => {
        this.setLoadingInitial(true);
        try {
            const events = await agent.Events.userEvents(id, pageNumber);
            runInAction(() => {
                this.eventsRegistry.clear();
                events.items.forEach(event => {
                    this.eventsRegistry.set(event.id, event);
                })
                this.pageNumber = events.pageNumber;
                this.totalPages = events.totalPages;
            })
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    createEvent = async (event: ManipulateEvent) => {
        event.id = uuid();
        try {
            const createdEvent = await agent.Events.create(event);
            runInAction(() => this.selectedEvent = createdEvent);
            router.navigate(`/event/${event.id}`);
            store.modalStore.closeModal();
            toast.success('Event created successfully!');
        } catch (error) {
            throw error;
        }
    }

    editEvent = async (event: ManipulateEvent) => {
        if (event.country === '') event.country = null;
        if (event.city === '') event.city = null;
        if (event.street === '') event.street = null;
        if (event.postalCode === '') event.postalCode = null;

        try {
            const editedEvent = await agent.Events.update(event, event.id);
            runInAction(() => this.selectedEvent = editedEvent);
            router.navigate(`/event/${event.id}`);
            store.modalStore.closeModal();
            toast.info('Event edited successfully!');
        } catch (error) {
            throw error;
        }
    }

    deleteEvent = async (id: string) => {
        try {
            await agent.Events.delete(id);
            router.navigate('/');
            toast.info('Event deleted successfully!');
        } catch (error) {
            console.log(error);
        }
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }
}