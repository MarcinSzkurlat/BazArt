import { observer } from "mobx-react-lite";
import { ChangeEvent, useEffect, useState } from "react";
import { Button, Checkbox, CheckboxProps, Form, Header, Icon, Popup, Segment } from "semantic-ui-react";
import { ManipulateProduct } from "../../app/models/Product/manipulateProduct";
import { ActionTypes } from "../../app/models/actionTypes";
import { useStore } from "../../app/stores/store";
import ErrorMessageBox from "../errors/ErrorMessageBox";
import { toast } from "react-toastify";

interface Props {
    id?: string;
    action: ActionTypes;
}

export default observer(function ProductForm({ id, action }: Props) {
    const { categoryStore, productStore } = useStore();
    const { selectedProduct, addProductImage, deleteProductImage } = productStore;

    const [isForSell, setIsForSell] = useState(false);
    const [errors, setErrors] = useState<Map<string, string[]>>();
    const [image, setImage] = useState<File>();

    const [formData, setFormData] = useState<ManipulateProduct>({
        id: '',
        name: '',
        description: '',
        price: null,
        isForSell: isForSell,
        quantity: null,
        imageUrl: null,
        category: 0
    });

    const handleInputTextChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData((previousData) => ({ ...previousData, [name]: value }));
    };

    const handleInputNumberChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        const number = parseFloat(value);
        setFormData((previousData) => ({ ...previousData, [name]: number }));
    };

    const handleIsForSellCheckbox = (e: React.FormEvent<HTMLInputElement>, data: CheckboxProps) => {
        if (data.checked !== undefined) {
            const isForSell = data.checked;
            setIsForSell(isForSell);
            setFormData({ ...formData, isForSell })
        }
    };

    const handleCategoryChange = (e: ChangeEvent<HTMLSelectElement>) => {
        if (e.target.value !== '0') {
            const category = parseInt(e.target.value);
            setFormData({ ...formData, category });
        }
    };

    const handleImageInput = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files?.length > 0) {
            const selectedFile = e.target.files[0];
            const maxFileSize = 1024 * 1024;

            if (selectedFile.size <= maxFileSize) {
                if (selectedFile.type === 'image/jpeg' || selectedFile.type === 'image/png') {
                    setImage(selectedFile);
                }
            } else {
                toast.error('File size is too large!');
            }
        } else {
            setImage(undefined);
        }
    }

    const handleResetImageButton = () => {
        deleteProductImage(formData.id);
    }

    const handleButton = () => {
        switch (action) {
            case ActionTypes.Create:
                productStore.createProduct(formData).catch(errors => setErrors(errors));
                break;
            case ActionTypes.Edit:
                productStore.editProduct(formData).catch(errors => setErrors(errors));
                if (image) addProductImage(formData.id, image);
                break;
        }
    }

    useEffect(() => {
        if (categoryStore.categories.length === 0) {
            categoryStore.loadCategories();
        }

        if (id && selectedProduct !== undefined) {
            setFormData({
                id: id,
                name: selectedProduct.name,
                description: selectedProduct.description,
                price: selectedProduct.price,
                isForSell: selectedProduct.isForSell,
                quantity: selectedProduct.quantity,
                imageUrl: selectedProduct.imageUrl,
                category: selectedProduct.categoryId
            })
            setIsForSell(selectedProduct.isForSell);
        }
    }, [id])

    return (
        <Segment className='modal-background' padded='very'>
            <Header textAlign='center'>{action} product</Header>
            <Form onSubmit={handleButton}>
                <Form.Field required>
                    <label>Name</label>
                    <input
                        defaultValue={formData.name}
                        onChange={handleInputTextChange}
                        name='name'
                        placeholder='Name'
                        minLength={5} maxLength={100} />
                </Form.Field>
                <Form.Field required>
                    <label>Description</label>
                    <textarea
                        defaultValue={formData.description}
                        onChange={handleInputTextChange}
                        name='description'
                        placeholder='Describe your product...'
                        minLength={5} maxLength={1000} />
                </Form.Field>
                <Form.Field>
                    <Checkbox
                        label='This product is for sell?'
                        name='isForSell'
                        checked={isForSell}
                        onChange={handleIsForSellCheckbox} />
                </Form.Field>
                {isForSell
                    ? <>
                        <Form.Field>
                            <label>Price for each</label>
                            <input
                                defaultValue={formData.price?.toString()}
                                onChange={handleInputNumberChange}
                                type='number'
                                step='0.01'
                                name='price'
                                placeholder='0 $'
                                min={0} max={99999999} />
                        </Form.Field>
                        <Form.Field>
                            <label>Quantity</label>
                            <input
                                defaultValue={formData.quantity?.toString()}
                                onChange={handleInputNumberChange}
                                type='number'
                                name='quantity'
                                placeholder='1'
                                min={1} max={999} />
                        </Form.Field>
                    </>
                    : <></>}
                {action === ActionTypes.Edit
                    ? <>
                        < Form.Field >
                            <label>Product image</label>
                            <input
                                type='file'
                                accept='.jpg, .png'
                                onChange={handleImageInput}
                                style={{ width: '40%' }} />
                            <Popup pinned position='top center' trigger={
                                <Button size='large' icon floated='right' color='red' circular onClick={handleResetImageButton} >
                                    <Icon name='x' />
                                </Button>}>
                                Reset product image
                            </Popup>
                        </Form.Field>
                        <Header color='red' size='tiny' style={{ marginLeft: '20px', marginTop: '0' }}>* maximum file size is 1MB</Header>
                    </>
                    : <></>}
                <Form.Field required>
                    <label>Category</label>
                    <select
                        value={formData.category}
                        onChange={handleCategoryChange}>
                        <option value='0'>Choose category</option>
                        {categoryStore.categories.map((category) => (
                            <option key={category.id} value={category.id}>{category.name}</option>
                        ))}
                    </select>
                </Form.Field>
                {errors
                    ? <ErrorMessageBox errors={errors} />
                    : <></>}
                <Form.Button content={action} disabled={formData.name.length < 5 || formData.description.length < 5 || formData.category === 0} />
            </Form>
        </Segment>
    )
})