import config from './../../common/config';

export const transAPI = config.API_BASE + 'static-assets/Translations/{{lang}}.json';//'constraints/translations_1_{{lang}}.json';
export const constraintsAPI = config.API_BASE + 'static-assets/limits/constraints_1.json';//'constraints/constraints_1.json';
export const formationAPI = config.API_BASE + 'static-assets/Composition/composition_1.json';//'constraints/composition_1.json';
export const playersAPI = config.API_BASE + 'services/feeds/players/playerdetails_1_en.json';//'constraints/players.json';
export const playersCardAPI = config.API_BASE + 'services/feeds/players/playercarddetails_{{Id}}_1_en.json';//'constraints/players.json';

//services
export const getUserTeamUrl = config.ACCOUNTS_API_BASE + 'Gameplay/user/{{guid}}/team';
export const getRevealedTeamUrl = config.ACCOUNTS_API_BASE + 'Unviel/team';
export const postUserTeamAPI = config.ACCOUNTS_API_BASE + 'Gameplay/user/{{guid}}/saveteam';
export const postLoginAPI = config.ACCOUNTS_API_BASE + 'Session/user/login';
export const gdprCookieAPI = config.ACCOUNTS_API_BASE + 'CookiePolicy/user/accept';

