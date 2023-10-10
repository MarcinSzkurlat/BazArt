import NavBar from "../../app/layout/NavBar";
import { Divider, Header, Image } from "semantic-ui-react";
import HomePageCategory from "./HomePageCategory";
import { observer } from "mobx-react-lite";
import ProductCarousel from "../../app/layout/Carousels/Product/ProductCarousel";
import EventCarousel from "../../app/layout/Carousels/Event/EventCarousel";
import HomePageAbout from "./HomePageAbout";

export default observer(function HomePage() {
    return (
        <div className='content'>
            <NavBar className='navbar-homepage' />
            <Image src="/assets/BazArt_logo_Theme_Light.jpeg" alt="logo" size="large" centered />
            <HomePageCategory />
            <br/><br/>
            <Divider horizontal>
                <Header as='h1'>Latest works</Header>
            </Divider>
            <ProductCarousel page='home' />
            <br/><br/>
            <Divider horizontal>
                <Header as='h1'>Latest events</Header>
            </Divider>
            <EventCarousel page='home' />
            <br/><br/>
            <Divider horizontal id='about'>
                <Header as='h1'>About</Header>
            </Divider>
            <HomePageAbout />
        </div>
    )
})