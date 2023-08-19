using Microsoft.UI.Xaml.Controls.Primitives;
using NovaApp.Popups;
using Mopups.Services;

namespace NovaApp.Views;

public partial class StaffPage : ContentView
{
    public StaffPage()
    {
        InitializeComponent();
    }

    private void OnAddNewClicked(object sender, EventArgs e)
    {
        MopupService.Instance.PushAsync(new AddStaff());
    }
}
