import { Product } from "../../../models/Product/product";
import { Card, Icon, Image, Label } from "semantic-ui-react";

interface Props {
    product: Product;
}

export default function ProductItemCarousel({ product }: Props) {
    return (
        <Card raised style={{ padding: '15px', position: 'relative', background: 'transparent', margin:'10px' }}>
            <div style={{ position: 'absolute', top: '30px', right: '30px', zIndex: '1' }}>
                <Label as='a' href='/' ribbon='right' >{product.categoryName}</Label> {/* TODO change href to appropriate category page */}
            </div>
            <Image as='a' href='/' src={product.imageUrl} alt={product.name} size='medium' style={{ zIndex: '0' }} /> {/* TODO change href to product detail page */}
            <Card.Header as='h3' style={{margin:'10px'}}><b>{product.name}</b></Card.Header>
            <Card.Description style={{height:'70px'}}>
                <i>{product.description.length > 100
                    ? product.description.substring(0, 100) + "..."
                    : product.description}</i>
            </Card.Description>
            <Card.Content extra>
                <Icon name='dollar' /> {product.price}
            </Card.Content>
        </Card>
    )
}