import { observer } from "mobx-react-lite"
import { useEffect } from "react"
import { Link, useParams } from "react-router-dom"
import { Button, Divider, Grid, Header, Icon, Image, Label, Segment } from "semantic-ui-react"
import LoadingComponent from "../../app/layout/LoadingComponent"
import { ActionTypes } from "../../app/models/actionTypes"
import { useStore } from "../../app/stores/store"
import EventForm from "./EventForm"

export default observer(function EventPage() {
    const { id } = useParams();
    const { eventStore, accountStore, modalStore } = useStore();
    const { loadingInitial, loadEvent, selectedEvent } = eventStore;

    const handleDeleteButton = () => {
        eventStore.deleteEvent(id!);
    }

    useEffect(() => {
        if (id) loadEvent(id)
    }, [id])

    if (loadingInitial) return <LoadingComponent />

    return (
        <Grid container>
            <Grid.Column width={10}>
                <div style={{ position: 'absolute', top: '30px', right: '28px', zIndex: '1' }}>
                    <Label as={Link} to={`/category/${selectedEvent?.categoryName}`} ribbon='right' color='black'>{selectedEvent?.categoryName}</Label>
                </div>
                <Image src={selectedEvent?.imageUrl} rounded style={{ width: '100%', zIndex: '0' }} />
                <Grid columns={'equal'} container>
                    <Grid.Row style={{ padding: '0', marginTop: '20px' }}>
                        <Grid.Column floated='left' textAlign='left'>
                            <b>Created at</b>
                        </Grid.Column>
                        <Grid.Column floated='right' textAlign='right'>
                            <b>Created by</b>
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row style={{ padding: '0' }}>
                        <Grid.Column floated='left' textAlign='left'>
                            {selectedEvent?.created.split('T')[0]}
                        </Grid.Column>
                        <Grid.Column floated='right' textAlign='right'>
                            <Link to={`/user/${selectedEvent?.organizerId}`}>{selectedEvent?.organizerName ?? selectedEvent?.organizerEmail.split('@')[0]}</Link>
                        </Grid.Column>
                    </Grid.Row>
                </Grid>
            </Grid.Column>
            <Grid.Column width={6}>
                <Header textAlign='center' size='large'>{selectedEvent?.name}</Header>
                <Divider horizontal>
                    <Header as='h6'>Description</Header>
                </Divider>
                <p>{selectedEvent?.description}</p>
                <Divider horizontal>
                    <Header as='h6'>Contact</Header>
                </Divider>
                <Grid columns={'equal'}>
                    <Grid.Row>
                        <Grid.Column width={6} floated='left' textAlign='center'>
                            <span>
                                <Icon name='calendar alternate outline' style={{ marginBottom: 5 }} size='large' />
                                <b>Start date</b>
                                <br />
                                <i>
                                    {selectedEvent?.startingDate.toString().split('T')[0]}
                                    <br />
                                    {selectedEvent?.startingDate.toString().split('T')[1].slice(0, 5)}
                                </i>
                            </span>
                        </Grid.Column>
                        <Grid.Column textAlign='center' verticalAlign='middle'>
                            <Icon name='long arrow alternate right' size='big' />
                        </Grid.Column>
                        <Grid.Column width={6} floated='right' textAlign='center'>
                            <Icon name='calendar check outline' style={{ marginBottom: 5 }} size='large' />
                            <span>
                                <b>End date</b>
                                <br />
                                <i>
                                    {selectedEvent?.endingDate.toString().split('T')[0]}
                                    <br />
                                    {selectedEvent?.endingDate.toString().split('T')[1].slice(0, 5)}
                                </i>
                            </span>
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row />
                    <Grid.Row textAlign='center'>
                        <Grid.Column floated='left'>
                            <Icon name='flag' style={{ marginBottom: 5 }} size='large' />
                            <span>
                                <b>Country</b>
                                <br />
                                <i>{selectedEvent?.country ? selectedEvent?.country : '-'}</i>
                            </span>
                        </Grid.Column>
                        <Grid.Column>
                            <Icon name='building' style={{ marginBottom: 5 }} size='large' />
                            <span>
                                <b>City</b>
                                <br />
                                <i>{selectedEvent?.city ? selectedEvent?.city : '-'}</i>
                            </span>
                        </Grid.Column>
                        <Grid.Column floated='right'>
                            <Icon name='envelope outline' style={{ marginBottom: 5 }} size='large' />
                            <span>
                                <b>Postal code</b>
                                <br />
                                <i>{selectedEvent?.postalCode ? selectedEvent?.postalCode : '-'}</i>
                            </span>
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row columns={2} textAlign='center'>
                        <Grid.Column>
                            <Icon name='road' style={{ marginBottom: 5 }} size='large' />
                            <span>
                                <b>Street</b>
                                <br />
                                <i>{selectedEvent?.street ? selectedEvent?.street : '-'}</i>
                            </span>
                        </Grid.Column>
                        <Grid.Column>
                            <Icon name='home' style={{ marginBottom: 5 }} size='large' />
                            <span>
                                <b>Number</b>
                                <br />
                                <i>{selectedEvent?.houseNumber ? selectedEvent?.houseNumber : '-'}</i>
                            </span>
                        </Grid.Column>
                    </Grid.Row>
                </Grid>
                {selectedEvent?.organizerId === accountStore.user?.id || accountStore.user?.role === "Admin"
                    ? <Segment basic style={{ bottom: '0' }}>
                        <Button floated='left' onClick={() => modalStore.openModal(<EventForm action={ActionTypes.Edit} id={id} />)}>
                            Edit
                        </Button>
                        <Button color='red' floated='right' onClick={handleDeleteButton}>
                            Delete
                        </Button>
                    </Segment>
                    : <></>
                }
            </Grid.Column>
        </Grid>
    )
})