import React from 'react'
import PropTypes from 'prop-types'
import styled, { css } from 'styled-components'

import MuiButton from '@material-ui/core/Button';

const styles = css`&& {
        border-radius: 10px;
    }
`

const DefaultButton = styled(MuiButton)`${styles}`;

const Button = ({ type, ...props }) => {
    const { to, href } = props
    if (to) {
        return <DefaultButton {...props} />
    } if (href) {
        return <DefaultButton {...props} />
    }
    return <DefaultButton {...props} type={type} />
}

export default Button;