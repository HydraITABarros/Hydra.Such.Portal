import React, {Component} from 'react';
import MuiTabs from '@material-ui/core/Tabs';
import MuiTab from '@material-ui/core/Tab';
import styled, {css, theme, injectGlobal, withTheme} from 'styled-components';
import {
    Button,
    Text,
    Icon,
    Circle,
    Wrapper,
    OmDatePicker,
    CheckBox,
    Input,
    Avatars,
    ModalLarge,
    Tooltip,
    Spacer
} from 'components';
import AppBar from '@material-ui/core/AppBar';
import {Grid} from '@material-ui/core';
import functions from '../../../../helpers/functions';
import _theme from '../../../../themes/default';
import ReactDOM from 'react-dom';
import SplitedReport from './splitedReport';
import ReportFooter from './reportFooter';
import './index.scss';

const {DialogTitle, DialogContent, DialogActions} = ModalLarge;

var timeout = 0;
var refAux = 0;

class Report extends Component {
    state = {
        splited: false,
        selectedEquipments: [],
        report: [],
        refsHeader: [],
        refsMaintenance: [],
        refsQuality: [],
        refsQuantity: [],
        refsComments: [],
        refsFooter: []
    }

    constructor(props) {
        super(props);
        this.resetRefs = this.resetRefs.bind(this);

        this.state.selectedEquipments = props.selectedEquipments;
    }

    resetRefs() {
        //return;
        this.state.refsHeader = [];
        this.state.refsMaintenance = [];
        this.state.refsQuality = [];
        this.state.refsQuantity = [];
        this.state.refsComments = [];
        this.state.refsFooter = [];
    }

    componentDidMount() {
        this.resetRefs();
        this.state.selectedEquipments = this.props.selectedEquipments;
        setTimeout(() => {
            this.setState({splited: true});
        }, 100);
    }

    componentDidUpdate() {
        refAux = 0;
        //this.resetRefs();
    }

    buildReportWidthPagination() {

    }

    componentWillUnmount() {
        this.state.selectedEquipments = [];
        this.resetRefs();
    }


    componentWillReceiveProps(nextProps) {
        if (nextProps.open !== this.state.open) {
            this.setState({open: nextProps.open});
        }

        if (nextProps.selectedEquipments !== this.state.selectedEquipments) {
            this.resetRefs();
            var state = this.state;
            state.selectedEquipments = nextProps.selectedEquipments;
            //state.splited = false;
            this.setState(state);
            // setTimeout(() => {
            //     this.setState({splited: true});
            // }, 0);
        }
    }

    render() {
        refAux = 0;

        var order = this.props.order || {};
        var maintenance, quality, quantity;
        if (this.props.$equipments && this.props.$equipments.value[0]) {
            maintenance = this.props.$equipments.value[0].planMaintenance;
            quality = this.props.$equipments.value[0].planQuality;
            quantity = this.props.$equipments.value[0].planQuantity;
        }

        var date = (this.state.selectedEquipments[this.state.selectedEquipments.length - 1] &&
            this.state.selectedEquipments[this.state.selectedEquipments.length - 1].dataRelatorio) &&
            this.state.selectedEquipments[this.state.selectedEquipments.length - 1].dataRelatorio;

        var maintenanceResponsible = (this.props.order && this.props.order.maintenanceResponsibleObj && this.props.order.maintenanceResponsibleObj.nome);
        var responsibleEmployee = (this.props.order && this.props.order.responsibleEmployeeObj && this.props.order.responsibleEmployeeObj.nome);
        return (
            <div>
                <div className="report__wrapper report__wrapper--raw">
                    <div className="report">

                        <div className="report__page report__page--master">

                            <div className="report__header">
                                <div className="col-xs-5">
                                    <img src="/images/such-engenharia.png" alt="Such Engenharia"
                                         className="img-responsive"/>
                                </div>
                                <div className="col-xs-7 padding-0">
                                    {maintenanceResponsible &&
                                    <div className="col-xs-6 report__header__avatar__wrapper">
                                        Chefe de Projecto
                                        <Avatars.Avatars
                                            className="report__header__avatar"
                                            letter
                                            color={_theme.palette.primary.light} data-tip={"nome"}
                                        >
                                            {functions.getInitials(maintenanceResponsible)}
                                        </Avatars.Avatars>
                                    </div>
                                    }

                                    {responsibleEmployee &&
                                    <div className="col-xs-6 report__header__avatar__wrapper">
                                        Resp. Manutenção
                                        <Avatars.Avatars
                                            className="report__header__avatar"
                                            letter
                                            color={_theme.palette.primary.light} data-tip={"nome"}
                                        >
                                            {functions.getInitials(responsibleEmployee)}
                                        </Avatars.Avatars>
                                    </div>
                                    }

                                </div>
                                <div className="clearfix"></div>
                            </div>

                            <div className="report__title">
                                <Text p>
                                    Relatório de Manutenção {(date && "- " + date)}
                                </Text>
                                <Text p className="report__page-counter">1/3</Text>
                                <Text h1 className="f-s-40"><Icon preventiva/>{this.props.equipmentType}
                                </Text>
                            </div>

                            <div className="report__body report__body--header">

                                <div className="report__row__el"
                                     ref={(el) => this.state.refsHeader[refAux++] = el}>
                                    <div className="report__spacer--45"></div>
                                    {/* Cliente */}
                                    <div className="col-xs-2">
                                        <Text b className="report__label">Cliente</Text>
                                    </div>
                                    <div className="col-xs-10">
                                        <Text p className="report__text">{order.customerName}</Text>
                                    </div>
                                    <div className="clearfix"></div>
                                </div>
                                <div className="report__row__el"
                                     ref={(el) => this.state.refsHeader[refAux++] = el}>
                                    {/* Instituição */}
                                    <div className="col-xs-2">
                                        <Text b className="report__label">Instituição</Text>
                                    </div>
                                    <div className="col-xs-10">
                                        <Text p className="report__text">{order.institutionName}</Text>
                                    </div>
                                    <div className="clearfix"></div>
                                </div>

                                {/* Serviço */}
                                {order.serviceName &&
                                <div className="report__row__el"
                                     ref={(el) => this.state.refsHeader[refAux++] = el}>
                                    <div>
                                        <div className="col-xs-2">
                                            <Text b className="report__label">Serviço</Text>
                                        </div>
                                        <div className="col-xs-10">
                                            <Text p className="report__text">{order.serviceName}</Text>
                                        </div>
                                        <div className="clearfix"></div>
                                    </div>
                                </div>
                                }

                                <div className="report__row__el"
                                     ref={(el) => this.state.refsHeader[refAux++] = el}>
                                    <div className="report__spacer--35"></div>
                                    <div className="col-xs-2">
                                        <Text b className="report__label">Tipo de Ordem</Text></div>
                                    <div className="col-xs-10">
                                        <Text p className="report__text">{order.orderType}</Text>
                                    </div>
                                    <div className="clearfix"></div>
                                </div>

                                <div className="report__row__el"
                                     ref={(el) => this.state.refsHeader[refAux++] = el}>
                                    <div className="report__spacer--35"></div>
                                    <div className="col-xs-2">
                                        <Text b className="report__label">#Marca</Text></div>
                                    <div className="col-xs-2">
                                        <Text b className="report__label">Modelo</Text>
                                    </div>
                                    <div className="col-xs-2">
                                        <Text b className="report__label">NºSérie</Text></div>
                                    <div className="col-xs-2">
                                        <Text b className="report__label">NºEquip.</Text></div>
                                    <div className="col-xs-2">
                                        <Text b className="report__label">Nº Inventário</Text></div>
                                    <div className="clearfix"></div>
                                </div>

                                {this.state.selectedEquipments.map((equipment, i) => {
                                    return (
                                        <div key={i} className="report__row__el"
                                             ref={(el) => this.state.refsHeader[refAux++] = el}>
                                            <div>
                                                <div className="col-xs-2 ws-nowrap">
                                                    <Text p className="report__text">
                                                        {i + 1}&nbsp; {equipment.marcaText}
                                                    </Text>
                                                </div>
                                                <div className="col-xs-2 ws-nowrap">
                                                    <Text p className="report__text">
                                                        {equipment.modeloText}
                                                    </Text>
                                                </div>
                                                <div className="col-xs-2 ws-nowrap">
                                                    <Text p className="report__text">
                                                        {equipment.numSerie}
                                                    </Text>
                                                </div>
                                                <div className="col-xs-2 ws-nowrap">
                                                    <Text p className="report__text">
                                                        {equipment.numEquipamento}
                                                    </Text>
                                                </div>
                                                <div className="col-xs-2 ws-nowrap">
                                                    <Text p className="report__text">
                                                        {equipment.numInventario}
                                                    </Text>
                                                </div>
                                                <div className="clearfix"></div>
                                            </div>
                                            {(i + 1) == this.state.selectedEquipments.length &&
                                            <div className="report__spacer--35"></div>
                                            }
                                        </div>
                                    )
                                })}

                                <div className="report__row__el"
                                     ref={(el) => this.state.refsHeader[refAux++] = el}>
                                    {/* Material Aplicado To Do */}
                                    <div className="col-xs-1">
                                        <Text b className="report__label">Equip.</Text>
                                    </div>
                                    <div className="col-xs-5">
                                        <Text b className="report__label">Material Aplicado</Text>
                                    </div>
                                    <div className="col-xs-2">
                                        <Text b className="report__label">Qtd.</Text>
                                    </div>
                                    <div className="col-xs-4">
                                        <Text b className="report__label">Fornecido por</Text>
                                    </div>

                                    <div className="col-xs-1 ws-nowrap">
                                        <Text p className="report__text"> - </Text>
                                    </div>
                                    <div className="col-xs-5 ws-nowrap">
                                        <Text p className="report__text"> - </Text>
                                    </div>
                                    <div className="col-xs-2 ws-nowrap">
                                        <Text p className="report__text"> - </Text>
                                    </div>
                                    <div className="col-xs-4 ws-nowrap">
                                        <Text p className="report__text"> - </Text>
                                    </div>

                                    <div className="clearfix"></div>
                                    <div className="report__spacer--45"></div>
                                </div>
                            </div>

                            <div className="report__hr"></div>

                            <div className="report__body report__body--content">
                                {maintenance &&
                                <div>
                                    <div className="report__row__el"
                                         ref={(el) => this.state.refsMaintenance[refAux++] = el}>
                                        <div className="report__spacer--40"></div>
                                        <div className="report__row">
                                            <div className="col-xs-7">
                                                <Text b className="report__label">Manutenção</Text>
                                            </div>
                                            {this.state.selectedEquipments.map((equipments, index) => {
                                                return (
                                                    <div key={index}
                                                         className={"col-xs-" + (this.state.selectedEquipments.length / 5) + " text-center"}>
                                                        <Text b className="report__label">
                                                            #{index + 1}
                                                        </Text>
                                                    </div>
                                                )
                                            })}
                                            <div className="clearfix"></div>
                                        </div>
                                    </div>
                                    {maintenance.map((item, index) => {
                                        return (
                                            <div key={index} className="report__row__el"
                                                 ref={(el) => this.state.refsMaintenance[refAux++] = el}>
                                                <div className="report__hr"></div>
                                                <div className="report__row">
                                                    <div className="col-xs-7">
                                                        <Text p className="report__text">
                                                            {item.descricao}
                                                        </Text>
                                                    </div>
                                                    {this.state.selectedEquipments.map((equipment, i) => {
                                                        return (
                                                            <div key={index + "" + i}
                                                                 className={"col-xs-" + (this.state.selectedEquipments.length / 5) + " text-center"}>
                                                                <Text p className="report__text">
                                                                    {equipment.planMaintenance[index].$resultado.value == 1 &&
                                                                    <Icon approved/>}
                                                                    {equipment.planMaintenance[index].$resultado.value == 2 &&
                                                                    <Icon remove
                                                                          className={"f-s-32 l-h-0"}/>}
                                                                    {equipment.planMaintenance[index].$resultado.value == 3 &&
                                                                    <Icon decline className={"f-s-24"}/>}
                                                                    {equipment.planMaintenance[index].$resultado.value == 0 &&
                                                                    <Icon/>}
                                                                </Text>
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

                                {quality &&
                                <div>
                                    <div className="report__row__el"
                                         ref={(el) => this.state.refsQuality[refAux++] = el}>
                                        <div className="report__spacer"></div>
                                        <div className="report__row">
                                            <div className="col-xs-7">
                                                <Text b className="report__label">Qualitativos</Text>
                                            </div>
                                            {this.state.selectedEquipments.map((equipments, index) => {
                                                return (
                                                    <div key={index}
                                                         className={"col-xs-" + (this.state.selectedEquipments.length / 5) + " text-center"}>
                                                        <Text b className="report__label">
                                                            #{index + 1}
                                                        </Text>
                                                    </div>
                                                )
                                            })}
                                            <div className="clearfix"></div>
                                        </div>
                                    </div>
                                    {quality.map((item, index) => {
                                        return (
                                            <div key={index} className="report__row__el"
                                                 ref={(el) => this.state.refsQuality[refAux++] = el}>
                                                <div className="report__hr"></div>
                                                <div className="report__row">
                                                    <div className="col-xs-7">
                                                        <Text p className="report__text">
                                                            {item.descricao}
                                                        </Text>
                                                    </div>
                                                    {this.state.selectedEquipments.map((equipment, i) => {
                                                        return (
                                                            <div key={index + "" + 1}
                                                                 className={"col-xs-" + (this.state.selectedEquipments.length / 5) + " text-center"}>
                                                                <Text p className="report__text">
                                                                    {equipment.planQuality[index].$resultado.value == 1 &&
                                                                    <Icon approved/>}
                                                                    {equipment.planQuality[index].$resultado.value == 2 &&
                                                                    <Icon remove
                                                                          className={"f-s-32 l-h-0"}/>}
                                                                    {equipment.planQuality[index].$resultado.value == 3 &&
                                                                    <Icon decline className={"f-s-24"}/>}
                                                                    {equipment.planQuality[index].$resultado.value == 0 &&
                                                                    <Icon/>}
                                                                </Text>
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

                                {quantity &&
                                <div>
                                    <div className="report__row__el"
                                         ref={(el) => this.state.refsQuality[refAux++] = el}>
                                        <div className="report__spacer"></div>
                                        <div className="report__row">
                                            <div className="col-xs-6">
                                                <Text b className="report__label">Quantitativos</Text>
                                            </div>
                                            <div className="col-xs-1 text-center">
                                                <Text b className="report__label"></Text>
                                            </div>
                                            {this.state.selectedEquipments.map((equipments, index) => {
                                                return (
                                                    <div key={index}
                                                         className={"col-xs-" + (this.state.selectedEquipments.length / 5) + " text-center"}>
                                                        <Text b className="report__label">
                                                            #{index + 1}
                                                        </Text>
                                                    </div>
                                                )
                                            })}
                                            <div className="clearfix"></div>
                                        </div>
                                    </div>
                                    {quantity.map((item, index) => {
                                        return (
                                            <div key={index} className="report__row__el"
                                                 ref={(el) => this.state.refsQuality[refAux++] = el}>

                                                <div className="report__hr"></div>
                                                <div className="report__row">
                                                    <div className="col-xs-6">
                                                        <Text p className="report__text">
                                                            {item.descricao}
                                                        </Text>
                                                    </div>
                                                    <div className="col-xs-1 text-center">
                                                        <Text p
                                                              className="report__text color-primary-light">
                                                            {item.unidadeCampo1}
                                                        </Text>
                                                    </div>
                                                    {this.state.selectedEquipments.map((equipment, i) => {
                                                        return (
                                                            <div key={index + "" + i}
                                                                 className={"col-xs-" + (this.state.selectedEquipments.length / 5) + " text-center"}>
                                                                <Text p className="report__text">
                                                                    {equipment.planQuantity[index].resultado}
                                                                </Text>
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

                                <div className="report__row__el"
                                     ref={(el) => this.state.refsComments[refAux++] = el}>
                                    <div className="report__spacer"></div>
                                    <div className="report__row">
                                        <div className="col-xs-12">
                                            <Text b className="report__label">Observações</Text>
                                        </div>
                                        <div className="clearfix"></div>
                                    </div>
                                    <div className="report__hr"></div>
                                </div>
                                {this.state.selectedEquipments.length > 0 && this.state.selectedEquipments.map((equipment, i) => {
                                    var printed = false;
                                    return (
                                        <div key={i} className="report__row__el"
                                             ref={(el) => this.state.refsComments[refAux++] = el}>
                                            {(equipment.observacao && equipment.observacao != "") &&
                                            <div>
                                                {(!printed) && (printed = true) &&
                                                <div className="col-xs-12 p-t-10">
                                                    <Text b className="report__text m-b-5">Geral</Text>
                                                </div>
                                                }
                                                <div className="col-xs-1">
                                                    <Text p
                                                          className="report__text color-primary-light">#{i + 1}</Text>
                                                </div>
                                                <div className="col-xs-11 p-l-0">
                                                    <Text p className="report__text">
                                                        {equipment.observacao}
                                                    </Text>
                                                </div>
                                                <div className="clearfix"></div>
                                            </div>}
                                        </div>
                                    )
                                })}

                                {(maintenance) && maintenance.map((item, index) => {
                                    var printed = false;
                                    return (
                                        <div key={index}>
                                            {this.state.selectedEquipments.map((equipment, i) => {
                                                return (
                                                    <div key={index + "" + i} className="report__row__el"
                                                         ref={(el) => this.state.refsComments[refAux++] = el}>
                                                        {(equipment.planMaintenance[index].observacoes && equipment.planMaintenance[index].observacoes != "") &&
                                                        <div>
                                                            {!printed && (() => {
                                                                printed = true;
                                                                return (
                                                                    <div className="col-xs-12 p-t-10">
                                                                        <Text b
                                                                              className="report__text m-b-5">
                                                                            {item.descricao}
                                                                        </Text>
                                                                    </div>
                                                                )
                                                            })()}
                                                            <div className="col-xs-1">
                                                                <Text p
                                                                      className="report__text color-primary-light">
                                                                    #{i + 1}
                                                                </Text>
                                                            </div>
                                                            <div className="col-xs-11 p-l-0">
                                                                <Text p className="report__text">
                                                                    {equipment.planMaintenance[index].observacoes}
                                                                </Text>
                                                            </div>
                                                            <div className="clearfix"></div>
                                                        </div>
                                                        }
                                                    </div>
                                                )
                                            })}
                                        </div>
                                    )
                                })}

                                {(quality) && quality.map((item, index) => {
                                    var printed = false;
                                    return (
                                        <div key={index}>
                                            {this.state.selectedEquipments.map((equipment, i) => {
                                                return (
                                                    <div key={index + "" + i} className="report__row__el"
                                                         ref={(el) => this.state.refsComments[refAux++] = el}>
                                                        {(equipment.planQuality[index].observacoes && equipment.planQuality[index].observacoes != "") &&
                                                        <div>
                                                            {!printed && (() => {
                                                                printed = true;
                                                                return (
                                                                    <div className="col-xs-12 p-t-10">
                                                                        <Text b
                                                                              className="report__text m-b-5">
                                                                            {item.descricao}
                                                                        </Text>
                                                                    </div>
                                                                )
                                                            })()}
                                                            <div className="col-xs-1">
                                                                <Text p
                                                                      className="report__text color-primary-light">
                                                                    #{i + 1}
                                                                </Text>
                                                            </div>
                                                            <div className="col-xs-11 p-l-0">
                                                                <Text p className="report__text">
                                                                    {equipment.planQuality[index].observacoes}
                                                                </Text>
                                                            </div>
                                                            <div className="clearfix"></div>
                                                        </div>}
                                                    </div>
                                                )
                                            })}
                                        </div>
                                    )
                                })}

                                {(quantity) && quantity.map((item, index) => {
                                    var printed = false;
                                    return (
                                        <div key={index}>
                                            {this.state.selectedEquipments.map((equipment, i) => {
                                                return (
                                                    <div key={index + "" + i} className="report__row__el"
                                                         ref={(el) => this.state.refsComments[refAux++] = el}>
                                                        {(equipment.planQuantity[index].observacoes && equipment.planQuantity[index].observacoes != "") &&
                                                        <div>
                                                            {!printed && (() => {
                                                                printed = true;
                                                                return (
                                                                    <div className="col-xs-12 p-t-10">
                                                                        <Text b
                                                                              className="report__text m-b-5">
                                                                            {item.descricao}
                                                                        </Text>
                                                                    </div>
                                                                )
                                                            })()}
                                                            <div className="col-xs-1">
                                                                <Text p
                                                                      className="report__text color-primary-light">
                                                                    #{i + 1}
                                                                </Text>
                                                            </div>
                                                            <div className="col-xs-11 p-l-0">
                                                                <Text p className="report__text">
                                                                    {equipment.planQuantity[index].observacoes}
                                                                </Text>
                                                            </div>
                                                            <div className="clearfix"></div>
                                                        </div>
                                                        }
                                                    </div>
                                                )
                                            })}
                                        </div>
                                    )
                                })}

                                <div className="report__row__el"
                                     ref={(el) => this.state.refsComments[refAux++] = el}>
                                    <div className="report__spacer--35"></div>
                                    <div className="report__spacer--35"></div>
                                </div>

                            </div>
                            <div className="report__hr"></div>

                            <div className="report__row__el"
                                 ref={(el) => {
                                     this.state.refsFooter[refAux++] = el;
                                 }}>
                                <ReportFooter
                                    date={date}
                                    assinaturaTecnico={
                                        this.state.selectedEquipments[0] &&
                                        this.state.selectedEquipments[0].$assinaturaTecnico ?
                                            this.state.selectedEquipments[0].$assinaturaTecnico.value : null
                                    }
                                    assinaturaCliente={
                                        this.state.selectedEquipments[0] &&
                                        this.state.selectedEquipments[0].$assinaturaCliente ?
                                            this.state.selectedEquipments[0].$assinaturaCliente.value : null}
                                    assinaturaSie={
                                        this.state.selectedEquipments[0] &&
                                        this.state.selectedEquipments[0].$assinaturaSie ?
                                            this.state.selectedEquipments[0].$assinaturaSie.value : null}
                                    technical={
                                        this.state.selectedEquipments[0] &&
                                        this.state.selectedEquipments[0].$utilizadorAssinaturaTecnico ?
                                            this.state.selectedEquipments[0].$utilizadorAssinaturaTecnico.value : null}
                                />
                            </div>
                        </div>

                    </div>
                </div>
                {this.state.splited &&
                <SplitedReport
                    order={this.props.order}
                    refsHeader={this.state.refsHeader}
                    refsMaintenance={this.state.refsMaintenance}
                    refsQuality={this.state.refsQuality}
                    refsQuantity={this.state.refsQuantity}
                    refsComments={this.state.refsComments}
                    refsFooter={this.state.refsFooter}
                    equipmentType={this.props.equipmentType}
                    date={date}
                />
                }
            </div>
        )
    }
}

window.makePDF = function (pageTotal, fileName) {
    var quotes = document.getElementById('report');
    pageTotal = pageTotal - 1;
    html2canvas(quotes, {
        scale: 1,
        dpi: 300
    }).then((canvas) => {
        //! MAKE YOUR PDF
        var pdf = new jsPDF('p', 'pt', 'a4');

        console.log(quotes.clientHeight - 15);

        for (var i = 0; i <= pageTotal; i++) {
            //! This is all just html2canvas stuff
            var srcImg = canvas;
            var sX = 0;
            var sY = 1123 * i; // start 980 pixels down for every new page
            var sWidth = 794;
            var sHeight = 1123;
            var dX = 0;
            var dY = 0;
            var dWidth = 794;
            var dHeight = 1123;

            window.onePageCanvas = document.createElement("canvas");
            //resolution
            onePageCanvas.setAttribute('width', 794);
            onePageCanvas.setAttribute('height', 1123);
            var ctx = onePageCanvas.getContext('2d');
            // details on this usage of this function:
            // https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API/Tutorial/Using_images#Slicing
            ctx.drawImage(srcImg, sX, sY, sWidth, sHeight, dX, dY, dWidth, dHeight);

            // document.body.appendChild(canvas);
            var canvasDataURL = onePageCanvas.toDataURL("image/png", .5);

            var width = onePageCanvas.width;
            var height = onePageCanvas.clientHeight;

            //! If we're on anything other than the first page,
            // add another page
            if (i > 0) {
                //pdf.addPage(612, 791); //8.5" x 11" in pts (in*72)
                pdf.addPage(595, 842); //8.5" x 11" in pts (in*72)
                //pdf.addPage(794, 1123); //8.5" x 11" in pts (in*72)
                //pdf.addPage(1240, 1754); //8.5" x 11" in pts (in*72)
            }
            //! now we declare that we're working on that page
            pdf.setPage(i + 1);
            //! now we add content to that page!
            pdf.addImage(canvasDataURL, 'PNG', 0, 0, (width * .75), (height * .75), null, 'SLOW');

        }
        //! after the for loop is finished running, we save the pdf.
        pdf.save(fileName + '.pdf' || 'File.pdf');
    });
}

export default Report;