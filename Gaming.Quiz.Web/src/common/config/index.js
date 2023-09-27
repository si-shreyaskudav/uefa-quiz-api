const config = {};

config.webTitle = 'PZPN';
config.lang = {
    'English': 'en',
    'Polish': 'po',
};

config.enumClientIds = {
    FB: 1,
    TW: 2,
    EM: 3,
    PH: 4
};

config.lang2D = Object.keys(config.lang).map(key => {
    return config.lang[key];
});
config.defaultLanguage = 'po';
config.CURRENT_LANGUAGE = config.defaultLanguage;
let splitUrl = window.location.href.split('?lang=');
let subURL = splitUrl && splitUrl.length > 1 ? splitUrl[1] : null;
if (subURL && subURL.length) {
    let possibleLang = subURL.split('&').filter(Boolean)[0];
    if (config.lang2D.indexOf(possibleLang) > -1) {
        config.CURRENT_LANGUAGE = possibleLang;
    } else {
        config.CURRENT_LANGUAGE = config.defaultLanguage;
    }
}
config.COOKIE_FANTASY = 'PZPN_RAW';
config.COOKIE_STRP = 'COOKIE_STRP';
config.headers = { 'entity': 'd3tR0!t5m@sh' };
config.enumLoginCookie = {
    bothNotPresent: 1,
    fifaPresent: 2,
    fantasyPresent: 3,
    bothPresent: 4,
};

config.FB_ID = '505670046484855';
let location = window.location.href;

if (location.indexOf('int') > -1) {
    config.BASE_DOMAIN = '/';
    config.API_BASE = config.BASE_DOMAIN;
    config.IMG_BASE = config.BASE_DOMAIN + 'static-assets/build/images/';
    config.IMG_TEAM = config.IMG_BASE + 'flags/{{teamId}}.png';
    config.IMG_PITCH = config.IMG_BASE + 'players/onpitch/M_{{playerId}}.png';
    config.IMG_CARD = config.IMG_BASE + 'players/playercard/L_{{playerId}}.png';
    config.IMG_TOPFILTREPLAYER = config.IMG_BASE + 'players/playerfiltertop/S_{{playerId}}.png';
    config.URL_BASE = ('/');
    config.ACCOUNTS_API_BASE = (config.BASE_DOMAIN + 'services/api/');
    config.LEAGUES_API_BASE = (config.ACCOUNTS_API_BASE + 'League/user/{{guid}}/');
    config.LEADERBOARD_API_BASE = (config.ACCOUNTS_API_BASE + 'Leaderboard/user/');
    config.generateImagePath = (config.API_BASE + 'services/api/image/{guid}/{lang}/generate?download={bool}');
    config.sDomain = '.pzpn.sportz.io';
}
else if (location.indexOf(':300') > -1) {
    config.BASE_DOMAIN = '/';
    config.API_BASE = config.BASE_DOMAIN;
    config.IMG_BASE = config.BASE_DOMAIN + 'images/';
    config.IMG_TEAM = config.IMG_BASE + 'flags/{{teamId}}.png';
    config.IMG_PITCH = config.IMG_BASE + 'players/onpitch/M_{{playerId}}.png';
    config.IMG_CARD = config.IMG_BASE + 'players/playercard/L_{{playerId}}.png';
    config.IMG_TOPFILTREPLAYER = config.IMG_BASE + 'players/playerfiltertop/S_{{playerId}}.png';
    config.URL_BASE = ('/');
    config.ACCOUNTS_API_BASE = (config.BASE_DOMAIN + 'services/api/');
    config.LEAGUES_API_BASE = (config.ACCOUNTS_API_BASE + 'League/user/{{guid}}/');
    config.LEADERBOARD_API_BASE = (config.ACCOUNTS_API_BASE + 'Leaderboard/user/');
    config.sDomain = '.pzpn.sportz.io';
}
else {
    config.BASE_DOMAIN = '/';
    config.API_BASE = config.BASE_DOMAIN;
    config.IMG_BASE = config.BASE_DOMAIN + 'static-assets/build/images/';
    config.IMG_TEAM = config.IMG_BASE + 'flags/{{teamId}}.png';
    config.IMG_PITCH = config.IMG_BASE + 'players/onpitch/M_{{playerId}}.png';
    config.IMG_CARD = config.IMG_BASE + 'players/playercard/L_{{playerId}}.png';
    config.IMG_TOPFILTREPLAYER = config.IMG_BASE + 'players/playerfiltertop/S_{{playerId}}.png';
    config.URL_BASE = ('/');
    config.ACCOUNTS_API_BASE = (config.BASE_DOMAIN + 'services/api/');
    config.LEAGUES_API_BASE = (config.ACCOUNTS_API_BASE + 'League/user/{{guid}}/');
    config.LEADERBOARD_API_BASE = (config.ACCOUNTS_API_BASE + 'Leaderboard/user/');
    config.generateImagePath = (config.API_BASE + 'services/api/image/{guid}/{lang}/generate?download={bool}');
    config.sDomain = 'od100lat.laczynaspilka.pl';
}


config.IS_MOBILE = window.outerWidth < 768;
config.IS_TABLET = window.outerWidth <= 768

config.playerEmptyValue = 1;
config.emptyValue = 0;

config.Platform = { web: 1 };
config.redirectLogin = (window.location.protocol + '//'
    + window.location.host + config.ACCOUNTS_API_BASE + 'Session/user/sso/login?lang={{lang}}');
config.redirectLogout = (window.location.protocol + '//'
    + window.location.host + config.ACCOUNTS_API_BASE + 'Session/user/sso/logout');
config.GA_TRACKING_ID = (window.GA_TRACKING_ID ? window.GA_TRACKING_ID : 'UA-120293397-1');


config.playerImageUrl = '';
config.formationImageUrl = '';
config.jerseyImageUrl = '';
config.gkJerseyImageUrl = '';
config.arrSelectedPlayer = [];
config.loginFbUrl = config.BASE_DOMAIN + 'login?cid=fb';
config.loginGoogleUrl = config.BASE_DOMAIN + 'login?cid=google';

/*---leagues---*/
config.MaxLeaderBoardMembers = 1000;
config.noOfItemFirstPage = 5;
config.noOfItemAddOnPage = 5;
config.maxlength = '20';

/*---leaderboard---*/
config.leaderboardPageOneChunk = 10;
config.leaderboardAddOnPageChunk = 10;

/*---SetReminder---*/
config.SetReminderTitle = "setReminder_title"
config.SetReminderDesc = "setReminder_text"
config.SetReminderTimezone = 'CET'
config.SetReminderAddress = 'FRANCE'
config.SetReminderStart = new Date('Thu June 20 2019 23:30:00 GMT+2 (Central European Time)')
config.SetReminderEnd = new Date('Sun June 22 2019 16:00:00 GMT+2 (Central European Time)')
config.knockoutStageDate = 'Thu June 20 2019 23:30:00';//'20 June 23:30 (CET)'

config.shreGenerateImage = (config.ACCOUNTS_API_BASE + 'Share/user/{{guid}}/{{gamedayid}}/generate');
config.dwnGenerateImage = (config.ACCOUNTS_API_BASE + 'Share/user/{{guid}}/{{gamedayid}}/download?phaseId={{phaseId}}&scenario={{scenario}}&LangCode={{LangCode}}&backdoor=shaktiman');
config.shreGenerateImageResult = (config.ACCOUNTS_API_BASE + 'Unviel/share');
config.dwnGenerateImageResult = (config.ACCOUNTS_API_BASE + 'Unviel/download?formationId={{formationId}}&backdoor=shaktiman');
config.BrowserDetect = {
    Android: function () {
        return !!navigator.userAgent.match(/Android/i)
    },
    BlackBerry: function () {
        return !!navigator.userAgent.match(/BlackBerry/i)
    },
    iOS: function () {
        return !!navigator.userAgent.match(/iPhone|iPad|iPod/i)
    },
    iPAD: function () {
        return !!navigator.userAgent.match(/iPad/i)
    },
    Windows: function () {
        return !!navigator.userAgent.match(/IEMobile/i)
    },
    Windows_surface: function () {
        return !(!navigator.userAgent.match(/Trident/i) || !navigator.userAgent.match(/Tablet/i))
    },
    any: function () {
        return config.BrowserDetect.Android() || config.BrowserDetect.BlackBerry() || config.BrowserDetect.iOS() || config.BrowserDetect.Windows()
    },
    ie9: function () {
        return !!navigator.userAgent.match(/MSIE 9.0/i)
    },
    ie10: function () {
        return !!navigator.userAgent.match(/MSIE 10.0/i)
    },
    ie: function () {
        return !(!navigator.userAgent.match(/MSIE/i) && !navigator.userAgent.match(/Trident/i))
    },
    FF: function () {
        return "undefined" != typeof InstallTrigger
    },
    safari: function () {
        var ua = navigator.userAgent.toLowerCase();
        if (ua.indexOf('safari') != -1) {
            if (ua.indexOf('crios') > -1) {
                return false; // Chrome
            } else {
                return true; // Safari
            }
        }
        // return !!navigator.userAgent.match(/Safari/i)
    }
}
config.enumGameMode = {
    live: 0,
    maintenance: 1,
    comingSoon: 2
}
config.enumGameStatus = {
    open: 1,
    locked: 2,
    finished: 3
}

config.enumStaticUrls = {
    home: 'home',
    createTeam: 'create-team',
    faq: 'faq',
    contact: 'contact',
    rules: 'rules',
    prizes: 'prizes',
    terms: 'terms',
    result: 'result'
};

config.timeoutForToast = 10000;
config.scrollTop = 45;
config.redirectDefault = `/home`;
config.cookie = {
    gdprCookie: 'GDPR_ACCEPT',
    loginSaveCookie: 'LOGIN_SAVE'
};
config.enumBusters = {
    team: 'TEAM_BUSTER',

}

config.enumPosition = {
    'goalkeepers': 1,
    'defenders': 2,
    'midfielders': 3,
    'forwards': 4
};
config.enumMode = {
    open: 1,
    switch: 2
};
config.enumPositionText = {};
config.enumPositionText[config.enumPosition.goalkeepers] = 'goalkeepers';//(window.TRANS && window.TRANS.goalkeeper ? window.TRANS.goalkeeper : 'Goalkeeper');
config.enumPositionText[config.enumPosition.defenders] = 'defenders';//(window.TRANS && window.TRANS.defender ? window.TRANS.defender : 'Defender');
config.enumPositionText[config.enumPosition.midfielders] = 'midfielders';//(window.TRANS && window.TRANS.midfielder ? window.TRANS.midfielder : 'Midfielder');
config.enumPositionText[config.enumPosition.forwards] = 'forwards';
config.enumPositionTextFormationChange = {};
config.enumPositionTextFormationChange[config.enumPosition.goalkeepers] = 'goalkeepers';//(window.TRANS && window.TRANS.goalkeeper ? window.TRANS.goalkeeper : 'Goalkeeper');
config.enumPositionTextFormationChange[config.enumPosition.defenders] = 'defenders';//(window.TRANS && window.TRANS.defender ? window.TRANS.defender : 'Defender');
config.enumPositionTextFormationChange[config.enumPosition.midfielders] = 'midfielder';//(window.TRANS && window.TRANS.midfielder ? window.TRANS.midfielder : 'Midfielder');
config.enumPositionTextFormationChange[config.enumPosition.forwards] = 'forward';
config.enumPositionError = {};
config.enumPositionError[config.enumPosition.goalkeepers] = 'gkQuotaError';//(window.TRANS && window.TRANS.goalkeeper ? window.TRANS.goalkeeper : 'Goalkeeper');
config.enumPositionError[config.enumPosition.defenders] = 'defQuotaError';//(window.TRANS && window.TRANS.defender ? window.TRANS.defender : 'Defender');
config.enumPositionError[config.enumPosition.midfielders] = 'midQuotaError';//(window.TRANS && window.TRANS.midfielder ? window.TRANS.midfielder : 'Midfielder');
config.enumPositionError[config.enumPosition.forwards] = 'fwdQuotaError';
config.enumRenderPlayerType = { default: 1, formationChange: 2 };
config.tempSelectedPlayerCookieName = 'si-tempSelectedPlayerCookie';
config.formationId = 'si-formationId';
config.redirectToRegister = 'https://login.laczynaspilka.pl';
config.redirectToLogin = '/login?cid=customauth';

config.enumEventPage = {
    teamSubmit: { action: 'successful_team_submission', category: 'Successful team submission', label: 'Successful team submission' },
    fbShare: { action: 'fb_share', category: 'Facebook share', label: 'Facebook share' },
    twShare: { action: 'tw_share', category: 'Tweeter share', label: 'Tweeter share' },
    dwShare: { action: 'dw_share', category: 'Download Image', label: 'Download Image' },
    fbShareResult: { action: 'fb_share_unveil', category: 'Facebook share unveil', label: 'Facebook share unveil' },
    twShareResult: { action: 'tw_share_unveil', category: 'Tweeter share unveil', label: 'Tweeter share unveil' },
    dwShareResult: { action: 'dw_share_unveil', category: 'Download Image unveil', label: 'Download Image unveil' },
    answer: { action: 'answer', category: 'Answer', label: 'Answer' },
}

export default config;