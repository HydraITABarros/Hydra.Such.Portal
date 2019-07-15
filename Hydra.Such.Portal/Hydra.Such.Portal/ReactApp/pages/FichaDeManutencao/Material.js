import React, { Component } from 'react';
import { Input, Button, Icon, Text, Wrapper, Select, MenuItem, GSelect } from 'components';
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
          <GSelect           
            placeholder=
              {"#"}
            options={[
              {value:10, title: "#1	3215"}, 
              {value:20, title: "#2	3041"},
              ]}
            />
          </Grid>
          <Grid item xs={10} md={3}>
            <InputModal value={"Troca de cabo elétrico"} />
          </Grid>
          <Grid item xs={10} md={2}>
            <InputModal value={"3 metros"} />
          </Grid>
          <Grid item xs={10} md={3}>
          <GSelect           
            placeholder=
              {"Fornecido por"}
            options={[
              {value:10, title: "Cliente"}, 
              {value:20, title: "Such"},
              ]}
            />
          </Grid>
          <Grid item xs={1} md={1}>
            <ButtonNew round><Icon remove/></ButtonNew>
          </Grid>
        </Grid>
      </Wrapper>

      <Wrapper padding={'0 0 16px'}>
        <Grid container spacing={16}>
          <Grid item xs={10} md={3}>
          <GSelect           
            placeholder=
              {"#"}
            options={[
              {value:10, title: "#1	3215"}, 
              {value:20, title: "#2	3041"},
              ]}
            />
          </Grid>
          <Grid item xs={10} md={3}>
            <InputModal value={"Troca de cabo elétrico"} />
          </Grid>
          <Grid item xs={10} md={2}>
            <InputModal value={"3 metros"} />
          </Grid>
          <Grid item xs={10} md={3}>
          <GSelect           
            placeholder=
              {"Fornecido por"}
            options={[
              {value:10, title: "Cliente"}, 
              {value:20, title: "Such"},
              ]}
            />
          </Grid>
          <Grid item xs={1} md={1}>
            <ButtonNew round><Icon remove/></ButtonNew>
          </Grid>
        </Grid>
      </Wrapper>

      <Wrapper padding={'0 0 16px'}>
        <Grid container spacing={16}>
          <Grid item xs={10} md={3}>
          <GSelect           
            placeholder=
              {"#"}
            options={[
              {value:10, title: "#1	3215"}, 
              {value:20, title: "#2	3041"},
              ]}
            />
          </Grid>
          <Grid item xs={10} md={3}>
            <InputModal placeholder={"Inserir material"} />
          </Grid>
          <Grid item xs={10} md={2}>
            <InputModal placeholder={"Quantidade"} />
          </Grid>
          <Grid item xs={10} md={3}>
          <GSelect           
            placeholder=
              {"Fornecido por"}
            options={[
              {value:10, title: "Cliente"}, 
              {value:20, title: "Such"},
              ]}
            />
          </Grid>
          <Grid item xs={1} md={1}>
            <ButtonNew round><Icon remove/></ButtonNew>
          </Grid>
        </Grid>
      </Wrapper>

    </div>
  )
} 

export default Material;