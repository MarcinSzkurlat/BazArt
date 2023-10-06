import { makeAutoObservable } from "mobx";
import agent from "../api/agent";
import { Event } from "../models/Event/event";
import { EventDetails } from "../models/Event/eventDetails";

export default class EventStore {
    events: Event[] = [];
    eventsRegistry = new Map<string, Event>();
    latestEventsRegistry = new Map<string, Event>();
    loadingInitial: boolean = false;
    selectedEvent: EventDetails | undefined = undefined;

    constructor() {
        makeAutoObservable(this);
    }

    loadEvents = async (category: string) => {
        try {
            const events = await agent.Events.list(category);
            events.forEach(event => {
                this.eventsRegistry.set(event.id, event);
            })
        } catch (error) {
            console.log(error);
        }
    }

    loadEvent = async (id: string) => {
        try {
            const event = await agent.Events.details(id);
            this.selectedEvent = event;
        } catch (error) {
            console.log(error);
        }
    }

    loadLatestEvents = async () => {
        this.setLoadingInitial(true);
        try {
            const events = await agent.Events.latest();
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