<?php
session_start();
if (isset($_GET["error"])) {
    $error = $_GET["error"];
}

if (isset($_POST["username"]) && isset($_POST["password"])) {
    $username = $_POST["username"];
    $password = $_POST["password"];

    $url = 'http://insulinepredictionplatform.com:8082/oauth/token';
    $data = array('grant_type' => 'password', 'username' => $username, 'password' => $password);
    $options = array(
        'http' => array(
            'header' => "Accept: application/json\r\n" .
                "Authorization: Basic dGVzdENsaWVudDp0ZXN0Q2xpZW50U2VjcmV0\r\n",
            'method' => 'POST',
            'content' => http_build_query($data)
        )
    );

    $context = stream_context_create($options);
    @$result = file_get_contents($url, false, $context);

    if ($http_response_header[0] == 'HTTP/1.1 200 OK') {
        $token = json_decode($result)->access_token;
        $_SESSION["authToken"] = $token;
        header("Location: /");
        die();
    } else {
        header("Location: /login?error=auth");
        die();
    }
}

?>

<html lang="en">
<head>
    <title>Login</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css"
          integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
</head>
<body>
<div class="container-fluid">
    <div class="alert alert-danger" role="alert" <?= $error == 'auth' ? true : false ?>hidden>
        Bad credentials
    </div>
    <form method="post">
        <div class="form-group">
            <label for="username">Username</label>
            <input class="form-control" name="username" id="username" placeholder="Enter username">
        </div>
        <div class="form-group">
            <label for="password">Password</label>
            <input type="password" class="form-control" name="password" id="password" placeholder="Password">
        </div>
        <button type="submit" class="btn btn-primary">Login</button>
    </form>
</div>
</body>
</html>