import React from 'react'
import _ from 'lodash';
import styled, { css, theme } from 'styled-components'
import Color from 'color'
import { Button, Icon } from 'components'
import DayPicker, { DateUtils } from 'react-day-picker';
import 'react-day-picker/lib/style.css';

var rangeColor = "#FFE8D9";
var selectedColor = "#F9703E";
var textColor = "#333333";

const styles = css`&& {
    font-family: 'Inter var', system-ui, sans-serif;    
    color: textColor; 
    .DayPicker-Day:focus, .DayPicker-Months:focus, &:focus, .DayPicker-wrapper:focus {
        outline: none;
    }
    .DayPicker-Caption {
        text-align: center;
        margin-bottom: 1.2em;
        div {
            font-weight: 100;
            font-size: 18px;
        }
    }
    .DayPicker-Month {
        margin-right: 0;
        margin-left: 0;
        margin-right: 1em;
        margin-left: 1em;
    }
    .DayPicker-Months {
        width: 100%;
    }
    .DayPicker-NavBar {
        position: absolute;
        padding: 10px;
        top: 0;
        left: 0;
        right: 0;
    }

    && .DayPicker-Day {
        font-weight: 100;
        font-size: 14px;
        padding: 0.35em 0.8em;
        position: relative;
        &:not(.DayPicker-Day--disabled):not(.DayPicker-Day--selected):not(.DayPicker-Day--outside) {
            &:hover {
                z-index:1;
                    background: ${Color(rangeColor).lighten(0.05).hex()};
            }
        }
        &--today {
            font-weight: 600;
            color: ${Color(textColor).hex()}
        }
    }
    
    .DayPicker-NavButton--prev {
        left: 1.3em;
        top: .9em;
        right: auto;
    }
    .DayPicker-NavButton--next {
        right: 1.3em;
        top: .9em;
    }
    .DayPicker-Weekday {
        font-weight: 700;
        color: #333;
        font-size: 14px;
    }
    
    && .DayPicker-Day {
        border-radius: 0;
        &--selected {
            &:not(.DayPicker-Day--start):not(.DayPicker-Day--end) {
                &:hover {
                        background: ${Color(rangeColor).lighten(0.04).hex()};
                }
            }
        }
    }
    .DayPicker-Day--selected:not(.DayPicker-Day--outside) {
        background-color: transparent;
        color: ${textColor};
    }
    &.DayPicker--range {
        .DayPicker-Day--selected:not(.DayPicker-Day--outside) {
            background-color: ${rangeColor};
        }
        && .DayPicker-Day--start  {
            border-top-left-radius: 50%;
            border-bottom-left-radius: 50%;
            &:not(.DayPicker-Day--outside) {
                background: linear-gradient(to left, ${rangeColor} 50%, transparent 50%);
            }
        }
        && .DayPicker-Day--end  {
            border-top-right-radius: 50%;
            border-bottom-right-radius: 50%;
            &:not(.DayPicker-Day--outside) {
                background: linear-gradient(to left,  transparent 50%, ${rangeColor} 50%);
                }
        }
    }
    
    && .DayPicker-Day--start, && .DayPicker-Day--end  {
        z-index: 2;
    }

    .DayPicker-Day:not(.DayPicker-Day--start):not(.DayPicker-Day--end):not(:hover) {
        border-radius: 0;
    }
    && .DayPicker-Day--start,&& .DayPicker-Day--end {
        color: white;
        background: transparent;
    }
    
    .DayPicker-Day--start,  .DayPicker-Day--end  {
        &:not(.DayPicker-Day--outside) {
            border-radius: 0;
            z-index: 1;
            background-color: ${rangeColor};
            &:after {
                content: "";
                display: inline-block;
                background: ${selectedColor};
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                bottom:0;
                z-index: -1;
                border-radius: 50%;
                width: 30px;
                height: 30px;
                margin: auto;
            }
        }
    }
}
`
const CustomDayPicker = styled(DayPicker)`${styles}`

const locale = "pt";
const MONTHS = { pt: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'] };
const MONTHS_SHORT = { pt: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'] };
const WEEKDAYS_LONG = { pt: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'] };
const WEEKDAYS_SHORT = { pt: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'] };
const langProps = { locale, months: MONTHS_SHORT[locale], weekdaysLong: WEEKDAYS_LONG[locale], weekdaysShort: WEEKDAYS_SHORT[locale] }

function Navbar({
    nextMonth,
    previousMonth,
    onPreviousClick,
    onNextClick,
    className,
    localeUtils,
}) {
    const months = localeUtils.getMonths();
    const prev = months[previousMonth.getMonth()];
    const next = months[nextMonth.getMonth()];
    const styleLeft = {
        float: 'left',
    };
    const styleRight = {
        float: 'right',
    };

    const LeftButton = styled(Button)`&& {
            float: left;
            font-size: 24px;
        }
    `;

    const RightButton = styled(Button)`&& {
            float: right;
            font-size: 24px;
        }
    `;

    return (
        <div className={className}>
            <LeftButton iconSolo onClick={() => onPreviousClick()}> <Icon arrow-left /></LeftButton>
            <RightButton iconSolo onClick={() => onNextClick()} > <Icon arrow-right /></RightButton>
        </div>
    );
}

class DatePicker extends React.Component {
    static defaultProps = {
        numberOfMonths: 3
    }

    constructor(props) {
        super(props);
        this.handleDayClick = this.handleDayClick.bind(this);
        this.handleResetClick = this.handleResetClick.bind(this);
        this.state = this.getInitialState();
        this.state.from = props.from;
        this.state.to = props.to;
    }

    componentWillReceiveProps(props) {
        this.setState({ to: props.to, from: props.from });
    }

    getInitialState() {
        return {
            from: undefined,
            to: undefined,
        };
    }
    handleDayClick(day) {
        const range = DateUtils.addDayToRange(day, this.state);
        this.setState(range, () => _.invoke(this.props, 'onChange', this.state));
    }
    handleResetClick() {
        this.setState(this.getInitialState());
    }
    render() {
        return (
            <div>
                <CustomDayPicker
                    {...langProps}
                    {...this.props}
                    className={this.state.from && this.state.to && this.state.from.toString() != this.state.to.toString() ? 'DayPicker--range' : ''}
                    numberOfMonths={this.props.numberOfMonths}
                    modifiers={{ start: this.state.from, end: this.state.to }}
                    onDayClick={this.handleDayClick}
                    selectedDays={[this.state.from, { from: this.state.from, to: this.state.to }]}
                    navbarElement={<Navbar />}
                />
            </div>
        )
    }
}

export default DatePicker;
