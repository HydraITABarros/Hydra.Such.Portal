import React, { Component } from 'react';
import { Input, Button, Icon, Text, Wrapper, Select, MenuItem } from 'components';
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
          <Select>
              <MenuItem value="">#1 236465</MenuItem>
              <MenuItem value={10}>#2 321354</MenuItem>
              <MenuItem value={20}>#3 354544</MenuItem>
              <MenuItem value={30}>#4 654321</MenuItem>
          </Select>
          </Grid>
          <Grid item xs={10}>
              <Button iconPrimary={<Icon download/>}>Upload</Button><TextUpload>Até 100MB</TextUpload>
          </Grid>
        </Grid>
      </Wrapper>

    </div>
  )
} 

export default Upload;