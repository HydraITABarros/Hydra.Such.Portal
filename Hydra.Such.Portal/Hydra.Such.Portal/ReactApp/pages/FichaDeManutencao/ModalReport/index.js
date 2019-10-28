import React, { Component } from 'react';
import MuiTabs from '@material-ui/core/Tabs';
import MuiTab from '@material-ui/core/Tab';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { Button, Text, Icon, Circle, Wrapper, OmDatePicker, CheckBox, Input, Avatars, ModalLarge, Tooltip } from 'components';
import AppBar from '@material-ui/core/AppBar';


const { DialogTitle, DialogContent, DialogActions } = ModalLarge;

const Tabs = styled(MuiTabs)`
    [class*="MuiTabs-scroller"]>span{
      background-color: ${props => props.theme.palette.secondary.default};
      height: 5px;
      border-radius: 2.5px;
      z-index: 2;
    }
    [class*="MuiTabs-fixed"]>span{
      margin-left: 0px;
    }
    [class*="icon"] {
            color: ${props => props.theme.palette.primary.medium};
          }
    [aria-selected="true"]  {
          [class*="icon"] {
            color: ${props => props.theme.palette.secondary.default};
          }
    }
`;
const Tab = styled(MuiTab)`&&{
      text-transform: capitalize;
      text-align: left;
      min-width: 0;
    }
    [class*="MuiTab-labelContainer"] {
          padding: 6px 12px;
    }
`;
const Bar = styled(AppBar)`&&{
      background-color: ${props => props.theme.palette.white};
      box-shadow: none;
      margin-bottom: 0px;
      padding-left: 0;
      padding-right: 0;
      hr{position: relative; margin-top: -3px; margin-left: -40px; z-index: 1;}
    }
`;

class ModalReport extends Component {
	state = {
		open: false,
	}
	constructor(props) {
		super(props);
		this.handleChange = this.handleChange.bind(this);
	}
	handleChange(e, value) {
		this.setState({

		});
	}

	componentWillReceiveProps(nextProps) {
		if (nextProps.open !== this.state.open) {
			this.setState({ open: nextProps.open });
		}
	}

	render() {
		return (
			<ModalLarge
				onOpen={() => {
					if (this.props.children.props.disabled) {
						return this.setState({ open: false });
					}
					this.setState({ open: true });
				}}
				onClose={() => {
					this.setState({ open: false });
				}}
				action={this.props.children} children={
					<div>
						<DialogTitle>
							<Text h2><Icon report /> Relatório de Manutenção</Text>
						</DialogTitle>
						<hr />

						<DialogContent>

						</DialogContent>
						<hr />
						<DialogActions>
							<Button onClick={() => this.setState({ open: false })} primary color="primary">Guardar</Button>
						</DialogActions>
					</div>
				} open={this.state.open} />
		)
	}
}

export default ModalReport;