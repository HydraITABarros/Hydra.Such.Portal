import React, { Component } from 'react';
import logo from '../../logo.svg';
import './index.css';
import Button from '@material-ui/core/Button';
import { Grid, Table, TableHeaderRow } from '@devexpress/dx-react-grid-material-ui';
import { Divider } from '@material-ui/core';

class App extends Component {
  render() {
    return (
      <div>
        <h1>Teste</h1>
        <Grid
          rows={[
            { id: 0, product: 'DevExtreme', owner: 'DevExpress' },
            { id: 1, product: 'DevExtreme Reactive', owner: 'DevExpress' },
          ]}
          columns={[
            { name: 'id', title: 'ID' },
            { name: 'product', title: 'Product' },
            { name: 'owner', title: 'Owner' },
          ]}>
          <Table />
          <TableHeaderRow />
        </Grid>
      </div>
    );
  }

}

export default App;
