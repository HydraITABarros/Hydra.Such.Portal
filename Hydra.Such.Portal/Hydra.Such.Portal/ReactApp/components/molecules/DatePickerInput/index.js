import React from 'react'
import _ from 'lodash';
import PropTypes from 'prop-types';
import styled, { css, theme, injectGlobal } from 'styled-components'
import Color from 'color'
import DayPicker, { DateUtils } from 'react-day-picker';
// import DayPickerInput from 'react-day-picker/DayPickerInput';
import SwipeableDrawer from '@material-ui/core/SwipeableDrawer';
import Drawer from '@material-ui/core/Drawer';
import Grid from '@material-ui/core/Grid';
import IconButton from '@material-ui/core/IconButton';
import Divider from '@material-ui/core/Divider';

import { withStyles } from '@material-ui/core/styles';
import { OutlinedInput, Wrapper, DatePicker, Text, Spacer, MenuItem, Select, Icon, Button } from 'components';

import Paper from '@material-ui/core/Paper';

injectGlobal`
    [class*="MuiModal-root"] {
        &[class*="MuiDrawer-root"] {
            &[class*="MuiDrawer-modal"] {
                [class*="MuiPaper-root"] {
                    transform: translate(0px, 0px) !important;
                    opacity: 0;
                }
            }
        }
    }
    body {
        &[style*="overflow: hidden;"] {
            [class*="MuiModal-root"] {
                &[class*="MuiDrawer-root"] {
                    &[class*="MuiDrawer-modal"] {
                        & [class*="MuiPaper-root"] {
                            transform: translate(0px, 0px) !important;
                            opacity: 1;
                        }
                    }
                }
            }
        }
    }
`;

const closeIcon = css`
    position: absolute;
    top: 8px;
    right: 8px;
`;

const CloseIcon = styled(Button)`${closeIcon}`;


const StyledCloseButton = styled(IconButton)`&& {
    position: absolute;
    top:10px;
    right:10px;
}`;

class DatePickerInput extends React.Component {
    state = {
        numberOfMonths: 3,
        open: false,
        staticRange: null,
        selected: null,
        from: null,
        to: null,
    }

    constructor(props) {
        super(props);
        this.handleDayClick = this.handleDayClick.bind(this);
        this.handleResetClick = this.handleResetClick.bind(this);
        this.handleStaticRange = this.handleStaticRange.bind(this);

    }
    handleDayClick(day) {
        // const range = DateUtils.addDayToRange(day, this.state);
        // this.setState(range);
        // console.log('imp', range);
        //this.setState(range, () => _.invoke(this.props, 'onChange', this.state));
    }
    handleResetClick() {
        this.setState(this.getInitialState());
    }
    handleStaticRange(e) {
        var selected = e.target.value.split("_");
        var value = this.props.staticRanges[selected[0]].ranges[selected[1]];

        this.setState({
            from: value.from,
            to: value.to,
            selected: e.target.value
        });
    }

    toggleDrawer = (open) => () => {
        this.setState({
            open
        });
    };

    render() {
        const { classes, theme } = this.props;
        const { from, to } = this.state;
        const modifiers = { start: from, end: to };
        const hasSelect = this.state.hasSelect;
        let select;
        if (hasSelect || true) {
            select = <Wrapper maxWidth="265" width="100%" padding="32px" inline textAlign="left">
                <Wrapper textAlign="left" padding="5px 16px 5px 0">
                    <Text b>Seleccionar datas...</Text>
                </Wrapper>
                <Select value={this.state.selected || ''} onChange={this.handleStaticRange} >
                    {this.props.staticRanges.map((group, k) =>
                        [(k != 0 ? <Divider component="li" /> : ""), (group.label && group.label != "" ? <MenuItem group="true" ><Text p>{group.label}</Text></MenuItem> : ""), group.ranges.map((item, key) =>
                            <MenuItem value={k + "_" + key} name={item}> {item.label}</MenuItem>
                        )]
                    )}
                </Select>
            </Wrapper>
        }
        return (
            <div>
                <Drawer
                    variant="persistent"
                    ModalProps={{ disableBackdropClick: true }}
                    anchor="top"
                    open={this.state.open}
                    onClose={this.toggleDrawer(false)}
                    onOpen={this.toggleDrawer(true)}
                >
                    <div tabIndex={0} role="button" onKeyDown={this.toggleDrawer(false)} >
                        <Wrapper padding="22px" >
                            <CloseIcon iconSolo onClick={this.toggleDrawer(false)} ><Icon decline /></CloseIcon>
                            {select}
                            <Wrapper maxWidth="950" padding="0" inline>
                                <DatePicker {...this.props} onChange={this.handleDayClick} from={from} to={to} month={from} />
                            </Wrapper>
                        </Wrapper>
                    </div>
                </Drawer>
            </div>
        )
    }
}

export default DatePickerInput;
