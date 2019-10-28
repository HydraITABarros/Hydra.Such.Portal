import React, { Component } from 'react'
import styled, { css, theme, withTheme } from 'styled-components'
import MuiCheckbox from '@material-ui/core/Checkbox';
import _ from 'lodash';
import { Icon, Button, MenuItem } from 'components';
import Menu from '@material-ui/core/Menu';
import './index.scss';


var timeout = 0;
class MenuSelect extends Component {
	state = {
		anchorEl: null,
	};
	constructor(props) {
		super(props);
		this.handleClick = this.handleClick.bind(this);
	}

	handleClick = event => {
		this.setState({ anchorEl: event.currentTarget });
	};

	handleClose = () => {
		this.setState({ anchorEl: null });
	};

	render() {
		const { anchorEl } = this.state;

		return (
			<div style={this.props.containerStyle}>
				<span onClick={this.handleClick} >
					{this.props.action}
				</span>
				<Menu
					id="simple-menu"
					anchorEl={anchorEl}
					open={Boolean(anchorEl)}
					onClose={this.handleClose}
				>
					{this.props.children != null && (
						this.props.children.count > 1 ?
							this.props.children.map((item) => {
								return (React.cloneElement(item, {
									onClick: () => {
										item.props.onClick ? item.props.onClick() : null;
										this.handleClose();
									}
								}));
							})
							:
							React.cloneElement(this.props.children, {
								onClick: () => {

									this.props.children.props.onClick ? this.props.children.props.onClick() : null;
									this.handleClose();
								}
							})
					)}
				</Menu>
			</div>
		)
	}
}

export default MenuSelect;