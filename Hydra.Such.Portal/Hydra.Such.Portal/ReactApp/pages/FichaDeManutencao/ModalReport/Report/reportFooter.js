import React, {Component} from 'react';
import MuiTabs from '@material-ui/core/Tabs';
import MuiTab from '@material-ui/core/Tab';
import styled, {css, theme, injectGlobal, withTheme} from 'styled-components';
import {
    Button,
    Text,
    Icon,
    Circle,
    Wrapper,
    OmDatePicker,
    CheckBox,
    Input,
    Avatars,
    ModalLarge,
    Tooltip,
    Spacer
} from 'components';
import AppBar from '@material-ui/core/AppBar';
import {Grid} from '@material-ui/core';
import functions from '../../../../helpers/functions';
import _theme from '../../../../themes/default';
import ReactDOM from 'react-dom';
import reactElementToJSXString from 'react-element-to-jsx-string';
import '../index.scss';

const {DialogTitle, DialogContent, DialogActions} = ModalLarge;


var timeout = 0;

class SplitedReport extends Component {
    state = {}

    constructor(props) {
        super(props);
    }


    componentDidMount() {

    }

    componentWillReceiveProps(nextProps) {
    }

    componentWillUnmount() {
    }

    render() {
        let date = this.props.date;
        let assinaturaTecnico = this.props.assinaturaTecnico;
        let assinaturaCliente = this.props.assinaturaCliente;
        let assinaturaSie = this.props.assinaturaSie;
        let technical = this.props.technical;

        return (
            <div className="report__footer">
                <div className="col-xs-12 p-b-25">
                    {(date &&
                        <div className="report__label">
                            <Text b className="f-s-12">Data</Text>&nbsp;&nbsp;
                            <Text span>{date}</Text>
                        </div>
                    )}
                </div>
                <div className="col-xs-6">
                    <div className="report__label p-b-5">
                        <Text h2>Such</Text>
                    </div>
                    <div className="col-xs-12 p-l-0 p-r-0">
                        {technical &&
                        <div
                            className="report__header__avatar__wrapper text-left m-t-5 m-b-5 text-normal">
                            <Avatars.Avatars
                                className="report__header__avatar m-l-0 m-r-15"
                                letter
                                color={_theme.palette.primary.light} data-tip={technical.nome}
                            >
                                {functions.getInitials(technical ? technical.nome || "" : "")}
                            </Avatars.Avatars>
                            <Text span>{technical ? technical.nome || "" : ""}</Text>
                        </div>
                        }
                    </div>
                    <div className="col-xs-5 p-l-0 p-r-0 col-xs-offset-1 p-t-10">
                        {technical &&
                        <img
                            src={assinaturaTecnico}
                            className="report__signature img-responsive"/>
                        }
                    </div>
                    <div className="col-xs-2 p-l-0 p-r-0"></div>
                    <div className="clearfix"></div>
                </div>

                <div className="col-xs-6">
                    <div className="report__label m-b-15">
                        <Text h2>Cliente</Text>
                    </div>
                    <div className="col-xs-5 p-l-0 p-r-0 m-b-15">
                        <div className="text-left m-t-10 m-b-5 f-s-12">
                            <Text b className="f-s-12 text-uppercase l-h-1">
                                Assinatura
                            </Text>
                        </div>
                    </div>
                    <div className="col-xs-5 p-l-0 p-r-0">
                        <img src={assinaturaCliente} className="report__signature img-responsive"/>
                    </div>
                    <div className="col-xs-2 p-l-0 p-r-0 m-t-15"></div>
                    <div className="clearfix"></div>
                    <div className="col-xs-5 p-l-0 p-r-0">
                        <div className="text-left m-t-10 m-b-5 f-s-12 ">
                            <Text b className="f-s-12 text-uppercase l-h-1">
                                Sie/Aprovis. <br/> Assinatura
                            </Text>
                        </div>
                    </div>
                    <div className="col-xs-5 p-l-0 p-r-0">
                        <img src={assinaturaSie} className="report__signature img-responsive"/>
                    </div>
                    <div className="col-xs-2 p-l-0 p-r-0"></div>
                </div>

                <div className="col-xs-12 p-b-15"></div>
                <div className="clearfix"></div>

            </div>
        )
    }
}

export default SplitedReport;