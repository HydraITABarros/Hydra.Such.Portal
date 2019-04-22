import React from 'react'
import PropTypes from 'prop-types'
import styled, { css, theme } from 'styled-components'
import _theme from '../../themes/default'
import MuiButton from '@material-ui/core/Button';

const HtmlTag = styled.div` 
    display: inline-block;
    padding-right: 40px;
`
const H1 = styled.div` 
    font-family: ${_theme.fonts.primary};
    font-weight: 200;
    font-size: 48px;
    padding-bottom: 20px;
`
const H2 = styled.div` 
    font-family: ${_theme.fonts.primary};
    font-weight: 200;
    font-size: 24px;
    padding-bottom: 20px;
`
const H3 = styled.div` 
    font-family: ${_theme.fonts.primary};
    font-weight: 700;
    font-size: 18px;
    padding-bottom: 20px;
`
const PTag = styled.div` 
    font-family: ${_theme.fonts.primary};
    font-weight: 400;
    font-size: 14px;
    padding-bottom: 20px;
`
const Label = styled.div` 
    font-family: ${_theme.fonts.primary};
    font-weight: 700;
    font-size: 12px;
    text-transform: uppercase;
    padding-bottom: 20px;
`
const Texto = styled.div` 
    font-family: ${_theme.fonts.primary};
    font-weight: 200;
    font-size: 48px;
`
const Text = ({ type, ...props }) => {
console.log(_theme);
    
    return(
    <div>   
        <HtmlTag>
            <span class="rsg--para-11">html tag</span>
            <H1 color={_theme.fonts.primary}>h1</H1>
            <H2 color={_theme.fonts.primary}>h2</H2>
            <H3 color={_theme.fonts.primary}>h3</H3>
            <PTag color={_theme.fonts.primary}>p</PTag>
            <Label color={_theme.fonts.primary}>label</Label>
        </HtmlTag>

        <HtmlTag>
            <span class="rsg--para-11">font / weight</span>
            <H1 color={_theme.fonts.primary}>Inter Extra Light</H1>
            <H2 color={_theme.fonts.primary}>Inter Light</H2>
            <H3 color={_theme.fonts.primary}>Inter Light</H3>
            <PTag color={_theme.fonts.primary}>Inter Regular</PTag>
            <Label color={_theme.fonts.primary}>Inter Regular</Label>
        </HtmlTag>

        <HtmlTag>
            <span class="rsg--para-11">size</span>
            <H1 color={_theme.fonts.primary}>48px</H1>
            <H2 color={_theme.fonts.primary}>24px</H2>
            <H3 color={_theme.fonts.primary}>18px</H3>
            <PTag color={_theme.fonts.primary}>14px</PTag>
            <Label color={_theme.fonts.primary}>12px</Label>
        </HtmlTag>

        <HtmlTag>
            <span class="rsg--para-11">line-height</span>
            <H1 color={_theme.fonts.primary}>58px</H1>
            <H2 color={_theme.fonts.primary}>32px</H2>
            <H3 color={_theme.fonts.primary}>24px</H3>
            <PTag color={_theme.fonts.primary}>24px</PTag>
            <Label color={_theme.fonts.primary}>16px</Label>
        </HtmlTag>
    </div>
    )
}

export default Text;
