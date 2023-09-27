import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import config from './../../common/config';
import app from './../../common/app';
import core from './../../common/core';
import Trans from './../../componentsReusable/trans';
import apiError from './../../constants/apiError';

class Home extends Component {
    constructor(props) {
        super(props);
        this.state = {
            goAhead: false
        };
    }
   
    render() {
        let self = this;
        
        return <Fragment>
            HOME
        </Fragment>
    }
    componentDidMount() {
        core.addClassToElement(document, '.si-main', 'si-main--hp', false); 
    }

    componentWillUnmount() {
        core.addClassToElement(document, '.si-main', 'si-main--hp', true);
        core.addClassToElement(document, '.si-main', 'si-main--hp-reveal', true);
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
const HomeWrapper = connect(mapStateToProps, mapDispatchToProps)(Home);
export default HomeWrapper;