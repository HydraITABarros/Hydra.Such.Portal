import React from 'react'
import PropTypes from 'prop-types'
import styled, { css } from 'styled-components'

import Button from '@material-ui/core/Button';

const styles = css`&& {
        border-radius: 10px;
    }
`

const DefaultButton = styled(Button)`${styles}`;

const MButton = ({ type, ...props }) => {
    const { to, href } = props
    if (to) {
        return <DefaultButton {...props} />
    } if (href) {
        return <DefaultButton {...props} />
    }
    return <DefaultButton {...props} type={type} />
}

DefaultButton.propTypes = Button.propTypes;

export default DefaultButton;