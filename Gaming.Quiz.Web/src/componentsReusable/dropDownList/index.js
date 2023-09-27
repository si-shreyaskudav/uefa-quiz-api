import React from 'react';
import config from './../../common/config';
import { Scrollbars } from 'react-custom-scrollbars';
import OutsideNotifier from './../outsideNotifier';

class DropDownList extends React.Component {

    constructor(props) {
        super(props);
        this.state = { isChallengeFilter: false };
    }
    onSelectFilter = (isChallengeFilter) => {
        this.setState(function (state, props) {
            return {
                isChallengeFilter: (typeof isChallengeFilter === 'undefined' ? !(this.state.isChallengeFilter) : isChallengeFilter)
            }
        });
    }
    onChangeFilter = (index) => {
        let self = this;
        if (self.props.jChallengeFilter !== index) {
            self.props.changeFormation(index);
        }
    }
    renderFilter = (isWeb, arrChallengeFilter, jChallengeFilter, onChangeFilter, isChallengeFilter) => {
        let self = this;
        let resp = null;
        const { sId, sName, isFormation } = this.props;
        const objSelected = this.getSelectedValue();
        let jSelectedId = 0;
        if (objSelected) {
            jSelectedId = objSelected[sId];
        }
        resp = arrChallengeFilter && arrChallengeFilter.map((element, index) => {
            let sClass = (jChallengeFilter === index ? "active" : "");
            let sName1 = element[sName];
            let jId = element[sId];
            //if (jId !== -1) {
            if (isWeb) {
                if (isFormation && jSelectedId === jId) {
                    return null;
                }
                return (<li key={index} className={sClass} onClick={() => onChangeFilter(index)}>
                    <span>{sName1}</span>
                </li>);
            }
            else {
                return (<option key={index} value={index}>{sName1}</option>);
            }
            //}
            //else {
            //    return null;
            //}
        });
        const compRet = isChallengeFilter ? <OutsideNotifier
            handler={() => self.onSelectFilter(false)}>
            <ul>
                {resp}
            </ul>
        </OutsideNotifier> : null;
        return compRet;
    }
    getSelectedValue = (arrChallengeFilterP, jChallengeFilterP) => {
        let arrChallengeFilter = arrChallengeFilterP ? arrChallengeFilterP : this.props.arrChallengeFilter;
        let jChallengeFilter = (jChallengeFilterP || jChallengeFilterP === 0) ? jChallengeFilterP : this.props.jChallengeFilter;
        const objSelected = arrChallengeFilter && arrChallengeFilter.length ? arrChallengeFilter[jChallengeFilter] : null;
        return {
            ...objSelected
        }
    }
    componentDidMount() {
    }
    render() {
        const trans = config.trans;
        const { onChangeFilter, onSelectFilter, getSelectedValue, props, state } = this;
        const { sLabel, arrChallengeFilter, sId, sName, jChallengeFilter } = props;
        const { isChallengeFilter, } = state;
        const objSelected = getSelectedValue();
        const sClassFilter = isChallengeFilter ? 'active' : '';
        let sLabelT = sLabel ? (sLabel + ':') : '';
        return (<div className="si-frmation__drpDwn">
            <div className="si-form__control">
                <div className={("si-drpDwn " + sClassFilter)} onClick={() => onSelectFilter()} >
                    {/* <!-- active --> */}
                    <div className="si-drpDwn__value">
                        <span>{sLabel}: <em>{objSelected && objSelected[sName]}</em></span>

                    </div>
                    <div className="si-drpDwn__list">
                        {
                            this.renderFilter(true, arrChallengeFilter, jChallengeFilter, onChangeFilter, isChallengeFilter)
                        }
                    </div>
                </div>
            </div>
        </div>);

    }
}

export default DropDownList;
