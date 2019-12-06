// https://github.com/diegohaz/arc/wiki/Atomic-Design
/*react-styleguide: ignore*/
import React, {Component} from 'react';
import _theme from '../../../../themes/default';
import {
    Button,
    Text,
    Icon,
    Wrapper,
    CheckBox,
    ModalLarge,
    Spacer
} from 'components';
import {Grid} from '@material-ui/core';
import SignaturePad from 'react-signature-pad-wrapper'
import "./index.scss";

const {DialogContent} = ModalLarge;

class Signature extends Component {
    state = {
        clientSignaturePadOpen: false,
        clientSignaturePadPng: null,
        technicalSignaturePadOpen: false,
        technicalSignaturePadPng: null,
        sieSignaturePadOpen: false,
        sieSignaturePadPng: null,
        selectedEquipments: [],
        assinaturaSieIgualCliente: false,
        assinaturaClienteManual: false
    }

    constructor(props) {
        super(props);
        this.state.selectedEquipments = this.props.$equipments.value;

        this.state.clientSignaturePadPng = this.state.selectedEquipments && this.state.selectedEquipments[0] ? this.state.selectedEquipments[0].assinaturaCliente : null;
        this.state.technicalSignaturePadPng = this.state.selectedEquipments && this.state.selectedEquipments[0] ? this.state.selectedEquipments[0].assinaturaTecnico : null;
        this.state.sieSignaturePadPng = this.state.selectedEquipments && this.state.selectedEquipments[0] ? this.state.selectedEquipments[0].assinaturaSie : null;

        let assinaturaSieIgualClienteCount = 0;
        this.state.selectedEquipments.map((s) => {
            if (s.$assinaturaSieIgualCliente.value == true) {
                assinaturaSieIgualClienteCount++;
            }
        });

        let assinaturaClienteManualCount = 0;
        this.state.selectedEquipments.map((s) => {
            if (s.$assinaturaClienteManual.value == true) {
                assinaturaClienteManualCount++;
            }
        });

        this.state.assinaturaSieIgualCliente = this.state.selectedEquipments.length == assinaturaSieIgualClienteCount;
        this.state.assinaturaClienteManual = this.state.selectedEquipments.length == assinaturaClienteManualCount;
    }

    render() {

        return (

            <Grid container direction="row" justify="space-between" alignitems="top" spacing={0}
                  maxwidth={'100%'} margin={0} className={'signature'}>

                <Grid item xs={12} md={6} lg={6} xl={5}
                      className={(this.props.$equipments && this.props.$equipments.value.length < 1) ? "content-disabled" : ""}
                      style={{borderRight: '1px solid #E4E7EB'}}>
                    <DialogContent>
                        <Text h2>1<sup>a</sup> Fase</Text>
                        <br/>
                        <div>
                            <Text b>Técnico</Text>
                            {/*<Text span className="pull-right"> <small>Nº Mecanográfico: 215411</small></Text>*/}
                            <div className="clearfix"></div>
                        </div>
                        <Spacer height={"10px"}/>
                        <div style={{position: 'relative'}}>
                            {/* <Input placeholder={"Assinatura"} onClick={() => this.setState({ signaturePadOpen: true })} /> */}
                            <Button icon={<Icon signature/>} onClick={() => {
                                this.setState({technicalSignaturePadOpen: true}, () => {
                                    if (this.state.technicalSignaturePadPng) {
                                        this.technicalSignaturePad.fromDataURL(this.state.technicalSignaturePadPng);
                                    }
                                });
                            }}></Button>
                            {this.state.technicalSignaturePadPng &&
                            <Wrapper inline padding="0 10px">
                                <Icon approved style={{color: _theme.palette.alert.good}}/>
                            </Wrapper>}
                        </div>
                        <div className="p-b-40"></div>
                        <div>
                            <Text b>SIE / Aprovisionamento</Text>
                        </div>
                        <Spacer height={"10px"}/>
                        <div style={{position: 'relative'}}>
                            {/* <Input placeholder={"Assinatura"} onClick={() => this.setState({ signaturePadOpen: true })} /> */}
                            <Button icon={<Icon signature/>}
                                    onClick={() => {
                                        this.setState({sieSignaturePadOpen: true}, () => {
                                            if (this.state.sieSignaturePadPng) {
                                                this.sieSignaturePad.fromDataURL(this.state.sieSignaturePadPng);
                                            }
                                        }, () => {
                                            this.props.onChange();
                                        });
                                    }}
                            ></Button>
                            {this.state.sieSignaturePadPng &&
                            <Wrapper inline padding="0 10px">
                                <Icon approved style={{color: _theme.palette.alert.good}}/>
                            </Wrapper>
                            }
                        </div>
                        <Spacer height={"10px"}/>
                        <CheckBox
                            checked={this.state.assinaturaSieIgualCliente}
                            id="sieisclient"
                            onChange={(e) => {
                                let value = e.target.checked;
                                if (this.props.$equipments) {
                                    this.props.$equipments.value.map((s) => {
                                        s.$assinaturaSieIgualCliente.value = value;
                                        if (value) {
                                            s.$assinaturaClienteManual.value = false;
                                        }
                                    });
                                }

                                this.setState({
                                    assinaturaSieIgualCliente: value,
                                    assinaturaClienteManual: false
                                }, () => {
                                    this.props.onChange();
                                });
                            }}/>
                        <label htmlFor="sieisclient" className="pointer">
                            <Text span>Assinatura é igual à do cliente</Text>
                        </label>

                    </DialogContent>
                </Grid>
                <Grid item xs={12} md={6} lg={6} xl={5}
                      className={(this.props.$equipments && this.props.$equipments.value.length < 1) || (this.state.assinaturaSieIgualCliente) ? "content-disabled" : ""}>
                    <DialogContent>
                        <Text h2>2<sup>a</sup> Fase</Text>
                        <br/>
                        <div className={(this.state.assinaturaClienteManual) ? "content-disabled" : ""}>
                            <div>
                                <Text b>Cliente</Text>
                            </div>
                            <Spacer height={"10px"}/>
                            <div style={{position: 'relative'}}>
                                <Button icon={<Icon signature/>}
                                        onClick={() => {
                                            this.setState({clientSignaturePadOpen: true}, () => {
                                                if (this.state.clientSignaturePadPng) {
                                                    this.clientSignaturePad.fromDataURL(this.state.clientSignaturePadPng);
                                                }
                                            }, () => {
                                                this.props.onChange();
                                            });
                                        }}
                                ></Button>
                                {this.state.clientSignaturePadPng && <Wrapper inline padding="0 10px">
                                    <Icon approved style={{color: _theme.palette.alert.good}}/></Wrapper>}
                            </div>
                        </div>
                        <Spacer height={"10px"}/>
                        <CheckBox
                            checked={this.state.assinaturaClienteManual}
                            id="assinaturamanual"
                            onChange={(e) => {
                                let value = e.target.checked;
                                if (this.props.$equipments) {
                                    this.props.$equipments.value.map((s) => {
                                        s.$assinaturaClienteManual.value = value;
                                    });
                                }

                                this.setState({assinaturaClienteManual: value}, () => {
                                    this.props.onChange();
                                });

                            }}
                        />
                        <label htmlFor="assinaturamanual" className="pointer">
                            <Text span>Assinatura manual</Text>
                        </label>
                    </DialogContent>
                </Grid>
                {this.state.clientSignaturePadOpen &&
                <div className={"signature-pad__wrapper signature-pad__wrapper--open"}>
                    <div className="signature-pad__close__wrapper">
                        <Button iconSolo><Icon decline
                                               onClick={() => this.setState({clientSignaturePadOpen: false})}/></Button>
                    </div>
                    <SignaturePad height={window.innerHeight} className="signature-pad" redrawOnResize={true}
                                  options={{minWidth: 5, maxWidth: 13 /*, penColor: 'rgb(66, 133, 244)'*/}}
                                  velocityFilterWeight={2}
                                  throttle={25}
                                  ref={ref => this.clientSignaturePad = ref}/>
                    <div className="signature-pad__actions">
                        <Button default onClick={() => this.clientSignaturePad.clear()}
                                color="primary">Apagar</Button>
                        <Button primary onClick={() => {
                            var image = this.clientSignaturePad.toDataURL();
                            if (this.clientSignaturePad.isEmpty()) {
                                image = "";
                            }
                            if (this.props.$equipments) {
                                this.props.$equipments.value.map((s) => {
                                    s.$assinaturaCliente.value = image;
                                });
                            }
                            this.setState({clientSignaturePadPng: image, clientSignaturePadOpen: false}, () => {
                                if (this.props.onChange) {
                                    this.props.onChange();
                                }
                            });
                        }} color="primary">Guardar</Button>
                    </div>
                </div>
                }
                {this.state.sieSignaturePadOpen &&
                <div className={"signature-pad__wrapper signature-pad__wrapper--open"}>
                    <div className="signature-pad__close__wrapper">
                        <Button iconSolo><Icon decline
                                               onClick={() => this.setState({sieSignaturePadOpen: false})}/></Button>
                    </div>
                    <SignaturePad height={window.innerHeight} className="signature-pad" redrawOnResize={true}
                                  velocityFilterWeight={2}
                                  throttle={25}
                                  options={{minWidth: 5, maxWidth: 13 /*, penColor: 'rgb(66, 133, 244)'*/}}
                                  ref={ref => this.sieSignaturePad = ref}/>
                    <div className="signature-pad__actions">
                        <Button default onClick={() => this.sieSignaturePad.clear()} color="primary">Apagar</Button>
                        <Button primary onClick={() => {
                            var image = this.sieSignaturePad.toDataURL();
                            if (this.sieSignaturePad.isEmpty()) {
                                image = "";
                            }
                            if (this.props.$equipments) {
                                this.props.$equipments.value.map((s) => {
                                    s.$assinaturaSie.value = image;
                                });
                            }
                            this.setState({sieSignaturePadPng: image, sieSignaturePadOpen: false}, () => {
                                if (this.props.onChange) {
                                    this.props.onChange();
                                }
                            });
                        }} color="primary">Guardar</Button>
                    </div>
                </div>
                }
                {this.state.technicalSignaturePadOpen &&
                <div className={"signature-pad__wrapper signature-pad__wrapper--open"}>
                    <div className="signature-pad__close__wrapper">
                        <Button iconSolo><Icon decline
                                               onClick={() => this.setState({technicalSignaturePadOpen: false})}/></Button>
                    </div>
                    <div className="signature-pad">
                        <SignaturePad height={window.innerHeight}
                                      redrawOnResize={true}
                                      velocityFilterWeight={2}
                                      throttle={25}
                                      options={{minWidth: 5, maxWidth: 13 /*, penColor: 'rgb(66, 133, 244)'*/}}
                                      ref={ref => this.technicalSignaturePad = ref}/>
                    </div>
                    <div className="signature-pad__actions">
                        <Button default onClick={() => this.technicalSignaturePad.clear()}
                                color="primary">Apagar</Button>
                        <Button primary onClick={() => {
                            var image = this.technicalSignaturePad.toDataURL();
                            if (this.technicalSignaturePad.isEmpty()) {
                                image = "";
                            }
                            if (this.props.$equipments) {
                                this.props.$equipments.value.map((s) => {
                                    s.$assinaturaTecnico.value = image;
                                    s.$utilizadorAssinaturaTecnico.value = this.props.$currentUser.value;
                                });
                            }
                            this.setState({technicalSignaturePadPng: image, technicalSignaturePadOpen: false}, () => {
                                if (this.props.onChange) {
                                    this.props.onChange();
                                }
                            });
                        }} color="primary">Guardar</Button>
                    </div>
                </div>
                }
            </Grid>
        )
    }
}

export default Signature;