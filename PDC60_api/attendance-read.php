<?php
include('servercon.php');

$sql = "SELECT id, student_id, student_name, attendance_date, status FROM attendance";
$result = $dbconnect->query($sql);

if (!$result) {
    // Handle the query error
    echo json_encode(array("error" => "Query error: " . $dbconnect->error));
} else {
    // Fetch data
    $attendanceData = array();
    while ($row = $result->fetch_assoc()) {
        $attendanceData[] = $row;
    }

    // Close the database connection
    $dbconnect->close();

    // Return JSON response
    header('Content-Type: application/json');

    if (!empty($attendanceData)) {
        echo json_encode($attendanceData);
    } else {
        // If no data found
        echo json_encode(array("message" => "No attendance data found."));
    }
}
?>
