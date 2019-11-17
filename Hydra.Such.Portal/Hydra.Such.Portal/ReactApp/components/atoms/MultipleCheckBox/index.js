import React, {Component} from 'react'
import styled, {css, theme, withTheme} from 'styled-components'
import MuiCheckbox from '@material-ui/core/Checkbox';
import _ from 'lodash';
import {Icon, Button} from 'components';
import './index.scss';


var timeout = 0;

class MultipleCheckBox extends Component {
    state = {
        value: 0
    }

    constructor(props) {
        super(props);
        this.handleClick = this.handleClick.bind(this);

        if (this.props.value) {
            this.state.value = this.props.value * 1;
        }

        if (this.props.$value) {
            this.state.value = this.props.$value.value * 1;
        }
    }

    handleClick() {
        if (this.props.disabled) {
            return;
        }
        var value;
        if (this.state.value == 3) {
            value = 0;
        } else {
            value = this.state.value + 1;
        }
        this.setState({value: value}, () => {
            if (typeof this.props.onChange == 'function') {
                this.props.onChange(value);
            }
            if (this.props.$value) {
                this.props.$value.value = value;
            }
        });
    }

    render() {
        return (
            <div className={"m-checkbox" + (this.props.disabled ? " m-checkbox--disabled" : "")}>
                <Button round className={"m-checkbox__button m-checkbox__button--" + this.state.value}
                        onClick={this.handleClick}>
                    {this.state.value == 0 && <Icon circle-off className="m-checkbox__icon"/>}
                    {this.state.value == 1 && <Icon validation className="m-checkbox__icon"/>}
                    {this.state.value == 2 && <Icon remove className="m-checkbox__icon"/>}
                    {this.state.value == 3 &&
                    <div className="m-checkbox__icon-grouped"><Icon circle-off className="m-checkbox__icon"/><Icon
                        decline className="m-checkbox__icon"/></div>}
                </Button>
            </div>
        )
    }
}

export default MultipleCheckBox;