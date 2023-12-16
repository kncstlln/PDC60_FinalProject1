<?php
include 'servercon.php';
header('Content-Type: application/json');
header("Access-Control-Allow-Origin: *");

if ($_SERVER['REQUEST_METHOD'] === 'PUT') {
    $data = json_decode(file_get_contents("php://input"));

    $id = $data->id;
    $gpa = $data->gpa;
    $status = $data->status;
    $year = $data->year;

    $stmt = $dbconnect->prepare("UPDATE academic_history SET gpa = ?, status = ?, year = ? WHERE id = ?");
    if (!$stmt) {
        echo json_encode(array("message" => "Error in preparing statement: " . $dbconnect->error));
    }

    $stmt->bind_param("dssi", $gpa, $status, $year, $id);
    if (!$stmt) {
        echo json_encode(array("message" => "Error in binding parameters: " . $stmt->error));
    }

    if ($stmt->execute()) {
        echo json_encode(array("message" => "Academic History updated successfully."));
    } else {
        echo json_encode(array("message" => "Error: " . $stmt->error));
    }

    $stmt->close();
}

$dbconnect->close();
?>
