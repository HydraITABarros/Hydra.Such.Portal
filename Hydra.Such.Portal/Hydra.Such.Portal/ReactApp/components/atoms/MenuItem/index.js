import React from 'react';
import styled, { css, theme } from 'styled-components';
import MuiMenuItem from '@material-ui/core/MenuItem';
import Color from 'color';

const menuItemStyles = css`&& {
    font-family: ${({ theme }) => theme.fonts.primary};
    font-style: normal;
    font-weight: 400;
    font-size: 14px;
    line-height: 24px;
    color: ${props => props.color || props.theme.palette.primary.default};
    padding-left: ${props => props.group ? '18px' : '23px'};
    ${'' /* background: ${props => props.group ? _theme.palette.primary.keylines : 'inherit'}; */}
    pointer-events: ${props => props.group ? 'none' : ''};
    opacity: ${props => props.group ? '.5' : '1'};

    &&[class*="MuiListItem-selected"] {
        background-color: ${props => Color(props.theme.palette.primary.keylines).lighten(0.5).rgb()};
    }
}
`

const MenuItem = styled(MuiMenuItem)`${menuItemStyles}`;

export default MenuItem;
