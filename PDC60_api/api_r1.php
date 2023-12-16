<?php
header("Content-Type: application/json");

include('servercon.php');

$stmt = $dbconnect->prepare("SELECT id, name, section, level, age, email, mobile_num FROM students_info WHERE level = 1");

$stmt->execute();
$result = $stmt->get_result();
$outp = $result->fetch_all(MYSQLI_ASSOC);

echo json_encode($outp);
?>
