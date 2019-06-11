import React from 'react'
import PropTypes from 'prop-types'
import styled, { css, theme, injectGlobal } from 'styled-components'
import _theme from '../../../themes/default'
import MuiButton from '@material-ui/core/Button';

const assetsPath = process.env.ASSETS_PATH || '';

injectGlobal`
    @font-face {
            font-family: 'Open Sans Condensed Light';
            src:  url('${assetsPath}/fonts/OpenSansCondensed-Light.ttf?x1kme9') format('truetype');
            font-weight: 300;
            font-style: normal;
    }
`
const Title = styled.h1` && {
        font-family: ${_theme.fonts.primary};
        font-style: normal;
        font-weight: 200;
        font-size: 48px;
        line-height: 56px;
        color: ${props => props.color || _theme.palette.primary.default};
    }
`
const Header = styled.h2` && {
        font-family: ${_theme.fonts.primary};
        font-style: normal;
        font-weight: 400;
        font-size: 24px;
        line-height: 32px;
        margin: 0;
        color: ${props => props.color || _theme.palette.primary.default};
    }
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
    margin: 0;
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
    /* font-size: 104px; */
    font-size: 99px;
    /* line-height: 104px; */
    line-height: 99px;
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
        return <Title {..._.omit(props, ['h1'])} />
    } else if (props.h2) {
        return <Header {..._.omit(props, ['h2'])} />
    } else if (props.h3) {
        return <SubHeader {..._.omit(props, ['h3'])} />
    } else if (props.p) {
        return <Paragraph {..._.omit(props, ['p'])} />
    } else if (props.b) {
        return <Bold {..._.omit(props, ['b'])} />
    } else if (props.label) {
        return <Label {..._.omit(props, ['label'])} />
    } else if (props.dataBig) {
        return <DataBig {..._.omit(props, ['dataBig'])} />
    } else if (props.dataSmall) {
        return <DataSmall {..._.omit(props, ['dataSmall'])} />
    }
    return <Default {..._.omit(props, ['span'])} />
}

export default Text;
