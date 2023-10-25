import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { useStore } from "../../app/stores/store";
import { Button, Container, Divider, Grid, Header, Icon, Image, Popup, Segment } from "semantic-ui-react";
import ProductCarousel from "../../app/layout/Carousels/Product/ProductCarousel";
import EventCarousel from "../../app/layout/Carousels/Event/EventCarousel";

export default observer(function UserPage() {
    const { id } = useParams();
    const { userStore, accountStore } = useStore();
    const { userDetails } = userStore;

    useEffect(() => {
        if (id) {
            userStore.loadUserDetails(id);
        }
    }, [id])

    if (userStore.loadingInitial) return <LoadingComponent />

    return (
        <>
            <Segment basic style={{ height: '400px', position:'relative' }}>
                <Image rounded className='image-fit-container' src="/assets/home-page-2.jpg" style={{ zIndex: '0' }}>
                </Image>
                <div className='center-element-to-its-container' style={{ zIndex: '1' }}>
                    <Image avatar src="/assets/user-image-placeholder.png" size='small' />
                    <Header className='background-color-creme' block size='huge' style={{border:'none'} }>{userDetails?.stageName ?? userDetails?.email.split('@')[0]}</Header>
                </div>
                {accountStore.user?.id === id
                    ? <></>
                    : <Popup pinned trigger={
                        <Button size='huge' icon floated='right' color='red' circular style={{ position: 'absolute', bottom: '50px', right: '60px' }}>
                            <Icon name='heart' />
                        </Button>}>
                        Add user to favorite
                    </Popup>}
            </Segment>
            <Divider horizontal>
                <Header as='h1'>Products</Header>
            </Divider>
            <ProductCarousel page='user' userId={id} />
            <Divider horizontal>
                <Header as='h1'>Events</Header>
            </Divider>
            <EventCarousel page='user' userId={id} />
            <Divider horizontal />
            <Grid columns={2}>
                <Grid.Column>
                    <Divider horizontal>
                        <Header as='h1'>About me</Header>
                    </Divider>
                    <Container textAlign='center'>
                        {userDetails?.description
                            ? <Header block as='h4'>
                                <span>"<i>{userDetails?.description}</i>"</span>
                            </Header>
                            : <Header disabled block as='h4'>
                                <i>User description...</i>
                            </Header>}
                    </Container>
                </Grid.Column>
                <Grid.Column>
                    <Divider horizontal>
                        <Header as='h1'>Contact</Header>
                    </Divider>
                    <Grid>
                        <Grid.Row columns={3} textAlign='center'>
                            <Grid.Column floated='left'>
                                <Icon name='flag' style={{ marginBottom: 5 }} size='large' />
                                <span>
                                    <b>Country</b>
                                    <br />
                                    <i>{userDetails?.country}</i>
                                </span>
                            </Grid.Column>
                            <Grid.Column>
                                <Icon name='building' style={{ marginBottom: 5 }} size='large' />
                                <span>
                                    <b>City</b>
                                    <br />
                                    <i>{userDetails?.city}</i>
                                </span>
                            </Grid.Column>
                            <Grid.Column floated='right'>
                                <Icon name='envelope outline' style={{ marginBottom: 5 }} size='large' />
                                <span>
                                    <b>Postal code</b>
                                    <br />
                                    <i>{userDetails?.postalCode}</i>
                                </span>
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row columns={3} textAlign='center'>
                            <Grid.Column floated='left'>
                                <Icon name='road' style={{ marginBottom: 5 }} size='large' />
                                <span>
                                    <b>Street</b>
                                    <br />
                                    <i>{userDetails?.street}</i>
                                </span>
                            </Grid.Column>
                            <Grid.Column>
                                <Button href={`mailto:${userDetails?.email}`}>
                                    Mail to me
                                </Button>
                            </Grid.Column>
                            <Grid.Column floated='right'>
                                <Icon name='home' style={{ marginBottom: 5 }} size='large' />
                                <span>
                                    <b>Number</b>
                                    <br />
                                    <i>{userDetails?.houseNumber}</i>
                                </span>
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row textAlign='center'>
                            <Grid.Column>
                            </Grid.Column>
                        </Grid.Row>
                    </Grid>
                </Grid.Column>
            </Grid>
        </>
    )
})