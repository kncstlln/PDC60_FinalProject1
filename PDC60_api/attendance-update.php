<?php
include 'servercon.php';
header('Content-Type: application/json');
header("Access-Control-Allow-Origin: *");

if ($_SERVER['REQUEST_METHOD'] === 'PUT') {
    // Assuming data is sent as JSON
    $data = json_decode(file_get_contents("php://input"));

    $id = $data->id;
    $status = $data->status;

    // Using prepared statement to prevent SQL injection
    $stmt = $dbconnect->prepare("UPDATE attendance SET status = ? WHERE id = ?");
    $stmt->bind_param("si", $status, $id);

    if ($stmt->execute()) {
        echo json_encode(array("message" => "Attendance record updated successfully."));
    } else {
        echo json_encode(array("message" => "Error: " . $stmt->error));
    }

    $stmt->close();
}

$dbconnect->close();
?>
