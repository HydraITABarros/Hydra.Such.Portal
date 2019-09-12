import React, { Component } from 'react';
import Tabs from '@material-ui/core/Tabs';
import MuiTab from '@material-ui/core/Tab';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { Wrapper, Tooltip, Button, Text as eText } from 'components';
import MuiGrid from '@material-ui/core/Grid';

const Root = styled.div`
	display: block;
	white-space: nowrap;
`;

const Grid = styled(MuiGrid)`
`;

const Text = styled(eText)`
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

const PlanActions = (props) => {
	return (
		<Root>
			<ActionWrapper container>
				{props.planMaintenance &&
					<Text p className={props.position == 0 ? 'active' : ''} onClick={() => props.onSelect(0)}>
						<b>Manutenção</b>
						&nbsp;<small>{props.planMaintenance.length}</small>
					</Text>
				}
				{props.planQuality &&
					<Text p className={props.position == 1 ? 'active' : ''} onClick={() => props.onSelect(1)}>
						<b>Qualitativo</b>
						&nbsp;<small>{props.planQuality.length}</small>
					</Text>
				}
				{props.planQuantity &&
					<Text p className={props.position == 2 ? 'active' : ''} onClick={() => props.onSelect(2)}>
						<b>Quantitativo</b>
						&nbsp;<small>{props.planQuantity.length}</small>
					</Text>
				}
			</ActionWrapper>
		</Root>
	);
}

export default PlanActions;