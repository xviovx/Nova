using NovaApp.Components;
using NovaApp.Models;
using Mopups.Services;
using System.Diagnostics;
using NovaApp.ViewModels;

namespace NovaApp.Popups
{

    private ClientViewModel _viewModel;
    public partial class AddClient
    {
        
        public AddClient()
        {
            InitializeComponent();
            _viewModel = new ClientViewModel(new Services.RestService()); //init our service
            BindingContext = _viewModel; //the context of the xaml is this viewmodel

        }

       public void OnAddNewButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Clicked");

            try
            {
                // Access the BindingContext (ClientData instance)
                if (BindingContext is ClientData clientData)
                {
                    // Print data to console
                    Debug.WriteLine($"Company Name: {clientData.CompanyName}");
                    Debug.WriteLine($"Email: {clientData.Email}");
                    Debug.WriteLine($"Client Type: {clientData.ClientType}");
                }
                else
                {
                    Debug.WriteLine("BindingContext is not a ClientData instance");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }
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
