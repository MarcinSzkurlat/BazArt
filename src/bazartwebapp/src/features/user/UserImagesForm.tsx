import { observer } from "mobx-react-lite";
import { useState } from "react";
import { toast } from "react-toastify";
import { Button, Grid, Header, Icon, Popup, Image } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";

export default observer(function UserImagesForm() {
    const { userStore } = useStore();
    const { currentUserDetails, loadingInitial, addUserAvatar, deleteUserAvatar, loadCurrentUserDetails, addUserBackgroundImage, deleteUserBackgroundImage } = userStore;

    const [avatar, setAvatar] = useState<File>();
    const [background, setBackground] = useState<File>();

    const handleInput = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files?.length > 0) {
            const selectedFile = e.target.files[0];
            const maxFileSize = 1024 * 1024;

            if (selectedFile.size <= maxFileSize) {
                if (selectedFile.type === 'image/jpeg' || selectedFile.type === 'image/png') {
                    if (e.target.name === 'avatar-input') setAvatar(selectedFile);
                    if (e.target.name === 'background-input') setBackground(selectedFile);
                }
            } else {
                toast.error('File size is too large!');
            }
        } else {
            if (e.target.name === 'avatar-input') setAvatar(undefined);
            if (e.target.name === 'background-input') setBackground(undefined);

        }
    }

    const handleResetAvatarButton = () => {
        deleteUserAvatar();
        loadUserData();
    }

    const handleResetBackgroundImageButton = () => {
        deleteUserBackgroundImage();
        loadUserData();
    }

    const handleSubmitButton = async () => {
        if (avatar || background) {
            if (avatar) await addUserAvatar(avatar);
            if (background) await addUserBackgroundImage(background);

            loadUserData();
        }

        setAvatar(undefined);
        setBackground(undefined);
    }

    const loadUserData = () => {
        setTimeout(() => {
            loadCurrentUserDetails();

        }, 5000)
    }

    return (
        <>
            <div style={{ position: 'relative' }}>
                <Image centered rounded className='image-fit-container' src={currentUserDetails?.backgroundImage} style={{ zIndex: '0' }} />
                <Popup pinned position='top center' trigger={
                    <Button size='large' icon color='red' circular style={{ position: 'absolute', zIndex: '2', top: '20px', right: '20px' }} onClick={handleResetBackgroundImageButton}>
                        <Icon name='x' />
                    </Button>}>
                    Reset background image
                </Popup>
            </div>
            <div className='center-element-to-its-container' style={{ zIndex: '1' }}>
                <div style={{ position: 'relative' }}>
                    <Image avatar src={currentUserDetails?.avatar} size='small' />
                    <Popup pinned position='top center' trigger={
                        <Button size='large' icon color='red' circular style={{ position: 'absolute', zIndex: '2', top: '0', right: '0' }} onClick={handleResetAvatarButton}>
                            <Icon name='x' />
                        </Button>}>
                        Reset avatar
                    </Popup>
                </div>
            </div>
            <Grid columns={2} textAlign='center' padded>
                <Grid.Column>
                    <Header size='small'>
                        Avatar:
                        <input name='avatar-input' type='file' accept='.jpg, .png' onChange={handleInput} />
                    </Header>
                </Grid.Column>
                <Grid.Column>
                    <Header size='small'>
                        Background image:
                        <input name='background-input' type='file' accept='.jpg, .png' onChange={handleInput} />
                    </Header>
                </Grid.Column>
            </Grid>
            <Header color='red' size='tiny' style={{ marginLeft: '20px', marginTop: '0' }}>* maximum file size is 1MB</Header>
            <Button loading={loadingInitial} onClick={handleSubmitButton} content='Submit' />
        </>
    )
})