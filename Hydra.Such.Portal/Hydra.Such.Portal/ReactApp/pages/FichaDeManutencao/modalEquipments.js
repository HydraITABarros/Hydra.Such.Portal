// https://github.com/diegohaz/arc/wiki/Atomic-Design
/*react-styleguide: ignore*/
import React, { Component, Fragment } from 'react';
import _theme from '../../themes/default';
import MuiTabs from '@material-ui/core/Tabs';
import MuiTab from '@material-ui/core/Tab';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { Button, Text, Icon, Circle, Wrapper, OmDatePicker, CheckBox, Input, Avatars, ModalLarge, Tooltip, Spacer } from 'components';
import MuiGrid from '@material-ui/core/Grid';
import AppBar from '@material-ui/core/AppBar';
import EMM from './emm';
import Material from './material';
import Upload from './upload';
import Documentos from './documentos';
import Select from 'react-select';
import axios from 'axios';
import Functions from '../../helpers/functions';

const addLinkedPropsToObject = Functions.addLinkedPropsToObject;

axios.defaults.headers.post['Accept'] = 'application/json';
axios.defaults.headers.get['Accept'] = 'application/json';

const { DialogTitle, DialogContent, DialogActions } = ModalLarge;

injectGlobal`
	.equipmentsCounter {
		position: absolute;
    	left: 0px;
		span {
			color: ${_theme.palette.primary.medium};
		}
	}
	[class*="css"]{
		&[class*="menu"] {
			z-index:1000 !important;			
		}
		&[class*="container"] {
		line-height: 24px;	
		}
	}
`
const Grid = styled(MuiGrid)`&& {
	padding: ${props => props.padding ? props.padding : (props.container ? '0' : '0 15px')};
	white-space: nowrap;
	overflow: visible;
	text-overflow: ellipsis;

	line-height: 40px;
}`

class ModalEquipments extends Component {
	state = {
		open: false,
		search: false,
		selected: null,
		searchEquipments: [],
		isLoading: false
	}

	constructor(props) {
		super(props);
		this.fetch = this.fetch.bind(this);
		this.fetch();
	}

	fetch() {
		var url = `/ordens-de-manutencao/equipments`;
		var params = {
			categoryId: this.props.categoryId,
			orderId: this.props.orderId
		};
		axios.get(url, { params }).then((result) => {
			var data = result.data;
			if (data) {

				var state = {
					searchEquipments: data
				}

				this.setState(state);
			}
		}).catch(function (error) {
		}).then(() => {
			this.setState({
				isLoading: false
			});
		});
	}

	fetchEquipmentPlan(equipmentId) {
		var url = `/ordens-de-manutencao/ficha-de-manutencao`;
		var params = {
			categoryId: this.props.categoryId,
			orderId: this.props.orderId,
			equipmentIds: "" + equipmentId
		};
		return axios.get(url, { params });
	}

	componentDidUpdate(props) {
		var newState = {};
		if (props.$equipments !== this.state.$equipments) {
			newState.$equipments = props.$equipments;
		}
		if (Object.keys(newState).length > 0) {
			this.setState(newState, () => {/*console.log(this.state)*/ });
		}
	}

	render() {
		return (
			<ModalLarge
				onClose={() => this.setState({ open: false })}
				onOpen={() => this.setState({ open: true })}
				open={this.state.open}
				action={this.props.children} children={
					<div>
						<DialogTitle>
							<Text h2>{this.props.title}</Text>

						</DialogTitle>
						<hr />

						<DialogContent>
							{!this.state.search &&
								<Grid container direction="row" justify="space-between" alignitems="middle" spacing={0} maxwidth={'100%'} margin={0}>
									<Grid item xs={12} sm={2} padding="4px">
										<Input placeholder={'Marca'} />
									</Grid>
									<Grid item xs={12} sm={2} padding="4px">
										<Input placeholder={'Modelo'} />
									</Grid>
									<Grid item xs={12} sm={2} padding="4px">
										<Input placeholder={'Cliente'} />
									</Grid>
									<Grid item xs={12} sm={2} padding="4px">
										<Input placeholder={'Nº Série'} />
									</Grid>
									<Grid item xs={12} sm={2} padding="4px">
										<Button link style={{ display: 'block' }} onClick={() => this.setState({ search: true })}>Procurar</Button>
									</Grid>
									<Grid item xs={12} sm={2} padding="4px" >
										<Button default className={'pull-right'}>Adicionar</Button>
									</Grid>
								</Grid>
							}
							{this.state.search &&
								<Grid container direction="row" justify="space-between" alignitems="middle" spacing={0} maxwidth={'100%'} margin={0}>
									<Grid item xs={12} sm={8} padding="4px" style={{ overflow: 'visible' }}>
										<Select placeholder={'Adicionar ' + this.props.title}
											value={this.state.selected}
											options={
												this.state.searchEquipments.filter((item) => {
													return !(this.state.$equipments.value.filter((i) => {
														return i.idEquipamento == item.idEquipamento;
													}).length > 0);
												})
													.map((item) => {
														return { value: item, label: item.numEquipamento }
													})
											}
											onChange={(selected) => {
												this.setState({ selected: selected });
											}}
											isSearchable
											isClearable
										/>
									</Grid>
									<Grid item xs={12} sm={2} padding="4px">
										<Button link style={{ display: 'block' }} onClick={() => this.setState({ search: false })}>Novo</Button>
									</Grid>
									<Grid item xs={12} sm={2} padding="4px" >
										<Button default className={'pull-right'} onClick={(e) => {
											this.fetchEquipmentPlan(this.state.selected.value.idEquipamento).then((result) => {
												var data = result.data;

												if (data.order && data.equipments) {

													var equipmentPlan = data.equipments[0]
													addLinkedPropsToObject(equipmentPlan, this);
													this.setState({ selected: null });
													var eqArray = this.props.$equipments.value;
													eqArray.push(equipmentPlan);
													this.props.$equipments.value = eqArray;

													console.log('Cenas', data);

													this.props.onChange();

													console.log('IMP ', updateURLParameter(window.location.href, 'equipmentsIds', this.props.$equipments.value.map((i) => {
														return i.idEquipamento;
													}).join(',')));

													history.push()
												}
											}).catch(function (error) {
											})

										}}>Adicionar</Button>
									</Grid>
								</Grid>
							}
							<Spacer height={'16px'} />
							<hr />
							<Spacer height={'24px'} />
							<Grid container direction="row" justify="space-between" alignitems="middle" spacing={0} maxwidth={'100%'} margin={0} >
								<Grid item xs={2} ><Text label data-tip={'Marca'}>#&nbsp;&nbsp;Marca</Text></Grid>
								<Grid item xs={2} ><Text label data-tip={'Modelo'}>Modelo</Text></Grid>
								<Grid item xs={2} ><Text label data-tip={'Nº Equip.'}>Nº Equip.</Text></Grid>
								<Grid item xs={2} ><Text label data-tip={'Nº Série.'}>Nº Série.</Text></Grid>
								<Grid item xs={2} ></Grid>
								<Grid item xs={2} ></Grid>
								{this.props.$equipments && this.props.$equipments.value.map((item, index) => {
									return (<Grid container key={index} direction="row" justify="space-between" alignitems="middle" spacing={0} maxwidth={'100%'} margin={0} >
										<Grid item xs={2} ><Text span data-tip={item.marcaText}>{index + 1}&nbsp;&nbsp;{item.marcaText}</Text></Grid>
										<Grid item xs={2} ><Text span data-tip={item.modeloText}>{item.modeloText}</Text></Grid>
										<Grid item xs={2} ><Text span data-tip={item.numEquipamento}>{item.numEquipamento}</Text></Grid>
										<Grid item xs={2} ><Text span data-tip={item.numSerie}>{item.numSerie}</Text></Grid>
										<Grid item xs={2} >{this.props.$equipments.value.length > 1 && <Button iconSolo onClick={() => {
											var eqArray = this.props.$equipments.value;
											eqArray.splice(index, 1);
											this.props.$equipments.value = eqArray;
										}
										}><Icon decline /></Button>}</Grid>
										<Grid item xs={2} ></Grid>
									</Grid>)
								})}
							</Grid>
						</DialogContent>
						<hr />
						<DialogActions>
							<Wrapper className="equipmentsCounter" padding="0 0 0 40px"><Text span>{this.props.$equipments && this.props.$equipments.value.length} Equipamentos</Text></Wrapper>
							<Button primary color="primary"
								onClick={() => this.setState({ open: false })}
							>Guardar</Button>
						</DialogActions>
					</div>
				} />
		)
	}
}

/**
 * http://stackoverflow.com/a/10997390/11236
 */
function updateURLParameter(url, param, paramVal) {
	var newAdditionalURL = "";
	var tempArray = url.split("?");
	var baseURL = tempArray[0];
	var additionalURL = tempArray[1];
	var temp = "";
	if (additionalURL) {
		tempArray = additionalURL.split("&");
		for (var i = 0; i < tempArray.length; i++) {
			if (tempArray[i].split('=')[0] != param) {
				newAdditionalURL += temp + tempArray[i];
				temp = "&";
			}
		}
	}

	var rows_txt = temp + "" + param + "=" + paramVal;
	return baseURL + "?" + newAdditionalURL + rows_txt;
}

export default ModalEquipments;