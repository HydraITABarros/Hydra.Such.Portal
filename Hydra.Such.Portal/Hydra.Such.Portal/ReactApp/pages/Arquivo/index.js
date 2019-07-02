// https://github.com/diegohaz/arc/wiki/Atomic-Design
/*react-styleguide: ignore*/
import React, { Component } from 'react';
import axios from 'axios';
import { PageTemplate } from 'components';
import CircularProgress from '@material-ui/core/CircularProgress';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import _theme from '../../themes/default';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import MuiGrid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import { Button, Text, Icon, Circle, Wrapper, OmDatePicker, CheckBox, Input, Avatars, Modal, Tooltip } from 'components';
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


const {DialogTitle,DialogContent,DialogActions} = Modal;


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
              <Modal action={<Button primary >Open Modal</Button>} children={
                <div>
                  <DialogTitle>
                  <AppBar position="static" color="default">
                    <Tabs
                      value={this.state.tab}
                      onChange={this.handleChange}
                      indicatorColor="primary"
                      textColor="primary"
                      variant="scrollable"
                      scrollButtons="auto"
                    >
                      <Tab label="Item One" />
                      <Tab label="Item Two" />
                      <Tab label="Item Three" />
                      <Tab label="Item Four" />
                      <Tab label="Item Five" />
                      <Tab label="Item Six" />
                      <Tab label="Item Seven" />
                    </Tabs>
                  </AppBar>
                  </DialogTitle>
                  <hr/>  
                  <DialogContent>


                  {this.state.tab === 0 && <div>Item One</div>}
                  {this.state.tab === 1 && <div>Item Two</div>}
                  {this.state.tab === 2 && <div>Item Three</div>}
                  {this.state.tab === 3 && <div>Item Four</div>}
                  {this.state.tab === 4 && <div>Item Five</div>}
                  {this.state.tab === 5 && <div>Item Six</div>}
                  {this.state.tab === 6 && <div>Item Seven</div>}

                      <Text p>
                          Cras mattis consectetur purus sit amet fermentum. Cras justo odio, dapibus ac
                          facilisis in, egestas eget quam. Morbi leo risus, porta ac consectetur ac, vestibulum
                          at eros.
                      </Text>
                      <Text p>
                          Praesent commodo cursus magna, vel scelerisque nisl consectetur et. Vivamus sagittis
                          lacus vel augue laoreet rutrum faucibus dolor auctor.
                      </Text>
                      <Text p>
                          Aenean lacinia bibendum nulla sed consectetur. Praesent commodo cursus magna, vel
                          scelerisque nisl consectetur et. Donec sed odio dui. Donec ullamcorper nulla non metus
                          auctor fringilla.
                          Cras mattis consectetur purus sit amet fermentum. Cras justo odio, dapibus ac
                          facilisis in, egestas eget quam. Morbi leo risus, porta ac consectetur ac, vestibulum
                          at eros.
                      </Text>
                      <Text p>
                          Praesent commodo cursus magna, vel scelerisque nisl consectetur et. Vivamus sagittis
                          lacus vel augue laoreet rutrum faucibus dolor auctor.
                      </Text>
                      <Text p>
                          Aenean lacinia bibendum nulla sed consectetur. Praesent commodo cursus magna, vel
                          scelerisque nisl consectetur et. Donec sed odio dui. Donec ullamcorper nulla non metus
                          auctor fringilla.
                      </Text>
                  </DialogContent>
                  <hr/>  
                  <DialogActions>
                      <Button primary color="primary">Save changes</Button>
                  </DialogActions> 
              </div>
              } />
          </div>
          )
      }
}

export default Arquivo;