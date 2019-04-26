import React, { Component } from 'react';
import Viz from './Viz.js'

export default class Controller extends Component {
    state = {
        color: "",
        width: "",
        toDraw: [],
    }

    onSubmit = (evt) => {
        evt.preventDefault();
        const newShape = {
            color: this.state.color,
            width: this.state.width,
        }
        this.setState({ toDraw: [...this.state.toDraw, newShape] })
    }

    onChange = (evt) => {
        this.setState({ [evt.target.name]: evt.target.value })
    }

    render() {
        return (
            <Viz shapes={this.state.toDraw} />
        )
    }
}