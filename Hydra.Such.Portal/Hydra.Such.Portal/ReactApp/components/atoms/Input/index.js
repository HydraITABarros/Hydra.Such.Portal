import React from 'react'
import styled, { css, theme } from 'styled-components'
import _theme from '../../themes/default';
import MuiOutlinedInput from '@material-ui/core/OutlinedInput';

const styles = css`&& {
        input {
            padding: 11px 15px;
            line-height: 13px;
            font-family: Inter,Helvetica,sans-serif;
            font-style: normal;
            font-weight: 400;
            font-size: 14px;
        }
        fieldset {
            border-radius: ${_theme.radius.primary};
        }
        [role="button"] {
            padding: 11px 35px 11px 15px;
        }
        &[class*="MuiInputBase-focused"] {
            fieldset {
                border-color: ${_theme.palette.secondary.default};
            }
        }
    }
`

const DefaultOutlinedInput = styled(MuiOutlinedInput)`${styles}`;

const Input = ({ ...props }) => {

    return <DefaultOutlinedInput fullWidth={props.fullWidth || true} labelWidth={props.labelWidth || 0} {...props} />
}

export default Input;
