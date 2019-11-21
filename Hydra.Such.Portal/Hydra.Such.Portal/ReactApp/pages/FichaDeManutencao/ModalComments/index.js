import React, {Component} from 'react';
import _theme from '../../../themes/default';
import {
    Button,
    Text,
    Icon,
    MenuItem,
    Input,
    Select,
    Modal
} from 'components';
import {Grid} from '@material-ui/core';
import "./index.scss";
import ReactDOM from 'react-dom';

const {DialogTitle, DialogContent, DialogActions} = Modal;

class Comments extends Component {
    state = {
        equipments: [],
        comments: [],
        open: false
    }

    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.getOptions = this.getOptions.bind(this);
        this.state.equipments = this.props.$equipments.value;
        this.state.open = !!props.open;

        this.state.equipments.map((equipment) => {
            var observacao = equipment[this.props.category][this.props.itemIndex].$observacoes.value
            if (observacao) {
                this.state.comments.push({equipment: equipment});
            }
        });

        if (this.state.comments.length < 1) {
            this.state.comments = [{equipment: null}];
        }
    }

    handleChange(e, value) {
        this.setState({});
    }

    componentWillReceiveProps(nextProps) {
        if (nextProps.open !== this.state.open) {

            this.setState({open: nextProps.open});
        }
    }

    getOptions() {
        var options = [];
        this.state.equipments.map((item, i) => {
            var val = {value: item, label: "#" + (i + 1) + " " + item.numEquipamento};
            var alreadyCommented = this.state.comments.filter((_i) => {
                return _i.equipment == item;
            });
            
            if (alreadyCommented.length == 0) {
                return options.push(val);
            }
            val.disabled = true;
            options.push(val);

        });
        return options;
    }

    render() {
        var options = this.getOptions();

        return (
            <Modal
                onClose={() => {
                    this.setState({open: false});
                    this.props.onClose ? this.props.onClose() : null;
                }}
                onOpen={() => this.setState({open: true})}
                open={this.state.open}
                action={this.props.children}
                containerStyle={this.props.containerStyle}
                children={
                    <div className="modal-signature">
                        <DialogTitle>
                            <Text h2><Icon observacoes/> Observação</Text>
                        </DialogTitle>
                        <hr/>
                        <DialogContent>
                            <Text b>
                                {this.props.description}
                            </Text>
                            {this.state.comments.map((item, i) => {

                                return (
                                    <Grid key={i} container spacing={2} direction="row" justify="space-between"
                                          alignitems="top" maxwidth={'100%'}>
                                        <Grid item xs={12}>

                                        </Grid>
                                        <Grid item xs={12} md={4}>
                                            <Select
                                                onChange={(selected) => {
                                                    item.equipment = selected.target.value;
                                                    this.setState({comments: this.state.comments});
                                                }}
                                                placeholder="teste"
                                                value={item.equipment ? item.equipment : ''}>
                                                {options.map((o, j) => {
                                                    return <MenuItem key={j} disabled={o.disabled}
                                                                     value={o.value}>{o.label}</MenuItem>
                                                })}
                                            </Select>
                                        </Grid>
                                        <Grid item xs={12} md={7}>
                                            <Input
                                                key={"_" + (new Date().getTime())}
                                                multiline rows={3}
                                                   disabled={item.equipment == null}
                                                   $value={item.equipment ? item.equipment[this.props.category][this.props.itemIndex].$observacoes : null}
                                                   onBlur={() => {
                                                       this.setState();
                                                   }}
                                                   onChange={() => {
                                                       this.setState();
                                                   }}></Input>
                                        </Grid>
                                        <Grid item xs={12} md={1}>
                                            {(i +1 ) == this.state.equipments.length ? <div></div> : i +1 == this.state.comments.length ?
                                                <Button round
                                                        disabled={
                                                            item.equipment == null
                                                        }
                                                        onClick={() => {
                                                            if (this.state.comments.length < this.state.equipments.length) {
                                                                this.state.comments.push({equipment: null});
                                                            }
                                                            this.setState({});
                                                        }} ><Icon add/></Button>
                                                :
                                            <Button round
                                                    disabled={
                                                        item.equipment == null
                                                    }
                                                    onClick={() => {
                                                        item.equipment[this.props.category][this.props.itemIndex].$observacoes.value = "";
                                                        this.state.comments.splice(i, 1);
                                                        if (this.state.comments.length == 0) {
                                                            this.state.comments.push({equipment: null});
                                                        }
                                                        
                                                        this.setState({}, ()=>{

                                                            console.log('IMP', this.state.comments);
                                                        });
                                                        
                                                        
                                                    }}><Icon remove/></Button>
                                            }
                                            <div className="p-t-40"></div>

                                        </Grid>
                                    </Grid>
                                );
                            })}
                            <br/>
                            <div className="p-t-50"></div>
                        </DialogContent>
                        <hr/>
                        <DialogActions>
                            <Button primary onClick={() => {
                                this.setState({open: false});
                                this.props.onClose ? this.props.onClose() : null;
                            }} color="primary">Guardar</Button>
                        </DialogActions>
                    </div>
                }/>
        )
    }
}

export default Comments;