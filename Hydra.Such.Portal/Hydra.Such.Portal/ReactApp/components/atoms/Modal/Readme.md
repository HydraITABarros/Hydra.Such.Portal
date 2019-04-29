```jsx
import Button from '../Button';
import Text from '../Text';
import {Modal, DialogTitle,DialogContent,DialogActions} from './';

const ModalContent = (
    <div>
        <DialogTitle>Modal title </DialogTitle>
        <DialogContent>
        <hr/>  
            <Text p>
                Cras mattis consectetur purus sit amet fermentum. Cras justo odio, dapibus ac
                facilisis in, egestas eget quam. Morbi leo risus, porta ac consectetur ac, vestibulum
                at eros.
            </Text>
            <Text p>
                Praesent commodo cursus magna, vel scelerisque nisl consectetur et. Vivamus sagittis
                lacus vel augue laoreet rutrum faucibus dolor auctor.
            </Text>
            <Text p>
                Aenean lacinia bibendum nulla sed consectetur. Praesent commodo cursus magna, vel
                scelerisque nisl consectetur et. Donec sed odio dui. Donec ullamcorper nulla non metus
                auctor fringilla.
            </Text>
        </DialogContent>
        <DialogActions>
            <Button color="primary">Save changes</Button>
        </DialogActions> 
    </div>
);

<Modal action={<Button primary >Open Modal</Button>} children={ModalContent} />
```