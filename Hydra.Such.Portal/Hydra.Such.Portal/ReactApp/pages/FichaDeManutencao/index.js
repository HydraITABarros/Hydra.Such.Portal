import React, { Component } from 'react';
import axios from 'axios';
import { PageTemplate } from 'components';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { Wrapper, OmDatePicker, Tooltip } from 'components';
import moment from 'moment';
import ReactDOM from 'react-dom';
import { withRouter } from 'react-router-dom';

axios.defaults.headers.post['Accept'] = 'application/json';
axios.defaults.headers.get['Accept'] = 'application/json';

class FichaDeManutencao extends Component {
	state = {}

	constructor(props) {
		super(props);

	}

	componentDidMount() {
		//window.addEventListener("resize", this.handleResize);
	}

	handleResize() {
	}
	render() {

		return (
			<PageTemplate >
				<Wrapper padding={'0 0 0'} width="100%" minHeight="274px" ref={el => this.highlightWrapper = el}>
					Ficha de manutenção
				</Wrapper>
			</PageTemplate>
		)
	}
}

export default withTheme(withRouter(FichaDeManutencao));