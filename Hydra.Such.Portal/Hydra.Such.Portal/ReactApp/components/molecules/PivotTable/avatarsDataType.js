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
import functions from '../../../helpers/functions';

const {AvatarGroup} = Avatars;
const avatarColors = [
    "#990000",
    "#33DDEE",
    "#5533DD",
    "#339900",
    "#cc00cc"
];

const AvatarsType = (props) => {
    //return (<div></div>);
    return (
        <AvatarGroup style={{top: "-7px", position: "relative", height: '1px', fontSize: '1px'}}>
            {props.value && props.value.map((_item, i) => (<Avatars.Avatars key={i} letter color={avatarColors[i]}
                                                                            data-tip={_item.nome}>{functions.getInitials(_item.nome)}</Avatars.Avatars>))}
        </AvatarGroup>
    )
};

export default withTheme(AvatarsType);
