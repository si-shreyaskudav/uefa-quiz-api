import React, { Component } from 'react';
import { connect } from 'react-redux';
import config from './../../common/config'; 

class Stage extends Component {
    constructor(props) {
        super(props);
        this.state = {

        };
    }

    render() {
        let self = this;

        return <div className="si-main__container">
            Step3
        </div>
    }
}
const mapStateToProps = (state) => {
    return {

    }
}
const mapDispatchToProps = (dispatch) => {
    return {

    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Stage);