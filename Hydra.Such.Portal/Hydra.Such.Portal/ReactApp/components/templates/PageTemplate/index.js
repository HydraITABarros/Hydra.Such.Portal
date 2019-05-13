// https://github.com/diegohaz/arc/wiki/Atomic-Design#templates
import React from 'react'
import PropTypes from 'prop-types'
import styled from 'styled-components'
import { size } from 'styled-theme'

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

const PageTemplate = ({
    header, hero, sponsor, children, footer,highlight, ...props
}) => {
    return (
        <Wrapper {...props}>
            {header && <Header>{header}</Header>}
            <Highlight>{highlight}</Highlight>
            <Content>{children}</Content>
            {footer && <Footer>{footer}</Footer>}
        </Wrapper>
    )
}

PageTemplate.propTypes = {
    highlight: PropTypes.node,
    header: PropTypes.node,
    footer: PropTypes.node,
    children: PropTypes.any.isRequired,
}

export default PageTemplate
