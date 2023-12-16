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

namespace FinalProject
{
    public class AcademicRecord
    {
        public int id { get; set; }
        public int student_id { get; set; }
        public decimal gpa { get; set; }
        public string status { get; set; }
        public int year { get; set; }
    }


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AcademicHistory : ContentPage
	{
        private Students _selectedStudent;
        private ObservableCollection<AcademicRecord> academicHistoryList;
        private const string academicHistory_read = "http://192.168.100.86/PDC60_api/academichistory-read.php";


        public AcademicHistory (Students selectedStudent)
		{
			InitializeComponent ();
            _selectedStudent = selectedStudent;
            BindingContext = _selectedStudent;
            academicHistoryList = new ObservableCollection<AcademicRecord>();
            LoadAcademicHistory();


        }

        public Students SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                if (_selectedStudent != value)
                {
                    _selectedStudent = value;
                    OnPropertyChanged(nameof(SelectedStudent));
                    BindingContext = this;
                }
            }
        }

        private async void LoadAcademicHistory()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(academicHistory_read);
                var academicData = JsonConvert.DeserializeObject<List<AcademicRecord>>(response);

                
                var filteredAcademicHistory = academicData.Where(record => record.student_id == _selectedStudent.id);

                foreach (var record in filteredAcademicHistory)
                {
                    var frame = new Frame
                    {
                        Padding = new Thickness(20, 10),
                        BackgroundColor = Color.FromHex("#F3F3F3")
                    };

                    var tapGesture = new TapGestureRecognizer();
   

                    tapGesture.Tapped += async (sender, e) =>
                    {
                        await Navigation.PushAsync(new UpdateAcademicHistoryPage(_selectedStudent, record.id));
                    };
                    frame.GestureRecognizers.Add(tapGesture);


                    var grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });

                    var yearLabel = new Label
                    {
                        Text = record.year.ToString(),
                        FontFamily = "MontserratM",
                        TextColor = Color.Black,
                        FontSize = 14,
                     
                    };

                    var gpaLabel = new Label
                    {
                        Text = record.gpa.ToString(),
                        FontFamily = "MontserratM",
                        TextColor = Color.Black,
                        FontSize = 14,

                    };

                    var remarksLabel = new Label
                    {
                        Text = record.status,
                        FontFamily = "MontserratM",
                        TextColor = Color.Black,
                        FontSize = 14,
                        
                    };

                    grid.Children.Add(yearLabel, 0, 0);
                    grid.Children.Add(gpaLabel, 1, 0);
                    grid.Children.Add(remarksLabel, 2, 0);

                    frame.Content = grid;

       


                    academicHistoryStackLayout.Children.Add(frame);
                }
            }
        }

        private void OnRefreshAcademicHistory(object sender, EventArgs e)
        {

            academicHistoryStackLayout.Children.Clear();
            LoadAcademicHistory();
        }


        //private async Task DeleteAcademicRecord(int recordId)
        //{
        //    // Show a confirmation dialog
        //    bool userConfirmed = await DisplayAlert("Confirmation", "Are you sure you want to delete this Academic Record?", "Yes", "No");

        //    // Check user's response
        //    if (userConfirmed)
        //    {
        //        // User confirmed, delay for 2 seconds before deleting
        //        await Task.Delay(2000);

        //        var deleteApiUrl = $"http://192.168.100.86/PDC60_api/academichistory-delete.php?id={recordId}";

        //        using (var httpClient = new HttpClient())
        //        {
        //            var deleteResponse = await httpClient.DeleteAsync(deleteApiUrl);

        //            if (deleteResponse.IsSuccessStatusCode)
        //            {
        //                // Record deleted successfully
        //                await DisplayAlert("Success", "Academic Record deleted successfully.", "OK");
        //            }
        //            else
        //            {
        //                // Handle the error case
        //                await DisplayAlert("Error", "Failed to delete Academic Record.", "OK");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // User cancelled the deletion
        //        await DisplayAlert("Cancelled", "Deletion cancelled.", "OK");
        //    }
        //}





        private async void OnAddAcademicHistory(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAcademicHistoryPage(_selectedStudent));
        }

        //private async void OnUpdateAcademicHistory(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new UpdateAcademicHistoryPage(_selectedStudent, int academicHistoryId));
        //}




    }
}

