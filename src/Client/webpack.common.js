const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");

module.exports = {
    entry: {
        "app": [
			"applicationinsights-js",
            "whatwg-fetch",
            "@babel/polyfill",
            path.join(__dirname, "./Client.fsproj")
        ],
        "style": [ path.join(__dirname, './scss/main.scss') ]
    },
    resolve: {
        symlinks: false
    },
    optimization: {
        splitChunks: {
            cacheGroups: {
                commons: {
                    test: /node_modules/,
                    name: "vendors",
                    chunks: "all"
                }
            }
        }
    },
    plugins: [
        new HtmlWebpackPlugin({ template: "./src/Client/template.html" }),
    ],
    module: {
        rules: [
            {
                test: /\.fs(x|proj)?$/,
                use: "fable-loader"
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: [
                            ["@babel/preset-env", {
                                "targets": {
                                    "browsers": ["last 2 versions"]
                                },
                                "modules": false,
                                "useBuiltIns": "usage"
                            }]
                        ],
                        plugins: [ "@babel/plugin-transform-runtime" ]
                    }
                }
            }
        ]
    }
};
