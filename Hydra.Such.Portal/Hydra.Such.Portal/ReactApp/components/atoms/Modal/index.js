import React from 'react';
import PropTypes from 'prop-types';
import styled, { css, theme } from 'styled-components';
import _theme from '../../themes/default';
import { Text } from 'components';
import Dialog from '@material-ui/core/Dialog';
import MuiDialogTitle from '@material-ui/core/DialogTitle';
import MuiDialogContent from '@material-ui/core/DialogContent';
import MuiDialogActions from '@material-ui/core/DialogActions';

const ActionDiv = styled.div`
    display: inline-block;
`;

const Action = ({ ...props }) => {
    return <ActionDiv {...props}>{props.children}</ActionDiv>
}

class Modal extends React.Component {
    state = {
        open: false,
    }

    constructor(props) {
        super(props);
        this.handleClickOpen = this.handleClickOpen.bind(this);
    }

    handleClickOpen = () => {
        console.log(23423423);
        this.setState({
            open: true,
        });
    };

    handleClose = () => {
        this.setState({ open: false });
    };

    render() {
        return (
            <div>
                <Action onClick={this.handleClickOpen} >
                    {this.props.action}
                </Action>
                <Dialog
                    onClose={this.handleClose}
                    aria-labelledby="customized-dialog-title"
                    open={this.state.open}
                >
                    {this.props.children}
                </Dialog>
            </div>

        );

    }
}
const DialogTitle = MuiDialogTitle;
const DialogContent = MuiDialogContent;
const DialogActions = MuiDialogActions;

export {
    Modal,
    DialogTitle,
    DialogContent,
    DialogActions
};