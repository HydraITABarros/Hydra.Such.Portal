// https://github.com/diegohaz/arc/wiki/Example-app
import 'react-hot-loader/patch'
import React from 'react'
import {render} from 'react-dom'
import {BrowserRouter} from 'react-router-dom'

import {basename} from 'config'
import App from 'App'

import {useStrict} from 'mobx';
import {Provider} from 'mobx-react';

// For easier debugging
// window._____APP_STATE_____ = stores;

//useStrict(true);

const renderApp = () => (
    <BrowserRouter basename={basename}>
        <App/>
    </BrowserRouter>
);

const root = document.getElementById('basicreactcomponent')
render(renderApp(), root);

if (module.hot) {
    module.hot.accept('App', () => {
        require('App');
        render(renderApp(), root)
    })
}
