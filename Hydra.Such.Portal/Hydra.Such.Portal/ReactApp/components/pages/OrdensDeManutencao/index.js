// https://github.com/diegohaz/arc/wiki/Atomic-Design
import React, { Component } from 'react';
import axios from 'axios';
import {
    PageTemplate, Organism, Button, Loading
} from 'components';
import CircularProgress from '@material-ui/core/CircularProgress';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';


const fetchTechnicals = ({ orderId, technicalId, local }, cb) => {
    cb = cb || function () { };
    axios.get('/ordens-de-manutencao/technicals', {
        params: { orderId, technicalId, local }
    })
        .then(function (data) {
            cb(null, data);
        }).catch(function (error) {
            cb(error);
        });
}

const updateTechnicals = ({ orderId, technicalsId }, cb) => {
    cb = cb || function () { };
    axios.put('/ordens-de-manutencao/technicals', { orderId: orderId, technicalsId: technicalsId })
        .then(function (data) {
            cb(null, data);
        }).catch(function (error) {
            cb(error);
        });
}

//updateTechnicals({ orderId: "OM1901562", technicalsId: ["42664", "105590","106624"] });


class OrdensDeManutencao extends Component {

    state = {
        isLoading: true,
        client: {
            "Id": null,
            "Name": null
        },
        calender: {
            "from": null,
            "to": null
        },
        ordersCounts: {
            preventive: null,
            preventiveToExecute:null,
            curative:null,
            curativeToExecute:null,
        },
        maintenenceOrders:[]
    }

    fetchTechnicals({ orderId, technicalId, local }, cb) {
        axios.get('/ordens-de-manutencao/technicals', {
            params: { orderId, technicalId, local }
        })
            .then(function (data) {
                cb(null, data);
            }).catch(function (error) {
                cb(error);
            });
    }

    fetchMaintenenceOrders({ from, to }, cb) {
        axios.get('/ordens-de-manutencao/all', {
            params: { from, to }
        }).then((result) => {
            var data = result.data;

            if (data.ordersCounts && data.result && data.result.items) {
                var list = data.result.items;

                list = list.map(item => {
                    item.technicals = [];

                    console.log(item);
                    this.fetchTechnicals({ orderId: item.no }, (err, result) => {
                        item.technicals = result.technicals;
                        //this.setState({ maintenenceOrders: list });
                    });
                    return item;
                });

                this.setState({ ordersCounts: data.ordersCounts, maintenenceOrders: list });

            }
        }).catch(function (error) {
        }).then(() => {
            this.setState({ isLoading: false });
        }); 
    }   



    constructor(props) {
        super(props);
        this.fetchMaintenenceOrders({ from:"2019-01-01", to: "2019-01-02" });

        //fetchTechnicals({ orderId: "OM1209462" }, function (err, result) {
        //    console.log('fetchTechnicals', result.data)
        //});
        //fetchTechnicals({ technicalId: "1314" }, function (err, result) {
        //    console.log('fetchTechnicals', result.data)
        //});
        //fetchTechnicals({ local: "33" }, function (err, result) {
        //    console.log('fetchTechnicals', result.data)
        //});
        //UpdateTechnicals({ orderId: "OM1904477", technicalsId: ["107303", "107446", "106269", "104728"] }, function (err, result) {
        //    console.log('UpdateTechnicals', result.data)
        //});

    }

    render() {
        const { isLoading, ordersCounts, maintenenceOrders } = this.state;
        var teste = maintenenceOrders.length;

        return (
            <PageTemplate
                header={<div>Header</div>}
                hero={<div></div>}
                footer={<div>Footer</div>} >

                {isLoading ? <CircularProgress /> :
                    
                    <div>
                        <p>curative: {ordersCounts.curative}</p>
                        <p>curativeToExecute: {ordersCounts.curativeToExecute}</p>
                        <p>preventive: {ordersCounts.preventive}</p>
                        <p>preventiveToExecute: {ordersCounts.preventiveToExecute}</p>

                        <List>
                            {maintenenceOrders.map((item, index) => {
                                return(
                                    <ListItem key={index}>
                                        {item.customerCity}
                                        {item.nome}
                                        {item.technicals && item.technicals[0] ? item.technicals[0].nome : '' }
                                       
                                    </ListItem>
                                )
                            })}
                        </List>
                    </div>

                }
                

            </PageTemplate>
        )
    }
}

export default OrdensDeManutencao
