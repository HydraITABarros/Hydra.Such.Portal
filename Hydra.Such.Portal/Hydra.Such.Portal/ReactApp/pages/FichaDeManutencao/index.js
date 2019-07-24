import React, { Component } from 'react';
import axios from 'axios';
import { PageTemplate } from 'components';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { Wrapper, OmDatePicker, Tooltip } from 'components';
import moment from 'moment';
import ReactDOM from 'react-dom';
import { withRouter } from 'react-router-dom';
import queryString from 'query-string';
import Breadcrumb from './breadcrumb';
import Header from './header';
import { createMuiTheme } from '@material-ui/core/styles';
import { TestScheduler } from 'rx';

axios.defaults.headers.post['Accept'] = 'application/json';
axios.defaults.headers.get['Accept'] = 'application/json';

const muiTheme = createMuiTheme();
const breakpoints = muiTheme.breakpoints.values;

class FichaDeManutencao extends Component {
	state = {
		isLoading: true,
		tooltipReady: false,
		orderId: null,
		categoryId: null,
		equipmentsIds: null,
		institution: null,
		client: null,
		service: null,
		types: null,
		technicals: null,
		equipments: null,
		room: null
	}

	constructor(props) {
		super(props);
		this.state.orderId = this.props.match.params.orderid;
		this.fetch = this.fetch.bind(this);
		var query = queryString.parse(props.location.search);
		this.state.categoryId = query.categoryId;
		this.state.equipmentsIds = query.equipmentsIds;

		this.fetch();
	}

	componentDidMount() {
	}

	fetch() {
		var url = `/ordens-de-manutencao/ficha-de-manutencao`;
		var params = {
			categoryId: this.state.categoryId,
			orderId: this.state.orderId,
			equipmentIds: this.state.equipmentsIds
		};


		axios.get(url, { params }).then((result) => {
			var data = result.data;
			console.log(data);
			if (data.order && data.equipments) {

				var state = {
					client: data.order.clientName,
					institution: data.order.institutionName,
					title: data.equipments.length > 0 ? data.equipments[0].categoriaText : "",
					service: data.equipments.length > 0 ? data.equipments[0].servicoText : "",
					room: data.equipments.length == 1 ? data.equipments[0].sala : "",
					technicals: data.order.technicals,
					equipments: data.equipments,
				}

				this.setState(state);
			}
		}).catch(function (error) {
		}).then(() => {
			this.setState({ isLoading: false });
			setTimeout(() => {
				this.setState({ tooltipReady: true });
				Tooltip.Hidden.hide();
				Tooltip.Hidden.rebuild();
			}, 1200);
		});
	}

	render() {
		return (
			<PageTemplate >
				<Wrapper padding={'0 0 0'} width="100%" minHeight="274px" ref={el => this.highlightWrapper = el}>
					<Breadcrumb orderId={this.state.orderId} client={this.state.client} institution={this.state.institution} />
					<Header
						title={this.state.title}
						room={this.state.room}
						service={this.state.service}
						types={this.state.types}
						technicals={this.state.technicals} />
				</Wrapper>
			</PageTemplate>
		)
	}
}

export default withTheme(withRouter(FichaDeManutencao));