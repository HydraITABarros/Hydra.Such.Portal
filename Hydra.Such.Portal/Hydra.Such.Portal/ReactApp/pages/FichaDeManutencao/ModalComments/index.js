import React, { Component } from 'react';
import _theme from '../../../themes/default';
import { Button, Text, Icon, Circle, Wrapper, OmDatePicker, MenuItem, Input, Select, Avatars, Modal, Tooltip, Spacer } from 'components';
import { Grid } from '@material-ui/core';
import "./index.scss";
import ReactDOM from 'react-dom';

const { DialogTitle, DialogContent, DialogActions } = Modal;

class Comments extends Component {
	state = {
		equipments: [],
		comments: [
			{ equipment: null }
		],
		open: false
	}
	constructor(props) {
		super(props);
		this.handleChange = this.handleChange.bind(this);
		this.state.equipments = this.props.$equipments.value;
		this.state.open = !!props.open;
	}
	handleChange(e, value) {
		this.setState({

		});
	}

	componentWillReceiveProps(nextProps) {
		if (nextProps.open !== this.state.open) {

			this.setState({ open: nextProps.open });
		}
	}
	render() {
		var options = [];
		this.state.equipments.map((item, i) => {
			var val = { value: item, label: "#" + (i + 1) + " " + item.numEquipamento };
			if ((this.state.comments.filter((i) => { return i.equipment == item.idEquipamento; }).length == 0)) {
				options.push(val);
			};
		});

		return (
			<Modal
				onClose={() => { this.setState({ open: false }); this.props.onClose ? this.props.onClose() : null; }}
				onOpen={() => this.setState({ open: true })}
				open={this.state.open}
				action={this.props.children}
				containerStyle={this.props.containerStyle}
				children={
					<div className="modal-signature">
						<DialogTitle>
							<Text h2><Icon observacoes /> Observação</Text>
						</DialogTitle>
						<hr />
						<DialogContent>
							<Text b>
								{this.props.description}
							</Text>
							{this.state.comments.map((item, i) => {

								return (
									<Grid key={i} container spacing={16} direction="row" justify="space-between" alignitems="top" maxwidth={'100%'}>
										<Grid item xs={12}>

										</Grid>
										<Grid item xs={12} md={4}>
											<Select
												onChange={(selected) => {
													item.equipment = selected.target.value;

													this.setState({ comments: this.state.comments });
												}}
												placeholder="teste"
												value={item.equipment ? item.equipment : 0}>

												{options.map((o, j) => {
													return <MenuItem key={j} value={o.value}>{o.label}</MenuItem>
												})}
											</Select>
										</Grid>
										<Grid item xs={12} md={7}>
											<Input multiline rows={3}
												disabled={item.equipment == null}
												$value={item.equipment ? item.equipment[this.props.category][this.props.itemIndex].$observacoes : null}
												value={item.equipment == null ? "" : undefined}
												onBlur={() => {
													this.setState({});
												}}></Input>
										</Grid>
										<Grid item xs={12} md={1}>
											{this.state.equipments.length > this.state.comments.length ?
												<Button round
													disabled={
														item.equipment == null ||
														item.equipment[this.props.category][this.props.itemIndex].observacoes == null ||
														item.equipment[this.props.category][this.props.itemIndex].observacoes == ""
													}
													onClick={() => {
														if (this.state.comments.length > this.state.comments.length) {
															this.state.comments.push({ equipment: null });
														}
														this.setState({});
													}}>
													<Icon add />
												</Button> :
												<Button round
													disabled={
														item.equipment == null
													}
													onClick={() => {
														item.equipment[this.props.category][this.props.itemIndex].$observacoes.value = "";
														this.state.comments.splice(i, 1);
														if (this.state.comments.length == 0) {
															this.state.comments.push({ equipment: null });
														}
														this.setState({});
													}}
												>
													<Icon remove />
												</Button>
											}
										</Grid>
										<Grid item xs={12}>
											<br />
										</Grid>
									</Grid>
								);
							})}
							<br />
						</DialogContent>
						<hr />
						<DialogActions>
							<Button primary onClick={() => {
								this.setState({ open: false });
								this.props.onClose ? this.props.onClose() : null;
							}} color="primary">Guardar</Button>
						</DialogActions>
					</div>
				} />
		)
	}
}

export default Comments;