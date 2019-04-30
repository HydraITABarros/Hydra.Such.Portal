import React from 'react';
import PropTypes from 'prop-types';
import styled, { css, theme } from 'styled-components';
import _theme from '../../themes/default';
import { Text, Button, Icon } from 'components';
import MuiDialog from '@material-ui/core/Dialog';
import MuiDialogTitle from '@material-ui/core/DialogTitle';
import MuiDialogContent from '@material-ui/core/DialogContent';
import MuiDialogActions from '@material-ui/core/DialogActions';

const ActionDiv = styled.div`
    display: inline-block;
`;

const closeIcon = css`
    position: absolute;
    top: 8px;
    right: 8px;
`
const CloseIcon = styled(Button)`${closeIcon}`;

const dialog = css`
    [class*="MuiPaper-root"] {
        position: relative;
    }
    hr {
        margin: 0;
        border: 0;
        height: 0;
        border-top: 1px solid  ${_theme.palette.primary.keylines};
    }
`
const Dialog = styled(MuiDialog)`${dialog}`;



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
                <CloseIcon iconSolo><Icon decline/></CloseIcon>
                    {this.props.children}
                </Dialog>
            </div>

        );
    }
}

const styles = css`&& {
    h2 {
        margin: 0;        
    }
    
    &[class*="MuiPaper-rounded"]{
        border-radius: 0;
        }
    
    &[class*="MuiDialogTitle-root"]{
        padding: 48px 40px 16px 40px;
        margin: 0;
        overflow: auto;
        }   
        
    &[class*="Modal__DialogContent"]{
        height: 344px;
        padding: 8px 40px 0 40px;
        margin: 0;
        }

    &[class*="MuiDialogActions-root"]{
        padding: 16px 40px 40px 40px;
        margin: 0;
        }
}
`
const dialogTitle = css`&& {
    [class*="icon-"] {
        padding: 0;
    }
}
`

const DialogTitle = styled(MuiDialogTitle)`${dialogTitle}${styles}`;
const DialogContent = styled(MuiDialogContent)`${styles}`;
const DialogActions = styled(MuiDialogActions)`${styles}`;


export {
    Modal,
    DialogTitle,
    DialogContent,
    DialogActions
};