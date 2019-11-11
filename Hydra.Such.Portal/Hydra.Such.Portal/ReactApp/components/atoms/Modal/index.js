import React from 'react';
import PropTypes from 'prop-types';
import styled, {css, theme} from 'styled-components';
// import { Text, Button, Icon } from 'components';
import Text from '../Text';
import Button from '../Button';
import Icon from '../Icon';
import MuiDialog from '@material-ui/core/Dialog';
import MuiDialogTitle from '@material-ui/core/DialogTitle';
import MuiDialogContent from '@material-ui/core/DialogContent';
import MuiDialogActions from '@material-ui/core/DialogActions';

const ActionDiv = styled.div`
    display: inline-block;
`;

const closeIcon = css`&& {
        position: absolute;
        z-index: 10;
        top: 8px;
        right: 8px;
        span[class*="icon"] {
            font-size: 14px;
        }
    }
`

const CloseIcon = styled(Button)`${closeIcon}`;

const dialog = css`
	display: inline-block;
	[class*="MuiPaper-root"] {
		position: relative;
			width: 100%;
	}
	hr {
		margin: 0;
		border: 0;
		height: 0;
		border-top: 1px solid  ${props => props.theme.palette.primary.keylines};
	}
`
const Dialog = styled(MuiDialog)`${dialog}`;

const Action = ({...props}) => {
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

        this.setState({
            open: true,
        }, () => {
            this.props.onOpen ? this.props.onOpen() : '';
        });
    };

    handleClose = () => {
        this.setState({open: false}, () => {
            this.props.onClose ? this.props.onClose() : '';
        });
    };

    componentWillReceiveProps(nextProps) {
        if (nextProps.open !== this.state.open) {

            this.setState({open: nextProps.open});
        }
    }

    render() {
        var containerStyle = {display: 'inline-block'};
        if (this.props.containerStyle) {
            containerStyle = this.props.containerStyle;
        }

        return (
            <div style={containerStyle}>
                <Action onClick={this.handleClickOpen}>
                    {this.props.action}
                </Action>
                <Dialog onClose={this.handleClose} aria-labelledby="customized-dialog-title"
                        open={this.props.open || this.state.open}>
                    <CloseIcon iconSolo onClick={this.handleClose}><Icon decline/></CloseIcon>
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
    
    &[class*="MuiDialogActions-root"]{
        padding: 20px 35px 35px 35px;
        margin: 0;
    }
}
`
const DialogTitle = styled(MuiDialogContent)`&&&& {
        position:relative;
        padding: 52px 35px 10px 35px;
        margin: 0;
        overflow: hidden;
    }   
    [class*="icon-"] {
        padding: 0;
    }
`;
const DialogContent = styled(MuiDialogContent)`&& {
        height: 100%;
		max-height: 65vh;
        padding: 24px 35px 0 35px;
        margin: 0;
    }`;
const DialogActions = styled(MuiDialogActions)`${styles}`;

Modal.DialogTitle = DialogTitle;
Modal.DialogContent = DialogContent;
Modal.DialogActions = DialogActions;

export default Modal;