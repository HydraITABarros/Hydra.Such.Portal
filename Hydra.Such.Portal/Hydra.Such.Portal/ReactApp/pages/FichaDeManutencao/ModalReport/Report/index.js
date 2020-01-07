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
        refsSimplified: [],
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
        this.state.refsSimplified = [];
        this.state.refsMaintenance = [];
        this.state.refsQuality = [];
        this.state.refsQuantity = [];
        this.state.refsComments = [];
        this.state.refsFooter = [];
    }

    componentDidMount() {
        //this.resetRefs();
        this.state.selectedEquipments = this.props.selectedEquipments;

        this.setState({splited: true});

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
        }
    }

    render() {
        refAux = 0;

        console.log(this.props);

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
                                    <div className="report__spacer--20"></div>
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
                                {this.state.selectedEquipments.filter(item => item.idEquipamento != 0).length > 0 ?
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
                                    </div> :
                                    <div className="report__spacer--35"
                                         ref={(el) => this.state.refsHeader[refAux++] = el}> &nbsp;</div>
                                }
                                {this.state.selectedEquipments.filter(item => item.idEquipamento != 0).map((equipment, i) => {
                                    return (
                                        <div key={i} className="report__row__el"
                                             ref={(el) => this.state.refsHeader[refAux++] = el}>
                                            <div>
                                                <div className="col-xs-2 ws-nowrap to-ellipsis">
                                                    <Text p className="report__text">
                                                        {i + 1}&nbsp; {equipment.marcaText}
                                                    </Text>
                                                </div>
                                                <div className="col-xs-2 ws-nowrap to-ellipsis">
                                                    <Text p className="report__text">
                                                        {equipment.modeloText}
                                                    </Text>
                                                </div>
                                                <div className="col-xs-2 ws-nowrap to-ellipsis">
                                                    <Text p className="report__text">
                                                        {equipment.numSerie}
                                                    </Text>
                                                </div>
                                                <div className="col-xs-2 ws-nowrap to-ellipsis">
                                                    <Text p className="report__text">
                                                        {equipment.numEquipamento}
                                                    </Text>
                                                </div>
                                                <div className="col-xs-2 ws-nowrap to-ellipsis">
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
                                {this.state.selectedEquipments.filter(e => {
                                    return e.materials.length > 0;
                                }).length > 0 &&
                                <div className="report__row__el"
                                     ref={(el) => this.state.refsHeader[refAux++] = el}>
                                    {/* Material Aplicado To Do */}
                                    <div className="col-xs-2">
                                        <Text b className="report__label">Equip.</Text>
                                    </div>
                                    <div className="col-xs-4">
                                        <Text b className="report__label">Material Aplicado</Text>
                                    </div>
                                    <div className="col-xs-2">
                                        <Text b className="report__label">Qtd.</Text>
                                    </div>
                                    <div className="col-xs-4">
                                        <Text b className="report__label">Fornecido por</Text>
                                    </div>

                                    {this.state.selectedEquipments.map((equipment, i) => {
                                        return (
                                            equipment.materials.map((material, j) => {
                                                return (
                                                    <div key={i + "-" + j}>
                                                        <div className="col-xs-2 ws-nowrap to-ellipsis">
                                                            <Text p
                                                                  className="report__text"> {equipment.numEquipamento || "-"} </Text>
                                                        </div>
                                                        <div className="col-xs-4 ws-nowrap to-ellipsis">
                                                            <Text p
                                                                  className="report__text"> {material.descricao} </Text>
                                                        </div>
                                                        <div className="col-xs-2 ws-nowrap to-ellipsis">
                                                            <Text p
                                                                  className="report__text"> {material.quantidade} </Text>
                                                        </div>
                                                        <div className="col-xs-4 ws-nowrap to-ellipsis">
                                                            <Text p
                                                                  className="report__text"> {material.fornecidoPor == 1 ? "Cliente" : "Such"} </Text>
                                                        </div>
                                                    </div>)
                                            })
                                        )
                                    })}


                                    <div className="clearfix"></div>
                                    <div className="report__spacer--20"></div>
                                </div>
                                }
                            </div>

                            <div className="report__hr"></div>

                            <div className="report__body report__body--content">

                                {(this.props.isCurative || this.props.isSimplified) &&
                                <div>

                                    <div className="report__row__el"
                                         ref={(el) => this.state.refsSimplified[refAux++] = el}
                                         key={'customerRequest_first'}>
                                        <div className="report__spacer--40"></div>
                                    </div>
                                    {this.state.selectedEquipments.length > 0 && this.state.selectedEquipments[0].customerRequest != "" && this.state.selectedEquipments[0].customerRequest != null &&
                                    <div className="report__row__el"
                                         ref={(el) => this.state.refsSimplified[refAux++] = el}
                                         key={'customerRequest'}>
                                        <div className="report__row">
                                            <div className="col-xs-2">
                                                <Text b className="report__label">Pedido do Cliente</Text></div>
                                            <div className="col-xs-10">
                                                <Text p
                                                      className="report__text">{this.state.selectedEquipments.length > 0 && this.state.selectedEquipments[0].customerRequest}</Text>
                                            </div>
                                            <div className="clearfix"></div>
                                        </div>
                                    </div>
                                    }

                                    {this.state.selectedEquipments.length > 0 && this.state.selectedEquipments[0].malfunctionDescription != "" && this.state.selectedEquipments[0].malfunctionDescription != null &&
                                    <div className="report__row__el"
                                         ref={(el) => this.state.refsSimplified[refAux++] = el}
                                         key={'malfunctionDescription'}>
                                        <div className="report__row">
                                            <div className="col-xs-2">
                                                <Text b className="report__label">Descrição da Avaria</Text></div>
                                            <div className="col-xs-10">
                                                <Text p
                                                      className="report__text">{this.state.selectedEquipments.length > 0 && this.state.selectedEquipments[0].malfunctionDescription}</Text>
                                            </div>
                                            <div className="clearfix"></div>
                                        </div>
                                    </div>
                                    }

                                    <div className="report__row__el"
                                         ref={(el) => this.state.refsSimplified[refAux++] = el}
                                         key={'relatorioTrabalho'}>
                                        <div className="report__row">
                                            <div className="col-xs-7">
                                                <Text b className="report__label">Relatório de Trabalho</Text>
                                            </div>
                                            <div className="clearfix"></div>
                                            <div className="p-b-10"></div>
                                            <div className="report__row__el">
                                                <div className="report__hr"></div>
                                                <div className="report__row">
                                                    <div className="col-xs-7">
                                                        <Text p className="report__text">
                                                            {this.state.selectedEquipments.length > 0 && this.state.selectedEquipments[0].relatorioTrabalho}
                                                        </Text>
                                                    </div>
                                                    <div className="clearfix"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                }

                                {(maintenance && maintenance.length > 0) &&
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
                                                    <div key={'_maintenance' + index}
                                                         className={"col-xs-" + Math.floor(5 / this.state.selectedEquipments.length) + " text-center"}>
                                                        <Text b className="report__label">
                                                            #{index + 1}
                                                        </Text>
                                                    </div>
                                                )
                                            })}
                                            <div className="clearfix"></div>
                                        </div>
                                        <div className="report__hr"></div>
                                    </div>
                                    {maintenance.map((item, index) => {
                                        return (
                                            <div key={'maintenance' + index} className="report__row__el"
                                                 ref={(el) => this.state.refsMaintenance[refAux++] = el}>
                                                <div className="report__row">
                                                    <div className="col-xs-7">
                                                        <Text p className="report__text">
                                                            {item.descricao}
                                                        </Text>
                                                    </div>
                                                    {this.state.selectedEquipments.map((equipment, i) => {
                                                        return (
                                                            <div key={'maintenance' + index + "" + i}
                                                                 className={"col-xs-" + Math.floor(5 / this.state.selectedEquipments.length) + " text-center"}>
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
                                                <div className="report__hr"></div>
                                            </div>
                                        )
                                    })}
                                    <div className="clearfix"></div>
                                </div>
                                }

                                {(quality && quality.length > 0) &&
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
                                                    <div key={'_quality' + index}
                                                         className={"col-xs-" + Math.floor(5 / this.state.selectedEquipments.length) + " text-center"}>
                                                        <Text b className="report__label">
                                                            #{index + 1}
                                                        </Text>
                                                    </div>
                                                )
                                            })}
                                            <div className="clearfix"></div>
                                        </div>
                                        <div className="report__hr"></div>
                                    </div>
                                    {quality.map((item, index) => {
                                        return (
                                            <div key={'quality' + index} className="report__row__el"
                                                 ref={(el) => this.state.refsQuality[refAux++] = el}>
                                                <div className="report__row">
                                                    <div className="col-xs-7">
                                                        <Text p className="report__text">
                                                            {item.descricao}
                                                        </Text>
                                                    </div>
                                                    {this.state.selectedEquipments.map((equipment, i) => {
                                                        return (
                                                            <div key={'quality' + index + "" + i}
                                                                 className={"col-xs-" + Math.floor(5 / this.state.selectedEquipments.length) + " text-center"}>
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
                                                <div className="report__hr"></div>
                                            </div>
                                        )
                                    })}
                                    <div className="clearfix"></div>
                                </div>
                                }

                                {(quantity && quantity.length > 0) &&
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
                                                    <div key={'_quantity' + index}
                                                         className={"col-xs-" + Math.floor(5 / this.state.selectedEquipments.length) + " text-center"}>
                                                        <Text b className="report__label">
                                                            #{index + 1}
                                                        </Text>
                                                    </div>
                                                )
                                            })}
                                            <div className="clearfix"></div>
                                        </div>
                                        <div className="report__hr"></div>
                                    </div>
                                    {quantity.map((item, index) => {
                                        return (
                                            <div key={'quantity' + index} className="report__row__el"
                                                 ref={(el) => this.state.refsQuality[refAux++] = el}>

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
                                                            <div key={'quantity' + index + "" + i}
                                                                 className={"col-xs-" + Math.floor(5 / this.state.selectedEquipments.length) + " text-center"}>
                                                                <Text p className="report__text">
                                                                    {equipment.planQuantity[index].resultado}
                                                                </Text>
                                                            </div>
                                                        )
                                                    })}
                                                    <div className="clearfix"></div>
                                                </div>
                                                <div className="report__hr"></div>

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
                                        (equipment.observacao && equipment.observacao != "") ?
                                            <div key={(new Date).getTime() + i} className="report__row__el"
                                                 ref={(el) => this.state.refsComments[refAux++] = el}>
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
                                                </div>
                                            </div> :
                                            <div key={(new Date).getTime() + i}></div>

                                    )
                                })}

                                {(maintenance && maintenance.length > 0) && maintenance.map((item, index) => {
                                    var printed = false;
                                    return (
                                        <div key={index}>
                                            {this.state.selectedEquipments.map((equipment, i) => {
                                                return (
                                                    (equipment.planMaintenance[index].observacoes && equipment.planMaintenance[index].observacoes != "") ?
                                                        <div key={index + "" + i} className="report__row__el"
                                                             ref={(el) => this.state.refsComments[refAux++] = el}>
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
                                                        </div> :
                                                        <div key={index + "" + i}></div>
                                                )
                                            })}
                                        </div>
                                    )
                                })}

                                {(quality && quality.length > 0) && quality.map((item, index) => {
                                    var printed = false;
                                    return (
                                        <div key={index}>
                                            {this.state.selectedEquipments.map((equipment, i) => {
                                                return (
                                                    (equipment.planQuality[index].observacoes && equipment.planQuality[index].observacoes != "") ?
                                                        <div key={index + "" + i} className="report__row__el"
                                                             ref={(el) => this.state.refsComments[refAux++] = el}>
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
                                                            </div>
                                                        </div> :
                                                        <div key={index + "" + i}></div>
                                                )
                                            })}
                                        </div>
                                    )
                                })}

                                {(quantity && quantity.length > 0) && quantity.map((item, index) => {
                                    var printed = false;
                                    return (
                                        <div key={index}>
                                            {this.state.selectedEquipments.map((equipment, i) => {
                                                return (
                                                    (equipment.planQuantity[index].observacoes && equipment.planQuantity[index].observacoes != "") ?
                                                        <div key={index + "" + i} className="report__row__el"
                                                             ref={(el) => this.state.refsComments[refAux++] = el}>
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
                                                        </div> :
                                                        <div key={index + "" + i}></div>
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
                                    assinaturaSieIgualCliente={this.state.selectedEquipments[0].assinaturaSieIgualCliente}
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
                    refsSimplified={this.state.refsSimplified}
                    refsMaintenance={this.state.refsMaintenance}
                    refsQuality={this.state.refsQuality}
                    refsQuantity={this.state.refsQuantity}
                    refsComments={this.state.refsComments}
                    refsFooter={this.state.refsFooter}
                    equipmentType={this.props.equipmentType}
                    date={date}
                    onReportSplit={(totalPages) => {
                        if (this.props.onReportSplit) {
                            this.props.onReportSplit(totalPages);
                        }
                    }}
                />
                }
            </div>
        )
    }
}

export default Report;