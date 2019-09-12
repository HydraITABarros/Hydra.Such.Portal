import React, { Component } from 'react';
import axios from 'axios';
import { PageTemplate } from 'components';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { Wrapper, OmDatePicker, Tooltip, PivotTable, Button, Icon } from 'components';
import moment from 'moment';
import ReactDOM from 'react-dom';
import { withRouter } from 'react-router-dom';
import Breadcrumb from './breadcrumb';

import Header from './header';
import HeaderSelection from './headerSelection';

axios.defaults.headers.post['Accept'] = 'application/json';
axios.defaults.headers.get['Accept'] = 'application/json';

injectGlobal`
    body {
        background-color: white;   
    }
    .navbar-container, .navbar-header {
        background-color: ${props => props.theme.palette.secondary.default};
    }
    .app-main {
        .row {
            margin: 0;
        }
        .wrap {
            padding: 0;
        }
    }
    @keyframes fade {
        from { opacity: 1.0; }
        50% { opacity: 0.5; }
        to { opacity: 1.0; }
    }                                                                                                                                                                                                                                  

    @-webkit-keyframes fade {
        from { opacity: 1.0; }
        50% { opacity: 0.5; }
        to { opacity: 1.0; }
    }
    .blink {
        animation:fade 1000ms infinite;
        -webkit-animation:fade 1000ms infinite;
    }
    mark, .mark {
        background-color: ${props => props.theme.palette.search} ;
        padding: 0;
    }
        [class*="MuiTableRow"] {
                .first-cell {
                        [class*="icon"] {
                                left: 24px !important;
                        }
                }
        }
        [class*="GroupPanelContainer"] {
                [class*="MuiChip-root"] {
                        position: relative;
                        background: ${props => props.theme.palette.primary.default};
                        border-radius: 7px;
                        padding: 0;
                        color: white;
                        font-family: Inter,Helvetica,sans-serif;
                        font-style: normal;
                        font-weight: 400;
                        font-size: 12px;
                        line-height: 16px;
                        text-transform: uppercase;
                        margin: 0 5px;
                        height: 24px;
                        &:hover, &:active, &:focus {
                                background: ${props => props.theme.palette.primary.default};
                                border-radius: 7px;
                                color: white;
                                font-family: Inter,Helvetica,sans-serif;
                                font-style: normal;
                                font-weight: 400;
                                font-size: 12px;
                                line-height: 16px;
                                text-transform: uppercase;
                                margin: 0 5px;
                        }
                        [class*="MuiButtonBase-root"] {
                                color: white;
                        }
                        [class*="MuiChip-label"] {
                                padding-left: 8px;
                                padding-right: 0px;
                        }
                        [class*="MuiChip-deleteIcon"] {
                                color: ${props => props.theme.palette.primary.default};
                        }
                        &[class*="MuiChip-deletable"] {
                                &:before {
                                        font-family: 'eSuch' !important;
                                        content: '\\e90b';
                                        color: white;
                                        position: absolute;
                                        top: 4px;                                                                
                                        right: 4px;
                                        pointer-events: none;
                                }
                        }
                }
        }
        [class*="MuiPopover-paper"] {
                [class*="DragDrop-container"] {
                        [role="button"] {
                                background: ${props => props.theme.palette.primary.default};
                                border-radius: 7px;
                                padding: 0;
                                color: white;
                                font-family: Inter,Helvetica,sans-serif;
                                font-style: normal;
                                font-weight: 400;
                                font-size: 12px;
                                line-height: 16px;
                                text-transform: uppercase;
                                margin: 0 5px;
                                height: 24px;
                                opacity: .3;
                        }
                }
                [class*="MuiSvgIcon-root"] {
                        &[focusable] {
                                color: ${props => props.theme.palette.primary.default}
                        }
                }
                [class*="MuiTypography-root"] {
                        font-family: Inter,Helvetica,sans-serif;
                        font-style: normal;
                        font-weight: 400;
                        font-size: 14px;
                        line-height: 24px;
                        margin: 0;
                        color: #323F4B;
                }
        }
        .table--row--hoverable {
                cursor: pointer;
                &:hover {
                        background: ${props => props.theme.palette.bg.grey};
                }
        }
        
`
const PickerButton = styled(Button)` && {
	position: relative;
	z-index: 100;
	padding-left: 25px;
	padding-right: 25px;
}
`;

const ListContainer = styled.div`
    position: absolute;
    top:0;
    left:0;
    right:0;
    bottom: 0;
    z-index: 0;
    overflow: auto;
    padding: 0;
    [class*="RootBase-root"]{
            position: absolute;
            top: 0;
            bottom: 0;
    }
`
const Hr = styled.hr`
    margin-bottom: 0;
    margin-top: 0;
`;

var cancelToken = axios.CancelToken;
var call;
var headerHeightTimer;

class OrdensDeManutencaoLine extends Component {
	state = {
		orderId: "OM1900719",
		isLoading: true,
		ordersCountsLines: {
			toSigning: null,
			toExecute: null,
			executed: null
		},
		marcas: [],
		servicos: [],
		tooltipReady: false,
		maintenanceOrder: {},
		equipments: [],
		equipmentsTotal: 0,
		equipmentsIsLoading: false,
		equipmentsLinesNext: "",
		listContainerStyle: {},
		selectionMode: false,
		selectedRows: [],
		datePickerOpen: false,
		calendar: {
			"from": null,
			"to": null,
			"olderFrom": null,
			"olderTo": null
		}

	}

	constructor(props) {
		super(props);
		moment.locale("pt");
		this.state.calendar.from = moment().subtract(1, 'month').startOf('month');

		this.state.calendar.to = moment().subtract(1, 'month').endOf('month');
		this.handleDateChange = this.handleDateChange.bind(this);
		this.handleResize = this.handleResize.bind(this);
		this.handleGridScroll = this.handleGridScroll.bind(this);
		this.fetchEquipements = this.fetchEquipements.bind(this);
		this.handleFetchEquipementsRequest = this.handleFetchEquipementsRequest.bind(this);
		this.state.orderId = this.props.match.params.orderid;
		this.addTechnical = this.addTechnical.bind(this);
	}

	componentDidMount() {
		window.addEventListener("resize", this.handleResize);
		this.setTableMarginTop();
		this.addTechnical();
	}

	handleResize() {
		setTimeout(() => {
			this.setTableMarginTop();
		}, 0)

	}

	handleGridScroll(e) {
		setTimeout(() => {
			Tooltip.Hidden.hide();
			Tooltip.Hidden.rebuild();
		});
	}

	setTableMarginTop() {
		clearTimeout(headerHeightTimer);
		headerHeightTimer = setTimeout(() => {
			if (typeof $ == 'undefined') {
				return setTimeout(() => {
					this.setTableMarginTop();
				}, 600);
			}
			var highlightWrapper = ReactDOM.findDOMNode(this.highlightWrapper);
			var listContainer = ReactDOM.findDOMNode(this.listContainer);
			var hr = 0;
			var top = (highlightWrapper.offsetHeight * 1) + hr;
			var appNavbarCollapse = document.getElementById("app-navbar-collapse");
			if (appNavbarCollapse) {
				var height = window.innerHeight - top - (document.getElementById("app-navbar-collapse").offsetHeight * 1);
				this.setState({ listContainerStyle: { "height": height, marginTop: top } }, () => {
					console.log(this.state.listContainerStyle.height, this.state.listContainerStyle.marginTop);
				})
			}
		}, 100);
	}

	fetchEquipements({ search, sort, page }, cb) {
		cb = cb || (() => { });
		var isNext = page > 1;

		this.setState({ equipmentsIsLoading: true }, () => {
			if (isNext && this.state.equipmentsLinesNext != "") {
				call = axios.CancelToken.source();
				this.handleFetchEquipementsRequest(axios.get(this.state.equipmentsLinesNext, { cancelToken: call.token }), isNext);
			} else {
				if (call) { call.cancel(); }
				call = axios.CancelToken.source();
				this.setState({ equipmentsIsLoading: true, equipmentsLinesNext: "", equipments: [], equipmentsTotal: 0 }, () => {
					var orderId = this.state.orderId;
					var filter = "";
					if (search && search.length > 0) {
						search.map((value, index) => {
							if (index > 0) {
								filter += " and "
							}
							filter += "(contains(nome,'" + value + "') or contains(numInventario,'" + value + "') or contains(numEquipamento,'" + value + "') or contains(numSerie,'" + value + "')";
							var _marcas = this.state.marcas.filter((item) => { return item.nome.toLowerCase().indexOf(value.toLowerCase()) > -1; }).map(item => item.idMarca).join(',');
							if (_marcas.length > 0) {
								filter += " or marca in (" + _marcas + ")"
							}
							var _servico = this.state.servicos.filter((item) => { return item.nome.toLowerCase().indexOf(value.toLowerCase()) > -1; }).map(item => item.idServico).join(',');
							if (_servico.length > 0) {
								filter += " or idServico in (" + _servico + ")"
							}
							filter += ")";
						});
					}
					var params = {
						$select: 'idEquipamento,nome,categoria,numSerie,numInventario,numEquipamento,marca,idServico,idRegiao',
						$filter: filter != "" ? filter : null,
						$count: true,
						cancelToken: call.token
					}
					if (typeof sort != 'undefined' && typeof sort[0] != 'undefined' && typeof sort[0].columnName != 'undefined' && typeof sort[0].direction != 'undefined') {
						params['$orderby'] = sort[0].columnName + " " + sort[0].direction;
					}
					this.handleFetchEquipementsRequest(axios.get(`/ordens-de-manutencao/OM1900719`, { params }), isNext);
				});
			}
		});

	}

	handleFetchEquipementsRequest(request, isNext) {
		request.then((result) => {
			var data = result.data;
			this.setTableMarginTop();
			if (data.ordersCountsLines && data.resultLines && data.resultLines.items) {
				var list = data.resultLines.items;
				var nextPageLink = data.resultLines.nextPageLink;
				this.setState({
					maintenanceOrder: data.order,
					marcas: data.marcas,
					servicos: data.servicos,
					equipments: isNext ? this.state.equipments.concat(list) : list,
					equipmentsTotal: data.resultLines.count,
					ordersCountsLines: data.ordersCountsLines,
					equipmentsLinesNext: nextPageLink,
				});
			}
		}).catch(function (error) {
			if (axios.isCancel(error)) {
				console.log('First request canceled', error.message);
			} else {
				console.log(error);
			}
		}).then(() => {
			this.setState({ isLoading: false, equipmentsIsLoading: false });
			setTimeout(() => {
				this.setState({ tooltipReady: true });
				Tooltip.Hidden.hide();
				Tooltip.Hidden.rebuild();
			}, 1200);
		});
	}

	addTechnical() {
		axios.put(`/ordens-de-manutencao/${this.state.orderId}/technicals/logged`).then((result) => {
			console.log('added technical', result);
		}).catch(function (error) {
			console.log(error);
		});
	}

	handleDateChange(e) {
		//if (this.state.calendar.from != this.state.calendar.olderFrom && this.state.calendar.to != this.state.calendar.olderTo) {
		this.setState({
			datePickerOpen: false, isLoading: true, maintenenceOrdersIsLoading: false,
			calendar: { from: this.state.calendar.from, to: this.state.calendar.to, olderFrom: this.state.calendar.from, olderTo: this.state.calendar.to }
		}, () => { /*this.fetchMaintenenceOrders({ ...this.state.calendar }); */ });
		//}
	}

	setDatePickerMarginTop() {
		if (window.outerWidth < 960) {
			var pickerButton = ReactDOM.findDOMNode(this.pickerButton);
			var pickerButtonTop = (pickerButton.getBoundingClientRect().top * 1) - 70;
			this.setState({ datePickerMarginTop: pickerButtonTop + 'px' });
		} else if (this.state.datePickerMarginTop != 0) {
			this.setState({ datePickerMarginTop: 0 });
		}
	}
	render() {
		const { isLoading } = this.state;
		var calendar = this.state.calendar;
		return (
			<PageTemplate >
				<Wrapper padding={'0 0 0'} width="100%" minHeight="120px" ref={el => this.highlightWrapper = el}>
					{this.state.selectionMode ?
						<HeaderSelection count={this.state.selectedRows.length} openEnabled={
							this.state.selectedRows.filter((item, i) => {
								return item.categoriaText == this.state.selectedRows[i == 0 ? 0 : i - 1].categoriaText;
							}).length == this.state.selectedRows.length} onClickOpen={(e) => { }}
							onBackClick={() => {
								this.table.resetSelection();
							}}
							onOpenClick={(e) => {
								console.log(this.state.selectedRows);
								var rows = this.state.selectedRows;

								this.props.history.push(`/ordens-de-manutencao/${this.state.orderId}/ficha-de-manutencao?categoryId=${rows[0].categoria}&equipmentsIds=${rows.map((item) => { return item.idEquipamento }).join(',')}`);
							}}
						/> :
						<div>
							<Header isLoading={this.state.isLoading}
								maintenanceOrder={this.state.maintenanceOrder}
								orderId={this.state.orderId}
								middle={
									<PickerButton ref={el => this.pickerButton = el} picker icon={<Icon calendar />} onClick={(e) => {
										this.setState({ datePickerOpen: !this.state.datePickerOpen }, () => {
											if (!this.state.datePickerOpen) this.handleDateChange(e);
										});
									}}>
										{calendar.from ? calendar.from.format('DD MMM YYYY') : ''} - {calendar.to ? calendar.to.format('DD MMM YYYY') : ''}
									</PickerButton>
								}
							/>
						</div>
					}


					<OmDatePicker
						ref={el => this.datePicker = el}
						open={this.state.datePickerOpen}
						from={calendar.from}
						to={calendar.to}
						onChange={(e) => this.setState({ calendar: { ...this.datePicker.value() } })}
						onCloseClick={this.handleDateChange}
						margintop={this.state.datePickerMarginTop}>
					</OmDatePicker>

				</Wrapper>

				{this.state.listContainerStyle.marginTop &&
					<ListContainer ref={el => this.listContainer = el} style={{ ...this.state.listContainerStyle }} onScroll={this.handleGridScroll} >
						<PivotTable
							onRef={el => this.table = el}
							isLoading={this.state.equipmentsIsLoading}
							rows={this.state.equipments}
							pageSize={30}
							total={this.state.equipmentsTotal}
							rowId={'numEquipamento'}
							columns={[
								{ name: 'estado', title: ' ', sortingEnabled: false, selectionEnabled: false, width: 60, groupingEnabled: false, sortingEnabled: false },
								{ name: 'categoriaText', title: 'Equipamentos', selectionEnabled: false, groupingEnabled: false, defaultExpandedGroup: true, sortingEnabled: false },
								{ name: 'nome', title: 'Nome', dataType: 'bold', groupingEnabled: false, sortingEnabled: true },
								{ name: 'marcaText', title: 'Marca', sortingEnabled: false },
								{ name: 'servicoText', title: 'Serviço', sortingEnabled: false },
								{ name: 'numSerie', title: 'Nº Série', groupingEnabled: false, sortingEnabled: true },
								{ name: 'numInventario', title: 'Nº Inventário', groupingEnabled: false, sortingEnabled: true },
								{ name: 'numEquipamento', title: 'Nº Equipamento', groupingEnabled: false, sortingEnabled: true },
								{ name: 'action', title: ' ', sortingEnabled: false, selectionEnabled: false, width: 60, groupingEnabled: false, sortingEnabled: false },
							]}
							getRows={this.fetchEquipements}
							onRowSelectionChange={(e) => {
								this.setState({ selectionMode: e.selectionMode, selectedRows: e.selectedRows, equipmentsIsLoading: true }, () => {
									this.setState({ equipmentsIsLoading: false })
								});
							}}
							onRowClick={(row) => {
								//this.props.history.push(`/ordens-de-manutencao/${this.state.orderId}/ficha-de-manutencao?categoryId=${row.categoria}&equipmentsIds=${row.idEquipamento}`);
							}}
							groupingEnabled={true}
							searchEnabled={true}
							allowMultiple={true}
						/>
					</ListContainer>
				}
			</PageTemplate>
		)
	}
}

export default withTheme(withRouter(OrdensDeManutencaoLine));