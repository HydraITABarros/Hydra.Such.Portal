import { createStore } from "redux";
// import { persistStore, persistReducer } from 'redux-persist';
//import storage from 'redux-persist/lib/storage';

const initialState = {
        isLoading: null,
        client: {
                "id": null,
                "name": null
        },
        calendar: {
                "from": null,
                "to": null,
                "olderFrom": null,
                "olderTo": null
        },
        ordersCounts: {
                preventive: null,
                preventiveToExecute: null,
                curative: null,
                curativeToExecute: null
        },
        tooltipReady: false,
        maintenenceOrders: [],
        maintenenceOrdersFiltered: [],
        maintenenceOrdersIsLoading: null,
        maintenenceOrdersSearchValue: "",
        maintenenceOrdersNext: "",
        technicals: [],
        technicalsFiltered: [],
        technicalsIsLoading: null,
        technicalsSearchValue: "",
        technicalsOpen: false,
        technicalsSelectedOrder: { technicals: [] },
        technicalsSelectedOrderOld: { technicals: [] },
        selectedOrder: "",
        datePickerOpen: false,
        datePickerMarginTop: 0,
        listContainerStyle: {},
        avatarColors: [
                "#990000",
                "#33DDEE",
                "#5533DD",
                "#339900",
                "#cc00cc"
        ]
}

function rootReducer(state = initialState, action) {
        if (action.type === 'SET_STATE') {
                return { ...state, ...action.payload };
        }
        return state;
};
const store = createStore(rootReducer);

export default store;