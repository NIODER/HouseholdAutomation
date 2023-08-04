using System.ComponentModel;

namespace HouseholdAutomationDesktop.ViewModel
{
    public class ViewModelBase
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}