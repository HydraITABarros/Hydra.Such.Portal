// https://github.com/diegohaz/arc/wiki/Atomic-Design
/*react-styleguide: ignore*/
import React, { Component } from 'react';
import _theme from '../../../themes/default';
import MuiTabs from '@material-ui/core/Tabs';
import MuiTab from '@material-ui/core/Tab';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { Button, Text, Icon, Circle, Wrapper, OmDatePicker, CheckBox, Input, Avatars, ModalLarge, Tooltip, Spacer } from 'components';
import AppBar from '@material-ui/core/AppBar';
import EMM from '../emm';
import Material from '../material';
import Upload from '../upload';
import Documentos from '../documentos';
import Fotografias from '../fotografias';
import { Grid } from '@material-ui/core';
import SignaturePad from 'react-signature-pad-wrapper'
import "./index.scss";
import ReactDOM from 'react-dom';

const { DialogTitle, DialogContent, DialogActions } = ModalLarge;


const Tabs = styled(MuiTabs)`
    [class*="MuiTabs-scroller"]>span{
      background-color: ${_theme.palette.secondary.default};
      height: 5px;
      border-radius: 2.5px;
      z-index: 2;
    }
    [class*="MuiTabs-fixed"]>span{
      margin-left: 0px;
    }
    [class*="icon"] {
            color: ${props => props.theme.palette.primary.medium};
          }
    [aria-selected="true"]  {
          [class*="icon"] {
            color: ${props => props.theme.palette.secondary.default};
          }
    }
`;
const Tab = styled(MuiTab)`&&{
      text-transform: capitalize;
      text-align: left;
      min-width: 0;
    }
    [class*="MuiTab-labelContainer"] {
          padding: 6px 12px;
    }
`;
const Bar = styled(AppBar)`&&{
      background-color: ${_theme.palette.white};
      box-shadow: none;
      margin-bottom: 0px;
      padding-left: 0;
      padding-right: 0;
      hr{position: relative; margin-top: -3px; margin-left: -40px; z-index: 1;}
    }
`;

var canvasWidth = window.innerWidth;

class Options extends Component {
	state = {
		open: false,
		clientSignaturePadOpen: false,
		clientSignaturePadPng: null,
		technicalSignaturePadOpen: false,
		technicalSignaturePadPng: null,
		sieSignaturePadOpen: false,
		sieSignaturePadPng: null,
		selectedEquipments: []
	}
	constructor(props) {
		super(props);
		this.handleChange = this.handleChange.bind(this);
		this.handleClose = this.handleClose.bind(this);
	}
	handleChange(e, value) {
	}
	handleClose() {
		this.setState({ open: false });
	}
	render() {

		canvasWidth = window.innerWidth;

		return (
			<ModalLarge
				onClose={this.handleClose}
				onOpen={() => this.setState({ open: true })}
				open={this.state.open}
				action={this.props.children} children={
					<div className="modal-signature">
						<DialogTitle>
							<Text h2><Icon report /> Assinar</Text>
						</DialogTitle>
						<hr />
						<Grid container direction="row" justify="space-between" alignitems="top" spacing={0} maxwidth={'100%'} margin={0} >
							{/* <DialogContent></DialogContent> */}
							<Grid item xs={12} md={4} lg={3} style={{ borderRight: '1px solid #E4E7EB', borderBottom: '1px solid #E4E7EB' }}>
								<DialogContent>
									<div className="col-xs-10 p-l-0 p-r-0">
										<Text b className="ws-nowrap to-ellipsis">{this.props.equipmentType}</Text>
									</div>
									<div className="col-xs-2 p-l-0 p-r-0">
										<Text b>({this.props.$equipments && this.props.$equipments.value.length})</Text>
									</div>
									<div className="clearfix"></div>
									{/* <div>debug selected count: {this.state.selectedEquipments.length}</div> */}
									{this.props.$equipments && this.props.$equipments.value.map((equipment, i) => {
										return (
											<div key={i} style={{ lineHeight: '32px' }}>

												<div className="w-auto ws-nowrap to-ellipsis v-a-m">
													<Text b className="w-20">#{i + 1}</Text> &nbsp;<Text span>{equipment.numEquipamento}</Text>
												</div>
											</div>
										)
									})}
								</DialogContent>
							</Grid>
							<Grid item xs={12} md={4} lg={4} className={(this.props.$equipments && this.props.$equipments.value.length < 1) ? "content-disabled" : ""} style={{ borderRight: '1px solid #E4E7EB', borderBottom: '1px solid #E4E7EB' }}>
								<DialogContent>
									<Text h2 >1<sup>a</sup> Fase</Text>
									<br />
									<div>
										<Text b>Técnico</Text>
										<Text span className="pull-right"> <small>Nº Mecanográfico: 215411</small></Text>
										<div className="clearfix"></div>
									</div>
									<Spacer height={"10px"} />
									<div style={{ position: 'relative' }}>
										{/* <Input placeholder={"Assinatura"} onClick={() => this.setState({ signaturePadOpen: true })} /> */}
										<Button icon={<Icon signature />} onClick={() => {
											this.setState({ technicalSignaturePadOpen: true }, () => {
												if (this.state.technicalSignaturePadPng) {
													this.technicalSignaturePad.fromDataURL(this.state.technicalSignaturePadPng);
												}
											});
										}} ></Button>
										{this.state.technicalSignaturePadPng && <Wrapper inline padding="0 10px"><Icon approved style={{ color: _theme.palette.alert.good }} /></Wrapper>}
									</div>
									<br /><br /><br />
									<div>
										<Text b>SIE / Aprovisionamento</Text>
									</div>
									<Spacer height={"10px"} />
									<div style={{ position: 'relative' }}>
										{/* <Input placeholder={"Assinatura"} onClick={() => this.setState({ signaturePadOpen: true })} /> */}
										<Button icon={<Icon signature />}
											onClick={() => {
												this.setState({ sieSignaturePadOpen: true }, () => {
													if (this.state.sieSignaturePadPng) {
														this.sieSignaturePad.fromDataURL(this.state.sieSignaturePadPng);
													}
												});
											}}
										></Button>
										{this.state.sieSignaturePadPng && <Wrapper inline padding="0 10px"><Icon approved style={{ color: _theme.palette.alert.good }} /></Wrapper>}
									</div>
									<Spacer height={"10px"} />
									<CheckBox /> <Text span>Assinatura é igual à do cliente</Text>
								</DialogContent>
							</Grid>
							<Grid item xs={12} md={4} lg={4} className={(this.props.$equipments && this.props.$equipments.value.length < 1) ? "content-disabled" : ""}>
								<DialogContent>
									<Text h2>2<sup>a</sup> Fase</Text>
									<br />
									<div>
										<Text b>Cliente</Text>
									</div>
									<Spacer height={"10px"} />
									<div style={{ position: 'relative' }}>
										<Button icon={<Icon signature />}
											onClick={() => {
												this.setState({ clientSignaturePadOpen: true }, () => {
													if (this.state.clientSignaturePadPng) {
														this.clientSignaturePad.fromDataURL(this.state.clientSignaturePadPng);
													}
												});
											}}
										></Button>
										{this.state.clientSignaturePadPng && <Wrapper inline padding="0 10px"><Icon approved style={{ color: _theme.palette.alert.good }} /></Wrapper>}

									</div>
									<Spacer height={"10px"} />
									<CheckBox id="assinaturamanual" /> <label htmlFor="assinaturamanual" className="pointer"><Text span>Assinatura manual</Text></label>
								</DialogContent>
							</Grid>
							<Grid item xs={12} lg={1} >
							</Grid>
						</Grid>
						<hr />
						<DialogActions>
							<Button primary onClose={this.handleClose} color="primary">Finalizar</Button>
						</DialogActions>
						{this.state.clientSignaturePadOpen &&
							<div className={"signature-pad__wrapper signature-pad__wrapper--open"}>
								<div className="signature-pad__close__wrapper">
									<Button iconSolo><Icon decline onClick={() => this.setState({ clientSignaturePadOpen: false })} /></Button>
								</div>
								<SignaturePad height={window.innerHeight} className="signature-pad" redrawOnResize={true} options={{ minWidth: 2, maxWidth: 10 /*, penColor: 'rgb(66, 133, 244)'*/ }}
									ref={ref => this.clientSignaturePad = ref} />
								<div className="signature-pad__actions">
									<Button default onClick={() => this.clientSignaturePad.clear()} color="primary">Apagar</Button>
									<Button primary onClick={() => {
										var image = this.clientSignaturePad.toDataURL();
										if (this.props.$equipments) {
											this.props.$equipments.value.map((s) => {
												s.$assinaturaTecnico.value = image;
											});
										}
										this.setState({ clientSignaturePadPng: image, clientSignaturePadOpen: false });
									}} color="primary">Guardar</Button>
								</div>
							</div>
						}
						{this.state.sieSignaturePadOpen &&
							<div className={"signature-pad__wrapper signature-pad__wrapper--open"}>
								<div className="signature-pad__close__wrapper">
									<Button iconSolo><Icon decline onClick={() => this.setState({ sieSignaturePadOpen: false })} /></Button>
								</div>
								<SignaturePad height={window.innerHeight} className="signature-pad" redrawOnResize={true} options={{ minWidth: 2, maxWidth: 10 /*, penColor: 'rgb(66, 133, 244)'*/ }}
									ref={ref => this.sieSignaturePad = ref} />
								<div className="signature-pad__actions">
									<Button default onClick={() => this.sieSignaturePad.clear()} color="primary">Apagar</Button>
									<Button primary onClick={() => {
										var image = this.sieSignaturePad.toDataURL();
										if (this.props.$equipments) {
											this.props.$equipments.value.map((s) => {
												s.$assinaturaSie.value = image;
											});
										}
										this.setState({ sieSignaturePadPng: image, sieSignaturePadOpen: false });
									}} color="primary">Guardar</Button>
								</div>
							</div>
						}
						{this.state.technicalSignaturePadOpen &&
							<div className={"signature-pad__wrapper signature-pad__wrapper--open"}>
								<div className="signature-pad__close__wrapper">
									<Button iconSolo><Icon decline onClick={() => this.setState({ technicalSignaturePadOpen: false })} /></Button>
								</div>
								<SignaturePad height={window.innerHeight} className="signature-pad" redrawOnResize={true} options={{ minWidth: 2, maxWidth: 10 /*, penColor: 'rgb(66, 133, 244)'*/ }}
									ref={ref => this.technicalSignaturePad = ref} />
								<div className="signature-pad__actions">
									<Button default onClick={() => this.technicalSignaturePad.clear()} color="primary">Apagar</Button>
									<Button primary onClick={() => {
										var image = this.technicalSignaturePad.toDataURL();
										if (this.props.$equipments) {
											this.props.$equipments.value.map((s) => {
												s.$assinaturaTecnico.value = image;
											});
										}
										this.setState({ technicalSignaturePadPng: image, technicalSignaturePadOpen: false });
									}} color="primary">Guardar</Button>
								</div>
							</div>
						}
					</div>
				} />
		)
	}
}

export default Options;