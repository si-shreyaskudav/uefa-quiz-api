import React, { Component } from 'react';
import core from './../../common/core';

export default class Popup extends Component {
    constructor(props) {
        super(props);
        this.state = { isRemove: false };
    }
    componentWillReceiveProps(nextProps) {
        if (!nextProps.isShow && this.props.isShow) {
            core.addClassToElement(document, 'body', 'bodyNoScroll', true);
        }
    }

    componentDidMount() {
        let isRemove = true;

        setTimeout(() => {
            this.setState((state, props) => {
                return { isRemove: isRemove };
            });
            //     common.addClassToElement(document, '.si-popup__wrap.si-fadeRight', 'out', isRemove);
        }, 100);
        core.addClassToElement(document, 'body', 'bodyNoScroll', false);
    }
    render() {
        const isRemove = this.state.isRemove;
        let sClass = ' out';
        const { isShow, className } = this.props;
        if (isRemove && isShow) {
            sClass = 'in';
        }
        return <div className={("si-popup__wrap si-fade " + className + " " + sClass)}>
            {
                this.props.children
            }
        </div>;
    }
    componentWillUnmount() {
        core.addClassToElement(document, 'body', 'bodyNoScroll', true);
    }

}