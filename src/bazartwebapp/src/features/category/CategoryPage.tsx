import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Link, useParams } from "react-router-dom";
import { Divider, Grid, Header, Image } from "semantic-ui-react";
import EventCarousel from "../../app/layout/Carousels/Event/EventCarousel";
import ProductCarousel from "../../app/layout/Carousels/Product/ProductCarousel";
import NavBar from "../../app/layout/NavBar";
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
            <Grid>
                <Grid.Row>
                    <Grid.Column width={6} textAlign='center'>
                        <Image as={Link} to='/' src="/assets/BazArt_logo_Theme_Light.jpeg" alt="logo" size="medium" verticalAlign='middle' />
                    </Grid.Column>
                    <Grid.Column width={10}>
                        <NavBar className="navbar-category-page" />
                    </Grid.Column>
                </Grid.Row>
            </Grid>
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