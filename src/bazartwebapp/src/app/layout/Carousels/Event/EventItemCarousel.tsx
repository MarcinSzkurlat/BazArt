import { Event } from "../../../models/Event/event";
import { Card, Image, Label } from "semantic-ui-react";

interface Props {
    event: Event;
}

export default function EventItemCarousel({ event }: Props) {
    return (
        <Card raised style={{ padding: '15px', position: 'relative', background: 'transparent', margin: '10px' }}>
            <div style={{ position: 'absolute', top: '30px', right: '30px', zIndex: '1' }}>
                <Label as='a' href='/' ribbon='right' >{event.categoryName}</Label> {/* TODO change href to appropriate category page */ }
            </div>
            <Image as='a' href='/' src={event.imageUrl} alt={event.name} size='medium' style={{ zIndex: '0' }} /> {/* TODO change href to event detail page */}
            <Card.Header as='h3' style={{ margin: '10px' }}><b>{event.name}</b></Card.Header>
            <Card.Description style={{ height: '70px' }}>
                <i>{event.description.length > 100
                    ? event.description.substring(0, 100) + "..."
                    : event.description}</i>
            </Card.Description>
        </Card>
    )
}