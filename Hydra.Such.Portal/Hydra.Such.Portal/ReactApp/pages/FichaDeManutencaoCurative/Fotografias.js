import React, { Component } from 'react';
import { Text, Wrapper } from 'components';
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
    margin-right: 8px;
    margin-bottom: 4px;
`
const WrapperText = styled(Wrapper)`&&{
    margin-left: 16px;
}
`
const TextDate = styled(Text)`&&{
  text-align: right;
  display: block;
}
`

const Fotografias = (props) => {
  return (
    <div>      

      <Wrapper padding={'0 0 16px'}>
        <Grid container spacing={16}>
        
           <Grid item container xs={12}>
            <Grid item xs={1}>
            <TextDate>25 Jul 2019</TextDate>
              </Grid>
            <Grid item xs={11}>
              <WrapperText>
                <DocImg/><DocImg/><DocImg/>
              </WrapperText>
            </Grid>
          </Grid>

          <Grid item container xs={12}>
          <Grid item xs={1}>
            <TextDate>28 Jul 2019</TextDate>
              </Grid>
            <Grid item xs={11}>
              <WrapperText>
                <DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/>
              </WrapperText>
            </Grid>
          </Grid>

          <Grid item container xs={12}>
          <Grid item xs={1}>
            <TextDate>28 Jul 2019</TextDate>
              </Grid>
            <Grid item xs={11}>
              <WrapperText>
                <DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/>
              </WrapperText>
            </Grid>
          </Grid>
          
          <Grid item container xs={12}>
          <Grid item xs={1}>
            <TextDate>20 Jul 2019</TextDate>
              </Grid>
            <Grid item xs={11}>
              <WrapperText>
                <DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/><DocImg/>
              </WrapperText>
            </Grid>
          </Grid>

          <Grid item container xs={12}>
          <Grid item xs={1}>
            <TextDate>19 Jul 2019</TextDate>
              </Grid>
            <Grid item xs={11}>
              <WrapperText>
                <DocImg/>
              </WrapperText>
            </Grid>
          </Grid>

          <Grid item container xs={12}>
          <Grid item xs={1}>
            <TextDate>16 Jul 2019</TextDate>
              </Grid>
            <Grid item xs={11}>
              <WrapperText>
                <DocImg/><DocImg/>
              </WrapperText>
            </Grid>
          </Grid>


        </Grid>
      </Wrapper>
      

    </div>
  )
} 

export default Fotografias;