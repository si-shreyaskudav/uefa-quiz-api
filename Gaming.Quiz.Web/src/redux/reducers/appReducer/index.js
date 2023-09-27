import {
    FETCH_TRANS_PENDING,
    FETCH_TRANS_REJECTED,
    FETCH_TRANS_FULFILLED,

    FETCH_PLAYERS_PENDING,
    FETCH_PLAYERS_REJECTED,
    FETCH_PLAYERS_FULFILLED,

    FETCH_USER_TEAM_PENDING,
    FETCH_USER_TEAM_REJECTED,
    FETCH_USER_TEAM_FULFILLED,

    FETCH_REVEALED_TEAM_PENDING,
    FETCH_REVEALED_TEAM_REJECTED,
    FETCH_REVEALED_TEAM_FULFILLED,

    FETCH_PLAYER_CARD_PENDING,
    FETCH_PLAYER_CARD_REJECTED,
    FETCH_PLAYER_CARD_FULFILLED,

    FETCH_CONSTRAINTS_PENDING,
    FETCH_CONSTRAINTS_REJECTED,
    FETCH_CONSTRAINTS_FULFILLED,

    FETCH_FORMATION_PENDING,
    FETCH_FORMATION_REJECTED,
    FETCH_FORMATION_FULFILLED,

    ENABLE_DROP_MODE,
    DROP_ITEM,

    POST_SAVE_TEAM_PENDING,
    POST_SAVE_TEAM_REJECTED,
    POST_SAVE_TEAM_FULFILLED,

    POST_LOGIN_PENDING,
    POST_LOGIN_REJECTED,
    POST_LOGIN_FULFILLED,

    POST_GDPR_PENDING,
    POST_GDPR_REJECTED,
    POST_GDPR_FULFILLED,

    FETCH_USER_BRACKET_FULFILLED,
    CHANGE_MODE,
    RESET_MODE,

    SHOW_HIDE_POPUP,
    CHANGE_FORMATION,
    AFTER_CHANGE_FORMATION,
    CREATE_EMPTY_TEAM_BY_FORMATION,
    CHANGE_SKILL_TAB,
    ADD_PLAYER,
    REMOVE_PLAYER,
    SHOW_HIDE_PLAYERLISTING,
    ADD_REMOVED_PLAYER_FROM_FORMATION,
    REMOVE_REMOVED_PLAYER_FROM_FORMATION,
    ADD_PLAYER_FOR_CARD,
    REMOVE_PLAYER_FOR_CARD
} from './../../actionTypes';
import config from './../../../common/config';
import core from './../../../common/core';
import app from './../../../common/app';
const initialState = {
    isGameStatus: config.enumGameStatus.open,
    isLoading: false,
    isTransLoading: false,
    isTransError: false,
    isTransSuccess: false,
    trans: {},
    //
    isPlayersLoading: false,
    isPlayersError: false,
    isPlayersSuccess: false,
    arrPlayers: null,
    //
    isUserTeamLoading: false,
    isUserTeamError: false,
    isUserTeamSuccess: false,
    arrUserTeam: null,
    //
    isRevealedTeamLoading: false,
    isRevealedTeamError: false,
    isRevealedTeamSuccess: false,
    arrRevealedTeam: null,
    //
    isPlayerCardLoading: false,
    isPlayerCardError: false,
    isPlayerCardSuccess: false,
    arrPlayerCard: null,
    //
    isConstraintsLoading: false,
    isConstraintsError: false,
    isConstraintsSuccess: false,
    objContraints: null,
    //
    isLoginLoading: false,
    isLoginError: false,
    isLoginSuccess: false,
    objLogin: null,
    //
    isGdprLoading: false,
    isGdprError: false,
    isGdprSuccess: false,
    objGdpr: null,
    //
    isFormationLoading: false,
    isFormationError: false,
    isFormationSuccess: false,
    arrFormations: [
    ],
    jFormation: config.emptyValue,
    isSaveLoading: false,
    isSaveError: false,
    isSaveSuccess: false,
    save: {},
    arrSelectedPlayer: {},
    arrSelectedPlayerRevealed: {},
    objDrag: { id: config.emptyValue, groupId: config.emptyValue },
    jMode: config.enumMode.open,
    objPopup: null,
    arrSkillFiltreTab: [
        { id: config.enumPosition.goalkeepers },
        { id: config.enumPosition.defenders },
        { id: config.enumPosition.midfielders },
        { id: config.enumPosition.forwards },
    ],
    jSkillFiltreTab: config.emptyValue,
    jSkillFiltreTabPlayer: config.emptyValue,
    isOpenPlayerListing: false,
    jAvoidFiltreIndex: config.emptyValue,
    objPlayerCardFiltreIndex: null,
    hasTeam: false,
    sAddPlayerError: '',
    resultCompId: config.emptyValue
};
export default function appData(state = initialState, action) {
    switch (action.type) {
        /** START API */
        ////////////////get//////////////////////
        case FETCH_TRANS_PENDING: {
            //core.setLoader(true);
            return {
                ...state,
                isLoading: true,
                isTransLoading: true,
                isTransError: false,
                isTransSuccess: false,
            };
        }
        case FETCH_TRANS_REJECTED: {
            //core.setLoader();
            return {
                ...state,
                isLoading: false,
                isTransLoading: false,
                isTransError: true,
                isTransSuccess: false,
            };
        }
        case FETCH_TRANS_FULFILLED: {
            let trans = null;
            if (action.payload.data && action.payload.data.value) {
                trans = action.payload.data.value.Translations;
            }
            //core.setLoader();
            return {
                ...state,
                isLoading: false,
                isTransLoading: false,
                isTransError: false,
                isTransSuccess: true,
                trans: trans
            };
        }

        case FETCH_PLAYERS_PENDING: {
            //core.setLoader(true);
            return {
                ...state,
                isPlayersLoading: true,
                isPlayersError: false,
                isPlayersSuccess: false,
            };
        }
        case FETCH_PLAYERS_REJECTED: {
            //core.setLoader();
            return {
                ...state,
                isPlayersLoading: false,
                isPlayersError: true,
                isPlayersSuccess: false,
            };
        }
        case FETCH_PLAYERS_FULFILLED: {
            let arrPlayers = null;
            if (action.payload.data && action.payload.data.Value) {
                arrPlayers = action.payload.data.Value;
            }
            //core.setLoader();
            // if (arrPlayers && arrPlayers.length) {
            //     arrPlayers = arrPlayers.sort((a, b) => {
            //         const sAPlayerDisplayName = a.PlayerDisplayName;
            //         const sBPlayerDisplayName = b.PlayerDisplayName;
            //         if (sAPlayerDisplayName < sBPlayerDisplayName) {
            //             return -1;
            //         }
            //         if (sAPlayerDisplayName > sBPlayerDisplayName) {
            //             return 1;
            //         }
            //         // a must be equal to b
            //         return 0;
            //     });
            // }
            return {
                ...state,
                isPlayersLoading: false,
                isPlayersError: false,
                isPlayersSuccess: true,
                arrPlayers: arrPlayers
            };
        }

        case FETCH_USER_TEAM_PENDING: {
            //core.setLoader(true);
            return {
                ...state,
                isUserTeamLoading: true,
                isUserTeamError: false,
                isUserTeamSuccess: false,
            };
        }
        case FETCH_USER_TEAM_REJECTED: {
            //core.setLoader();
            return {
                ...state,
                isUserTeamLoading: false,
                isUserTeamError: true,
                isUserTeamSuccess: false,
            };
        }
        case FETCH_USER_TEAM_FULFILLED: {
            let arrUserTeam = null;
            let jFormation = state.jFormation;
            let arrFormations = state.arrFormations;
            let arrSelectedPlayerMain = state.arrSelectedPlayer;
            let hasTeam = state.hasTeam;
            if (action.payload.data.data && action.payload.data.data.value) {
                arrUserTeam = action.payload.data.data.value;
                hasTeam = true;
                let objFormation = null;
                for (let j = 0; j < (arrFormations || []).length; j++) {
                    const objRow = arrFormations[j];
                    if (objRow.CompId === arrUserTeam.gdCompId) {
                        objFormation = objRow;
                        jFormation = j;
                        break;
                    }
                }
                arrSelectedPlayerMain = app.creteEmptyTeamFormation(objFormation, state.trans);
                let arrUserTeamPlayer = arrUserTeam.players;
                loop1:
                for (let j = 0; j < (arrUserTeamPlayer || []).length; j++) {
                    const objRowTeamPlayer = arrUserTeamPlayer[j];
                    loop2:
                    for (let jKey in arrSelectedPlayerMain) {
                        const arrSkill = arrSelectedPlayerMain[jKey];
                        loop3:
                        for (let jN = 0; jN < (arrSkill || []).length; jN++) {
                            let objSkill = arrSkill[jN];
                            if (objSkill.filterIndex === config.emptyValue && objSkill.skill === objRowTeamPlayer.skill) {
                                objSkill.filterIndex = objRowTeamPlayer.id;
                                break loop2;
                            }
                        }
                    }
                }
            }
            //core.setLoader();
            return {
                ...state,
                isUserTeamLoading: false,
                isUserTeamError: false,
                isUserTeamSuccess: true,
                arrUserTeam: arrUserTeam,
                jFormation: jFormation,
                arrSelectedPlayer: arrSelectedPlayerMain,
                hasTeam: hasTeam
            };
        }

        case FETCH_REVEALED_TEAM_PENDING: {
            //core.setLoader(true);
            return {
                ...state,
                isRevealedTeamLoading: true,
                isRevealedTeamError: false,
                isRevealedTeamSuccess: false,
            };
        }
        case FETCH_REVEALED_TEAM_REJECTED: {
            //core.setLoader();
            return {
                ...state,
                isRevealedTeamLoading: false,
                isRevealedTeamError: true,
                isRevealedTeamSuccess: false,
            };
        }
        case FETCH_REVEALED_TEAM_FULFILLED: {
            let arrRevealedTeam = null;
            let jFormation = state.jFormation;
            let arrFormations = state.arrFormations;
            let arrSelectedPlayerMain = state.arrSelectedPlayerRevealed;
            let hasTeam = state.hasTeam;
            let resultCompId = state.resultCompId;
            if (action.payload.data.data && action.payload.data.data.value) {
                arrRevealedTeam = action.payload.data.data.value;
                let objFormation = null;
                for (let j = 0; j < (arrFormations || []).length; j++) {
                    const objRow = arrFormations[j];
                    if (objRow.CompId === arrRevealedTeam.unvieledMDCompId) {
                        objFormation = objRow;
                        resultCompId = objFormation.CompId;
                        //jFormation = j;
                        break;
                    }
                }
                arrSelectedPlayerMain = app.creteEmptyTeamFormation(objFormation, state.trans);
                let arrRevealedTeamPlayer = arrRevealedTeam.unvieledTeams;
                loop1:
                for (let j = 0; j < (arrRevealedTeamPlayer || []).length; j++) {
                    const objRowTeamPlayer = arrRevealedTeamPlayer[j];
                    loop2:
                    for (let jKey in arrSelectedPlayerMain) {
                        const arrSkill = arrSelectedPlayerMain[jKey];
                        loop3:
                        for (let jN = 0; jN < (arrSkill || []).length; jN++) {
                            let objSkill = arrSkill[jN];
                            if (objSkill.filterIndex === config.emptyValue && objSkill.skill === objRowTeamPlayer.skill) {
                                objSkill.filterIndex = objRowTeamPlayer.id;
                                break loop2;
                            }
                        }
                    }
                }
            }
            //core.setLoader();
            return {
                ...state,
                isRevealedTeamLoading: false,
                isRevealedTeamError: false,
                isRevealedTeamSuccess: true,
                arrRevealedTeam: arrRevealedTeam,
                arrSelectedPlayerRevealed: arrSelectedPlayerMain,
                resultCompId: resultCompId
            };
        }

        case FETCH_PLAYER_CARD_PENDING: {
            //core.setLoader(true);
            return {
                ...state,
                arrPlayerCard: null,
                isPlayerCardLoading: true,
                isPlayerCardError: false,
                isPlayerCardSuccess: false,
            };
        }
        case FETCH_PLAYER_CARD_REJECTED: {
            //core.setLoader();
            return {
                ...state,
                isPlayerCardLoading: false,
                isPlayerCardError: true,
                isPlayerCardSuccess: false,
            };
        }
        case FETCH_PLAYER_CARD_FULFILLED: {
            let arrPlayerCard = null;
            if (action.payload.data && action.payload.data.Value) {
                arrPlayerCard = action.payload.data.Value;
            }
            //core.setLoader();
            return {
                ...state,
                isPlayerCardLoading: false,
                isPlayerCardError: false,
                isPlayerCardSuccess: true,
                arrPlayerCard: arrPlayerCard
            };
        }

        case FETCH_FORMATION_PENDING: {
            //core.setLoader(true);
            return {
                ...state,
                isFormationLoading: true,
                isFormationError: false,
                isFormationSuccess: false,
            };
        }
        case FETCH_FORMATION_REJECTED: {
            //core.setLoader();
            return {
                ...state,
                isFormationLoading: false,
                isFormationError: true,
                isFormationSuccess: false,
            };
        }
        case FETCH_FORMATION_FULFILLED: {
            let arrFormations = [];
            let jFormation = state.jFormation;
            let jFormationTemp = core.getLocalStorageValue(config.formationId);;
            if (action.payload.data && action.payload.data.Value) {
                arrFormations = action.payload.data.Value;
                const arrResultFormations = arrFormations.map((ele, jIndex) => {
                    if (ele.IsActive) {
                        if (ele.IsDefault) {
                            jFormation = jIndex;
                        }
                        return { ...ele, value: `${ele.DEF}-${ele.MID}-${ele.FWD}` };
                    }
                    else {
                        return null;
                    }
                });
                if (jFormationTemp || (jFormationTemp && Number(jFormationTemp) === 0)) {
                    jFormation = Number(jFormationTemp);
                }
                return {
                    ...state,
                    isFormationLoading: false,
                    isFormationError: false,
                    isFormationSuccess: true,
                    arrFormations: arrResultFormations,
                    jFormation: jFormation
                };
            }
            else {
                return {
                    ...state,
                    isLoading: false,
                    isFormationLoading: false,
                    isFormationError: false,
                    isFormationSuccess: true,
                }
            }
        }

        case FETCH_CONSTRAINTS_PENDING: {
            //core.setLoader(true);
            return {
                ...state,
                isConstraintsLoading: true,
                isConstraintsError: false,
                isConstraintsSuccess: false
            };
        }
        case FETCH_CONSTRAINTS_REJECTED: {
            //core.setLoader();
            return {
                ...state,
                isConstraintsLoading: false,
                isConstraintsError: true,
                isConstraintsSuccess: false
            };
        }
        case FETCH_CONSTRAINTS_FULFILLED: {
            let objContraints = null;
            let isGameStatus = state.isGameStatus;
            if (action.payload.data) {
                objContraints = action.payload.data.Value;
                if (objContraints) {
                    isGameStatus = Number(objContraints.ISTourStatus);
                }
            }
            return {
                ...state,
                isConstraintsLoading: false,
                isConstraintsError: false,
                isConstraintsSuccess: true,
                objContraints: objContraints,
                isGameStatus: isGameStatus
            };
        }
        ////////////////get//////////////////////

        ////////////////post/////////////////////
        case POST_SAVE_TEAM_PENDING: {
            //core.setLoader(true);
            return {
                ...state,
                isSaveLoading: true,
                isSaveError: false,
                isSaveSuccess: false,
            };
        }
        case POST_SAVE_TEAM_REJECTED: {
            //core.setLoader();
            return {
                ...state,
                isSaveLoading: false,
                isSaveError: true,
                isSaveSuccess: false,
            };
        }
        case POST_SAVE_TEAM_FULFILLED: {
            let save = null;
            if (action.payload.data && action.payload.data.value) {
                //console.log(action.payload.data.value);
                save = action.payload.data.value;
            }
            //core.setLoader();
            return {
                ...state,
                isSaveLoading: false,
                isSaveError: false,
                isSaveSuccess: true,
                save: save
            };
        }
        case POST_LOGIN_PENDING: {
            //core.setLoader(true);
            return {
                ...state,
                isLoginLoading: true,
                isLoginError: false,
                isLoginSuccess: false,
            };
        }
        case POST_LOGIN_REJECTED: {
            //core.setLoader();
            return {
                ...state,
                isLoginLoading: false,
                isLoginError: true,
                isLoginSuccess: false,
            };
        }
        case POST_LOGIN_FULFILLED: {
            let objLogin = null;
            if (action.payload.data && action.payload.data.value) {
                //console.log(action.payload.data.value);
                objLogin = action.payload.data.value;
            }
            //core.setLoader();
            return {
                ...state,
                isLoginLoading: false,
                isLoginError: false,
                isLoginSuccess: true,
                objLogin: objLogin
            };
        }
        case POST_GDPR_PENDING: {
            //core.setLoader(true);
            return {
                ...state,
                isGdprLoading: true,
                isGdprError: false,
                isGdprSuccess: false,
            };
        }
        case POST_GDPR_REJECTED: {
            //core.setLoader();
            return {
                ...state,
                isGdprLoading: false,
                isGdprError: true,
                isGdprSuccess: false,
            };
        }
        case POST_GDPR_FULFILLED: {
            //core.setLoader();
            return {
                ...state,
                isGdprLoading: false,
                isGdprError: false,
                isGdprSuccess: true,
            };
        }
        ////////////////post/////////////////////
        /** END API */
        case ENABLE_DROP_MODE: {
            return {
                ...state,
                objDrag: { ...action.payload.objDrag },
                jMode: action.payload.jMode
            }
        }
        case CHANGE_MODE: {
            return {
                ...state,
                jMode: action.payload.jMode
            }
        }
        case DROP_ITEM: {
            const objDropFiltreIndex = action.payload;
            const { objPlayerCardFiltreIndex, arrSelectedPlayer, jFormation } = state;
            let arrSelectedPlayerMain = app.breakRefInSelection(arrSelectedPlayer);

            for (let jKey in arrSelectedPlayerMain) {
                let arrSkill = arrSelectedPlayerMain[jKey];
                for (let jN = 0; jN < (arrSkill || []).length; jN++) {
                    let objRow = arrSkill[jN];
                    if (objDropFiltreIndex.filterIndex !== config.emptyValue) {
                        if (objPlayerCardFiltreIndex.filterIndex === objRow.filterIndex) {
                            objRow.filterIndex = objDropFiltreIndex.filterIndex;
                        }
                        else if (objDropFiltreIndex.filterIndex === objRow.filterIndex) {
                            objRow.filterIndex = objPlayerCardFiltreIndex.filterIndex;
                        }
                    }
                    else {
                        if (objPlayerCardFiltreIndex.skill === objRow.skill && objPlayerCardFiltreIndex.jorder === objRow.jorder) {
                            objRow.filterIndex = objDropFiltreIndex.filterIndex;
                        }
                        else if (objDropFiltreIndex.skill === objRow.skill && objDropFiltreIndex.jorder === objRow.jorder) {
                            objRow.filterIndex = objPlayerCardFiltreIndex.filterIndex;
                        }
                    }
                }
            }
            core.setLocalStorageValue(config.formationId, jFormation);
            core.setLocalStorageValue(config.tempSelectedPlayerCookieName, JSON.stringify(arrSelectedPlayerMain));
            return {
                ...state,
                jMode: config.enumMode.open,
                objPlayerCardFiltreIndex: null,
                arrSelectedPlayer: arrSelectedPlayerMain
            }
        }

        case RESET_MODE: {
            let arrBracket = state.arrBracket;
            let arrContraints = app.resetBracket(arrBracket);
            return {
                ...state,
                arrContraints: arrContraints,
                arrFormation: []
            }
        }

        case SHOW_HIDE_POPUP: {
            return {
                ...state,
                objPopup: { ...state.objPopup, ...action.payload }
            }
        }
        case CHANGE_FORMATION: {
            return {
                ...state,
                jFormation: action.payload
            }
        }
        case SHOW_HIDE_PLAYERLISTING: {
            return {
                ...state,
                isOpenPlayerListing: action.payload
            }
        }

        case AFTER_CHANGE_FORMATION: {
            const { objFormation, jAvoidFiltreIndex } = action.payload;
            const { arrSelectedPlayer, jFormation } = state;
            const arrSelectedPlayerMain = app.creteEmptyTeamFormation(objFormation, state.trans);

            const arrFWDOld = arrSelectedPlayer[config.enumPosition.forwards];
            const arrMIDOld = arrSelectedPlayer[config.enumPosition.midfielders];
            const arrDEFOld = arrSelectedPlayer[config.enumPosition.defenders];
            const arrGKOld = arrSelectedPlayer[config.enumPosition.goalkeepers];

            let arrFWDNew = arrSelectedPlayerMain[config.enumPosition.forwards];
            let arrMIDNew = arrSelectedPlayerMain[config.enumPosition.midfielders];
            let arrDEFNew = arrSelectedPlayerMain[config.enumPosition.defenders];
            let arrGKNew = arrSelectedPlayerMain[config.enumPosition.goalkeepers];

            //Forwards
            arrFWDNew = app.creatrNewSkillRow(arrFWDOld, arrFWDNew, jAvoidFiltreIndex);
            //Midfielders
            arrMIDNew = app.creatrNewSkillRow(arrMIDOld, arrMIDNew, jAvoidFiltreIndex);
            //Defenders
            arrDEFNew = app.creatrNewSkillRow(arrDEFOld, arrDEFNew, jAvoidFiltreIndex);
            //Goakippers
            arrGKNew = app.creatrNewSkillRow(arrGKOld, arrGKNew, jAvoidFiltreIndex);
            const objTab = app.getCurrentSelectedTabIndexes(null, state.arrSkillFiltreTab, arrSelectedPlayerMain);
            core.setLocalStorageValue(config.formationId, jFormation);
            core.setLocalStorageValue(config.tempSelectedPlayerCookieName, JSON.stringify(arrSelectedPlayerMain));
            return {
                ...state,
                arrSelectedPlayer: arrSelectedPlayerMain,
                jSkillFiltreTab: objTab.jSkillFiltreTab,
                jSkillFiltreTabPlayer: objTab.jSkillFiltreTabPlayer,
                jAvoidFiltreIndex: config.emptyValue
            }
        }
        case CREATE_EMPTY_TEAM_BY_FORMATION: {
            const objFormation = action.payload.objFormation;
            let arrSelectedPlayerMain = app.creteEmptyTeamFormation(objFormation, state.trans);
            //const objCookie = app.getLoginCookie();
            //if (!objCookie) {
            let arrSelectedPlayerCookie = core.getLocalStorageValue(config.tempSelectedPlayerCookieName);
            let arrSelectedPlayerTemp = JSON.parse(arrSelectedPlayerCookie);
            if (arrSelectedPlayerTemp) {
                arrSelectedPlayerMain = arrSelectedPlayerTemp;
            }
            //}
            const objTab = app.getCurrentSelectedTabIndexes(null, state.arrSkillFiltreTab, arrSelectedPlayerMain);
            return {
                ...state,
                arrSelectedPlayer: arrSelectedPlayerMain,
                jSkillFiltreTab: objTab.jSkillFiltreTab,
                jSkillFiltreTabPlayer: objTab.jSkillFiltreTabPlayer,
            }
        }
        case CHANGE_SKILL_TAB: {
            const { jSkillFiltreTab, jSkillFiltreTabPlayer } = action.payload;
            let jSkillFiltreTabP = jSkillFiltreTab;
            let jSkillFiltreTabPlayerP = jSkillFiltreTabPlayer;

            /*if (!(jSkillFiltreTabP && jSkillFiltreTabP === 0)) {
    
            }
            else */
            if ((!jSkillFiltreTabPlayerP && jSkillFiltreTabPlayerP !== 0)) {
                const objTab = app.getCurrentSelectedTabIndexes(jSkillFiltreTabP, state.arrSkillFiltreTab, state.arrSelectedPlayer);
                jSkillFiltreTabP = jSkillFiltreTabP;
                jSkillFiltreTabPlayerP = jSkillFiltreTabP === objTab.jSkillFiltreTab ? objTab.jSkillFiltreTabPlayer : 0;
            }

            return {
                ...state,
                jSkillFiltreTab: jSkillFiltreTabP,
                jSkillFiltreTabPlayer: jSkillFiltreTabPlayerP
            }
        }
        case ADD_PLAYER: {
            const { arrSelectedPlayer, isOpenPlayerListing, jFormation } = state;
            let isOpenPlayerListingP = isOpenPlayerListing;
            let arrSelectedPlayerMain = app.breakRefInSelection(arrSelectedPlayer);
            let { jId, jSkill } = action.payload;
            const arrSkill = arrSelectedPlayerMain[jSkill];
            let isPlayerAdded = false;
            let sAddPlayerError = '';
            for (let jN = 0; jN < (arrSkill || []).length; jN++) {
                let objSkill = arrSkill[jN];
                if (objSkill.filterIndex === config.emptyValue) {
                    objSkill.filterIndex = jId;
                    isPlayerAdded = true;
                    break;
                }
            }
            if (!isPlayerAdded) {
                sAddPlayerError = config.enumPositionError[jSkill];
            }
            //
            let flag = false;
            for (let jKey in arrSelectedPlayerMain) {
                const arrSkill = arrSelectedPlayerMain[jKey];
                const jSKill = jKey;
                const jFilledCount = app.getFilledCountOfPlayerSelectionBySkill(arrSelectedPlayerMain, jSKill);
                if (arrSkill.length !== jFilledCount) {
                    flag = true;
                    break;
                }
            }
            if (!flag) {
                isOpenPlayerListingP = false;
            }
            //
            const objTab = app.getCurrentSelectedTabIndexes(state.jSkillFiltreTab, state.arrSkillFiltreTab, arrSelectedPlayerMain);
            core.setLocalStorageValue(config.formationId, jFormation);
            core.setLocalStorageValue(config.tempSelectedPlayerCookieName, JSON.stringify(arrSelectedPlayerMain));
            return {
                ...state,
                arrSelectedPlayer: arrSelectedPlayerMain,
                jSkillFiltreTab: objTab.jSkillFiltreTab,
                jSkillFiltreTabPlayer: objTab.jSkillFiltreTabPlayer,
                isOpenPlayerListing: isOpenPlayerListingP,
                sAddPlayerError: sAddPlayerError
            }
        }
        case REMOVE_PLAYER: {
            const { arrSelectedPlayer, jFormation } = state;
            let arrSelectedPlayerMain = app.breakRefInSelection(arrSelectedPlayer);
            let { jId, jSkill } = action.payload;
            const arrSkill = arrSelectedPlayerMain[jSkill];
            //const arrSkillNew = [];
            //const arrSkillNewEmpty = [];
            for (let jN = 0; jN < (arrSkill || []).length; jN++) {
                let objSkill = arrSkill[jN];
                if (objSkill.filterIndex === jId) {
                    objSkill.filterIndex = config.emptyValue;
                }
                // if (objSkill.filterIndex === jId) {
                //     objSkill.filterIndex = config.emptyValue;
                //     arrSkillNewEmpty.push({ ...objSkill });
                // }
                // else {
                //     arrSkillNew.push({ ...objSkill });
                // }
            }
            // for (let jN = 0; jN < (arrSkillNewEmpty || []).length; jN++) {
            //     let objSkill = arrSkillNewEmpty[jN];
            //     arrSkillNew.push({ ...objSkill });
            // }
            arrSelectedPlayerMain[jSkill] = arrSkill;
            const objTab = app.getCurrentSelectedTabIndexes(state.jSkillFiltreTab, state.arrSkillFiltreTab, arrSelectedPlayerMain);
            core.setLocalStorageValue(config.formationId, jFormation);
            core.setLocalStorageValue(config.tempSelectedPlayerCookieName, JSON.stringify(arrSelectedPlayerMain));
            return {
                ...state,
                arrSelectedPlayer: arrSelectedPlayerMain,
                jSkillFiltreTab: objTab.jSkillFiltreTab,
                jSkillFiltreTabPlayer: objTab.jSkillFiltreTabPlayer,
            }
        }
        case ADD_REMOVED_PLAYER_FROM_FORMATION: {
            return {
                ...state,
                jAvoidFiltreIndex: (action.payload)
            }
        }
        case REMOVE_REMOVED_PLAYER_FROM_FORMATION: {
            return {
                ...state,
                jAvoidFiltreIndex: config.emptyValue
            }
        }
        case ADD_PLAYER_FOR_CARD: {
            return {
                ...state,
                objPlayerCardFiltreIndex: (action.payload)
            }
        }
        case REMOVE_PLAYER_FOR_CARD: {
            return {
                ...state,
                objPlayerCardFiltreIndex: null
            }

        }
        default: {
            return state;
        }
    }
}