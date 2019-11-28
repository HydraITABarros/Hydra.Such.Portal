import React, {Component} from 'react';
import {Input, Button, Icon, Text, Wrapper, Select, MenuItem} from 'components';
import MuiAddIcon from '@material-ui/icons/Add';
import styled, {css, theme, injectGlobal, withTheme} from 'styled-components';
import MuiGrid from '@material-ui/core/Grid';
import _theme from '../../../themes/default';
import axios from "axios";
import moment from "moment";


const Grid = styled(MuiGrid)`
    position: relative;
`
const DocImg = styled.div` 
    display: inline-block;
    width: 64px;
    height: 64px;
    border: 1px solid ${_theme.palette.primary.medium};
    margin-right: 16px;
`
const ButtonNew = styled(Button)`&&{
}
`;
const WrapperText = styled(Wrapper)`&&{
    margin-left: 16px;
}
`;

const TextDoc = styled(Text)`&&{
  text-transform:none; 
  margin-top: 4px;
  font-weight: 400;
  line-height: 18px;
}
`;

class Documentos extends Component {
    state = {
        documents: [],
        photoIndex: 0,
        lightboxOpen: false
    };

    constructor(props) {
        super(props);
        this.fetch = this.fetch.bind(this);
        if (this.props.$equipments) {
            this.fetch();
            var planEquipmentMap = {};
            this.props.$equipments.value.map((e) => {
                planEquipmentMap[e.id] = e.numEquipamento;
            });
            this.state.planEquipmentMap = planEquipmentMap;
        }

    };

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.$equipments != this.props.$equipments) {
            this.fetch();
            var planEquipmentMap = {};
            this.props.$equipments.value.map((e) => {
                planEquipmentMap[e.id] = e.numEquipamento;
            });
            this.state.planEquipmentMap = planEquipmentMap;
        }
    }

    fetch() {
        axios.get('/ordens-de-manutencao/files/' + this.props.orderId + '/?planIds=' +
            (this.props.$equipments ? this.props.$equipments.value.map((e) => e.id).join(",") : "") +
            '&documentType=document'
        ).then((e) => {
            console.log(e);
            this.setState({
                documents: e.data
            }, () => {
                console.log(this.state.documents)
            });
        }).catch((e) => {

        });
    }

    render() {
        this.documents = [];
        return (
            <div>

                <Wrapper padding={'0 0 16px'}>
                    <Grid container spacing={1}>

                        {this.state.documents.map((document, i) => {
                            this.documents.push(document.url);
                            var documentIndex = this.documents.length - 1;
                            return (
                                <Grid item key={i} container xs={12} md={4} onClick={() => {
                                    window.open(document.url, '_blank');
                                }} className={"cursor-pointer"}>
                                    <Grid item xs={3} lg={2}>
                                        <DocImg/>
                                    </Grid>
                                    <Grid item xs={9} lg={10}>
                                        <WrapperText>
                                            <Text b className={'ws-nowrap to-ellipsis'}>{document.name}</Text><br/>
                                            <TextDoc label>
                                                <span className={'ws-nowrap to-ellipsis'}>
                                                    .{document.name.split('.')[1]} ({Math.round((document.size * 0.001) * 100) / 100}kb)
                                                </span> <br/>
                                                <span className={'ws-nowrap'}>
                                                    {moment(document.createdAt).format('DD MMM YYYY')}
                                                </span>
                                            </TextDoc>
                                        </WrapperText>
                                    </Grid>
                                </Grid>
                            )
                        })}

                    </Grid>
                </Wrapper>

            </div>
        )
    }
}

export default Documentos;