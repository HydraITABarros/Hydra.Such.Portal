import React from 'react'
import styled, { css, theme, withTheme } from 'styled-components';
import MuiOutlinedInput from '@material-ui/core/OutlinedInput';

const styles = css`&& {
        input {
            padding: 11px 15px;
            line-height: 13px;
            font-family: Inter,Helvetica,sans-serif;
            font-style: normal;
            font-weight: 400;
            font-size: 14px;
	    background: ${props => props.theme.palette.bg.white};
        }
        fieldset {
            border-radius: ${props => { return props.theme.radius.primary }};
        }
        [role="button"] {
            padding: 11px 35px 11px 15px;
        }
        &[class*="MuiInputBase-focused"] {
            background-color: white;
            [class*="MuiSelect-select"] {
                background-color: transparent;
            }
            fieldset {
                border-color: ${props => props.theme.palette.secondary.default};
		border-width: 1px;
            }
        }
    }
`

const DefaultOutlinedInput = styled(MuiOutlinedInput)`${styles}`;

const Input = ({ ...props }) => {
	return <DefaultOutlinedInput fullWidth={props.fullWidth || true} labelWidth={props.labelWidth || 0} {...props} />
}

export default withTheme(Input);
