// https://github.com/diegohaz/arc/wiki/Atomic-Design#templates
import React, { Component } from 'react'
import PropTypes from 'prop-types'
import styled from 'styled-components'
import { size } from 'styled-theme'
import { renderComponent } from 'recompose'

const Wrapper = styled.div`
  
`

const Header = styled.header`
  
`

const Highlight = styled.section`
  
`
const Content = styled.section`
    position: relative;
`
const Footer = styled.footer`
  
`

class PageTemplate extends Component {
	constructor(props) {
		super(props);
	}

	render() {
		//var header, hero, sponsor, children, footer, highlight = this.props;
		return (
			<Wrapper {...this.props}>
				{this.props.header && <Header>{this.props.header}</Header>}
				<Highlight>{this.props.highlight}</Highlight>
				<Content>{this.props.children}</Content>
				{this.props.footer && <Footer>{this.props.footer}</Footer>}
			</Wrapper>
		)
	}
}

PageTemplate.propTypes = {
	highlight: PropTypes.node,
	header: PropTypes.node,
	footer: PropTypes.node,
	children: PropTypes.any.isRequired,
}

export default PageTemplate
