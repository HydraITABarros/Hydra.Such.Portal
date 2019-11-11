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

const Tabs = styled(MuiTabs)`
    [class*="MuiTabs-scroller"]>span{
      background-color: ${props => props.theme.palette.secondary.default};
      height: 5px;
      border-radius: 2.5px;
      z-index: 2;
    }
    [class*="MuiTabs-fixed"]>span{
      margin-left: 0px;
    }
    [class*="icon"] {
            color: ${props => props.theme.palette.primary.medium};
          }
    [aria-selected="true"]  {
          [class*="icon"] {
            color: ${props => props.theme.palette.secondary.default};
          }
    }
`;

const Tab = styled(MuiTab)`&&{
      text-transform: capitalize;
      text-align: left;
      min-width: 0;
    }
    [class*="MuiTab-labelContainer"] {
          padding: 6px 12px;
    }
`;
const Bar = styled(AppBar)`&&{
      background-color: ${props => props.theme.palette.white};
      box-shadow: none;
      margin-bottom: 0px;
      padding-left: 0;
      padding-right: 0;
      hr{position: relative; margin-top: -3px; margin-left: -40px; z-index: 1;}
    }
`;
var timeout = 0;

class SplitedReport extends Component {
    state = {
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
        this.state.refsHeader = props.refsHeader;
        this.state.refsMaintenance = props.refsMaintenance;
        this.state.refsQuality = props.refsQuality;
        this.state.refsQuantity = props.refsQuantity;
        this.state.refsComments = props.refsComments;
        this.state.refsFooter = props.refsFooter;
    }


    componentDidMount() {

    }

    componentWillReceiveProps(nextProps) {
        console.log('IMP', nextProps);
        var state = {};
        state.refsHeader = nextProps.refsHeader;
        state.refsMaintenance = nextProps.refsMaintenance;
        state.refsQuality = nextProps.refsQuality;
        state.refsQuantity = nextProps.refsQuantity;
        state.refsComments = nextProps.refsComments;
        state.refsFooter = nextProps.refsFooter;
        this.setState(state);
    }

    componentWillUnmount() {
    }

    render() {
        var date = this.props.date;

        var maintenanceResponsible = (this.props.order && this.props.order.maintenanceResponsibleObj && this.props.order.maintenanceResponsibleObj.nome);
        var responsibleEmployee = (this.props.order && this.props.order.responsibleEmployeeObj && this.props.order.responsibleEmployeeObj.nome);

        let report = [
            {header: [], maintenance: [], quality: [], quantity: [], comments: [], footer: []}
        ];
        let pageHeight = 1123 - (151 + 73 + 45);
        let tolerance = 20;
        let currentPage = 1;
        let currentHeight = 1;

        const pushToReport = (el, partial, i) => {
            if (!el) {
                return;
            }
            var elHeight = el.clientHeight;
            if (((currentHeight + elHeight + tolerance) > (pageHeight * currentPage))) {
                currentPage++;
                //currentHeight = 0;
                report.push({header: [], maintenance: [], quality: [], quantity: [], comments: [], footer: []});
            }
            currentHeight += elHeight;
            report[currentPage - 1][partial].push(
                <div key={i} dangerouslySetInnerHTML={{__html: ReactDOM.findDOMNode(el).cloneNode(true).innerHTML}}/>
            );
            console.log(partial, currentHeight);
        };

        this.state.refsHeader.map((el, i) => {
            pushToReport(el, 'header', i);
        });
        this.state.refsMaintenance.map((el, i) => {
            pushToReport(el, 'maintenance', i);
        });
        this.state.refsQuality.map((el, i) => {
            pushToReport(el, 'quality', i);
        });
        this.state.refsQuantity.map((el, i) => {
            pushToReport(el, 'quantity', i);
        });
        this.state.refsComments.map((el, i) => {
            pushToReport(el, 'comments', i);
        });
        this.state.refsFooter.map((el, i) => {
            pushToReport(el, 'footer', i);
        });

        console.log('REPORT 123', report);

        return (
            <div className="report__wrapper">
                <div className="report" id={"report"}>
                    {report.map((item, index) => {
                        return (
                            <React.Fragment key={index}>
                                <div className="report__page">
                                    <ReportTop
                                        equipmentType={this.props.equipmentType}
                                        mManager={maintenanceResponsible} mResp={responsibleEmployee}
                                        date={date}
                                        page={index + 1} total={report.length}
                                    />
                                    {item.header.length > 0 &&
                                    <React.Fragment>
                                        <div className="report__body">
                                            {item.header}
                                        </div>
                                        <div className="report__hr"></div>
                                    </React.Fragment>
                                    }
                                    {item.maintenance.length > 0 &&
                                    <React.Fragment>
                                        <div className="report__body">
                                            {item.maintenance}
                                        </div>
                                    </React.Fragment>
                                    }
                                    {item.quality.length > 0 &&
                                    <React.Fragment>
                                        <div className="report__body">
                                            {item.quality}
                                        </div>
                                    </React.Fragment>
                                    }
                                    {item.quantity.length > 0 &&
                                    <React.Fragment>
                                        <div className="report__body">
                                            {item.quantity}
                                        </div>
                                    </React.Fragment>
                                    }
                                    {item.comments.length > 0 &&
                                    <React.Fragment>
                                        <div className="report__body">
                                            {item.comments}
                                        </div>
                                    </React.Fragment>
                                    }
                                    {item.footer.length > 0 &&
                                    <React.Fragment>
                                        <div className="report__footer__wrapper">
                                            <div className="report__hr"></div>
                                            {item.footer}
                                        </div>
                                    </React.Fragment>
                                    }
                                </div>
                                {index + 1 < report.length &&
                                <div className="report__blank" data-html2canvas-ignore="true"></div>
                                }
                            </React.Fragment>
                        )
                    })}
                </div>
            </div>
        )
    }
}

const ReportTop = ({...props}) => {
    return (
        <div className="report__top">
            <div className="report__header">
                <div className="col-xs-5">
                    <img src="/images/such-engenharia.png"
                         alt="Such Engenharia"
                         className="img-responsive"/>
                </div>
                <div className="col-xs-7 padding-0">
                    {props.mManager && <div className="col-xs-6 report__header__avatar__wrapper">
                        Chefe de Projecto
                        <Avatars.Avatars
                            className="report__header__avatar"
                            letter
                            color={_theme.palette.primary.light} data-tip={"nome"}
                        >
                            {functions.getInitials(props.mManager || "")}
                        </Avatars.Avatars>
                    </div>}
                    {props.mResp && <div className="col-xs-6 report__header__avatar__wrapper">
                        Resp. Manutenção
                        <Avatars.Avatars
                            className="report__header__avatar"
                            letter
                            color={_theme.palette.primary.light} data-tip={"nome"}
                        >
                            {functions.getInitials(props.mResp || "")}
                        </Avatars.Avatars>
                    </div>}
                </div>
                <div className="clearfix"></div>
            </div>

            <div className="report__title">
                <Text p>Relatório de Manutenção {props.date && "- " + props.date}</Text>
                <Text p className="report__page-counter">{props.page}/{props.total}</Text>
                <Text h1 className="f-s-40"><Icon preventiva/>{props.equipmentType}
                </Text>
            </div>
        </div>
    )
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

export default SplitedReport;