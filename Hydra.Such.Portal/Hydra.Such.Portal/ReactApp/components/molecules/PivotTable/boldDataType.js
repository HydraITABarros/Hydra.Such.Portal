import React, { Component } from 'react';
import styled, { css, theme, injectGlobal, withTheme } from 'styled-components';
import { renderToString } from 'react-dom/server';
import Highlighter from "react-highlight-words";
import { Button, Text, Icon, Circle, Wrapper, OmDatePicker, CheckBox, Input, Avatars, Modal, Tooltip, Spacer, Breadcrumb } from 'components';

const Bold = (props) => {

	return (
		<Text b
			style={{ color: props.theme.palette.primary.default }}
			data-html={true}
			data-tip={
				renderToString(<Highlighter searchWords={props.searchValues} autoEscape={true} textToHighlight={props.value || ""}></Highlighter>)
			} >
			<Highlighter searchWords={props.searchValues} autoEscape={true} textToHighlight={props.value || ""}></Highlighter>
		</Text>
	)
};

export default withTheme(Bold);
