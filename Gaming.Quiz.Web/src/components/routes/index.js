import React, { Fragment, Suspense, lazy } from 'react';
import { BrowserRouter, Switch, Route, Redirect } from 'react-router-dom';
import arrMenu from './../../constants/menuRoutes';
import config from './../../common/config';
import app from './../../common/app';
import core from './../../common/core';
import Loader from './../loader';
// import Tnc from './../statictnc';

 
const Home = lazy(() => import(/* webpackChunkName: 'home' */'./../home'));
const MyTeam = lazy(() => import(/* webpackChunkName: 'myTeam' */'../myTeam'));
 
const NoMatch = lazy(() => import(/* webpackChunkName: 'noMatch' */'./../noMatch'));
 
const GDPR = lazy(() => import(/* webpackChunkName: 'gdpr' */'./../gdpr'));


let location = window.location.href;
let ObjRoute = {
};
ObjRoute[config.enumStaticUrls.home] = Home;
ObjRoute[config.enumStaticUrls.createTeam] = MyTeam;
// ObjRoute[config.enumStaticUrls.rules] = Rules; 
// ObjRoute[config.enumStaticUrls.terms] = Tnc;
// ObjRoute[config.enumStaticUrls.faq] = FAQ;
// ObjRoute[config.enumStaticUrls.contact] = Contact;
// ObjRoute[config.enumStaticUrls.result] = MyResult;
const doDefaultNavigation = (props) => {
    const path = props.sPath;
    const routeObject = props.routeObject;
    const DefaultComponent = props.DefaultComponent; 
    const trans = props.trans;
    const isGameStatus = props.isGameStatus;
    const cookie = app.getLoginCookie();
    let isAuthorize = routeObject.validator ? routeObject.validator(routeObject, cookie, isGameStatus) : true;
    let sTitle = '';

    document.title = trans && trans[routeObject.titleTransKey] ? trans[routeObject.titleTransKey] : routeObject.titleTransKey;

    if ((!isAuthorize)) {
        const sRedirect = config.redirectDefault.replace('{{lang}}', config.CURRENT_LANGUAGE);
        return (<Redirect to={sRedirect} />);
    }

    sTitle = document.title;
    config.CURRENT_PATH = path;
    let CompReturn = null;

    if (!CompReturn) {
        CompReturn = (<DefaultComponent {...props} />);
    }
    //app.googleAnalyticsTracking(sTitle, (config.URL_BASE + config.CURRENT_PATH + window.location.search).toLowerCase());
    //console.log((config.URL_BASE + config.CURRENT_PATH + window.location.search).toLowerCase());
    return CompReturn;
};

class Routes extends React.Component {
    constructor() {
        super();  
        this.state = {
            myRoutes: [],
            isDateUpdate: null,
            popupNode: null,
            jMode: config.enumGameMode.live,
        }; 


        let eventValidators = app.eventValidators;
        let eventValidatorsLoggedInWithBracket = app.eventValidatorsLoggedInWithBracket;
        let eventValidatorsLocked = app.eventValidatorsLocked;
        let eventValidatorsLoggedIn = app.eventValidatorsLoggedIn;

        this.routes = [

        ];

        //login menu add kiya hai
        for (let jN = 0; jN < (arrMenu || []).length; jN++) {
            let row = arrMenu[jN];
            row.validator = eventValidators;
            if (row.isEndOfTour === 1) {
                row.validator = eventValidatorsLocked;
            }
            else if (row.isEndOfTour === 2) {
                row.validator = eventValidatorsLoggedIn;
            }

            this.routes.push({
                ...row,
                path: (row.link ? row.link.toLowerCase() : ''),
                component: ObjRoute[row.link ? row.link.toLowerCase() : ''],
                validator: row.validator
            });
        }
    }
     
    
    render() {
        const self = this;
        let myRoutes = self.routes.map((el, index) => {
            let component = el.component;
            let sPath = el.path;
            let propsRoute = {
                DefaultComponent: component,
                sPath: sPath,
                routeObject: el, 
                ...self.props
            }
            //console.log((config.URL_BASE + el.path))

            return <Route key={index} path={(config.URL_BASE + el.path)}
                render={(props) => {
                    return (doDefaultNavigation({ ...props, ...propsRoute }))
                }}
            />
        });

        return (<BrowserRouter>
            <Suspense fallback={<Loader />}> 
                <Switch >
                    {myRoutes}
                    <Route component={NoMatch} />
                </Switch>
                <GDPR />
            </Suspense>
        </BrowserRouter>);
    }; 

}

export default Routes;