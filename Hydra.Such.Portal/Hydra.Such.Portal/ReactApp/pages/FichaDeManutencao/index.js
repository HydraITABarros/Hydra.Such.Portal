import React, {Component} from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import {PageTemplate} from 'components';
import styled, {injectGlobal, withTheme} from 'styled-components';
import {Wrapper, Tooltip, Text, MultipleCheckBox, MenuItem, Input, Icon, Button, Modal, Menu} from 'components';
import {withRouter} from 'react-router-dom';
import queryString from 'query-string';
import Breadcrumb from './breadcrumb';
import {createMuiTheme} from '@material-ui/core/styles';
import StickyEl from 'react-sticky-el';
import {Waypoint} from 'react-waypoint';
import MuiGrid from '@material-ui/core/Grid';
import Header from './header';
import PlanActions from './planActions';
import PlanEquipmentsHeader from './planEquipmentsHeader';
import PlanEquipmentsItem from './planEquipmentsItem';
import ModalComments from './ModalComments';
import FinalState from './finalState';
import PlanRow from './planRow';
import _theme from '../../themes/default';
import Color from 'color';
import _ from 'lodash';
import Functions from '../../helpers/functions';
import {Offline, Online} from "react-detect-offline";
import storageService from './storageService';
import './index.scss';
import {Scrollbars} from 'react-custom-scrollbars';
import {AvRecentActors} from "material-ui/svg-icons/index.es";

const addLinkedPropsToObject = Functions.addLinkedPropsToObject;

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
			padding-top: 0; 
			line-height: 1em;
			h1 {				 
				line-height: 1.4em;
			}
			div {
				line-height: 0;
			}
		}
		${PlanHeader} {
			border-bottom: solid 1px ${props => props.theme.palette.primary.keylines};
			transform: translateZ(0px) translateY(65px);
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

var renderCount = 0;

//@observer
class FichaDeManutencao extends Component {
    state = {
        isLoading: true,
        tooltipReady: false,
        orderId: null,
        order: null,
        categoryId: null,
        equipmentsIds: null,
        equipments: [],
        equipmentsCount: 0,
        rotinaId: 1,
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
        servicoIds: null,
        toUpdate: 0,
        selectedOptionsEl: null,
        orderPlanSimplified: null,
        isCurative: false,
        isSimplified: false
    }

    constructor(props) {
        super(props);
        this.state.orderId = this.props.match.params.orderid;
        this.fetch = this.fetch.bind(this);
        var query = queryString.parse(props.location.search);
        this.state.marcaIds = query.marcaIds;
        this.state.servicoIds = query.servicoIds;
        this.state.categoryId = query.categoryId;
        this.state.equipmentsIds = query.equipmentsIds;
        this.setBodyHeight = this.setBodyHeight.bind(this);
        this.setPlanHeight = this.setPlanHeight.bind(this);
        this.getScrollShadow = this.getScrollShadow.bind(this);
        this.waipointQualitativoHandlerLeave = this.waipointQualitativoHandlerLeave.bind(this);
        this.waipointQuantitativoHandlerLeave = this.waipointQuantitativoHandlerLeave.bind(this);
        this.handleScrollTo = this.handleScrollTo.bind(this);
        this.handleRotinaChange = this.handleRotinaChange.bind(this);
        this.savePlans = this.savePlans.bind(this);
        this.addTechnical = this.addTechnical.bind(this);

        window.addEventListener('online', () => {
            setTimeout(() => {
                $(".scrollarea")[0].scrollTop = $(".scrollarea")[0].scrollTop - 1
            }, 0);
        });

        window.addEventListener('offline', () => {
            setTimeout(() => {
                $(".scrollarea")[0].scrollTop = $(".scrollarea")[0].scrollTop - 1
            }, 0);
        });

        //console.log(storageService);
    }

    shouldComponentUpdate(nextProps, nextState) {
        let isToUpdate = false;
        if (nextState.toUpdate != this.state.toUpdate) {
            isToUpdate = true;
        } else if (nextState.$equipmentsCount && nextState.$equipmentsCount.value != this.state.equipmentsCount) {
            nextState.equipmentsCount = nextState.$equipmentsCount.value;
            isToUpdate = true;
        } else if (this.state.equipmentsHeaderScroll.innerWidth != nextState.equipmentsHeaderScroll.innerWidth) {
            isToUpdate = true;
        } else {
            isToUpdate = nextState.isLoading !== this.state.isLoading || nextState.order !== this.state.order || nextState.equipmentsCount != this.state.equipmentsCount;
        }
        if (isToUpdate == true) {
            //this.savePlans();
        }
        return isToUpdate;
    }

    componentDidMount() {
        this._ismounted = true;
        storageService.onFirstSync = () => {
            if (this._ismounted) {
                this.fetch();
            }
        };
        storageService.init();
        this.setBodyHeight();
        //var planContentEl = this.planContent.getElement();
        var planContentEl = ReactDOM.findDOMNode(this.planContent);
        //var planEquipmentsHeaderEl = this.planEquipmentsHeader.getElement();
        var planEquipmentsHeaderEl = ReactDOM.findDOMNode(this.planEquipmentsHeader);
        var startClientY = 0;


        planEquipmentsHeaderEl.ontouchstart = (e) => {
            startClientY = e.changedTouches[0].clientY;
        };

        planEquipmentsHeaderEl.ontouchmove = (e) => {
            this.mainScroll.scrollTop = this.mainScroll.scrollTop + (e.changedTouches[0].clientY, startClientY - e.changedTouches[0].clientY);

            startClientY = e.changedTouches[0].clientY;
        };

        planContentEl.ontouchstart = (e) => {
            startClientY = e.changedTouches[0].clientY;
        };

        planContentEl.ontouchmove = (e) => {
            this.mainScroll.scrollTop = this.mainScroll.scrollTop + (e.changedTouches[0].clientY, startClientY - e.changedTouches[0].clientY);

            startClientY = e.changedTouches[0].clientY;
        };

        setTimeout(() => {
            this.state.equipmentsHeaderScroll = {
                outerWidth: ReactDOM.findDOMNode(this.equipmentsHeaderWrapper).offsetWidth,
                innerWidth: ReactDOM.findDOMNode(this.planEquipmentsHeader).scrollWidth,
                scrollLeft: 0
            };
            this.setState({});
            this.addTechnical();
        }, 500)

    }

    componentWillUnmount() {
        this._ismounted = false;
    }

    setBodyHeight() {
        var windowHeight = window.innerHeight;
        var mainHeaderHeight = 60;
        var breadcrumbHeight = ReactDOM.findDOMNode(this.breadcrumbWrapper).offsetHeight;
        this.setState({bodyHeight: (windowHeight * 1) - (mainHeaderHeight * 1) - (breadcrumbHeight * 1)});
    }

    setPlanHeight() {
        var bodyHeight = this.state.bodyHeight;
        var headerHeight = ReactDOM.findDOMNode(this.breadcrumbWrapper).offsetHeight;
        var tabsHeight = ReactDOM.findDOMNode(this.breadcrumbWrapper).offsetHeight;
        this.setState({tabsContentHeight: (bodyHeight * 1) - (headerHeight * 1) - (tabsHeight)});
    }

    fetch() {
        var url = `/ordens-de-manutencao/ficha-de-manutencao`;
        var params = {
            categoryId: this.state.categoryId,
            orderId: this.state.orderId,
            equipmentIds: this.state.equipmentsIds,
            servicoIds: this.state.servicoIds,
            marcaIds: this.state.marcaIds
        };
        axios.get(url, {params}).then((result) => {
            var data = result.data;
            if (data.order && data.equipments) {

                var state = {
                    order: data.order,
                    title: data.isCurative || data.isSimplified ? this.state.orderId : data.equipments.length > 0 ? data.equipments[0].categoriaText : "",
                    service: data.equipments.length > 0 ? data.equipments[0].servicoText : "",
                    room: data.equipments.length == 1 ? data.equipments[0].sala : "",
                    equipments: data.equipments,
                    planMaintenance: data.planMaintenance,
                    planQuality: data.planQuality,
                    planQuantity: data.planQuantity,
                    equipmentsCount: data.equipments.length,
                    currentUser: data.currentUser,
                    rotinaId: data.equipments.length > 0 ? data.equipments[0].rotinaId : 1,
                    orderPlanSimplified: data.orderPlanSimplified,
                    isCurative: data.isCurative,
                    isSimplified: data.isSimplified
                };

                addLinkedPropsToObject(state, this);

                this.setState(state);
            }
        }).catch(function (error) {
            if (error.response.status == 404) {
                window.history.back();
            }
        }).then(() => {
            this.setState({
                isLoading: false,
                equipmentsHeaderScroll: {
                    outerWidth: ReactDOM.findDOMNode(this.equipmentsHeaderWrapper).offsetWidth,
                    innerWidth: ReactDOM.findDOMNode(this.planEquipmentsHeader).scrollWidth,
                    scrollLeft: 0
                }
            });
            //this.setPlanHeight();
            setTimeout(() => {
                this.setState({tooltipReady: true});
                Tooltip.Hidden.hide();
                Tooltip.Hidden.rebuild();
            }, 1200);
        });
    }

    addTechnical() {
        axios.put(`/ordens-de-manutencao/${this.state.orderId}/technicals/logged?all=true`).then((result) => {
        }).catch(function (error) {
            console.log(error);
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

    waipointQualitativoHandlerLeave({previousPosition, currentPosition, event}) {
        const isAbove = currentPosition === Waypoint.above;
        const wasInside = previousPosition === Waypoint.inside;
        if (isAbove && wasInside) {
            this.setState({position: 1});
            this.planActionsRef.setState({position: 1});
        }
        if (!isAbove && !wasInside) {
            this.setState({position: 0});
            this.planActionsRef.setState({position: 0});
        }
    }

    waipointQuantitativoHandlerLeave({previousPosition, currentPosition, event}) {
        const isAbove = currentPosition === Waypoint.above;
        const wasInside = previousPosition === Waypoint.inside;
        if (isAbove && wasInside) {
            this.setState({position: 2});
            this.planActionsRef.setState({position: 2});
        }
        if (!isAbove && !wasInside) {
            this.setState({position: 1});
            this.planActionsRef.setState({position: 1});
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
    };

    handleRotinaChange(rotina) {

        this.state.$equipments.value.map((equipment) => {
            equipment.rotinaId = rotina;
            equipment.$rotinaId.value = rotina;
        });
        this.setState({rotinaId: rotina, toUpdate: this.state.toUpdate + 1}, () => {
            this.savePlans();
        });

    };

    savePlans() {
        console.log("Saving");
        this.state.equipments.map((item) => {
            storageService.saveData(item);
        });
        setTimeout(() => {
            this.setState({toUpdate: this.state.toUpdate + 1}, () => {
                this.state.equipments.map((item) => {
                    storageService.saveData(item);
                });
            });
        }, 150);
    }

    render() {
        var completed = false;
        if (this.state.$equipments) {
            completed = this.state.$equipments.value.filter((e) => {
                return e.$estadoFinal.value > 0;
            }).length == this.state.$equipments.value.length;
        }
        return (
            <PageTemplate>
                <Offline>
                    <div className="bg-search-default p-t-5 p-l-10 p-b-5 p-r-10">
                        <Icon no-wifi/>
                        <Text b className={"p-l-10 p-r-20"}> Sem ligação à internet</Text>
                        <Text span>Todas as alterações serão gravadas offline</Text>
                        <Text span className={"pull-right"}>
                            <small>Última
                                actualização {storageService.syncDate && storageService.syncDate.format("DD MMM HH:mm")}
                            </small>
                        </Text>
                    </div>
                </Offline>

                <Wrapper padding={'0 0 0'} width="100%" minHeight="274px" ref={el => this.pageWrapper = el}>
                    <Breadcrumb
                        isLoading={this.state.isLoading}
                        isCurative={this.state.isCurative}
                        order={this.state.order} onRef={el => this.breadcrumbWrapper = el}/>
                    <div className="scrollarea " style={{height: this.state.bodyHeight + 'px', overflow: 'auto'}}
                         ref={el => this.mainScroll = el}>
                        <Sticky scrollElement=".scrollarea" style={{zIndex: 11, position: 'relative'}}>

                            <HeaderTitle
                                isCurative={this.state.isCurative}
                                isSimplified={this.state.isSimplified}

                                orderPlanSimplified={this.state.orderPlanSimplified}
                                title={this.state.title}
                                order={this.state.order}
                                onRef={el => this.headerTitleWrapper = el}
                                $equipments={this.state.$equipments}
                                $currentUser={this.state.$currentUser}
                                orderId={this.state.orderId}
                                onReportChange={() => {
                                    this.savePlans();
                                }}
                                onOptionsChange={() => {
                                    this.savePlans();
                                }}
                            />

                        </Sticky>
                        <HeaderDescription
                            isCurative={this.state.isCurative}
                            isSimplified={this.state.isSimplified}

                            orderPlanSimplified={this.state.orderPlanSimplified}
                            service={this.state.service} types={this.state.types}
                            order={this.state.order} equipments={this.state.equipments}
                            $equipments={this.state.$equipments}
                            $equipmentsCount={this.state.$equipmentsCount}
                            title={this.state.title}
                            orderId={this.state.orderId}
                            categoryId={this.state.categoryId}
                            onEquipmentsChange={
                                () => {
                                    this.setState({
                                        toUpdate: this.state.toUpdate++
                                    });

                                    setTimeout(() => {
                                        this.savePlans();
                                        this.setState({
                                            isLoading: false,
                                            equipmentsHeaderScroll: {
                                                outerWidth: ReactDOM.findDOMNode(this.equipmentsHeaderWrapper).offsetWidth,
                                                innerWidth: ReactDOM.findDOMNode(this.planEquipmentsHeader).scrollWidth,
                                                scrollLeft: 0
                                            }
                                        });
                                    }, 300);

                                    //this.setPlanHeight();
                                    setTimeout(() => {
                                        this.setState({tooltipReady: true});
                                        Tooltip.Hidden.hide();
                                        Tooltip.Hidden.rebuild();
                                    }, 1200);
                                }
                            }
                            onRotinaChange={this.handleRotinaChange}
                        />


                        {/* has equipments */}
                        <Grid container direction="row" justify="space-between" alignitems="top" spacing={0}
                              maxwidth={'100%'} margin={0}>

                            {/* has orderPlanSimplified */}
                            {this.state.isSimplified && this.state.equipments[0].relatorioTrabalho != null &&
                            <Grid container direction="row" justify="space-between" alignitems="top" spacing={0}
                                  maxwidth={'100%'} margin={0}>
                                <Grid item xs={12}>

                                    <Wrapper padding="32px 42px 16px 42px">
                                        {this.state.isCurative &&
                                        <div>
                                            <div className={"p-b-15"}>
                                                <Text b>Pedido do Cliente</Text> <br/>
                                                <Text p>{this.state.equipments[0].customerRequest}</Text>
                                            </div>
                                            <div className={"p-b-15"}>
                                                <Text b>Descrição da Avaria</Text>
                                                <Text p>{this.state.equipments[0].malfunctionDescription}</Text>
                                            </div>
                                        </div>
                                        }
                                    </Wrapper>

                                    <Wrapper padding="0px 22px 64px 22px">
                                        <div style={{borderRadius: '8px'}}
                                             className={"bg-bg-grey p-l-20 p-r-20 p-t-20 p-b-20"}>
                                            <div className={"p-b-10"}>
                                                <Text b>Relatório de Trabalho</Text>
                                            </div>
                                            <Grid container direction="row" justify="space-between" alignitems="top"
                                                  spacing={0}
                                                  maxwidth={'100%'} margin={0}>
                                                <Grid item xs={12} md={9}>
                                                    <Input
                                                        multiline rows={8}
                                                        placeholder={"Relatório..."}
                                                        className={"bg-bg-white"}
                                                        $value={this.state.equipments[0].$relatorioTrabalho}
                                                        disabled={this.state.equipments[0].estadoFinal > 0}
                                                        onBlur={() => {
                                                        }}
                                                        onChange={this.savePlans}>

                                                    </Input>
                                                </Grid>
                                            </Grid>
                                        </div>
                                    </Wrapper>

                                </Grid>
                            </Grid>
                            }
                            <Grid item xs={8} sm={8} md={6}
                                  className={"before-to-scroll"}
                                  ref={el => this.beforetoscroll = el}>
                                <Sticky scrollElement=".scrollarea" style={{zIndex: 10}} topOffset={-65}
                                        className={(!this.state.isSimplified || this.state.isLoading ? "" : " display-none")}>
                                    <div className={"bg-bg-white"}>
                                        <PlanHeader>
                                            <Wrapper padding="32px 0px 16px 32px">
                                                <PlanActions
                                                    planMaintenance={this.state.planMaintenance}
                                                    planQuality={this.state.planQuality}
                                                    planQuantity={this.state.planQuantity}
                                                    position={this.state.position}
                                                    ref={el => this.planActionsRef = el}
                                                    onSelect={(item) => {
                                                        //this.planActionsRef.setState({ position: item });
                                                        switch (item) {
                                                            case 0:
                                                                this.handleScrollTo(this.maintenanceRef);
                                                                break;
                                                            case 1:
                                                                this.handleScrollTo(this.qualitativoRef);
                                                                break;
                                                            case 2:
                                                                this.handleScrollTo(this.quantitativo);
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                    }}
                                                />
                                            </Wrapper>
                                        </PlanHeader>
                                    </div>
                                </Sticky>
                                <Wrapper padding={"0 0 0 32px"}
                                         className={(!this.state.isSimplified || this.state.isLoading ? "" : " display-none")}>
                                    <div ref={(el) => this.maintenanceRef = el}
                                         style={{position: 'relative', top: '-400px'}}></div>
                                    {this.state.planMaintenance.length > 0 && this.state.planMaintenance
                                        .filter((item) => {
                                            return item.rotinasList.indexOf(this.state.rotinaId) > -1;
                                        })
                                        .map((item, index) => {
                                            return (
                                                <PlanRow odd={index % 2 == 0} key={index} text>
                                                    <PlanRowText p>{item.descricao}</PlanRowText>
                                                </PlanRow>
                                            );
                                        })}
                                    <div ref={(el) => this.qualitativoRef = el}
                                         style={{position: 'relative', top: '-200px'}}></div>
                                    <Wrapper padding="16px"><Text b>&nbsp;</Text></Wrapper>
                                    {this.state.planQuality.length > 0 &&
                                    <Waypoint
                                        onLeave={this.waipointQualitativoHandlerLeave}
                                        topOffset="50%"
                                        onEnter={this.waipointQualitativoHandlerLeave}
                                    >
                                        <Wrapper padding="16px"><Text b>Qualitativo</Text></Wrapper>
                                    </Waypoint>
                                    }
                                    {this.state.planQuality.length > 0 && this.state.planQuality
                                        .filter((item) => {
                                            return item.rotinasList.indexOf(this.state.rotinaId) > -1;
                                        })
                                        .map((item, index) => {
                                            return (
                                                <PlanRow odd={index % 2 == 0} key={index} text>
                                                    <PlanRowText p>{item.descricao}</PlanRowText>
                                                </PlanRow>
                                            );
                                        })}
                                    <div ref={(el) => this.quantitativo = el}
                                         style={{position: 'relative', top: '-200px'}}></div>
                                    <Wrapper padding="16px"><Text b>&nbsp;</Text></Wrapper>
                                    {this.state.planQuantity.length > 0 &&
                                    <Waypoint
                                        onLeave={this.waipointQuantitativoHandlerLeave}
                                        topOffset="50%"
                                        onEnter={this.waipointQuantitativoHandlerLeave}
                                    >
                                        <Wrapper padding="16px"><Text b>Quantitativo</Text></Wrapper>
                                    </Waypoint>
                                    }
                                    {this.state.planQuantity.length > 0 && this.state.planQuantity
                                        .filter((item) => {
                                            return item.rotinasList.indexOf(this.state.rotinaId) > -1;
                                        })
                                        .map((item, index) => {
                                            return (
                                                <PlanRow odd={index % 2 == 0} key={index} text>
                                                    <PlanRowText p>{item.descricao}</PlanRowText>
                                                </PlanRow>
                                            );
                                        })}
                                    <Wrapper padding="32px"><Text b>&nbsp;</Text></Wrapper>
                                    <div className="p-t-30"></div>
                                </Wrapper>
                                <Wrapper padding="0" minHeight="164px"
                                         background={Color(this.props.theme.palette.secondary.default).alpha(0.2).toString()}>
                                    <Wrapper padding="48px 16px 56px 50px">
                                        <Text h2><b>Estado final</b></Text>
                                    </Wrapper>
                                </Wrapper>
                            </Grid>
                            <Grid item xs={4} sm={4} md={6}
                                  className={"before-to-scroll" + (this.state.equipments.length > 0 || this.state.isLoading ? "" : " display-none")}
                                  style={{overflow: 'hidden'}}
                            >
                                <Sticky scrollElement=".scrollarea" style={{zIndex: 10}} topOffset={-65}
                                        className={(!this.state.isSimplified || this.state.isLoading ? "" : " display-none")}>
                                    <PlanHeader className={this.getScrollShadow()}
                                                ref={(el) => {
                                                    this.equipmentsHeaderWrapper = el
                                                }}>
                                        <div
                                            className={"h100 _scroll-bars"}
                                            style={{
                                                //overflow: 'hidden'
                                            }}
                                            //autoHide
                                            //autoHideTimeout={700}
                                            //autoHideDuration={200}
                                            //thumbMinSize={30}

                                            ref={el => this.planEquipmentsHeaderScroll = el}
                                            onScroll={(e) => {
                                                ReactDOM.findDOMNode(this.planContent).scrollLeft = e.target.scrollLeft;
                                                var className = ReactDOM.findDOMNode(this.equipmentsHeaderWrapper).className;
                                                if (e.target.scrollLeft > 0 && className.indexOf('left') == -1) {
                                                    ReactDOM.findDOMNode(this.equipmentsHeaderWrapper).className += ' left ';
                                                }
                                                if (e.target.scrollLeft == 0) {
                                                    ReactDOM.findDOMNode(this.equipmentsHeaderWrapper).className = className.replace('left', '');
                                                }
                                                var _className = ReactDOM.findDOMNode(this.planContentWrapper).className;
                                                if (e.target.scrollLeft > 0 && _className.indexOf('left') == -1) {
                                                    ReactDOM.findDOMNode(this.planContentWrapper).className += ' left ';
                                                }
                                                if (e.target.scrollLeft == 0) {
                                                    ReactDOM.findDOMNode(this.planContentWrapper).className = _className.replace('left', '');
                                                }
                                            }}>

                                            <Wrapper padding="0 0 0 0"
                                                //style={{overflow: 'visible'}}
                                                     ref={el => this.planEquipmentsHeader = el}>
                                                <PlanEquipmentsHeader equipments={this.state.equipments}/>
                                            </Wrapper>

                                        </div>
                                    </PlanHeader>
                                </Sticky>

                                <div ref={el => this.planContentWrapper = el} className={this.getScrollShadow()}
                                     style={{position: 'relative', overflow: 'hidden', height: '100%'}}
                                >
                                    <div
                                        //className={"scroll-container natural-scroll"}
                                        ref={el => this.planContent = el}
                                        //nativeMobileScroll={true}
                                        style={{overflow: 'auto', height: '100%'}}
                                        onScroll={(e) => {

                                            ReactDOM.findDOMNode(this.planEquipmentsHeader).scrollLeft = e.target.scrollLeft;
                                            ReactDOM.findDOMNode(this.planEquipmentsHeaderScroll).scrollLeft = e.target.scrollLeft;
                                            var className = ReactDOM.findDOMNode(this.equipmentsHeaderWrapper).className;
                                            if (e.target.scrollLeft > 0 && className.indexOf('left') == -1) {
                                                ReactDOM.findDOMNode(this.equipmentsHeaderWrapper).className += ' left ';
                                            }
                                            if (e.target.scrollLeft == 0) {
                                                ReactDOM.findDOMNode(this.equipmentsHeaderWrapper).className = className.replace('left', '');
                                            }

                                            var _className = ReactDOM.findDOMNode(this.planContentWrapper).className;
                                            if (e.target.scrollLeft > 0 && _className.indexOf('left') == -1) {
                                                ReactDOM.findDOMNode(this.planContentWrapper).className += ' left ';
                                            }
                                            if (e.target.scrollLeft == 0) {
                                                ReactDOM.findDOMNode(this.planContentWrapper).className = _className.replace('left', '');
                                            }
                                        }}>
                                        {/* form */}

                                        <div
                                            className={(!this.state.isSimplified || this.state.isLoading ? "" : " display-none")}>
                                            {this.state.planMaintenance.length > 0 && this.state.planMaintenance
                                                .filter((item) => {
                                                    return item.rotinasList.indexOf(this.state.rotinaId) > -1;
                                                })
                                                .map((item, index) => {

                                                    return (
                                                        <PlanRow odd={index % 2 == 0} key={index} right
                                                                 width={this.state.equipmentsHeaderScroll.innerWidth - 42}
                                                                 style={{marginRight: '50px'}}
                                                                 className={"form__row min-width-95"}>
                                                            {this.state.equipmentsCheckBoxHtml}
                                                            {this.state.equipments.map((e, i) => {
                                                                return (
                                                                    <PlanEquipmentsItem
                                                                        key={index + '' + i + i.descricao}>
                                                                        <span className={"inline-block form__item" +
                                                                        (e.planMaintenance[index].$observacoes.value != null &&
                                                                        e.planMaintenance[index].$observacoes.value != "" ?
                                                                            " form__item--commented" :
                                                                            "")}>
                                                                        <MultipleCheckBox
                                                                            $value={e.planMaintenance[index].$resultado}
                                                                            onChange={this.savePlans}
                                                                            disabled={e.estadoFinal > 0}
                                                                        />
                                                                        </span>

                                                                    </PlanEquipmentsItem>
                                                                )
                                                            })}
                                                            <Menu
                                                                action={
                                                                    <Button iconSolo
                                                                            disabled={completed}
                                                                    >
                                                                        <Icon row-menu/>
                                                                    </Button>
                                                                }
                                                                containerStyle={{
                                                                    float: 'right',
                                                                    display: 'inline-block',
                                                                    marginTop: '5px'
                                                                }}>
                                                                <MenuItem onClick={() => {
                                                                    item.open = true;
                                                                    this.setState({toUpdate: this.state.toUpdate + 1});
                                                                }}>
                                                                    Observações
                                                                </MenuItem>
                                                            </Menu>
                                                            <ModalComments open={item.open || false}
                                                                           description={item.descricao}
                                                                           $equipments={this.state.$equipments}
                                                                           category={'planMaintenance'}
                                                                           itemIndex={index}
                                                                           onClose={() => {
                                                                               item.open = false;
                                                                               this.setState({toUpdate: this.state.toUpdate + 1}, () => {
                                                                                   this.savePlans();
                                                                               });
                                                                           }}>
                                                            </ModalComments>
                                                        </PlanRow>
                                                    );
                                                })}
                                            <Wrapper padding="16px"> <Text b>&nbsp;</Text></Wrapper>
                                            {this.state.planQuality.length > 0 &&
                                            <Wrapper padding="16px"><Text b>&nbsp;</Text></Wrapper>}
                                            {this.state.planQuality.length > 0 && this.state.planQuality
                                                .filter((item) => {
                                                    return item.rotinasList.indexOf(this.state.rotinaId) > -1;
                                                })
                                                .map((item, index) => {
                                                    return (
                                                        <PlanRow odd={index % 2 == 0} key={index} right
                                                                 width={this.state.equipmentsHeaderScroll.innerWidth - 42}
                                                                 className={"min-width-95"}>
                                                            {this.state.equipments.map((e, i) => {
                                                                renderCount++;
                                                                return (
                                                                    <PlanEquipmentsItem
                                                                        key={index + '' + i + i.descricao}>
                                                <span className={"inline-block form__item" +
                                                (e.planQuality[index].$observacoes.value != null &&
                                                e.planQuality[index].$observacoes.value != "" ?
                                                    " form__item--commented" :
                                                    "")}>
                                                <MultipleCheckBox
                                                    $value={e.planQuality[index].$resultado}
                                                    onChange={this.savePlans}
                                                    disabled={e.estadoFinal > 0}
                                                />
                                                </span>
                                                                    </PlanEquipmentsItem>
                                                                )
                                                            })}
                                                            <Menu
                                                                action={<Button iconSolo
                                                                                disabled={completed}><Icon
                                                                    row-menu/></Button>}
                                                                containerStyle={{
                                                                    float: 'right',
                                                                    display: 'inline-block',
                                                                    marginTop: '5px'
                                                                }}>
                                                                <MenuItem onClick={() => {
                                                                    item.open = true;
                                                                    this.setState({toUpdate: this.state.toUpdate + 1});
                                                                }}>
                                                                    Observações
                                                                </MenuItem>
                                                            </Menu>
                                                            <ModalComments open={item.open || false}
                                                                           description={item.descricao}
                                                                           $equipments={this.state.$equipments}
                                                                           category={'planQuality'}
                                                                           itemIndex={index}
                                                                           item={item}
                                                                           onClose={() => {
                                                                               item.open = false;
                                                                               this.setState({toUpdate: this.state.toUpdate + 1}, () => {
                                                                                   this.savePlans();
                                                                               });
                                                                           }}>
                                                            </ModalComments>
                                                        </PlanRow>
                                                    );
                                                })}
                                            <Wrapper padding="16px"><Text b>&nbsp;</Text></Wrapper>
                                            {this.state.planQuantity.length > 0 &&
                                            <Wrapper padding="16px"><Text b>&nbsp;</Text></Wrapper>}
                                            {this.state.planQuantity.length > 0 && this.state.planQuantity
                                                .filter((item) => {
                                                    return item.rotinasList.indexOf(this.state.rotinaId) > -1;
                                                })
                                                .map((item, index) => {

                                                    return (
                                                        <PlanRow odd={index % 2 == 0} key={index} right
                                                                 width={this.state.equipmentsHeaderScroll.innerWidth - 42}
                                                                 className={"min-width-95"}>
                                                            {this.state.equipments.map((e, i) => {
                                                                return (
                                                                    <PlanEquipmentsItem
                                                                        key={index + '' + i + i.descricao}>
                                                                        <Wrapper padding="0 8px"
                                                                                 className={"form__item" +
                                                                                 (e.planQuantity[index].$observacoes.value != null &&
                                                                                 e.planQuantity[index].$observacoes.value != "" ?
                                                                                     " form__item--commented" :
                                                                                     "")}>
                                                                            <Input type="number" step="0.01"
                                                                                   $value={e.planQuantity[index].$resultado}
                                                                                   onChange={this.savePlans}
                                                                                   disabled={e.estadoFinal > 0}
                                                                            />
                                                                        </Wrapper>
                                                                    </PlanEquipmentsItem>
                                                                )
                                                            })}
                                                            <Text span>{item.unidadeCampo1}</Text>

                                                            <Menu
                                                                action={<Button iconSolo
                                                                                disabled={completed}><Icon
                                                                    row-menu/></Button>}
                                                                containerStyle={{
                                                                    float: 'right',
                                                                    display: 'inline-block',
                                                                    marginTop: '5px'
                                                                }}>
                                                                <MenuItem onClick={() => {
                                                                    item.open = true;
                                                                    this.setState({toUpdate: this.state.toUpdate + 1});
                                                                }}>
                                                                    Observações
                                                                </MenuItem>
                                                            </Menu>
                                                            <ModalComments open={item.open || false}
                                                                           description={item.descricao}
                                                                           $equipments={this.state.$equipments}
                                                                           category={'planQuantity'}
                                                                           itemIndex={index}
                                                                           item={item}
                                                                           onClose={() => {
                                                                               item.open = false;
                                                                               this.setState({toUpdate: this.state.toUpdate + 1});
                                                                               this.savePlans();
                                                                           }}>
                                                            </ModalComments>
                                                        </PlanRow>
                                                    );
                                                })}
                                            <Wrapper padding="32px"><Text b>&nbsp;</Text></Wrapper>
                                            <div className="p-t-30"></div>
                                        </div>
                                        <Wrapper
                                            padding="0"
                                            minHeight="164px"
                                            background={Color(this.props.theme.palette.secondary.default).alpha(0.2).toString()}
                                            width={this.state.equipmentsHeaderScroll.innerWidth + 'px'}
                                            margin="none"
                                            style={{minWidth: "100%"}}>
                                            <Wrapper padding="48px 12px 56px" margin="none"
                                                     ref={el => this.finalStateEl = el}>
                                                {this.state.equipments.map((equipment, i) => {
                                                    console.log(equipment);
                                                    return (
                                                        <PlanEquipmentsItem key={i}>
                                                            <Wrapper padding="0 8px">
                                                                <FinalState
                                                                    $value={equipment.$estadoFinal}
                                                                    $message={equipment.$observacao}
                                                                    disabled={equipment.estadoFinal > 0 &&
                                                                    (
                                                                        (equipment.assinaturaCliente != "" && equipment.assinaturaCliente != null) ||
                                                                        (equipment.assinaturaSie != "" && equipment.assinaturaSie != null &&
                                                                            (equipment.assinaturaSieIgualCliente == true || equipment.assinaturaClienteManual == true))
                                                                    )}
                                                                    onChange={() => {

                                                                        //this.finalStateEl.current.blur();
                                                                        if (this.state.order.technicals.filter((item) => {
                                                                            return item.id == this.state.currentUser.id;
                                                                        }).length < 1) {
                                                                            this.state.order.$technicals.value = this.state.order.$technicals.value.unshift(this.state.currentUser);
                                                                            this.state.order.$technicals.value = this.state.order.technicals.filter((el, i) => i < 4);
                                                                        }

                                                                        this.state.equipments.map((item) => {
                                                                            item.assinaturaSie = "";
                                                                            item.assinaturaCliente = "";
                                                                        });

                                                                        this.setState({toUpdate: this.state.toUpdate + 1}, () => {
                                                                            this.savePlans();
                                                                        });
                                                                    }}
                                                                    brand={equipment.marcaText}
                                                                    model={equipment.modeloText}
                                                                    serialNumber={equipment.numSerie}
                                                                    inventoryNumber={equipment.numInventario}
                                                                    isCurative={this.state.isCurative}
                                                                    isSimplified={this.state.isSimplified}/>
                                                            </Wrapper>
                                                        </PlanEquipmentsItem>
                                                    )
                                                })}
                                            </Wrapper>
                                        </Wrapper>

                                    </div>
                                </div>
                            </Grid>
                        </Grid>
                    </div>
                </Wrapper>
                <TooltipHidden/>
            </PageTemplate>
        )
    }
}

export default withTheme(withRouter(FichaDeManutencao));