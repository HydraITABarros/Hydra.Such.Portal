import React from 'react'
import styled, { css, theme } from 'styled-components'
import _theme from '../../themes/default'

import MuiSelect from '@material-ui/core/Select';
import { OutlinedInput } from 'components';

const styles = css`&& {
        text-align: left;
        font-family: ${_theme.fonts.primary};
        font-style: normal;
        font-weight: 400;
        font-size: 14px;
        line-height: 18px;
        color: ${props => props.color || _theme.palette.primary.default};
        svg {
            top: 0;
        }
    }
`

const DefaultSelect = styled(MuiSelect)`${styles}`;

class Select extends React.Component {

    constructor(props) {
        super(props);
        this.state = { value: '' };
        this.onChangeHandler = this.onChangeHandler.bind(this);
    }

    onChangeHandler(event) {
        this.setState({ value: event.target.value }, () => {
            if (typeof this.props.onChange == 'function') {
                this.props.onChange(event);
            }
        });
    }

    render() {
        return (
            <DefaultSelect value={this.state.value || 0} input={<OutlinedInput />}  {...this.props} onChange={this.onChangeHandler} >{this.props.children}</DefaultSelect>
        );
    }
}

export default Select;
