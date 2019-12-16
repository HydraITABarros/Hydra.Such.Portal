import React, {Component} from 'react';
import ReactDOM from 'react-dom';
import {Text, Icon, Circle, Wrapper, Spacer, Button, Avatars} from 'components';
import functions from '../../helpers/functions';
import MuiGrid from '@material-ui/core/Grid';
import styled, {css, theme, injectGlobal, withTheme} from 'styled-components';
import {createMuiTheme} from '@material-ui/core/styles';
import Hidden from '@material-ui/core/Hidden';
import {withRouter} from 'react-router-dom';

const muiTheme = createMuiTheme();
const breakpoints = muiTheme.breakpoints.values;
injectGlobal`
	
`
const CustomWrapper = styled(Wrapper)` && {
	border-bottom: solid 1px ${props => props.theme.palette.primary.keylines};
}`

const Grid = styled(MuiGrid)`&& {
 	padding: 20px 25px;
	white-space: nowrap;
	overflow: hidden;
	text-overflow: ellipsis;
}`

class Breadcrumb extends Component {
    state = {
        isLoading: true,
        order: null,
        progectManager: null,
        info: {
            type: null,
            sector: null,
            status: null
        }
    }

    constructor(props) {
        super(props);
        this.state = {...props};
    }

    componentDidUpdate(props) {
        if (props.order !== this.state.order) {
            this.setState({order: props.order});
        }
    }

    componentDidMount() {
        typeof this.props.onRef == 'function' ? this.props.onRef(this) : '';
    }

    componentWillUnmount() {
        typeof this.props.onRef == 'function' ? this.props.onRef(undefined) : '';
    }

    render() {
        var teamLeader = this.state.order && (this.state.order.maintenanceResponsibleObj ? this.state.order.maintenanceResponsibleObj.nome : this.state.order.responsibleEmployeeObj && responsibleEmployeeObj.nome);
        return (
            <CustomWrapper width="100%" className={this.state.isLoading ? 'transparent' : ''}>
                <MuiGrid container direction="row" justify="space-between" alignitems="middle" spacing={0}
                         maxwidth={'100%'} margin={0}>
                    <Grid item xs={12} sm={3}>
						<span data-tip={this.state.order && this.state.order.no}>
							<Text b onClick={() => {
                                this.props.history.push(`/ordens-de-manutencao`)
                            }}
                                  style={{
                                      verticalAlign: 'middle',
                                      textDecoration: 'underline',
                                      cursor: 'pointer'
                                  }}>OMs</Text>
							<Icon arrow-right style={{verticalAlign: 'middle'}}/>
                            &nbsp;
                            <Text b style={{
                                verticalAlign: 'middle',
                                position: 'relative',
                                top: '1px',
                                textDecoration: 'underline',
                                cursor: 'pointer'
                            }}
                                  onClick={() => {
                                      this.props.history.push(`/ordens-de-manutencao/${this.state.order && this.state.order.no}`)
                                  }}>
								{this.state.order && this.state.order.no}
							</Text>
							<Icon arrow-right style={{verticalAlign: 'middle'}}/>
						</span>
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <MuiGrid container>
                            <Grid item xs={12} sm={5} style={{padding: 0}}>
								<span data-tip={this.state.order && this.state.order.customerName}>
									<Text span style={{verticalAlign: 'middle'}}>
										Cliente&nbsp;
									</Text>
									<Text b style={{verticalAlign: 'middle'}}>
										{this.state.order && this.state.order.customerName}
									</Text>
								</span>
                            </Grid>
                            <Grid item xs={12} sm={5} style={{padding: 0}}>
								<span data-tip={this.state.order && this.state.order.institutionName}>
									<Text span style={{verticalAlign: 'middle'}}>
										Instituição&nbsp;
									</Text>
									<Text b style={{verticalAlign: 'middle'}}>
										{this.state.order && this.state.order.institutionName}
									</Text>
								</span>
                            </Grid>
                            <Grid item xs={12} sm={2} style={{padding: 0}}>
                                <Hidden smDown>
									<span>
										&nbsp;&nbsp;
                                        &nbsp;&nbsp;
									</span>
                                </Hidden>
                                <span>
									{/*<Text span style={{ verticalAlign: 'middle' }}>*/}
                                    {/*	+ Info*/}
                                    {/*</Text>*/}
								</span>
                            </Grid>
                        </MuiGrid>
                    </Grid>
                    <Grid item xs={12} sm={3} style={{paddingRight: '20px'}}>
                        {teamLeader &&
                        <span style={{float: (window.innerWidth > breakpoints.sm ? 'right' : 'none')}}
                              data-tip={teamLeader}>
								<Text span style={{verticalAlign: 'middle'}}>
									Chefe De Projecto &nbsp;&nbsp;
							</Text>
								<span>
									<Avatars.Avatars letter color=""
                                                     style={{marginBottom: '-10px', marginTop: '-10px'}}>
										<span style={{color: 'black'}}>{functions.getInitials(teamLeader)}</span>
									</Avatars.Avatars>
								</span>
							</span>
                        }
                        <Hidden smUp>
                            <Spacer height="5px"/>
                        </Hidden>
                    </Grid>
                </MuiGrid>
            </CustomWrapper>
        )
    }
};

export default withRouter(withTheme(Breadcrumb));
