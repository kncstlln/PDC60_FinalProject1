<?php
include('servercon.php');

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: DELETE");

// Ensure the request method is DELETE
if ($_SERVER['REQUEST_METHOD'] === 'DELETE') {
    // Extract 'id' parameter from the URL
    $pid = isset($_GET["id"]) ? intval($_GET["id"]) : 0;

    if ($pid > 0) {
        // Using prepared statement to prevent SQL injection
        $stmt = $dbconnect->prepare("DELETE FROM attendance WHERE id = ?");
        $stmt->bind_param("i", $pid);

        // Execute the SQL statement
        if ($stmt->execute()) {
            $response = array(
                "success" => true,
                "message" => "Attendance record deleted successfully."
            );
            echo json_encode($response);
        } else {
            // Log the detailed error on the server
            error_log("Error deleting record: " . $stmt->error);

            $response = array(
                "error" => true,
                "message" => "Failed to delete Attendance record."
            );
            echo json_encode($response);
        }

        $stmt->close();
    } else {
        $response = array(
            "error" => true,
            "message" => "Invalid or missing 'id' parameter for deleting attendance."
        );
        echo json_encode($response);
    }
} else {
    $response = array(
        "error" => true,
        "message" => "Invalid request method. Only DELETE requests are allowed."
    );
    echo json_encode($response);
}

// Close the database connection
$dbconnect->close();
?>
