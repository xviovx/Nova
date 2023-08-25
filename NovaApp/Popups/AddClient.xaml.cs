using NovaApp.Components;
using NovaApp.Models;
using Mopups.Services;
using System.Diagnostics;
using NovaApp.ViewModels;

namespace NovaApp.Popups
{

    
    public partial class AddClient
    {
        private ClientViewModel _viewModel;

        public AddClient()
        {
            InitializeComponent();
            _viewModel = new ClientViewModel(new Services.RestService()); //init our service
            BindingContext = _viewModel; //the context of the xaml is this viewmodel

        }

        public void OnCloseButtonClicked(object sender, EventArgs e)
      {
         try
           {
              MopupService.Instance.PopAllAsync();
          }
         catch (Exception ex)
         {
                Debug.WriteLine($"An error occurred while popping: {ex.Message}");
                
           }
        }
    }
}
