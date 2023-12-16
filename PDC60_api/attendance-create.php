<?php
include('servercon.php');
header('Content-Type: application/json');
header("Access-Control-Allow-Origin: *");

// Assuming data is sent as JSON
$json_data = file_get_contents("php://input");
$data = json_decode($json_data);

// Check if the required parameters are set
if (
    isset($data->student_name) && isset($data->student_id) &&
    isset($data->attendance_date) && isset($data->status)
) {
    // Retrieve data from JSON
    $studentName = $data->student_name;
    $studentId = $data->student_id;
    $attendanceDate = $data->attendance_date;
    $status = $data->status;

    // Using prepared statement to prevent SQL injection
    $stmt = $dbconnect->prepare("INSERT INTO attendance (student_name, student_id, attendance_date, status) VALUES (?, ?, ?, ?)");
    $stmt->bind_param("siss", $studentName, $studentId, $attendanceDate, $status);

    if ($stmt->execute()) {
        $response = array(
            "success" => true,
            "message" => "Attendance added successfully"
        );
        echo json_encode($response);
    } else {
        $response = array(
            "error" => true,
            "message" => "Error adding attendance: " . $stmt->error
        );
        echo json_encode($response);
    }

    $stmt->close();
} else {
    $response = array(
        "error" => true,
        "message" => "Missing parameters for adding attendance"
    );
    echo json_encode($response);
}

// Close the database connection
$dbconnect->close();
?>
