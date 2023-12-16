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

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentList4 : ContentPage
    {
        private const string retrieve = "http://192.168.100.86/PDC60_api/api_r4.php";
        private const string search = "http://192.168.100.86/PDC60_api/api-search.php";
        private ObservableCollection<Students> _studentsList;
        private ObservableCollection<Students> _originalStudentsList;
        private HttpClient _Client = new HttpClient();

        public StudentList4()
        {
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            var content = await _Client.GetStringAsync(retrieve);
            var students = JsonConvert.DeserializeObject<List<Students>>(content);

            _studentsList = new ObservableCollection<Students>(students);
            _originalStudentsList = new ObservableCollection<Students>(students);

            studentFilter.ItemsSource = _studentsList;
            base.OnAppearing();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_originalStudentsList == null)
                return;

            // Perform the search based on the entered text
            var searchTerm = e.NewTextValue.ToLower();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Reset the list if the search term is empty
                _studentsList = new ObservableCollection<Students>(_originalStudentsList);
            }
            else
            {
                // Perform a case-insensitive search
                _studentsList = new ObservableCollection<Students>(
                    _originalStudentsList.Where(s => s.name.ToLower().Contains(searchTerm))
                );
            }

            // Update the ListView with the filtered list
            studentFilter.ItemsSource = _studentsList;

            // Show or hide the "No results found" label based on the search results
            NoResultsLabel.IsVisible = _studentsList.Count == 0;
        }

        private async void StudentProfile_Tapped(Object sender, EventArgs e)
        {
            var frame = sender as Frame;
            var selectedStudent = frame?.BindingContext as Students;

            if (selectedStudent != null)
            {
                await Navigation.PushAsync(new StudentProfile(selectedStudent));
            }
        }

        private async void AddStudentPage_Clicked(Object sender, EventArgs e)
        {

            await Navigation.PushAsync(new AddStudentPage4());

        }

        private async void UpdateStudentPage_Clicked(Object sender, EventArgs e)
        {

            var frame = sender as Frame;
            var selectedStudent = frame?.BindingContext as Students;

            if (selectedStudent != null)
            {
                await Navigation.PushAsync(new UpdateStudentPage(selectedStudent));
            }

        }

    }
}