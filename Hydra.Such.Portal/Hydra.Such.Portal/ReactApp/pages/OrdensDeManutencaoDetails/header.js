import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import { Text, Icon, Circle, Wrapper, Spacer, Button } from 'components';
import Functions from '../../helpers/functions';
import MuiGrid from '@material-ui/core/Grid';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { createMuiTheme } from '@material-ui/core/styles';
import Hidden from '@material-ui/core/Hidden';
import { withRouter } from 'react-router-dom';
injectGlobal`
	.transparent {
		opacity: 0
	}
`
const muiTheme = createMuiTheme();
const breakpoints = muiTheme.breakpoints.values;

const Grid = styled(MuiGrid)`
    position: relative;
`

const TextHeader = styled(Text)`
    display: inline-block;
`
const CircleOm = {
	wrapper: styled.div`
        margin: auto;
        white-space: nowrap;
        ${props => props.primary && css`
        
        `}
    `,
	icon: styled.div`
        font-size: 33px;
        line-height: 35px;
        display: inline-block;
        vertical-align: middle;
        padding: 17px;
        > [class*="icon"] {
            text-align: center;
            width: 57px;
            height: 57px;
            display: inline-block;
            background:  ${props => props.background};
            color:  ${props => props.color};
            border-radius: 50%;
            line-height: 56px;
            font-size: 70px;
            position: relative;
            &:before {
                position: absolute;
                left: -7px;
                right: -7px;
                top: -7px;
                bottom: -7px;
                margin: auto;
                text-align: center;
                line-height: 70px;
                font-size: ${props => props.fontSize ? props.fontSize : '33px'};
            }
        }
    `,
	chart: styled.div`
        padding: 0px 6px 0 6px;
        display: inline-block;
        vertical-align: middle;
        text-align: center;
    `,
};

const CircleOmWrapper = styled(CircleOm.wrapper)`
    @media (max-width: ${breakpoints["sm"] + "px"}) {
        transform: scale(0.8) translateX(-50%);
        transform-origin: 0;
        left: 50%;
        position: relative;
    }
    @media (max-width:  455px ) {
        transform: scale(0.7) translateX(-50%);
        margin-top: -20px;
        margin-bottom: -20px;
    }
    @media (max-width:  400px ) {
        transform: scale(0.6) translateX(-50%);
        margin-top: -30px;
        margin-bottom: -30px;
    }
    @media (max-width:  350px ) {
        transform: scale(0.5) translateX(-50%);
        margin-top: -40px;
        margin-bottom: -40px;
    }
`;

class Header extends Component {
	state = {
		isLoading: true,
		toSigning: 0,
		toExecute: 0,
		executed: 0
	}

	constructor(props) {
		super(props);
	}

	componentWillReceiveProps(nextProps) {
		if (nextProps.isLoading !== this.state.isLoading) {

			this.setState({ isLoading: nextProps.isLoading });
		}

		if (nextProps.executed !== this.state.executed) {
			this.setState({ executed: nextProps.executed });
		}

		if (nextProps.toExecute !== this.state.toExecute) {
			this.setState({ toExecute: nextProps.toExecute });
		}

		if (nextProps.toSigning !== this.state.toSigning) {
			this.setState({ toSigning: nextProps.toSigning });
		}
	}

	shouldComponentUpdate(nextProps, nextState) {
		return true;
	}

	render() {
		return (
			<div>
				<Wrapper padding={' 20px 25px'} width="100%" className={this.state.isLoading ? 'transparent' : ''}>
					<span>
						<Text b onClick={() => { this.props.history.push(`/ordens-de-manutencao`) }}
							style={{ verticalAlign: 'middle', textDecoration: 'underline', cursor: 'pointer' }}>OMs</Text>
						<Icon arrow-right style={{ verticalAlign: 'middle' }} />
						&nbsp;<Text b style={{ verticalAlign: 'middle', position: 'relative', top: '1px' }} >
							{this.props.orderId}
						</Text>
					</span>
				</Wrapper>

				<Grid container direction="row" justify="space-between" alignitems="top" spacing={0} maxwidth={'100%'} margin={0} padding={"200px"}>
					<Grid item md={3} xs={12} >
						{!this.state.isLoading &&
							<Wrapper padding={'8px 25px 0'}>
								<TextHeader h2 style={{
									whiteSpace: 'nowrap',
									textOverflow: 'ellipsis',
									overflow: 'hidden',
									width: '100%'
								}}
									data-tip={this.props.maintenanceOrder.description}
								><b> {this.props.maintenanceOrder.description}</b></TextHeader> <br /><br />
								{this.props.maintenanceOrder.clientName && <Text p style={{
									whiteSpace: 'nowrap',
									textOverflow: 'ellipsis',
									overflow: 'hidden',
									width: '100%'
								}}
									data-tip={this.props.maintenanceOrder.clientName}
								> {this.props.maintenanceOrder.clientName}</Text>}
								{this.props.maintenanceOrder.institutionDescription && <Text p style={{
									whiteSpace: 'nowrap',
									textOverflow: 'ellipsis',
									overflow: 'hidden',
									width: '100%'
								}}
									data-tip={this.props.maintenanceOrder.institutionDescription}
								> {this.props.maintenanceOrder.institutionDescription}</Text>}
								{this.props.maintenanceOrder.contractNo && <Text p style={{
									whiteSpace: 'nowrap',
									textOverflow: 'ellipsis',
									overflow: 'hidden',
									width: '100%'
								}}
									data-tip={this.props.maintenanceOrder.contractNo}
								><i>Contrato {this.props.maintenanceOrder.contractNo}</i></Text>}
							</Wrapper>
						}
					</Grid>
					<Grid container item md={6} xs={12}>
						<Wrapper padding={'1px'} textAlign="center" width="100%">
						</Wrapper>
						{this.state.isLoading ? true : this.state.executed + this.state.toExecute > 0 &&
							<CircleOmWrapper className={this.state.isLoading ? 'blink' : ''}>
								<CircleOm.icon background={this.state.isLoading ? this.props.theme.palette.primary.keylines : this.props.theme.palette.primary.dark} color={this.props.theme.palette.primary.keylines} fontSize="71px" >
									<Spacer height="15px" />
									<Icon sad />
									<Wrapper textAlign="center">
										{!this.state.isLoading && <Text dataSmall>{this.state.toExecute}</Text>}
									</Wrapper>
								</CircleOm.icon>
								<CircleOm.chart>
									<Circle
										loading={this.state.isLoading}
										label="Equipamentos"
										trailValue={this.state.isLoading ? 0 : (this.state.toExecute * 1) + (this.state.executed * 1)}
										strokeValue={this.state.isLoading ? 0 : (this.state.executed * 1)}
										strokeIcon={<Icon happy />}
										trailIcon={<Icon sad />}
										width={191}
									/>
								</CircleOm.chart>
								<CircleOm.icon
									background={this.state.isLoading ? this.props.theme.palette.primary.keylines : 'white'}
									color={this.state.isLoading ? this.props.theme.palette.primary.keylines : this.props.theme.palette.secondary.default} fontSize="71px" >
									<Spacer height="20px" />
									<Icon happy />
									<Wrapper textAlign="center">
										{!this.state.isLoading && <Text dataSmall color={this.props.theme.palette.secondary.default}>{this.state.executed}</Text>}
									</Wrapper>
								</CircleOm.icon>
							</CircleOmWrapper>
						}
					</Grid>
					<Grid item md={3} xs={12}>
						<Hidden mdDown>
							{!this.state.isLoading &&
								<Wrapper textAlign="center" inline>
									<Spacer height="25px" />
									<Text dataBig> {this.state.toSigning} </Text>
									<Text p> Relatórios por <Button style={{ padding: 0 }} link href={"javascript:void(0)"}>assinar</Button> </Text>
								</Wrapper>
							}
						</Hidden>
					</Grid>
				</Grid>
			</div>
		)
	}
};

export default withRouter(withTheme(Header));
