import HomePageNavBar from "./HomePageNavBar";
import { Divider, Header, Image } from "semantic-ui-react";
import HomePageCategory from "./HomePageCategory";
import { useStore } from "../../app/stores/store";
import { useEffect, useRef } from "react";
import { observer } from "mobx-react-lite";
import ProductCarousel from "../../app/layout/Carousels/Product/ProductCarousel";
import EventCarousel from "../../app/layout/Carousels/Event/EventCarousel";
import HomePageAbout from "./HomePageAbout";

export default observer(function HomePage() {
    const { productStore, eventStore } = useStore();
    const { latestProductRegistry, loadLatestProducts } = productStore;
    const { latestEventsRegistry, loadLatestEvents } = eventStore;

    const aboutRef = useRef<HTMLDivElement>(null);
    const scrollToAbout = () => {

        aboutRef.current?.scrollIntoView({ behavior: 'smooth', block: 'start' });

    };

    useEffect(() => {
        loadLatestProducts();
        loadLatestEvents();
    }, [])

    return (
        <>
            <HomePageNavBar scrollToAbout={scrollToAbout} />
            <Image src="/assets/BazArt_logo_Theme_Light.jpeg" alt="logo" size="huge" centered />
            <HomePageCategory />
            <br/><br/>
            <Divider horizontal>
                <Header as='h1'>Latest works</Header>
            </Divider>
            <ProductCarousel products={Array.from(latestProductRegistry.values())} />
            <br/><br/>
            <Divider horizontal>
                <Header as='h1'>Latest events</Header>
            </Divider>
            <EventCarousel events={Array.from(latestEventsRegistry.values())} />
            <br/><br/>
            <Divider horizontal>
                <Header as='h1'>About</Header>
            </Divider>
            <div ref={aboutRef}>
                <HomePageAbout />
            </div>
        </>
    )
})