// https://github.com/diegohaz/arc/wiki/Atomic-Design#do-not-worry
const req = require.context('.', true, /\.\/[^/]+\/index\.js$/)

req.keys().forEach((key) => {
    const componentName = key.replace(/^.+\/([^/]+)\/index\.js/, '$1');

    if (req(key).default) {
        module.exports[componentName] = req(key).default;
    } else {
        module.exports[componentName] = req(key);
    }
})
