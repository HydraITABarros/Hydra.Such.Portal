import React, { Component } from 'react';
import Tabs from '@material-ui/core/Tabs';
import MuiTab from '@material-ui/core/Tab';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { Wrapper, Tooltip, Button, Text as eText } from 'components';
import MuiGrid from '@material-ui/core/Grid';

const Root = styled.div`
`;

const PlanHeader = () => {

	return (
		<Root>
			<Grid container direction="row" justify="space-between" alignitems="top" spacing={0} maxwidth={'100%'} margin={0} >
				<Grid item xs={12} sm={6}>

				</Grid>
			</Grid>
		</Root>
	);
}

export default PlanHeader;