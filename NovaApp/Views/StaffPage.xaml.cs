using Microsoft.UI.Xaml.Controls.Primitives;
using NovaApp.Popups;
using Mopups.Services;
using NovaApp.ViewModels;
using System.Diagnostics;
using NovaApp.Models;

namespace NovaApp.Views
{
    public partial class StaffPage : ContentView

    {
        private StaffViewModel _viewModel;

        public StaffPage()
        {
            InitializeComponent();
            _viewModel = new StaffViewModel(new Services.RestService());
            BindingContext = _viewModel;


            LoadStaff(); // Call the LoadStaff method in the constructor
        }

        public async void LoadStaff()
        {
            await _viewModel.FetchAllStaff();
        }

        private void OnAddNewClicked(object sender, EventArgs e)
        {
            MopupService.Instance.PushAsync(new AddStaff());
        }

        private void OnEditTapped(object sender, EventArgs e)
        {
            var staffId = _viewModel.SelectedStaff?.id;
            if (staffId != null)
            {
                MopupService.Instance.PushAsync(new UpdateStaff(staffId));
            }

        }


        private async void OnCardTapped(object sender, EventArgs e)
        {
            var staffId = ((sender as Image)?.BindingContext as Staff)?.id; // Adjust property name accordingly
            if (staffId != null)
            {
                await _viewModel.FetchStaffById(staffId);

            }
        }


        void OnTextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
        {
            var filteredList = new List<Staff>(_viewModel.StaffList);

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                // Reset the CollectionView's ItemsSource to the original list
                MyCollectionViews.ItemsSource = _viewModel.StaffList;
            }
            else
            {
                // Filter the items based on the search text
                filteredList = filteredList
                    .Where(client => client.username.ToLower().Contains(e.NewTextValue.ToLower()))
                    .ToList();

                // If there's only one in the filtered list, add a blank card
                if (filteredList.Count == 1)
                {
                    var blankCard = new Staff(); // Create a new instance with default or empty values
                    filteredList.Add(blankCard);
                }

                MyCollectionViews.ItemsSource = filteredList;
            }
        }


    }
} 
