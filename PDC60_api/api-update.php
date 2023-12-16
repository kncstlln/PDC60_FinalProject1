<?php

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: PUT");
header("Access-Control-Allow-Headers: Access-Control-Allow-Headers,Content-Type, Access-Control-Allow-Methods, Authorization");

$data = json_decode(file_get_contents("php://input"), true);

$sid = $data["id"];
$sname = $data["name"];
$ssection = $data["section"];
$slevel = $data["level"];
$sage = $data["age"];
$semail = $data["email"];
$smobile = $data["mobile_num"];

include('servercon.php');

$query = "UPDATE students_info SET name= '".$sname."', section= '".$ssection."', level= '".$slevel."'  ,age= '".$sage."' , email= '".$semail."',  mobile_num= '".$smobile."' WHERE id ='".$sid."' ";

if(mysqli_query($dbconnect, $query) or die("Update Query Failed"))
{	
    echo json_encode(array("message" => "Update Successfully", "status" => true));	
}
else
{	
    echo json_encode(array("message" => "Failed", "status" => false));	
}

?>
