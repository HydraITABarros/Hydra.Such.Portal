import React from 'react';
import { withTheme } from 'styled-components';
import { renderToString } from 'react-dom/server';
import Highlighter from "react-highlight-words";
import { Text } from 'components';
import moment from 'moment';
import { identity } from 'rxjs';

const Default = (props) => {

	moment.locale("pt");

	var date = moment(props.value).format('DD MMM YYYY');

	var searchWords = [];
	if (props.searchValues && props.searchValues.length > 0) {
		props.searchValues.map((value) => {
			value.split(" ").map((val) => {
				searchWords.push(val);
				var months = ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
				if (val.length > 2) {
					months.map((i) => {
						if (i.substring(0, val.length).toLowerCase() == val.toLowerCase()) {
							searchWords.push(i.substring(0, 3).toLowerCase());
						}
					});
				}
			});
		});
	}
	return (
		<Text p
			style={{ color: props.theme.palette.primary.default, overflow: 'hidden', textOverflow: 'ellipsis' }}
			data-html={true}
			data-tip={
				renderToString(<Highlighter searchWords={searchWords} autoEscape={true} textToHighlight={date.toString() || ""}></Highlighter>)
			} >
			<Highlighter searchWords={searchWords} autoEscape={true} textToHighlight={date.toString() || ""}></Highlighter>
		</Text>
	)
};

export default withTheme(Default);
