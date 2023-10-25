import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Event } from "../models/Event/event";
import { EventDetails } from "../models/Event/eventDetails";

export default class EventStore {
    events: Event[] = [];
    eventsRegistry = new Map<string, Event>();
    latestEventsRegistry = new Map<string, Event>();
    loadingInitial: boolean = false;
    selectedEvent?: EventDetails = undefined;

    constructor() {
        makeAutoObservable(this);
    }

    loadEvents = async (category: string) => {
        this.setLoadingInitial(true);
        try {
            const events = await agent.Events.list(category);
            this.eventsRegistry.clear();
            events.forEach(event => {
                this.eventsRegistry.set(event.id, event);
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

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }
}