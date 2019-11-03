import React, { Component } from 'react';
import MuiTabs from '@material-ui/core/Tabs';
import MuiTab from '@material-ui/core/Tab';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { Button, Text, Icon, Circle, Wrapper, OmDatePicker, CheckBox, Input, Avatars, ModalLarge, Tooltip, Spacer } from 'components';
import AppBar from '@material-ui/core/AppBar';
import { Grid } from '@material-ui/core';
import functions from '../../../helpers/functions';
import _theme from '../../../themes/default';
import './index.scss';

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
		selectedEquipments: []
	}
	constructor(props) {
		super(props);
		this.handleChange = this.handleChange.bind(this);
	}
	handleChange(e, value) {
		this.setState({

		});
	}

	componentDidMount() {
		var selectedEquipments = [];
	}

	componentWillUnmount() {
		this.state.selectedEquipments = [];
	}

	componentWillReceiveProps(nextProps) {
		if (nextProps.open !== this.state.open) {
			this.setState({ open: nextProps.open });
		}
		if (nextProps.$equipments && this.props.$equipments && nextProps.$equipments.value !== this.props.$equipments) {
			var selectedEquipments = [];
			if (this.props.$equipments) {
				this.props.$equipments.value.map((item, i) => {
					if (i > 4) {
						return;
					}
					item.checked = true;
					selectedEquipments.push(item);
				})
				this.setState({
					selectedEquipments
				});
			}

			console.log('IMP', selectedEquipments);

		}
	}

	render() {
		var order = this.props.order || {};
		var maintenance, quality, quantity;
		if (this.props.$equipments && this.props.$equipments.value[0]) {
			maintenance = this.props.$equipments.value[0].planMaintenance;
			quality = this.props.$equipments.value[0].planQuality;
			quantity = this.props.$equipments.value[0].planQuantity;
		}
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
					<div className="modal-report">
						<DialogTitle>
							<Text h2><Icon report /> Relatório de Manutenção</Text>
						</DialogTitle>
						<hr />

						<Grid container direction="row" justify="space-between" alignitems="top" spacing={0} maxwidth={'100%'} margin={0} >
							<Grid item xs={12} md={4} lg={3} style={{ borderBottom: '1px solid #E4E7EB' }}>
								<DialogContent>
									<div className="col-xs-10 p-l-0 p-r-0">
										<Text b className="ws-nowrap to-ellipsis">{this.props.equipmentType}</Text>
									</div>
									<div className="col-xs-2 p-l-0 p-r-0">
										<Text b>({this.props.$equipments && this.props.$equipments.value.length})</Text>
									</div>
									<div className="clearfix"></div>
									{/* <div>debug selected count: {this.state.selectedEquipments.length}</div> */}
									{this.props.$equipments && this.props.$equipments.value.map((equipment, i) => {
										return (
											<div key={i} style={{ lineHeight: '32px' }}>
												<div className="w-30 v-a-m">
													<CheckBox id={"report-checkbox-" + i} className="p-t-0 p-l-0 p-r-0 p-b-0"
														checked={equipment.checked}
														onChange={(event) => {
															if (event.target.checked) {
																this.state.selectedEquipments = this.state.selectedEquipments.filter((selected) => {
																	return selected.idEquipamento != e.idEquipamento;
																});
																equipment.checked = true;
																this.state.selectedEquipments.push(equipment);
																this.setState({ selectedEquipments: this.state.selectedEquipments });
															} else {
																this.state.selectedEquipments = this.state.selectedEquipments.filter((selected) => {
																	return selected.idEquipamento != equipment.idEquipamento;
																});
																equipment.checked = false;
																this.setState({ selectedEquipments: this.state.selectedEquipments || [] });
															}
														}} />
												</div>
												<div className="w-auto ws-nowrap to-ellipsis v-a-m">
													<label htmlFor={"report-checkbox-" + i}>
														<Text b className="w-20">#{i + 1}</Text> &nbsp;<Text span>{equipment.numEquipamento}</Text>
													</label>
												</div>
											</div>
										)
									})}
								</DialogContent>
							</Grid>
							<Grid item xs={12} md={8} lg={9}
								style={{ lineHeight: 0, overflow: 'auto', borderRight: '1px solid #E4E7EB', borderBottom: '1px solid #E4E7EB', textAlign: 'right' }}
								className={this.state.selectedEquipments.length > 0 ? "" : "content-disabled"}>

								<div className="report__wrapper">
									<div className="report">

										<div className="report__page report__page--master">

											<div className="report__header">
												<div className="col-xs-5">
													<img src="/images/such-engenharia.png" alt="Such Engenharia" className="img-responsive" />
												</div>
												<div className="col-xs-7 padding-0">
													<div className="col-xs-6 report__header__avatar__wrapper">
														Chefe de Projecto
														<Avatars.Avatars
															className="report__header__avatar"
															letter
															color={_theme.palette.primary.light} data-tip={"nome"}
														>
															{functions.getInitials("nome")}
														</Avatars.Avatars>
													</div>
													<div className="col-xs-6 report__header__avatar__wrapper">
														Resp. Manutenção
														<Avatars.Avatars
															className="report__header__avatar"
															letter
															color={_theme.palette.primary.light} data-tip={"nome"}
														>
															{functions.getInitials("nome")}
														</Avatars.Avatars>
													</div>
												</div>
												<div className="clearfix"></div>
											</div>

											<div className="report__title">
												<Text p>Relatório de Manutenção - 11 Mar 2019</Text>
												<Text p className="report__page-counter">1/3</Text>
												<Text h1 className="f-s-40"><Icon preventiva />{this.props.equipmentType}</Text>
											</div>

											<div className="report__body">

												<div className="report__spacer--45"></div>
												{/* Cliente */}
												<div className="col-xs-2"><Text b className="report__label">Cliente</Text></div>
												<div className="col-xs-10"><Text p className="report__text">{order.customerName}</Text></div>
												<div className="clearfix"></div>
												{/* Instituição */}
												<div className="col-xs-2"><Text b className="report__label">Instituição</Text></div>
												<div className="col-xs-10"><Text p className="report__text">{order.institutionName}</Text></div>
												<div className="clearfix"></div>
												{/* Serviço */}
												{order.serviceName &&
													<div>
														<div className="col-xs-2"><Text b className="report__label">Serviço</Text></div>
														<div className="col-xs-10"><Text p className="report__text">{order.serviceName}</Text></div>
													</div>
												}

												<div className="clearfix"></div>
												<div className="report__spacer--35"></div>

												<div className="col-xs-2"><Text b className="report__label">Tipo de Ordem</Text></div>
												<div className="col-xs-10"><Text p className="report__text">{order.orderType}</Text></div>
												<div className="clearfix"></div>

												<div className="col-xs-2"><Text b className="report__label">Estado</Text></div>
												<div className="col-xs-10"><Text p className="report__text">{order.status}</Text></div>

												<div className="clearfix"></div>
												<div className="report__spacer--35"></div>

												<div className="col-xs-2"><Text b className="report__label"># Marca</Text></div>
												<div className="col-xs-2"><Text b className="report__label">Modelo</Text></div>
												<div className="col-xs-2"><Text b className="report__label">Nº Série</Text></div>
												<div className="col-xs-2"><Text b className="report__label">Nº Equip.</Text></div>
												<div className="col-xs-2"><Text b className="report__label">Nº Inventário</Text></div>
												<div className="clearfix"></div>
												{this.state.selectedEquipments.map((equipment, i) => {
													return (
														<div key={i}>
															<div className="col-xs-2 ws-nowrap"><Text p className="report__text">{i + 1}&nbsp; {equipment.marcaText}</Text></div>
															<div className="col-xs-2 ws-nowrap"><Text p className="report__text">{equipment.modeloText}</Text></div>
															<div className="col-xs-2 ws-nowrap"><Text p className="report__text">{equipment.numSerie}</Text></div>
															<div className="col-xs-2 ws-nowrap"><Text p className="report__text">{equipment.numEquipamento}</Text></div>
															<div className="col-xs-2 ws-nowrap"><Text p className="report__text">{equipment.numInventario}</Text></div>
															<div className="clearfix"></div>
														</div>
													)
												})}
												<div className="clearfix"></div>
												<div className="report__spacer--35"></div>

												{/* Material Aplicado To Do */}
												<div className="col-xs-1"><Text b className="report__label">Equip.</Text></div>
												<div className="col-xs-5"><Text b className="report__label">Material Aplicado</Text></div>
												<div className="col-xs-2"><Text b className="report__label">Qtd.</Text></div>
												<div className="col-xs-4"><Text b className="report__label">Fornecido por</Text></div>

												<div className="col-xs-1 ws-nowrap"><Text p className="report__text"> - </Text></div>
												<div className="col-xs-5 ws-nowrap"><Text p className="report__text"> - </Text></div>
												<div className="col-xs-2 ws-nowrap"><Text p className="report__text"> - </Text></div>
												<div className="col-xs-4 ws-nowrap"><Text p className="report__text"> - </Text></div>

												<div className="clearfix"></div>
												<div className="report__spacer--45"></div>
											</div>
											<div className="report__hr"></div>



											<div className="report__body">
												<div className="report__spacer--40"></div>
												{maintenance &&
													<div>
														<div className="report__row">
															<div className="col-xs-7"><Text b className="report__label">Manutenção</Text></div>
															{this.state.selectedEquipments.map((equipments, index) => {
																return (
																	<div key={index} className={"col-xs-" + (this.state.selectedEquipments.length / 5) + " text-center"}>
																		<Text b className="report__label">#{index + 1}</Text>
																	</div>
																)
															})}
															<div className="clearfix"></div>
														</div>
														{maintenance.map((item, index) => {
															return (
																<div key={index}>
																	<div className="report__hr"></div>
																	<div className="report__row">
																		<div className="col-xs-7">
																			<Text p className="report__text">{item.descricao}</Text>
																		</div>
																		{this.state.selectedEquipments.map((equipment, i) => {
																			return (
																				<div key={index + "" + i} className={"col-xs-" + (this.state.selectedEquipments.length / 5) + " text-center"}>
																					<Text p className="report__text"><Icon approved /></Text>
																				</div>
																			)
																		})}
																		<div className="clearfix"></div>
																	</div>
																</div>
															)
														})}
														<div className="clearfix"></div>
													</div>
												}

												{/* </div> */}

												{/* <div className="report__blank" data-html2canvas-ignore="true"></div> */}

												{/*
											<div className="report__header">
												<div className="col-xs-5">
													<img src="/images/such-engenharia.png" alt="Such Engenharia" className="img-responsive" />
												</div>
												<div className="col-xs-7 padding-0">
													<div className="col-xs-6 report__header__avatar__wrapper">
														Chefe de Projecto
														<Avatars.Avatars
															className="report__header__avatar"
															letter
															color={_theme.palette.primary.light} data-tip={"nome"}
														>
															{functions.getInitials("nome")}
														</Avatars.Avatars>
													</div>
													<div className="col-xs-6 report__header__avatar__wrapper">
														Resp. Manutenção
														<Avatars.Avatars
															className="report__header__avatar"
															letter
															color={_theme.palette.primary.light} data-tip={"nome"}
														>
															{functions.getInitials("nome")}
														</Avatars.Avatars>
													</div>
												</div>
												<div className="clearfix"></div>
											</div>

											<div className="report__title">
												<Text p>Relatório de Manutenção - 11 Mar 2019</Text>
												<Text p className="report__page-counter">2/3</Text>
												<Text h1><Icon preventiva />Electrobisturi</Text>
											</div>
											*/}

												{/* <div className="report__body"> */}
												{/* <div className="report__row">
													<div className="col-xs-7"><Text b className="report__label">Manutenção (cont.)</Text></div>
													<div className="col-xs-1 text-center"><Text b className="report__label">#1</Text></div>
													<div className="col-xs-1 text-center"><Text b className="report__label">#2</Text></div>
													<div className="col-xs-1 text-center"><Text b className="report__label">#3</Text></div>
													<div className="col-xs-1 text-center"><Text b className="report__label">#4</Text></div>
													<div className="col-xs-1 text-center"><Text b className="report__label">#5</Text></div>
													<div className="clearfix"></div>
												</div> */}

												<div className="report__spacer"></div>
												{quality &&
													<div>
														<div className="report__row">
															<div className="col-xs-7"><Text b className="report__label">Qualitativos</Text></div>
															{this.state.selectedEquipments.map((equipments, index) => {
																return (
																	<div key={index} className={"col-xs-" + (this.state.selectedEquipments.length / 5) + " text-center"}>
																		<Text b className="report__label">#{index + 1}</Text>
																	</div>
																)
															})}
															<div className="clearfix"></div>
														</div>
														{quality.map((item, index) => {
															return (
																<div key={index}>
																	<div className="report__hr"></div>
																	<div className="report__row">
																		<div className="col-xs-7">
																			<Text p className="report__text">{item.descricao}</Text>
																		</div>
																		{this.state.selectedEquipments.map((equipment, i) => {
																			return (
																				<div key={index + "" + 1} className={"col-xs-" + (this.state.selectedEquipments.length / 5) + " text-center"}>
																					<Text p className="report__text"><Icon approved /></Text>
																				</div>
																			)
																		})}
																		<div className="clearfix"></div>
																	</div>
																</div>
															)
														})}
														<div className="clearfix"></div>
													</div>
												}

												<div className="report__spacer"></div>
												{quantity &&
													<div>
														<div className="report__row">
															<div className="col-xs-6"><Text b className="report__label">Quantitativos</Text></div>
															<div className="col-xs-1 text-center"><Text b className="report__label"></Text></div>
															{this.state.selectedEquipments.map((equipments, index) => {
																return (
																	<div key={index} className={"col-xs-" + (this.state.selectedEquipments.length / 5) + " text-center"}>
																		<Text b className="report__label">#{index + 1}</Text>
																	</div>
																)
															})}
															<div className="clearfix"></div>
														</div>
														{quantity.map((item, index) => {
															return (
																<div key={index}>
																	<div className="report__hr"></div>
																	<div className="report__row">
																		<div className="col-xs-6">
																			<Text p className="report__text">{item.descricao}</Text>
																		</div>
																		<div className="col-xs-1 text-center">
																			<Text p className="report__text">{item.unidadeCampo1}</Text>
																		</div>
																		{this.state.selectedEquipments.map((equipment, i) => {
																			return (
																				<div key={index + "" + i} className={"col-xs-" + (this.state.selectedEquipments.length / 5) + " text-center"}>
																					<Text p className="report__text">{equipment.planQuantity[index].resultado}</Text>
																				</div>
																			)
																		})}
																		<div className="clearfix"></div>
																	</div>
																</div>
															)
														})}
														<div className="clearfix"></div>
													</div>
												}

												<div className="report__spacer"></div>
												<div className="report__row">
													<div className="col-xs-12"><Text b className="report__label">Observações</Text></div>
													<div className="clearfix"></div>
												</div>
												<div className="report__hr"></div>
												{this.state.selectedEquipments.length > 0 &&
													this.state.selectedEquipments.map((equipment, i) => {
														var printed = false;
														return (
															<div key={i}>
																{(equipment.observacao && equipment.observacao != "") &&
																	<div >
																		{(!printed) &&
																			(printed = true) &&
																			<div className="col-xs-12 p-t-10"><Text b className="report__text m-b-5">Geral</Text> </div>
																		}
																		<div className="col-xs-1">
																			<Text p className="report__text">#{i + 1}</Text>
																		</div>
																		<div className="col-xs-11">
																			<Text p className="report__text">{equipment.observacao}</Text>
																		</div>
																		<div className="clearfix"></div>
																	</div>}
															</div>
														)
													})
												}
												{(maintenance) &&
													maintenance.map((item, index) => {
														var printed = false;
														return (
															<div key={index}>
																{this.state.selectedEquipments.map((equipment, i) => {
																	return (
																		<div key={index + "" + i}>
																			{(equipment.planMaintenance[index].observacoes && equipment.planMaintenance[index].observacoes != "") &&
																				<div>
																					{!printed && (() => {
																						printed = true;
																						return (
																							<div className="col-xs-12">
																								<Text b className="report__text m-b-5">
																									{item.descricao}
																								</Text>
																							</div>
																						)
																					})()}
																					<div className="col-xs-1">
																						<Text p className="report__text">#{i + 1}</Text>
																					</div>
																					<div className="col-xs-11">
																						<Text p className="report__text">{equipment.planMaintenance[index].observacoes}</Text>
																					</div>
																					<div className="clearfix"></div>
																				</div>
																			}
																		</div>
																	)
																})}
															</div>
														)
													})
												}

												{(quality) &&
													quality.map((item, index) => {
														var printed = false;
														return (
															<div key={index}>
																{this.state.selectedEquipments.map((equipment, i) => {
																	return (
																		<div key={index + "" + i}>
																			{(equipment.planQuality[index].observacoes && equipment.planQuality[index].observacoes != "") &&
																				<div>
																					{!printed && (() => {
																						printed = true;
																						return (
																							<div className="col-xs-12">
																								<Text b className="report__text m-b-5">
																									{item.descricao}
																								</Text>
																							</div>
																						)
																					})()}
																					<div className="col-xs-1">
																						<Text p className="report__text">#{i + 1}</Text>
																					</div>
																					<div className="col-xs-11">
																						<Text p className="report__text">{equipment.planQuality[index].observacoes}</Text>
																					</div>
																					<div className="clearfix"></div>
																				</div>}
																		</div>
																	)
																})}
															</div>
														)
													})
												}

												{(quantity) &&
													quantity.map((item, index) => {
														var printed = false;
														return (
															<div key={index}>
																{this.state.selectedEquipments.map((equipment, i) => {
																	return (
																		<div key={index + "" + i}>
																			{(equipment.planQuantity[index].observacoes && equipment.planQuantity[index].observacoes != "") &&
																				<div index={index + "" + i}>
																					{!printed && (() => {
																						printed = true;
																						return (
																							<div className="col-xs-12">
																								<Text b className="report__text m-b-5">
																									{item.descricao}
																								</Text>
																							</div>
																						)
																					})()}
																					<div className="col-xs-1">
																						<Text p className="report__text">#{i + 1}</Text>
																					</div>
																					<div className="col-xs-11">
																						<Text p className="report__text">{equipment.planQuantity[index].observacoes}</Text>
																					</div>
																					<div className="clearfix"></div>
																				</div>
																			}
																		</div>
																	)
																})}
															</div>
														)
													})
												}

												<div className="report__spacer--35"></div>
												<div className="report__spacer--35"></div>
											</div>
											<div className="report__hr"></div>
											<div className="report__footer">

												<div className="col-xs-12 p-b-15">
													<div className="report__label">
														<Text b className="f-s-12">Data</Text>&nbsp;&nbsp; <Text span>20 Mar 2019</Text>
													</div>
												</div>
												<div className="col-xs-6">
													<div className="report__label p-b-5">
														<Text h2>Such</Text>
													</div>
													<div className="col-xs-5 p-l-0 p-r-0">
														<div className="report__header__avatar__wrapper text-left m-t-10 m-b-5 text-normal">
															<Avatars.Avatars
																className="report__header__avatar m-l-0 m-r-15"
																letter
																color={_theme.palette.primary.light} data-tip={"nome"}
															>
																{functions.getInitials("nome")}
															</Avatars.Avatars>
															<Text span>Andreia Silva</Text>
														</div>
													</div>
													<div className="col-xs-5 p-l-0 p-r-0">
														<img src={this.props.$equipments && this.props.$equipments.value[0].assinaturaTecnico} className="report__signature img-responsive" />
													</div>
													<div className="col-xs-2 p-l-0 p-r-0">
													</div>
													<div className="clearfix"></div>
												</div>

												<div className="col-xs-6">
													<div className="report__label m-b-5">
														<Text h2>Cliente</Text>
													</div>
													<div className="col-xs-5 p-l-0 p-r-0">
														<div className="text-left m-t-10 m-b-5 f-s-12">
															<Text b className="f-s-12 text-uppercase l-h-1"> Assinatura</Text>
														</div>
													</div>
													<div className="col-xs-5 p-l-0 p-r-0">
														<img src={
															this.state.selectedEquipments[this.state.selectedEquipments.length - 1] &&
															this.state.selectedEquipments[this.state.selectedEquipments.length - 1].assinaturaCliente
														} className="report__signature img-responsive" />
													</div>
													<div className="col-xs-2 p-l-0 p-r-0">
													</div>
													<div className="clearfix"></div>
													<div className="col-xs-5 p-l-0 p-r-0">
														<div className="text-left m-t-10 m-b-5 f-s-12 ">
															<Text b className="f-s-12 text-uppercase l-h-1">Sie/Aprovis. <br /> Assinatura</Text>
														</div>
													</div>
													<div className="col-xs-5 p-l-0 p-r-0">
														<img src={
															this.state.selectedEquipments[this.state.selectedEquipments.length - 1] &&
															this.state.selectedEquipments[this.state.selectedEquipments.length - 1].assinaturaSie
														} className="report__signature img-responsive" />
													</div>
													<div className="col-xs-2 p-l-0 p-r-0">
													</div>
												</div>

												<div className="col-xs-12 p-b-5"></div>
												<div className="clearfix"></div>
											</div>
										</div>
									</div>
								</div>

							</Grid>
						</Grid>

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