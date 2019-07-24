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

const Grid = styled(MuiGrid)`&& {
	padding: 20px 25px;
       white-space: nowrap;
       overflow: hidden;
       text-overflow: ellipsis;
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
			newState.orderId = props.orderId;
		}
		if (props.technicals !== this.state.technicals) {
			newState.technicals = props.technicals;
		}
		if (props.service !== this.state.service) {
			newState.service = props.service;
		}
		if (props.room !== this.state.room) {
			newState.room = props.room;
		}
		if (props.title !== this.state.title) {
			newState.title = props.title;
		}
		if (props.equipments !== this.state.equipments) {
			newState.equipments = props.equipments;
		}

		console.log('IMP', Object.keys(newState), newState);

		if (Object.keys(newState).length > 0) {
			this.setState(newState, () => console.log(this.state));
		}
	}

	render() {
		return (
			<CustomWrapper width="100%" >
				<MuiGrid container direction="row" justify="space-between" alignitems="middle" spacing={0} maxwidth={'100%'} margin={0} style={{ background: this.props.theme.palette.bg.grey }}>
					<Grid item xs={12} sm={6} style={{ padding: 0 }}>

						<Text h1>{this.state.title}</Text>
						<MuiGrid container direction="row" justify="space-between" alignitems="middle" spacing={0} maxwidth={'100%'} margin={0} >
							<Grid item xs={6} style={{ padding: 0 }}>
								{this.state.room && <Text span>Piso, Sala</Text>}
							</Grid>
							<Grid item xs={6} style={{ padding: 0 }}>
								{this.state.room && <Text b>{this.state.room}</Text>}
							</Grid>

							<Grid item xs={6} style={{ padding: 0 }}>
								<Text span>Serviço</Text>
							</Grid>
							<Grid item xs={6} style={{ padding: 0 }}>
								<Text b>{this.state.service}</Text>
							</Grid>

							<Grid item xs={6} style={{ padding: 0 }}>
								<Text span>Tipo tarefa</Text>
							</Grid>
							<Grid item xs={6} style={{ padding: 0 }}>

							</Grid>

							<Grid item xs={6} style={{ padding: 0 }}>
								<Text span>Técnicos</Text>
							</Grid>
							<Grid item xs={6} style={{ padding: 0 }}>

							</Grid>

						</MuiGrid>

					</Grid>
				</MuiGrid>
			</CustomWrapper>
		)
	}
};

export default withRouter(withTheme(Header));
