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
	public partial class UpdateAcademicHistoryPage : ContentPage
	{
        public int AcademicHistoryRecordId { get; set; }
        private Students _selectedStudent;
        public const string ApiUrl = "http://192.168.100.86/PDC60_api/academichistory-update.php";
        public const string Delete = "http://192.168.100.86/PDC60_api/academichistory-delete.php";

        public UpdateAcademicHistoryPage(Students selectedStudent, int academicHistoryId)
        {
            InitializeComponent();
            _selectedStudent = selectedStudent;
            BindingContext = _selectedStudent;
            AcademicHistoryRecordId = academicHistoryId;
        }

        private async void SubmitUpdateAcademicHistory(object sender, EventArgs e)
        {
            try
            {
                // Get values from the Entry fields
                decimal gpa = Convert.ToDecimal(gpaEntry.Text);
                string status = (statusPicker.SelectedItem as string);
                int year = Convert.ToInt32(yearEntry.Text);

                // Create an object with the updated data
                var updatedAcademicRecord = new
                {
                    id = AcademicHistoryRecordId,
                    gpa = gpa,
                    status = status,
                    year = year
                };

                // Convert the object to JSON
                string json = JsonConvert.SerializeObject(updatedAcademicRecord);

                // Create the HTTP client
                using (HttpClient client = new HttpClient())
                {
                    // Set the content type to JSON
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Send the PUT request to the API endpoint
                    var response = await client.PutAsync(ApiUrl, content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Show a success message and navigate back
                        await DisplayAlert("Success", "Academic History updated successfully.", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        // Show an error message
                        Debug.WriteLine("Error: " + response.ReasonPhrase);
                        await DisplayAlert("Error", "Failed to update Academic History.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                await DisplayAlert("Error", "An error occurred. Please check your input.", "OK");
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
                        string deleteUrlWithId = $"{Delete}?id={AcademicHistoryRecordId}";

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




        //private async void CancelUpdateAcademicHistory(object sender, EventArgs e)
        //{


    }
}
