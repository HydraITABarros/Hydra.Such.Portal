import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import { Text, Icon, Circle, Wrapper, Spacer, Button, Avatars } from 'components';
import Functions from '../../helpers/functions';
import MuiGrid from '@material-ui/core/Grid';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { createMuiTheme } from '@material-ui/core/styles';
import Hidden from '@material-ui/core/Hidden';
import { withRouter } from 'react-router-dom';
const muiTheme = createMuiTheme();
const breakpoints = muiTheme.breakpoints.values;
injectGlobal`
	
`
const CustomWrapper = styled(Wrapper)` && {
	border-bottom: solid 1px ${props => props.theme.palette.primary.keylines};
}`


class Header extends Component {
	state = {
		isLoading: true,

		orderId: null,
		technicals: null,
		service: null,
		room: null,
		title: null,
		equipments: null
	}

	constructor(props) {
		super(props);
		this.state = { ...props };
	}

	componentDidUpdate(props) {
		var newState = {};
		if (props.isLoading !== this.state.isLoading) {
			newState.isLoading = props.isLoading;
		}
		if (props.orderId !== this.state.orderId) {
			newState.orderId = this.state.orderId;
		}
		if (props.technicals !== this.state.technicals) {
			newState.technicals = this.state.technicals;
		}
		if (props.service !== this.state.service) {
			newState.service = this.state.service;
		}
		if (props.room !== this.state.room) {
			newState.room = this.state.room;
		}
		if (props.title !== this.state.title) {
			newState.title = this.state.title;
		}
		if (props.equipments !== this.state.equipments) {
			newState.equipments = this.state.equipments;
		}
		if (Object.keys(newState) > 0) {
			this.setState({ ...newState });
		}
	}

	render() {
		return (
			<CustomWrapper width="100%" >
				<MuiGrid container direction="row" justify="space-between" alignitems="middle" spacing={0} maxwidth={'100%'} margin={0} style={{ background: this.props.theme.palette.bg.grey }}>

				</MuiGrid>
			</CustomWrapper>
		)
	}
};

export default withRouter(withTheme(Header));
