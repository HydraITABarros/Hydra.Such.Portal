import React, { Component } from 'react';
import { Text, Icon, Circle, Wrapper, Spacer } from 'components';
import Functions from './functions';
import MuiGrid from '@material-ui/core/Grid';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { createMuiTheme } from '@material-ui/core/styles';
import Hidden from '@material-ui/core/Hidden';

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
        padding: 6px;
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

export default withTheme(function (props) {
	return (
		<div>
			<Wrapper padding={' 20px 25px'} width="100%" >
				{!props.isLoading && (<span>
					<Text b onClick={() => { this.props.history.push(`/ordens-de-manutencao`) }} style={{ verticalAlign: 'middle', textDecoration: 'underline', cursor: 'pointer' }}>OMs</Text>
					<Icon arrow-right style={{ verticalAlign: 'middle' }} />
					&nbsp;<Text b style={{ verticalAlign: 'middle', position: 'relative', top: '1px' }} >{Functions.toTitleCase(props.maintenanceOrder.description)}</Text>
				</span>)}
			</Wrapper>

			<Grid container direction="row" justify="space-between" alignitems="top" spacing={0} maxwidth={'100%'} margin={0} padding={"200px"}>
				<Grid item xs>
					{!props.isLoading &&
						<Wrapper padding={'0 25px 0'}>
							<TextHeader h2><b> {Functions.toTitleCase(props.maintenanceOrder.nomeInstituicao)}</b></TextHeader> <br />
							{props.orderId && <div><Text b>OM</Text> {props.orderId} </div>}
							{props.maintenanceOrder.contrato && <div><Text b>Contrato</Text> {props.maintenanceOrder.contrato} </div>}
						</Wrapper>
					}
				</Grid>
				<Grid container item md={6} xs={12}>
					<CircleOmWrapper className={''}>
						<CircleOm.icon background={props.theme.palette.primary.dark} color={props.theme.palette.primary.keylines} fontSize="71px" >
							<Spacer height="15px" />
							<Icon sad />
							<Wrapper textAlign="center">
								<Text dataSmall>300</Text>
							</Wrapper>
						</CircleOm.icon>
						<CircleOm.chart>
							<Circle loading={false} label="Equipamentos" strokeValue={241} trailValue={300} strokeIcon={<Icon happy />} trailIcon={<Icon sad />} width={191} />
						</CircleOm.chart>
						<CircleOm.icon background={'white'} color={props.theme.palette.secondary.default} fontSize="71px" >
							<Spacer height="20px" />
							<Icon happy />
							<Wrapper textAlign="center">
								<Text dataSmall color={props.theme.palette.secondary.default}>241</Text>
							</Wrapper>
						</CircleOm.icon>
					</CircleOmWrapper>
				</Grid>
				<Grid item xs>
					<Hidden mdDown>
						<Wrapper textAlign="center" inline>
							<Spacer height="25px" />
							<Text dataBig> 20 </Text>
							<Text p> Relatórios por assinar </Text>
						</Wrapper>
					</Hidden>
				</Grid>
			</Grid>
		</div>
	);
});
