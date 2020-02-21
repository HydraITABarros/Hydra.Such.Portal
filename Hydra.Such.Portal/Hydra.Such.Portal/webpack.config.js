const path = require('path');
const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const extractCSS = new ExtractTextPlugin('allstyles.css');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');
const WebpackNotifierPlugin = require("webpack-notifier");
const BrowserSyncPlugin = require("browser-sync-webpack-plugin");
const HtmlWebPackPlugin = require("html-webpack-plugin");
const HappyPack = require('happypack');
const CircularDependencyPlugin = require('circular-dependency-plugin')

const sourceDir = process.env.SOURCE || 'ReactApp'
const publicPath = `/${process.env.PUBLIC_PATH || ''}/`.replace('//', '/')
const sourcePath = path.join(process.cwd(), sourceDir)

process.env.BABEL_DISABLE_CACHE = false;
//process.env.NODE_ENV = 'production';

module.exports = {
    mode: process.env.NODE_ENV || 'development',
    entry: {
        'main': [sourcePath]
    },
    output: {
        path: path.resolve(__dirname, 'wwwroot/dist'),
        filename: 'bundle.js',
        publicPath: '/dist/'
    },
    plugins: [
        new CircularDependencyPlugin({
            // exclude detection of files based on a RegExp
            exclude: /a\.js|node_modules/,
            // add errors to webpack instead of warnings
            failOnError: true,
            // set the current working directory for displaying module paths
            cwd: process.cwd(),
        }),
        new webpack.ProgressPlugin(),
        new HappyPack({
            loaders: ['babel-loader']
        }),
        extractCSS,
        new WebpackNotifierPlugin(),
        new BrowserSyncPlugin(),
        new webpack.DefinePlugin({
            NODE_ENV: process.env.NODE_ENV,
            PUBLIC_PATH: publicPath.replace(/\/$/, ''),
            'process.env.ASSETS_PATH': JSON.stringify(process.env.ASSETS_PATH)
        })
    ],
    resolve: {
        extensions: ['.js', '.jsx', '.json'],
        modules: [].concat(sourceDir, ['node_modules']),
    },
    optimization: {
        minimize: false,
        minimizer: [

            new UglifyJsPlugin(
                // {
                //     cache: true,
                //     parallel: true,
                //     uglifyOptions: {
                //         compress: {
                //             sequences: true,
                //             dead_code: true,
                //             conditionals: true,
                //             booleans: true,
                //             unused: true,
                //             if_return: true,
                //             join_vars: true,
                //             warnings: false, // good for prod apps so users can't peek behind curtain
                //             drop_debugger: true,
                //             evaluate: true,
                //             drop_console: true, // strips console statements
                //             comparisons: true,
                //             loops: true,
                //             hoist_funs: true,
                //             negate_iife: true
                //         },
                //         ecma: 6,
                //         mangle: true,
                //         output: {
                //             comments: false
                //         }
                //     },
                //     sourceMap: false
                // }
            )
        ]
    },
    module: {
        rules: [{
            test: /\.(css|scss)$/,
            use: ["style-loader", // creates style nodes from JS strings
                "css-loader", // translates CSS into CommonJS
                "sass-loader" // compiles Sass to CSS, using Node Sass by default
            ]
        },
            {
                test: /\.(jpg|jpeg|png|gif|mp3|svg)$/,
                loaders: ['file-loader']
            },
            //{ test: /\.css$/, use: extractCSS.extract(['css-loader?minimize']) },
            //{ test: /\.js?$/, use: { loader: 'babel-loader', options: { presets: ['@babel/preset-react', '@babel/preset-env'] } } },
            {
                test: /\.js$/,
                exclude: /(node_modules)/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['@babel/preset-react', '@babel/preset-env'],
                        plugins: ["@babel/plugin-proposal-class-properties"]
                    }
                }
            }
        ]
    },
    externals: {
        // "node/npm module name": "name of exported library variable"
        //"react": "React",
        //"react-dom": "ReactDOM"
    },
    devtool: "inline-source-map"
};