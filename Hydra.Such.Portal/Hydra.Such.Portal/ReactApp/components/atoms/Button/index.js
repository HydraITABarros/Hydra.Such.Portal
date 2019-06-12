import React from 'react';
import styled, { css, theme } from 'styled-components';
import MuiButton from '@material-ui/core/Button';
import _theme from '../../../themes/default';
import Text from '../Text';
import Color from 'color';
import PropTypes from 'prop-types';
import _ from 'lodash';

const buttonTextPadding = {
    padding: '8px 24px',
}

const sharedStyles = css`&& {
    text-transform: none; 
    font-size: 23px;
    padding: ${buttonTextPadding.padding};
    box-shadow: ${props => props.boxShadow || `1px 1px 2px 0px ${Color(_theme.palette.primary.default).alpha(0.3).toString()}`}; 
    border-radius: ${_theme.radius.primary};
    &:hover {
        background: inherit;
    }
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
    box-shadow: ${props => props.boxShadow || `1px 1px 2px 0px ${Color(_theme.palette.primary.default).alpha(0.3).toString()}`}; 
    border-radius: ${_theme.radius.round};
    background-color: ${_theme.palette.white};
    color: ${_theme.palette.primary.medium};
    b {
        color: ${_theme.palette.primary.medium};
        line-height: 14px;
    }
    &:hover {
        background: inherit;
    }
}
`
const buttonSolo = css`&& {   
    width: 32px;
    min-width: auto;
    height: 32px;
    line-height: 14px;
    padding: 0;     
    text-transform: none; 
    border-radius: ${_theme.radius.round};
    color: ${_theme.palette.primary.medium};
}
`
const buttonOutline = css`&& {
    background-color: ${_theme.palette.white};
    color: ${_theme.palette.secondary.default};
    text-transform: none; 
    padding: ${buttonTextPadding.padding};
    box-shadow: none; 
    border: 2px solid ${_theme.palette.secondary.default};
    border-radius: ${_theme.radius.primary};
    b {
        color: ${_theme.palette.secondary.default};
    }
}
`
const picker = css`&& {
    color: ${_theme.palette.primary.default};
    box-shadow: none;
    padding: ${_theme.padding[8]};
    text-decoration: none;
    text-transform: none;
    color: ${_theme.palette.primary.default};
    font-size: 23px;
    line-height: 1;
    [class*="icon"] {
    }
    p {
        margin: 0;
        margin-top: 3px;
    }
}
`

const ButtonPrimary = styled(MuiButton)`${sharedStyles}${buttonPrimary}`;
const ButtonDefault = styled(MuiButton)`${sharedStyles}${buttonDefault}`;
const ButtonIcon = styled(MuiButton)`${sharedStyles}${buttonIcon}`;
const ButtonRound = styled((props) => <MuiButton {..._.omit(props, ['boxShadow'])} />)`${buttonRound}`;
const ButtonSolo = styled(MuiButton)`${buttonSolo}`;
const ButtonOutline = styled(MuiButton)`${buttonOutline}`;
const Picker = styled(MuiButton)`${picker}`;

const removePropertyFromObject = (obj, propertyName) => {
    let rest = {};
    Object.keys(obj).forEach((key, index) => {
        if (!(key == propertyName)) {
            rest[key] = obj[key];
        }
    });
    return rest;
}


const removeDefaultPropertiesFromComponentProps = (props, component) => {
    let rest = {};
    Object.keys(props).forEach((key, index) => {
        if (!(key in component.defaultProps)) {
            rest[key] = props[key];
        }
    });
    return rest;
}

const Button = ({ ...props }) => {
    if (props.primary) {
        return (
            <ButtonPrimary
                variant="contained"
                //color="primary"
                {..._.omit(props, ['primary', 'color'])}
            ><Text b>{props.children}</Text>
            </ButtonPrimary>
        )
    } else if (props.default) {
        return (
            <ButtonDefault
                variant="contained"
                {..._.omit(props, ['default', 'color'])}
            ><Text b>{props.children}</Text>
            </ButtonDefault>
        )
    } else if (props.picker) {
        return (
            <Picker
                {..._.omit(props, ['picker', 'color'])}
            >{props.icon}{"\u00a0"}<Text p>{props.children}</Text>
            </Picker>
        )
    } else if (props.icon) {
        return (
            <ButtonIcon
                variant="contained"
                {..._.omit(props, ['icon', 'color'])}
            >{props.icon}{"\u00a0"}<Text b>{props.children}</Text>
            </ButtonIcon>
        )
    } else if (props.link) {
        return (
            <Link src="javascript:void(0)"
                {..._.omit(props, ['link', 'color'])}
            ><Text b>{props.children}</Text>
            </Link>
        )
    } else if (props.round) {
        return (
            <ButtonRound
                {..._.omit(props, ['round', 'color'])}
            />
        )
    } else if (props.iconSolo) {
        return (
            <ButtonSolo
                {..._.omit(props, ['iconSolo', 'color'])}
            >
            </ButtonSolo>
        )
    } else if (props.outline) {
        return (
            <ButtonOutline
                variant="contained"
                {..._.omit(props, ['outline', 'color'])}
            ><Text b>{props.children}</Text>
            </ButtonOutline>
        )
    }
    return <ButtonPrimary {..._.omit(props, ['color'])} />
}

Button.propTypes = {
    primary: PropTypes.bool,
    default: PropTypes.bool,
    picker: PropTypes.bool,
    icon: PropTypes.object,
    link: PropTypes.bool,
    round: PropTypes.bool,
    iconSolo: PropTypes.bool,
    outline: PropTypes.bool
};

export default Button;
