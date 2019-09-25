<?php
session_start();
if (empty($_SESSION['authToken'])) {
    header("Location: /login/");
    die();
}
