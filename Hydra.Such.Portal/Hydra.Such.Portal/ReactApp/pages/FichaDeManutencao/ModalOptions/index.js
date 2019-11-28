import React, {Component, Fragment} from 'react';
import {
    Button, Text, Icon, ModalLarge
} from 'components';
import './index.scss';
import axios from 'axios';
import Functions from '../../../helpers/functions';
import Bar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import Material from './material';
import Emm from "./emm";
import Upload from './upload';
import Documentos from './documentos';
import Fotografias from './fotografias';

const addLinkedPropsToObject = Functions.addLinkedPropsToObject;

axios.defaults.headers.post['Accept'] = 'application/json';
axios.defaults.headers.get['Accept'] = 'application/json';

const {DialogTitle, DialogContent, DialogActions} = ModalLarge;

class ModalOptions extends Component {
    state = {
        open: false,
        tab: 0
    };

    constructor(props) {
        super(props);
        this.fetch = this.fetch.bind(this);
        this.handleChange = this.handleChange.bind(this);
        //this.fetch();
    }

    handleChange(e, value) {
        console.log("222", value);
        this.setState({
            tab: value
        });
    }

    fetch() {
        return;
        var url = `/ordens-de-manutencao/equipments`;
        var params = {
            categoryId: this.props.categoryId,
            orderId: this.props.orderId
        };
        axios.get(url, {params}).then((result) => {
            var data = result.data;
            if (data) {

                var state = {
                    searchEquipments: data
                }

                this.setState(state);
            }
        }).catch(function (error) {
        }).then(() => {
            this.setState({
                isLoading: false
            });
        });
    }

    componentDidUpdate(props) {

        console.log("IMPPPPP", props.$equipments);
    }

    render() {
        return (

            <ModalLarge
                onOpen={() => {
                    if (this.props.children.props.disabled) {
                        return this.setState({open: false});
                    }
                    this.setState({open: true});
                }}
                open={this.state.open}
                onClose={() => {
                    this.setState({open: false, tab: 0});
                }}
                action={this.props.children} children={
                <div id={"emm-modal"}>
                    <DialogTitle>
                        <Bar position="static" color="default" className={"emm__bar"}>
                            <Tabs
                                className={"emm__tabs"}
                                value={this.state.tab}
                                onChange={this.handleChange}
                                indicatorColor="primary"
                                textColor="primary"
                                variant="standard"
                                scrollButtons="off"
                            >
                                <Tab className={"emm__tabs"} label={<Text b><Icon meter/>EMMs</Text>}/>
                                <Tab className={"emm__tabs"} label={<Text b><Icon material/>Mat. Aplicado</Text>}/>
                                <Tab className={"emm__tabs"} label={<Text b><Icon fotografias/>Fotografias</Text>}/>
                                <Tab className={"emm__tabs"} label={<Text b><Icon folder/>Documentos</Text>}/>
                                <Tab className={"emm__tabs"} label={<Text b><Icon upload/>Upload</Text>}/>
                            </Tabs>
                            <hr/>
                        </Bar>
                    </DialogTitle>

                    <DialogContent>
                        {this.state.tab == 0 &&
                        <Emm orderId={this.props.orderId} $equipments={this.props.$equipments}/>}
                        {this.state.tab == 1 &&
                        <Material orderId={this.props.orderId} $equipments={this.props.$equipments}/>}
                        {this.state.tab == 2 &&
                        <Fotografias orderId={this.props.orderId} $equipments={this.props.$equipments}/>}
                        {this.state.tab == 3 &&
                        <Documentos orderId={this.props.orderId} $equipments={this.props.$equipments}/>}
                        {this.state.tab == 4 &&
                        <Upload orderId={this.props.orderId} $equipments={this.props.$equipments}/>}

                    </DialogContent>
                    <hr/>
                    <DialogActions>
                        <Button onClick={() => this.setState({open: false, tab: 0})} primary
                                color="primary">Guardar</Button>
                    </DialogActions>
                </div>
            }/>
        )
    }
}

export default ModalOptions;