// https://github.com/diegohaz/arc/wiki/Atomic-Design
/*react-styleguide: ignore*/
import React, { Component } from 'react';
import axios from 'axios';
import { PageTemplate } from 'components';
import CircularProgress from '@material-ui/core/CircularProgress';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import _theme from '../../themes/default';
import MuiTabs from '@material-ui/core/Tabs';
import MuiTab from '@material-ui/core/Tab';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import MuiGrid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import { Button, Text, Icon, Circle, Wrapper, OmDatePicker, CheckBox, Input, Avatars, ModalLarge, Tooltip } from 'components';
import MuiDeleteIcon from '@material-ui/icons/Delete';
import moment from 'moment';
import ReactDOM from 'react-dom';
import Hidden from '@material-ui/core/Hidden';
import { createMuiTheme } from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import MuiTableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import MuiAddIcon from '@material-ui/icons/Add';
import AutoSizer from "react-virtualized-auto-sizer";
import { FixedSizeList as ListWindow } from "react-window";
import Color from 'color';
import Highlighter from "react-highlight-words";
import MuiTextField from '@material-ui/core/TextField';
import MuiInput from '@material-ui/core/Input';
import InputAdornment from '@material-ui/core/InputAdornment';
import { renderToString } from 'react-dom/server';
import { withRouter } from 'react-router';
import { connect } from 'react-redux';
import AppBar from '@material-ui/core/AppBar';
import EMM from '../../pages/FichaDeManutencao/EMM';
import Material from '../../pages/FichaDeManutencao/Material';
import Upload from '../../pages/FichaDeManutencao/Upload';
import Documentos from '../../pages/FichaDeManutencao/Documentos';


const {DialogTitle,DialogContent,DialogActions} = ModalLarge;

const Tabs = styled(MuiTabs)`
    [class*="MuiTabs-scroller"]>span{
      background-color: ${_theme.palette.secondary.default};
      height: 5px;
      border-radius: 2.5px;
      z-index: 2;
    }
    [class*="MuiTabs-fixed"]>span{
      margin-left: 0px;
    }
    [class*="icon"] {
            color: ${props=>props.theme.palette.primary.medium};
          }
    [aria-selected="true"]  {
          [class*="icon"] {
            color: ${props=>props.theme.palette.secondary.default};
          }
    }
`;
const Tab = styled(MuiTab)`&&{
      text-transform: capitalize;
      text-align: left;
      min-width: 0;
    }
    [class*="MuiTab-labelContainer"] {
          padding: 6px 12px;
    }
`;
const Bar = styled(AppBar)`&&{
      background-color: ${_theme.palette.white};
      box-shadow: none;
      margin-bottom: 0px;
      padding-left: 0;
      padding-right: 0;
      hr{position: relative; margin-top: -3px; margin-left: -40px; z-index: 1;}
    }
`;

class Arquivo extends Component {
    state = {
      open: false,
      tab: 0,
    }
      constructor(props) {
        super(props);
        this.handleChange=this.handleChange.bind(this);
      }
      handleChange(e,value) {
        console.log(value)
        this.setState({
          tab: value,
        });
      }

      render() {
        return (
          <div>
              <ModalLarge action={<Button primary >Open Modal</Button>} children={
                <div>
                  <DialogTitle>
                  <Bar position="static" color="default">
                    <Tabs
                      value={this.state.tab}
                      onChange={this.handleChange}
                      indicatorColor="primary"
                      textColor="primary"
                      variant="standard"
                      scrollButtons="off"
                    >
                      <Tab label={<Text b><Icon meter/>EMMs</Text>}/>
                      <Tab label={<Text b><Icon material/>Mat. Aplicado</Text>} />
                      <Tab label={<Text b><Icon fotografias/>Fotografias</Text>} />
                      <Tab label={<Text b><Icon folder/>Documentos</Text>} />
                      <Tab label={<Text b><Icon upload/>Upload</Text>} />
                    </Tabs>
                    <hr/>  
                  </Bar>
                  </DialogTitle>
                  
                  <DialogContent>
                      {this.state.tab === 0 && <EMM/>}
                      {this.state.tab === 1 && <Material/>}
                      {this.state.tab === 2 && <div>Item Three</div>}
                      {this.state.tab === 3 && <Documentos/>}
                      {this.state.tab === 4 && <Upload/>}
                  </DialogContent>
                  <hr/>  
                  <DialogActions>
                      <Button primary color="primary">Save changes</Button>
                  </DialogActions>
              </div>
              }/>
          </div>
          )
      }
}

export default Arquivo;