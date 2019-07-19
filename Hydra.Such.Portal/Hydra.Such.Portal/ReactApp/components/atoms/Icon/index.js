import React from 'react';
import PropTypes from 'prop-types';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';

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
    font-size: ${props => props.size || props.decline ? "14px" : "24px"};

    /* Better Font Rendering =========== */
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
 

&.icon-no-data:before {
  content: "\\e900";
}
&.icon-voltar:before {
  content: "\\e901";
}
&.icon-observacoes:before {
  content: "\\e902";
}
&.icon-fotografias:before {
  content: "\\e903";
}
&.icon-upload:before {
  content: "\\e904";
}
&.icon-options:before {
  content: "\\e905";
}
&.icon-circle-on:before {
  content: "\\e906";
}
&.icon-circle-off:before {
  content: "\\e907";
}
&.icon-validation:before {
  content: "\\e908";
}
&.icon-remove:before {
  content: "\\e909";
}
&.icon-add:before {
  content: "\\e90a";
}
&.icon-arrow-left:before {
  content: "\\e90b";
}
&.icon-arrow-right:before {
  content: "\\e90c";
}
&.icon-arrow-up:before {
  content: "\\e90d";
}
&.icon-arrow-down:before {
  content: "\\e90e";
}
&.icon-decline:before {
  content: "\\e90f";
}
&.icon-comments:before {
  content: "\\e910";
}
&.icon-approved:before {
  content: "\\e911";
}
&.icon-row-menu:before {
  content: "\\e912";
}
&.icon-report-menu:before {
  content: "\\e913";
}
&.icon-observation:before {
  content: "\\e914";
}
&.icon-warning:before {
  content: "\\e915";
}
&.icon-print:before {
  content: "\\e916";
}
&.icon-no-wifi:before {
  content: "\\e917";
}
&.icon-attach:before {
  content: "\\e918";
}
&.icon-signature:before {
  content: "\\e919";
}
&.icon-lock:before {
  content: "\\e91a";
}
&.icon-open:before {
  content: "\\e91b";
}
&.icon-equipamentos:before {
  content: "\\e91c";
}
&.icon-share:before {
  content: "\\e91d";
}
&.icon-tecnico:before {
  content: "\\e91e";
}
&.icon-folder:before {
  content: "\\e91f";
}
&.icon-material:before {
  content: "\\e920";
}
&.icon-meter:before {
  content: "\\e921";
}
&.icon-tool:before {
  content: "\\e922";
}
&.icon-eye:before {
  content: "\\e923";
}
&.icon-report:before {
  content: "\\e924";
}
&.icon-download:before {
  content: "\\e925";
}
&.icon-archive:before {
  content: "\\e926";
}
&.icon-search:before {
  content: "\\e927";
}
&.icon-calendar:before {
  content: "\\e928";
}
&.icon-happy:before {
  content: "\\e929";
}
&.icon-sad:before {
  content: "\\e92a";
}
&.icon-curativa:before {
  content: "\\e92b";
}
&.icon-preventiva:before {
  content: "\\e92c";
}
`

// ['add','approved','archive','arrow-down','arrow-left','arrow-right','arrow-up','attach','calendar','comments','curativa','decline','download','equipamentos','eye','folder','happy','lock','material','meter','no-wifi','observation','open','preventiva','print','remove','report-menu','report','row-menu','sad','search','share','signature','tecnico','tool','validation','warning']

const Icon = ({ type, ...props }) => {
        return <IconFont {...props} className={"icon-" + Object.keys(props)[0] + " " + props.className} />
}

export default Icon;
