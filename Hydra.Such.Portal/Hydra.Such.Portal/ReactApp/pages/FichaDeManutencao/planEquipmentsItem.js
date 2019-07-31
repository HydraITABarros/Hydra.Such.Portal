import React, { Component } from 'react';
import Tabs from '@material-ui/core/Tabs';
import MuiTab from '@material-ui/core/Tab';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { Wrapper, Tooltip, Button, Text } from 'components';
import MuiGrid from '@material-ui/core/Grid';

const Root = styled.div`
	white-space: nowrap;
	display: inline-block;
`;

const Container = styled.div`
	width: 108px;
	padding: 0 8px; 
	display: inline-block;
	text-align: center;
`;

const PlanEquipmentsItem = (props) => {

	return (
		<Container key={props.key}>
			{props.children}
		</Container>
	);
}

export default PlanEquipmentsItem;