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
import {Offline, Online} from "react-detect-offline";

const addLinkedPropsToObject = Functions.addLinkedPropsToObject;

axios.defaults.headers.post['Accept'] = 'application/json';
axios.defaults.headers.get['Accept'] = 'application/json';

const {DialogTitle, DialogContent, DialogActions} = ModalLarge;

class ModalOptions extends Component {
    state = {
        open: false,
        tab: 0,
        online: true
    };

    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.state.online = navigator.onLine;
    }

    handleChange(e, value) {
        console.log("222", value);
        this.setState({
            tab: value
        });
    }

    componentDidMount() {
        window.addEventListener('online', () => {
            setTimeout(() => {
                this.setState({online: true});
            }, 0);
        });

        window.addEventListener('offline', () => {
            setTimeout(() => {
                this.setState({online: false, tab: 0});
            }, 0);
        });
        this.setState({online: navigator.onLine});
    }

    componentDidUpdate(props) {
        this.state.online = navigator.onLine;
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

                                <Tab disabled={!this.state.online}
                                     className={"emm__tabs" + (!this.state.online && " disabled")}
                                     label={<Text b><Icon fotografias/>Fotografias</Text>}/>
                                <Tab disabled={!this.state.online}
                                     className={"emm__tabs" + (!this.state.online && " disabled")}
                                     label={<Text b><Icon folder/>Documentos</Text>}/>
                                <Tab disabled={!this.state.online}
                                     className={"emm__tabs" + (!this.state.online && " disabled")}
                                     label={<Text b><Icon upload/>Upload</Text>}/>

                            </Tabs>
                            <hr/>
                        </Bar>
                    </DialogTitle>

                    <DialogContent>
                        {this.state.tab == 0 &&
                        <Emm orderId={this.props.orderId} $equipments={this.props.$equipments}
                             onChange={this.props.onChange}/>}
                        {this.state.tab == 1 &&
                        <Material orderId={this.props.orderId} $equipments={this.props.$equipments}
                                  onChange={this.props.onChange}/>}

                        <Online>
                            {this.state.tab == 2 &&
                            <Fotografias orderId={this.props.orderId} $equipments={this.props.$equipments}/>}
                            {this.state.tab == 3 &&
                            <Documentos orderId={this.props.orderId} $equipments={this.props.$equipments}/>}
                            {this.state.tab == 4 &&
                            <Upload orderId={this.props.orderId} $equipments={this.props.$equipments}/>}
                        </Online>

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