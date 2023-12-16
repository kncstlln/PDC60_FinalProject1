<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST");

$data = json_decode(file_get_contents("php://input"), true);

$name = $data["name"];
$section = $data["section"];
$level = $data["level"];
$age = $data["age"];
$email = $data["email"];
$mobilenum = $data["mobile_num"];

include('servercon.php');

$query = "INSERT INTO students_info (name, section, level, age, email, mobile_num) 
          VALUES ('".$name."', '".$section."', '".$level."', '".$age."', '".$email."','".$mobilenum."')";

if (mysqli_query($dbconnect, $query)) {
    echo json_encode(array("message" => "Inserted Successfully", "status" => true));
} else {
    echo json_encode(array("message" => "Not Inserted", "status" => false));
}

?>
