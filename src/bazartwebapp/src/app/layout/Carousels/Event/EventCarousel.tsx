import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import ReactSimplyCarousel from 'react-simply-carousel';
import { Card, Header, Icon } from "semantic-ui-react";
import { Event } from "../../../models/Event/event";
import { useStore } from "../../../stores/store";
import LoadingComponent from "../../LoadingComponent";
import EventItem from "../../../../features/event/EventItem";

interface Props {
    page: string;
    categoryName?: string;
    userId?: string;
}

export default observer(function EventCarousel({ page, categoryName, userId }: Props) {
    const { eventStore, userStore, accountStore } = useStore();
    const { loadingInitial, latestEventsRegistry, loadLatestEvents, loadEvents, eventsRegistry } = eventStore;

    const [activeSliderIndex, setActiveSliderIndex] = useState(0);
    const [eventItems, setEventItems] = useState<Event[]>();

    useEffect(() => {
        switch (page) {
            case 'home':
                loadLatestEvents().then(() => {
                    setEventItems(Array.from(latestEventsRegistry.values()));
                })
                break;
            case 'category':
                loadEvents(categoryName!).then(() => {
                    setEventItems(Array.from(eventsRegistry.values()));
                })
                break;
            case 'user':
                if (accountStore.isLoggedIn) {
                    userStore.loadUserEvents(userId!).then(() => {
                        setEventItems(Array.from(userStore.userEventsRegistry.values()));
                    })
                }
                break;
        }
    }, [])

    if (loadingInitial) return <LoadingComponent />

    if (eventItems?.length === 0) return <Header textAlign='center'>There are currently no available events</Header>

    return (
        <>
            {eventItems?.length! > 4
                ? <ReactSimplyCarousel
                    activeSlideIndex={activeSliderIndex}
                    onRequestChange={setActiveSliderIndex}
                    itemsToShow={4}
                    itemsToScroll={1}
                    forwardBtnProps={{
                        style: {
                            alignSelf: 'center',
                            background: 'transparent',
                            border: 'none',
                            cursor: 'pointer',
                            fontSize: '40px'
                        },
                        children: <span><Icon name="chevron right" /></span>,
                    }}
                    backwardBtnProps={{
                        style: {
                            alignSelf: 'center',
                            background: 'transparent',
                            border: 'none',
                            cursor: 'pointer',
                            fontSize: '40px'
                        },
                        children: <span><Icon name="chevron left" /></span>,
                    }}
                    responsiveProps={[
                        {
                            itemsToShow: 4,
                            itemsToScroll: 1,
                            autoplay: true,
                            autoplayDirection: 'forward',
                            autoplayDelay: 2000
                        }
                    ]}
                    speed={1200}
                    easing='linear'
                >
                    {eventItems?.map((event: Event) => (
                        <EventItem key={event.id} event={event} />
                    ))}
                </ReactSimplyCarousel>
                : <Card.Group centered>
                    {eventItems?.map((event: Event) => (
                        <EventItem key={event.id} event={event} />
                    ))}
                </Card.Group>}
        </>
    )
})