import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Grid, Label, Image, Header, Icon, Popup, Button } from "semantic-ui-react";
import { Product } from "../../app/models/Product/product";
import { useStore } from "../../app/stores/store";

interface Props {
    product: Product;
}

export default observer(function CartProductItem({ product }: Props) {
    const { productStore } = useStore();

    const handleRemoveFavoriteButton = () => {
        productStore.deleteCartProduct(product.id);
    }

    return (
        <Grid columns={3}>
            <Grid.Column width={5} textAlign='center'>
                <div style={{ position: 'absolute', top: '20px', right: '30px', zIndex: '1' }}>
                    <Label as={Link} to={`/category/${product.categoryName}`} ribbon='right' color='black'>{product.categoryName}</Label>
                </div>
                <Image floated='right' size='small' as={Link} to={`/product/${product.id}`} src={product.imageUrl} alt={product.name} />
            </Grid.Column>
            <Grid.Column width={9} style={{ position: 'relative' }}>
                <Header as={Link} to={`/product/${product.id}`}>{product.name}</Header>
                <br/>
                <p><i>{product.description.length > 250
                    ? product.description.substring(0, 250) + "..."
                    : product.description}</i></p>
                <div style={{ position: 'absolute', bottom: '20px' }}>
                    {product.isForSell
                        ? <span><Icon name='dollar' /> {product.price}</span>
                        : <b>This product is not for sell</b>}
                </div>
            </Grid.Column>
            <Grid.Column width={2} verticalAlign='middle' textAlign='center'>
                <Popup pinned position='top center' trigger={
                    <Button size='large' icon color='red' circular onClick={handleRemoveFavoriteButton}>
                        <Icon name='x' />
                    </Button>}>
                    Remove product from cart
                </Popup>
            </Grid.Column>
        </Grid>
    )
})