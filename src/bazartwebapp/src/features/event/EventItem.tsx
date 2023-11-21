import { Card, Image, Label } from "semantic-ui-react";
import { Link, useLocation } from "react-router-dom";
import { Event } from "../../app/models/Event/event";

interface Props {
    event: Event;
}

export default function EventItem({ event }: Props) {
    const location = useLocation();

    return (
        <Card raised style={{ padding: '15px', position: 'relative', background: 'transparent', margin: '10px' }}>
            {location.pathname === '/'
                ? <div style={{ position: 'absolute', top: '30px', right: '30px', zIndex: '1' }}>
                    <Label as={Link} to={`/category/${event.categoryName}`} ribbon='right' color='black'>{event.categoryName}</Label>
                </div>
                : <></>}
            <Image centered as={Link} to={`/event/${event.id}`} src={event.imageUrl} alt={event.name} size='medium' style={{ zIndex: '0' }} />
            <Card.Header as='h3' textAlign='center' style={{ margin: '10px' }}><b>{event.name}</b></Card.Header>
            <Card.Description style={{ height: '70px' }}>
                <p><i>{event.description.length > 100
                    ? event.description.substring(0, 100) + "..."
                    : event.description}</i></p>
            </Card.Description>
        </Card>
    )
}