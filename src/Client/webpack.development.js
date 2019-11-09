const path = require("path");
const webpack = require("webpack");

const merge = require("webpack-merge");
const commonConfiguration = require("./webpack.common");

module.exports = merge(commonConfiguration, {
    mode: "development",
    output: {
        path: path.join(__dirname, "public"),
        filename: "[name].js"
    },
    devtool: "source-map",
    plugins:     [
        new webpack.HotModuleReplacementPlugin(),
        new webpack.NamedModulesPlugin()
    ],
    devServer: {
        proxy: {
            '/api/*': {
                target: 'https://localhost:8085',
                secure: false
            }
        },
        hot: true,
        contentBase: path.join(__dirname, "public")
    },
    module: {
        rules: [
            {
                test: /\.scss$/,
                use: [
                    'style-loader',
                    'css-loader',
                    'sass-loader'
                ]
            }
        ]
    }
});
