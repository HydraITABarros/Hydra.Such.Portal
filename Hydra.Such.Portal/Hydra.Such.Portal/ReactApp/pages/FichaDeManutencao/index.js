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
import './index.scss';

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
        selectedOptionsEl: null
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

        this.fetch();
    }

    shouldComponentUpdate(nextProps, nextState) {

        if (nextState.toUpdate != this.state.toUpdate) {
            return true;
        }
        if (nextState.$equipmentsCount && nextState.$equipmentsCount.value != this.state.equipmentsCount) {
            nextState.equipmentsCount = nextState.$equipmentsCount.value;
            return true;
        }
        if (this.state.equipmentsHeaderScroll.innerWidth != nextState.equipmentsHeaderScroll.innerWidth) {
            return true;
        }
        return nextState.isLoading !== this.state.isLoading || nextState.order !== this.state.order || nextState.equipmentsCount != this.state.equipmentsCount;
    }

    componentDidMount() {
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
        }, 500)

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
                    title: data.equipments.length > 0 ? data.equipments[0].categoriaText : "",
                    service: data.equipments.length > 0 ? data.equipments[0].servicoText : "",
                    room: data.equipments.length == 1 ? data.equipments[0].sala : "",
                    equipments: data.equipments,
                    planMaintenance: data.planMaintenance,
                    planQuality: data.planQuality,
                    planQuantity: data.planQuantity,
                    equipmentsCount: data.equipments.length,
                    currentUser: data.currentUser
                }

                addLinkedPropsToObject(state, this);

                console.log(state);

                state.planMaintenanceHtml = data.planMaintenance.map((item, index) => {
                    return (
                        <PlanRow odd={index % 2 == 0} key={index} text>
                            <PlanRowText p>{item.descricao}</PlanRowText>
                        </PlanRow>
                    );
                });
                state.planQualityHtml = data.planQuality.map((item, index) => {
                    return (
                        <PlanRow odd={index % 2 == 0} key={index} text>
                            <PlanRowText p>{item.descricao}</PlanRowText>
                        </PlanRow>
                    );
                });
                state.planQuantityHtml = data.planQuantity.map((item, index) => {
                    return (
                        <PlanRow odd={index % 2 == 0} key={index} text>
                            <PlanRowText p>{item.descricao}</PlanRowText>
                        </PlanRow>
                    );
                });
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
    }

    postPlans() {
        console.log(this.state.equipments);

        var url = `/ordens-de-manutencao/ficha-de-manutencao`;

        axios.post(url + "?orderId=" + this.state.orderId,
            this.state.equipments.map((item) => {
                var retval = {};
                Object.keys(item).map(k => {
                    if (k.indexOf('$') > -1) {
                        return;
                    }
                    retval[k] = item[k];
                })
                return retval;
            })
        ).then((result) => {
        });
    }

    render() {
        return (
            <PageTemplate>
                <Wrapper padding={'0 0 0'} width="100%" minHeight="274px" ref={el => this.pageWrapper = el}>
                    <Breadcrumb order={this.state.order} onRef={el => this.breadcrumbWrapper = el}/>
                    <div className="scrollarea" style={{height: this.state.bodyHeight + 'px', overflow: 'auto'}}
                         ref={el => this.mainScroll = el}>
                        <Sticky scrollElement=".scrollarea" style={{zIndex: 11}}>
                            <HeaderTitle
                                title={this.state.title}
                                order={this.state.order}
                                onRef={el => this.headerTitleWrapper = el}
                                $equipments={this.state.$equipments}
                                $currentUser={this.state.$currentUser}

                            />
                        </Sticky>
                        <HeaderDescription
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
                        />
                        <Grid container direction="row" justify="space-between" alignitems="top" spacing={0}
                              maxwidth={'100%'} margin={0}>
                            <Grid item xs={8} sm={8} md={6} className="before-to-scroll"
                                  ref={el => this.beforetoscroll = el}>
                                <Sticky scrollElement=".scrollarea" style={{zIndex: 10}} topOffset={-65}>
                                    <div className="bg-bg-white">
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
                                <Wrapper padding="0 0 0 32px">
                                    <div ref={(el) => this.maintenanceRef = el}
                                         style={{position: 'relative', top: '-400px'}}></div>
                                    {this.state.planMaintenance.length > 0 && this.state.planMaintenanceHtml}
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
                                    {this.state.planQuality.length > 0 && this.state.planQualityHtml}
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
                                    {this.state.planQuantity.length > 0 && this.state.planQuantityHtml}
                                    <Wrapper padding="32px"><Text b>&nbsp;</Text></Wrapper>
                                </Wrapper>
                                <Wrapper padding="0" minHeight="164px"
                                         background={Color(this.props.theme.palette.secondary.default).alpha(0.2).toString()}>
                                    <Wrapper padding="48px 16px 56px 50px">
                                        <Text h2><b>Estado final</b></Text>
                                    </Wrapper>
                                </Wrapper>
                            </Grid>
                            <Grid item xs={4} sm={4} md={6}
                                  style={{overflow: 'hidden'}}

                            >
                                <Sticky scrollElement=".scrollarea" style={{zIndex: 10}} topOffset={-65}>
                                    <PlanHeader className={this.getScrollShadow()}
                                                ref={(el) => {
                                                    this.equipmentsHeaderWrapper = el
                                                }}>
                                        <div className="scroll-container h100 natural-scroll bg-bg-white"
                                             ref={el => this.planEquipmentsHeader = el}
                                            //nativeMobileScroll={true}
                                             style={{overflow: 'scroll'}}
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
                                            {/* <Wrapper innerRef={el => this.planEquipmentsHeader = el} > */}
                                            <Wrapper padding="0  32px 0 0">
                                                <PlanEquipmentsHeader equipments={this.state.equipments}/>
                                            </Wrapper>
                                        </div>
                                    </PlanHeader>
                                </Sticky>

                                <div ref={el => this.planContentWrapper = el} className={this.getScrollShadow()}
                                     style={{position: 'relative', overflow: 'scroll'}}
                                >
                                    <div
                                        className={"scroll-container natural-scroll"} ref={el => this.planContent = el}
                                        //nativeMobileScroll={true}
                                        style={{overflow: 'scroll'}}
                                        onScroll={(e) => {
                                            ReactDOM.findDOMNode(this.planEquipmentsHeader).scrollLeft = e.target.scrollLeft;
                                            var className = ReactDOM.findDOMNode(this.equipmentsHeaderWrapper).className;
                                            if (ee.target.scrollLeft > 0 && className.indexOf('left') == -1) {
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
                                        <Wrapper>
                                            <Wrapper padding="0  32px 0 0">
                                                {this.state.planMaintenance.length > 0 && this.state.planMaintenance.map((item, index) => {

                                                    return (
                                                        <PlanRow odd={index % 2 == 0} key={index} right
                                                                 width={this.state.equipmentsHeaderScroll.innerWidth - 32}
                                                                 className={"form__row"}>
                                                            {this.state.equipmentsCheckBoxHtml}
                                                            {this.state.equipments.map((e, i) => {
                                                                return (
                                                                    <PlanEquipmentsItem key={index + '' + i}>
																		<span className={"inline-block form__item" +
                                                                        (e.planMaintenance[index].$observacoes.value != null &&
                                                                        e.planMaintenance[index].$observacoes.value != "" ?
                                                                            " form__item--commented" :
                                                                            "")}>
																			<MultipleCheckBox
                                                                                $value={e.planMaintenance[index].$resultado}/>
																		</span>
                                                                    </PlanEquipmentsItem>
                                                                )
                                                            })}
                                                            <Menu action={<Button iconSolo><Icon row-menu/></Button>}
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
                                                                               this.setState({toUpdate: this.state.toUpdate + 1});
                                                                           }}>
                                                            </ModalComments>
                                                        </PlanRow>
                                                    );
                                                })}
                                                <Wrapper padding="16px"> <Text b>&nbsp;</Text></Wrapper>
                                                {this.state.planQuality.length > 0 &&
                                                <Wrapper padding="16px"><Text b>&nbsp;</Text></Wrapper>}
                                                {this.state.planQuality.length > 0 && this.state.planQuality.map((item, index) => {
                                                    return (
                                                        <PlanRow odd={index % 2 == 0} key={index} right
                                                                 width={this.state.equipmentsHeaderScroll.innerWidth - 32}>
                                                            {this.state.equipments.map((e, i) => {
                                                                renderCount++;
                                                                return (
                                                                    <PlanEquipmentsItem key={index + '' + i}>
																		<span className={"inline-block form__item" +
                                                                        (e.planQuality[index].$observacoes.value != null &&
                                                                        e.planQuality[index].$observacoes.value != "" ?
                                                                            " form__item--commented" :
                                                                            "")}>
																			<MultipleCheckBox
                                                                                $value={e.planQuality[index].$resultado}/>
																		</span>
                                                                    </PlanEquipmentsItem>
                                                                )
                                                            })}
                                                            <Menu action={<Button iconSolo><Icon row-menu/></Button>}
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
                                                                           category={'planQuality'} itemIndex={index}
                                                                           item={item}
                                                                           onClose={() => {
                                                                               item.open = false;
                                                                               this.setState({toUpdate: this.state.toUpdate + 1});
                                                                           }}>
                                                            </ModalComments>
                                                        </PlanRow>
                                                    );
                                                })}
                                                <Wrapper padding="16px"><Text b>&nbsp;</Text></Wrapper>
                                                {this.state.planQuantity.length > 0 &&
                                                <Wrapper padding="16px"><Text b>&nbsp;</Text></Wrapper>}
                                                {this.state.planQuantity.length > 0 && this.state.planQuantity.map((item, index) => {
                                                    return (
                                                        <PlanRow odd={index % 2 == 0} key={index} right
                                                                 width={this.state.equipmentsHeaderScroll.innerWidth - 32}>
                                                            {this.state.equipments.map((e, i) => {
                                                                return (
                                                                    <PlanEquipmentsItem key={index + '' + i}>
                                                                        <Wrapper padding="0 8px"
                                                                                 className={"form__item" +
                                                                                 (e.planQuantity[index].$observacoes.value != null &&
                                                                                 e.planQuantity[index].$observacoes.value != "" ?
                                                                                     " form__item--commented" :
                                                                                     "")}>
                                                                            <Input type="number" step="0.01"
                                                                                   $value={e.planQuantity[index].$resultado}/>
                                                                        </Wrapper>
                                                                    </PlanEquipmentsItem>
                                                                )
                                                            })}
                                                            <Text span>{item.unidadeCampo1}</Text>

                                                            <Menu action={<Button iconSolo><Icon row-menu/></Button>}
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
                                                                           category={'planQuantity'} itemIndex={index}
                                                                           item={item}
                                                                           onClose={() => {
                                                                               item.open = false;
                                                                               this.setState({toUpdate: this.state.toUpdate + 1});
                                                                           }}>
                                                            </ModalComments>
                                                        </PlanRow>
                                                    );
                                                })}
                                                <Wrapper padding="32px"><Text b>&nbsp;</Text></Wrapper>
                                            </Wrapper>
                                        </Wrapper>
                                        <Wrapper
                                            padding="0"
                                            minHeight="164px"
                                            background={Color(this.props.theme.palette.secondary.default).alpha(0.2).toString()}
                                            width={this.state.equipmentsHeaderScroll.innerWidth + 'px'}
                                            margin="none"
                                            style={{minWidth: "45vw"}}>
                                            <Wrapper padding="48px 12px 56px" margin="none">
                                                {this.state.equipments.map((equipment, i) => {
                                                    return (
                                                        <PlanEquipmentsItem key={i}>
                                                            <Wrapper padding="0 8px">
                                                                <FinalState
                                                                    $value={equipment.$estadoFinal}
                                                                    $message={equipment.$observacao}
                                                                    onChange={() => {

                                                                        if (this.state.order.technicals.filter((item) => {
                                                                            return item.id == this.state.currentUser.id;
                                                                        }).length < 1) {
                                                                            this.state.order.$technicals.value = this.state.order.$technicals.value.unshift(this.state.currentUser);
                                                                            this.state.order.$technicals.value = this.state.order.technicals.filter((el, i) => i < 4);
                                                                        }

                                                                        this.setState({toUpdate: this.state.toUpdate + 1});
                                                                    }}
                                                                    brand={equipment.marcaText}
                                                                    model={equipment.modeloText}
                                                                    serialNumber={equipment.numSerie}
                                                                    inventoryNumber={equipment.numInventario}/>
                                                                <Button default onClick={() => {
                                                                    this.postPlans();
                                                                }}>Gravar</Button>
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