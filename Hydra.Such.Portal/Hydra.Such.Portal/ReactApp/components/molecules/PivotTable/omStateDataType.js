import React, {Component} from 'react';
import styled, {css, theme, injectGlobal, withTheme} from 'styled-components';
import {renderToString} from 'react-dom/server';
import Highlighter from "react-highlight-words";
import {
    Button,
    Text,
    Icon,
    Circle,
    Wrapper,
    OmDatePicker,
    CheckBox,
    Input,
    Avatars,
    Modal,
    Tooltip,
    Spacer,
    Breadcrumb
} from 'components';

const IsPerventive = (props) => {
    return (
        <div style={{height: '1px', position: 'relative', top: '-21px'}}>
            <Text b
                  style={{color: props.theme.palette.primary.default}}
                  data-html={true}
                  data-tip={
                      renderToString(<Highlighter searchWords={props.searchValues} autoEscape={true}
                                                  textToHighlight={props.value ? props.value.toString() : ""}></Highlighter>)
                  }>
                {props.value == 0 || props.value == null ?
                    <span></span> :
                    props.value == 1 ?
                        <Icon approved style={{fontSize: '20px', top: "9px", position: "relative"}}/> :
                        props.value == 2 ?
                            <Icon comments style={{fontSize: '20px', top: "9px", position: "relative"}}/> :
                            props.value == 3 ?
                                <Icon decline style={{fontSize: '20px', top: "9px", position: "relative"}}/> :
                                props.value == 4 ?
                                    <Icon observacoes style={{fontSize: '20px', top: "9px", position: "relative"}}/> :
                                    <span></span>
                }
            </Text>
        </div>
    )
};

export default withTheme(IsPerventive);
