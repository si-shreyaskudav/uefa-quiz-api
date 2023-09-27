import React, { Component } from 'react';
import { connect } from 'react-redux';
import { gdprPost } from './../../redux/actions/appActions';
import config from './../../common/config';
import app from './../../common/app';
import core from './../../common/core';
import Trans from './../../componentsReusable/trans';

class GDPR extends Component {
    constructor(props) {
        super(props);
        this.state = {
            date: Date.now()
        };
    }
    onClose = () => {
        const { gdprPost } = this.props;
        const gdprPostAPI = gdprPost();
        gdprPostAPI.then((resp) => {
            const retVal = resp && resp.action.payload.data && resp.action.payload.data.meta && resp.action.payload.data.meta.retVal;
            if (retVal === 1) {
                core.setLocalStorageValue(config.cookie.gdprCookie, true);
                this.setState((state, props) => {
                    return {
                        date: Date.now()
                    }
                });
                if (window.gtag) {
                    window.gtag('config', window.gtagId);
                }
            }
        });
    }
    render() {
        let self = this;
        const { onClose } = self;
        const propsCLose = {
            onClick: onClose,
            className: 'si-popup__close'
        };
        const objGDPRCookie = core.getLocalStorageValue(config.cookie.gdprCookie);
        let sClass = 'in';
        if (objGDPRCookie) {
            sClass = 'out';
        }
        return <div className={"si-cookies__wrap si-fade " + sClass}>
            <div className="si-main__container">
                <div {...propsCLose}></div>
                <Trans sKey={'gdprDesc'} /> <a href="https://media.laczynaspilka.pl/files/pzphp/Y2Y7MDA_/6ea02534e4f13e01c2b3911e83768537.pdf" target="_blank"><Trans sKey={'privacyPolicy'} /></a>.
      </div>
        </div>
    }
}
const mapStateToProps = (state) => {
    return {

    }
}
const mapDispatchToProps = (dispatch) => {
    return {
        gdprPost: () => dispatch(gdprPost()),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(GDPR);