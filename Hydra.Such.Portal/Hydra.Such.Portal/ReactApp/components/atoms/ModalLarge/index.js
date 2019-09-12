import React from 'react';
import PropTypes from 'prop-types';
import styled, { css, theme } from 'styled-components';
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
    [class*="MuiPaper-root"] {
        position: relative;
    }
    hr {
        margin: 0;
        border: 0;
        height: 0;
        border-top: 1px solid  ${props => props.theme.palette.primary.keylines};
    }
`
const Dialog = styled(MuiDialog)`${dialog}`;

const Action = ({ ...props }) => {
	return <ActionDiv {...props}>{props.children}</ActionDiv>
}

class ModalLarge extends React.Component {
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
		this.setState({ open: false }, () => {
			this.props.onClose ? this.props.onClose() : '';
		});
	};

	componentWillReceiveProps(props) {
		if (this.state.open != props.open) {
			this.setState({ open: props.open });
		}
	}

	render() {
		return (
			<span>
				<Action onClick={this.handleClickOpen} >
					{this.props.action}
				</Action>
				<Dialog onClose={this.handleClose}
					aria-labelledby="customized-dialog-title"
					open={this.props.open || this.state.open}
					fullWidth={true}
					maxWidth={'lg'}>
					<CloseIcon iconSolo onClick={this.handleClose}><Icon decline /></CloseIcon>
					{this.props.children}
				</Dialog>
			</span>

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
        padding: 16px 35px 40px 35px;
        margin: 0;
    }
}
`
const DialogTitle = styled(MuiDialogContent)`&&&& {
        position:relative;
        padding: 52px 0px 10px 40px;
        margin: 0;
        overflow: hidden;
    }   
    [class*="icon-"] {
        padding: 0px 8px 0px 0px;
        vertical-align: middle;
    }
`;
const DialogContent = styled(MuiDialogContent)`&& {
        height: 440px;
        padding: 32px 40px 0 40px;
        margin: 0;
    }`;
const DialogActions = styled(MuiDialogActions)`${styles}`;

ModalLarge.DialogTitle = DialogTitle;
ModalLarge.DialogContent = DialogContent;
ModalLarge.DialogActions = DialogActions;

export default ModalLarge;