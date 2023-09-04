using Microsoft.Maui.Controls;
using NovaApp.Views;

namespace NovaApp.Views
{
    public partial class StaffPageWrapper : ContentPage
    {
        public StaffPageWrapper()
        {
            Title = "Staff Page";
            Content = new StaffPage();
        }
    }
}