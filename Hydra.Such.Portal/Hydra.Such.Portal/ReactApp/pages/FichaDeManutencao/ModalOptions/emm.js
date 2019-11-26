import React, {Component} from 'react';
import styled from 'styled-components';
import MuiGrid from '@material-ui/core/Grid';
import {Input, Button, Icon, Text, Wrapper, Select, MenuItem} from 'components';
import axios from 'axios';
import './emm.scss';
import SimpleReactValidator from 'simple-react-validator';
//import _theme from '../../../themes/default';

const Grid = styled(MuiGrid)`
    position: relative;
`;


const TextCol = styled(Text)`&&{
    line-height: 36px;
}`;

class Emm extends Component {

    state = {
        emms: []
    }

    constructor(props) {
        super(props);
        this.findEmmBySerial = this.findEmmBySerial.bind(this);

        if (this.props.$equipments) {
            this.state.emms = fromEquipmentsPlanToEmms(this.props.$equipments.value, true);
            this.state.emms.push({selected: this.props.$equipments.value, emm: null});
        }

        this.validator = new SimpleReactValidator();
    }

    componentDidMount() {
        if (this.props.$equipments) {
            this.state.emms = fromEquipmentsPlanToEmms(this.props.$equipments.value, true);
            this.state.emms.push({selected: this.props.$equipments.value, emm: null});
        }
    }

    componentDidUpdate(prevProps, prevState, snapshot) {

        if (prevProps.$equipments != this.props.$equipments && !!this.props.$equipments) {
            this.state.emms = fromEquipmentsPlanToEmms(this.props.$equipments.value, true);
            this.state.emms.push({selected: this.props.$equipments.value, emm: null});
        }

    }

    findEmmBySerial(emm) {
        if (!this.validator.allValid()) {
            this.validator.showMessages();
            this.forceUpdate();
            return;
        }
        emm.serialError = false;
        axios.get('/ordens-de-manutencao/emms/find', {
            params: {
                orderId: "OM1907378",
                serial: emm.serial
            }
        }).then((result) => {

            emm.emm = result.data;
            emm.selected.map((equipment, i) => {
                if (equipment.emms == null) {
                    equipment.emms = [];
                }
                equipment.emms.push(emm.emm);

                return equipment;
            });

            this.state.emms[this.state.emms.length - 1].emm = emm.emm;

            this.state.emms.push({selected: this.props.$equipments.value, emm: null});

            this.setState({}, () => {
            });
        }).catch((err) => {
            emm.serialError = true;
            this.setState({});
        });
    };


    removeEmm(emm) {

        emm.selected.map((equipment, i) => {

            return equipment.emms = equipment.emms.filter((_emm, i) => {
                return _emm.numSerie != emm.emm.numSerie;
            });

        });

        var emms = this.state.emms.filter((item, i) => {
            return item.emm != emm.emm;
        });

        if (emms.length == 0) {
            emms = [{selected: this.props.$equipments.value, emm: null}];
        }

        this.state.emms = emms;

        console.log("IMPORTANT", this.props.$equipments.value);
        this.setState({}, () => {
        });
    };

    render() {
        this.validator.purgeFields();
        return (
            <div>
                <Wrapper padding={'0 0 16px'}>

                    {this.state.emms.map((emm, i) => {
                        return (
                            <Grid container spacing={1} key={i}>
                                <Grid item xs={10} md={2}>
                                    <div>
                                        <Select
                                            multiple
                                            value={emm.selected}
                                            onChange={(e) => {
                                                emm.selected = e.target.value;
                                                this.setState({});
                                            }}
                                            error={!!this.validator.message('equipments_' + i, emm.selected, 'required')}
                                        >
                                            {/*<MenuItem key={""} value={0}>Todos</MenuItem>*/}
                                            {this.props.$equipments && this.props.$equipments.value.map((o, j) => {
                                                return <MenuItem
                                                    key={j}
                                                    disabled={!!emm.emm}
                                                    value={o}>{"#" + (j + 1) + " " + o.numEquipamento}</MenuItem>
                                            })}
                                        </Select>
                                    </div>
                                    <div className="hide">
                                        {this.validator.message('equipments_' + i, emm.selected, 'required')}
                                    </div>
                                </Grid>
                                <Grid item xs={10} md={5}>
                                    <div className="input-width-button">
                                        <Input
                                            key={emm.emm && emm.emm.numSerie}
                                            disabled={!!emm.emm}
                                            error={emm.serialError || !!this.validator.message('serial_' + i, emm.selected, 'required')}
                                            className={'input-width-button__input'}
                                            defaultValue={emm.emm && emm.emm.numSerie}
                                            placeholder={"Inserir Numero de Série"} onChange={(e) => {
                                            emm.serial = e.target.value;
                                        }}/>

                                        <div className="hide">
                                            {this.validator.message('serial_' + i, emm.selected, 'required')}
                                        </div>
                                        {!emm.emm &&
                                        <Button
                                            round
                                            className={'input-width-button__button'}
                                            onClick={(e) => {
                                                this.findEmmBySerial(emm);
                                            }}
                                        ><Icon add/></Button>
                                        }
                                        {!!emm.emm &&
                                        <Button
                                            round
                                            className={'input-width-button__button'}
                                            onClick={(e) => {
                                                this.removeEmm(emm);
                                            }}
                                        ><Icon remove/></Button>
                                        }
                                    </div>
                                </Grid>

                                <Grid item xs={12} md={5}>
                                    <div className="p-t-10">
                                        <Text b className={"p-l-35 p-r-20"}>{emm.emm && emm.emm.marcaText}</Text>
                                        <Text span className={"p-l-20 p-r-20"}>{emm.emm && emm.emm.modeloText}</Text>
                                    </div>
                                </Grid>
                                <Grid item xs={12}>
                                </Grid>

                            </Grid>
                        )
                    })}
                </Wrapper>

            </div>
        )
    }
}

const fromEquipmentsPlanToEmms = (equipments, grouped) => {
    var emms = {};
    if (!equipments) {
        return [];
    }
    equipments.map((equipment, index) => {
        if (!equipment.emms) {
            return;
        }
        console.log("SUPER", equipment)
        equipment.emms.map((emm, x) => {
            if (!emms[emm.numSerie]) {
                emms[emm.numSerie] = emm;
                emms[emm.numSerie].equipments = [];
            }
            emms[emm.numSerie].equipments.push(equipment);
        });
    });

    var retval = [];
    if (!grouped) {
        Object.keys(emms).map((key, i) => {
            var emm = emms[key];
            if (emm.equipments.length < equipments.length) {
                emm.equipments.map((e, j) => {
                    retval.push({selected: e, emm});
                });
            } else {
                retval.push({selected: 0, emm});
            }
            retval.push();
        });
    } else {
        Object.keys(emms).map((key, i) => {
            var emm = emms[key];
            retval.push({selected: emm.equipments, emm});
        });
    }

    return retval;
};

export default Emm;