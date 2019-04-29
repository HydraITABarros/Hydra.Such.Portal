```jsx
import Button from '../Button';
import Text from '../Text';
import {Modal, DialogTitle,DialogContent,DialogActions} from './';
import { Icon } from 'components';

const ModalContent = (
    <div>
        <DialogTitle><Text h2><Icon tecnico />{"\u00a0"}Modal title</Text></DialogTitle>
        <hr/>  
        <DialogContent>
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
        <hr/>  
        <DialogActions>
            <Button primary color="primary">Save changes</Button>
        </DialogActions> 
    </div>
);

<Modal action={<Button primary >Open Modal</Button>} children={ModalContent} />
```