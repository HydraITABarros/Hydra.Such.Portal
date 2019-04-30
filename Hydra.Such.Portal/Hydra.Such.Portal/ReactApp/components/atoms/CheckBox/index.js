import React from 'react'
import styled, { css, theme } from 'styled-components'
import _theme from '../../themes/default';
import MuiCheckbox from '@material-ui/core/Checkbox';
import { Icon } from 'components';

const styles = css`&& {
    padding: 8px;
    &[class*="MuiCheckbox-checked"]{
        color: ${_theme.palette.alert.good};
        }
    }
`

const DefaultCheckBox = styled(MuiCheckbox)`${styles}`;

const CheckBox = ({ ...props }) => {

    return <DefaultCheckBox checkedIcon={<Icon validation />} icon={<Icon circle />} {...props} />
}

export default CheckBox;