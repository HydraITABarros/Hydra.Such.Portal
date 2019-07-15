import React, { Component } from 'react';
import { Input, Button, Icon, Text, Wrapper, Select, MenuItem, GSelect } from 'components';
import MuiAddIcon from '@material-ui/icons/Add';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import MuiGrid from '@material-ui/core/Grid';


const Grid = styled(MuiGrid)`
    position: relative;
`
const TextUpload = styled(Text)`
    margin-left: 16px;
`

const Upload = (props) => {
  return (
    <div>      

      <Wrapper padding={'0 0 16px'}>
        <Grid container spacing={16}>
          <Grid item xs={10} md={5}>
          <GSelect           
            placeholder=
              {"Associar a equipamento"}
            options={[
              {value:10, title: "#1 19283012"}, 
              {value:20, title: "#2 19283012"},
              ]}
            />
          </Grid>
          <Grid item xs={10}>
              <Button iconPrimary={<Icon upload/>}>Upload</Button><TextUpload>Até 100MB</TextUpload>
          </Grid>
        </Grid>
      </Wrapper>

    </div>
  )
} 

export default Upload;