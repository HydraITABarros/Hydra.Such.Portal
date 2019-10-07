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
import "./index.scss";


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

class Options extends Component {
	state = {
		open: false,
		tab: 0,
	}
	constructor(props) {
		super(props);
		this.handleChange = this.handleChange.bind(this);
	}
	handleChange(e, value) {
		this.setState({
			tab: value,
		});
	}

	render() {
		return (
			<ModalLarge
				className="modal-signature"
				onClose={() => this.setState({ open: false })}
				onOpen={() => this.setState({ open: true })}
				open={this.state.open}
				action={this.props.children} children={
					<div>
						<DialogTitle>
							<Text h2><Icon report /> Assinar</Text>
						</DialogTitle>
						<hr />
						<Grid container direction="row" justify="space-between" alignitems="top" spacing={0} maxwidth={'100%'} margin={0} >
							{/* <DialogContent></DialogContent> */}
							<Grid item xs={12} md={4} lg={3} style={{ borderRight: '1px solid #E4E7EB', borderBottom: '1px solid #E4E7EB' }}>
								<DialogContent>
									<Text b>{this.props.equipmentType} ({this.props.$equipments && this.props.$equipments.value.length})</Text>
									<br /><br />
									{this.props.$equipments && this.props.$equipments.value.map((e, i) => {
										return <div key={i} style={{ lineHeight: '32px' }}><Text b style={{ width: '45px', display: 'inline-block' }}>#{i + 1}</Text> &nbsp;&nbsp;<Text span>{e.numEquipamento}</Text></div>
									})}
								</DialogContent>
							</Grid>
							<Grid item xs={12} md={4} lg={4} style={{ borderRight: '1px solid #E4E7EB', borderBottom: '1px solid #E4E7EB' }}>
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
										<Input placeholder={"Assinatura"} />
										<Icon signature
											style={{
												position: 'absolute',
												top: 0,
												right: 0,
												bottom: 0,
												margin: 'auto',
												lineHeight: '40px',
												width: '40px',
												pointerEvents: 'none'
											}}
										/>
									</div>
									<br /><br /><br />
									<div>
										<Text b>SIE / Aprovisionamento</Text>
									</div>
									<Spacer height={"10px"} />
									<div style={{ position: 'relative' }}>
										<Input placeholder={"Assinatura"} />
										<Icon signature
											style={{
												position: 'absolute',
												top: 0,
												right: 0,
												bottom: 0,
												margin: 'auto',
												lineHeight: '40px',
												width: '40px',
												pointerEvents: 'none'
											}}
										/>
									</div>
									<Spacer height={"10px"} />
									<CheckBox /> <Text span>Assinatura é igual à do cliente</Text>
								</DialogContent>
							</Grid>
							<Grid item xs={12} md={4} lg={4} >
								<DialogContent>
									<Text h2>2<sup>a</sup> Fase</Text>
									<br />
									<div>
										<Text b>Cliente</Text>
									</div>
									<Spacer height={"10px"} />
									<div style={{ position: 'relative' }}>
										<Input placeholder={"Assinatura"} />
										<Icon signature
											style={{
												position: 'absolute',
												top: 0,
												right: 0,
												bottom: 0,
												margin: 'auto',
												lineHeight: '40px',
												width: '40px',
												pointerEvents: 'none'
											}}
										/>
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
							<Button primary onClick={() => this.setState({ open: false })} color="primary">Finalizar</Button>
						</DialogActions>
					</div>
				} />
		)
	}
}

export default Options;