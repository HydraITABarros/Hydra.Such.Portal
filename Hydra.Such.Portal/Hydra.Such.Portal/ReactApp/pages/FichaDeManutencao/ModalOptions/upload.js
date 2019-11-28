import React, {Component} from 'react';
import {Input, Button, Icon, Text, Wrapper, Select, MenuItem, GSelect, Radio} from 'components';
import MuiAddIcon from '@material-ui/icons/Add';
import styled, {css, theme, injectGlobal, withTheme} from 'styled-components';
import MuiGrid from '@material-ui/core/Grid';
import axios from 'axios';


const Grid = styled(MuiGrid)`
    position: relative;
`
const TextUpload = styled(Text)`
    margin-left: 16px;
`


class Upload extends React.Component {

    state = {
        selected: 0,
        error: false,
        errorMessage: "",
        loading: false,
        success: [],
        documentType: 'image'
    };

    constructor(props) {
        super(props);
        this.uploadFileHandler = this.uploadFileHandler.bind(this);
    };

    uploadFileHandler(e) {
        var file = e.target.files[0];
        var size = file.size;
        var type = file.type;

        this.setState({
            errorMessage: '',
            error: false,
            loading: true
        }, () => {
            if (type != "application/pdf" && type != "image/jpeg") {
                this.setState({
                    errorMessage: 'Formato inválido.',
                    error: true,
                    loading: false
                });
                return;
            }

            if (size > 10485760) {
                this.setState({
                    errorMessage: 'Deve seleccionar um ficheiro inferior a 10MB',
                    error: true,
                    loading: false
                });
                return;
            }

            var formData = new FormData();

            formData.append("file", file);

            axios.post('/ordens-de-manutencao/files/' + this.props.orderId + '/' + this.state.selected.id + "?documentType=" + this.state.documentType, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            }).then((e) => {

                this.state.success.push(file.name);

                this.setState({
                    loading: false
                });

            }).catch((e) => {

            });

        });
    }

    render() {
        return (
            <div>

                <Wrapper padding={'0 0 16px'}>
                    <Grid container spacing={1}>
                        <Grid item xs={10} md={10}>
                            <Radio
                                onChange={() => this.setState({documentType: 'image'})}
                                checked={this.state.documentType === 'image'}
                                name="radio-button-demo"
                                label="Fotografia"
                            />
                            <Radio
                                onChange={() => this.setState({documentType: 'document'})}
                                checked={this.state.documentType === 'document'}
                                name="radio-button-demo"
                                label="Documento"
                            />
                        </Grid>
                        <Grid item xs={10} md={5}>
                            <div className="p-b-5"></div>
                            <Select
                                value={this.state.selected}
                                error={this.state.error}
                                onChange={(e) => {
                                    this.setState({
                                        selected: e.target.value,
                                        success: [],
                                        error: false,
                                        errorMessage: ''
                                    });
                                }}
                            >
                                <MenuItem
                                    value={0}
                                    style={{display: 'none'}}>
                                    <span>Associar a Equipamento</span>
                                </MenuItem>
                                {this.props.$equipments && this.props.$equipments.value.map((o, j) => {
                                    return <MenuItem
                                        key={j}
                                        value={o}>{"#" + (j + 1) + " " + o.numEquipamento}</MenuItem>
                                })}
                            </Select>
                        </Grid>
                        <Grid item xs={10}>
                            <div className="p-b-20"></div>
                            {this.state.documentType == "image" &&
                            <input type="file" name="file" accept={".jpg, .jpeg"} className={'hide'}
                                   ref={(el) => this.uploadRef = el} onChange={this.uploadFileHandler}
                            />
                            }

                            {this.state.documentType == "document" &&
                            <input type="file" name="file" accept={".pdf"} className={'hide'}
                                   ref={(el) => this.uploadRef = el} onChange={this.uploadFileHandler}
                            />}
                            <div className={this.state.selected != 0 ? "" : "content-disabled"}>
                                <Button iconPrimary={<Icon upload/>}
                                        onClick={() => {
                                            this.uploadRef.click();
                                        }}>Upload</Button><TextUpload>Até 10MB</TextUpload>
                            </div>
                            <Text p className={"color-alert-bad p-t-15 p-b-10"}> <i>{this.state.errorMessage}</i></Text>
                            {this.state.success && this.state.success.map((fileName, i) => {
                                return <Text p className={""} key={i}><i>{fileName}</i></Text>;
                            })}
                        </Grid>
                    </Grid>
                </Wrapper>

            </div>
        )
    }
}

export default Upload;