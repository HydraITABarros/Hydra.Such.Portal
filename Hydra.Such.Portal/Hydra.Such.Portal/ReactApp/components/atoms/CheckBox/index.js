import React from 'react'
import styled, { css, theme, withTheme } from 'styled-components'
import MuiCheckbox from '@material-ui/core/Checkbox';
import _ from 'lodash';
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
var timeout = 0;
const CheckBox = ({ ...props }) => {
	if (props.$checked) {
		props.onChange = (e) => {
			var val = e.target.value;
			clearTimeout(timeout);
			timeout = setTimeout(() => {
				props.$checked.value = val;
			}, 100);
		}
		props.defaultChecked = props.$checked.value || '';
	}
	return <DefaultCheckBox checkedIcon={<Icon validation />} icon={<Icon circle-off />} {..._.omit(props, ['$checked'])} />
}

export default withTheme(CheckBox);