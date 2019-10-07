import React, { Component } from 'react';
import Tabs from '@material-ui/core/Tabs';
import MuiTab from '@material-ui/core/Tab';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { Wrapper, Tooltip, Button, Text as EText, Select, MenuItem, Modal, Input, Icon } from 'components';
import MuiGrid from '@material-ui/core/Grid';
import { observable } from 'mobx';
import { observer } from 'mobx-react';
import _theme from '../../themes/default';
const { DialogTitle, DialogContent, DialogActions } = Modal;

const Root = styled.div`
	display: block;
	white-space: nowrap;
`;

const Grid = styled(MuiGrid)`
`;

const Text = styled(EText)`
	background-color:  ${props => props.theme.palette.bg.white};
	/* color: #F9703E; */
	text-transform: none;
	padding: 8px 24px;
	box-shadow: none;
	border-radius: 6px;
	display: inline-block;
	border: 2px solid transparent;
	cursor: pointer;
	&.active {
		border: 2px solid ${props => props.theme.palette.secondary.default};
		color:  ${props => props.theme.palette.secondary.default};
		small {
			color:  ${props => props.theme.palette.secondary.default};
		}
	}
`;

const ActionWrapper = styled(Grid)` && {
		white-space: nowrap;
		display: block;
		white-space: nowrap;
	}
`;

const ActionItem = styled(Grid)`
	padding:  8px 0px;
`;

var FinalInput = styled(Input)`
	.icon-arrow-down {
		display: none;
	}

	border-radius: ${props => props.theme.radius.primary};
		
	fieldset {
		border: none;
	}	
	
	[class*="MuiSelect-selectMenu"] {			
		&[class*="MuiSelect-selectMenu"] {			
			&[class*="MuiSelect-selectMenu"] {			
				padding: 11px 15px 11px 15px;
				text-align: center;
			}
		}
	}
	[class*="icon-"]:not(.icon-arrow-down) {		
		right: auto;
		text-align: center;
		font-size: 18px;
		text-align: center;
		display: inline-block;
		position: relative;
		padding: 0;
		vertical-align: bottom;
	}

	[class*="MuiSelect-root"]  {		
		background: white;
		color: ${props => props.theme.palette.alert.bad};
		box-shadow: 1px 1px 2px 0px rgba(50,63,75,0.3);
		border-radius: ${props => props.theme.radius.primary};
	}
`

injectGlobal`
	[class*="icon-"], .va-m {		
		vertical-align: middle;
		line-height: 1em;
	}

	.fs-red {
		[class*="MuiSelect-root"]  {
			color: ${_theme.palette.bg.white};
			background: ${_theme.palette.alert.bad};
		}
	}

	.fs-green {
		[class*="MuiSelect-root"]  {
			color: ${_theme.palette.bg.white};
			background: ${_theme.palette.alert.good};
		}
	}
`;

const Spacer = styled.div`
	height: 8px;
`;

class FinalState extends Component {
	state = {
		open: false
	}

	constructor(props) {
		super(props);
	}

	render() {
		let props = this.props;
		let state = this.state;

		return (
			<Root>
				<ActionWrapper container className={props.$value.value == 0 ? '' : props.$value.value == 3 ? 'fs-red' : 'fs-green'}>
					<Select
						input={<FinalInput value={props.$value.value} />}
						onChange={(e) => {
							props.$value.value = e.target.value;
							if (e.target.value >= 2) {
								this.setState({ open: true });
							}
						}}
					>
						<MenuItem value={0} style={{ padding: '11px', textAlign: 'center' }}>
							<div style={{ textAlign: 'center', width: '100%' }}>
								<span className={'va-m'}>0 &nbsp;&nbsp;</span><Icon remove className={'s-20'} />
							</div>
						</MenuItem>
						<MenuItem value={1} style={{ padding: '11px' }}>
							<div style={{ textAlign: 'center', width: '100%' }}>
								<span className={'va-m'}>1 &nbsp;&nbsp;</span><Icon approved className={'s-18'} />
							</div>
						</MenuItem>
						<MenuItem value={2} style={{ padding: '11px', textAlign: 'center' }}>
							<div style={{ textAlign: 'center', width: '100%' }}>
								<span className={'va-m'}>2 &nbsp;&nbsp;</span><Icon comments className={'s-18'} />
							</div>
						</MenuItem>
						<MenuItem value={3} style={{ padding: '11px', textAlign: 'center' }}>
							<div style={{ textAlign: 'center', width: '100%' }}>
								<span className={'va-m'}>3 &nbsp;&nbsp;</span><Icon decline className={'s-18'} />
							</div>
						</MenuItem>
					</Select>
					{/* <Button icon={<Icon remove />} style={{ maxWidth: '100%' }}></Button> */}

					<Modal open={this.state.open} onClose={() => this.setState({ open: false })} children={
						<div>
							<DialogTitle><EText h2>Estado Final</EText></DialogTitle>
							<hr />
							<DialogContent>
								<Grid container direction="row" justify="space-between" alignitems="top" spacing={0} maxwidth={'100%'} margin={0} >
									<Grid item md={9} >
										<Grid container direction="row" justify="space-between" alignitems="top" spacing={0} maxwidth={'100%'} margin={0} >
											<Grid item xs={3} >
												<div>
													<EText b> Marca</EText>
												</div>
												<div>
													<EText b> Modelo</EText>
												</div>
												<div>
													<EText b> Nº Série</EText>
												</div>
												<div>
													<EText b> Nº Inv.</EText>
												</div>
											</Grid>
											<Grid item xs={9} >
												<div>
													<EText span> {props.brand}</EText>
												</div>
												<div>
													<EText span> {props.model}</EText>
												</div>
												<div>
													<EText span> {props.serialNumber}</EText>
												</div>
												<div>
													<EText span> {props.inventoryNumber}</EText>
												</div>
											</Grid>
										</Grid>
										<Spacer /><Spacer />
										{/* <Input $value={props.$value} placeholder={'Definir notificação interna'} />
										<Spacer /><Spacer /> */}
										<Input multiline rowsMax="6" rows="6" $value={props.$message} placeholder={'Inserir observação geral'} />
										<Spacer /><Spacer />
										<Button icon={<Icon attach />}>Anexar Fotos</Button>
										<Spacer /><Spacer />
									</Grid>
								</Grid>
							</DialogContent>
							<hr />
							<DialogActions>
								<Button primary color="primary" onClick={() => {
									this.setState({ open: false });
								}}>Guardar</Button>
							</DialogActions>
						</div>
					} />
				</ActionWrapper>
			</Root>
		);
	}
};

export default FinalState;