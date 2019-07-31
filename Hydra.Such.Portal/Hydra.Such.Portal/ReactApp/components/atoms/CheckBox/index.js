import React from 'react'
import styled, { css, theme, withTheme } from 'styled-components'
import MuiCheckbox from '@material-ui/core/Checkbox';
import { Icon } from 'components';

const styles = css`&& {
    padding: 8px;
    &[class*="MuiCheckbox-checked"]{
        color: ${(props) => props.theme.palette.alert.good};
        }
    }
    .icon-circle-off:before {
	background: ${props => props.theme.palette.bg.white};
    	border-radius: 50%;
    }
`

const DefaultCheckBox = styled(MuiCheckbox)`${styles}`;

const CheckBox = ({ ...props }) => {

	return <DefaultCheckBox checkedIcon={<Icon validation />} icon={<Icon circle-off />} {...props} />
}

export default withTheme(CheckBox);