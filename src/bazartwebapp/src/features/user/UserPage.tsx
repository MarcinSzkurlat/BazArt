import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { useStore } from "../../app/stores/store";
import { Button, Confirm, Container, Divider, Grid, Header, Icon, Image, Popup, Segment } from "semantic-ui-react";
import ProductCarousel from "../../app/layout/Carousels/Product/ProductCarousel";
import EventCarousel from "../../app/layout/Carousels/Event/EventCarousel";
import { PageTypes } from "../../app/layout/Carousels/pageTypes";
import ProductGridItems from "../../app/layout/GridItems/Product/ProductGridItems";
import EventGridItems from "../../app/layout/GridItems/Event/EventGridItems";

export default observer(function UserPage() {
    const { id } = useParams();
    const { userStore, accountStore, productStore, eventStore } = useStore();
    const { userDetails, addFavoriteUser } = userStore;

    const [confirmDelete, setConfirmDelete] = useState(false);
    const [visibleEventsGrid, setVisibleEventsGrid] = useState(false);
    const [visibleProductsGrid, setVisibleProductsGrid] = useState(false);

    const handleDeleteUser = () => {
        if (userDetails) accountStore.deleteAccount(userDetails?.id);
        setConfirmDelete(false);
    }

    const handleFavoriteButton = () => {
        addFavoriteUser(id!);
    }

    useEffect(() => {
        if (id) {
            userStore.loadUserDetails(id);
        }
    }, [id, userStore.currentUserDetails])

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
                {accountStore.user?.role === "Admin" && accountStore.user.id !== userDetails?.id
                    ? <>
                        <Button onClick={() => setConfirmDelete(true)} size='medium' floated='left' color='red' style={{ position: 'absolute', bottom: '50px', left: '60px' }}>
                        Delete this user
                        </Button>
                        <Confirm
                            open={confirmDelete}
                            cancelButton='Cancel'
                            confirmButton="Yes, delete account"
                            onCancel={() => setConfirmDelete(false)}
                            onConfirm={handleDeleteUser}
                        />
                    </>
                    : <></>}
                {accountStore.user?.id === id
                    ? <></>
                    : <Popup pinned trigger={
                        <Button size='huge' icon floated='right' color='red' circular onClick={handleFavoriteButton} style={{ position: 'absolute', bottom: '50px', right: '60px' }}>
                            <Icon name='heart' />
                        </Button>}>
                        Add user to favorite
                    </Popup>}
            </Segment>
            <Divider horizontal>
                <Header as='h1'>Products</Header>
            </Divider>
            {visibleProductsGrid
                ? <ProductGridItems page={PageTypes.User} userId={id} />
                : <ProductCarousel page={PageTypes.User} userId={id} />}
            {productStore.totalPages > 1 && visibleProductsGrid === false
                ? <div style={{ textAlign: 'center', marginTop: '30px' }}>
                    <Button onClick={() => setVisibleProductsGrid(true)}>Show more</Button>
                </div>
                : <></>}
            <Divider horizontal>
                <Header as='h1'>Events</Header>
            </Divider>
            {visibleEventsGrid
                ? <EventGridItems page={PageTypes.User} userId={id} />
                : <EventCarousel page={PageTypes.User} userId={id} />}
            {eventStore.totalPages > 1 && visibleEventsGrid === false
                ? <div style={{ textAlign: 'center', marginTop: '30px' }}>
                    <Button onClick={() => setVisibleEventsGrid(true)}>Show more</Button>
                </div>
                : <></>}
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
                                    <i>{userDetails?.country ? userDetails?.country : '-'}</i>
                                </span>
                            </Grid.Column>
                            <Grid.Column>
                                <Icon name='building' style={{ marginBottom: 5 }} size='large' />
                                <span>
                                    <b>City</b>
                                    <br />
                                    <i>{userDetails?.city ? userDetails?.city : '-'}</i>
                                </span>
                            </Grid.Column>
                            <Grid.Column floated='right'>
                                <Icon name='envelope outline' style={{ marginBottom: 5 }} size='large' />
                                <span>
                                    <b>Postal code</b>
                                    <br />
                                    <i>{userDetails?.postalCode ? userDetails?.postalCode : '-'}</i>
                                </span>
                            </Grid.Column>
                        </Grid.Row>
                        <Grid.Row columns={3} textAlign='center'>
                            <Grid.Column floated='left'>
                                <Icon name='road' style={{ marginBottom: 5 }} size='large' />
                                <span>
                                    <b>Street</b>
                                    <br />
                                    <i>{userDetails?.street ? userDetails?.street : '-'}</i>
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
                                    <i>{userDetails?.houseNumber ? userDetails?.houseNumber : '-'}</i>
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