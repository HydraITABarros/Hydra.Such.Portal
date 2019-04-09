import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import * as serviceWorker from './serviceWorker';

const pages = {
    'one': () => import('./pages/one'),
    'ordemManutencao': () => import('./pages/ordemManutencao'),
}

const renderAppInElement = (el) => {
    console.log(el.id);

    if (pages[el.id]) {
        pages[el.id]().then((App) => {
            console.log(App);
            ReactDOM.render(<App.default {...el.dataset} />, el);
        });
    }
}

document
    .querySelectorAll('.react-root')
    .forEach(renderAppInElement)

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
