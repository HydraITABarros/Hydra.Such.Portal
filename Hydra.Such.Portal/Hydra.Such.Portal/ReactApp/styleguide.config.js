const path = require('path');

process.env.ASSETS_PATH = '/wwwroot';

const sourceDir = process.env.SOURCE || 'ReactApp'
const publicPath = `/${process.env.PUBLIC_PATH || ''}/`.replace('//', '/')
const sourcePath = path.join(process.cwd(), sourceDir)


module.exports = {
    components: ['ReactApp/components/atoms/**/index.js', 'ReactApp/components/molecules/**/index.js'],
    theme: {
        baseBackground: '#fdfdfc',
        link: '#274e75',
        linkHover: '#90a7bf',
        border: '#e0d2de',
        font: ['Helvetica', 'sans-serif'],
    },
    styles: {
        Playground: {
            preview: {
                paddingLeft: 0,
                paddingRight: 0,
                borderWidth: [
                    [0, 0, 1, 0]
                ],
                borderRadius: 0,
            },
        },
        Markdown: {
            pre: {
                border: 0,
                background: 'none',
            },
            code: {
                fontSize: 14,
            },
        },
    },
    getComponentPathLine(componentPath) {
        const name = path.basename(componentPath, '.js');
        return `import { ${name} } from 'my-awesome-library';`;
    },

    // Example of overriding the CLI message in local development.
    // Uncomment/edit the following `serverHost` entry to see in output
    // serverHost: 'your-domain',
    printServerInstructions(config) {
        // eslint-disable-next-line no-console
        console.log(`View your styleguide at: http://${config.serverHost}:${config.serverPort}`);
    },

    // Override Styleguidist components
    styleguideComponents: {
        LogoRenderer: path.join(__dirname, 'styleguide/components/Logo'),
        StyleGuideRenderer: path.join(__dirname, 'styleguide/components/StyleGuide'),
        SectionsRenderer: path.join(__dirname, 'styleguide/components/SectionsRenderer'),
    }
}