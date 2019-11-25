// https://github.com/diegohaz/arc/wiki/Atomic-Design
/*react-styleguide: ignore*/
import React, {Component, Fragment} from 'react';
import _theme from '../../../themes/default';
import styled, {injectGlobal} from 'styled-components';
import {
    Button,
    Text,
    Icon,
    Wrapper,
    Input,
    ModalLarge,
    Spacer
} from 'components';
import MuiGrid from '@material-ui/core/Grid';
import Select from 'react-select';
import axios from 'axios';
import Functions from '../../../helpers/functions';
import SimpleReactValidator from 'simple-react-validator';

const addLinkedPropsToObject = Functions.addLinkedPropsToObject;

axios.defaults.headers.post['Accept'] = 'application/json';
axios.defaults.headers.get['Accept'] = 'application/json';

const {DialogTitle, DialogContent, DialogActions} = ModalLarge;

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
		    line-height: 28px !important;
		}
		&[class*="control"] {
            &:hover {
                border-color: ${_theme.palette.primary.default};
            }
		}
        [aria-live="polite"]+[class*="control"] {
            border-color: ${_theme.palette.secondary.default};  
            box-shadow:0 0 0 1px ${_theme.palette.secondary.default};
            &:hover {
                border-color: ${_theme.palette.secondary.default};
            }
        }
	}
`
const Grid = styled(MuiGrid)`&& {
	padding: ${props => props.padding ? props.padding : (props.container ? '0' : '0 15px')};
	white-space: nowrap;
	overflow: visible;
	text-overflow: ellipsis;

	line-height: 40px;
	&.equipment-list {
        position: relative;
        .equipment-list__header {
            position: absolute;
            left:0;
            right:0;
            top:0;
            background: white;
            z-index:1;
        }
        .equipment-list__body {
            max-height: 319px;
            overflow: auto;
            padding-top:35px;
            padding-bottom:35px;
        }
	}
}`

class Index extends Component {
    state = {
        open: false,
        search: true,
        selected: null,
        searchEquipments: [],
        marca: '',
        modelo: '',
        serie: '',
        numero: '',
        newEquipmentCount: 0
    }

    constructor(props) {
        super(props);
        this.fetch = this.fetch.bind(this);
        this.addEquipmentHandler = this.addEquipmentHandler.bind(this);
        this.addNewEquipmentHandler = this.addNewEquipmentHandler.bind(this);
        this.updateUrl = this.updateUrl.bind(this);
        this.validator = new SimpleReactValidator();
        this.fetch();
    }

    fetch() {
        var url = `/ordens-de-manutencao/equipments`;
        var params = {
            categoryId: this.props.categoryId,
            orderId: this.props.orderId
        };
        axios.get(url, {params}).then((result) => {
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
        return axios.get(url, {params});
    }

    addEquipmentHandler() {
        //add equipment to om
        var url = `/ordens-de-manutencao/${this.props.orderId}/equipments?equipmentId=${this.state.selected.value.idEquipamento}`;

        axios.put(url).then((result) => {

            var planurl = `/ordens-de-manutencao/ficha-de-manutencao`;
            var planparams = {
                categoryId: this.props.categoryId,
                orderId: this.props.orderId,
                equipmentIds: this.state.selected.value.idEquipamento
            };

            axios.get(planurl, {params: planparams}).then((planresult) => {
                var equipment = planresult.data.equipments[0];
                addLinkedPropsToObject(equipment, this);
                this.props.$equipments.value.push(equipment);
                this.state.selected = null;
                this.setState({}, () => {
                    this.props.onChange ? this.props.onChange() : '';
                    this.updateUrl();
                });

            }).catch(function (error) {
            });

        }).catch(function (error) {
        });
    }

    addNewEquipmentHandler() {
        //add equipment to om
        this.state.newEquipmentCount++;
        if (!this.validator.allValid()) {
            this.validator.showMessages();
            this.forceUpdate();
            return;
        }
        this.validator.hideMessages();
        this.forceUpdate();
        var brand, model, serialNumber, orderId, previousEquipmentId, equipmentNumber;
        brand = this.state.marca;
        model = this.state.modelo;
        serialNumber = this.state.serie;
        equipmentNumber = this.state.numero;
        orderId = this.props.orderId;
        previousEquipmentId = this.props.$equipments.value[0].idEquipamento;
        //return;

        var url = `/ordens-de-manutencao/${orderId}/equipments`;

        axios.post(url, {
            brand, model, serialNumber, equipmentNumber, previousEquipmentId
        }).then((result) => {

            var equipmentId = result.data * 1;

            var planurl = `/ordens-de-manutencao/ficha-de-manutencao`;
            var planparams = {
                categoryId: this.props.categoryId,
                orderId: this.props.orderId,
                equipmentIds: equipmentId
            };

            axios.get(planurl, {params: planparams}).then((planresult) => {
                var equipment = planresult.data.equipments[0];
                addLinkedPropsToObject(equipment, this);
                this.props.$equipments.value.push(equipment);
                this.state.selected = null;
                this.setState({}, () => {
                    this.props.onChange ? this.props.onChange() : '';
                    this.updateUrl();
                });

            }).catch(function (error) {
            });

        }).catch(function (error) {
        });
    }

    updateUrl() {
        var equipmentsIds = this.props.$equipments.value.map((e) => e.idEquipamento).join(',');
        var newUrl = updateURLParameter(window.location.href, 'equipmentsIds', equipmentsIds);
        window.history.pushState({}, "", newUrl);
    }

    componentDidUpdate(props) {
        var newState = {};
        if (props.$equipments !== this.state.$equipments) {
            newState.$equipments = props.$equipments;
        }
        if (props.$equipmentsCount !== this.state.$equipmentsCount) {
            newState.$equipmentsCount = props.$equipmentsCount;
        }
        if (Object.keys(newState).length > 0) {
            this.setState(newState, () => {
            });
        }
    }

    render() {
        return (
            <ModalLarge
                onClose={() => this.setState({open: false, search: true})}
                onOpen={() => this.setState({open: true})}
                open={this.state.open}
                action={this.props.children} children={
                <Grid>
                    <DialogTitle>
                        <Text h2>{this.props.title}</Text>

                    </DialogTitle>
                    <hr/>

                    <DialogContent>
                        {!this.state.search &&
                        <form className="cmxform" id="commentForm" method="get" action="">
                            <Grid container direction="row" justify="space-between" alignitems="middle" spacing={0}
                                  maxwidth={'100%'} margin={0}>
                                <Grid item xs={12} sm={2} padding="4px">
                                    <Input placeholder={'Marca'}
                                           onChange={(e) => this.setState({marca: e.target.value})}
                                           error={this.state.newEquipmentCount > 0 && !this.validator.fieldValid('Marca')}
                                    />
                                    <div className="hide">
                                        {this.validator.message('Marca', this.state.marca, 'required')}
                                    </div>
                                </Grid>
                                <Grid item xs={12} sm={2} padding="4px">
                                    <Input placeholder={'Modelo'} ref={el => this.modeloRef = el}
                                           onChange={(e) => this.setState({modelo: e.target.value})}
                                           error={this.state.newEquipmentCount > 0 && !this.validator.fieldValid('Modelo')}
                                    />
                                    <div className="hide">
                                        {this.validator.message('Modelo', this.state.modelo, 'required')}
                                    </div>
                                </Grid>
                                <Grid item xs={12} sm={2} padding="4px">
                                    <Input placeholder={'Nº Equip.'} ref={el => this.serieRef = el}
                                           onChange={(e) => this.setState({numero: e.target.value})}
                                           error={this.state.newEquipmentCount > 0 && !this.validator.fieldValid('Nº Equip.')}
                                    />
                                    <div className="hide">
                                        {this.validator.message('Nº Equip.', this.state.numero, 'required')}
                                    </div>
                                </Grid>
                                <Grid item xs={12} sm={2} padding="4px">
                                    <Input placeholder={'Nº Série'} ref={el => this.serieRef = el}
                                           onChange={(e) => this.setState({serie: e.target.value})}/>
                                </Grid>
                                <Grid item xs={12} sm={2} padding="4px">
                                    <Button default className={''}
                                            onClick={this.addNewEquipmentHandler}>Adicionar</Button>
                                </Grid>
                                <Grid item xs={12} sm={2} padding="4px">
                                    <Button link style={{display: 'block'}}
                                            onClick={() => this.setState({search: true})}
                                            className={'m-l-20 pull-right'}>Procurar</Button>
                                </Grid>
                            </Grid>
                        </form>
                        }
                        {this.state.search &&
                        <Grid container direction="row" justify="space-between" alignitems="middle" spacing={0}
                              maxwidth={'100%'} margin={0}>
                            <Grid item xs={12} sm={8} padding="4px" style={{overflow: 'visible'}}>
                                {this.props.$equipments &&
                                <Select placeholder={'Adicionar ' + this.props.title}
                                        value={this.state.selected}
                                        options={
                                            this.state.searchEquipments.filter((item) => {
                                                return !(this.props.$equipments.value.filter((i) => {
                                                    return i.idEquipamento == item.idEquipamento;
                                                }).length > 0);
                                            }).map((item) => {
                                                return {value: item, label: item.numEquipamento}
                                            })
                                        }
                                        onChange={(selected) => {
                                            this.setState({selected: selected});
                                        }}
                                        isSearchable
                                        isClearable
                                />
                                }
                            </Grid>
                            <Grid item xs={12} sm={2} padding="4px">
                                <Button default className={''}
                                        onClick={this.addEquipmentHandler}>Adicionar</Button>
                            </Grid>
                            <Grid item xs={12} sm={2} padding="4px">
                                <Button link style={{display: 'block'}}
                                        onClick={() => {
                                            this.setState({search: false, newEquipmentCount: 0})
                                        }}
                                        className={'m-l-20 pull-right'}>Criar Novo</Button>
                            </Grid>
                        </Grid>
                        }
                        <Spacer height={'16px'}/>
                        <hr/>
                        <Spacer height={'24px'}/>
                        <Grid container direction="row" justify="space-between" alignitems="middle" spacing={0}
                              maxwidth={'100%'} margin={0} className={"equipment-list"}>
                            <Grid container direction="row" justify="space-between" alignitems="middle" spacing={0}
                                  maxwidth={'100%'} margin={0} className={"equipment-list__header"}>
                                <Grid item xs={2}><Text label data-tip={'Marca'}>#&nbsp;&nbsp;Marca</Text></Grid>
                                <Grid item xs={2}><Text label data-tip={'Modelo'}>Modelo</Text></Grid>
                                <Grid item xs={2}><Text label data-tip={'Nº Equip.'}>Nº Equip.</Text></Grid>
                                <Grid item xs={2}><Text label data-tip={'Nº Série'}>Nº Série</Text></Grid>
                                <Grid item xs={2}></Grid>
                                <Grid item xs={2}></Grid>

                            </Grid>
                            <Grid container direction="row" justify="space-between" alignitems="middle" spacing={0}
                                  maxwidth={'100%'} margin={0} className={"equipment-list__body"}>
                                {this.props.$equipments && this.props.$equipments.value.map((item, index) => {
                                    return (<Grid container key={index} direction="row" justify="space-between"
                                                  alignitems="middle" spacing={0} maxwidth={'100%'} margin={0}>
                                        <Grid item xs={2}>
                                            <Text span
                                                  data-tip={item.marcaText}
                                                  className={'to-ellipsis l-h-1 p-t-15'}
                                            >{index + 1}&nbsp;&nbsp;{item.marcaText}
                                            </Text>
                                        </Grid>
                                        <Grid item xs={2}>
                                            <Text span
                                                  data-tip={item.modeloText}
                                                  className={'to-ellipsis l-h-1 p-t-15'}
                                            >{item.modeloText}
                                            </Text>
                                        </Grid>
                                        <Grid item xs={2}>
                                            <Text span
                                                  data-tip={item.numEquipamento}
                                                  className={'to-ellipsis l-h-1 p-t-15'}
                                            >{item.numEquipamento}
                                            </Text>
                                        </Grid>
                                        <Grid item xs={2}>
                                            <Text span data-tip={item.numSerie}
                                                  className={'to-ellipsis l-h-1 p-t-15'}
                                            >{item.numSerie}
                                            </Text>
                                        </Grid>
                                        <Grid item xs={2}>{this.props.$equipments.value.length > 1 &&
                                        <Button iconSolo onClick={() => {
                                            var eqArray = this.props.$equipments.value;
                                            eqArray.splice(index, 1);
                                            this.props.$equipments.value = eqArray;
                                            this.setState({});
                                            this.props.onChange();
                                        }
                                        }><Icon decline/></Button>}</Grid>
                                        <Grid item xs={2}></Grid>
                                    </Grid>)
                                })}
                            </Grid>
                        </Grid>
                    </DialogContent>
                    <hr/>
                    <DialogActions>
                        <Wrapper className="equipmentsCounter" padding="0 0 0 40px"><Text
                            span>{this.props.$equipments && this.props.$equipments.value.length} Equipamentos</Text></Wrapper>
                        <Button primary color="primary"
                                onClick={() => this.setState({open: false, search: true})}
                        >Guardar</Button>
                    </DialogActions>
                </Grid>
            }/>
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


export default Index;