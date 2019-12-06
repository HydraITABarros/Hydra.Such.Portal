import React, {Component} from 'react';
import {Input, Button, Icon, Text, Wrapper, Select, MenuItem, GSelect} from 'components';
import MuiAddIcon from '@material-ui/icons/Add';
import styled, {css, theme, injectGlobal, withTheme} from 'styled-components';
import MuiGrid from '@material-ui/core/Grid';
import SimpleReactValidator from "simple-react-validator";
import _ from "lodash";

const Grid = styled(MuiGrid)`
    position: relative;
`
const InputModal = styled(Input)`&&{
  }
`;
const ButtonNew = styled(Button)`&&{
}
`;
const TextCol = styled(Text)`&&{
}
`;

class Material extends Component {
    state = {
        materials: []
    }

    constructor(props) {
        super(props);

        if (this.props.$equipments) {
            this.state.materials = fromEquipmentsPlanToMaterials(this.props.$equipments.value, true);
            this.state.materials.push({selected: this.props.$equipments.value, material: getDefaultMaterial()});
        }

        this.validator = new SimpleReactValidator();
    }

    componentDidMount() {
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.$equipments != this.props.$equipments) {
            this.state.materials = fromEquipmentsPlanToMaterials(this.props.$equipments.value, true);
            this.state.materials.push({selected: this.props.$equipments.value, material: getDefaultMaterial()});
        }
    }

    render() {
        this.validator.purgeFields();

        return (
            <div>
                <Wrapper padding={'0 0 16px'}>

                    {this.state.materials.map((material, i) => {
                        let disabled = false;
                        if ((this.state.materials.length - 1 != i)) {
                            disabled = material.selected.filter((item) => {
                                return item.estadoFinal > 0;
                            }).length > 0;
                        }
                        return (
                            <div key={i}>
                                <Grid container spacing={1}>
                                    <Grid item xs={10} md={2}>
                                        <Select
                                            key={material.material.descricao}
                                            multiple
                                            value={(this.state.materials.length - 1 != i) ? material.selected : material.selected.filter((e) => e.estadoFinal == 0)}
                                            disabled={disabled}
                                            onChange={(e) => {
                                                material.selected = e.target.value;
                                                this.setState({});
                                            }}
                                            error={!!this.validator.message('equipments_' + i, material.selected, 'required')}
                                        >
                                            {this.props.$equipments && this.props.$equipments.value.map((o, j) => {
                                                return <MenuItem
                                                    key={j}
                                                    disabled={this.state.materials.length != (i + 1) || o.estadoFinal > 0}
                                                    value={o}>{"#" + (j + 1) + " " + o.numEquipamento}</MenuItem>
                                            })}
                                        </Select>
                                        <div className="hide">
                                            {this.validator.message('equipments_' + i, material.selected, 'required')}
                                        </div>
                                    </Grid>
                                    <Grid item xs={10} md={4}>
                                        <Input
                                            disabled={this.state.materials.length != (i + 1) || disabled}
                                            key={material.material.descricao}
                                            defaultValue={material.material.descricao}
                                            onChange={(e) => {
                                                material.material.descricao = e.target.value;
                                            }}
                                            error={!!this.validator.message('descricao_' + i, material.material.descricao, 'required')}
                                            placeholder={"Inserir Material"}
                                        />
                                        <div className="hide">
                                            {this.validator.message('descricao_' + i, material.material.descricao, 'required')}
                                        </div>
                                    </Grid>
                                    <Grid item xs={10} md={2}>
                                        <Input
                                            disabled={this.state.materials.length != (i + 1) || disabled}
                                            key={material.material.descricao}
                                            defaultValue={material.material.quantidade}
                                            onChange={(e) => {
                                                material.material.quantidade = e.target.value;
                                            }}
                                            placeholder={"Quantidade"}/>
                                    </Grid>
                                    <Grid item xs={10} md={3}>
                                        <Select
                                            value={material.material.fornecidoPor || 0}
                                            disabled={this.state.materials.length != (i + 1) || disabled}
                                            key={material.material.descricao}
                                            onChange={(e) => {
                                                material.material.fornecidoPor = e.target.value;

                                                this.setState({}, () => {
                                                    attachMaterialToEquipments(this.state.materials, this.props.$equipments.value);

                                                    if (this.props.onChange) {
                                                        this.props.onChange();
                                                    }
                                                });

                                            }}
                                            error={!!this.validator.message('descricao_' + i, material.material.fornecidoPor, 'required')}
                                        >
                                            <MenuItem
                                                value={0}
                                                style={{display: 'none'}}>
                                                <span>Fornecido por</span>
                                            </MenuItem>
                                            {[{value: 1, title: "Cliente"}, {value: 2, title: "Such"}].map((o, j) => {
                                                return <MenuItem
                                                    key={i + '' + j}
                                                    value={o.value}>{o.title}</MenuItem>
                                            })}
                                        </Select>
                                        <div className="hide">
                                            {this.validator.message('fornecidoPor_' + i, material.material.fornecidoPor, 'required')}
                                        </div>

                                    </Grid>
                                    <Grid item xs={1} md={1}>
                                        {this.state.materials.length == (i + 1) &&
                                        <Button
                                            round
                                            className={'input-width-button__button' + (disabled ? " disabled" : "")}
                                            onClick={(e) => {
                                                if (disabled) {
                                                    return;
                                                }
                                                if (!this.validator.allValid()) {
                                                    this.validator.showMessages();
                                                    this.forceUpdate();
                                                    return;
                                                }

                                                this.state.materials.push({
                                                    selected: this.props.$equipments.value,
                                                    material: getDefaultMaterial()
                                                });
                                                this.setState({}, () => {
                                                    attachMaterialToEquipments(this.state.materials, this.props.$equipments.value);


                                                    if (this.props.onChange) {
                                                        this.props.onChange();
                                                    }
                                                });
                                            }}
                                        ><Icon add/></Button>
                                        }
                                        {this.state.materials.length != (i + 1) &&
                                        <Button
                                            round
                                            className={'input-width-button__button' + (disabled ? " disabled" : "")}
                                            onClick={(e) => {

                                                if (disabled) {
                                                    return;
                                                }
                                                this.state.materials.splice(i, 1);

                                                this.setState({}, () => {
                                                    attachMaterialToEquipments(this.state.materials, this.props.$equipments.value);

                                                    if (this.props.onChange) {
                                                        this.props.onChange();
                                                    }
                                                });
                                            }}
                                        ><Icon remove/></Button>
                                        }
                                    </Grid>
                                    <Grid item xs={12}>
                                    </Grid>
                                </Grid>
                            </div>
                        )
                    })}
                </Wrapper>

            </div>
        )
    }
}


const fromEquipmentsPlanToMaterials = (equipments, grouped) => {
    var materials = {};
    if (!equipments) {
        return [];
    }
    equipments.map((equipment, index) => {
        if (!equipment.materials) {
            return;
        }

        equipment.materials.map((material, x) => {
            var key = material.descricao + material.quantidade + material.fornecidoPor;
            if (!materials[key]) {
                materials[key] = material;
                materials[key].equipments = [];
            }
            materials[key].equipments.push(equipment);
        });
    });

    var retval = [];

    Object.keys(materials).map((key, i) => {
        var material = materials[key];
        retval.push({selected: material.equipments, material});
    });

    return retval;
};

const getDefaultMaterial = () => {
    return {
        fornecidoPor: null,
        quantidade: null,
        descricao: null
    }
};

const attachMaterialToEquipments = (materials, equipments) => {
    if (!materials) {
        return;
    }

    var equipmentsMaterials = {};

    materials.map((material, i) => {
        if (!material.material.descricao) {
            return;
        }
        material.selected.map((equipment) => {
            if (!equipmentsMaterials[equipment.numEquipamento]) {
                equipmentsMaterials[equipment.numEquipamento] = [];
            }
            equipmentsMaterials[equipment.numEquipamento].push(material.material);
        });
    });

    equipments.map((equipment) => {
        equipment.$materials.value = equipmentsMaterials[equipment.numEquipamento];
    });

};

export default Material;