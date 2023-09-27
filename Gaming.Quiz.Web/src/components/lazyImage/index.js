import React, {Component} from 'react';

export default class LazyImage extends Component {
    constructor(props) {
        super(props);
        this.state = {loaded: false}
    }

    componentWillUnmount() {
        if (this.img && this.state.loaded === false) {
            this.img.onload = function () {
            };
        }
    }

    handleLoad() {
        this.setState({loaded: true})
    }

    componentDidMount() {
        this.img = new Image();
        this.img.onload = this.handleLoad.bind(this);
        this.img.src = this.props.src
    }

    render() {
        return this.state.loaded ? <img src={this.props.src} alt={this.props.alt || ''}/> : this.props.placeholder
    }
}