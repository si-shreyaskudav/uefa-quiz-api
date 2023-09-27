import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import config from './../../common/config';
import app from './../../common/app';
import core from './../../common/core';
import Trans from './../../componentsReusable/trans';
import apiError from './../../constants/apiError';

import {  playersFetchData, addPlayer, removePlayer, } from './../../redux/actions/appActions';

class MyTeam extends Component {
    constructor(props) {
        super(props);
        this.state = {
            goAhead: false, 
        };

    }
     

    render() {
        const self = this; 
        return <Fragment>
             CREATE TEAM
        </Fragment>
    }

    componentWillReceiveProps(nextProps) {
        const self = this;
         
    }

    componentDidMount() {
        const self = this;
         

    }

    componentWillUnmount() {

    }

}
const mapStateToProps = (state) => {
    return { 
        arrSelectedPlayer: state.appData.arrSelectedPlayer,
        arrPlayers: state.appData.arrPlayers, 
    }
}
const mapDispatchToProps = (dispatch) => {
    return { 
        addPlayer: (payload) => dispatch(addPlayer(payload)),
        removePlayer: (payload) => dispatch(removePlayer(payload)), 
        playersFetchData: () => dispatch(playersFetchData()), 
    };
};
const TeamWrapper = connect(mapStateToProps, mapDispatchToProps)(MyTeam);
export default TeamWrapper;