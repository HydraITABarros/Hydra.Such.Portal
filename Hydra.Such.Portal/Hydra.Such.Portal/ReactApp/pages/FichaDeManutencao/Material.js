import React, { Component } from 'react';
import { Input, Button, Icon, Text, Wrapper, Select, MenuItem } from 'components';
import MuiAddIcon from '@material-ui/icons/Add';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import MuiGrid from '@material-ui/core/Grid';

const Grid = styled(MuiGrid)`
    position: relative;
`
const InputModal = styled(Input)`&&{
  }
`;
const ButtonNew = styled(Button)`&&{
}
`;
const TextCol = styled(Text)`&&{
}
`;

const Material = (props) => {
  return (
    <div>      

      <Wrapper padding={'0 0 16px'}>
        <Grid container spacing={16}>
           <Grid item xs={10} md={3}>
           <Select>
              <MenuItem value="">#1 236465</MenuItem>
              <MenuItem value={10}>#2 321354</MenuItem>
              <MenuItem value={20}>#3 354544</MenuItem>
              <MenuItem value={30}>#4 654321</MenuItem>
          </Select>
          </Grid>
          <Grid item xs={10} md={3}>
            <InputModal value={"Troca de cabo elétrico"} />
          </Grid>
          <Grid item xs={10} md={2}>
            <InputModal value={"3 metros"} />
          </Grid>
          <Grid item xs={10} md={3}>
          <Select>
              <MenuItem value="">Cliente</MenuItem>
              <MenuItem value={10}>SUCH</MenuItem>
          </Select>
          </Grid>
          <Grid item xs={1} md={1}>
            <ButtonNew round><Icon remove/></ButtonNew>
          </Grid>
        </Grid>
      </Wrapper>

      <Wrapper padding={'0 0 16px'}>
        <Grid container spacing={16}>
           <Grid item xs={10} md={3}>
           <Select>
              <MenuItem value="">#1 236465</MenuItem>
              <MenuItem value={10}>#2 321354</MenuItem>
              <MenuItem value={20}>#3 354544</MenuItem>
              <MenuItem value={30}>#4 654321</MenuItem>
          </Select>
          </Grid>
          <Grid item xs={10} md={3}>
            <InputModal value={"Troca de cabo elétrico"} />
          </Grid>
          <Grid item xs={10} md={2}>
            <InputModal value={"3 metros"} />
          </Grid>
          <Grid item xs={10} md={3}>
          <Select>
              <MenuItem value="">Cliente</MenuItem>
              <MenuItem value={10}>SUCH</MenuItem>
          </Select>
          </Grid>
          <Grid item xs={1} md={1}>
            <ButtonNew round><Icon remove/></ButtonNew>
          </Grid>
        </Grid>
      </Wrapper>

      <Wrapper padding={'0 0 16px'}>
        <Grid container spacing={16}>
           <Grid item xs={10} md={3}>
           <Select>
              <MenuItem value="">#1 236465</MenuItem>
              <MenuItem value={10}>#2 321354</MenuItem>
              <MenuItem value={20}>#3 354544</MenuItem>
              <MenuItem value={30}>#4 654321</MenuItem>
          </Select>
          </Grid>
          <Grid item xs={10} md={3}>
            <InputModal placeholder={"Tipo de material"} />
          </Grid>
          <Grid item xs={10} md={2}>
            <InputModal placeholder={"Quantidade"} />
          </Grid>
          <Grid item xs={10} md={3}>
            <Select>
                <MenuItem value="">Cliente</MenuItem>
                <MenuItem value={10}>SUCH</MenuItem>
            </Select>
          </Grid>
          <Grid item xs={1} md={1}>
            <ButtonNew round><Icon add/></ButtonNew>
          </Grid>
        </Grid>
      </Wrapper>

    </div>
  )
} 

export default Material;