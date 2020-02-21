import React from 'react'
import {Switch, Route} from 'react-router-dom'
import {injectGlobal, ThemeProvider} from 'styled-components'
import {
    HomePage,
    SamplePage,
    NotFoundPage,
    Template,
    OrdensDeManutencao,
    OrdensDeManutencaoDetails,
    FichaDeManutencao,
    FichaDeManutencaoCurative,
    OrdensDeManutencaoArchive
} from 'pages'

// https://github.com/diegohaz/arc/wiki/Styling
import theme from './themes/default'
import {StylesProvider, MuiThemeProvider, createMuiTheme, createGenerateClassName,} from '@material-ui/core/styles';
import {JssProvider} from 'react-jss';
import {red} from '@material-ui/core/colors';
import Color from 'color';
import {Provider} from 'react-redux';
import store from './store';
import "index.scss";

const generateClassName = createGenerateClassName({
    dangerouslyUseGlobalCSS: true, // won't minify CSS classnames when true
});

window.__MUI_USE_NEXT_TYPOGRAPHY_VARIANTS__ = true;
const muiTheme = createMuiTheme({
    typography: {
        useNextVariants: true,
    },
    palette: {
        action: {
            selected: Color(theme.palette.primary.keylines).rgb().fade(0.1).toString(),
            hover: Color(theme.palette.primary.keylines).rgb().fade(0.5).toString(),
            active: Color(theme.palette.primary.light).rgb().fade(0.5).toString(),
            disabled: Color(theme.palette.primary.keylines).rgb().fade(0.8).toString(),
        }
    }
});

injectGlobal`
  body {
    margin: 0;
  }
`;

const App = () => {
    return (
        <Provider store={store}>
            <ThemeProvider theme={theme} generateClassName={generateClassName}>
                <MuiThemeProvider theme={muiTheme}>
                    <StylesProvider generateClassName={generateClassName}>
                        <Switch>
                            {/* <Route path="/" component={HomePage} exact /> */}
                            {/* <Route component={Template} /> */}
                            <Route path="/ordens-de-manutencao" component={OrdensDeManutencao} exact/>
                            <Route path="/ordens-de-manutencao/arquivo" component={OrdensDeManutencaoArchive} exact/>
                            <Route path="/ordens-de-manutencao/:orderid" component={OrdensDeManutencaoDetails} exact/>
                            <Route path="/ordens-de-manutencao/:orderid/ficha-de-manutencao"
                                   component={FichaDeManutencao}
                                   exact/>
                            <Route path="/ordens-de-manutencao/:orderid/curativa" component={FichaDeManutencaoCurative}
                                   exact/>
                            {/* <Route component={NotFoundPage} /> */}
                        </Switch>
                    </StylesProvider>
                </MuiThemeProvider>
            </ThemeProvider>
        </Provider>
    )
}

export default App
