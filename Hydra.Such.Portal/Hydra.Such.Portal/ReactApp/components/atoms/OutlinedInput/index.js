import React from 'react'
import styled, { css, theme } from 'styled-components'

import MuiOutlinedInput from '@material-ui/core/OutlinedInput';

const styles = css`&& {
        input {
            padding: 11px 15px;
        }
        fieldset {
            border-radius: 5px;
        }
        [role="button"] {
            padding: 11px 35px 11px 15px;
        }
    }
`

const DefaultOutlinedInput = styled(MuiOutlinedInput)`${styles}`;

const OutlinedInput = ({ ...props }) => {

    return <DefaultOutlinedInput fullWidth={props.fullWidth || true} labelWidth={props.labelWidth || 0} {...props} />
}

export default OutlinedInput;
