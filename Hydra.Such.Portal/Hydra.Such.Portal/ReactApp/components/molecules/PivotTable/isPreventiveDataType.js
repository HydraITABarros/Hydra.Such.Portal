import React, { Component } from 'react';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { renderToString } from 'react-dom/server';
import Highlighter from "react-highlight-words";
import { Button, Text, Icon, Circle, Wrapper, OmDatePicker, CheckBox, Input, Avatars, Modal, Tooltip, Spacer, Breadcrumb } from 'components';

const IsPerventive = (props) => {
	return (
		<div style={{ height: '1px', position: 'relative', top: '-21px' }}>
			<Text b
				style={{ color: props.theme.palette.primary.default }}
				data-html={true}
				data-tip={
					renderToString(<Highlighter searchWords={props.searchValues} autoEscape={true} textToHighlight={props.value ? props.value.toString() : ""}></Highlighter>)
				} >
				{props.value ?
					<Icon preventiva style={{ fontSize: '26px', top: "9px", position: "relative" }} data-tip={'Preventiva'} /> :
					<Icon curativa style={{ fontSize: '26px', top: "9px", position: "relative", color: props.theme.palette.secondary.default }} data-tip={'Curativa'} />
				}
			</Text>
		</div>
	)
};

export default withTheme(IsPerventive);
