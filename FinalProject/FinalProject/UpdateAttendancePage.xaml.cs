using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

using System.Net.Http;

using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace FinalProject
{
    public partial class UpdateAttendancePage : ContentPage
    {
        public delegate void StatusUpdatedHandler(string status);
        public event StatusUpdatedHandler StatusUpdated;
        public int AttendanceRecordId { get; set; }
        private Students _selectedStudent;
        public const string ApiUrl = "http://192.168.100.86/PDC60_api/attendance-update.php";
        public const string Delete = "http://192.168.100.86/PDC60_api/attendance-delete.php";

        public UpdateAttendancePage(Students selectedStudent, int attendanceRecordId)
        {
            InitializeComponent();
            _selectedStudent = selectedStudent;
            BindingContext = _selectedStudent;
            AttendanceRecordId = attendanceRecordId;
        }

        private async void OnPresentClicked(object sender, EventArgs e)
        {
            await RecordAttendance("Present");
        }

        private async void OnAbsentClicked(object sender, EventArgs e)
        {
            await RecordAttendance("Absent");
        }

        private async Task RecordAttendance(string status)
        {
            try
            {
                var attendanceRecord = new
                {
                    id = AttendanceRecordId,
                    status
                };


                string jsonData = JsonConvert.SerializeObject(attendanceRecord);

                using (HttpClient client = new HttpClient())
                {
                    var updateApiUrl = $"{ApiUrl}?id={AttendanceRecordId}";

                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(updateApiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {

                        // Trigger the StatusUpdated event
                        StatusUpdated?.Invoke(status);

                        await DisplayAlert("Success", $"Attendance updated successfully as {status}", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        Console.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                        await DisplayAlert("Error", "Error updating attendance", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Display a confirmation alert
                var confirm = await DisplayAlert("Confirm Deletion", "Are you sure you want to delete this record?", "Yes", "No");

                if (confirm)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string deleteUrlWithId = $"{Delete}?id={AttendanceRecordId}";

                        // Create the HTTP request with the DELETE method
                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Delete,
                            RequestUri = new Uri(deleteUrlWithId),
                        };

                        // Send the DELETE request to the API endpoint
                        HttpResponseMessage response = await client.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                            // Handle success
                            await DisplayAlert("Success", "Data deleted successfully.", "OK");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            // Handle failure
                            await DisplayAlert("Error", $"Error: {response.StatusCode}", "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                await DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
            }
        }

    }
}
