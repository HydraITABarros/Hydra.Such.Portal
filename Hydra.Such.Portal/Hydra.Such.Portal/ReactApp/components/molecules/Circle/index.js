import React from 'react';
import {default as UUID} from "node-uuid";
import {Text} from 'components';
import d3Circle from './circle';
import ReactTooltip from 'react-tooltip';
import {renderToString} from 'react-dom/server';
import styled, {css, theme} from 'styled-components';

import {propTypes, defaultProps} from './types';
import {prop} from 'styled-tools';

const CircleWrapper = styled.div`
    position: relative;
    display: inline-block;
    z-index: 1;
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
    z-index: -1;
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
    border: 1px solid ${props => props.theme.palette.primary.light};
    border-radius: 5px;
    padding: 7px 16px 7px 16px;
    color: black;
    line-height: 20px;
    height: 37px;
    &&:after {
        border-top-color: ${props => props.theme.palette.primary.light};
        opacity: 0;
    }
}
`

const TooltipIcon = styled.span`
    font-size: 23px;
    color: ${props => props.color};
    position: relative;
    display: inline-block;
    margin-top: -5px;
    margin-bottom: 0px;
    top: 1px;
    left: -5px;
    height: 14px;
    line-height: 14px;
    text-shadow: 0 0 1px ${props => props.color};
    padding: 0;
`

const Tooltip = styled(ReactTooltip)`${tooltip}`;


class Circle extends React.Component {

    state = {
        tooltipReady: false,
        id: "_" + UUID.v4(),
        ready: true,
        loading: true
    }

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        //this.refreshCircle();
    }

    componentWillReceiveProps(props) {

        if (this.state.loading != props.loading) {
            this.setState({loading: props.loading});
        }

        if (this.state.trailValue != props.trailValue || this.state.strokeValue != props.strokeValue) {
            this.refreshCircle();
        }

    }

    refreshCircle() {
        let count = 0;
        this.setState({
            ready: false, tooltipReady: false,
            trailValue: this.props.trailValue,
            strokeValue: this.props.strokeValue,
        }, () => {
            count++;
            if (count > 1) return;
            this.setState({ready: true}, () => {

                d3Circle({
                    el: "#" + this.state.id,
                    width: this.props.width,
                    trailValue: this.props.trailValue,
                    strokeValue: this.props.loading ? 0 : this.props.strokeValue,
                    trailColor: this.props.trailColor,
                    strokeColor: this.props.strokeColor,
                    trailTooltipHtml: renderToString(<span><TooltipIcon
                        color={this.props.trailColor}>{this.props.trailIcon}</TooltipIcon> <Text
                        b>{this.props.trailValue - this.props.strokeValue}</Text></span>),
                    strokeTooltipHtml: renderToString(<span><TooltipIcon
                        color={this.props.strokeColor}>{this.props.strokeIcon}</TooltipIcon> <Text
                        b>{this.props.strokeValue}</Text></span>)
                });
                this.setState({tooltipReady: true}, () => {
                    count = 0;
                });
            })
        });
    }

    render() {
        return (
            <div>
                <CircleWrapper>
                    {this.state.ready && <div id={this.state.id}></div>}

                    {!this.state.loading &&
                    (this.props.full ?
                            <CircleTotal>
                                <Text dataBig
                                      style={{fontSize: '50px', lineHeight: '70px', color: this.props.trailColor}}>
                                    {this.props.trailValue - this.props.strokeValue}<span style={{
                                    fontSize: '50px',
                                    color: this.props.strokeColor
                                }}>/{this.props.strokeValue}</span>
                                </Text>
                                <Text p style={{color: this.props.strokeColor}}>
                                    {this.props.label}
                                </Text>
                            </CircleTotal> :
                            <CircleTotal>
                                <Text dataBig>
                                    {this.props.trailValue /*+ this.props.strokeValue*/}
                                </Text>
                                <Text p>
                                    {this.props.label}
                                </Text>
                            </CircleTotal>
                    )
                    }

                </CircleWrapper>
                {/*{this.state.tooltipReady && !this.state.loading ? <Tooltip/> : ''}*/}
            </div>
        )
    }
}

Circle.propTypes = propTypes;
Circle.defaultProps = defaultProps;

export default Circle;
