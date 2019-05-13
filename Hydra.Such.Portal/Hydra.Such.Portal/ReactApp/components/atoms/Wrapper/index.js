import React from 'react'
import styled, { css, theme } from 'styled-components'
import { createMuiTheme } from '@material-ui/core/styles';

const muiTheme = createMuiTheme();
const breakpoints = muiTheme.breakpoints.values;

const Wrapper = styled.div`
    margin: ${props => props.margin || (props.justify && props.justify == "left" ? "auto auto auto 0" : props.justify && props.justify == "right" ? "auto 0 auto auto" : "auto")};
    padding: ${props => props.padding || 0};
    max-width: ${props => props.maxWidth ? props.maxWidth + "px" : "none"};
    width:${props => props.width || "auto"};
    text-align: ${props => props.textAlign || "left"};
    line-height: ${props => props.lineHeight || "inherit"};
    display: ${props => props.inline ? 'inline-block' : 'block'};
    @media (max-width: ${breakpoints["md"] + "px"}) {
        padding: ${props => props.mdPadding || (props.padding || 0)};
        text-align: ${props => props.mdTextAlign || (props.textAlign || "left")};
    }
    @media (max-width: ${breakpoints["sm"] + "px"}) {
        padding: ${props => props.smPadding || (props.mdPadding || (props.padding || 0))};
        text-align: ${props => props.smTextAlign || (props.mdTextAlign || (props.textAlign || "left"))};
    }
`

export default Wrapper;