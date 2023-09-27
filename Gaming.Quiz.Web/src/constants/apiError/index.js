const data = {
    favTeam: {

    },
    saveTeam: {
        10001: 'STAGE_LOCKED',
    },
    loginPost: {

    },

};
const defaultError = 'somethingWentWrong';
/*const validator = {
    get(object, property) {
        if (typeof object[property] === 'object' && object[property] !== null) {
            return new Proxy(object[property], validator)
        } else {
            return object.hasOwnProperty(property) ? object[property] : defaultError;
        }
    }
};*/
const apiError = {
    getVal: (sKey, retValP) => {
        let retVal = data[sKey] ? data[sKey][retValP] : null;
        if (!retVal) {
            retVal = defaultError
        }
        return retVal;
    }
};// new Proxy(data, validator);

export default apiError;