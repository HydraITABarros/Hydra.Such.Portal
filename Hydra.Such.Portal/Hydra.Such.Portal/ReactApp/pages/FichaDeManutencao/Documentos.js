import React, { Component } from 'react';
import { Input, Button, Icon, Text, Wrapper, Select, MenuItem } from 'components';
import MuiAddIcon from '@material-ui/icons/Add';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import MuiGrid from '@material-ui/core/Grid';
import _theme from '../../themes/default';


const Grid = styled(MuiGrid)`
    position: relative;
`
const DocImg = styled.div` 
    display: inline-block;
    width: 64px;
    height: 64px;
    border: 1px solid ${_theme.palette.primary.medium};
    margin-right: 16px;
`
const ButtonNew = styled(Button)`&&{
}
`;
const WrapperText = styled(Wrapper)`&&{
    margin-left: 16px;
}
`;
const TextDoc = styled(Text)`&&{
  text-transform:none; 
  margin-top: 4px;
  font-weight: 400;
  line-height: 18px;
}
`;

const Documentos = (props) => {
	return (
		<div>

			<Wrapper padding={'0 0 16px'}>
				<Grid container spacing={16}>

					<Grid item container xs={12} md={4}>
						<Grid item xs={2}>
							<DocImg />
						</Grid>
						<Grid item xs={4}>
							<WrapperText>
								<Text b>Ficha Técnica</Text><br />
								<TextDoc label>.pdf (243kb)<br />11 Abr 2018</TextDoc>
							</WrapperText>
						</Grid>
					</Grid>

					<Grid item container xs={12} md={4}>
						<Grid item xs={2}>
							<DocImg />
						</Grid>
						<Grid item xs={4}>
							<WrapperText>
								<Text b>Ficha Técnica</Text><br />
								<TextDoc label>.pdf (243kb)<br />11 Abr 2018</TextDoc>
							</WrapperText>
						</Grid>
					</Grid>

					<Grid item container xs={12} md={4}>
						<Grid item xs={2}>
							<DocImg />
						</Grid>
						<Grid item xs={4}>
							<WrapperText>
								<Text b>Ficha Técnica</Text><br />
								<TextDoc label>.pdf (243kb)<br />11 Abr 2018</TextDoc>
							</WrapperText>
						</Grid>
					</Grid>

				</Grid>
			</Wrapper>

		</div>
	)
}

export default Documentos;