import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Button, Divider, Header, Image } from "semantic-ui-react";
import EventCarousel from "../../app/layout/Carousels/Event/EventCarousel";
import { PageTypes } from "../../app/layout/Carousels/pageTypes";
import ProductCarousel from "../../app/layout/Carousels/Product/ProductCarousel";
import EventGridItems from "../../app/layout/GridItems/Event/EventGridItems";
import ProductGridItems from "../../app/layout/GridItems/Product/ProductGridItems";
import { useStore } from "../../app/stores/store";

export default observer(function CategoryPage() {
    const { categoryName } = useParams();

    const { categoryStore, eventStore, productStore } = useStore();
    const { selectedCategory, loadCategory } = categoryStore;

    const [visibleEventsGrid, setVisibleEventsGrid] = useState(false);
    const [visibleProductsGrid, setVisibleProductsGrid] = useState(false);

    useEffect(() => {
        if (categoryName) loadCategory(categoryName)
    }, [])

    return (
        <>
            <div style={{ position: 'relative' }}>
                <Image centered src={selectedCategory?.imageUrl} alt={selectedCategory?.name} style={{ width: '70%', height: '350px', borderRadius:'15px' }} />
                <div className={'center-element-to-its-container'}>
                    <p className='category-header'>{selectedCategory?.name}</p>
                </div>
            </div>
            <Divider horizontal>
                <Header as='h1'>Products</Header>
            </Divider>
            {visibleProductsGrid
                ? <ProductGridItems page={PageTypes.Category} categoryName={`${categoryName}`} />
                : <ProductCarousel page={PageTypes.Category} categoryName={`${categoryName}`} />}
            {productStore.totalPages > 1 && visibleProductsGrid === false
                ? <div style={{ textAlign: 'center', marginTop: '30px' }}>
                    <Button onClick={() => setVisibleProductsGrid(true)}>Show more</Button>
                </div>
                : <></>}
            <Divider horizontal>
                <Header as='h1'>Events</Header>
            </Divider>
            {visibleEventsGrid
                ? <EventGridItems page={PageTypes.Category} categoryName={`${categoryName}`} />
                : <EventCarousel page={PageTypes.Category} categoryName={`${categoryName}`} />}
            {eventStore.totalPages > 1 && visibleEventsGrid === false
                ? <div style={{ textAlign: 'center', marginTop:'30px' }}>
                    <Button onClick={() => setVisibleEventsGrid(true)}>Show more</Button>
                </div>
                : <></>}
        </>
    )
})