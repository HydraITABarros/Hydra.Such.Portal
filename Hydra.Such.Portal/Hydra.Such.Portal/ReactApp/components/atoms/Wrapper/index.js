import React from 'react'
import styled, { css, theme } from 'styled-components'

const Wrapper = styled.div`
    margin: ${props => props.margin || props.justify && props.justify == "left" ? "auto auto auto 0" : props.justify && props.justify == "right" ? "auto 0 auto auto" : "auto"};
    padding: ${props => props.padding || 0};
    max-width: ${props => props.maxWidth + "px"};
    width:${props => props.width || "auto"};
    text-align: ${props => props.textAlign || "center"};
    line-height: ${props => props.lineHeight || "inherit"};
    display: ${props => props.inline ? 'inline-block' : 'block'};
`

export default Wrapper;