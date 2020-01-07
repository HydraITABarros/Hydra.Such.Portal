import _ from 'lodash';
import axios from "axios";
import moment from "moment";
import async from 'async';
import {Offline, Online} from "react-detect-offline";

let timeout = 0;
let firstSync = false;
const StorageService = {
    url: `/ordens-de-manutencao/ficha-de-manutencao`,
    syncQueue: [],
    isSyncing: false,
    syncDate: moment(),
    syncError: null,
    init: () => {
        StorageService.syncQueue = StorageService.getSyncQueueFromLocalStorage() || [];

        window.teste = StorageService;
        console.log(StorageService);

        StorageService.syncData();
        window.addEventListener('online', () => {
            StorageService.syncData();
        });
        if (firstSync && StorageService.onFirstSync) {
            StorageService.onFirstSync();
        }
        return StorageService;
    },
    getSyncQueueFromLocalStorage() {
        let UnparsedLocalData = localStorage.getItem('fichaDeManutencaoLocalStorage');
        try {
            return JSON.parse(UnparsedLocalData);
        } catch (e) {
            return [];
        }
    },
    setDataToLocalStorage() {
        localStorage.setItem('fichaDeManutencaoLocalStorage', JSON.stringify(StorageService.syncQueue));
    },
    getData: () => {
        return StorageService.syncQueue;
    },
    saveData: (newData) => {
        let _newData = JSON.parse(JSON.stringify(StorageService.cleanData(newData)));
        let item = StorageService.findQueue(_newData.om, _newData.idEquipamento);
        if (item) {
            item = _.assign(item, _newData);
        } else {
            StorageService.syncQueue.push(_newData);
        }
        StorageService.setDataToLocalStorage();
        clearTimeout(timeout);
        timeout = setTimeout(() => {
            StorageService.syncData();
        }, 3200);
        return StorageService;
    },
    findQueue: (om, idEquipamento) => {
        let item = StorageService.syncQueue.filter((item) => {
            return item.om == om && item.idEquipamento == idEquipamento;
        });
        return item[0];
    },
    isSynchronized: () => {
        return StorageService.syncQueue == null || StorageService.syncQueue.length == 0;
    },
    hassyncError: () => {
        return !!StorageService.getsyncError();
    },
    getsyncError: () => {
        return StorageService.syncError;
    },
    syncData: () => {
        if (StorageService.isSynchronized()) {
            if (firstSync == false && StorageService.onFirstSync) {
                StorageService.onFirstSync();
            }
            if (StorageService.onSync) {
                StorageService.onSync();
            }
            firstSync = true;
            return;
        }
        if (!navigator.onLine) {
            return;
        }
        StorageService.synching = true;
        async.forEachOf(StorageService.syncQueue, function (item, index, callback) {
            StorageService
                .postPlans(item)
                .then((result) => {
                    StorageService.syncQueue[index] = null;
                    console.log('IMPORTANT', index);
                    callback();
                })
                .catch((err) => {
                    callback(err);
                });
        }, function (err) {
            if (err) {
                StorageService.syncError = err;
                console.log('error', StorageService.syncQueue);
                if (StorageService.onSyncError) {
                    StorageService.onSyncError(error);
                }
            } else {
                StorageService.syncDate = moment();
                StorageService.syncError = null;
            }
            if (StorageService.onSync) {
                StorageService.onSync();
            }
            if (firstSync == false && StorageService.onFirstSync) {
                StorageService.onFirstSync();
            }
            firstSync = true;
            StorageService.syncQueue = StorageService.syncQueue.filter((item) => {
                return item != null;
            });
            console.log('IMPORTANT', StorageService.syncQueue);
            StorageService.setDataToLocalStorage();
        });
    },
    postPlans: (Data) => {
        //let plan = _.cloneDeep(Data);
        let plan = Data;
        let orderId = Data.om;
        let toPost = [];
        var retval = StorageService.cleanData(plan);
        toPost.push(retval);
        return axios.post(StorageService.url + "?orderId=" + orderId, toPost);
    },
    cleanData: (plan) => {
        var retval = {};
        Object.keys(plan).map(k => {
            if (k.indexOf('$') > -1) {
                return;
            }
            retval[k] = plan[k];
        });

        if (plan.emms) {
            retval.emms = plan.emms.map((emm) => {
                var _item = _.omit(emm, ['equipments']);
                return _item;
            });
        }

        if (plan.materials) {
            retval.materials = plan.materials.map((material) => {
                var _item = _.omit(material, ['equipments']);
                return _item;
            });
        }

        return retval;
    }
};

export default StorageService;