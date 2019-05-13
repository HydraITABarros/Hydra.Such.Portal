import React from 'react'
import styled, { css, theme } from 'styled-components'

const Spacer = styled.div`
    display: ${props => props.width ? 'inline-block' : 'block'};
    height: ${props => props.height || 0};
    width: ${props => props.width || 'auto'};
`

export default Spacer;