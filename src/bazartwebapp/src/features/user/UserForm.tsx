import { observer } from "mobx-react-lite";
import { ChangeEvent, useEffect, useState } from "react";
import { Form } from "semantic-ui-react";
import { EditUser } from "../../app/models/User/editUser";
import { useStore } from "../../app/stores/store";
import ErrorMessageBox from "../errors/ErrorMessageBox";

export default observer(function UserForm() {
    const { userStore, accountStore, categoryStore } = useStore();
    const { currentUserDetails, loadCurrentUserDetails } = userStore;

    const [errors, setErrors] = useState<Map<string, string[]>>();

    const [formData, setFormData] = useState<EditUser>({
        stageName: null,
        description: null,
        country: null,
        city: null,
        street: null,
        houseNumber: null,
        postalCode: null,
        categoryId: 0
    });

    const handleInputTextChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData((previousData) => ({ ...previousData, [name]: value }));
    };

    const handleInputNumberChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        const number = parseInt(value);
        setFormData((previousData) => ({ ...previousData, [name]: number }));
    };

    const handleCategoryChange = (e: ChangeEvent<HTMLSelectElement>) => {
        const categoryId = parseInt(e.target.value);
        setFormData({ ...formData, categoryId });
    };

    const handleSubmitButton = () => {
        userStore.editCurrentUserDetails(formData).catch(errors => setErrors(errors));
    }

    useEffect(() => {
        if (categoryStore.categories.length === 0) {
            categoryStore.loadCategories();
        }

        if (accountStore.user?.id === currentUserDetails?.id) {
            setFormData({
                stageName: currentUserDetails?.stageName!,
                description: currentUserDetails?.description!,
                country: currentUserDetails?.country!,
                city: currentUserDetails?.city!,
                street: currentUserDetails?.street!,
                houseNumber: currentUserDetails?.houseNumber!,
                postalCode: currentUserDetails?.postalCode!,
                categoryId: currentUserDetails?.categoryId ?? 0
            })
        } else {
            loadCurrentUserDetails();
        }
    }, [currentUserDetails])

    return (
        <Form onSubmit={handleSubmitButton}>
            <Form.Field>
                <label>Stage name</label>
                <input
                    defaultValue={formData.stageName!}
                    onChange={handleInputTextChange}
                    name='stageName'
                    placeholder='Stage name'
                    minLength={3} maxLength={50} />
            </Form.Field>
            <Form.Field>
                <label>Description</label>
                <textarea
                    defaultValue={formData.description!}
                    onChange={handleInputTextChange}
                    name='description'
                    placeholder='Description'
                    minLength={3} maxLength={1000} />
            </Form.Field>
            <Form.Field>
                <label>Country</label>
                <input
                    defaultValue={formData.country!}
                    onChange={handleInputTextChange}
                    name='country'
                    placeholder='Country'
                    minLength={2} maxLength={100} />
            </Form.Field>
            <Form.Field>
                <label>City</label>
                <input
                    defaultValue={formData.city!}
                    onChange={handleInputTextChange}
                    name='city'
                    placeholder='City'
                    minLength={3} maxLength={100} />
            </Form.Field>
            <Form.Field>
                <label>Street</label>
                <input
                    defaultValue={formData.street!}
                    onChange={handleInputTextChange}
                    name='street'
                    placeholder='Street'
                    minLength={3} maxLength={100} />
            </Form.Field>
            <Form.Field>
                <label>House number</label>
                <input
                    defaultValue={formData.houseNumber?.toString()}
                    onChange={handleInputNumberChange}
                    type='number'
                    name='houseNumber'
                    placeholder='House number'
                    min={0} max={100000} />
            </Form.Field>
            <Form.Field>
                <label>Postal code</label>
                <input
                    defaultValue={formData.postalCode!}
                    onChange={handleInputTextChange}
                    name='postalCode'
                    placeholder='Postal code'
                    minLength={3} maxLength={10} />
            </Form.Field>
            <Form.Field>
                <label>Category</label>
                <select
                    value={formData.categoryId!}
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
            <Form.Button content="Submit" />
        </Form>
    )
})