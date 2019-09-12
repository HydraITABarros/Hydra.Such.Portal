import React from 'react'
import styled, { css, theme } from 'styled-components';
import MuiSelect from '@material-ui/core/Select';
import { Input, Icon } from 'components';

const styles = css`&& {
        text-align: left;
        font-family: ${props => props.theme.fonts.primary};
        font-style: normal;
        font-weight: 400;
        font-size: 14px;
        line-height: 18px;
        color: ${props => props.color || props.theme.palette.primary.default};
        svg {
            top: 0;
        }
        [class*="icon-"] {
            position: absolute;
            top: 0;
            right: 0;
            font-size: 24px;
            padding: 9px;
            pointer-events: none;
        }
    }
`

const DefaultSelect = styled(MuiSelect)`${styles}`;

class Select extends React.Component {

	constructor(props) {
		super(props);
		this.state = { value: '' };
		this.state.className = props.className || '';
		this.onChangeHandler = this.onChangeHandler.bind(this);
	}

	onChangeHandler(event) {
		this.setState({ value: event.target.value }, () => {

			if (typeof this.props.onChange == 'function') {
				this.props.onChange(event);
			}
		});
	}

	render() {
		return (
			<DefaultSelect IconComponent={() => <Icon arrow-down />} value={this.state.value || 0} input={<Input classes={this.state.className} />}  {...this.props} onChange={this.onChangeHandler} >{this.props.children}</DefaultSelect>
		);
	}
}

export default Select;
