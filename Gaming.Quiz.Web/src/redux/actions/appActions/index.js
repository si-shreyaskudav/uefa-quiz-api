import {
    FETCH_TRANS_PENDING,
    FETCH_TRANS_REJECTED,
    FETCH_TRANS_FULFILLED,
    ENABLE_DROP_MODE,
    DROP_ITEM,
    POST_SAVE_TEAM,
    POST_LOGIN,
    POST_SAVE_BRACKET,
    CHANGE_MODE,
    RESET_MODE,
    SHOW_HIDE_POPUP,
    CHANGE_FORMATION,
    AFTER_CHANGE_FORMATION,
    FETCH_FORMATION,
    FETCH_CONSTRAINTS,
    CREATE_EMPTY_TEAM_BY_FORMATION,
    FETCH_PLAYERS,
    FETCH_USER_TEAM,
    CHANGE_SKILL_TAB,
    ADD_PLAYER,
    REMOVE_PLAYER,
    SHOW_HIDE_PLAYERLISTING,
    ADD_REMOVED_PLAYER_FROM_FORMATION,
    REMOVE_REMOVED_PLAYER_FROM_FORMATION,
    ADD_PLAYER_FOR_CARD,
    REMOVE_PLAYER_FOR_CARD,
    FETCH_PLAYER_CARD,
    POST_GDPR,
    FETCH_REVEALED_TEAM
} from './../../actionTypes';
import { transAPI, constraintsAPI, formationAPI, postUserTeamAPI, playersAPI, getUserTeamUrl, playersCardAPI, postLoginAPI, gdprCookieAPI, getRevealedTeamUrl } from "./../../apiUrls";
import config from './../../../common/config';
import core from './../../../common/core';
import app from './../../../common/app';

/** START API */
////////////////get//////////////////////
export function transFetchData() {
    const params = {
        url: transAPI.replace('{{lang}}', config.CURRENT_LANGUAGE),
        params: {

        }
    };
    return ({ type: 'FETCH_TRANS', payload: core.makeAPICall(params) });
}
export function playersFetchData() {
    const params = {
        url: playersAPI.replace('{{lang}}', config.CURRENT_LANGUAGE),
        params: {
            //lang:config.CURRENT_LANGUAGE
        }
    };
    return ({ type: FETCH_PLAYERS, payload: core.makeAPICall(params) });
}
export function constraintsFetchData() {
    const params = {
        url: constraintsAPI.replace('{{lang}}', config.CURRENT_LANGUAGE),
        params: {

        }
    };
    return ({ type: FETCH_CONSTRAINTS, payload: core.makeAPICall(params) });
}
export function formationFetchData() {
    const params = {
        url: formationAPI,
        params: {

        }
    };
    return ({ type: FETCH_FORMATION, payload: core.makeAPICall(params) });
}
export function userTeamFetchData() {
    const objCookie = app.getLoginCookie();
    let guid = objCookie ? objCookie.GUID : '0';
    const params = {
        url: getUserTeamUrl.replace('{{guid}}', guid),
        params: {
            optType: 1,
            gamedayId: 1,
            phaseId: 1,
            language: config.CURRENT_LANGUAGE
        }
    };
    return ({ type: FETCH_USER_TEAM, payload: core.makeAPICall(params) });
}
export function revealedTeamFetchData() {
    const params = {
        url: getRevealedTeamUrl,
        params: {
        }
    };
    return ({ type: FETCH_REVEALED_TEAM, payload: core.makeAPICall(params) });
}
export function playerCardFetchData(payload) {
    const sUrl = playersCardAPI.replace('{{Id}}', payload).replace('{{lang}}', config.CURRENT_LANGUAGE);
    const params = {
        url: sUrl,
        params: {
        }
    };
    return ({ type: FETCH_PLAYER_CARD, payload: core.makeAPICall(params) });
}
////////////////get//////////////////////

////////////////post/////////////////////
export function saveTeam(payload) {
    const params = {
        url: postUserTeamAPI.replace('{{guid}}', payload.guid),
        method: 'POST',
        params: {
        },
        data: payload
    };
    return ({ type: POST_SAVE_TEAM, payload: core.makeAPICall(params) });
}
export function loginPost(payload) {
    const params = {
        url: postLoginAPI,
        method: 'POST',
        params: {
        },
        data: payload
    };
    return ({ type: POST_LOGIN, payload: core.makeAPICall(params) });
}
export function gdprPost() {
    const params = {
        url: gdprCookieAPI,
        method: 'POST',
        params: {
        },
        //data: payload
    };
    return ({ type: POST_GDPR, payload: core.makeAPICall(params) });
}

////////////////post/////////////////////
/** END API */

export function enableDropMode(payload) {
    return {
        type: ENABLE_DROP_MODE,
        payload: payload
    };
}
export function changeMode(payload) {
    return {
        type: CHANGE_MODE,
        payload: payload
    };
}
export function resetMode() {
    return {
        type: RESET_MODE
    };
}
export function dropItem(payload) {
    return {
        type: DROP_ITEM,
        payload: payload
    };
}


export function showHidePopup(payload) {
    return {
        type: SHOW_HIDE_POPUP,
        payload: payload
    };
}
export function changeFormation(payload) {
    return {
        type: CHANGE_FORMATION,
        payload: payload
    };
}
export function creteEmptyTeamByFormation(payload) {
    return {
        type: CREATE_EMPTY_TEAM_BY_FORMATION,
        payload: payload
    };
}
export function afterChangeFormation(payload) {
    return {
        type: AFTER_CHANGE_FORMATION,
        payload: payload
    };
}
export function changeSkillTab(payload) {
    return {
        type: CHANGE_SKILL_TAB,
        payload: payload
    };
}
export function addPlayer(payload) {
    return {
        type: ADD_PLAYER,
        payload: payload
    };
}
export function removePlayer(payload) {
    return {
        type: REMOVE_PLAYER,
        payload: payload
    };
}
export function showHidePlayerListing(payload) {
    return {
        type: SHOW_HIDE_PLAYERLISTING,
        payload: payload
    };
}
export function addRemovedPlayerFromFormation(payload) {
    return {
        type: ADD_REMOVED_PLAYER_FROM_FORMATION,
        payload: payload
    };
}
export function removeRemovedPlayerFromFormation(payload) {
    return {
        type: REMOVE_REMOVED_PLAYER_FROM_FORMATION,
        payload: payload
    };
}
export function addPlayerForCard(payload) {
    return {
        type: ADD_PLAYER_FOR_CARD,
        payload: payload
    };
}
export function removePlayerForCard(payload) {
    return {
        type: REMOVE_PLAYER_FOR_CARD,
        payload: payload
    };
}
