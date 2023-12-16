<?php
include('servercon.php');

$sql = "SELECT id, student_id, name, gpa, status, year FROM academic_history";
$result = $dbconnect->query($sql);

if ($result->num_rows > 0) {
    // Fetch data
    $academicHistoryData = array();
    while ($row = $result->fetch_assoc()) {
        $academicHistoryData[] = $row;
    }

    // Close the database connection
    $dbconnect->close();

    // Return JSON response
    header('Content-Type: application/json');
    echo json_encode($academicHistoryData);
} else {
    // If no data found
    echo json_encode(array("message" => "No academic history data found."));
}
