import React, {Component} from 'react';
import axios from 'axios';
import {PageTemplate} from 'components';
import styled, {css, theme, injectGlobal, withTheme} from 'styled-components';
import {Wrapper, OmDatePicker, Tooltip, PivotTable, Button, Text} from 'components';
import moment from 'moment';
import ReactDOM from 'react-dom';
import {withRouter} from 'react-router-dom';
import classnames from 'classnames';
import Header from './header';
import HeaderSelection from './headerSelection';
import './index.scss';

axios.defaults.headers.post['Accept'] = 'application/json';
axios.defaults.headers.get['Accept'] = 'application/json';


const ListContainer = styled.div`
    
`
const Hr = styled.hr`
    margin-bottom: 0;
    margin-top: 0;
`;

var cancelToken = axios.CancelToken;
var call;
var headerHeightTimer;
var headerCollapsed = false;

var tableScrollTop = 0;
var headerScrollTop = 0;

class OrdensDeManutencaoLine extends Component {

    state = {
        orderId: "",
        isLoading: true,
        ordersCountsLines: {
            toSigning: 0,
            toExecute: 0,
            executed: 0
        },
        marcas: [],
        servicos: [],
        categorias: [],
        tooltipReady: false,
        maintenanceOrder: {},
        equipments: [],
        equipmentsTotal: 0,
        equipmentsIsLoading: false,
        equipmentsLinesNext: "",
        listContainerStyle: {},
        selectionMode: false,
        selectedRows: [],
        selectedRowsCount: null
    }

    constructor(props) {
        super(props);
        moment.locale("pt");
        this.handleResize = this.handleResize.bind(this);
        this.fetchEquipements = this.fetchEquipements.bind(this);
        this.handleFetchEquipementsRequest = this.handleFetchEquipementsRequest.bind(this);
        this.state.orderId = this.props.match.params.orderid;
        this.addTechnical = this.addTechnical.bind(this);
    }

    componentDidMount() {
        window.addEventListener("resize", this.handleResize);
        this.setTableMarginTop();
        this.addTechnical();
        document.getElementById("basicreactcomponent").addEventListener("scroll", (e) => {
            var height = 200;
            if (headerScrollTop < e.target.scrollTop + 5 /* up */) {
                console.log('up');
                if (!ReactDOM.findDOMNode(this.highlightWrapper).classList.contains("om-details__header--collapsed")) {
                    //ReactDOM.findDOMNode(this.highlightWrapper).classList.add("om-details__header--collapsed");
                }
            } else  /* down */ {
                console.log('down');
                ReactDOM.findDOMNode(this.highlightWrapper).classList.remove("om-details__header--collapsed");
            }
            headerScrollTop = e.target.scrollTop;
            var parallaxHeader = ReactDOM.findDOMNode(this.highlightWrapper);
            var opacity = Math.round((e.target.scrollTop - height) / (-height) * 100) / 100;
            if (parallaxHeader !== null) {
                //if (!ReactDOM.findDOMNode(this.highlightWrapper).classList.contains("om-details__header--selection")) {
                parallaxHeader.style = "will-change: transform; transform: translate3d(0px, " + e.target.scrollTop + "px, 0px);";
                if (document.getElementById('header__actions')) {
                    document.getElementById('header__actions').style = "position: absolute; bottom:" + e.target.scrollTop + "px;";
                }
                //parallaxHeader.style = "will-change: transform; height:  " + e.target.scrollTop + "px; ";
                //}
                //parallaxHeader.style = "will-change: transform; opacity: " + opacity + "; transform: translate3d(0px, " + e.target.scrollTop / 2 + "px, 0px); ";
            }
            //window.teste = parallaxHeader;
            // if (e.target.scrollTop + 10 >= parallaxHeader.offsetHeight) {
            // 	ReactDOM.findDOMNode(this.page).classList.remove("scroll-overlay");
            // } else {
            // 	ReactDOM.findDOMNode(this.page).classList.add("scroll-overlay");
            // }
        });
    }

    shouldComponentUpdate(nextProps, nextState) {
        //return true;
        return nextState.isLoading !== this.state.isLoading ||
            nextState.equipmentsIsLoading !== this.state.equipmentsIsLoading ||
            nextState.equipmentsTotal !== this.state.equipmentsTotal ||
            nextState.listContainerStyle.marginTop !== this.state.listContainerStyle.marginTop ||
            nextState.selectedRows !== this.state.selectedRows ||
            nextState.selectedRowsCount !== this.state.selectedRowsCount ||
            nextState.selectionMode !== this.state.selectionMode ||
            nextState.tooltipReady !== this.state.tooltipReady ||
            nextState.listContainerStyle.height !== this.state.listContainerStyle.height;
    }

    handleResize() {
        setTimeout(() => {
            this.setTableMarginTop();
        }, 0)
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
                height = window.innerHeight - ($('.navbar-container').height() * 1) - 60;

                this.setState({
                    listContainerStyle: {
                        height: height,
                        marginTop: '0px'/*top*/,
                        position: 'relative'
                    }
                }, () => {
                })
            }
        }, 100);
    }

    fetchEquipements({search, sort, page}, cb) {
        cb = cb || (() => {
        });
        var isNext = page > 1;

        this.setState({equipmentsIsLoading: true}, () => {
            if (isNext && this.state.equipmentsLinesNext != "") {
                call = axios.CancelToken.source();
                this.handleFetchEquipementsRequest(axios.get(this.state.equipmentsLinesNext, {cancelToken: call.token}), isNext);
            } else {
                if (call) {
                    call.cancel();
                }
                call = axios.CancelToken.source();
                this.setState({equipmentsLinesNext: "", equipments: [], equipmentsTotal: 0}, () => {
                    var orderId = this.state.orderId;
                    var filter = "";
                    if (search && search.length > 0) {
                        search.map((value, index) => {
                            if (index > 0) {
                                filter += " and "
                            }
                            filter += "(contains(nome,'" + value + "') or contains(numInventario,'" + value + "') or contains(numEquipamento,'" + value + "') or contains(numSerie,'" + value + "')";
                            var _marcas = this.state.marcas.filter((item) => {
                                return item.nome.toLowerCase().indexOf(value.toLowerCase()) > -1;
                            }).map(item => item.idMarca).join(',');
                            if (_marcas.length > 0) {
                                filter += " or marca in (" + _marcas + ")"
                            }
                            var _servico = this.state.servicos.filter((item) => {
                                return item.nome.toLowerCase().indexOf(value.toLowerCase()) > -1;
                            }).map(item => item.idServico).join(',');
                            if (_servico.length > 0) {
                                filter += " or idServico in (" + _servico + ")"
                            }
                            filter += ")";
                        });
                    }
                    var params = {
                        $select: 'idEquipamento,nome,categoria,numSerie,numInventario,numEquipamento,marca,marcaText,idServico,idRegiao',
                        $filter: filter != "" ? filter : null,
                        $count: true,
                        cancelToken: call.token
                    }
                    if (typeof sort != 'undefined' && typeof sort[0] != 'undefined' && typeof sort[0].columnName != 'undefined' && typeof sort[0].direction != 'undefined') {
                        params['$orderby'] = sort[0].columnName + " " + sort[0].direction;
                    }

                    const urlParams = new URLSearchParams(window.location.search);
                    const v = urlParams.get('v');
                    this.handleFetchEquipementsRequest(axios.get(`/ordens-de-manutencao/${orderId}` + (v ? '?v=' + v : ''), {params}), isNext);
                });
            }
        });

    }

    handleFetchEquipementsRequest(request, isNext) {
        // console.log('request', 'isNext', isNext);
        request.then((result) => {
            var data = result.data;
            this.setTableMarginTop();
            if (data.ordersCountsLines && data.resultLines && data.resultLines.items) {
                var list = data.resultLines.items;
                var nextPageLink = data.resultLines.nextPageLink;
                var equipments = isNext ? this.state.equipments.concat(list) : list;
                // console.log('IMP', equipments);
                this.setState({
                    maintenanceOrder: data.order,
                    marcas: data.marcas,
                    servicos: data.servicos,
                    categorias: data.categorias,
                    equipments: equipments,
                    equipmentsTotal: data.resultLines.count,
                    equipmentsLinesNext: nextPageLink,
                    ordersCountsLines: {
                        executed: data.ordersCountsLines.executed,
                        toExecute: data.ordersCountsLines.toExecute,
                        toSigning: data.ordersCountsLines.toSigning
                    }
                });
            }
        }).catch(function (error) {
            if (axios.isCancel(error)) {
                console.log('First request canceled', error.message);
            } else {
                console.log(error);
            }
        }).then(() => {

            this.setState({
                isLoading: false,
                equipmentsIsLoading: false
            });

            setTimeout(() => {
                this.setState({tooltipReady: true});
                Tooltip.Hidden.hide();
                Tooltip.Hidden.rebuild();
            }, 1200);
        });
    }

    addTechnical() {
        axios.put(`/ordens-de-manutencao/${this.state.orderId}/technicals/logged`).then((result) => {
        }).catch(function (error) {
            console.log(error);
        });
    }

    render() {
        const {isLoading} = this.state;
        // console.log(headerCollapsed);
        return (
            <PageTemplate>
                <div ref={el => this.highlightWrapper = el} className={classnames(
                    {
                        "om-details__header": true,
                        "om-details__header--selection": this.state.selectionMode,
                        "om-details__header--collapsed": headerCollapsed
                    }
                )}>
                    {this.state.selectionMode ?
                        <HeaderSelection
                            count={this.state.selectedRows.length} openEnabled={() => {
                            this.state.selectedRows = this.state.selectedRows || [];
                            return this.state.selectedRows.filter((item, i) => {
                                return item.categoriaText == this.state.selectedRows[i == 0 ? 0 : i - 1].categoriaText;
                            }).length == this.state.selectedRows.length;
                        }}
                            onClickOpen={(e) => {
                            }}
                            onBackClick={() => {
                                this.setState({selectionMode: false});
                                this.table.resetSelection();
                            }}
                            onOpenClick={(e) => {
                                var rows = this.state.selectedRows;
                                this.props.history.push(`/ordens-de-manutencao/${this.state.orderId}/ficha-de-manutencao?categoryId=${rows[0].categoria}&equipmentsIds=${rows.map((item) => {
                                    return item.idEquipamento
                                }).join(',')}`);
                            }}
                        /> :
                        <Header isLoading={this.state.isLoading}
                                maintenanceOrder={this.state.maintenanceOrder}
                                executed={this.state.ordersCountsLines.executed}
                                toExecute={this.state.ordersCountsLines.toExecute}
                                toSigning={this.state.ordersCountsLines.toSigning}
                                orderId={this.state.orderId}/>
                    }
                </div>

                {this.state.listContainerStyle.marginTop &&
                <ListContainer ref={el => this.listContainer = el} style={{...this.state.listContainerStyle}}
                               className="om-details__list-container"
                               onScroll={(e) => {
                                   var scrollTop = e.target.scrollTop;
                                   if (/*scrollTop + 50 < tableScrollTop ||*/ scrollTop - 20 <= 0) {
                                       ReactDOM.findDOMNode(this.highlightWrapper).classList.remove("om-details__header--collapsed");
                                       headerCollapsed = false;
                                   } else if (scrollTop > tableScrollTop) {
                                       ReactDOM.findDOMNode(this.highlightWrapper).classList.add("om-details__header--collapsed");
                                       headerCollapsed = true;
                                   }
                                   setTimeout(() => {
                                       Tooltip.Hidden.hide();
                                       Tooltip.Hidden.rebuild();
                                   });
                               }}>
                    <PivotTable
                        onRef={el => this.table = el}
                        isLoading={this.state.equipmentsIsLoading}
                        rows={this.state.equipments}
                        pageSize={150}
                        total={this.state.equipmentsTotal}
                        rowId={'numEquipamento'}
                        columns={[
                            {
                                name: 'estado',
                                title: ' ',
                                sortingEnabled: false,
                                selectionEnabled: false,
                                width: 60,
                                groupingEnabled: false,
                                sortingEnabled: false
                            },
                            {
                                name: 'categoriaText',
                                title: 'Equipamentos',
                                selectionEnabled: false,
                                groupingEnabled: false,
                                defaultExpandedGroup: true,
                                sortingEnabled: false
                            },
                            {
                                name: 'nome',
                                title: 'Nome',
                                dataType: 'bold',
                                groupingEnabled: false,
                                sortingEnabled: true
                            },
                            {name: 'marcaText', title: 'Marca', sortingEnabled: false},
                            {name: 'servicoText', title: 'Serviço', sortingEnabled: false},
                            {name: 'numSerie', title: 'Nº Série', groupingEnabled: false, sortingEnabled: true},
                            {
                                name: 'numInventario',
                                title: 'Nº Inventário',
                                groupingEnabled: false,
                                sortingEnabled: true
                            },
                            {
                                name: 'numEquipamento',
                                title: 'Nº Equipamento',
                                groupingEnabled: false,
                                sortingEnabled: true
                            },
                            {
                                name: 'action',
                                title: ' ',
                                sortingEnabled: false,
                                selectionEnabled: false,
                                width: 60,
                                groupingEnabled: false,
                                sortingEnabled: false
                            },
                        ]}
                        getRows={this.fetchEquipements}
                        onRowSelectionChange={(e) => {
                            this.setState({
                                selectionMode: e.selectionMode,
                                selectedRows: e.selectedRows,
                                selectedRowsCount: e.selectedRows.length
                            }, () => {
                                //this.setState({ equipmentsIsLoading: false })
                            });
                        }}
                        onRowClick={(row) => {
                            this.props.history.push(`/ordens-de-manutencao/${this.state.orderId}/ficha-de-manutencao?categoryId=${row.categoria}&equipmentsIds=${row.idEquipamento}`);
                        }}
                        onGroupRowClick={selection => {
                            var idsMarca, idsServico, idCategoria, idsCategorias;
                            if (selection.categoriaText) {
                                idsCategorias = this.state.categorias.filter((item) => {
                                    return item.nome.toLowerCase() == selection.categoriaText.toLowerCase()
                                }).map(item => item.idCategoria);
                            }
                            if (selection.servicoText) {
                                idsServico = this.state.servicos.filter((item) => {
                                    return item.nome.toLowerCase() == selection.servicoText.toLowerCase()
                                }).map(item => item.idServico).join(',');
                            }
                            if (selection.marcaText) {
                                idsMarca = this.state.marcas.filter((item) => {
                                    return item.nome.toLowerCase() == selection.marcaText.toLowerCase()
                                }).map(item => item.idMarca).join(',');
                            }
                            if (idsCategorias) {
                                idCategoria = idsCategorias[0];
                                var url = `/ordens-de-manutencao/${this.state.orderId}/ficha-de-manutencao?categoryId=${idCategoria}`;
                                if (idsMarca) {
                                    url += `&marcaIds=${idsMarca}`;
                                }
                                if (idsServico) {
                                    url += `&servicoIds=${idsServico}`;
                                }
                                this.props.history.push(url);
                            }
                        }}
                        groupingEnabled={true}
                        searchEnabled={true}
                        allowMultiple={true}
                        noData={this.state.ordersCountsLines.executed + this.state.ordersCountsLines.toExecute == 0 &&
                        <td style={{
                            position: "absolute",
                            top: 0,
                            left: 0,
                            right: 0,
                            bottom: 0,
                            textAlign: 'center'
                        }}>
                            <div style={{
                                display: 'inline-block',
                                top: '40%',
                                margin: 'auto',
                                position: 'relative'
                            }}>
                                <Text p style={{
                                    color: this.props.theme.palette.primary.default,
                                    marginBottom: '5px'
                                }}>OM sem equipamentos</Text>
                                <Button primary>Ficha de Manutenção</Button>
                            </div>
                        </td>
                        }
                    />
                </ListContainer>
                }
            </PageTemplate>
        )
    }
}

export default withTheme(withRouter(OrdensDeManutencaoLine));