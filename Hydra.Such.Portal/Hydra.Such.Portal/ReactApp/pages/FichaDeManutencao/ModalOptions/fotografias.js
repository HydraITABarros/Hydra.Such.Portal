import React, {Component} from 'react';
import {Text, Wrapper} from 'components';
import styled, {css, theme, injectGlobal, withTheme} from 'styled-components';
import MuiGrid from '@material-ui/core/Grid';
import _theme from '../../../themes/default';
import axios from "axios";
import moment from 'moment';
import Lightbox from "react-image-lightbox";
import "react-image-lightbox/style.css";
import "./fotografias.scss";


const Grid = styled(MuiGrid)`
    position: relative;
`
const DocImg = styled.div` 
    display: inline-block;
    width: 64px;
    height: 64px;
    border: 1px solid ${_theme.palette.primary.light};
    margin-right: 8px;
    margin-bottom: 4px;
    background-image: url('${props => props.src}');
    background-position: center center;
    background-repeat: no-repeat;
    background-size: cover;
`
const WrapperText = styled(Wrapper)`&&{
    margin-left: 16px;
}
`
const TextDate = styled(Text)`&&{
  text-align: right;
  display: block;
}
`

class Fotografias extends Component {

    state = {
        pictures: [],
        planEquipmentMap: {},
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
            '&documentType=image'
        ).then((e) => {
            console.log(e);
            this.setState({
                pictures: _.chain(e.data)
                    .groupBy("createdAt")
                    .map((value, key) => ({date: key, pictures: value}))
                    .value()
            }, () => {
                console.log(this.state.pictures)
            });
        }).catch((e) => {

        });
    }

    render() {
        this.pictures = [];
        return (
            <div>

                <Wrapper padding={'0 0 16px'}>
                    <Grid container spacing={1}>

                        {this.state.pictures.map((group, i) => {
                            return (
                                <Grid item container xs={12} key={i}>
                                    <Grid item xs={1}>
                                        <TextDate>{moment(group.date).format('DD MMM YYYY')}</TextDate>
                                    </Grid>
                                    <Grid item xs={11}>
                                        <WrapperText>
                                            {group.pictures.map((picture, j) => {
                                                this.pictures.push(picture.url);
                                                var photoIndex = this.pictures.length - 1;
                                                return (
                                                    <div className={'inline-block w-64'} key={j}>
                                                        <div className={'l-h-0'}>
                                                            <DocImg src={picture.url + "?thumb=true"}
                                                                    className={'m-b-0 m-r-0 cursor-pointer'}
                                                                    onClick={() => {
                                                                        this.setState({
                                                                            lightboxOpen: true,
                                                                            photoIndex
                                                                        });
                                                                    }}/>
                                                        </div>
                                                        <Text span className={'to-ellipsis ws-nowrap l-h-1'}>
                                                            <small className={'f-s-9 color-primary-medium m-t-1'}>
                                                                {this.state.planEquipmentMap[picture.idRelatorio]}
                                                            </small>
                                                        </Text>
                                                    </div>
                                                )
                                            })}
                                        </WrapperText>
                                    </Grid>
                                </Grid>
                            )
                        })}

                    </Grid>
                </Wrapper>

                {this.state.lightboxOpen && (
                    <Lightbox
                        mainSrc={this.pictures[this.state.photoIndex]}
                        nextSrc={this.pictures[(this.state.photoIndex + 1) % this.pictures.length]}
                        prevSrc={this.pictures[(this.state.photoIndex - 1) % this.pictures.length]}
                        onCloseRequest={() => this.setState({lightboxOpen: false})}
                        onMovePrevRequest={() =>
                            this.setState({
                                photoIndex: (this.state.photoIndex + this.pictures.length - 1) % this.pictures.length
                            })
                        }
                        onMoveNextRequest={() =>
                            this.setState({
                                photoIndex: (this.state.photoIndex + 1) % this.pictures.length
                            })
                        }
                    />
                )}

            </div>
        )
    }
}

export default Fotografias;