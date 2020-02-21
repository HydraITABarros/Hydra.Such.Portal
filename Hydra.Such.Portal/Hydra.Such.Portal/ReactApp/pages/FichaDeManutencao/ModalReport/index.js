import React, {Component} from 'react';
import {
    Button,
    Text,
    Icon,
    CheckBox,
    ModalLarge,
    Radio
} from 'components';
import {Grid} from '@material-ui/core';
import Report from './Report';
import Signature from './Signature';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import './index.scss';

const {DialogTitle, DialogContent, DialogActions} = ModalLarge;

var timeout = 0;

class ModalReport extends Component {
    state = {
        open: false,
        reportMode: true,
        selectedEquipments: [],
        isGrouped: false,
        reportList: []
    };

    constructor(props) {
        super(props);
        this.setReportGroup = this.setReportGroup.bind(this);
    }

    componentDidMount() {
        var selectedEquipments = [];
    }

    componentDidUpdate() {
    }

    componentWillUnmount() {
        this.state.selectedEquipments = [];
        this.state.reportList = [];
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
                    // if (i > 4) {
                    //     return;
                    // }
                    item.checked = true;
                    selectedEquipments.push(item);
                    this.setReportGroup(selectedEquipments);
                });


            }
        }
    }

    setReportGroup(selectedEquipments) {
        let reportList = [];
        if (this.state.isGrouped) {
            reportList = _.chunk(selectedEquipments, 5)
        } else {
            reportList = _.chunk(selectedEquipments, 1)
        }

        this.setState({
            reportList,
            selectedEquipments,

        });
    }

    makePDF(pageTotal, fileName) {
        //var quotes = document.getElementById('report');
        var quotes_list = $('.report-js');
        window.onePageCanvas = [];
        $('.report__blank').hide();
        quotes_list.map((index, quotes) => {
            console.log(quotes, index);
            quotes = quotes;
            // if (index == 0) {
            //     $(quotes).parents('.report__container')[0].scrollTop = 0;
            // } else {
            //     $(quotes).parents('.report__container')[0].scrollTop = $($('.report__wrapper__root')[index - 1]).height() + 50;
            // }

            pageTotal = $(quotes).find('.report__page').length - 1;
            html2canvas(quotes, {
                scale: 1,
                dpi: 350
            }).then((canvas) => {
                //! MAKE YOUR PDF
                var pdf = new jsPDF('p', 'pt', 'a4');

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

                    window.onePageCanvas[index] = document.createElement("canvas");
                    //resolution
                    onePageCanvas[index].setAttribute('width', 794);
                    onePageCanvas[index].setAttribute('height', 1123);
                    var ctx = onePageCanvas[index].getContext('2d');
                    // details on this usage of this function:
                    // https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API/Tutorial/Using_images#Slicing
                    ctx.drawImage(srcImg, sX, sY, sWidth, sHeight, dX, dY, dWidth, dHeight);

                    // document.body.appendChild(canvas);
                    var canvasDataURL = onePageCanvas[index].toDataURL("image/png", .5);

                    var width = onePageCanvas[index].width;
                    var height = onePageCanvas[index].clientHeight;

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
                pdf.output('dataurlnewwindow');
                $('.report__blank').show();
                //pdf.save(fileName + '.pdf' || 'File.pdf');
            });
        });
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
                onClose={() => {
                    this.setState({open: false, reportMode: true});
                    if (this.props.onClose) {
                        this.props.onClose();
                    }
                }}
                action={this.props.children} children={
                <div className="modal-report">
                    <DialogTitle>
                        {this.state.reportMode &&
                        <Text h2><Icon report/> Relatório de Manutenção</Text>
                        }
                        {!this.state.reportMode &&
                        <Text h2><Icon report/> Assinar</Text>
                        }

                    </DialogTitle>
                    <hr/>

                    <Grid container direction="row" justify="space-between" alignitems="bottom" spacing={0}
                          maxwidth={'100%'} margin={0}
                          className={"modal-container"}>
                        <Grid item xs={12} md={3} lg={3}
                              style={{borderBottom: '1px solid #E4E7EB', 'position': 'relative', marginBottom: '9px'}}>
                            <DialogContent>
                                <div className="col-xs-10 p-l-0 p-r-0">
                                    <Text b className="ws-nowrap to-ellipsis">{this.props.equipmentType}</Text>
                                </div>
                                <div className="col-xs-2 p-l-0 p-r-0">
                                    <Text
                                        b>{!this.props.isSimplified && this.props.$equipments && "(" + this.props.$equipments.value.length + ")"}</Text>
                                </div>
                                <div className="clearfix"></div>
                                {/* <div>debug selected count: {this.state.selectedEquipments.length}</div> */}
                                {this.state.reportMode ?

                                    (this.props.$equipments && !this.props.isSimplified && !this.props.isCurative) && this.props.$equipments.value.map((equipment, i) => {
                                        return (
                                            <div key={i} style={{lineHeight: '32px'}}>
                                                <div className="w-30 v-a-m">
                                                    <CheckBox id={"report-checkbox-" + i}
                                                              className="p-t-0 p-l-0 p-r-0 p-b-0"
                                                              checked={equipment.checked}
                                                              onChange={(event) => {

                                                                  if (event.target.checked) {
                                                                      var selectedEquipments = this.state.selectedEquipments.filter((selected) => {
                                                                          return selected.idEquipamento != equipment.idEquipamento;
                                                                      });
                                                                      equipment.checked = true;
                                                                      selectedEquipments.push(equipment);
                                                                  } else {
                                                                      var selectedEquipments = this.state.selectedEquipments.filter((selected) => {
                                                                          return selected.idEquipamento != equipment.idEquipamento;
                                                                      });
                                                                      equipment.checked = false;
                                                                  }

                                                                  this.setState({
                                                                      selectedEquipments: selectedEquipments,
                                                                      reportList: []
                                                                  }, () => {
                                                                      setTimeout(() => {
                                                                          this.setReportGroup(this.state.selectedEquipments);
                                                                      }, 40)
                                                                  })
                                                              }}/>
                                                </div>
                                                <div className="w-auto ws-nowrap to-ellipsis v-a-m">
                                                    <label htmlFor={"report-checkbox-" + i}>
                                                        <Text b className="w-20">#{i + 1}</Text> &nbsp;<Text
                                                        span>{equipment.numEquipamento}</Text>
                                                    </label>
                                                </div>


                                                <div className={"modal-report__group"}>
                                                    <CheckBox
                                                        onChange={(ev) => {
                                                            this.setState({
                                                                isGrouped: ev.target.checked,
                                                                reportList: []
                                                            }, () => {
                                                                setTimeout(() => {
                                                                    this.setReportGroup(this.state.selectedEquipments);
                                                                }, 4)
                                                            })
                                                        }}
                                                        checked={this.state.isGrouped}
                                                        name="radio-button-group"
                                                        id="radio-button-group"
                                                        label="Agrupar"
                                                        className={""}
                                                    />
                                                    <label htmlFor={"radio-button-group"}>
                                                        <Text span>Agrupar</Text>
                                                    </label>
                                                </div>
                                            </div>
                                        )
                                    })
                                    :
                                    this.props.$equipments && this.props.$equipments.value.map((equipment, i) => {
                                        return (
                                            <div key={i} style={{lineHeight: '32px'}}>

                                                <div className="w-auto ws-nowrap to-ellipsis v-a-m">
                                                    <Text b className="w-20">#{i + 1}</Text> &nbsp;<Text
                                                    span>{equipment.numEquipamento}</Text>
                                                </div>
                                            </div>
                                        )
                                    })
                                }
                            </DialogContent>
                        </Grid>
                        <Grid item xs={12} md={9} lg={9}
                              style={{
                                  lineHeight: 0,
                                  overflow: 'auto',
                                  borderRight: '1px solid #E4E7EB',
                                  borderBottom: '1px solid #E4E7EB',
                                  borderLeft: '1px solid #E4E7EB',
                                  textAlign: 'center'
                              }}
                              className={"report__container " + (this.state.selectedEquipments.length > 0 ? "" : "content-disabled")}>
                            <div>
                                {this.state.reportMode ?
                                    this.state.reportList.map((selectedEquipments, i) => {
                                        return (
                                            <React.Fragment
                                                key={i}>
                                                <Report
                                                    className={'report__container'}
                                                    selectedEquipments={selectedEquipments}
                                                    order={this.props.order}
                                                    equipmentType={this.props.equipmentType}
                                                    $equipments={this.props.$equipments}

                                                    isCurative={this.props.isCurative}
                                                    isSimplified={this.props.isSimplified}

                                                    onReportSplit={(totalPages) => {
                                                        this.state.totalPages = totalPages;
                                                    }}
                                                />
                                            </React.Fragment>
                                        )
                                    }) :
                                    <Signature $equipments={this.props.$equipments}
                                               $currentUser={this.props.$currentUser}
                                               onChange={() => {
                                                   if (this.props.onChange) {
                                                       this.props.onChange();
                                                   }
                                               }}
                                    />
                                }
                            </div>
                        </Grid>
                    </Grid>

                    <DialogActions>

                        {this.state.reportMode &&
                        <React.Fragment>
                            <div className={'flex-grow-1'}>
                                <Button icon={<Icon download/>}
                                        className={"box-shadow-none p-l-10 p-r-10 color-primary-dark"}
                                        onClick={() => {
                                            this.makePDF(this.state.totalPages);
                                        }}></Button>
                            </div>
                            <Button onClick={() => this.setState({reportMode: false})} icon={<Icon signature/>}
                                    color="primary">Assinar</Button>
                        </React.Fragment>
                        }
                        {!this.state.reportMode &&
                        <Button link onClick={() => this.setState({reportMode: true})}
                                className={"text-decoration-none p-r-25"} icon={<Icon voltar/>} link>Voltar</Button>
                        }
                        <Button
                            onClick={() => {
                                this.setState({open: false, reportMode: true});
                                if (this.props.onClose) {
                                    this.props.onClose();
                                }
                            }}
                            primary
                            color="primary" className={'m-l-15'}>Fechar</Button>
                    </DialogActions>
                </div>
            } open={this.state.open}/>
        )
    }
}

export default ModalReport;