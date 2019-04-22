import React from 'react'
import PropTypes from 'prop-types'
import styled, { css, theme } from 'styled-components'
import _theme from '../../themes/default'
import MuiButton from '@material-ui/core/Button';

const Wrapper = styled.div` 
   padding-bottom: 40px;
`
const ColorBox = styled.div` 
    display: inline-block;
    width: 140px;
    span {
        display: block;
    }
    &:after{
        content: "";
        display: block;
        width: 60px;
        height: 60px;
        background: ${props => props.color};
        border: ${props => (props.border||0)+"px"} solid #CCCCCC;
    }
`
const Colors = ({ type, ...props }) => {
console.log(_theme);
    
    return(
    <div>   
        <Wrapper>
            <ColorBox>
                <span class="rsg--para-11">Primary</span>
            </ColorBox>
            <ColorBox color={_theme.palette.primary.default} >
                <span class="rsg--para-11">default</span>
            </ColorBox>
            <ColorBox color={_theme.palette.primary.dark} >
                <span class="rsg--para-11">dark</span>
            </ColorBox>
            <ColorBox color={_theme.palette.primary.medium} >
                <span class="rsg--para-11">medium</span>
            </ColorBox>
            <ColorBox color={_theme.palette.primary.light} >
                <span class="rsg--para-11">light</span>
            </ColorBox>
            <ColorBox color={_theme.palette.primary.keylines} >
                <span class="rsg--para-11">keylines</span>
            </ColorBox>
        </Wrapper>

        <Wrapper>
            <ColorBox>
                <span class="rsg--para-11">Secondary</span>
            </ColorBox>
            <ColorBox color={_theme.palette.secondary.default} >
                <span class="rsg--para-11">default</span>
            </ColorBox>
            <ColorBox color={_theme.palette.secondary.light} >
                <span class="rsg--para-11">light</span>
            </ColorBox>
        </Wrapper>

        <Wrapper>
            <ColorBox>
                <span class="rsg--para-11">Background</span>
            </ColorBox>
            <ColorBox color={_theme.palette.bg.white} border="1">
                <span class="rsg--para-11">white</span>
            </ColorBox>
            <ColorBox color={_theme.palette.bg.grey} >
                <span class="rsg--para-11">grey</span>
            </ColorBox>
        </Wrapper>

        <Wrapper>
            <ColorBox>
                <span class="rsg--para-11">Alert</span>
            </ColorBox>
            <ColorBox color={_theme.palette.alert.bad} >
                <span class="rsg--para-11">bad</span>
            </ColorBox>
            <ColorBox color={_theme.palette.alert.good} >
                <span class="rsg--para-11">good</span>
            </ColorBox>
        </Wrapper>

        <Wrapper>
            <ColorBox>
                <span class="rsg--para-11">Search</span>
            </ColorBox>
            <ColorBox color={_theme.palette.search} >
                <span class="rsg--para-11">search</span>
            </ColorBox>
        </Wrapper>

        <Wrapper>
            <ColorBox>
                <span class="rsg--para-11">White</span>
            </ColorBox>
            <ColorBox  color={_theme.palette.bg.white} border="1">
                <span class="rsg--para-11">white</span>
            </ColorBox>
        </Wrapper>
    </div>
    )
}

export default Colors;
