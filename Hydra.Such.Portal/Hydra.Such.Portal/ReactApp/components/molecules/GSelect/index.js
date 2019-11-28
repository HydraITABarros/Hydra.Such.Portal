import React from 'react';
import styled, {css, theme} from 'styled-components';
import {Input, Button, Icon, Text, Wrapper, Select, MenuItem} from 'components';

const Span = styled.span` 
    color: ${props => props.theme.palette.primary.medium};
`

const GSelect = (props) => {

    return (
        <div>
            <Select value>
                {props.nullable &&
                <MenuItem value><em>None</em></MenuItem>
                }
                {props.placeholder &&
                <MenuItem value style={{display: 'none'}}><Span>{props.placeholder}</Span></MenuItem>
                }
                {props.options && props.options.map((item, i) => {
                    return <MenuItem value={item.value} key={i}>{item.title}</MenuItem>
                })}
            </Select>
        </div>
    )
}

export default GSelect;
