import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { Card, Icon, Pagination, PaginationProps } from "semantic-ui-react";
import EventItem from "../../../../features/event/EventItem";
import { Event } from "../../../models/Event/event";
import { useStore } from "../../../stores/store";
import { PageTypes } from "../../Carousels/pageTypes";

interface Props {
    page: PageTypes;
    categoryName?: string;
    userId?: string;
}

export default observer(function EventGridItems({ page, categoryName, userId }: Props) {
    const { eventStore, accountStore } = useStore();
    const { loadEvents, eventsRegistry, pageNumber, totalPages, loadUserEvents } = eventStore;

    const [eventItems, setEventItems] = useState<Event[]>();
    const [activePage, setActivePage] = useState(pageNumber + 1);

    const onChanged = (event: React.MouseEvent<HTMLAnchorElement, MouseEvent>, pageInfo: PaginationProps) => {
        setActivePage(pageInfo.activePage as number);
    }

    useEffect(() => {
        switch (page) {
            case PageTypes.Category:
                loadEvents(categoryName!, activePage).then(() => {
                    setEventItems(Array.from(eventsRegistry.values()));
                })
                break;
            case PageTypes.User:
                if (accountStore.isLoggedIn) {
                    loadUserEvents(userId!, activePage).then(() => {
                        setEventItems(Array.from(eventsRegistry.values()));
                    })
                }
                break;
        }
    }, [activePage])

    return (
        <div style={{ width: '80%', marginLeft:'10%' }}>
            <Card.Group centered itemsPerRow={4} stackable>
                {eventItems?.map((event: Event) => (
                    <EventItem key={event.id} event={event} />
                ))}
            </Card.Group>
            <div style={{ marginTop:'30px', textAlign: 'center' }}>
            <Pagination
                activePage={pageNumber}
                ellipsisItem={{ content: <Icon name="ellipsis horizontal" />, icon: true }}
                firstItem={{ content: <Icon name="angle double left" />, icon: true }}
                lastItem={{ content: <Icon name="angle double right" />, icon: true }}
                prevItem={{ content: <Icon name="angle left" />, icon: true }}
                nextItem={{ content: <Icon name="angle right" />, icon: true }}
                totalPages={totalPages}
                onPageChange={onChanged}
                siblingRange={1}
            />
            </div>
        </div>
    )
})