import config from './../../common/config';
import core from './../../common/core';
let objUrlsToPurge = {
    setBusterToUrls: (paramP, arrUrls, sCookieName, objBuster) => {
        let param = paramP;
        let buster = '';
        let sSourceUrl = param.url;
        let cookie = core.getCookie(sCookieName);
        if (cookie) {
            if (param.method.toLowerCase() === 'get') {
                for (let j = 0; j < arrUrls.length; j++) {
                    let row = arrUrls[j];
                    if (sSourceUrl.indexOf(row) > -1) {
                        buster = cookie;
                        let busterT = objBuster[row];
                        if (busterT && buster && !isNaN(busterT) && !isNaN(buster)) {
                            if (Number(busterT) > Number(buster)) {
                                buster = busterT;
                            }
                        }
                        objBuster[row] = buster;
                    }
                }
            }
        }

        return {
            objBuster: objBuster,
            buster: buster
        };//param;
    }
};
objUrlsToPurge[config.enumBusters.team] = ['/team', '/composition_1'];

export default objUrlsToPurge;
