import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { PageTemplate } from 'components';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { Wrapper, OmDatePicker, Tooltip, Text, CheckBox, Input, Icon, Button, Select, MenuItem, Modal } from 'components';
import moment from 'moment';
import { withRouter } from 'react-router-dom';
import queryString from 'query-string';
import Breadcrumb from './breadcrumb';
import { createMuiTheme } from '@material-ui/core/styles';
import StickyEl from 'react-sticky-el';
import { Waypoint } from 'react-waypoint';
import MuiGrid from '@material-ui/core/Grid';
import ScrollContainer from 'react-indiana-drag-scroll';
import Header from './header';
import PlanActions from './planActions';
import PlanEquipmentsHeader from './planEquipmentsHeader';
import PlanEquipmentsItem from './planEquipmentsItem';
import FinalState from './finalState';
import PlanMaintenance from './planMaintenance';
import PlanRow from './PlanRow';
import _theme from '../../themes/default';
import Color from 'color';
import _ from 'lodash';
import Functions from '../../helpers/functions';
import { NotificationPhoneBluetoothSpeaker } from 'material-ui/svg-icons';
import { observable } from 'mobx';
import { observer } from 'mobx-react';

const addLinkedPropsToObject = Functions.addLinkedPropsToObject;

const { DialogTitle, DialogContent, DialogActions } = Modal;

axios.defaults.headers.post['Accept'] = 'application/json';
axios.defaults.headers.get['Accept'] = 'application/json';

const muiTheme = createMuiTheme();
const breakpoints = muiTheme.breakpoints.values;

injectGlobal`
	input[type=number]::-webkit-outer-spin-button,
	input[type=number]::-webkit-inner-spin-button {
		-webkit-appearance: none;
		margin: 0;
	}

	input[type=number] {
		-moz-appearance:textfield;
	}
	.scrollable {
		overflow-x: scroll;   
    	}
	/* .scrollable::-webkit-scrollbar {
	} */
	.scroll-container {
	}
	.scroll-shadow {
		/* box-shadow: 'inset -30px 0 28px -30px grey'; */
		&.right {
			box-shadow: inset -30px 0 16px -30px ${_theme.palette.primary.keylines};
		}
		&.left {
			box-shadow: inset 30px 0 16px -30px ${_theme.palette.primary.keylines};
		}
		&.left.right {
			box-shadow: inset 30px 0 16px -30px ${_theme.palette.primary.keylines},  inset -30px 0 16px -30px ${_theme.palette.primary.keylines};
		}
	}
	.padding-r-32 {
		padding-right: 32px;
	}
	.s-22 {
		font-size: 22px;
	}
	.s-20 {
		font-size: 20px;
	}
	.s-18 {
		font-size: 18px;
	}
`
const TooltipHidden = styled(Tooltip.Hidden)`
	z-index: 1030;
`;

const Grid = styled(MuiGrid)`
`;

const HeaderTitle = styled(Header.HeaderTitle)``;

const HeaderDescription = styled(Header.HeaderDescription)``;

const PlanHeader = styled.div`
	display: block;
	white-space: nowrap;
	height: 104px;
	background: ${props => props.theme.palette.bg.white};
`;

const Sticky = styled(StickyEl)`
	&.sticky {
		${HeaderTitle} {
			border-bottom: solid 1px ${props => props.theme.palette.primary.keylines};
		}
		${PlanHeader} {
			border-bottom: solid 1px ${props => props.theme.palette.primary.keylines};
			transform: translateZ(0px) translateY(114px);
		}
	}
`;

let equipmentsHeaderTimer = 0;

const PlanRowText = styled(Text)`
	position: absolute;
	top: 50%;
	transform: translateY(-50%);
	padding-right: 16px;
`;

const Index = styled(Text)`
	font-size: 16px;

`;

const Num = styled(Text)`
	font-size: 13px;
	line-height: 24px;
`;
//@observer
class FichaDeManutencao extends Component {
	state = {
		isLoading: true,
		tooltipReady: false,
		orderId: null,
		order: null,
		categoryId: null,
		equipmentsId: null,
		equipments: [],
		institution: null,
		client: null,
		service: null,
		types: null,
		technicals: null,
		planMaintenance: [],
		planMaintenanceHtml: null,
		planQuality: [],
		planQuantity: [],
		room: null,
		bodyHeight: 0,
		tabsContentHeight: null,
		planWidth: '800px',
		equipmentsHeaderScroll: {
			outerWidth: null,
			innerWidth: null,
			scrollLeft: null
		},
		position: 0,
		marcaIds: null,
		servicoIds: null
	}

	constructor(props) {
		super(props);
		this.state.orderId = this.props.match.params.orderid;
		var query = queryString.parse(props.location.search);
		this.state.equipmentId = query.equipmentId;
		this.state.marcaIds = query.marcaIds;
		this.state.servicoIds = query.servicoIds;

		console.log('imp', query);

		this.fetch = this.fetch.bind(this);

		this.setBodyHeight = this.setBodyHeight.bind(this);
		this.setPlanHeight = this.setPlanHeight.bind(this);
		this.getScrollShadow = this.getScrollShadow.bind(this);
		this.waipointQualitativoHandlerLeave = this.waipointQualitativoHandlerLeave.bind(this);
		this.waipointQuantitativoHandlerLeave = this.waipointQuantitativoHandlerLeave.bind(this);
		this.handleScrollTo = this.handleScrollTo.bind(this);
		this.fetch();
	}

	componentDidMount() {
		this.setBodyHeight();
	}

	setBodyHeight() {
		var windowHeight = window.innerHeight;
		var mainHeaderHeight = 60;
		var breadcrumbHeight = ReactDOM.findDOMNode(this.breadcrumbWrapper).offsetHeight;
		this.setState({ bodyHeight: (windowHeight * 1) - (mainHeaderHeight * 1) - (breadcrumbHeight * 1) });
	}

	setPlanHeight() {
		var bodyHeight = this.state.bodyHeight;
		var headerHeight = ReactDOM.findDOMNode(this.breadcrumbWrapper).offsetHeight;
		var tabsHeight = ReactDOM.findDOMNode(this.breadcrumbWrapper).offsetHeight;
		this.setState({ tabsContentHeight: (bodyHeight * 1) - (headerHeight * 1) - (tabsHeight) });
	}

	fetch() {
		var url = `/ordens-de-manutencao/ficha-de-manutencao/curativa`;
		var params = {
			orderId: this.state.orderId,
			equipmentId: this.state.equipmentId,
			servicoIds: this.state.servicoIds,
			marcaIds: this.state.marcaIds
		};
		axios.get(url, { params }).then((result) => {
			var data = result.data;
			if (data.order && data.equipments) {

				var state = {
					order: data.order,
					title: data.equipments.length > 0 ? data.equipments[0].categoriaText : "",
					service: data.equipments.length > 0 ? data.equipments[0].servicoText : "",
					room: data.equipments.length == 1 ? data.equipments[0].sala : "",
					equipments: data.equipments
				}

				addLinkedPropsToObject(state, this);

				this.setState(state);
			}
		}).catch(function (error) {
		}).then(() => {
			this.setState({
				isLoading: false,
				equipmentsHeaderScroll: {
					//outerWidth: ReactDOM.findDOMNode(this.equipmentsHeaderWrapper).offsetWidth,
					//innerWidth: ReactDOM.findDOMNode(this.planEquipmentsHeader).scrollWidth,
					scrollLeft: 0
				}
			});
			//this.setPlanHeight();
			setTimeout(() => {
				this.setState({ tooltipReady: true });
				Tooltip.Hidden.hide();
				Tooltip.Hidden.rebuild();
			}, 1200);
		});
	}

	getScrollShadow() {
		//return "scroll-shadow " + (this.state.equipmentsHeaderScroll.innerWidth > this.state.equipmentsHeaderScroll.outerWidth ? "right left" : "");
		return "scroll-shadow " + (this.state.equipmentsHeaderScroll.innerWidth > this.state.equipmentsHeaderScroll.outerWidth ?
			this.state.equipmentsHeaderScroll.scrollLeft == 0 ? "right" :
				(this.state.equipmentsHeaderScroll.scrollLeft + this.state.equipmentsHeaderScroll.outerWidth >= this.state.equipmentsHeaderScroll.innerWidth ? "left" : "right left")
			: ""
		)
	}

	waipointQualitativoHandlerLeave({ previousPosition, currentPosition, event }) {
		const isAbove = currentPosition === Waypoint.above;
		const wasInside = previousPosition === Waypoint.inside;
		if (isAbove && wasInside) {
			this.setState({ position: 1 });
		}
		if (!isAbove && !wasInside) {
			this.setState({ position: 0 });
		}
	}

	waipointQuantitativoHandlerLeave({ previousPosition, currentPosition, event }) {
		const isAbove = currentPosition === Waypoint.above;
		const wasInside = previousPosition === Waypoint.inside;
		if (isAbove && wasInside) {
			this.setState({ position: 2 });
		}
		if (!isAbove && !wasInside) {
			this.setState({ position: 1 });
		}
	}

	handleScrollTo = (elRef) => {
		// Incase the ref supplied isn't ref.current
		const el = elRef.current ? elRef.current : elRef

		// Scroll the element into view
		el.scrollIntoView({
			behavior: 'smooth',
			block: 'start'
		});
	}

	render() {

		return (
			<PageTemplate >
				<Wrapper padding={'0 0 0'} width="100%" minHeight="274px" ref={el => this.pageWrapper = el} background={Color(this.props.theme.palette.secondary.default).alpha(0.2).toString()}>
					<Wrapper background="white">
						<Breadcrumb order={this.state.order} onRef={el => this.breadcrumbWrapper = el} />
					</Wrapper>
					<div className="scrollarea" style={{ height: this.state.bodyHeight + 'px', overflow: 'auto' }}>
						<Sticky scrollElement=".scrollarea" style={{ zIndex: 11 }} >
							<HeaderTitle title={this.state.title} onRef={el => this.headerTitleWrapper = el}
								onEquipmentsChange={
									() => {
										this.setState({
											isLoading: false,
											equipmentsHeaderScroll: {
												//outerWidth: ReactDOM.findDOMNode(this.equipmentsHeaderWrapper).offsetWidth,
												//innerWidth: ReactDOM.findDOMNode(this.planEquipmentsHeader).scrollWidth,
												scrollLeft: 0
											}
										});
										//this.setPlanHeight();
										setTimeout(() => {
											this.setState({ tooltipReady: true });
											Tooltip.Hidden.hide();
											Tooltip.Hidden.rebuild();
										}, 1200);
									}
								}
							/>
						</Sticky>
						<HeaderDescription
							service={this.state.service} types={this.state.types}
							order={this.state.order} equipments={this.state.equipments}
							$equipments={this.state.$equipments}
							title={this.state.title}
							orderId={this.state.orderId}
							categoryId={this.state.categoryId}
						/>
						<Grid container direction="row" justify="space-between" alignitems="top" spacing={0} maxwidth={'100%'} margin={0} >
							<Grid item xs={12} >

								<Wrapper padding="48px 44px 48px 44px" background="white">
									<div className="col-sm-8 col-lg-10">
										<Wrapper padding="16px 0 16px 0">
											<Text b style={{ color: _theme.palette.secondary.default }}><Icon curativa data-tip="Curativa" />&nbsp;&nbsp; Ordem Curativa</Text>
											&nbsp;&nbsp;&nbsp;&nbsp;
											<Text span style={{ color: _theme.palette.primary.medium }}>Sem relatório disponível</Text>
										</Wrapper>
									</div>
									<div className="col-sm-4 col-lg-2 text-center">
										<Wrapper padding="4px 32px 4px 32px">
											<div className=" text-center">
												<Index p>1</Index>
												<Num p><small>{this.state.equipments[0] && this.state.equipments[0].numEquipamento}</small></Num>
											</div>
										</Wrapper>
									</div>
									<div className="clearfix"></div>
								</Wrapper>
								<Wrapper padding="0" minHeight="164px" >
									<Wrapper padding="48px 44px 52px 44px">

										<div className="col-sm-8 col-lg-10">
											<Wrapper padding="16px 0 16px 0">
												<Text h2 ><b>Estado final</b></Text>
											</Wrapper>
										</div>
										<div className="col-sm-4 col-lg-2 text-center">
											<Wrapper padding="4px 32px 4px 32px">
												{this.state.equipments.map((equipment, i) => {
													return (
														<Wrapper padding="0"
															key={i}>

															<FinalState
																$value={equipment.$estadoFinal}
																$message={equipment.$observacao}
																brand={equipment.marcaText} model={equipment.modeloText} serialNumber={equipment.numSerie} inventoryNumber={equipment.numInventario} />

														</Wrapper>

													)
												})}
											</Wrapper>
										</div>

									</Wrapper>

								</Wrapper>

							</Grid>
							<Grid item xs={12} >

							</Grid>
						</Grid>
					</div>
				</Wrapper>
				<TooltipHidden />
			</PageTemplate>
		)
	}
}

export default withTheme(withRouter(FichaDeManutencao));