import { observer } from "mobx-react-lite";
import { ChangeEvent, useEffect, useState } from "react";
import { Accordion, Button, Form, Header, Icon, Popup, Segment } from "semantic-ui-react";
import { ActionTypes } from "../../app/models/actionTypes";
import { useStore } from "../../app/stores/store";
import ErrorMessageBox from "../errors/ErrorMessageBox";
import DatePicker from 'react-datepicker';
import { ManipulateEvent } from "../../app/models/Event/manupulateEvent";
import { toast } from "react-toastify";

interface Props {
    id?: string;
    action: ActionTypes;
}

export default observer(function EventForm({ id, action }: Props) {
    const { categoryStore, eventStore } = useStore();
    const { selectedEvent, addEventImage, deleteEventImage } = eventStore;

    const [accordionVisible, setAccordionVisible] = useState(false);
    const [errors, setErrors] = useState<Map<string, string[]>>();
    const [startingDate, setStartingDate] = useState<Date | null>(null);
    const [endingDate, setEndingDate] = useState<Date | null>(null);
    const [image, setImage] = useState<File>();

    const isDataDisabled = (date: Date) => date > new Date();
    const isTimeDisabled = (date: Date) => date > startingDate!;

    const [formData, setFormData] = useState<ManipulateEvent>({
        id: '',
        name: '',
        description: '',
        imageUrl: null,
        category: 0,
        country: null,
        city: null,
        street: null,
        houseNumber: null,
        postalCode: null,
        startingDate: startingDate,
        endingDate: endingDate
    })

    const handleAccordionChange = () => {
        setAccordionVisible(!accordionVisible);
    }

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
        if (e.target.value !== '0') {
            const category = parseInt(e.target.value);
            setFormData({ ...formData, category });
        }
    };

    const handleStartingDateChange = (date: Date) => {
        setStartingDate(date);
        setFormData({ ...formData, ['startingDate']: date })
    }

    const handleEndingDateChange = (date: Date) => {
        setEndingDate(date);
        setFormData({ ...formData, ['endingDate']: date })
    }

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
        deleteEventImage(formData.id);
    }

    const handleButton = () => {
        switch (action) {
            case ActionTypes.Create:
                eventStore.createEvent(formData).catch(errors => setErrors(errors));
                break;
            case ActionTypes.Edit:
                eventStore.editEvent(formData).catch(errors => setErrors(errors));
                if (image) addEventImage(formData.id, image);
                break;
        }
    }

    useEffect(() => {
        if (categoryStore.categories.length === 0) {
            categoryStore.loadCategories();
        }

        if (id && selectedEvent !== undefined) {
            setFormData({
                id: id,
                name: selectedEvent.name,
                description: selectedEvent.description,
                imageUrl: selectedEvent.imageUrl,
                category: selectedEvent.categoryId,
                country: selectedEvent.country,
                city: selectedEvent.city,
                street: selectedEvent.street,
                houseNumber: selectedEvent.houseNumber,
                postalCode: selectedEvent.postalCode,
                startingDate: selectedEvent.startingDate,
                endingDate: selectedEvent.endingDate
            })
            setStartingDate(new Date(selectedEvent.startingDate));
            setEndingDate(new Date(selectedEvent.endingDate));
        }
    }, [id])

    return (
        <Segment className='modal-background' padded='very'>
            <Header textAlign='center'>{action} event</Header>
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
                        placeholder='Describe your event...'
                        minLength={5} maxLength={1000} />
                </Form.Field>
                <Form.Field required>
                    <label>Starting date</label>
                    <DatePicker
                        name='startingDate'
                        selectsStart
                        showTimeSelect
                        isClearable
                        dateFormat="dd/MM/yyyy h:mmaa"
                        selected={startingDate}
                        onChange={handleStartingDateChange}
                        monthsShown={2}
                        startDate={startingDate}
                        minDate={new Date()}
                        endDate={endingDate}
                        filterTime={isDataDisabled}
                    />
                </Form.Field>
                <Form.Field required>
                    <label>Ending date</label>
                    <DatePicker
                        name='endingDate'
                        selectsEnd
                        showTimeSelect
                        isClearable
                        dateFormat="dd/MM/yyyy h:mmaa"
                        selected={endingDate}
                        onChange={handleEndingDateChange}
                        startDate={startingDate}
                        endDate={endingDate}
                        minDate={startingDate}
                        monthsShown={2}
                        filterTime={isTimeDisabled}
                        disabled={startingDate === null && endingDate === null}
                    />
                </Form.Field>
                <Accordion style={{ marginBottom: '14px' }}>
                    <Accordion.Title onClick={handleAccordionChange}>
                        {accordionVisible
                            ? <Icon name='caret down' />
                            : <Icon name='caret right' />
                        }
                        Event details
                    </Accordion.Title>
                    <Accordion.Content active={accordionVisible}>
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
                                minLength={2} maxLength={100} />
                        </Form.Field>
                        <Form.Field>
                            <label>Street</label>
                            <input
                                defaultValue={formData.street!}
                                onChange={handleInputTextChange}
                                name='street'
                                placeholder='Street'
                                minLength={2} maxLength={100} />
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
                                minLength={2} maxLength={10} />
                        </Form.Field>
                        {action === ActionTypes.Edit
                            ? <>
                                < Form.Field >
                                    <label>Event image</label>
                                    <input
                                        type='file'
                                        accept='.jpg, .png'
                                        onChange={handleImageInput}
                                        style={{ width: '40%' }} />
                                    <Popup pinned position='top center' trigger={
                                        <Button size='large' icon floated='right' color='red' circular onClick={handleResetImageButton} >
                                            <Icon name='x' />
                                        </Button>}>
                                        Reset event image
                                    </Popup>
                                </Form.Field>
                                <Header color='red' size='tiny' style={{ marginLeft: '20px', marginTop: '0' }}>* maximum file size is 1MB</Header>
                            </>
                            : <></>}
                    </Accordion.Content>
                </Accordion>
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