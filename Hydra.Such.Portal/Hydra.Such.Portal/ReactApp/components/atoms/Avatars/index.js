import React from 'react';
import styled, { css, theme } from 'styled-components';
import _theme from '../../themes/default';
import Color from 'color';
import { Text } from 'components';
import  Button  from '../Button';

const avatarPhoto = css`&& {
    display: inline-block;
    overflow: hidden;
    img {
        width: 40px;        
    }
}
`
const avatarLetter = css`&& {
    display: inline-block;
    background-color: ${props => props.color};
    h3 {    
        margin: 0;
        line-height: 18px;
        font-weight: 400;
        color: ${_theme.palette.white};
    }
}
`
const AvatarGroup = styled.div` 
    display: inline-block;
    > Button {
        margin-left: -1.5em;
        &:first-child {
            margin-left: 0;
        }
    }
`

const AvatarLetter = styled(Button)`${avatarLetter}`;
const AvatarPhoto = styled(Button)`${avatarPhoto}`;

const Avatars = ({ ...props }) => {
    if (props.photo) {
        return <AvatarPhoto round {...props} ><img src={props.src}>{props.children}</img></AvatarPhoto>
    } else if (props.letter) {
        return <AvatarLetter round {...props} ><Text h3>{props.children}</Text></AvatarLetter>
    }
    return <AvatarPhoto {...props} />
}

// export default Avatars;
export {
    Avatars, AvatarGroup
}