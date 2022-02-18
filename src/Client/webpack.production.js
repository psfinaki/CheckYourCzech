const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const {CleanWebpackPlugin} = require('clean-webpack-plugin');

const { merge } = require("webpack-merge");
const commonConfiguration = require("./webpack.common");

module.exports = merge(commonConfiguration, {
    mode: "production",
    optimization: {
        minimizer: [
            (compiler) => {
              const TerserPlugin = require('terser-webpack-plugin');
              new TerserPlugin({
                terserOptions: {
                  compress: {},
                }
              }).apply(compiler);
            },
          ]
    },
    output: {
        path: path.join(__dirname, "public"),
        filename: "[name].[contenthash].js"
    },
    plugins: [
        new CleanWebpackPlugin(),
        new MiniCssExtractPlugin({ filename: 'style.[contenthash].css' })
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
