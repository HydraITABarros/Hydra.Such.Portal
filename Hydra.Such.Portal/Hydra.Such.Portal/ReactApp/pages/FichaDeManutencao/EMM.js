import React, { Component } from 'react';
import { Input, Button, Icon, Text, Wrapper } from 'components';
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
    line-height: 36px;
}
`;


const EMM = (props) => {
  return (
    <div>      

      <Wrapper padding={'0 0 16px'}>
        <Grid container spacing={16}>
          <Grid item xs={10} md={5}>
            <InputModal value={"Voltímetro: 12830912938–19219-29"} />
          </Grid>
          <Grid item xs={2} md={1}>
            <ButtonNew round><Icon remove/></ButtonNew>
          </Grid>
          <Grid item xs={12} md={2}>
            <TextCol>Marca: Stihl</TextCol>
          </Grid>
          <Grid item xs={12} md={3}>
              <TextCol>Certificado: ISQ – 10 Out 2022</TextCol>
          </Grid>
        </Grid>
      </Wrapper>

      <Wrapper padding={'0 0 16px'}>
        <Grid container spacing={16}>
          <Grid item xs={10} md={5}>
            <InputModal value={"Multímetro: 19192–1212v"} />
          </Grid>
          <Grid item xs={2} md={1}>
            <ButtonNew round><Icon remove/></ButtonNew>
          </Grid>
          <Grid item xs={12} md={2}>
            <TextCol>Marca: Stihl</TextCol>
          </Grid>
          <Grid item xs={12} md={3}>
              <TextCol>Certificado: ISQ – 10 Out 2022</TextCol>
          </Grid>
        </Grid>
      </Wrapper>

      <Wrapper padding={'0 0 16px'}>
        <Grid container={"true"} spacing={16}>
          <Grid item xs={10} md={5}>
            <InputModal placeholder={"Inserir EMM"}/>
          </Grid>
          <Grid item xs={2} md={1}>
            <ButtonNew round><Icon add/></ButtonNew>
          </Grid>
          <Grid item xs={12} md={2}>
            <TextCol></TextCol>
          </Grid>
          <Grid item xs={12} md={3}>
            <TextCol></TextCol>
          </Grid>
        </Grid>
      </Wrapper>

    </div>
  )
} 

export default EMM;