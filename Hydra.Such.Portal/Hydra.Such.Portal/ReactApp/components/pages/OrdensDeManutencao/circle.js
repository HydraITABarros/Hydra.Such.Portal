import React from 'react'
import draw from './draw.js'

class Circle extends React.Component {

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        draw(this.props);
    }

    componentDidUpdate(prevProps) {
        draw(this.props);
    }

    render() {
        return (
            <div>
                <div className="viz" />
            </div>
        )
    }
};

export default Circle;