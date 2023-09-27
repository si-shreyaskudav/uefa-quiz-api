import React, { Component } from 'react';
import { connect } from 'react-redux';

class Trans extends Component {
    constructor(props) {
        super(props);
    }
    render() {
        const { trans, sKey, sValue, replaceKey, replaceVal } = this.props;
        let sTextKey = trans[sKey];
        let sTextValue = sValue ? sValue : sKey;
        let sResp = (sTextKey ? sTextKey : sTextValue);
        if (replaceKey) {
            sResp = sResp.replace(replaceKey, replaceVal);
        }
        return sResp;
    }
}
const mapStateToProps = (state) => {
    return {
        trans: (state.appData.trans)
    }
}
const mapDispatchToProps = (dispatch) => {
    return {

    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Trans);