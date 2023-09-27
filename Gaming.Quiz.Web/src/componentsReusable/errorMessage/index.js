import React from 'react';

export default class ErrorMessage extends React.Component {
    constructor(props) {
        super(props)
        this.state = { isHide: this.props.isHide, sMessage: this.props.sMessage };
    }
    render() {
        var self = this;
        if (!self.props.isHide) {
            return (<div className={"si-frm-errMsg "}>
                <span>{self.props.sMessage}</span>
            </div>);
        } else {
            return null;
        }
    }
}