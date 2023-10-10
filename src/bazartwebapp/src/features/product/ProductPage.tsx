import { observer } from "mobx-react-lite"
import { useEffect } from "react"
import { Link, useParams } from "react-router-dom"
import { Button, Divider, Grid, Header, Icon, Image, Label, Segment } from "semantic-ui-react"
import LoadingComponent from "../../app/layout/LoadingComponent"
import { useStore } from "../../app/stores/store"

export default observer(function ProductPage() {
    const { id } = useParams();
    const { productStore } = useStore();
    const { loadingInitial, loadProduct, selectedProduct } = productStore;

    useEffect(() => {
        if (id) loadProduct(id)
    }, [])

    if (loadingInitial) return <LoadingComponent />

    return (
        <Grid container>
            <Grid.Column width={10}>
                <div style={{ position: 'absolute', top: '30px', right: '28px', zIndex: '1' }}>
                    <Label as='a' href={`/category/${selectedProduct?.categoryName}`} ribbon='right' color='black'>{selectedProduct?.categoryName}</Label>
                </div>
                <Image src={selectedProduct?.imageUrl} rounded style={{ width: '100%', zIndex: '0' }} />
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
                            {selectedProduct?.created.split('T')[0]}
                        </Grid.Column>
                        <Grid.Column floated='right' textAlign='right'>
                            <Link to={`/user/${selectedProduct?.creatorId}`}>{selectedProduct?.creatorName}</Link>
                        </Grid.Column>
                    </Grid.Row>
                </Grid>
            </Grid.Column>
            <Grid.Column width={6}>
                <Header textAlign='center' size='large'>{selectedProduct?.name}</Header>
                <Divider horizontal>
                    <Header as='h6'>Description</Header>
                </Divider>
                <p>{selectedProduct?.description}</p>
                <Divider horizontal>
                    <Header as='h6'>Details</Header>
                </Divider>
                {selectedProduct?.isForSell
                    ? <>
                        {selectedProduct.quantity > 1
                            ? <>
                                <span><b>Quantity: </b> <i>{selectedProduct.quantity}</i></span>
                                <br />
                            </>
                            : <></>}
                        <span>
                            <b>Price: </b>
                            <i>
                                ${selectedProduct.price}
                                {selectedProduct.quantity > 1
                                    ? <> each</>
                                    : ''}
                            </i>
                        </span>
                    </>
                    : <Header size='medium' textAlign='center'>This product is not for sell</Header>}
                <Segment basic style={{ position: 'absolute', bottom: '0', width: '100%' }}>
                    <Button floated='left' disabled={!selectedProduct?.isForSell}>
                        Add to cart
                    </Button>
                    <Button icon floated='right' color='red' circular>
                        <Icon name='heart' />
                    </Button>
                </Segment>
            </Grid.Column>
        </Grid>
    )
})