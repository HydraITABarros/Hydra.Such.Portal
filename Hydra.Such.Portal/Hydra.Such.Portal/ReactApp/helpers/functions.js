import _ from 'lodash';

const Functions = {
    toTitleCase: (str) => {
        if (!str) {
            return str;
        }
        return str.replace(
            /\w\S*/g,
            function (txt) {
                return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
            }
        );
    },

    getInitials(name) {
        var initials = name.match(/\b\w/g) || [];
        initials = ((initials.shift() || '') + (initials.pop() || '')).toUpperCase();
        return initials;
    },

    linkedPropTimeout: 0,
    makeLinkedProp: function (stateObj, path, ctx) {
        var retval = {};
        retval["_value"] = stateObj[path];
        Object.defineProperty(retval, 'value', {
            get: function () {
                return retval['_value'];
            },
            set: function (val) {
                val = val || '';
                retval['_value'] = val;
                //clearTimeout(Functions.linkedPropTimeout);
                Functions.linkedPropTimeout = setTimeout(() => {
                    stateObj[path] = val;
                    ctx.setState(ctx.state);
                });
            }
        });
        return retval;
    },
    addLinkedPropsToObject: (obj, ctx) => {
        if (_.isArray(obj)) {
            for (let index = 0; index < obj.length; index++) {
                const element = obj[index];
                Functions.addLinkedPropsToObject(element, ctx);
                //obj['$' + index] = Functions.makeLinkedProp(obj, index, ctx);
            }
        } else if (_.isObject(obj)) {
            var keys = Object.keys(obj);
            for (let index = 0; index < keys.length; index++) {
                const key = keys[index];
                const value = obj[key];
                obj['$' + key] = Functions.makeLinkedProp(obj, key, ctx);
                if (_.isObject(value) || _.isArray(value)) {
                    Functions.addLinkedPropsToObject(value, ctx);
                }
            }
        }
        return obj;
    }
}

export default Functions;