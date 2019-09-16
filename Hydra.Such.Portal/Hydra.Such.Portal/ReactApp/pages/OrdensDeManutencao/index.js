// https://github.com/diegohaz/arc/wiki/Atomic-Design
/*react-styleguide: ignore*/
import React, { Component } from 'react';
import axios from 'axios';
import { PageTemplate } from 'components';
import CircularProgress from '@material-ui/core/CircularProgress';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import _theme from '../../themes/default';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import MuiGrid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import { Button, Text, Icon, Circle, Wrapper, OmDatePicker, CheckBox, Input, Avatars, Modal, Tooltip, PivotTable } from 'components';
import MuiDeleteIcon from '@material-ui/icons/Delete';
import moment from 'moment';
import ReactDOM from 'react-dom';
import Hidden from '@material-ui/core/Hidden';
import { createMuiTheme } from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import MuiTableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import MuiAddIcon from '@material-ui/icons/Add';
import AutoSizer from "react-virtualized-auto-sizer";
import { FixedSizeList as ListWindow } from "react-window";
import Color from 'color';
import Highlighter from "react-highlight-words";
import MuiTextField from '@material-ui/core/TextField';
import MuiInput from '@material-ui/core/Input';
import InputAdornment from '@material-ui/core/InputAdornment';
import { renderToString } from 'react-dom/server';
import { withRouter } from 'react-router';
import { connect } from 'react-redux';
import _ from 'lodash';
import async from 'async';

axios.defaults.headers.post['Accept'] = 'application/json';
axios.defaults.headers.get['Accept'] = 'application/json';

const { DialogTitle, DialogContent, DialogActions } = Modal;

const muiTheme = createMuiTheme();
const breakpoints = muiTheme.breakpoints.values;

injectGlobal`
    body {
        background-color: white;   
    }
    .navbar-container, .navbar-header {
        background-color: ${_theme.palette.secondary.default};
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
        background-color: ${_theme.palette.search} ;
        padding: 0;
    }
`

const Grid = styled(MuiGrid)`
    position: relative;

`

const TableCell = styled(MuiTableCell)` && { 
        padding: 15px 24px 15px 24px;
        font-size: inherit;
        color: ${_theme.palette.primary.default};
        white-space: nowrap;
        text-overflow: ellipsis;
        border: none;
        max-width: 12vw;
        overflow: hidden;
        p {
            margin: 0;
        }
    }
`;

const fetchTechnicals = ({ orderId, technicalId, local }, cb) => {
	cb = cb || function () { };
	axios.get('/ordens-de-manutencao/technicals', {
		params: { orderId, technicalId, local }
	})
		.then(function (data) {
			cb(null, data);
		}).catch(function (error) {
			cb(error);
		});
}

const PickerButton = styled(Button)` && {
        position: relative;
        z-index: 100;
        padding-left: 25px;
        padding-right: 25px;
    }
`;

const CircleOm = {
	wrapper: styled.div`
        margin: auto;
        white-space: nowrap;
        ${props => props.primary && css`
        
        `}
    `,
	icon: styled.div`
        font-size: 33px;
        display: inline-block;
        vertical-align: middle;
        padding: 6px;
        > [class*="icon"] {
            text-align: center;
            width: 57px;
            height: 57px;
            display: inline-block;
            background:  ${props => props.background};
            color:  ${props => props.color};
            border-radius: 50%;
            line-height: 56px;
            font-size: 33px;
        }
    `,
	chart: styled.div`
        padding: 6px 6px 0 6px;;
        display: inline-block;
        vertical-align: middle;
        text-align: center;
    `,
};

const CircleOmWrapper = styled(CircleOm.wrapper)`
    @media (max-width: ${breakpoints["sm"] + "px"}) {
        transform: scale(0.8);	
        transform-origin: center;
        position: relative;
    }
  
`;

const PullRight = styled.span`
    float: right;
`

const TextHeader = styled(Text)`
    display: inline-block;
`

const TbleIcon = styled(Icon)`
    font-size: 24px;
`

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
//updateTechnicals({ orderId: "OM1901562", technicalsId: ["42664", "105590", "106624"] });

const ListGridRow = styled(Grid)`  
    padding: 16px 0px;
    cursor: pointer;
    &:hover {
        background-color: ${Color(_theme.palette.primary.keylines).rgb().fade(0.5).toString()};
    }
`

const ListGridItem = styled(Grid)`  
    padding: 10px 25px;
`

const ListGridItemTextOverflow = styled(ListGridItem)`
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    p {
        margin:0;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }
`

const TechnicalsListItem = styled(ListItem)` && {
    padding-left: 0;
    padding-right: 0;
}
`

const TchnicalsCheckBox = styled(CheckBox)` && {
    position: relative;
    left: -5px;
    font-size: 1.8em;
}`

const TechnicalsAvatars = styled(Avatars.Avatars)` && {
    margin-left: 5px;
    margin-right: 15px;
    pointer-events: none;
}`

const TextDiv = styled(Text)``.withComponent('div');

const TextField = styled(MuiInput)` && {
        margin: 0;
        display: block;
        margin: 0 auto;
        display: block;
        &[class*="MuiInputBase-focused"] {
            width: 100%;
            button {
                right: 7px;
                color: ${_theme.palette.secondary.default};
                pointer-events: none;
            }
            &:before {
                border-bottom: 1px solid ${_theme.palette.primary.dark} !important;
            }
            &:after {
                border-bottom: 1px solid ${_theme.palette.primary.dark} !important;
            }
        }
        &:before {
                border-bottom: 0px solid ${_theme.palette.primary.dark} !important;
        }
        &:after {
                border-bottom: 1px solid ${_theme.palette.primary.dark} !important;
        }
        &:hover {
            &:before  {
                border-bottom: 0px solid ${_theme.palette.primary.dark} !important;
            }
        }
       
        input {
            font-family: ${_theme.fonts.primary};
            font-style: normal;
            font-weight: 400;
            font-size: 14px;
            line-height: 24px;
            height: auto;
            padding-left: 10px;
            padding-right: ${props => props.affix ? '40px' : '5px'};
            padding-top: 10px;
            padding-bottom: 10px;
            width: 100%;
            color: transparent;
            text-shadow: 0 0 0 ${props => Color(props.color || _theme.palette.primary.dark).opaquer(-0.1).rgb().toString()};
            letter-spacing: .02em;
            &:focus {
                transition: all 200ms cubic-bezier(0.4, 0, 0.2, 1) .3s;
                color: ${props => props.color || _theme.palette.primary.dark};
                text-shadow: none;
            }
        }
    }
`;

const SearchButton = styled(Button)` && {
        transition: right 200ms cubic-bezier(0.4, 0, 0.2, 1) 0ms;
        background: none;
        position: absolute;
        right: 25px;
        top: 0px;
        color: ${_theme.palette.primary.default};
        [class*="icon"] {
            font-size: 27px;
        }
    }
`;

const SearchWrapper = styled.div`
    position: absolute;
    display: block;
    width: ${props => props.width || '50%'};
    bottom: 0;
    right: 0;
    z-index: 1;
    padding: 0 25px 0;
`;

var timer = 0;

var cancelToken = axios.CancelToken;
var call;

class OrdensDeManutencao extends Component {
	state = {
		isLoading: true,
		client: {
			"id": null,
			"name": null
		},
		calendar: {
			"from": null,
			"to": null,
			"olderFrom": null,
			"olderTo": null
		},
		ordersCounts: {
			preventive: null,
			preventiveToExecute: null,
			curative: null,
			curativeToExecute: null
		},
		tooltipReady: false,
		maintenenceOrders: [],
		maintenenceOrdersFiltered: [],
		maintenenceOrdersIsLoading: true,
		maintenenceOrdersSearchValue: "",
		maintenenceOrdersNext: "",
		maintenanceOrdersTotal: 0,
		technicals: [],
		technicalsFiltered: [],
		technicalsIsLoading: true,
		technicalsSearchValue: "",
		technicalsOpen: false,
		technicalsSelectedOrder: { technicals: [] },
		technicalsSelectedOrderOld: { technicals: [] },
		clients: null,
		institutions: null,
		selectedOrder: "",
		datePickerOpen: false,
		datePickerMarginTop: 0,
		listContainerStyle: {},
		avatarColors: [
			"#990000",
			"#33DDEE",
			"#5533DD",
			"#339900",
			"#cc00cc"
		]
	}

	constructor(props) {
		super(props);
		moment.locale("pt");
		this.fetchMaintenenceOrders = this.fetchMaintenenceOrders.bind(this);

		if (props.state.maintenenceOrders.length > 0) {
			this.state.maintenenceOrders = props.state.maintenenceOrders;
			this.state.ordersCounts = props.state.ordersCounts;
			this.state.maintenenceOrders = props.state.maintenenceOrders;
			this.state.maintenenceOrdersNext = props.state.maintenenceOrdersNext;
			this.state.isLoading = false;
			this.state.maintenenceOrdersIsLoading = false;
		} else {
			//this.fetchMaintenenceOrders();
		}

		this.handleResize = this.handleResize.bind(this);
		this.handleDateChange = this.handleDateChange.bind(this);
		this.filterListByKeysValue = this.filterListByKeysValue.bind(this);
		this.handleTechnicalsClose = this.handleTechnicalsClose.bind(this);
		this.handleFetchMaintenanceRequest = this.handleFetchMaintenanceRequest.bind(this);
	}

	fetchTechnicals({ orderId, technicalId, local }, cb) {
		this.setState({ technicalsIsLoading: true }, () => {
			axios.get('/ordens-de-manutencao/technicals', {
				params: { orderId, technicalId, local }
			}).then((data) => {
				if (data.data && data.data.technicals) {
					this.setState({
						technicals: data.data.technicals,
						technicalsFiltered: this.filterListByKeysValue({ list: data.data.technicals, keys: ['nome'], value: this.state.technicalsSearchValue })
					});
				}

			}).catch((error) => {
				cb ? cb(error) : console.log(error);
			}).then((data) => {
				this.setState({ technicalsIsLoading: false });
			});
		});
	}

	updateTechnicals({ orderId, technicalsId }, cb) {
		this.setState({ technicalsIsLoading: true }, () => {
			cb = cb || function () { };
			axios.put('/ordens-de-manutencao/technicals', { orderId: orderId, technicalsId: technicalsId })
				.then(function (data) {
					cb(null, data);
				}).catch(function (error) {
					this.setState({ technicalsIsLoading: false });
					cb(error);
				});
		});
	}


	fetchMaintenenceOrders({ search, sort, page }, cb) {
		console.log('AAAA', search);
		cb = cb || (() => { });
		var isNext = page > 1;
		this.setState({ maintenenceOrdersIsLoading: true }, () => {

			if (isNext && this.state.maintenenceOrdersNext != "") {
				call = axios.CancelToken.source();
				this.handleFetchMaintenanceRequest(axios.get(this.state.maintenenceOrdersNext, { cancelToken: call.token }), isNext);
			} else {
				if (call) { call.cancel(); }
				call = axios.CancelToken.source();
				this.setState({ maintenenceOrdersNext: "", maintenanceOrders: [], maintenanceOrdersTotal: 0 }, () => {
					async.parallel([
						(cb) => {
							if (this.state.institutions == null) {
								axios.get('/ordens-de-manutencao/institutions').then((result) => {
									this.setState({ institutions: result.data }, () => { cb(null, 1); });
								}).catch(function (error) { cb(null, 1); });
							} else {
								cb(null, 1);
							}
						}, (cb) => {
							if (this.state.clients == null) {
								axios.get('/ordens-de-manutencao/clients').then((result) => {
									this.setState({ clients: result.data }, () => { cb(null, 2); });
								}).catch(function (error) {
									cb(null, 2);
								});
							} else {
								cb(null, 2);
							}
						}, (cb) => {
							var filter = "";
							if (search && search.length > 0) {
								search.map((value, index) => {
									if (index > 0) { filter += " and "; };
									filter += "(contains(description,'" + value + "') or contains(no,'" + value + "') " + getOdataDateFilterExpression(true, 'orderDate', value, "or");
									//filter += "(contains(description,'" + value + "') or contains(no,'" + value + "') or " + getOdataDateFilterExpression('orderDate', value) + " or date(orderDate) eq " + moment(value).format('YYYY-MM-DD') + "";
									var _institutions = this.state.institutions.filter((item) => { return item.name.toLowerCase().indexOf(value.toLowerCase()) > -1; }).map(item => item.id).join(',');
									if (_institutions.length > 0) {
										filter += " or idInstituicaoEvolution in (" + _institutions + ")";
									}
									var _clients = this.state.clients.filter((item) => { return item.name.toLowerCase().indexOf(value.toLowerCase()) > -1; }).map(item => item.id).join(',');
									if (_clients.length > 0) {
										filter += " or idClienteEvolution in (" + _clients + ")";
									}
									filter += ")";
								});
							}
							var params = {
								$select: 'no,description,customerName,orderType,idTecnico1,idTecnico2,idTecnico3,idTecnico4,idTecnico5,orderDate,shortcutDimension1Code,idClienteEvolution,idInstituicaoEvolution,idServicoEvolution',
								$filter: filter == "" ? null : filter,
								$count: true,
								cancelToken: call.token
							}

							if (typeof sort != 'undefined' && typeof sort[0] != 'undefined' && typeof sort[0].columnName != 'undefined' && typeof sort[0].direction != 'undefined') {
								params['$orderby'] = sort[0].columnName + " " + sort[0].direction;
							}

							var request = axios.get('/ordens-de-manutencao', { params });

							cb(null, request);
						}
					], (err, results) => {
						this.handleFetchMaintenanceRequest(results[2]);
					});

				});
			};


		});
	}

	handleFetchMaintenanceRequest(request, isNext) {
		request.then((result) => {
			var data = result.data;
			this.setTableMarginTop();
			if (data.ordersCounts && data.result && data.result.items) {
				var list = data.result.items;
				var nextPageLink = data.result.nextPageLink;
				console.log(data.result.count, isNext);
				this.setState({
					ordersCounts: data.ordersCounts,
					maintenenceOrders: isNext ? this.state.maintenenceOrders.concat(list) : list,
					maintenanceOrdersTotal: data.result.count,
					maintenenceOrdersNext: nextPageLink,
				}, () => {
					this.props.dispatchState(this.state);
				});
			}
		}).catch(function (error) {
		}).then(() => {
			this.setState({ isLoading: false, maintenenceOrdersIsLoading: false });
			setTimeout(() => {
				this.setState({ tooltipReady: true });
				Tooltip.Hidden.hide();
				Tooltip.Hidden.rebuild();
			}, 1200);
		})
	}

	componentDidMount() {
		this.setState({ tooltipReady: true });
		window.addEventListener("resize", this.handleResize);
		this.setDatePickerMarginTop();
		this.setTableMarginTop();
	}


	handleResize() {
		(() => {
			setTimeout(() => {
				this.setDatePickerMarginTop();
				this.setTableMarginTop();
			}, 0)
		})();
	}

	handleDateChange(e) {
		if (this.state.calendar.from != this.state.calendar.olderFrom && this.state.calendar.to != this.state.calendar.olderTo) {
			this.setState({
				datePickerOpen: false, isLoading: true, maintenenceOrdersIsLoading: false,
				calendar: { from: this.state.calendar.from, to: this.state.calendar.to, olderFrom: this.state.calendar.from, olderTo: this.state.calendar.to }
			}, () => { this.fetchMaintenenceOrders({ ...this.state.calendar }); });
		}
	}

	handleTechnicalsOpen(item) {
		Tooltip.Hidden.hide();
		Tooltip.Hidden.rebuild();
		if (this.state.technicalsOpen == false) {
			this.setState({ technicalsOpen: true, technicalsSelectedOrder: item, technicalsSelectedOrderOld: _.cloneDeep(item) });
			this.fetchTechnicals({ orderId: item.no });
		}
	}

	handleTechnicalsClose() {
		this.setState({ technicalsSearchValue: "", technicalsOpen: false, technicals: [], technicalsFiltered: [], technicalsSelectedOrder: { technicals: [] }, technicalsSelectedOrderOld: { technicals: [] } }, () => { Tooltip.Hidden.rebuild(); });
	}

	setTableMarginTop() {
		(() => setTimeout(() => {
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
				})
			}
		}, 100))();
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

	filterListByKeysValue({ list, keys, value }) {
		keys = keys || [];
		value = value || "";
		value = value.toLowerCase();
		let filteredList = list.filter((item) => {
			var find = false;
			keys.map((k) => {
				if (item[k].toLowerCase().search(value) != -1) {
					find = true;
				}
			});
			return find;
		});
		return filteredList;
	}

	render() {
		const { isLoading, ordersCounts, maintenenceOrders, calendar, maintenenceOrdersIsLoading } = this.state;
		return (
			<PageTemplate>
				<Wrapper padding={'0 0 20px'} width="100%">
					<Grid container direction="row" justify="space-between" alignitems="top" spacing={0} maxwidth={'100%'} margin={0} ref={el => this.highlightWrapper = el} padding={"200px"}>
						<Grid item xs>
							<Wrapper padding={'25px 25px 0'}>
								<TextHeader h2>Ordens de Manutenção <br /> <b>Por executar</b></TextHeader>
								<Hidden mdUp >
									<br /> <br />
									<Button icon={<Icon archive />} onClick={() => { this.props.history.push(`/ordens-de-manutencao/arquivo`) }} >Arquivo</Button>
								</Hidden>

							</Wrapper>
						</Grid>
						<Grid container item md={6} xs={12}>
							<Hidden smDown >
								<Wrapper padding={'24px'} textAlign="center" width="100%">
								</Wrapper>
							</Hidden>

							<CircleOmWrapper className={this.state.isLoading ? 'blink' : ''}>
								<CircleOm.icon background={this.state.isLoading ? _theme.palette.primary.keylines : _theme.palette.primary.light} color={this.state.isLoading ? 'white' : _theme.palette.primary.default}>
									<Icon preventiva data-tip="Preventiva" />
								</CircleOm.icon>
								<CircleOm.chart>
									<Circle
										loading={this.state.isLoading} label=""
										strokeValue={this.state.isLoading ? 0 : ordersCounts.preventive}
										trailValue={ordersCounts.curative} strokeIcon={<Icon curativa />}
										trailIcon={<Icon preventiva />} width={191}
										strokeColor={_theme.palette.secondary.default}
										trailColor={_theme.palette.primary.dark} full
									/>
								</CircleOm.chart>

								<CircleOm.icon background={this.state.isLoading ? _theme.palette.primary.keylines : _theme.palette.secondary.default} color={'white'}>
									<Icon curativa data-tip="Curativa" />
								</CircleOm.icon>
							</CircleOmWrapper>
						</Grid>
						<Grid item xs>
							<Hidden smDown>
								<Wrapper padding={'25px'} textAlign="right" smTextAlign="center">
									<PullRight>
										<Button style={{ boxShadow: 'none' }}
											icon={<Icon archive />}
											onClick={() => { this.props.history.push(`/ordens-de-manutencao/arquivo`) }}>
											Arquivo
										</Button>
									</PullRight>
								</Wrapper>
							</Hidden>
						</Grid>

					</Grid>

				</Wrapper>


				{this.state.listContainerStyle.marginTop &&
					<ListContainer ref={el => this.listContainer = el} style={{ ...this.state.listContainerStyle }} >
						<PivotTable
							onRef={el => this.table = el}
							isLoading={this.state.maintenenceOrdersIsLoading}
							rows={this.state.maintenenceOrders}
							pageSize={30}
							total={this.state.maintenanceOrdersTotal}
							rowId={'no'}
							columns={[
								{ name: 'isPreventive', title: 'Tipo', width: 100, dataType: 'isPreventive', sortingEnabled: true, selectionEnabled: true, groupingEnabled: false },
								{ name: 'description', title: 'Descrição', dataType: 'bold', sortingEnabled: true, selectionEnabled: true, groupingEnabled: false },
								{ name: 'no', title: 'Nº OM ', sortingEnabled: true, selectionEnabled: true, groupingEnabled: false },
								{ name: 'orderDate', title: 'Data', dataType: 'date', sortingEnabled: true, selectionEnabled: true, groupingEnabled: false },
								{ name: 'clientName', title: 'Cliente', sortingEnabled: false, selectionEnabled: true, groupingEnabled: false },
								{ name: 'institutionName', title: 'Instituição', sortingEnabled: false, selectionEnabled: true, groupingEnabled: false },
								{ name: 'technicals', title: 'Técnicos', sortingEnabled: false, dataType: 'avatars', selectionEnabled: true, groupingEnabled: false },
							]}
							getRows={this.fetchMaintenenceOrders}
							onRowSelectionChange={(e) => {
								this.setState({ selectionMode: e.selectionMode, selectedRows: e.selectedRows, maintenanceOrdersLinesIsLoading: true }, () => {
									this.setState({ maintenanceOrdersLinesIsLoading: false })
								});
							}}
							onRowClick={(row) => {
								if (row.isPreventive) {
									this.props.history.push(`/ordens-de-manutencao/${row.no}`);
								} else {
									this.props.history.push(`/ordens-de-manutencao/${row.no}/curativa?equipmentId=6744`);
								}
							}}
							groupingEnabled={false}
							searchEnabled={true}
							allowMultiple={false}
						/>

						<Tooltip.Hidden id={'oms-tooltip'} />
					</ListContainer>}

				<Modal open={this.state.technicalsOpen}
					onClose={this.handleTechnicalsClose}
					action={<div className="hidden"></div>} >
					<div>
						<DialogTitle>
							<Text h2><Icon tecnico />{"\u00a0"} Editar Técnicos</Text>
							<SearchWrapper>
								<TextField
									inputProps={{ autoComplete: "off" }}
									id="technicals-search"
									onChange={(e) => {
										let search = e.target.value.toLowerCase();
										this.setState({
											technicalsSearchValue: search,
											technicalsFiltered: this.filterListByKeysValue({ list: this.state.technicals, keys: ['nome'], value: search })
										})
									}}
									type="search"
									margin="none"
									endAdornment={
										<InputAdornment position="end" onClick={() => { document.getElementById("technicals-search").focus() }}><SearchButton round boxShadow={"none"} ><Icon search /></SearchButton></InputAdornment>
									}
								/>
							</SearchWrapper>
						</DialogTitle>
						<hr />
						<DialogContent style={{ padding: '0' }}>
							<div style={{ width: '100vw', height: '100vh', maxWidth: '100%', maxHeight: '100%' }}>
								{this.state.technicalsIsLoading ? <ListContainer ref={el => this.technicalsLoadingWrapper = el} style={{ textAlign: 'center', 'zIndex': 10 }} onScroll={this.handleGridScroll}><CircularProgress style={{ position: 'relative', top: '40%', color: _theme.palette.secondary.default }} /></ListContainer> : ''}
								<AutoSizer>
									{({ height, width }) => {
										return (
											<ListWindow
												ref={el => this.technicalsListWrapper = el}
												className="List"
												height={height}
												itemCount={this.state.technicalsFiltered.length}
												itemSize={57}
												width={width}
											>
												{({ index, style }) => {
													var _item = this.state.technicalsFiltered[index];
													var orderTechnicals = this.state.technicalsSelectedOrder.technicals;
													var checked = _.find(orderTechnicals, ['numMec', _item.numMec]);
													return (
														<TechnicalsListItem key={index} style={{ padding: '16px 30px 0 30px', ...style }}>
															<TchnicalsCheckBox disabled={this.state.technicalsSelectedOrder.technicals.length >= 5 && !checked} value={index + ""} checked={checked ? true : false} onChange={(e) => {
																var __item = this.state.technicalsFiltered[e.target.value];
																var __orderTechnicals = this.state.technicalsSelectedOrder.technicals;
																var __numMec = __item.numMec;
																if (e.target.checked) {
																	if (!_.find(__orderTechnicals, ['numMec', __numMec])) {
																		__orderTechnicals.push(__item);
																	}
																} else {
																	_.remove(__orderTechnicals, ['numMec', __numMec]);
																}
																this.setState({ technicalsSelectedOrder: { technicals: __orderTechnicals, ...this.state.technicalsSelectedOrder } }, () => {

																})
															}} />
															<TechnicalsAvatars letter color={this.state.avatarColors[Math.floor(index) % 5]} >{this.getInitials(_item.nome)}</TechnicalsAvatars>
															<Text p><Highlighter searchWords={this.state.technicalsSearchValue.split(" ")} autoEscape={true} textToHighlight={_item.nome}></Highlighter></Text>
														</TechnicalsListItem>
													)
												}}

											</ListWindow>
										)
									}}
								</AutoSizer>
							</div>
						</DialogContent>
						<hr />
						<DialogActions>
							<Button primary onClick={(e) => {
								if (!_.isEqual(this.state.technicalsSelectedOrder, this.state.technicalsSelectedOrderOld)) {
									this.updateTechnicals({ orderId: this.state.technicalsSelectedOrder.no, technicalsId: this.state.technicalsSelectedOrder.technicals.map((t) => t.numMec) }, (err, result) => {
										this.handleTechnicalsClose();
									});
								}
							}}>Guardar</Button>
						</DialogActions>
					</div>
				</Modal>
			</PageTemplate>
		)
	}
}

const mapStateToProps = state => ({
	state: state
})

const mapDispatchToProps = dispatch => ({
	dispatchState: (payload) => dispatch({
		type: "SET_STATE",
		payload: payload
	})
})

const getOdataDateFilterExpression = (monthString = true, field, value, prefix) => {
	if (!value) {
		return "";
	}
	var retval = "";
	value.split(" ").map((val, i) => {

		if (monthString) {
			var months = ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
			var matchedMonth;
			months.map((item, index) => {
				if (item.substring(0, val.length).toLowerCase() == val.toLowerCase()) {
					matchedMonth = index + 1;
				}
			});

			if (matchedMonth) {
				if (i > 0) {
					retval += " and ";
				}
				retval += " month(" + field + ") eq " + matchedMonth + " ";
				return;
			}
		}
		if (!isNaN(val) && val.length == 2) {
			if (i > 0) {
				retval += " and ";
			}
			retval += " day(" + field + ") eq " + val + " " + (!monthString ? " and month(" + field + ") eq " + val + " " : "");
			return;
		}

		if (!isNaN(val) && val.length == 4) {
			if (i > 0) {
				retval += " and ";
			}
			retval += " year(" + field + ") eq " + val + " ";
			return;
		}
	});

	return retval != "" ? prefix + retval : retval;
}

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(withTheme(OrdensDeManutencao)));