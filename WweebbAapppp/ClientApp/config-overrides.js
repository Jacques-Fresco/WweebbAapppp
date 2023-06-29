const webpack = require('webpack');

module.exports = function override(config, env) {
    // Добавляем полифилы для Node.js модулей
    config.resolve.fallback = {
        ...config.resolve.fallback,
        buffer: require.resolve('buffer/'),
        crypto: require.resolve('crypto-browserify'),
        stream: require.resolve('stream-browserify'),
        util: require.resolve('util/'),
    };

    // Добавляем плагин webpack ProvidePlugin для полифилов
    config.plugins.push(
        new webpack.ProvidePlugin({
            process: 'process/browser',
            Buffer: ['buffer', 'Buffer'],
        })
    );

    return config;
};