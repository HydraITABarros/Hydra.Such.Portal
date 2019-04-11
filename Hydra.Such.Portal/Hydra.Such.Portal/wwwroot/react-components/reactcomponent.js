import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { combineReducers, createStore } from 'redux';
import { Provider } from 'react-redux';

import { Actions, jsonformsReducer } from '@jsonforms/core';
import { person } from '@jsonforms/examples';
import { materialFields, materialRenderers } from '@jsonforms/material-renderers';

import { JsonForms } from '@jsonforms/react';

import Form from "react-jsonschema-form";

import 'es6-promise';
import 'isomorphic-fetch';

const store = createStore(
  combineReducers({ jsonforms: jsonformsReducer() }),  
  {
    jsonforms: {
      renderers: materialRenderers,
      fields: materialFields,
    }
  }
  );

export default class Counter extends React.Component {
    
    constructor() {
        super();
        this.state = { 
            schema: {},
            loading: true ,
            data: {}
        };

        fetch('ordens-de-manutencao/getSchema')
        .then(response => response.json())
        .then(data => {
            this.setState({ schema: data, loading: false });
            store.dispatch(Actions.init({}, data, {}));
        });
    }

    render() {
        return(
            <div>
                <Form schema={this.state.schema}
                    onChange={console.log("changed")}
                    onSubmit={console.log("submitted")}
                    onError={console.log("errors")} 
                    uiSchema={{}}/>
                    <br /> <br />  
                   

                   {/*
                    <h1>Counter</h1>
                    <p>This is a simple example of a React...</p>
                    <p>Current count: <strong>{this.state.currentCount}</strong></p>
                    <button onClick={() => { this.incrementCounter() }}>Increment</button>
                    */}
                </div>
        );
    }

    incrementCounter() {
        this.setState({
            currentCount: this.state.currentCount + 1
        });
    }
}