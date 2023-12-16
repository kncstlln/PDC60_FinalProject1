<?php
include('servercon.php');
header('Content-Type: application/json');
header("Access-Control-Allow-Origin: *");

// Assuming data is sent as JSON
$json_data = file_get_contents("php://input");
$data = json_decode($json_data);

// Check if the required parameters are set
if (
    isset($data->student_id) && isset($data->name) &&
    isset($data->gpa) && isset($data->status) &&
    isset($data->year)
) {
    // Retrieve data from JSON
    $studentId = $data->student_id;
    $studentName = $data->name;
    $gpa = $data->gpa;
    $status = $data->status;
    $year = $data->year;

    // Using prepared statement to prevent SQL injection
    $stmt = $dbconnect->prepare("INSERT INTO academic_history (student_id, name, gpa, status, year) VALUES (?, ?, ?, ?, ?)");
    $stmt->bind_param("issss", $studentId, $studentName, $gpa, $status, $year);

    if ($stmt->execute()) {
        $response = array(
            "success" => true,
            "message" => "Academic history added successfully"
        );
        echo json_encode($response);
    } else {
        $response = array(
            "error" => true,
            "message" => "Error adding academic history: " . $stmt->error
        );
        echo json_encode($response);
    }

    $stmt->close();
} else {
    $response = array(
        "error" => true,
        "message" => "Missing parameters for adding academic history"
    );
    echo json_encode($response);
}

// Close the database connection
$dbconnect->close();
?>
