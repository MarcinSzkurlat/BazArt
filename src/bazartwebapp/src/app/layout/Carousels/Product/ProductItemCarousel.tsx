import { Product } from "../../../models/Product/product";
import { Card, Icon, Image, Label } from "semantic-ui-react";

interface Props {
    product: Product;
}

export default function ProductItemCarousel({ product }: Props) {
    return (
        <Card raised style={{ padding: '15px', position: 'relative', background: 'transparent', margin:'10px' }}>
            <div style={{ position: 'absolute', top: '30px', right: '30px', zIndex: '1' }}>
                <Label as='a' href={`category/${product.categoryName}`} ribbon='right' color='black'>{product.categoryName}</Label>
            </div>
            <Image as='a' href={`/product/${product.id}`} src={product.imageUrl} alt={product.name} size='medium' style={{ zIndex: '0' }} />
            <Card.Header as='h3' textAlign='center' style={{ margin: '10px' }}><b>{product.name}</b></Card.Header>
            <Card.Description style={{height:'70px'}}>
                <i>{product.description.length > 100
                    ? product.description.substring(0, 100) + "..."
                    : product.description}</i>
            </Card.Description>
            <Card.Content extra>
                {product.isForSell
                    ? <span><Icon name='dollar' /> {product.price}</span>
                    : <b>This product is not for sell</b>}
            </Card.Content>
        </Card>
    )
}