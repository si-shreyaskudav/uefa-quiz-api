import config from './../../common/config';
const data = [
    {

        "id": 0,
        "name": "home",
        "transKey": "menuHome",
        "titleTransKey": "homeTab",
        "isRemove": false,
        "isHideMenu": false,
        "isMaster": false,
        "link": config.enumStaticUrls.home,
        "linkMenu": config.enumStaticUrls.home,
        "isAuthorize": 0,
        "isEndOfTour": 0,
        "isActive": (match, location) => {
            if (location.pathname.indexOf(`/${config.enumStaticUrls.home}`) > -1) {
                return true;
            }
        },
    },
    {
        "id": 3,
        "name": "myTeam",
        "transKey": "menuMyTeam",
        "titleTransKey": "myteamTab",
        "isRemove": false,
        "isHideMenu": false,
        "isMaster": false,
        "link": config.enumStaticUrls.createTeam,
        "linkMenu": config.enumStaticUrls.createTeam,
        "isActive": (match, location) => {
            if (location.pathname.indexOf(`/${config.enumStaticUrls.createTeam}`) > -1) {
                return true;
            }
        },
        "isAuthorize": 0,
        "isEndOfTour": 0
    }, 
];
export default data