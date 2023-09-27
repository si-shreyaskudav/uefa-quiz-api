import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { transFetchData, constraintsFetchData, formationFetchData, loginPost, showHidePopup } from './../../redux/actions/appActions';
import config from './../../common/config';
import app from './../../common/app';
import core from './../../common/core';
import Routes from './../routes';
import Loader from './../loader';
import ErrorBoundary from './../errorBoundary';

class Wrapper extends Component {
    constructor(props) {
        super(props);
        this.state = {
            goAhead: false,
            objInvite: {}
        };
    }

    showHidePopup = () => {
        const self = this;
        let nodePopup = null;
        const objPopup = self.props.objPopup;
        if (objPopup) {
            var clonedElementWithMoreProps = React.cloneElement(
                (objPopup.component ? objPopup.component : <div />),
                { isShow: objPopup.isShow }
            );
            nodePopup = clonedElementWithMoreProps;
        }
        return nodePopup;
    }
    render() {
        let self = this;
        let props = self.props;

        const nodeBody = (self.state.goAhead) ?
            <Routes {...props} />
            :
            null;
        let nodePopup = self.showHidePopup();
        return <ErrorBoundary>
            <div className="si-main">
                {
                    nodeBody
                }
                {
                    nodePopup
                }
                <Loader />
            </div>
        </ErrorBoundary>
        // return <Routes trans={trans} />;
    }
    componentDidMount() {
        const self = this;
        // const transPromise = self.props.transFetchData();
        // const constraintsPromise = self.props.constraintsFetchData();
        // const formationsPromise = self.props.formationFetchData();
        // Promise.all([transPromise, constraintsPromise, formationsPromise]).then((resp) => {
        //     if (resp && resp.length && resp[0].value.data && resp[0].value.data.value) {
        //         self.setState({ goAhead: true });
        //     }
        // }).catch(reason => {
        //     console.log(reason);
        // });
        // const objGDPRCookie = core.getLocalStorageValue(config.cookie.gdprCookie);
        // if (objGDPRCookie && window.gtag && window.gtagId) {
        //     window.gtag('config', window.gtagId);
        // }
        self.setState({ goAhead: true });
    }

}
const mapStateToProps = (state) => {
    return {
        trans: state.appData.trans,
        objPopup: state.appData.objPopup, 
    }
}
const mapDispatchToProps = (dispatch) => {
    return {
        transFetchData: () => dispatch(transFetchData()),
        constraintsFetchData: () => dispatch(constraintsFetchData()), 
        showHidePopup: (payload) => dispatch(showHidePopup(payload)),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Wrapper);