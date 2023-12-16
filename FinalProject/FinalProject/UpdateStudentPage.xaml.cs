using System;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading;
using System.Security.Cryptography;
using System.Xml.Linq;
using Xamarin.Essentials;

namespace FinalProject
{	
	public partial class UpdateStudentPage : ContentPage
	{
        public delegate void StatusUpdatedHandler(string status);
        public event StatusUpdatedHandler StatusUpdated;
        private Students _selectedStudent;
        private const string url_update = "http://192.168.100.86/PDC60_api/api-update.php";
        private const string Delete = "http://192.168.100.86/PDC60_api/api-delete.php";

        private HttpClient _Client = new HttpClient();
   

        public UpdateStudentPage(Students selectedStudent)
        {
            InitializeComponent();
            _selectedStudent = selectedStudent;
            BindingContext = _selectedStudent;
        }

        private async void OnUpdateStudentClicked(Object sender, EventArgs e)
        {
            try
            {
                // Ensure _selectedStudent is not null
                if (_selectedStudent != null)
                {
                    var studentId = _selectedStudent.id;
                    var name = nameEntry.Text;
                    var age = ageEntry.Text;
                    var section = sectionEntry.Text;
                    var levelText = levelEntry.Text;
                    var mobileNum = mobile_numEntry.Text;
                    var email = emailEntry.Text;

                    // Validate that the entered level is within the allowed range
                    if (int.TryParse(levelText, out int level) && (level >= 1 && level <= 4))
                    {
                        var studentInfo = new
                        {
                            id = studentId,
                            name,
                            age,
                            section,
                            level,
                            mobile_num = mobileNum,
                            email
                        };

                        string jsonData = JsonConvert.SerializeObject(studentInfo);

                        using (HttpClient client = new HttpClient())
                        {
                            var updateApiUrl = $"{url_update}?id={studentId}";

                            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                            var response = await client.PutAsync(updateApiUrl, content);

                            if (response.IsSuccessStatusCode)
                            {
                                await DisplayAlert("Success", "Student information updated successfully.", "OK");
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                Console.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                                await DisplayAlert("Error", "Error updating student information", "OK");
                            }
                        }
                    }
                    else
                    {
                        // Display an error message if the entered level is outside the allowed range
                        await DisplayAlert("Error", "Level must be between 1 and 4.", "OK");
                    }
                }
                else
                {
                    // Handle the case where _selectedStudent is null
                    Console.WriteLine("Error: _selectedStudent is null");
                    await DisplayAlert("Error", "Unable to update student information. Selected student is null.", "OK");
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
                        var studentId = _selectedStudent.id;

                        string deleteUrlWithId = $"{Delete}?id={studentId}";

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


            private async void StudentProfile_Tapped(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StudentProfile(_selectedStudent));
        }
    }
}

