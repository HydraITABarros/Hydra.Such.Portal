// https://github.com/diegohaz/arc/wiki/Styling
import {
    reversePalette
} from 'styled-theme/composer'

const theme = {}

theme.palette = {
    primary:{ 
        default: '#2196f3',
        dark: '#1976d2',
        light: '#71bcf7',
        lightness: '#c2e2fb'
    },
    secondary: ['#c2185b', '#e91e63', '#f06292', '#f8bbd0'],
    alert: ['#ffa000', '#ffc107', '#ffd761', '#ffecb3'],
    search: ['#388e3c', '#4caf50', '#7cc47f', '#c8e6c9'],
    white: ['#fff', '#fff', '#eee'],
    grayscale: ['#212121', '#414141', '#616161', '#9e9e9e', '#bdbdbd', '#e0e0e0', '#eeeeee', '#ffffff', ],
}

theme.reversePalette = reversePalette(theme.palette)

theme.fonts = {
    primary: 'Helvetica Neue, Helvetica, Roboto, sans-serif',
    pre: 'Consolas, Liberation Mono, Menlo, Courier, monospace',
    quote: 'Georgia, serif',
}

theme.sizes = {
}

export default theme