import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import { Divider, Header, Image } from "semantic-ui-react";
import EventCarousel from "../../app/layout/Carousels/Event/EventCarousel";
import ProductCarousel from "../../app/layout/Carousels/Product/ProductCarousel";
import { useStore } from "../../app/stores/store";

export default observer(function CategoryPage() {
    const { categoryName } = useParams();

    const { categoryStore } = useStore();
    const { selectedCategory, loadCategory } = categoryStore;

    useEffect(() => {
        if (categoryName) loadCategory(categoryName)
    }, [])

    return (
        <>
            <div style={{ position: 'relative' }}>
                <Image centered src={selectedCategory?.imageUrl} alt={selectedCategory?.name} style={{ width: '70%', height: '350px', borderRadius:'15px' }} />
                <div className={'category-div-text'}>
                    <p className='category-header'>{selectedCategory?.name}</p>
                </div>
            </div>
            <Divider horizontal>
                <Header as='h1'>Products</Header>
            </Divider>
            <ProductCarousel page='category' categoryName={`${categoryName}`} />
            <Divider horizontal>
                <Header as='h1'>Events</Header>
            </Divider>
            <EventCarousel page='category' categoryName={`${categoryName}`} />
        </>
    )
})