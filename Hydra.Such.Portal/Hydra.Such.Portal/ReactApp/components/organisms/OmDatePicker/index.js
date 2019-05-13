import React from 'react'
import _ from 'lodash';
import styled, { css, theme, injectGlobal } from 'styled-components'
import MuiDrawer from '@material-ui/core/Drawer';
import Divider from '@material-ui/core/Divider';
import { OutlinedInput, Wrapper, DatePicker, Text, Spacer, MenuItem, Select, Icon, Button } from 'components';
import _theme from '../../themes/default';
import toRenderProps from 'recompose/toRenderProps';
import windowSize from 'react-window-size';
import onResize from 'any-resize-event';
import Hidden from '@material-ui/core/Hidden';
import { createMuiTheme } from '@material-ui/core/styles';
import moment from 'moment';

const muiTheme = createMuiTheme();
const breakpoints = muiTheme.breakpoints.values;

const Drawer = styled(MuiDrawer)`
    [class*="MuiPaper-root"] {
        transition: opacity 225ms cubic-bezier(0, 0, 0.2, 1) !important;
        position: absolute;
        transform: translate(0px, 0px) !important;
        opacity: 0; 
        display: none;
        z-index:1;
        max-height: none;
            z-index: 2;
        @media (max-width: ${breakpoints["md"] + "px"}) {
            z-index: 20;
        }
        @media (max-width: ${breakpoints["sm"] + "px"}) {
            position: fixed;
            bottom: 0;
            padding-top: 55px;
        }
    }
    &.open {
        [class*="MuiPaper-root"] {
            transform: translate(0px, 0px) !important;
            opacity: 1; 
            display: block;
            /* z-index: 2; */
        }
        &:before {
            content: "";
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: red;
            z-index:-1;
        }
    }
`

const closeButton = css`&& {
        position: absolute;
        top: 8px;
        right: 8px;
    }
`;

const CloseIcon = styled(Button)`${closeButton}`;


class DatePickerInput extends React.Component {
    state = {
        numberOfMonths: 3,
        open: false,
        staticRange: null,
        selected: null,
        from: null,
        to: null,
        width: -1,
        staticRanges: [
            {
                label: "", ranges: [
                    { from: moment().startOf('month'), to: moment().endOf('month'), label: "Mês atual" },
                    { from: moment().startOf('quarter'), to: moment().endOf('quarter'), label: "Trimestre atual" },
                    { from: moment().startOf('year'), to: moment().endOf('year'), label: "Ano atual" }
                ]
            },
            {
                label: "Ultimos meses", ranges: [
                    { from: moment().subtract(1, 'month').startOf('month'), to: moment().subtract(1, 'month').endOf('month'), label: moment().subtract(1, 'month').format("MMMM YYYY") },
                    { from: moment().subtract(2, 'month').startOf('month'), to: moment().subtract(2, 'month').endOf('month'), label: moment().subtract(2, 'month').format("MMMM YYYY") },
                    { from: moment().subtract(3, 'month').startOf('month'), to: moment().subtract(3, 'month').endOf('month'), label: moment().subtract(3, 'month').format("MMMM YYYY") }
                ]
            }]
    }

    constructor(props) {
        super(props);
        this.updateDimensions = this.updateDimensions.bind(this);
        this.handleStaticRange = this.handleStaticRange.bind(this);
        this.handleDayClick = this.handleDayClick.bind(this);
        this.wrapperElement = React.createRef();
        this.setValue = this.setValue.bind(this);

        this.state.from = this.props.from;
        this.state.to = this.props.to;
    }

    value() {
        return {
            from: this.state.from,
            to: this.state.to
        }
    }

    componentDidMount() {
        this.wrapperElement.current.addEventListener("onresize", this.updateDimensions);
    }

    componentWillReceiveProps(props) {
        this.setState({ open: props.open });
    }

    updateDimensions() {
        // if react element-> ReactDOM.findDOMNode(this.wrapperElement);
        // if html element-> this.wrapperElement.current;
        var wrapperElement = this.wrapperElement.current;
        this.setState({ width: wrapperElement.offsetWidth });
    }

    handleResetClick() {
        this.setState(this.getInitialState());
    }

    handleStaticRange(e) {
        var selected = e.target.value.split("_");
        var value = this.state.staticRanges[selected[0]].ranges[selected[1]];
        this.setValue({
            from: value.from,
            to: value.to,
            selected: e.target.value
        });
    }
    handleDayClick(e) {
        this.setValue({
            from: moment(e.from),
            to: moment(e.to),
            selected: ""
        });
    }

    setValue({ from, to, selected }) {
        this.setState({ from, to, selected }, this.props.onChange);
    }

    toggleDrawer = (open) => () => {
        this.setState({ open });
    };

    render() {
        const { classes, theme } = this.props;
        const { from, to } = this.state;
        const modifiers = { start: from, end: to };
        const hasSelect = this.state.hasSelect;
        let select;
        if (hasSelect || true) {
            select =
                <Wrapper maxWidth="329" width="100%" padding="32px" inline textAlign="left">
                    <Wrapper textAlign="left" padding="5px 16px 5px 0"><Text b>Seleccionar datas...</Text></Wrapper>
                    <Select value={this.state.selected || ''} onChange={this.handleStaticRange} >
                        {this.state.staticRanges.map((group, k) =>
                            [
                                (k != 0 ? /*<Divider component="li" />*/"" : ""),
                                (group.label && group.label != "" ? <MenuItem group="true" >{group.label}</MenuItem> : ""),
                                group.ranges.map((item, key) => <MenuItem value={k + "_" + key} name={item}> {item.label}</MenuItem>)
                            ]
                        )}
                    </Select>
                </Wrapper>
        }

        return (

            <Drawer variant="persistent" margintop={this.props.margintop} ModalProps={{ disableBackdropClick: true }} anchor="top" open={this.state.open} className={this.state.open ? 'open' : ''} >
                <div ref={this.wrapperElement} tabIndex={0} role="button" onKeyDown={this.props.onCloseClick || null} >
                    <Spacer height={"35px"} />
                    <Wrapper padding="22px" textAlign="center" mdPadding="0" >
                        <CloseIcon iconSolo onClick={this.props.onCloseClick || null} ><Icon decline /></CloseIcon>
                        {select}
                        <Wrapper maxWidth="950" padding="0" inline>
                            <DatePicker {...this.props} numberOfMonths={this.state.width < 1330 ? this.state.width < 1022 ? 1 : 2 : 3} onChange={this.handleDayClick} from={from.toDate()} to={to.toDate()} month={from.toDate()} />
                        </Wrapper>
                    </Wrapper>
                    <Hidden mdUp><Spacer height={"35px"} /></Hidden>
                </div>
            </Drawer>
        )
    }
}

const Backdrop = styled.div`
    transition: opacity 225ms cubic-bezier(0, 0, 0.2, 1) !important;
    position: fixed;
    left:0;
    right:0;
    top:0;
    bottom:0;
    opacity:0;
    display: none;
    background-color: ${_theme.palette.primary.dark};
    z-index: 1;
    &.open {
        opacity:0.4;
        display: block;
    }
`;

DatePickerInput.backdrop = ({ ...props }) => {
    return (
        <Backdrop className={props.open ? 'open' : ''} {...props} />
    )
}

export default DatePickerInput;
