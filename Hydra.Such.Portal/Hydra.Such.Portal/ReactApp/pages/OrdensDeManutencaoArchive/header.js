import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import { Text, Icon, Circle, Wrapper, Spacer, Button } from 'components';
import Functions from '../../helpers/functions';
import MuiGrid from '@material-ui/core/Grid';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { createMuiTheme } from '@material-ui/core/styles';
import Hidden from '@material-ui/core/Hidden';
import { withRouter } from 'react-router-dom';
injectGlobal`
	.transparent {
		opacity: 0
	}
`
const muiTheme = createMuiTheme();
const breakpoints = muiTheme.breakpoints.values;

const Grid = styled(MuiGrid)`
    position: relative;
`

const TextHeader = styled(Text)`
    display: inline-block;
`
const CircleOm = {
	wrapper: styled.div`
        margin: auto;
        white-space: nowrap;
        ${props => props.primary && css`
        
        `}
    `,
	icon: styled.div`
        font-size: 33px;
        line-height: 35px;
        display: inline-block;
        vertical-align: middle;
        padding: 17px;
        > [class*="icon"] {
            text-align: center;
            width: 57px;
            height: 57px;
            display: inline-block;
            background:  ${props => props.background};
            color:  ${props => props.color};
            border-radius: 50%;
            line-height: 56px;
            font-size: 70px;
            position: relative;
            &:before {
                position: absolute;
                left: -7px;
                right: -7px;
                top: -7px;
                bottom: -7px;
                margin: auto;
                text-align: center;
                line-height: 70px;
                font-size: ${props => props.fontSize ? props.fontSize : '33px'};
            }
        }
    `,
	chart: styled.div`
        padding: 6px;
        display: inline-block;
        vertical-align: middle;
        text-align: center;
    `,
};

const CircleOmWrapper = styled(CircleOm.wrapper)`
    @media (max-width: ${breakpoints["sm"] + "px"}) {
        transform: scale(0.8) translateX(-50%);
        transform-origin: 0;
        left: 50%;
        position: relative;
    }
    @media (max-width:  455px ) {
        transform: scale(0.7) translateX(-50%);
        margin-top: -20px;
        margin-bottom: -20px;
    }
    @media (max-width:  400px ) {
        transform: scale(0.6) translateX(-50%);
        margin-top: -30px;
        margin-bottom: -30px;
    }
    @media (max-width:  350px ) {
        transform: scale(0.5) translateX(-50%);
        margin-top: -40px;
        margin-bottom: -40px;
    }
`;

class Header extends Component {
	state = {
		isLoading: true
	}

	constructor(props) {
		super(props);
	}

	componentDidUpdate(prevProps) {

		if (prevProps.isLoading !== this.state.isLoading) {
			this.setState({ isLoading: prevProps.isLoading });
		}
	}

	render() {
		return (
			<div>
				<Grid container direction="row" justify="space-between" alignitems="top" spacing={0} maxwidth={'100%'} margin={0} ref={el => this.highlightWrapper = el} padding={"200px"}>
					<Grid item xs>
						<Wrapper padding={' 20px 25px'} width="100%" >
							<span>
								<Text b onClick={() => { this.props.history.push(`/ordens-de-manutencao`) }}
									style={{ verticalAlign: 'middle', textDecoration: 'underline', cursor: 'pointer' }}>OMs</Text>
								<Icon arrow-right style={{ verticalAlign: 'middle' }} />
								&nbsp;
								<Text span style={{ verticalAlign: 'middle', position: 'relative', top: '1px' }} >
									Arquivo
								</Text>
							</span>
						</Wrapper>
					</Grid>
					<Grid item xs className="text-center">
						<Wrapper padding={' 8px '} width="100%" className="text-center">
							<span style={{ display: 'inline-block' }}>{this.props.middle}</span>
						</Wrapper>
					</Grid>
					<Grid item xs></Grid>
				</Grid>
			</div>
		)
	}
};

export default withRouter(withTheme(Header));
