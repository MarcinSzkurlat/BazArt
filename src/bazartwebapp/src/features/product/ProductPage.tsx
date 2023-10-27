import { observer } from "mobx-react-lite"
import { useEffect } from "react"
import { Link, useParams } from "react-router-dom"
import { Button, Divider, Grid, Header, Icon, Image, Label, Popup, Segment } from "semantic-ui-react"
import LoadingComponent from "../../app/layout/LoadingComponent"
import { ProductActionTypes } from "../../app/models/Product/productActionTypes"
import { useStore } from "../../app/stores/store"
import ProductForm from "./ProductForm"

export default observer(function ProductPage() {
    const { id } = useParams();
    const { productStore, accountStore, modalStore } = useStore();
    const { loadingInitial, loadProduct, selectedProduct } = productStore;


    const handleDeleteButton = () => {
        productStore.deleteProduct(id!);
    }

    useEffect(() => {
        if (id) loadProduct(id)
    }, [id])

    if (loadingInitial) return <LoadingComponent />

    return (
        <Grid container>
            <Grid.Column width={10}>
                <div style={{ position: 'absolute', top: '30px', right: '28px', zIndex: '1' }}>
                    <Label as={Link} to={`/category/${selectedProduct?.categoryName}`} ribbon='right' color='black'>{selectedProduct?.categoryName}</Label>
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
                            <Link to={`/user/${selectedProduct?.creatorId}`}>{selectedProduct?.creatorStageName ?? selectedProduct?.creatorEmail.split('@')[0]}</Link>
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
                    : <Header size='medium' textAlign='center'>This product is not for sell</Header>
                }
                <Segment basic style={{ position: 'absolute', bottom: '0', width: '100%' }}>
                    {selectedProduct?.creatorId === accountStore.user?.id || accountStore.user?.role === "Admin"
                        ? <>
                            <Button floated='left' onClick={() => modalStore.openModal(<ProductForm action={ProductActionTypes.Edit} id={id} />)}>
                                Edit
                            </Button>
                            <Button color='red' floated='right' onClick={handleDeleteButton}>
                                Delete
                            </Button>
                        </>
                        : <>
                            <Button floated='left' disabled={!selectedProduct?.isForSell}>
                                Add to cart
                            </Button>
                            <Popup pinned trigger={
                                <Button size='large' icon floated='right' color='red' circular>
                                    <Icon name='heart' />
                                </Button>}>
                                Add product to favorite
                            </Popup>
                        </>
                    }
                </Segment>
            </Grid.Column>
        </Grid>
    )
})