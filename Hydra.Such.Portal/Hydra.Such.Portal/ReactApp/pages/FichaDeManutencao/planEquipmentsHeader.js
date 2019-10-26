import React, { Component } from 'react';
import Tabs from '@material-ui/core/Tabs';
import MuiTab from '@material-ui/core/Tab';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { Wrapper, Tooltip, Button, Text } from 'components';
import MuiGrid from '@material-ui/core/Grid';
import PlanEquipmentsItem from './planEquipmentsItem';

const Root = styled.div`
	white-space: nowrap;
	display: inline-block;
	padding: 0 144px 0 12px;
`;

const Index = styled(Text)`
	font-size: 16px;

`;

const Num = styled(Text)`
	font-size: 13px;
	line-height: 24px;
`;

const Container = styled.div`
	display: inline-block;
	padding: 35px 0 0;
`

const PlanEquipmentsHeader = (props) => {

	return (
		<Root>
			{props.equipments.map((item, index) => {
				return (
					<Container key={index}>
						<PlanEquipmentsItem>
							<span>
								<Index p>{index + 1}</Index>
								<Num p><small>{item.numEquipamento}</small></Num>
							</span>
						</PlanEquipmentsItem>
					</Container>
				);
			})}
		</Root>
	);
}

export default PlanEquipmentsHeader;