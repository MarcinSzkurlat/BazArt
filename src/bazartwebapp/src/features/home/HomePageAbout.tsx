import { useState } from "react";
import { Accordion, Grid, Header, Icon, Image, Segment } from "semantic-ui-react";

export default function HomePageAbout() {
    const [activeIndex, setActiveIndex] = useState(-1);

    const handleAccordionClick = (index: number) => {
        setActiveIndex(activeIndex === index ? -1 : index);
    };

    return (
        <Segment basic style={{ width: '60%', margin: '0 auto', background: 'transparent', border: 'none' }}>
            <Grid centered verticalAlign='middle' align='center'>
                <Grid.Row style={{ margin: '20px 0px 0px 0px' }}>
                    <Grid.Column width={7} textAlign='center' className='homepage-about-grid-column'>
                        <Header size='medium'>
                            Welcome to BazArt, the online platform that seamlessly blends the world of traditional bazaars with the realm of artistry. At BazArt, we provide a unique space where artists and artisans can come together to showcase their creations, connect with like-minded individuals, and sell their one-of-a-kind pieces.
                        </Header>
                    </Grid.Column>
                    <Grid.Column width={8} textAlign='right' className='homepage-about-grid-column'>
                        <Image src="/assets/home-page-1.jpg" alt='bazaar' style={{ width: '100%', height: '400px' }} />
                    </Grid.Column>
                </Grid.Row>
                <Grid.Row style={{ margin: '20px 0px 0px 0px' }}>
                    <Grid.Column width={7} textAlign='right' className='homepage-about-grid-column'>
                        <Image size='medium' src="/assets/home-page-2.jpg" alt='exhibition' style={{ width: '100%', height: '400px' }} />
                    </Grid.Column>
                    <Grid.Column width={8} textAlign='center' className='homepage-about-grid-column'>
                        <Header size='medium'>
                            BazArt is on a mission to empower both emerging and established artists, crafters, and sellers by offering them a digital marketplace that celebrates creativity and individuality. We believe that artistry knows no bounds, and everyone deserves a platform to share their passion and talents with the world.
                        </Header>
                    </Grid.Column>
                </Grid.Row>
                <Grid.Row style={{ margin: '40px 0px 40px 0px' }}>
                    <Grid.Column width={7} textAlign='center' className='homepage-about-grid-column'>
                        <Header>What we offer?</Header>
                        <Accordion>
                            <Accordion.Title active={activeIndex === 0} onClick={() => handleAccordionClick(0)} >
                                <Icon name='dropdown' /> <b>Artistic Storefronts</b>
                            </Accordion.Title>
                            <Accordion.Content active={activeIndex === 0}>
                                Create your own online store to display your artworks, crafts, and unique items. Customize your storefront to reflect your style and brand.
                            </Accordion.Content>
                            <Accordion.Title active={activeIndex === 1} onClick={() => handleAccordionClick(1)}>
                                <Icon name='dropdown' /> <b>Sell Your Creations</b>
                            </Accordion.Title>
                            <Accordion.Content active={activeIndex === 1}>
                                List your products for sale and reach a global audience of art enthusiasts and collectors. Our secure and user-friendly platform makes it easy to manage your inventory and transactions.
                            </Accordion.Content>
                            <Accordion.Title active={activeIndex === 2} onClick={() => handleAccordionClick(2)}>
                                <Icon name='dropdown' /> <b>Event Creation</b>
                            </Accordion.Title>
                            <Accordion.Content active={activeIndex === 2}>
                                Whether you're hosting an art exhibition, craft fair, or any creative event, BazArt lets you organize and promote it effortlessly. Categorize your event to attract the right audience.
                            </Accordion.Content>
                        </Accordion>
                    </Grid.Column>
                    <Grid.Column width={8} textAlign='center' className='homepage-about-grid-column'>
                        <Header>Why Choose BazArt?</Header>
                        <Accordion>
                            <Accordion.Title active={activeIndex === 3} onClick={() => handleAccordionClick(3)}>
                                <Icon name='dropdown' /> <b>Diverse Categories</b>
                            </Accordion.Title>
                            <Accordion.Content active={activeIndex === 3}>
                                Our platform covers a wide range of artistic categories, from painting and sculpture to handmade jewelry and unique crafts. You'll find your niche here.
                            </Accordion.Content>
                            <Accordion.Title active={activeIndex === 4} onClick={() => handleAccordionClick(4)}>
                                <Icon name='dropdown' /> <b>User-Friendly Interface</b>
                            </Accordion.Title>
                            <Accordion.Content active={activeIndex === 4}>
                                We've designed BazArt to be intuitive and easy to navigate, ensuring a seamless experience for both buyers and sellers.
                            </Accordion.Content>
                            <Accordion.Title active={activeIndex === 5} onClick={() => handleAccordionClick(5)}>
                                <Icon name='dropdown' /> <b>Global Reach</b>
                            </Accordion.Title>
                            <Accordion.Content active={activeIndex === 5}>
                                Whether you're an artist looking to expand your reach or a buyer seeking distinctive pieces from around the world, BazArt connects you to a global art community.
                            </Accordion.Content>
                        </Accordion>
                    </Grid.Column>
                </Grid.Row>
                <div className='homepage-about-div' >
                    <Header size='large' textAlign='center' style={{marginBottom:'100px'}}>
                        Join BazArt today and become part of a dynamic online marketplace that celebrates the beauty of handcrafted art. Unleash your creativity, discover unique treasures, and connect with fellow art enthusiasts.
                    </Header>
                    <Header size='huge' textAlign='center' >
                        <b>Welcome to the world of BazArt!</b>
                    </Header>
                </div>
            </Grid>
        </Segment>
    )
}