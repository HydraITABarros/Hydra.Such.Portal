```js

import MuiAddIcon from '@material-ui/icons/Add';
import MuiDeleteIcon from '@material-ui/icons/Delete';
import { Icon } from 'components';

<div>
    <Button primary>Primary</Button>{"\u00a0","\u00a0","\u00a0"}
    <Button default>Default</Button>{"\u00a0","\u00a0","\u00a0"}
    <Button icon={<Icon download/>}>Default</Button>{"\u00a0","\u00a0","\u00a0"}
    <Button iconPrimary={<Icon download/>}>Default</Button>{"\u00a0","\u00a0","\u00a0"}
    <Button link>Text Link</Button>{"\u00a0","\u00a0","\u00a0"}
    <Button round><Icon add/></Button>{"\u00a0","\u00a0","\u00a0"}
    <Button round b><Icon remove/></Button>{"\u00a0","\u00a0","\u00a0"}
    <Button outline>Outline{"\u00a0","\u00a0"}0/25</Button>{"\u00a0","\u00a0","\u00a0"}
    <Button iconSolo><Icon decline /></Button>

</div>
```