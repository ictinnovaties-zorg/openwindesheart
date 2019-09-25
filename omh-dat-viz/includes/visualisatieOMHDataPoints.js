/*=================================================================
=            Variable defninitions and initializations            =
=================================================================*/

var chart = null;
//data is reused if url is not changed
//var currentData = JSON.parse('<?= $file ?>');
var currentUrl = '';

var loadingMessage = d3.select('.loading-message');
var datapointDetails = d3.select('.datapoint-details');
var clickInteraction = null;
var clickInteractionComponent = null;

var options = {
    'userInterface': {
        'axes': {
            'yAxis': {
                'visible': true
            },
            'xAxis': {
                'visible': true
            }
        },
        'tooltips': {
            'timeFormat': 'DD-MM-YYYY HH:mm',
        }
    },
    'measures': {
        'heart_rate': {
            'xValueQuantization': {
                'period': OMHWebVisualizations.QUANTIZE_MINUTE,
            },
            'range': {'min': 0, 'max': 250}
        }
    }
};


/*===================================================
=            Example UI helper functions            =
===================================================*/

var hideLoadingMessage = function () {
    loadingMessage.classed('hidden', true);
};

var updateLoadingMessage = function (amountLoaded) {
    loadingMessage.classed('hidden', false);
    loadingMessage.text('Loading data... ' + Math.round(amountLoaded * 100) + '%');
};

var showLoadingError = function (error) {
    loadingMessage.classed('hidden', false);
    loadingMessage.html('There was an error while trying to load the data: <pre>' + JSON.stringify(error) + '</pre>');
};

var hideChart = function () {
    d3.select('.demo-chart').classed('hidden', true);
};

var showChart = function () {
    d3.select('.demo-chart').classed('hidden', false);
};

var disableUI = function () {
    d3.select('.measure-select').property('disabled', true);
    d3.select('.update-button').property('disabled', true);
};
var enableUI = function () {
    d3.select('.measure-select').property('disabled', false);
    d3.select('.update-button').property('disabled', false);
};

var updateDatapointDetails = function (datum) {
    var replacer = function (key, value) {
        if (key === 'groupName') {
            return undefined;
        } else {
            return value;
        }
    };
    datapointDetails.html('<h3>Data Point Details</h3> ' + JSON.stringify(datum, replacer, 4));
};

var showDatapointDetailsMessage = function (message) {
    datapointDetails.html('<h3>Data Point Details</h3> ' + message);
};


/*=====  End of Example UI helper functions  ======*/


/*====================================================
=            Chart construction functions            =
====================================================*/

var customizeChartComponents = function (components) {

    //move any label overlayed on the bottom right
    //of the chart up to the top left
    var plots = components.plots;

    showDatapointDetailsMessage('Choose a measure that displays as a scatter plot to see details here.');

    plots.forEach(function (component) {

        if (component instanceof Plottable.Components.Label &&
            component.yAlignment() === 'bottom' &&
            component.xAlignment() === 'right') {

            component.yAlignment('top');
            component.xAlignment('left');

        }
        if (component instanceof Plottable.Plots.Scatter && component.datasets().length > 0) {

            scatterPlot = component;

            if (!clickInteraction) {
                clickInteraction = new Plottable.Interactions.Click()
                    .onClick(function (point) {
                        var nearestEntity;
                        try {
                            nearestEntity = scatterPlot.entityNearest(point);
                            updateDatapointDetails(nearestEntity.datum.omhDatum);
                        } catch (e) {
                            return;
                        }
                    });
            }

            clickInteraction.attachTo(scatterPlot);
            clickInteractionComponent = scatterPlot;

            showDatapointDetailsMessage('Click on a point to see details here...');

        }

    });

};

var makeChartForUrl = function (url, element, measureList, configOptions) {

    var makeChart = function (data) {

        //if data is from shimmer, the points are in an array called 'body'
        if (data.hasOwnProperty('body')) {
            data = data.body;
        }

        if (chart) {
            chart.destroy();
            if (clickInteraction && clickInteractionComponent) {
                clickInteraction.detachFrom(clickInteractionComponent);
            }
        }

        //builds a new plottable chart
        chart = new OMHWebVisualizations.Chart(data, element, measureList, configOptions);

        if (chart.initialized) {

            //customizes the chart's components
            customizeChartComponents(chart.getComponents());

            //renders the chart to an svg element
            showChart();
            hideLoadingMessage();
            chart.renderTo(element.select("svg").node());

            currentData = data;
            currentUrl = url;


        } else {

            hideChart();
            showLoadingError('Chart could not be initialized with the arguments supplied.');

        }

        enableUI();

    };

    disableUI();

    if (url === currentUrl && currentData !== null) {

        makeChart(currentData);

    } else {

        hideChart();

        var xhr = d3.json(url)
            .on("progress", function () {
                updateLoadingMessage(d3.event.loaded / d3.event.total);
            })
            .on("load", function (json) {
                makeChart(json);
            })
            .on("error", function (error) {
                hideChart();
                showLoadingError(error);
            })
            .get();

    }


};


var parseInputAndMakeChart = function () {

    // Make the chart
    makeChartForUrl("", d3.select('.demo-chart-container'), "heart_rate", options);
};





d3.select('select').on('change', parseInputAndMakeChart);
d3.select('.update-button').on('click', parseInputAndMakeChart);


parseInputAndMakeChart();