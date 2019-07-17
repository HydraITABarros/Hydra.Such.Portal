import React from 'react';
import { withTheme } from 'styled-components';
import { renderToString } from 'react-dom/server';
import Highlighter from "react-highlight-words";
import { Text } from 'components';

const Default = (props) => {
	return (
		<Text p
			style={{ color: props.theme.palette.primary.default, overflow: 'hidden', textOverflow: 'ellipsis' }}
			data-html={true}
			data-tip={
				renderToString(<Highlighter searchWords={props.searchValues} autoEscape={true} textToHighlight={props.value || ""}></Highlighter>)
			} >
			<Highlighter searchWords={props.searchValues} autoEscape={true} textToHighlight={props.value || ""}></Highlighter>
		</Text>
	)
};

export default withTheme(Default);
