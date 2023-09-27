import React, { Component } from 'react';
import config from './../../common/config';
import { Redirect } from 'react-router-dom';

class NoMatch extends Component {
    constructor(props) {
        super(props);

    }

    render() {
        const sRedirect = config.redirectDefault.replace('{{lang}}', config.CURRENT_LANGUAGE);
        console.log('NoMatch = ', this.props);
        return <Redirect to={sRedirect} push />
    }
}

export default NoMatch;