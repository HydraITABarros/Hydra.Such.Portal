import React from 'react';
import PropTypes from 'prop-types';
import styled, { css, theme } from 'styled-components';
import MuiButton from '@material-ui/core/Button';
import SaveIcon from '@material-ui/icons/Save';
import DeleteIcon from '@material-ui/icons/Delete';
import _theme from '../../themes/default';
import {Text} from 'components';
import Color from 'color';

const sharedStyles = css`&& {
    text-transform: none; 
    padding: ${_theme.padding[8]} ${_theme.padding[24]};
    box-shadow: 1px 1px 2px 0px ${Color(_theme.palette.primary.default).alpha(0.3).toString()}; 
    border-radius: ${_theme.radius.primary};
}
`
const buttonPrimary = css`&& {
    background-color: ${_theme.palette.secondary.default};
    color: ${_theme.palette.white};
    b {
        color: ${_theme.palette.white};
        }
}
`
const buttonDefault = css`&& {
    background-color: ${_theme.palette.white};
    color: ${_theme.palette.primary.medium};
    b {
        color: ${_theme.palette.primary.medium};
        }
}
`
const buttonIcon = css`&& {
    height: 40px;
    line-height: 14px;
    background-color: ${_theme.palette.white};
    color: ${_theme.palette.primary.medium};
    b {
        color: ${_theme.palette.primary.medium};
    }
}
`
const Link = styled.a`
    color: ${_theme.palette.primary.default};
    box-shadow: none;
    padding: ${_theme.padding[8]};
    text-decoration: underline;
    b {
        color: ${_theme.palette.primary.default};
        }
`
const buttonRound = css`&& {   
    width: 40px;
    min-width: auto;
    height: 40px;
    line-height: 14px;
    padding: 0;     
    text-transform: none; 
    box-shadow: 1px 1px 2px 0px ${Color(_theme.palette.primary.default).alpha(0.3).toString()}; 
    border-radius: 50%;
    background-color: ${_theme.palette.white};
    color: ${_theme.palette.primary.medium};
    b {
    color: ${_theme.palette.primary.medium};
    line-height: 14px;
    }
}
`

const ButtonPrimary = styled(MuiButton)`${sharedStyles}${buttonPrimary}`;
const ButtonDefault = styled(MuiButton)`${sharedStyles}${buttonDefault}`;
const ButtonIcon = styled(MuiButton)`${sharedStyles}${buttonIcon}`;
const ButtonRound = styled(MuiButton)`${buttonRound}`;


const Button = ({ type, ...props }) => {
    
    const { to, href } = props
    if (props.primary) {
        return <ButtonPrimary variant="contained" color="primary" {...props} ><Text b>{props.children}</Text></ButtonPrimary>
    } if (href) {
        return <ButtonPrimary {...props} />
    } else if (props.default) {
        return <ButtonDefault variant="contained" {...props} ><Text b>{props.children}</Text></ButtonDefault>
    } if (href) {
        return <ButtonDefault {...props} />
    } else if (props.icon) {
        return <ButtonIcon variant="contained" {...props} >{props.icon}{"\u00a0"}<Text b>{props.children}</Text></ButtonIcon>
    } if (href) {
        return <ButtonIcon {...props} />
    } else if (props.link) {
        return <Link src="javascript:void(0)" {...props} ><Text b>{props.children}</Text></Link>
    } if (href) {
        return <Link {...props} />
    } else if (props.round) {
        return <ButtonRound src="javascript:void(0)" {...props} ></ButtonRound>
    } if (href) {
        return <ButtonRound {...props} />
    } 
    return <ButtonPrimary {...props} type={type} />
}

export default Button;
