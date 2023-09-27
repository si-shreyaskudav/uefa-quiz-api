import React from 'react';
import { NavLink } from 'react-router-dom';
//import $ from 'jquery';
import config from './../../common/config';
import app from './../../common/app';
import core from './../../common/core';
import arrMenu from './../../constants/menuRoutes';
import Trans from './../../componentsReusable/trans';

export default class CompMenu extends React.Component {
    constructor(props) {
        super(props);
        let eventValidators = app.eventValidators;
        let eventValidatorsLoggedInWithBracket = app.eventValidatorsLoggedInWithBracket;
        let eventValidatorsLocked = app.eventValidatorsLocked;
        let eventValidatorsLoggedIn = app.eventValidatorsLoggedIn;
        config.IS_WEBVIEW = app.isSetWebview();
        let menus = [];
        for (let jN = 0; jN < (arrMenu || []).length; jN++) {
            let row = arrMenu[jN];
            row.validator = eventValidators;

            if (row.isEndOfTour === 1) {
                row.validator = eventValidatorsLocked;
            }
            else if (row.isEndOfTour === 2) {
                row.validator = eventValidatorsLoggedIn;
            }
            row.link = config.URL_BASE + (row.link ? row.link.toLowerCase() : '');
            menus.push(row);
        }

        this.state = {
            menus: menus,
        };
    }
    renderMenu() {
        let self = this;
        let menus = this.state.menus || [];
        const { isGameStatus } = this.props;
        const cookie = app.getLoginCookie();
        return menus.filter(el => {
            if (el.isRemove || el.isHideMenu) {
                return false;
            }
            return el.validator(el, cookie, isGameStatus);
        }).map(function (element, index) {
            let hide = element.isHideMenu ? '' : '';
            let disable = element.isDisable ? '' : '';
            let isActive = '';//element.isActive({}, window.location) ? 'active' : '';
            let sClass = 'si-menu__list-item' + ' ' + isActive + ' ' + hide + ' ' + disable;
            let sName = element.transKey;
            let menuMarkup = null;
            let onClickTracking = null;
            let onClickMenu = { onClick: self.onClickMenu };
            const sRedirect = config.redirectDefault.replace('{{lang}}', config.CURRENT_LANGUAGE);
            const sLink = (`${config.URL_BASE}${element.linkMenu}`);
            menuMarkup = (<NavLink exact to={sLink} activeClassName={"active"} /*isActive={element.isActive}*/>
                <span>
                    <Trans sKey={sName} />
                </span></NavLink>);
            return (<li key={element.id} className={sClass} {...onClickMenu}>
                {
                    menuMarkup
                }
            </li>);
        });
    }
    onClickMenu = () => {
        this.onClickHambergerMenu();
    }
    onClickHambergerMenu() {
        const { callback } = this.props;
        this.setState((state, props) => {
            return { isShowMenu: !state.isShowMenu };
        }, () => {
            callback && callback(false);
        });
    }
    render() {

        const { isShowMenu } = this.state;
        // if (config.IS_WEBVIEW)
        //     return null;
        return (<ul className="si-nav__list si-nav__list--bg">
            {
                this.renderMenu()
            }
        </ul>);
    }

}