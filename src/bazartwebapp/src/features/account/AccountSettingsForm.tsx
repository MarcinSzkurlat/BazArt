import { observer } from "mobx-react-lite";
import { ChangeEvent, useState } from "react";
import { Button, Confirm, Form, FormField } from "semantic-ui-react";
import { AccountChangePassword } from "../../app/models/Account/accountChangePassword";
import { useStore } from "../../app/stores/store";
import ErrorMessageBox from "../errors/ErrorMessageBox";

export default observer(function AccountSettingsForm() {
    const { accountStore } = useStore();

    const [confirmPassword, setConfirmPassword] = useState('');
    const [confirmDelete, setConfirmDelete] = useState(false);
    const [errors, setErrors] = useState<Map<string, string[]>>();

    const [formData, setFormData] = useState<AccountChangePassword>({
        newPassword: '',
        oldPassword: ''
    });

    const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        if (name === 'confirmPassword') {
            setConfirmPassword(value);
        } else {
            setFormData((previousData) => ({ ...previousData, [name]: value }));
        }
    };

    const handleSubmitButton = () => {
        if (confirmPassword === formData.newPassword) {
            accountStore.changePassword(formData).catch(errors => setErrors(errors));
        } else {
            let errors = new Map<string, string[]>();
            errors.set("Confirm password", ["The confirmed password is not the same as new password"]);
            setErrors(errors);
        }
    };

    const handleDeleteConfirm = () => {
        if (accountStore.user === null) {
            accountStore.getCurrentUser();
        }

        accountStore.deleteAccount(accountStore.user?.id!).catch(errors => setErrors(errors));
        setConfirmDelete(false);
    }
    return (
        <>
            <Form onSubmit={handleSubmitButton} >
                <FormField>
                    <label>Old password</label>
                    <input
                        onChange={handleInputChange}
                        name='oldPassword'
                        placeholder='Old password'
                        type='password'
                        minLength={6} maxLength={40} />
                </FormField>
                <FormField>
                    <label>New password</label>
                    <input
                        disabled={formData.oldPassword.length < 6}
                        onChange={handleInputChange}
                        name='newPassword'
                        placeholder='New password'
                        type='password'
                        minLength={6} maxLength={40} />
                </FormField>
                <FormField>
                    <label>Confirm new password</label>
                    <input
                        disabled={formData.newPassword.length < 6}
                        onChange={handleInputChange}
                        name='confirmPassword'
                        placeholder='Confirm password'
                        type='password'
                        minLength={6} maxLength={40} />
                </FormField>
                {errors
                    ? <ErrorMessageBox errors={errors} />
                    : <></>}
                <Form.Button content="Submit" disabled={formData.newPassword !== confirmPassword || formData.newPassword === '' || formData.oldPassword.length < 6} />
            </Form>
            <br />
            <Button negative content="Delete account" onClick={() => setConfirmDelete(true)} />
            <Confirm
                open={confirmDelete}
                cancelButton='Cancel'
                confirmButton="Yes, delete account"
                onCancel={() => setConfirmDelete(false)}
                onConfirm={handleDeleteConfirm}
            />
        </>
    )
})