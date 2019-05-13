import React from 'react';
import styled, { css, theme } from 'styled-components';
import _theme from '../../themes/default';
import Color from 'color';
// import { Text, Button } from 'components';
import Text from '../Text';
import Button from '../Button';
import _ from 'lodash'
import PropTypes from 'prop-types';

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
    &:hover {
        background-color: ${props => props.color};
    }
}
`

const AvatarGroup = styled.div` 
    display: inline-block;
    > button {
        margin-left: -1.5em;
        &:first-child {
            margin-left: 0;
        }
    }
`

const AvatarLetter = styled(Button)`${avatarLetter}`;

const AvatarPhoto = styled(Button)`${avatarPhoto}`;

const Avatars = (props) => {
    if (props.photo) {
        return <AvatarPhoto round {..._.omit(props, ['photo', 'round'])} ><img src={props.src}>{props.children}</img></AvatarPhoto>
    } else if (props.letter) {
        return <AvatarLetter round {..._.omit(props, ['letter', 'round'])} ><Text h3>{props.children}</Text></AvatarLetter>
    }
    return <AvatarPhoto {...props} />
}
Avatars.propTypes = {
    photo: PropTypes.bool,
    letter: PropTypes.bool
};

Avatars.Avatars = Avatars;
Avatars.AvatarGroup = AvatarGroup;

export default Avatars;
// export {
//     Avatars,
//     AvatarGroup
// } 