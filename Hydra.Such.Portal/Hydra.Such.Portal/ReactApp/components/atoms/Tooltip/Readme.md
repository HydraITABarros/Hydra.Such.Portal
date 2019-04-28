```jsx
import Text from '../Text';
import Icon from '../Icon';
import { renderToString } from 'react-dom/server';

<div>
        <Text span  primary 
            data-tip={ renderToString(<span><Tooltip.Icon><Icon report /></Tooltip.Icon><Text b>Simple Tooltip</Text></span>) }
            data-html={true} 
        >
            Simple Tooltip
        </Text>
        <Tooltip.Hidden/>
</div>
```