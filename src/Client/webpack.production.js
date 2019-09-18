const MinifyPlugin = require("terser-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

const merge = require("webpack-merge");
const commonConfiguration = require("./webpack.common");

module.exports = merge(commonConfiguration, {
    mode: "production",
    optimization: {
        minimizer: [ new MinifyPlugin() ]
    },
    plugins: [
        new MiniCssExtractPlugin({ filename: 'style.css' })
    ],
    module: {
        rules: [
            {
                test: /\.scss$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    'css-loader',
                    'sass-loader'
                ]
            }
        ]
    }
});
