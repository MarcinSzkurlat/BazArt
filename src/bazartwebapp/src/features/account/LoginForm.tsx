import { observer } from "mobx-react-lite";
import { ChangeEvent, useState } from "react";
import { Form } from "semantic-ui-react";
import { AccountLogin } from "../../app/models/Account/accountLogin";
import { useStore } from "../../app/stores/store";
import ErrorMessageBox from "../errors/ErrorMessageBox";

export default observer(function LoginForm() {
    const { accountStore } = useStore();

    const [errors, setErrors] = useState<Map<string, string[]>>();
    const [formData, setFormData] = useState<AccountLogin>({
        email: '',
        password: ''
    });

    const handleInputChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData((previousData) => ({ ...previousData, [name]: value }));
    };

    const handleLoginButton = () => {
        accountStore.login(formData).catch(errors => setErrors(errors));
    }

    return (
        <Form onSubmit={handleLoginButton}>
            <Form.Field>
                <label>Email</label>
                <input
                    defaultValue={formData.email}
                    onChange={handleInputChange}
                    name='email'
                    placeholder='Email'
                    type='email'
                    maxLength={50} />
            </Form.Field>
            <Form.Field>
                <label>Password</label>
                <input
                    defaultValue={formData.password}
                    onChange={handleInputChange}
                    name='password'
                    placeholder='Password'
                    type='password'
                    minLength={6} maxLength={40} />
            </Form.Field>
            {errors
                ? <ErrorMessageBox errors={errors} />
                : <></>}
            <Form.Button content='Login' disabled={formData.email.length < 3 || formData.password.length < 6} />
        </Form>
    )
})