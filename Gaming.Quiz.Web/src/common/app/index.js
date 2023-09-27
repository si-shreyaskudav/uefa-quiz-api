import config from './../config';
import core from './../core';
const app = {};
app.isSetWebview = function () {
    let cookieWEBVIEW = (window.source === (config.WEBVIEW_KEY));
    if (cookieWEBVIEW) {
        config.IS_WEBVIEW = true;
    }
    else {
        config.IS_WEBVIEW = false;
    }

    return config.IS_WEBVIEW;
}
app.resetBracket = (arrContraintsDefault) => {
    let arrContraints = [];
    for (let i = 0; i < arrContraintsDefault.length; i++) {
        let objStage = arrContraintsDefault[i];
        const { name, subStage } = objStage.stage;
        let arrSubstage = [];
        for (let j = 0; j < (subStage || []).length; j++) {
            let objSubStage = subStage[j];
            const { team, teamSelection } = objSubStage;
            let arrTeamSelection = [];
            let arrTeam = [];
            for (let jN = 0; jN < (team || []).length; jN++) {
                let objTeam = team[jN];
                arrTeam.push({ ...objTeam });
            }
            if (name === config.enumStage.teamSelection) {
                for (let jN = 0; jN < (teamSelection || []).length; jN++) {
                    let objTeam = teamSelection[jN];
                    arrTeamSelection.push({ ...objTeam });
                }
            }
            let pushSbstage = { ...objSubStage, team: arrTeam };
            if (arrTeamSelection.length) {
                pushSbstage.teamSelection = arrTeamSelection;
            }
            arrSubstage.push(pushSbstage);
        }
        arrSubstage = arrSubstage.sort((a, b) => {
            let valueA = (a.name);
            let valueB = (b.name);

            if (valueA > valueB) {
                return 1;
            }
            if (valueA < valueB) {
                return -1;
            }
            return 0;
        });
        arrContraints.push({ ...objStage, stage: { ...objStage.stage, name: name, subStage: arrSubstage } });
    }
    return arrContraints;
}
app.getFixtureStatusByRow = (element) => {
    let sMatchStatus = config.enumFixtureType.isUpcomming;
    if (element.isLocked == 0 && (element.isCurrent == 0)) {
        sMatchStatus = config.enumFixtureType.isUpcomming;
    }
    else if (element.isLocked == 0 && (element.isCurrent == 1)) {
        sMatchStatus = config.enumFixtureType.isOpen;
    }
    else if (element.isLocked == 1 && element.isCurrent == 1) {
        sMatchStatus = config.enumFixtureType.isLock;//'fixture-box match-closed';
    }
    else if (element.isCurrent == 2 && element.isLocked == 1) {
        //Match finished
        sMatchStatus = config.enumFixtureType.isFinished;
    }
    // else if (element.IsLive && (element.IsLive == 1 || element.IsLive == 2)) {
    //     //Live
    //     sMatchStatus = config.enumFixtureType.isLive;
    // }

    // console.log("isLocked = ",element.isLocked," ,isCurrent = ",element.isCurrent," ,IsLive = ",element.IsLive);
    return sMatchStatus;
}
//======================Tracking======================
app.googleAnalyticsTracking = function (sTitle, sPath) {
    if (window.gtag) {
        window.gtag('config', config.GA_TRACKING_ID, {
            'page_title': sTitle,
            'page_path': sPath
        });

    }
}
app.googleAnalyticsEventTracking = function (action, category, opt_label) {
    if (window.gtag) {
        window.gtag('event', action, {
            'event_category': category,
            'event_label': opt_label
        });
    }
}
//======================Tracking======================

app.getLoginCookie = function () {
    let cookie = core.getCookie(config.COOKIE_FANTASY);
    let returnCookie = null;
    try {
        if (cookie) {
            cookie = window.decodeURIComponent(cookie);
            let parsedCookie = JSON.parse(cookie);
            if (parsedCookie) {
                returnCookie = parsedCookie;
            }
        }
        return returnCookie;
    }
    catch (error) {
        if (cookie) {
            cookie = window.decodeURIComponent(cookie);
            let cookieSpecial = cookie.replace(/[\x00-\x09\x0B-\x0C\x0E-\x1F\x7F-\x9F]/g, '');
            let parsedCookie = JSON.parse(cookieSpecial);
            if (parsedCookie) {
                returnCookie = parsedCookie;
            }
        }

        return returnCookie;
    }

}
app.getLoginCookieStatus = () => {
    let parentCookie = core.getCookie(config.COOKIE_PARENT);
    let fantasyCookie = core.getCookie(config.COOKIE_FANTASY);
    let ret = config.enumLoginCookie.bothNotPresent;
    if (!parentCookie && !fantasyCookie) {
        ret = config.enumLoginCookie.bothNotPresent;
    }
    else if (parentCookie && fantasyCookie) {
        ret = config.enumLoginCookie.bothPresent;
    }
    else if (parentCookie && !fantasyCookie) {
        // only Fifa cookie present.
        ret = config.enumLoginCookie.parentPresent;
    }
    else if (!parentCookie && fantasyCookie) {
        // only Fantasy cookie present.
        ret = config.enumLoginCookie.fantasyPresent;
    }

    return ret;
};

app.eventValidators = (el, cookie) => {
    let resp = true;
    return resp;
};

app.eventValidatorsLoggedInWithBracket = (el, cookie, arrPlayedGamedays) => {
    let resp = false;
    if (cookie && !Number(cookie.isAnonymous) && arrPlayedGamedays && arrPlayedGamedays.length) {
        // if (arrPlayedGamedays && arrPlayedGamedays.length && arrPlayedGamedays.indexOf(config.enumGameStage.knockoutStage) > -1) {
        resp = true;
    }
    return resp;
};

app.eventValidatorsLocked = (el, cookie, isGameStatus) => {
    let resp = true;
    if (!(cookie && Number(cookie.HasTeam)) && isGameStatus !== config.enumGameStatus.open) {
        resp = false;
    }
    return resp;
};
app.eventValidatorsLoggedIn = (el, cookie, isGameStatus) => {
    let resp = false;
    if (/*(cookie && cookie.IsRegistered) &&*/ isGameStatus === config.enumGameStatus.finished) {
        resp = true;
    }
    return resp;
};

app.deleteAllCookies = () => {
    var cookies = document.cookie.split(";");
    let exDate = new Date();
    exDate.setDate(exDate.getDate() + -1);
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        var eqPos = cookie.indexOf("=");
        var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;

        document.cookie = name + "=;expires=" + exDate.toUTCString() + ";domain=" + config.sDomain + ";path=/";

        document.cookie = name + "=;expires=" + exDate.toUTCString() + ";path=/";

    }
    //
    //core.removeLocalStorageValue(config.enumCookie.inviteCookie);
    window.location.reload();
}

app.creteEmptyTeamFormation = (objFormation, trans) => {

    let arrSelectedPlayer = [];
    const jDfCount = objFormation ? objFormation.DEF : 0;
    const jMfCount = objFormation ? objFormation.MID : 0;
    const jFwCount = objFormation ? objFormation.FWD : 0;
    const jGkCount = 1;
    let arrSelectedPlayerMain = {};
    //Forward Row 
    for (let j = 0; j < jFwCount; j++) {
        let row = {
            id: (j + 1), name: (trans && trans.forwards ? trans.forwards : 'Forwards'), filterIndex: config.emptyValue, skill: config.enumPosition.forwards, jorder: (j + 1)
            , skillNameShort: (trans && trans.fwd ? trans.fwd : 'FWD').toUpperCase()
        };
        arrSelectedPlayer.push(row);
    }
    arrSelectedPlayerMain[config.enumPosition.forwards] = arrSelectedPlayer;
    //MidFielders Row
    arrSelectedPlayer = [];
    for (let j = 0; j < jMfCount; j++) {
        let row = {
            id: (j + 1), name: (trans && trans.midfielders ? trans.midfielders : 'Midfielders'), filterIndex: config.emptyValue, skill: config.enumPosition.midfielders, jorder: (j + 1)
            , skillNameShort: (trans && trans.mid ? trans.mid : 'MID').toUpperCase()
        };
        arrSelectedPlayer.push(row);
    }
    arrSelectedPlayerMain[config.enumPosition.midfielders] = arrSelectedPlayer;
    //Defenders Row
    arrSelectedPlayer = [];
    for (let j = 0; j < jDfCount; j++) {
        let row = {
            id: (j + 1), name: (trans && trans.defenders ? trans.defenders : 'Defenders'), filterIndex: config.emptyValue, skill: config.enumPosition.defenders, jorder: (j + 1)
            , skillNameShort: (trans && trans.def ? trans.def : 'DEF').toUpperCase()
        };
        arrSelectedPlayer.push(row);
    }
    arrSelectedPlayerMain[config.enumPosition.defenders] = arrSelectedPlayer;
    //GoalKeepers Row
    arrSelectedPlayer = [];
    for (let j = 0; j < jGkCount; j++) {
        let row = {
            id: (j + 1), name: (trans && trans.goalkeepers ? trans.goalkeepers : 'Goalkeepers'), filterIndex: config.emptyValue, skill: config.enumPosition.goalkeepers, jorder: (j + 1)
            , skillNameShort: (trans && trans.gk ? trans.gk : 'GK').toUpperCase()
        };
        arrSelectedPlayer.push(row);
    }
    arrSelectedPlayerMain[config.enumPosition.goalkeepers] = arrSelectedPlayer;
    return arrSelectedPlayerMain;
}
app.creatrNewSkillRow = (arrSkillOld, arrSkillNew, jAvoidFiltreIndex) => {
    for (let jN = 0; jN < (arrSkillOld || []).length; jN++) {
        const objRowOld = arrSkillOld[jN];
        for (let jNDex = 0; jNDex < (arrSkillNew || []).length; jNDex++) {
            const objRowNew = arrSkillNew[jNDex];
            if (jAvoidFiltreIndex !== objRowOld.filterIndex && objRowNew.filterIndex === config.emptyValue) {
                objRowNew.filterIndex = objRowOld.filterIndex;
                break;
            }
        }

    }
    return arrSkillNew;
}
app.getCurrentSelectedTabIndexes = (jSkillP, arrSkillFiltreTab, arrSelectedPlayer) => {
    let jSkillFiltreTab = jSkillP;
    let jSkillFiltreTabPlayer = -1;
    loop1:
    for (let j = 0; j < (arrSkillFiltreTab || []).length; j++) {
        const objSkill = arrSkillFiltreTab[j];
        const jSkillKey = objSkill.id;
        const arrSelectedPlayerBySkill = arrSelectedPlayer[jSkillKey];
        //
        const arrSkillNew = [];
        const arrSkillNewEmpty = [];
        for (let jN = 0; jN < (arrSelectedPlayerBySkill || []).length; jN++) {
            let objSkill = arrSelectedPlayerBySkill[jN];
            if (objSkill.filterIndex === config.emptyValue) {
                arrSkillNewEmpty.push({ ...objSkill });
            }
            else {
                arrSkillNew.push({ ...objSkill });
            }
        }
        for (let jN = 0; jN < (arrSkillNewEmpty || []).length; jN++) {
            let objSkill = arrSkillNewEmpty[jN];
            arrSkillNew.push({ ...objSkill });
        }
        //
        for (let jN = 0; jN < (arrSkillNew || []).length; jN++) {
            const objPlayer = arrSkillNew[jN];
            if (objPlayer.filterIndex === config.emptyValue) {
                if ((!jSkillFiltreTab && jSkillFiltreTab !== 0) || (jSkillFiltreTab <= j)) {
                    jSkillFiltreTab = j;
                    jSkillFiltreTabPlayer = jN;
                    break loop1;
                }
            }
        }

    }
    if (jSkillFiltreTabPlayer === -1) {
        loop1:
        for (let j = 0; j < (arrSkillFiltreTab || []).length; j++) {
            const objSkill = arrSkillFiltreTab[j];
            const jSkillKey = objSkill.id;
            const arrSelectedPlayerBySkill = arrSelectedPlayer[jSkillKey];
            //
            const arrSkillNew = [];
            const arrSkillNewEmpty = [];
            for (let jN = 0; jN < (arrSelectedPlayerBySkill || []).length; jN++) {
                let objSkill = arrSelectedPlayerBySkill[jN];
                if (objSkill.filterIndex === config.emptyValue) {
                    arrSkillNewEmpty.push({ ...objSkill });
                }
                else {
                    arrSkillNew.push({ ...objSkill });
                }
            }
            for (let jN = 0; jN < (arrSkillNewEmpty || []).length; jN++) {
                let objSkill = arrSkillNewEmpty[jN];
                arrSkillNew.push({ ...objSkill });
            }
            //
            for (let jN = 0; jN < (arrSkillNew || []).length; jN++) {
                const objPlayer = arrSkillNew[jN];
                if (objPlayer.filterIndex === config.emptyValue) {
                    if ((jSkillFiltreTab > j)) {
                        jSkillFiltreTab = j;
                        jSkillFiltreTabPlayer = jN;
                        break loop1;
                    }
                }
            }

        }
    }
    if (jSkillFiltreTabPlayer === -1) {
        jSkillFiltreTab = config.emptyValue;
        jSkillFiltreTabPlayer = config.emptyValue;;
    }
    return {
        jSkillFiltreTab: jSkillFiltreTab,
        jSkillFiltreTabPlayer: jSkillFiltreTabPlayer
    }
}
app.isPlayerFromSelectionBySkill = (arrSelectedPlayer, jSKill, jId) => {
    let falg = false;
    const arrSkill = arrSelectedPlayer[jSKill];
    for (let jNDex = 0; jNDex < (arrSkill || []).length; jNDex++) {
        const objRowNew = arrSkill[jNDex];
        if (jId === objRowNew.filterIndex) {
            falg = true;
            break;
        }

    }
    return falg;
}
app.isPlayerFromSelection = (arrSelectedPlayer, jId) => {
    let falg = false;
    let jSKill = config.enumPosition.forwards;
    falg = app.isPlayerFromSelectionBySkill(arrSelectedPlayer, jSKill, jId);
    if (!falg) {
        //mid
        jSKill = config.enumPosition.midfielders;
        falg = app.isPlayerFromSelectionBySkill(arrSelectedPlayer, jSKill, jId);
    }
    if (!falg) {
        //def
        jSKill = config.enumPosition.defenders;
        falg = app.isPlayerFromSelectionBySkill(arrSelectedPlayer, jSKill, jId);
    }
    if (!falg) {
        //gk
        jSKill = config.enumPosition.goalkeepers;
        falg = app.isPlayerFromSelectionBySkill(arrSelectedPlayer, jSKill, jId);
    }
    return falg;
}
app.getPlayerDetailsById = (arrPlayers, jId) => {
    let objPlayer = null;
    if (jId) {
        objPlayer = arrPlayers && arrPlayers.find(ele => ele.Id === jId);
    }
    return objPlayer;
}
app.breakRefInSelection = (arrSelectedPlayer) => {
    let arrSelectedPlayerMain = {};
    for (let jKey in arrSelectedPlayer) {
        const arrSkill = arrSelectedPlayer[jKey];
        let arrSkillNew = [];
        for (let jN = 0; jN < (arrSkill || []).length; jN++) {
            let objSkill = arrSkill[jN];
            arrSkillNew.push({ ...objSkill });
        }
        arrSelectedPlayerMain[jKey] = arrSkillNew;
    }
    return arrSelectedPlayerMain;
}
app.getFilledCountOfPlayerSelectionBySkill = (arrSelectedPlayer, jSKill) => {
    let jCount = 0;
    const arrSkill = arrSelectedPlayer[jSKill];
    for (let jNDex = 0; jNDex < (arrSkill || []).length; jNDex++) {
        const objRowNew = arrSkill[jNDex];
        if (config.emptyValue !== objRowNew.filterIndex) {
            jCount = jCount + 1;
        }

    }
    return jCount;
}
app.getGenerateImage = function (guid, scenarioCode, callback) {
    let sUrl = config.shreGenerateImage.replace('{{guid}}', guid);
    sUrl = sUrl.replace('{{gamedayid}}', 1);
    let obj = {
        url: sUrl,
        method: 'post',
        params: {
            LangCode: config.CURRENT_LANGUAGE,
            phaseId: 1,
            scenario: scenarioCode
        }
    };
    return core.makeAPICall(obj)
        .then(function (response) {

            if (callback) {
                //console.log(response)
                callback(response.data);
            }
        })
        .catch(function (error) {

            if (callback) {
                callback(false);
            }
        });
}
app.getGenerateImageResult = function (formationId, callback) {
    let sUrl = config.shreGenerateImageResult;
    let obj = {
        url: sUrl,
        method: 'get',
        params: {
            formationId: formationId
        }
    };
    return core.makeAPICall(obj)
        .then(function (response) {

            if (callback) {
                //console.log(response)
                callback(response.data);
            }
        })
        .catch(function (error) {

            if (callback) {
                callback(false);
            }
        });
}
export default app;