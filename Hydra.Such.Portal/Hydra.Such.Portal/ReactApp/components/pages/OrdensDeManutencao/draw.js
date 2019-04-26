import * as d3 from 'd3';

const draw = (props) => {
    const w = Math.max(document.documentElement.clientWidth, window.innerWidth || 0);
    const h = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);
    d3.select('.viz').append('svg')
        .attr('height', h)
        .attr('width', w)
        .attr('id', 'svg-viz')

    const bubbles = props.shapes;
    const max = d3.max(bubbles);
    const radiusScale = d3.scaleSqrt().domain([0, max]).range([0, max]);

    const simulation = d3.forceSimulation()
        .force('x', d3.forceX(w / 3).strength(0.05))
        .force('y', d3.forceY(h / 3).strength(0.05))
        .force('charge', d3.forceManyBody().strength(-1300))
        .force('collide', d3.forceCollide(d => radiusScale(d.number) + 1))

    const circles = d3.select('#svg-viz').selectAll('circle')
        .data(props.shapes)
        .enter()
        .append('svg:circle')
        .attr('r', d => d.width / 2 + "px")
        .style('fill', (d) => d.color ? d.color : 'purple')

    simulation.nodes(props.shapes)
        .on('tick', ticked)

    function ticked() {
        circles
            .attr('cx', d => d.x)
            .attr('cy', d => d.y)
    }



    var colors = {
        'pink': '#E1499A',
        'yellow': '#f0ff08',
        'green': '#47e495'
    };

    var color = colors.pink;

    var radius = 100;
    var border = 5;
    var padding = 30;
    var startPercent = 0;
    var endPercent = 0.35;


    var twoPi = Math.PI * 2;
    var formatPercent = d3.format('.0%');
    var boxSize = (radius + padding) * 2;


    var count = Math.abs((endPercent - startPercent) / 0.01);
    var step = endPercent < startPercent ? -0.01 : 0.01;

    var arc = d3.svg.arc()
        .startAngle(0)
        .innerRadius(radius)
        .outerRadius(radius - border);

    var parent = d3.select('div#content');

    var svg = parent.append('svg')
        .attr('width', boxSize)
        .attr('height', boxSize);

    var defs = svg.append('defs');

    var filter = defs.append('filter')
        .attr('id', 'blur');

    filter.append('feGaussianBlur')
        .attr('in', 'SourceGraphic')
        .attr('stdDeviation', '7');

    var g = svg.append('g')
        .attr('transform', 'translate(' + boxSize / 2 + ',' + boxSize / 2 + ')');

    var meter = g.append('g')
        .attr('class', 'progress-meter');

    meter.append('path')
        .attr('class', 'background')
        .attr('fill', '#ccc')
        .attr('fill-opacity', 0.5)
        .attr('d', arc.endAngle(twoPi));

    var foreground = meter.append('path')
        .attr('class', 'foreground')
        .attr('fill', color)
        .attr('fill-opacity', 1)
        .attr('stroke', color)
        .attr('stroke-width', 5)
        .attr('stroke-opacity', 1)
        .attr('filter', 'url(#blur)');

    var front = meter.append('path')
        .attr('class', 'foreground')
        .attr('fill', color)
        .attr('fill-opacity', 1);

    var numberText = meter.append('text')
        .attr('fill', '#fff')
        .attr('text-anchor', 'middle')
        .attr('dy', '.35em');

    function updateProgress(progress) {
        foreground.attr('d', arc.endAngle(twoPi * progress));
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
export default draw;