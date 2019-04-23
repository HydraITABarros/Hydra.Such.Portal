import React from 'react'
import styled, { css, theme } from 'styled-components'
import _theme from '../../themes/default';
import MuiCheckbox from '@material-ui/core/Checkbox';

const styles = css`&& {
    }
`

const DefaultCheckBox = styled(MuiCheckbox)`${styles}`;

const CheckBox = ({ ...props }) => {

    return <DefaultCheckBox {...props} />
}

export default CheckBox;
