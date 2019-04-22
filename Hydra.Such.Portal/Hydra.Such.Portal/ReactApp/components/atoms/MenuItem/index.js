import React from 'react';
import styled, { css, theme } from 'styled-components';
import MuiMenuItem from '@material-ui/core/MenuItem';
import _theme from '../../themes/default';
import Color from 'color';

const menuItemStyles = css`&& {
    font-family: ${_theme.fonts.primary};
    font-style: normal;
    font-weight: 400;
    font-size: 14px;
    line-height: 24px;
    color: ${props => props.color || _theme.palette.primary.default};
    padding-left: ${props => props.group ? '16px' : '18px'};
    pointer-events: ${props => props.group ? 'none' : ''};
    opacity: ${props => props.group ? '.8' : '1'};
}
`

const MenuItem = styled(MuiMenuItem)`${menuItemStyles}`;

export default MenuItem;
