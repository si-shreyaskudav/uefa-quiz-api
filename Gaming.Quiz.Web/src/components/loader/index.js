import React, { Component } from 'react';
import { connect } from 'react-redux';
import config from './../../common/config';

class Loader extends Component {
    constructor(props) {
        super(props);
        this.state = {

        };
    }

    render() {
        let self = this; 
        return <div className="si-pg-loader-mainWrap">
      <div className="si-pg-loader"> 
      </div>
    </div>
    }
}
export default Loader;