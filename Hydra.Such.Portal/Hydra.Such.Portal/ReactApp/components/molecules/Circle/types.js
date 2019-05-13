import PropTypes from 'prop-types';
import _theme from '../../themes/default';

export const defaultProps = {
    width: 190,
    strokeColor: _theme.palette.secondary.default,
    strokeLinecap: 'round',
    trailColor: _theme.palette.primary.dark,
    showTotal: true
};

const mixedType = PropTypes.oneOfType([PropTypes.number, PropTypes.string]);

export const propTypes = {
    width: PropTypes.number,
    strokeValue: PropTypes.number,
    strokeIcon: PropTypes.object,
    strokeColor: PropTypes.string,
    strokeLinecap: PropTypes.oneOf(['butt', 'round', 'square']),
    trailValue: PropTypes.number,
    trailIcon: PropTypes.object,
    trailColor: PropTypes.string,
    showTotal: PropTypes.bool,
    label: PropTypes.string
};
