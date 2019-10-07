import React, { Component } from 'react';
import styled from 'styled-components';
import Color from 'color';

const Root = styled.div`
	display: inline-block;
	position: relative;
	height: 64px;
	width: ${props => props.width};
`;

const Content = styled.div`
	position: absolute;
	display: inline-block;
	border-radius: 8px;
	border-top-right-radius: ${ props => !props.right ? '0' : '8px'};
	border-bottom-right-radius: ${ props => !props.right ? '0' : '8px'};
	border-top-left-radius: ${ props => !props.right ? '8px' : '0'};
	border-bottom-left-radius: ${ props => !props.right ? '8px' : '0'};
	/* white-space: nowrap; */
	padding: ${ props => props.text ? '20px' : '12px'};
	height: 64px;
	top:0;
	left: 0;
	right: 0;
	bottom: 0;
	background: ${ props => props.odd ? Color(props.theme.palette.primary.dark).alpha(0.05).rgb().toString() : Color(props.theme.palette.bg.white).alpha(0.05).rgb().toString()};
	line-height: 16px;
	
`;

const PlanRow = (props) => {
	return (
		<Root width={props.width ? props.width + 'px' : '100%'}  >
			<Content odd={props.odd} right={props.right} text={props.text}>
				{props.children}
			</Content>
		</Root>
	);
}

export default PlanRow;