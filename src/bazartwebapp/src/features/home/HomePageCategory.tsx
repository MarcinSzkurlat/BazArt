import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Link } from "react-router-dom";
import { Card, Reveal, Image } from "semantic-ui-react";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { Category } from "../../app/models/Category/category";
import { useStore } from "../../app/stores/store";

export default observer(function HomePageCategory() {
    const { categoryStore } = useStore();
    const { categoriesRegistry, loadingInitial, loadCategories } = categoryStore;

    useEffect(() => {
        loadCategories();
    },[])

    if (loadingInitial) return <LoadingComponent />

    return (
        <Card.Group itemsPerRow={2} stackable centered>
            {Array.from(categoriesRegistry.values()).map((category: Category) => (
                <Card as={Link} to={`category/${category.name}`} key = { category.id } style = {{ width: '35%', height: '350px' }}>
                    <Reveal animated='small fade'>
                        <Reveal.Content visible style={{ width:'100%', height: '350px' }}>
                            <Image src={category.imageUrl} alt={category.name} style={{ width: '100%', height: '350px' }} />
                            <div className={'category-div-text'}>
                                <Card.Header className='category-header'>{category.name}</Card.Header>
                            </div>
                        </Reveal.Content>
                        <Reveal.Content hidden style={{ width:'100%', height: '350px' }}>
                            <Image src={category.imageUrl} alt={category.name} style={{ width: '100%', height: '350px', filter: 'blur(8px)' }} />
                            <div className={'category-div-text'}>
                                <Card.Description className='category-description'>{category.description}</Card.Description>
                            </div>
                        </Reveal.Content>
                    </Reveal>
                </Card>
            ))}
        </Card.Group>
    )
})