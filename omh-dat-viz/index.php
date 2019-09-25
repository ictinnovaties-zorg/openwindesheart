<?php
require 'includes/sentry.php';

if ($_SESSION['authToken'] == 'admin') {
    header("Location: /admin/");
    die();
}
$token = $_SESSION['authToken'];
// Create a stream
$opts = array(
    'http' => array(
        'method' => "GET",
        'header' => "Accept: application/json\r\n" .
            "Authorization: Bearer " . $token . "\r\n"
    )
);
$startDate = gmdate("Y-m-d\TH:i:s\Z", strtotime("-3 day"));
$context = stream_context_create($opts);


// Open the file using the HTTP headers set above
@$file = file_get_contents('http://insulinepredictionplatform.com:8085/v1.0.M1/dataPoints?schema_namespace=omh&schema_name=heart-rate&schema_version=1.0&created_on_or_after='.$startDate.'&limit=864', false, $context);

$jsonarray = json_decode($file, TRUE);
?>

<html>

<head>
    <link rel="stylesheet" type="text/css" href="includes/style.css">
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css'>
    <link rel="stylesheet" type="text/css" href="bower_components/plottable/plottable.css">
    <link rel="stylesheet" type="text/css" href="dist/omh-web-visualizations-all.css">
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css"
          integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
 
</head>

<body>

<button type="submit" style="position: absolute; top: 20px; right: 20px"
        onclick="window.location.href='/login/logout.php';" class="btn btn-danger">Logout
</button>
<div class="page">
    <div class="chart-data-selector">
        <h1>Welkom <?= $jsonarray[0]["header"]["user_id"] ?>!</h1>
        <p>
            Hier zie je een visualisatie van je hartslag van de afgelopen tijd:
        </p>
    </div>

    <div class="demo-chart-container">
        <div class="loading-message">
            Loading data...
        </div>
        <svg class="demo-chart">
        </svg>
    </div>
    <div class="additional-information">
        <div class="datapoint-details">
        </div>
    </div>
</div>
</body>
<script>var currentData = JSON.parse('<?= $file ?>');</script>
<script src="bower_components/d3/d3.min.js"></script>
<script src="bower_components/d3-tip/index.js"></script>
<script src="bower_components/plottable/plottable.js"></script>
<script src="bower_components/moment/moment.js"></script>
<script src="dist/omh-web-visualizations-all.js"></script>
<script src= "includes/visualisatieOMHDataPoints.js"></script>


</html>
