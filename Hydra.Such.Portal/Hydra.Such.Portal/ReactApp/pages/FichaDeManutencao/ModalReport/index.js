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
import functions from '../../../helpers/functions';
import _theme from '../../../themes/default';
import ReactDOM from 'react-dom';
import Report from './Report';
import './index.scss';

const {DialogTitle, DialogContent, DialogActions} = ModalLarge;

var timeout = 0;
var refAux = 0;

class ModalReport extends Component {
    state = {
        open: false,
        selectedEquipments: [],
        report: [],
        refsHeader: [],
        refsMaintenance: [],
        refsQuality: [],
        refsQuantity: [],
        refsComments: [],
        refsFooter: []
    }

    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.resetRefs = this.resetRefs.bind(this);
    }

    resetRefs() {
        this.state.refsHeader = [];
        this.state.refsMaintenance = [];
        this.state.refsQuality = [];
        this.state.refsQuantity = [];
        this.state.refsComments = [];
        this.state.refsFooter = [];
    }

    handleChange(e, value) {
        this.setState({});
    }

    componentDidMount() {
        this.resetRefs();
        var selectedEquipments = [];
    }

    componentDidUpdate() {
        refAux = 0;
        this.resetRefs();
    }

    buildReportWidthPagination() {

    }

    componentWillUnmount() {
        this.state.selectedEquipments = [];
        this.resetRefs();
    }


    componentWillReceiveProps(nextProps) {
        if (nextProps.open !== this.state.open) {
            this.setState({open: nextProps.open});
            setTimeout(() => {
                this.setState({});
            }, 10)
        }
        if (nextProps.$equipments && this.props.$equipments && nextProps.$equipments.value !== this.props.$equipments) {
            var selectedEquipments = [];
            if (this.props.$equipments) {
                this.props.$equipments.value.map((item, i) => {
                    if (i > 4) {
                        return;
                    }
                    item.checked = true;
                    selectedEquipments.push(item);
                })
                this.setState({
                    selectedEquipments
                });
            }
        }
        console.log('selectedEquipments', this.state.selectedEquipments)
    }

    render() {

        refAux = 0;

        var order = this.props.order || {};
        var maintenance, quality, quantity;
        if (this.props.$equipments && this.props.$equipments.value[0]) {
            maintenance = this.props.$equipments.value[0].planMaintenance;
            quality = this.props.$equipments.value[0].planQuality;
            quantity = this.props.$equipments.value[0].planQuantity;
        }

        var date = (this.state.selectedEquipments[this.state.selectedEquipments.length - 1] &&
            this.state.selectedEquipments[this.state.selectedEquipments.length - 1].dataRelatorio) &&
            this.state.selectedEquipments[this.state.selectedEquipments.length - 1].dataRelatorio;

        var maintenanceResponsible = (this.props.order && this.props.order.maintenanceResponsibleObj && this.props.order.maintenanceResponsibleObj.nome);
        var responsibleEmployee = (this.props.order && this.props.order.responsibleEmployeeObj && this.props.order.responsibleEmployeeObj.nome);
        return (
            <ModalLarge
                onOpen={() => {
                    if (this.props.children.props.disabled) {
                        return this.setState({open: false});
                    }
                    this.setState({open: true});
                }}
                onClose={() => {
                    this.setState({open: false});
                }}
                action={this.props.children} children={
                <div className="modal-report">
                    <DialogTitle>
                        <Text h2><Icon report/> Relatório de Manutenção</Text>
                    </DialogTitle>
                    <hr/>

                    <Grid container direction="row" justify="space-between" alignitems="top" spacing={0}
                          maxwidth={'100%'} margin={0}>
                        <Grid item xs={12} md={4} lg={3} style={{borderBottom: '1px solid #E4E7EB'}}>
                            <DialogContent>
                                <div className="col-xs-10 p-l-0 p-r-0">
                                    <Text b className="ws-nowrap to-ellipsis">{this.props.equipmentType}</Text>
                                </div>
                                <div className="col-xs-2 p-l-0 p-r-0">
                                    <Text b>({this.props.$equipments && this.props.$equipments.value.length})</Text>
                                </div>
                                <div className="clearfix"></div>
                                {/* <div>debug selected count: {this.state.selectedEquipments.length}</div> */}
                                {this.props.$equipments && this.props.$equipments.value.map((equipment, i) => {
                                    return (
                                        <div key={i} style={{lineHeight: '32px'}}>
                                            <div className="w-30 v-a-m">
                                                <CheckBox id={"report-checkbox-" + i}
                                                          className="p-t-0 p-l-0 p-r-0 p-b-0"
                                                          checked={equipment.checked}
                                                          onChange={(event) => {

                                                              this.resetRefs();

                                                              setTimeout(() => {
                                                                  this.setState({});
                                                              }, 10)

                                                              if (event.target.checked) {
                                                                  this.state.selectedEquipments = this.state.selectedEquipments.filter((selected) => {
                                                                      return selected.idEquipamento != e.idEquipamento;
                                                                  });
                                                                  equipment.checked = true;
                                                                  this.state.selectedEquipments.push(equipment);
                                                                  this.setState({selectedEquipments: this.state.selectedEquipments});
                                                              } else {
                                                                  this.state.selectedEquipments = this.state.selectedEquipments.filter((selected) => {
                                                                      return selected.idEquipamento != equipment.idEquipamento;
                                                                  });
                                                                  equipment.checked = false;
                                                                  this.setState({selectedEquipments: this.state.selectedEquipments || []});
                                                              }
                                                          }}/>
                                            </div>
                                            <div className="w-auto ws-nowrap to-ellipsis v-a-m">
                                                <label htmlFor={"report-checkbox-" + i}>
                                                    <Text b className="w-20">#{i + 1}</Text> &nbsp;<Text
                                                    span>{equipment.numEquipamento}</Text>
                                                </label>
                                            </div>
                                        </div>
                                    )
                                })}
                            </DialogContent>
                        </Grid>
                        <Grid item xs={12} md={8} lg={9}
                              style={{
                                  lineHeight: 0,
                                  overflow: 'auto',
                                  borderRight: '1px solid #E4E7EB',
                                  borderBottom: '1px solid #E4E7EB',
                                  borderLeft: '1px solid #E4E7EB',
                                  textAlign: 'center'
                              }}
                              className={"report__container " + (this.state.selectedEquipments.length > 0 ? "" : "content-disabled")}>

                            <Report
                                selectedEquipments={this.state.selectedEquipments}
                                order={this.props.order}
                                equipmentType={this.props.equipmentType}
                                $equipments={this.props.$equipments}
                            />

                        </Grid>
                    </Grid>

                    <hr/>
                    <DialogActions>
                        <Button onClick={() => this.setState({open: false})} icon={<Icon signature/>}
                                color="primary">Assinar</Button>
                        <Button onClick={() => this.setState({open: false})} primary color="primary">Finalizar</Button>
                    </DialogActions>
                </div>
            } open={this.state.open}/>
        )
    }
}

window.makePDF = function (pageTotal, fileName) {
    var quotes = document.getElementById('report');
    pageTotal = pageTotal - 1;
    html2canvas(quotes, {
        scale: 1,
        dpi: 300
    }).then((canvas) => {
        //! MAKE YOUR PDF
        var pdf = new jsPDF('p', 'pt', 'a4');

        console.log(quotes.clientHeight - 15);

        for (var i = 0; i <= pageTotal; i++) {
            //! This is all just html2canvas stuff
            var srcImg = canvas;
            var sX = 0;
            var sY = 1123 * i; // start 980 pixels down for every new page
            var sWidth = 794;
            var sHeight = 1123;
            var dX = 0;
            var dY = 0;
            var dWidth = 794;
            var dHeight = 1123;

            window.onePageCanvas = document.createElement("canvas");
            //resolution
            onePageCanvas.setAttribute('width', 794);
            onePageCanvas.setAttribute('height', 1123);
            var ctx = onePageCanvas.getContext('2d');
            // details on this usage of this function:
            // https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API/Tutorial/Using_images#Slicing
            ctx.drawImage(srcImg, sX, sY, sWidth, sHeight, dX, dY, dWidth, dHeight);

            // document.body.appendChild(canvas);
            var canvasDataURL = onePageCanvas.toDataURL("image/png", .5);

            var width = onePageCanvas.width;
            var height = onePageCanvas.clientHeight;

            //! If we're on anything other than the first page,
            // add another page
            if (i > 0) {
                //pdf.addPage(612, 791); //8.5" x 11" in pts (in*72)
                pdf.addPage(595, 842); //8.5" x 11" in pts (in*72)
                //pdf.addPage(794, 1123); //8.5" x 11" in pts (in*72)
                //pdf.addPage(1240, 1754); //8.5" x 11" in pts (in*72)
            }
            //! now we declare that we're working on that page
            pdf.setPage(i + 1);
            //! now we add content to that page!
            pdf.addImage(canvasDataURL, 'PNG', 0, 0, (width * .75), (height * .75), null, 'SLOW');

        }
        //! after the for loop is finished running, we save the pdf.
        pdf.save(fileName + '.pdf' || 'File.pdf');
    });
}

export default ModalReport;