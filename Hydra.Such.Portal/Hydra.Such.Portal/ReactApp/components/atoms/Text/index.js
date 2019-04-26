import React from 'react'
import PropTypes from 'prop-types'
import styled, { css, theme } from 'styled-components'
import _theme from '../../themes/default'
import MuiButton from '@material-ui/core/Button';


const Title = styled.h1`
    font-family: ${_theme.fonts.primary};
    font-style: normal;
    font-weight: 200;
    font-size: 48px;
    line-height: 56px;
    color: ${props => props.color || _theme.palette.primary.default};

`
const Header = styled.h2`
    font-family: ${_theme.fonts.primary};
    font-style: normal;
    font-weight: 300;
    font-size: 24px;
    line-height: 32px;
    color: ${props => props.color || _theme.palette.primary.default};
`
const SubHeader = styled.h3`
    font-family: ${_theme.fonts.primary};
    font-style: normal;
    font-weight: 700;
    font-size: 18px;
    line-height: 24px;
    color: ${props => props.color || _theme.palette.primary.default};
`
const Paragraph = styled.p`
    font-family: ${_theme.fonts.primary};
    font-style: normal;
    font-weight: 400;
    font-size: 14px;
    line-height: 24px;
    color: ${props => props.color || _theme.palette.primary.default};
     
`
const Bold = styled.b`
    font-family: ${_theme.fonts.primary};
    font-style: normal;
    font-weight: 700;
    font-size: 14px;
    line-height: 24px;
    color: ${props => props.color || _theme.palette.primary.default};
     
`
const Label = styled.label`
    font-family: ${_theme.fonts.primary};
    font-style: normal;
    font-weight: 700;
    font-size: 12px;
    line-height: 16px;
    text-transform: uppercase; 
    color: ${props => props.color || _theme.palette.primary.medium};    
`
const Default = styled.span`
    font-family: ${_theme.fonts.primary};
    font-weight: 200;
    font-size: 14px;
    line-height: 24px;
    color: ${props => props.color || _theme.palette.primary.default};     
`
const DataBig = styled.span`
    font-family: ${_theme.fonts.data};
    font-style: light;
    font-weight: 300;
    font-size: 104px;
    line-height: 104px;
    color: ${props => props.color || _theme.palette.primary.dark};    
`
const DataSmall = styled.span`
    font-family: ${_theme.fonts.data};
    font-style: light;
    font-weight: 300;
    font-size: 32px;
    line-height: 32px;
    color: ${props => props.color || _theme.palette.primary.dark};    
`

const Text = ({ ...props }) => {

    if (props.h1) {
        return <Title {...props} />
    } else if (props.h2) {
        return <Header {...props} />
    } else if (props.h3) {
        return <SubHeader {...props} />
    } else if (props.p) {
        return <Paragraph {...props} />
    } else if (props.b) {
        return <Bold {...props} />
    } else if (props.label) {
        return <Label {...props} />
    } else if (props.dataBig) {
        return <DataBig {...props} />
    } else if (props.dataSmall) {
        return <DataSmall {...props} />
    }
    return <Default {...props} />
}

export default Text;
