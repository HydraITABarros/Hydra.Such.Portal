import React from 'react'
import _ from 'lodash';
import PropTypes from 'prop-types';
import styled, { css, theme } from 'styled-components'
import Color from 'color'
import DayPicker, { DateUtils } from 'react-day-picker';
// import DayPickerInput from 'react-day-picker/DayPickerInput';
import SwipeableDrawer from '@material-ui/core/SwipeableDrawer';
import Button from '@material-ui/core/Button';
import Grid from '@material-ui/core/Grid';
import IconButton from '@material-ui/core/IconButton';
import CloseIcon from '@material-ui/icons/Close';
import Divider from '@material-ui/core/Divider';

import { withStyles } from '@material-ui/core/styles';
import { OutlinedInput, Wrapper, DatePicker, Text, Spacer, MenuItem } from 'components';
import Select from 'components/atoms/Select';

import Paper from '@material-ui/core/Paper';

var rangeColor = "#FFE8D9";
var selectedColor = "#F9703E";
var textColor = "#333333";

// const Wrapper = styled.div`
//     padding: 25px;
// `
const locale = "pt";
const MONTHS = { pt: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'] };
const WEEKDAYS_LONG = { pt: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'] };
const WEEKDAYS_SHORT = { pt: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'] };
const langProps = { locale, months: MONTHS[locale], weekdaysLong: WEEKDAYS_LONG[locale], weekdaysShort: WEEKDAYS_SHORT[locale] }

const StyledCloseButton = styled(IconButton)`&& {
    position: absolute;
    top:10px;
    right:10px;
}`;

class DatePickerInput extends React.Component {
    static defaultProps = {
        numberOfMonths: 3,
        open: false,
    }

    constructor(props) {
        super(props);
        this.handleDayClick = this.handleDayClick.bind(this);
        this.handleResetClick = this.handleResetClick.bind(this);
        this.state = this.getInitialState();

    }
    getInitialState() {
        return {
            from: undefined,
            to: undefined,
            open: false,
            hasSelect: false
        };
    }
    handleDayClick(day) {
        const range = DateUtils.addDayToRange(day, this.state);
        this.setState(range, () => _.invoke(this.props, 'onChange', this.state));
    }
    handleResetClick() {
        this.setState(this.getInitialState());
    }
    toggleDrawer = (open) => () => {
        this.setState({
            open
        });
    };

    render() {
        const { from, to } = this.state;
        const modifiers = { start: from, end: to };
        const hasSelect = this.state.hasSelect;
        let select;
        if (hasSelect || true) {
            select = <Wrapper maxWidth="265" width="100%" padding="32px" inline textAlign="left">
                <Wrapper textAlign="left" padding="5px 16px 5px 0">
                    <Text b>Seleccionar datas...</Text>
                </Wrapper>
                <Select >
                    {this.props.staticRanges.map((group, k) =>
                        [(k != 0 ? <Divider component="li" /> : ""), (group.label && group.label != "" ? <MenuItem group ><Text p>{group.label}</Text></MenuItem> : ""), group.ranges.map((item, key) =>
                            <MenuItem value={key + "" + k} > {item.label}</MenuItem>
                        )]
                    )
                    }
                </Select>
            </Wrapper>
        }
        return (
            <div>

                <Button onClick={this.toggleDrawer(true)}>Open Top</Button>

                <SwipeableDrawer ModalProps={{ disableBackdropClick: true }} anchor="top" open={this.state.open} onClose={this.toggleDrawer(false)} onOpen={this.toggleDrawer(true)} >
                    <div tabIndex={0} role="button" onKeyDown={this.toggleDrawer(false)} >
                        <Wrapper padding="22px" >

                            <StyledCloseButton aria-label="Delete" onClick={this.toggleDrawer(false)} > <CloseIcon fontSize="small" /> </StyledCloseButton>
                            {select}
                            <Wrapper maxWidth="950" padding="0" inline>
                                <DatePicker {...langProps} {...this.props} className={from && to && from.toString() != to.toString() ? 'DayPicker--range' : ''}
                                    numberOfMonths={this.props.numberOfMonths} modifiers={modifiers} onDayClick={this.handleDayClick} selectedDays={[from, { from, to }]} />
                            </Wrapper>
                        </Wrapper>
                    </div>
                </SwipeableDrawer>
            </div>
        )
    }
}

export default DatePickerInput;
