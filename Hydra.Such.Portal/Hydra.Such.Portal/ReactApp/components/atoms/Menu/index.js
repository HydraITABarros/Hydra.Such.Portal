import React, { Component } from 'react'
import ReactDOM from 'react-dom'
import styled, { css, theme, withTheme } from 'styled-components'
import MuiCheckbox from '@material-ui/core/Checkbox';
import _ from 'lodash';
import { Icon, Button, MenuItem } from 'components';
import Menu from '@material-ui/core/Menu';
import MenuList from '@material-ui/core/MenuList';
import Popper from '@material-ui/core/Popper';
import ClickAwayListener from '@material-ui/core/ClickAwayListener';
import Grow from '@material-ui/core/Grow';
import Fade from '@material-ui/core/Fade';
import Paper from '@material-ui/core/Paper';
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
		event.target.blur();
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
				<Popper
					id="simple-menu"
					anchorEl={anchorEl}
					open={Boolean(anchorEl)}
					onClose={this.handleClose}
					ref={(el) => this.menuEl = el}
					transition
				//placement="bottom-start"
				>
					{({ TransitionProps, placement }) => (
						<Grow
							{...TransitionProps}
							id="menu-list-grow"
							style={{
								transformOrigin: placement === 'bottom' ? 'center top' : 'center bottom'
							}}
							timeout={{
								enter: 400, exit: 180
							}}
						>
							<Paper elevation={8}
								style={{ position: 'relative', top: '-35px' }}
							>
								<ClickAwayListener onClickAway={this.handleClose}>
									<MenuList
									>
										{this.props.children != null && (
											this.props.children.length > 1 ?
												this.props.children.map((item, index) => {
													return (React.cloneElement(item, {
														key: index,
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
									</MenuList>
								</ClickAwayListener>
							</Paper>
						</Grow>
					)}
				</Popper>
			</div>
		)
	}
}

export default MenuSelect;