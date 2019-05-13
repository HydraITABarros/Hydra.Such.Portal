import React from 'react';
import styled, { css, theme } from 'styled-components';
import _theme from '../../themes/default';
import ReactTooltip from 'react-tooltip';

const tooltip = css`&& {
    margin-top: -7px;
    opacity: 1;
    padding: 1px;
    background: white;
    border: 1px solid ${_theme.palette.primary.light};
    border-radius: 5px;
    padding: 7px 16px 7px 41px;
    padding-left: ${props => props.icon ? /*'41px'*/ '16px' : '16px'};
    color: black;
    &&:after {
        border-top-color: ${_theme.palette.primary.light};
        opacity: 0;
    }
}
`

const Icon = styled.span`&& {
        font-size: 23px;
        color: ${props => props.color};
        position: absolute;
        top: -2px;
        left: 1px;
        bottom: 0;
        margin: auto;
        text-shadow: 0 0 1px ${props => props.color};
        padding: 9px;
    }
`

const Hidden = styled(ReactTooltip)`${tooltip}`;

export {
    Hidden,
    Icon
};
