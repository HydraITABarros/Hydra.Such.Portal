import React from 'react';
import PropTypes from 'prop-types';
import styled, { css, theme, injectGlobal } from 'styled-components';
import _theme from '../../themes/default';

const assetsPath = process.env.ASSETS_PATH || '';

injectGlobal`
    @font-face {
        font-family: 'eSuch';
        src:  url( '${assetsPath}/fonts/eSuch.eot?x1kme9');
        src:  url('${assetsPath}/fonts/eSuch.eot?x1kme9#iefix') format('embedded-opentype'),
            url('${assetsPath}/fonts/eSuch.ttf?x1kme9') format('truetype'),
            url('${assetsPath}/fonts/eSuch.woff?x1kme9') format('woff'),
            url('${assetsPath}/fonts/eSuch.svg?x1kme9#eSuch') format('svg');
        font-weight: normal;
        font-style: normal;
}
`
const IconFont = styled.span`

    /* use !important to prevent issues with browser extensions that change fonts */
    font-family: 'eSuch' !important;
    speak: none;
    font-style: normal;
    font-weight: normal;
    font-variant: normal;
    text-transform: none;
    line-height: 1;

    /* Better Font Rendering =========== */
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
 

    &.icon-add:before {
    content: "\\e900";
    }
    &.icon-approved:before {
    content: "\\e901";
    }
    &.icon-archive:before {
    content: "\\e902";
    }
    &.icon-arrow-down:before {
    content: "\\e903";
    }
    &.icon-arrow-left:before {
    content: "\\e904";
    }
    &.icon-arrow-right:before {
    content: "\\e905";
    }
    &.icon-arrow-up:before {
    content: "\\e906";
    }
    &.icon-attach:before {
    content: "\\e907";
    }
    &.icon-calendar:before {
    content: "\\e908";
    }
    &.icon-comments:before {
    content: "\\e909";
    }
    &.icon-curativa:before {
    content: "\\e90a";
    }
    &.icon-decline:before {
    content: "\\e90b";
    }
    &.icon-download:before {
    content: "\\e90c";
    }
    &.icon-equipamentos:before {
    content: "\\e90d";
    }
    &.icon-eye:before {
    content: "\\e90e";
    }
    &.icon-folder:before {
    content: "\\e90f";
    }
    &.icon-happy:before {
    content: "\\e910";
    }
    &.icon-lock:before {
    content: "\\e911";
    }
    &.icon-material:before {
    content: "\\e912";
    }
    &.icon-meter:before {
    content: "\\e913";
    }
    &.icon-no-wifi:before {
    content: "\\e914";
    }
    &.icon-observation:before {
    content: "\\e915";
    }
    &.icon-open:before {
    content: "\\e916";
    }
    &.icon-preventiva:before {
    content: "\\e917";
    }
    &.icon-print:before {
    content: "\\e918";
    }
    &.icon-remove:before {
    content: "\\e919";
    }
    &.icon-report-menu:before {
    content: "\\e91a";
    }
    &.icon-report:before {
    content: "\\e91b";
    }
    &.icon-row-menu:before {
    content: "\\e91c";
    }
    &.icon-sad:before {
    content: "\\e91d";
    }
    &.icon-search:before {
    content: "\\e91e";
    }
    &.icon-share:before {
    content: "\\e91f";
    }
    &.icon-signature:before {
    content: "\\e920";
    }
    &.icon-tecnico:before {
    content: "\\e921";
    }
    &.icon-tool:before {
    content: "\\e922";
    }
    &.icon-validation:before {
    content: "\\e923";
    }
    &.icon-warning:before {
    content: "\\e925";
    }
`

// ['add','approved','archive','arrow-down','arrow-left','arrow-right','arrow-up','attach','calendar','comments','curativa','decline','download','equipamentos','eye','folder','happy','lock','material','meter','no-wifi','observation','open','preventiva','print','remove','report-menu','report','row-menu','sad','search','share','signature','tecnico','tool','validation','warning']

const Icon = ({ type, ...props }) => {
    return <IconFont className={"icon-" + Object.keys(props)[0]} {...props} />
}

export default Icon;
