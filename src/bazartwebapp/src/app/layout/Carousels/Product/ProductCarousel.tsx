import { Product } from "../../../models/Product/product";
import ReactSimplyCarousel from 'react-simply-carousel';
import { useState } from "react";
import ProductItemCarousel from "./ProductItemCarousel";
import { Icon } from "semantic-ui-react";

interface Props {
    products: Product[];
}

export default function ProductCarousel({ products }: Props) {
    const [activeSliderIndex, setActiveSliderIndex] = useState(0);

    return (
        <div>
        <ReactSimplyCarousel
            activeSlideIndex={activeSliderIndex}
            onRequestChange={setActiveSliderIndex}
            itemsToShow={4}
            itemsToScroll={1}
            forwardBtnProps={{
                style: {
                    alignSelf: 'center',
                    background: 'transparent',
                    border: 'none',
                    cursor: 'pointer',
                    fontSize: '40px'
                },
                children: <span><Icon name="chevron right" /></span>,
            }}
            backwardBtnProps={{
                style: {
                    alignSelf: 'center',
                    background: 'transparent',
                    border: 'none',
                    cursor: 'pointer',
                    fontSize: '40px'
                },
                children: <span><Icon name="chevron left" /></span>,
            }}
            responsiveProps={[
                {
                    itemsToShow: 4,
                    itemsToScroll: 1,
                    autoplay: true,
                    autoplayDirection: 'forward',
                    autoplayDelay: 2000
                }
            ]}
            speed={1200}
            easing='linear'
        >
            {products.map((product: Product) => (
                <ProductItemCarousel key={product.id} product={product} />
            ))}
            </ReactSimplyCarousel>
        </div>
    )
}