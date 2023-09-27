import config from './../config';
import axios from 'axios';
import objUrlsToPurge from './../../constants/busters';

const core = {};
window.apiCalls = 0;
core.isFilledObj = (arg) => {
    if (!arg) {
        return false;
    }
    return arg !== null && typeof arg === 'object' && !!Object.keys(arg).length;
};
core.isBetween = (value, min, max) => {
    return value >= min && value <= max;
};
core.NO_OP = function () {
};
core.daysBetween = function (date1, date2) {
    //Get 1 day in milliseconds
    let one_day = 1000 * 60 * 60 * 24;

    // Convert both dates to milliseconds
    let date1_ms = date1.getTime();
    let date2_ms = date2.getTime();

    // Calculate the difference in milliseconds
    let difference_ms = date2_ms - date1_ms;
    let ms_Diff = difference_ms;
    //take out milliseconds
    difference_ms = difference_ms / 1000;
    let seconds = Math.floor(difference_ms % 60);
    difference_ms = difference_ms / 60;
    let minutes = Math.floor(difference_ms % 60);
    difference_ms = difference_ms / 60;
    let hours = Math.floor(difference_ms % 24);
    let days = Math.floor(difference_ms / 24);
    //hours = hours + (days * 24);
    let jTotal = days + hours + minutes + seconds;
    let jTotalHours = /*(days * 24) +*/ hours;
    let hoursText = "hr" + (jTotalHours > 1 ? "s" : "");
    let minutesText = "min" + (minutes > 1 ? "s" : "");
    let daysText = "day" + (days > 1 ? "s" : "");
    // if (jTotal < 0) {
    //     return 0 + ' h, ' + 0 + ' m';
    // }
    // if (days > 0) {
    //     jTotalHours = days;
    //     minutes = hours;
    //     hoursText = "days";
    //     minutesText = "hrs";
    // }
    // else {
    //     let sDate = dateFormat(date2, 'dd mmm yyyy');
    //     return sDate;
    // }
    if (minutes < 0) {
        minutes = 0;
    }
    return {
        ms_Diff: ms_Diff,
        jTotal: jTotal,
        days: days,
        hours: jTotalHours,
        minutes: minutes,
        daysText: daysText,
        hoursText: hoursText,
        minutesText: minutesText,
        seconds: seconds
    };
};
core.getIndexOfArrayByValue = function (arr, sKey, value) {
    for (let j = 0; j < arr.length; j++) {
        if (arr[j][sKey] === value) {
            return j;
        }
    }
    return -1;
};
core.shuffle = (array) => {
    let currentIndex = array.length, temporaryValue, randomIndex;

    // While there remain elements to shuffle...
    while (0 !== currentIndex) {

        // Pick a remaining element...
        randomIndex = Math.floor(Math.random() * currentIndex);
        currentIndex -= 1;

        // And swap it with the current element.
        temporaryValue = array[currentIndex];
        array[currentIndex] = array[randomIndex];
        array[randomIndex] = temporaryValue;
    }

    return array;
};
core.deepCopy = function (target, source) {
    let i, j;
    if (arguments.length > 1) {
        for (i = 1; i < arguments.length; i++) {
            for (j in arguments[i]) {
                if (arguments[i].hasOwnProperty(j)) {
                    if (typeof arguments[i][j] === "object") {
                        if (Array.isArray(arguments[i][j]))
                            target[j] = core.deepCopy(target[j] ? target[j] : [], arguments[i][j]);
                        else
                            target[j] = core.deepCopy(target[j] ? target[j] : {}, arguments[i][j]);
                    }
                    else {
                        target[j] = arguments[i][j];
                    }
                }
            }
        }
    }
    return target;
};
core.setCookie = (c_name, value, exDays = null) => {
    let exDate = new Date();
    exDate.setDate(exDate.getDate() + exDays);
    let c_value = escape(value) + ((exDays === null) ? ";" : "; expires=" + exDate.toUTCString() + ";domain=" + config.sDomain + ";path=/");
    document.cookie = c_name + "=" + c_value;
};
core.getCookie = (c_name) => {
    let i, x, y, cookiesArray = document.cookie.split(";");
    for (i = 0; i < cookiesArray.length; i++) {
        x = cookiesArray[i].substr(0, cookiesArray[i].indexOf("="));
        y = cookiesArray[i].substr(cookiesArray[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x === c_name) {
            return window.decodeURIComponent(y);
        }
    }
};

core.deleteCookie = (name) => {
    core.setCookie(name, '', -1);
};

core.setSessionValue = (name, value) => {
    if (typeof name !== 'undefined' && typeof value !== 'undefined') sessionStorage.setItem(name, value);
};

core.getSessionValue = (name, value) => {
    if (typeof name !== 'undefined') return sessionStorage.getItem(name);
};

core.removeSessionValue = (name, value) => {
    if (typeof name !== 'undefined') sessionStorage.removeItem(name);
};

core.setLocalStorageValue = (name, value) => {
    if (typeof name !== 'undefined' && typeof value !== 'undefined') localStorage.setItem(name, value);
};

core.getLocalStorageValue = (name, value) => {
    if (typeof name !== 'undefined') return localStorage.getItem(name);
};

core.removeLocalStorageValue = (name, value) => {
    if (typeof name !== 'undefined') localStorage.removeItem(name);
};

core.updateFromParent = function (callback) {
    callback();
}
core.fValidateEmail = (sEmail) => {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(sEmail);
}
core.getMobileOperatingSystem = function () {
    let userAgent = navigator.userAgent || navigator.vendor || window.opera;
    // Windows Phone must come first because its UA also contains "Android"
    if (/windows phone/i.test(userAgent)) {
        return "windows";
    }

    if (/android/i.test(userAgent)) {
        return "android";
    }

    // iOS detection from: http://stackoverflow.com/a/9039885/177710
    if (/iPad|iPhone|iPod/.test(userAgent) && !window.MSStream) {
        return "ios";
    }
    return "unknown";
};

core.CompCacheBusterCookie = function (cookiename) {
    let curdate = new Date();
    let jYear = curdate.getUTCFullYear();
    let jMonth = curdate.getUTCMonth();
    jMonth = jMonth + 1;
    let jDate = curdate.getUTCDate();
    let jHours = curdate.getUTCHours();
    let jMInutes = curdate.getUTCMinutes();
    let jSeconds = curdate.getUTCSeconds();

    let sNowUTC = jYear + '' + (jMonth < 10 ? ('0' + jMonth) : jMonth) + '' + (jDate < 10 ? ('0' + jDate) : jDate) + "" +
        (jHours < 10 ? ('0' + jHours) : jHours) + '' + (jMInutes < 10 ? ('0' + jMInutes) : jMInutes) +
        '' + (jSeconds < 10 ? ('0' + jSeconds) : jSeconds);
    // let curtime = curdate.getTime();
    let curtime = curdate.getTime();
    curdate.setTime((curtime + 31540000000)/*(5 * 60 * 1000)*/);//added one year expiry.
    let expires = "expires=" + curdate.toUTCString() + ";path=/";
    document.cookie = cookiename + "=" + sNowUTC + "; " + expires + ";";
};

core.splitIntoTwoWord = function (name) {
    if (name) {
        let splName = name.split(' ');
        let secondWord = '';
        let firstWord = '';
        for (let j in splName) {
            if (j == 0) {
                firstWord = splName[j];
            }
            else {
                secondWord = secondWord + " " + splName[j].trim();
            }

        }
        return {
            firstWord: firstWord,
            secondWord: secondWord
        }
    }
    else {
        return {
            firstWord: '',
            secondWord: ''
        }
    }
}

core.noFindImg = function (event) {
    var img = event.srcElement
    img.src = config.IMG_BASE + "dummy-plyr-img.png";
    img.onerror = null;
}

core.logger = function () {
    var oldConsoleLog = null;
    var pub = {};

    pub.enableLogger = function enableLogger() {
        if (oldConsoleLog == null)
            return;

        window['console']['warn'] = oldConsoleLog;
    };

    pub.disableLogger = function disableLogger() {
        oldConsoleLog = console.warn;
        window['console']['warn'] = function () { };
    };

    return pub;
}();
//core.logger.disableLogger();
core.makeAPICall = (param) => {
    if (!param.url) return false;
    const sNodeSource = '.si-pg-loader-mainWrap';
    const sNodeTarget = 'si-hidden';
    const testLoaderVisibility = () => {
        --window.apiCalls;
        if (window.apiCalls <= 0) {
            //$('#loader').css('display', 'none');

            const isRemove = false;
            core.addClassToElement(document, sNodeSource, sNodeTarget, isRemove);
            window.apiCalls = 0;
        }
    };
    param.credentials = 'include';
    param.method = param.method || 'GET';
    param.data = (param.data || {});
    param.headers = config.headers;
    ++window.apiCalls;
    if (param.loader !== false) {
        const isRemove = true;
        core.addClassToElement(document, sNodeSource, sNodeTarget, isRemove);
    }
    //Start Busters
    let buster = '';
    let objBusterP = {};
    let objBuster = {};
    for (let sKey in objUrlsToPurge) {
        const arrUrls = objUrlsToPurge[sKey];
        const sCookieName = sKey;
        if (typeof arrUrls !== 'function') {
            objBusterP = objUrlsToPurge.setBusterToUrls(param, arrUrls, sCookieName, objBuster);
            objBuster = objBusterP.objBuster;
            buster = objBusterP.buster;
            if (buster) {
                param.params = ({ ...param.params, 'buster': buster });
            }
        }
    }

    //End Busters

    let axiosPromise = axios(param);
    axiosPromise.then(() => {
        testLoaderVisibility();
    }).catch(() => {
        testLoaderVisibility();
    });
    return axiosPromise;
};

core.isMobile = () => {
    return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
};
core.isTablet = () => {
    return /Tablet|Trident/i.test(navigator.userAgent);
};

core.isNullArray = function (arr) {
    if (!arr) {
        return true;
    }
    return arr.join().replace(/,/g, '').length === 0;
};

core.addClassToElement = (objDocumentP, sNodeSource, sNodeTarget, isRemove) => {
    const objDocument = objDocumentP ? objDocumentP : document;
    let $bodyEle = objDocument && objDocument.querySelector ? objDocument.querySelector(sNodeSource) : null;
    if ($bodyEle && $bodyEle.classList) {
        if (!($bodyEle.classList.contains(sNodeTarget))) {
            $bodyEle.classList.add(sNodeTarget);
        }
        if (isRemove) {
            $bodyEle.classList.remove(sNodeTarget);
        }
    }
}
core.scrollToElement = (objDocument, sNodeSource) => {
    let element = objDocument && objDocument.querySelector ? objDocument && objDocument.querySelector(sNodeSource) : null;
    if (element && element.scrollIntoView) {
        // element.scrollIntoView();
        // element.scrollIntoView(false);
        // element.scrollIntoView({ block: "end" });
        element.scrollIntoView({ block: "start", behavior: "smooth" });

    }
}
core.scrollToPosition = (objDocumentP, sNodeSourceP) => {
    const objDocument = objDocumentP ? objDocumentP : document;
    let sNodeSource = sNodeSourceP ? sNodeSourceP : '.si-mod';
    let $ele = document.querySelector(sNodeSource);
    core.scrollToElement(objDocument, sNodeSource);
}
core.getPosition = (el) => {
    var xPos = 0;
    var yPos = 0;

    while (el) {
        if (el.tagName == "BODY") {
            // deal with browser quirks with body/window/document and page scroll
            var xScroll = el.scrollLeft || document.documentElement.scrollLeft;
            var yScroll = el.scrollTop || document.documentElement.scrollTop;

            xPos += (el.offsetLeft - xScroll + el.clientLeft);
            yPos += (el.offsetTop - yScroll + el.clientTop);
        } else {
            // for all other non-BODY elements
            xPos += (el.offsetLeft - el.scrollLeft + el.clientLeft);
            yPos += (el.offsetTop - el.scrollTop + el.clientTop);
        }

        el = el.offsetParent;
    }
    return {
        x: xPos,
        y: yPos
    };
}
core.dateFormat = function () {
    let d = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[ ^']*'/g,
        f = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
        c = /[^-+\dA-Z]/g, b = function (a, g) {
            for (a = String(a), g = g || 2; a.length < g;) {
                a = "0" + a
            }
            return a
        };
    return function (x, j, a, trans) {
        let A = core.dateFormat;
        if (1 !== arguments.length || "[object String]" !== Object.prototype.toString.call(x) || /\d/.test(x) || (j = x, x = void 0), x = x ? new Date(x) : new Date, isNaN(x)) {
            throw SyntaxError("invalid date")
        }
        j = String(A.masks[j] || j || A.masks["default"])//, "UTC:" === j.slice(0, 4) && (j = j.slice(4), a = !0);
        let C = a ? "getUTC" : "get", q = x[C + "Date"](), e = x[C + "Day"](), B = x[C + "Month"](),
            w = x[C + "FullYear"](), k = x[C + "Hours"](), m = x[C + "Minutes"](), t = x[C + "Seconds"](),
            z = x[C + "Milliseconds"](), v = a ? 0 : x.getTimezoneOffset(), p = {
                d: q,
                dd: b(q),
                ddd: A.i18n.dayNames[e],
                dddd: trans && trans[A.i18n.dayNames[e + 7]] ? trans[A.i18n.dayNames[e + 7]] : A.i18n.dayNames[e + 7],
                m: B + 1,
                mm: b(B + 1),
                mmm: A.i18n.monthNames[B],
                mmmm: trans && trans[A.i18n.monthNames[B + 12]] ? trans[A.i18n.monthNames[B + 12]] : A.i18n.monthNames[B + 12],
                yy: String(w).slice(2),
                yyyy: w,
                h: k % 12 || 12,
                hh: b(k % 12 || 12),
                H: k,
                HH: b(k),
                M: m,
                MM: b(m),
                s: t,
                ss: b(t),
                l: b(z, 3),
                L: b(z > 99 ? Math.round(z / 10) : z),
                t: 12 > k ? "a" : "p",
                tt: 12 > k ? "am" : "pm",
                T: 12 > k ? "A" : "P",
                TT: 12 > k ? "AM" : "PM",
                Z: a ? "UTC" : (String(x).match(f) || [""]).pop().replace(c, ""),
                o: (v > 0 ? "-" : "+") + b(100 * Math.floor(Math.abs(v) / 60) + Math.abs(v) % 60, 4),
                S: ["th", "st", "nd", "rd"][q % 10 > 3 ? 0 : (q % 100 - q % 10 !== 10) * q % 10]
            };
        return j.replace(d, function (g) {
            return g in p ? p[g] : g.slice(1, g.length - 1)
        })
    }
}();
core.dateFormat.masks = {
    default: "ddd mmm dd yyyy HH:MM:ss",
    shortDate: "m/d/yy",
    mediumDate: "mmm d, yyyy",
    longDate: "mmmm d, yyyy",
    fullDate: "dddd, mmmm d, yyyy",
    shortTime: "h:MM TT",
    mediumTime: "h:MM:ss TT",
    longTime: "h:MM:ss TT Z",
    isoDate: "yyyy-mm-dd",
    isoTime: "HH:MM:ss",
    isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
    isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
};
core.dateFormat.i18n = {
    dayNames: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
        'sunday',//(window.TRANS && window.TRANS.sunday ? window.TRANS.sunday : 'Sunday'),
        'monday',//(window.TRANS && window.TRANS.monday ? window.TRANS.monday : 'Monday'),
        'tuesday',//(window.TRANS && window.TRANS.tuesday ? window.TRANS.tuesday : 'Tuesday'),
        'wednesday',//(window.TRANS && window.TRANS.wednesday ? window.TRANS.wednesday : 'Wednesday'),
        'thursday',//(window.TRANS && window.TRANS.thursday ? window.TRANS.thursday : 'Thursday'),
        'friday',//(window.TRANS && window.TRANS.friday ? window.TRANS.friday : 'Friday'),
        'saturday'],//(window.TRANS && window.TRANS.saturday ? window.TRANS.saturday : 'Saturday')],
    monthNames: ["jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec", "january", "february", "march", "april", "may", 'june', 'july', "august", "september", "october", "november", "december"]
};
Date.prototype.format = function (a, b) {
    return core.dateFormat(this, a, b)
};
core.CompConvertHextoString = function (shareLoad) {
    let hex = shareLoad.toString();//force conversion
    let str = '';
    for (let i = 0; i < hex.length; i += 2)
        str += String.fromCharCode(parseInt(hex.substr(i, 2), 16));

    let leagueArr = ['Username', 'leagueid', 'league_name', 'league_code'];
    let new_tempArr = str.split('##');
    let temp = '';
    for (let j = 0; j < new_tempArr.length; j++) {
        temp += leagueArr[j] + '#=#' + new_tempArr[j];
        if (j < new_tempArr.length - 1)
            temp += '&=&';
    }
    let cookieobj = {};
    temp = temp.split('&=&');
    for (let i = 0, l = temp.length; i < l; i++) {
        let x = temp[i];
        let temp1 = x.split('#=#');
        let key = temp1[0];
        let val = temp1[1].replace(/\+/g, ' ');
        //val = decodeURIComponent(val);
        cookieobj[key] = val;
    }
    return cookieobj;
}
core.setLoader = (isIncreament) => {
    // if (isIncreament) {
    //     ++window.apiCalls;
    // }
    // else {
    //     --window.apiCalls;
    //     if (window.apiCalls <= 0) {
    //         window.apiCalls = 0;
    //     }
    // }
}
window.apiCalls = 0;
export default core;