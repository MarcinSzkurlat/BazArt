import { observer } from "mobx-react-lite";
import { ChangeEvent, useEffect, useState } from "react";
import { Form } from "semantic-ui-react";
import { AccountRegistration } from "../../app/models/Account/accountRegistration";
import { useStore } from "../../app/stores/store";
import ErrorMessageBox from "../errors/ErrorMessageBox";

interface Props {
    isArtist: boolean;
}

export default observer(function RegistrationForm({ isArtist }: Props) {
    const { accountStore, categoryStore } = useStore();

    const [errors, setErrors] = useState<Map<string, string[]>>();
    const [formData, setFormData] = useState<AccountRegistration>({
        email: '',
        password: '',
        stageName: undefined,
        description: undefined,
        category: undefined
    });

    const handleInputChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData((previousData) => ({ ...previousData, [name]: value }));
    };

    const handleCategoryChange = (e: ChangeEvent<HTMLSelectElement>) => {
        if (e.target.value !== '0') {
            const category = parseInt(e.target.value);
            setFormData({ ...formData, category });
        }
    };

    const handleRegistrationButton = () => {
        accountStore.registration(formData).catch(errors => setErrors(errors));
    }

    useEffect(() => {
        if (isArtist && categoryStore.categories.length === 0) {
            categoryStore.loadCategories();
        }
    }, [isArtist, categoryStore])

    return (
        <Form size='small' onSubmit={handleRegistrationButton} style={{ marginTop: '20px' }}>
            <Form.Field required>
                <label>Email</label>
                <input
                    defaultValue={formData.email}
                    onChange={handleInputChange}
                    name='email'
                    placeholder='Email'
                    type='email'
                    maxLength={50} />
            </Form.Field>
            <Form.Field required>
                <label>Password</label>
                <input
                    defaultValue={formData.password}
                    onChange={handleInputChange}
                    name='password'
                    placeholder='Password'
                    type='password'
                    minLength={6} maxLength={40} />
            </Form.Field>
            {isArtist
                ? <>
                    <Form.Field>
                        <label>Stage name</label>
                        <input
                            defaultValue={formData.stageName}
                            onChange={handleInputChange}
                            name='stageName'
                            placeholder='Stage name'
                            minLength={3} maxLength={50} />
                    </Form.Field>
                    <Form.Field>
                        <label>Description</label>
                        <textarea
                            defaultValue={formData.description}
                            onChange={handleInputChange}
                            name='description'
                            placeholder='Tell us more about you...'
                            minLength={3} maxLength={1000} />
                    </Form.Field>
                    <Form.Field>
                        <label>Category name</label>
                        <select
                            value={formData.category}
                            onChange={handleCategoryChange}>
                            <option value='0'>Choose category</option>
                            {categoryStore.categories.map((category) => (
                                <option key={category.id} value={category.id}>{category.name}</option>
                            ))}
                        </select>
                    </Form.Field>
                </>
                : <></>}
            {errors
                ? <ErrorMessageBox errors={errors} />
                : <></>}
            <Form.Button content='Register' disabled={formData.email.length < 3 || formData.password.length < 6} />
        </Form>
    )
})