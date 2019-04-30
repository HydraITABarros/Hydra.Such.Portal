import React from 'react';
import { default as UUID } from "node-uuid";
import { Text } from 'components';
import _theme from '../../themes/default';
import d3Circle from './circle';
import ReactTooltip from 'react-tooltip';
import { renderToString } from 'react-dom/server';
import styled, { css, theme } from 'styled-components';

import { propTypes, defaultProps } from './types';

const CircleWrapper = styled.div`
    position: relative;
    display: inline-block;
`

const CircleTotal = styled.div`
    padding-top: 1%;
    position: absolute;
    text-align: center;
    top: 0;
    left: 0;
    margin-top: 50%;
    margin-left: 50%;
    transform: translate(-50%,-50%);
    p {
        margin:0;
    }
    span {
        position: relative;
        letter-spacing: -0.03em;
        left: -0.02em;
    }
`
const tooltip = css`&& {
    margin-top: -7px;
    opacity: 1;
    padding: 1px;
    background: white;
    border: 1px solid ${_theme.palette.primary.light};
    border-radius: 5px;
    padding: 7px 16px 7px 41px;
    color: black;
    &&:after {
        border-top-color: ${_theme.palette.primary.light};
        opacity: 0;
    }
}
`

const TooltipIcon = styled.span`
    font-size: 23px;
    color: ${props => props.color};
    position: absolute;
    top: -2px;
    left: 1px;
    bottom: 0;
    margin: auto;
    text-shadow: 0 0 1px ${props => props.color};
    padding: 9px;
`

const Tooltip = styled(ReactTooltip)`${tooltip}`;

class Circle extends React.Component {

    state = {
        tooltipReady: false,
        id: "_" + UUID.v4()
    }

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        d3Circle({
            el: "#" + this.state.id,
            width: this.props.width,
            trailValue: this.props.trailValue,
            strokeValue: this.props.strokeValue,
            trailColor: this.props.trailColor,
            strokeColor: this.props.strokeColor,
            trailTooltipHtml: renderToString(<span><TooltipIcon color={this.props.trailColor}>{this.props.trailIcon}</TooltipIcon> <Text b>{this.props.trailValue}</Text></span>),
            strokeTooltipHtml: renderToString(<span><TooltipIcon color={this.props.strokeColor}>{this.props.strokeIcon}</TooltipIcon> <Text b>{this.props.strokeValue}</Text></span>)
        });
        this.setState({ tooltipReady: true });
    }

    render() {
        return (
            <div>
                <CircleWrapper>
                    <div id={this.state.id} ></div>
                    <CircleTotal>
                        <Text dataBig>
                            {this.props.trailValue + this.props.strokeValue}
                        </Text>
                        <Text p>
                            {this.props.label}
                        </Text>
                    </CircleTotal>
                </CircleWrapper>
                {this.state.tooltipReady ? <Tooltip /> : ''}
            </div>
        )
    }
}

Circle.propTypes = propTypes;
Circle.defaultProps = defaultProps;

export default Circle;
