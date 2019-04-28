import * as d3 from 'd3';
import ReactTooltip from 'react-tooltip'

export default (el) => {
    var colors = { 'pink': '#E1499A', 'yellow': '#f0ff08', 'green': '#47e495' };

    var color = colors.pink;

    var radius = 100 - 50;
    var border = 9;
    var padding = 30;
    var startPercent = 0;
    var endPercent = 0.35;

    var twoPi = Math.PI * 2;
    var formatPercent = d3.format('.0%');
    var boxSize = (radius + padding) * 2;

    var count = Math.abs((endPercent - startPercent) / 0.01);
    var step = endPercent < startPercent ? -0.01 : 0.01;

    var arc = d3.arc()
        .startAngle(0)
        .innerRadius(radius)
        .outerRadius(radius - border)
        .cornerRadius(20);

    var parent = d3.select(el);

    var svg = parent.append('svg')
        .attr('width', boxSize)
        .attr('height', boxSize);

    var defs = svg.append('defs');

    var g = svg.append('g').attr('transform', 'translate(' + boxSize / 2 + ',' + boxSize / 2 + ')');

    var meter = g.append('g')
        .attr('class', 'progress-meter');

    meter.append('path')
        .attr('class', 'background')
        .attr('fill', '#ccc')
        .attr('fill-opacity', 0.5)
        .attr('data-tip', "123")
        .attr('d', arc.endAngle(twoPi));

    var front = meter.append('path')
        .attr('class', 'foreground')
        .attr('data-tip', "teste tip")
        .attr('fill', color)
        .attr('fill-opacity', 1);

    var numberText = meter.append('text')
        .attr('fill', '#fff')
        .attr('text-anchor', 'middle')
        .attr('dy', '.35em');

    function updateProgress(progress) {
        front.attr('d', arc.endAngle(twoPi * progress));
        numberText.text(formatPercent(progress));
    }

    var progress = startPercent;

    (function loops() {
        updateProgress(progress);
        if (count > 0) {
            count--;
            progress += step;
            setTimeout(loops, 10);
        }
    })();
}