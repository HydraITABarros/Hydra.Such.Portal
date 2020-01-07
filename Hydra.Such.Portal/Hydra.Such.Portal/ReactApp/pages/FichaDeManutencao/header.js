import React, {Component} from 'react';
import {Text as eText, Icon, Wrapper, Spacer, Button, Avatars, Select, MenuItem, Menu, Modal} from 'components';
import MuiGrid from '@material-ui/core/Grid';
import styled, {css, theme, injectGlobal, withTheme} from 'styled-components';
import {createMuiTheme} from '@material-ui/core/styles';
import Hidden from '@material-ui/core/Hidden';
import {withRouter} from 'react-router-dom';
import functions from '../../helpers/functions';
import mEquipments from './ModalEquipments';
import ModalReport from './ModalReport';
import ModalOptions from './ModalOptions';
import _theme from "../../themes/default";

const {DialogTitle, DialogContent, DialogActions} = Modal;

const muiTheme = createMuiTheme();

const breakpoints = muiTheme.breakpoints.values;

const RootTitle = styled(Wrapper)` && {
	padding: 20px 25px 0;
	background:  ${props => props.theme.palette.bg.grey};
	z-index: 100;
	position: relative;
	border-bottom: solid 1px ${props => props.theme.palette.bg.grey};
	
}`

const RootDescription = styled(Wrapper)` && {
	border-bottom: solid 1px ${props => props.theme.palette.primary.keylines};
	padding: 0 25px 20px;
	background:  ${props => props.theme.palette.bg.grey};
	min-height: 190px;
	z-index: 1;
	&:after {
      content: "";
      position: absolute;
      top: -38px;
      left: 0;
      right: 0;
      height: 150px;
      background: ${props => props.theme.palette.bg.grey};
      z-index: -1;
    }
}`

const ModalEquipments = styled(mEquipments)` && {
	display:  inline-block;
}`

const Text = styled(eText)` && {
	margin: 0;
	overflow: hidden;
	white-space: nowrap;
	text-overflow: ellipsis;
	max-width: 100%;
	display: inline-block;
}`

const Title = styled(Text)` && {
	transition: font-size .12s  .0s ease-out;
	padding-left: 10px;
	line-height: 48px;
	@media (max-width: ${breakpoints.md}px) {
		padding-left: 0px;
	}
}`

injectGlobal`
	.sticky && {
		${Title} {
			transition: font-size .1s  .0s ease;
			font-size: 30px !important;
		}
	}
`;

const Grid = styled(MuiGrid)`&& {
	padding: ${props => props.padding ? props.padding : (props.container ? '0' : '0 15px')};
	white-space: nowrap;
	overflow: hidden;
	text-overflow: ellipsis;

	line-height: 32px;
}`

const CustomIcon = styled(Icon)` && {

}`;

const avatarColors = [
    "#990000",
    "#33DDEE",
    "#5533DD",
    "#339900",
    "#cc00cc"
];

class HTitle extends Component {
    state = {
        isLoading: true,
        title: null,
        isModalReportOpen: false
    }

    constructor(props) {
        super(props);
        this.state = {...props};
    }

    componentDidUpdate(props) {
        var newState = {};

        if (props.title !== this.state.title) {
            newState.title = props.title;
        }

        if (props.order !== this.props.order) {
            return true;
        }

        if (Object.keys(newState).length > 0) {
            this.setState(newState, () => {
            });
        }
    }

    componentDidMount() {
        typeof this.props.onRef == 'function' ? this.props.onRef(this) : '';
    }

    componentWillUnmount() {
        typeof this.props.onRef == 'function' ? this.props.onRef(undefined) : '';
    }

    //const[state, setState] = React.useState({});
    render() {
        var completed = false;
        if (this.props.$equipments) {
            completed = this.props.$equipments.value.filter((e) => {
                return e.$estadoFinal.value > 0;
            }).length == this.props.$equipments.value.length;

        } else {
            return <div></div>;
        }

        ModalReport.open = ModalReport.open || false;
        return (
            <RootTitle width="100%" className={this.props.className}>
                <Grid container direction="row" justify="space-between" alignitems="middle" spacing={0}
                      maxwidth={'100%'} margin={0}>
                    <Grid item xs={12} sm={6} md={6} lg={7} padding="15px">
                        {(this.props.isPreventive || this.props.isSimplified) ?
                            <Title h1 data-tip={this.props.order.no}>{this.props.order.no}</Title> :
                            <Title h1 data-tip={this.state.title}>{this.state.title}</Title>
                        }
                    </Grid>
                    <Grid item xs={12} sm={6} md={6} lg={5} padding="15px"
                          style={{textAlign: window.innerWidth > breakpoints.sm ? 'right' : 'left'}}>

                        {this.props.isCurative &&
                        <Text b style={{
                            color: _theme.palette.secondary.default,
                            verticalAlign: 'middle'
                        }} className={"p-r-25"}>
                            <Icon curativa data-tip="Curativa"/>
                            &nbsp;&nbsp; Ordem Curativa
                        </Text>
                        }
                        <ModalOptions
                            $equipments={this.props.$equipments}
                            orderId={this.props.orderId}
                            onChange={() => {
                                if (this.props.onOptionsChange) {
                                    this.props.onOptionsChange();
                                }
                            }}
                        >
                            <Button icon={<Icon options/>}
                                    style={{
                                        lineHeight: '14px',
                                        verticalAlign: 'middle',
                                        padding: 0,
                                        width: '40px',
                                        minWidth: '40px',
                                        textAlign: 'center',
                                        marginRight: '15px'
                                    }}
                                    disabled={false}
                            ></Button>
                        </ModalOptions>

                        <Menu
                            containerStyle={{marginRight: '15px', display: 'inline-block'}}
                            action={
                                <Button disabled={!completed}
                                        iconPrimary={<Icon report/>}
                                        onClick={() => {
                                            //window.location.href = window.location.origin + '/images/certificados.pdf';
                                            //this.props.history.push(`/images/certificado.pdf`);
                                        }}
                                >
                                    Relatórios
                                    <Icon arrow-down style={{lineHeight: '14px', verticalAlign: 'middle'}}/>
                                </Button>
                            }
                        >
                            <MenuItem onClick={() => {
                                this.state.isModalReportOpen = true;
                                this.setState({});
                            }}>
                                Relatório de Manutenção
                            </MenuItem>

                        </Menu>

                        <ModalReport
                            open={this.state.isModalReportOpen || false}
                            order={this.props.order}
                            equipmentType={this.props.title}
                            $equipments={this.props.$equipments}
                            $currentUser={this.props.$currentUser}

                            isCurative={this.props.isCurative}
                            isSimplified={this.props.isSimplified}

                            onClose={() => {
                                //item.open = false;
                                this.setState({toUpdate: this.state.toUpdate + 1, isModalReportOpen: false});
                            }}
                            onChange={() => {
                                if (this.props.onReportChange) {
                                    this.props.onReportChange();
                                }
                            }}>
                        </ModalReport>

                    </Grid>
                    <Grid item xs={12}>
                        <Spacer height="5px"/>
                        <Hidden smUp>
                            <Spacer height="15px"/>
                        </Hidden>
                    </Grid>
                </Grid>
            </RootTitle>
        )
    }
};

class HDescription extends Component {
    state = {
        isLoading: true,
        service: null,
        room: null,
        equipments: [],
        equipmentsCount: 0,
        $equipments: [],
        $equipmentsCount: 0,
        rotinaModalOpen: false,
        rotinaSelected: null
    }

    constructor(props) {
        super(props);
        this.state = {...props};
        this.state.rotinaModalOpen = false;

    }

    componentDidUpdate(props) {
        var newState = {};

        if (props.isLoading !== this.state.isLoading) {
            newState.isLoading = props.isLoading;
        }
        if (props.orderId !== this.state.orderId) {
            newState.orderId = props.orderId;
        }
        if (props.order !== this.state.order) {
            newState.order = props.order;
        }
        if (props.service !== this.state.service) {
            newState.service = props.service;
        }
        if (props.room !== this.state.room) {
            newState.room = props.room;
        }
        if (props.equipments !== this.state.equipments) {
            newState.equipments = props.equipments;
        }
        if (props.$equipments !== this.state.$equipments) {
            newState.$equipments = props.$equipments;
        }
        if (props.equipmentsCount !== this.state.equipmentsCount) {
            newState.equipmentsCount = props.equipmentsCount;
        }
        if (props.$equipmentsCount !== this.state.$equipmentsCount) {
            newState.$equipmentsCount = props.$equipmentsCount;
        }
        if (Object.keys(newState).length > 0) {
            this.setState(newState, () => {
            });
        }
    }

    //const[state, setState] = React.useState({});
    render() {
        var rotina = this.state.equipments && this.state.equipments.length > 0 ? this.state.equipments[0].rotinaId : '';

        return (
            <RootDescription width="100%">
                <Grid container direction="row" justify="space-between" alignitems="middle" spacing={0}
                      maxwidth={'100%'} margin={0}>
                    <Grid item xs={12} sm={6} md={5} padding="0">
                        <Grid container direction="row" justify="space-between" alignitems="middle" spacing={0}
                              maxwidth={'100%'} margin={0}>
                            <Grid item xs={12} md={2}></Grid>
                            <Grid item xs={12} md={10} padding="0">
                                <Grid container direction="row" justify="space-between" alignitems="middle" spacing={0}
                                      maxwidth={'100%'} margin={0} style={{lineHeight: '32px'}}>
                                    <Grid item xs={3} sm={4} md={3}>
                                        {this.state.room && <Text span data-tip={'Piso, Sala'}>Piso, Sala</Text>}
                                    </Grid>
                                    <Grid item xs={9} sm={8} md={9}>
                                        {this.state.room && <Text b data-tip={this.state.room}>{this.state.room}</Text>}
                                    </Grid>
                                    <Grid item xs={4} sm={3}>
                                        <Text span data-tip={'Serviço'}>Serviço</Text>
                                    </Grid>
                                    <Grid item xs={8} sm={9}>
                                        <Text b data-tip={this.state.service}>{this.state.service}</Text>
                                    </Grid>
                                    {!this.props.isSimplified &&
                                    <Grid item xs={4} sm={3}>
                                        <Text span data-tip={'Rotina'}>Rotina</Text>
                                    </Grid>
                                    }
                                    {!this.props.isSimplified &&
                                    <Grid item xs={8} sm={9}
                                          className={this.state.equipments.filter(e => e.estadoFinal > 0).length > 0 ? "disabled" : ""}>
                                        <Menu
                                            containerStyle={{marginRight: '15px', lineHeight: 0}}
                                            action={
                                                <Button link>{rotina == 2 ? "B" : "A"}</Button>
                                            }
                                        >
                                            <MenuItem onClick={() => {
                                                if (rotina == 1) {
                                                    return;
                                                }

                                                this.props.onRotinaChange(1);

                                            }} className={rotina == 1 ? "disabled" : ""}>
                                                Rotina &nbsp;&nbsp; <Text b>A</Text>
                                            </MenuItem>
                                            <MenuItem onClick={() => {
                                                if (rotina == 2) {
                                                    return;
                                                }

                                                this.props.onRotinaChange(2);
                                            }} className={rotina == 2 ? "disabled" : ""}>
                                                Rotina &nbsp;&nbsp; <Text b>B</Text>
                                            </MenuItem>
                                        </Menu>
                                    </Grid>
                                    }
                                    <Grid item xs={12}>
                                        <Spacer height="10px"/>
                                    </Grid>
                                    <Grid container>
                                        <Grid item xs={4} sm={3}>
                                            <Spacer height="7px"/>
                                            <Text span data-tip={'Técnicos'}>Técnicos</Text>
                                        </Grid>
                                        <Grid item xs={8} sm={9}>
                                            {this.state.order && this.state.order.technicals.map((_item, i) => (
                                                <Avatars.Avatars key={i} letter color={avatarColors[i]}
                                                                 data-tip={_item.nome}>{functions.getInitials(_item.nome)}</Avatars.Avatars>))}
                                        </Grid>
                                    </Grid>
                                    <Grid item xs={12}>
                                        <Spacer height="20px"/>
                                    </Grid>
                                </Grid>
                            </Grid>
                        </Grid>
                    </Grid>
                    <Hidden lgDown>
                        <Grid item xs={12} md={1}></Grid>
                    </Hidden>

                    <Grid item xs={12} sm={6} md={5} padding="0">
                        {!(this.props.isSimplified && !this.props.isCurative) &&
                        <Grid container direction="row" justify="space-between" alignitems="middle" spacing={0}
                              maxwidth={'100%'} margin={0}>
                            <Grid item xs={3}><Text label data-tip={'Marca'}>#&nbsp;&nbsp;Marca</Text></Grid>
                            <Grid item xs={3}><Text label data-tip={'Modelo'}>Modelo</Text></Grid>
                            <Grid item xs={3}><Text label data-tip={'Nº Equip.'}>Nº Equip.</Text></Grid>
                            <Grid item xs={3}><Text label data-tip={'Nº Série'}>Nº Série</Text></Grid>
                            {this.state.equipments.filter(item => item.idEquipamento != 0).slice(0, 5).map((item, index) => {
                                return (<Grid container key={index} direction="row" justify="space-between"
                                              alignitems="middle" spacing={0} maxwidth={'100%'} margin={0}>
                                    <Grid item xs={3}><Text span
                                                            data-tip={item.marcaText}>{index + 1}&nbsp;&nbsp;{item.marcaText}</Text></Grid>
                                    <Grid item xs={3}><Text span
                                                            data-tip={item.modeloText}>{item.modeloText}</Text></Grid>
                                    <Grid item xs={3}><Text span
                                                            data-tip={item.numEquipamento}>{item.numEquipamento}</Text></Grid>
                                    <Grid item xs={3}><Text span data-tip={item.numSerie}>{item.numSerie}</Text></Grid>
                                </Grid>)
                            })}

                            <Grid item xs={7}>
                                {this.state.equipments.length > 5 &&
                                <span>+
										<ModalEquipments $equipmentsCount={this.props.$equipmentsCount}
                                                         $equipments={this.props.$equipments} title={this.props.title}
                                                         categoryId={this.props.categoryId} orderId={this.props.orderId}
                                                         onChange={this.props.onEquipmentsChange}
                                                         isCurative={this.props.isCurative}>
											<Button link style={{cursor: 'pointer'}}>
												{this.state.equipments.length > 5 && (this.state.equipments.length - (this.state.equipments.slice(0, 5).length))}
											</Button>
										</ModalEquipments>
                                    &nbsp;&nbsp;
									</span>
                                }
                                {(!this.props.isCurative || (this.props.$equipments && this.props.$equipments.value[0].idEquipamento == 0)) &&
                                <ModalEquipments $equipmentsCount={this.props.$equipmentsCount}
                                                 $equipments={this.props.$equipments} title={this.props.title}
                                                 categoryId={this.props.categoryId} orderId={this.props.orderId}
                                                 onChange={this.props.onEquipmentsChange}
                                                 isCurative={this.props.isCurative}>
                                    <Button link style={{cursor: 'pointer'}}>
                                        Adicionar Equip.
                                    </Button>
                                </ModalEquipments>
                                }
                            </Grid>
                            <Grid item xs={5}></Grid>

                        </Grid>
                        }
                    </Grid>
                    <Grid item xs={12} md={1}></Grid>
                </Grid>
                <Spacer height={'24px'}/>
            </RootDescription>
        )
    }
};
const HeaderTitle = withRouter(withTheme(HTitle));
const HeaderDescription = withRouter(withTheme(HDescription));

const Header = {
    HeaderTitle,
    HeaderDescription
}

export default Header;
