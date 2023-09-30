import { useState } from "react";
import ReactSimplyCarousel from 'react-simply-carousel';
import { Icon } from "semantic-ui-react";
import { Event } from "../../../models/Event/event";
import EventItemCarousel from "./EventItemCarousel";

interface Props {
    events: Event[];
}

export default function EventCarousel({ events }: Props) {
    const [activeSliderIndex, setActiveSliderIndex] = useState(0);

    return (
        <div>
            <ReactSimplyCarousel
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
                {events.map((event: Event) => (
                    <EventItemCarousel key={event.id} event={event} />
                ))}
            </ReactSimplyCarousel>
        </div>
    )
}