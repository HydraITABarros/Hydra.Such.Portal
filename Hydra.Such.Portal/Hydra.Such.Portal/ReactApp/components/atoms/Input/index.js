import React from 'react'
import styled, {css, theme, withTheme} from 'styled-components';
import MuiOutlinedInput from '@material-ui/core/OutlinedInput';
import {observable} from 'mobx';
import {observer} from 'mobx-react';

const styles = css`&& {
        input {
            padding: 11px 15px;
            line-height: 13px;
            font-family: Inter,Helvetica,sans-serif;
            font-style: normal;
            font-weight: 400;
            font-size: 14px;
	    background: ${props => props.theme.palette.bg.white};
        }
        fieldset {
            border-radius: ${props => props.theme.radius.primary};
        }
		&.MuiOutlinedInput-root.Mui-disabled {
			cursor: not-allowed;
			* {
				cursor: not-allowed;
			}
			fieldset {
				border-color: rgba(0, 0, 0, 0.23) ;
				border-color:${props => props.theme.palette.primary.light};
        	}
		}
        [role="button"] {
            padding: 11px 35px 11px 15px;
        }
        &.Mui-focused {
            background-color: white;
            [class*="MuiSelect-select"] {
                background-color: transparent;
            }
            fieldset {
                border-color: ${props => props.theme.palette.secondary.default};
				border-width: 1px;
            }
        }
    }
`

const DefaultOutlinedInput = styled(MuiOutlinedInput)`${styles}`;
var timeout = 0;

const Input = ({...props}) => {
    if (props.$value) {
        props.onChange = (e) => {
            var val = e.target.value;
            clearTimeout(timeout);
            timeout = setTimeout(() => {
                props.$value.value = val;
            }, 100);
        }
        props.defaultValue = props.$value.value || '';
    }

    return <DefaultOutlinedInput fullWidth={props.fullWidth || true}
                                 labelWidth={props.labelWidth || 0} {..._.omit(props, ['$value', 'classes'])} />
}

export default withTheme(Input);
