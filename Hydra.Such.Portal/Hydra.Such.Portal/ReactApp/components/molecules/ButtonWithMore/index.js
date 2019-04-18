import React from 'react'
import PropTypes from 'prop-types'
import styled, { css, theme } from 'styled-components'

import MuiButton from '@material-ui/core/Button';

const _default = css`&& {
      color : ${props => props.theme.palette.primary};
    }
`

const DefaultButton = styled(MuiButton)`${styles}`;

const Button = ({ type, ...props }) => {
    const { to, href } = props
    if (props.default != 'undefined') {
        return <DefaultButton {...props} />
    } if (href) {
        return <DefaultButton {...props} />
    }
    return <DefaultButton {...props} type={type} />
}

export default Button;
