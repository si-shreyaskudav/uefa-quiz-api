import React, { Component } from 'react';
import config from './../../common/config';

const StepHoc = (Comp, props) => {
    return class Stage extends Component {
        constructor(props) {
            super(props);
            this.state = {

            };
        }

        render() {
            let self = this;

            return <div className="">
                Step HOC
                <Comp {...this.props} />
            </div>
        }
    }
}


export default StepHoc;