import React from 'react'
import styled, { css, theme, withTheme } from 'styled-components'
import MuiRadio from '@material-ui/core/Radio';
import Text from '../Text';

const styles = css`&& {
  padding-right: 8px;
  &[class*="MuiRadio-root"]{
      color: ${(props) => props.theme.palette.primary.medium};
      }
   }
`
const RadioText = styled(Text)`
  vertical-align: middle;
`

const CustomRadio = styled(MuiRadio)`${styles}`;

const Radio = (props) => {
  return (
    <span>
      <CustomRadio {...props} />
      {props.checked?<RadioText b>{props.label}</RadioText>:<RadioText>{props.label}</RadioText>}
    </span>
  )
}

export default withTheme(Radio);
