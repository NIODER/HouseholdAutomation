using CommunityToolkit.Mvvm.Input;
using System;

namespace HouseholdAutomationDesktop.ViewModel.DialogsViewModel
{
    public class SaveChangesWarningViewModel : ViewModelBase, IDialogViewModel
    {
        public class DialogWindowEventArgs : EventArgs
        {
            public bool SaveChanges { get; set; }

            public DialogWindowEventArgs(bool saveChanges)
            {
                SaveChanges = saveChanges;
            }
        }

        public RelayCommand SaveChangesCommand { get; private set; }
        public RelayCommand DontSaveChangesCommand { get; private set; }

        public event EventHandler<EventArgs>? OnDialogResult;

        public SaveChangesWarningViewModel()
        {
            SaveChangesCommand = new(OnSaveChangesCommand);
            DontSaveChangesCommand = new(OnDontSaveChangesCommand);
        }

        private void OnDontSaveChangesCommand()
        {
            OnDialogResult?.Invoke(this, new DialogWindowEventArgs(false));
        }

        private void OnSaveChangesCommand()
        {
            OnDialogResult?.Invoke(this, new DialogWindowEventArgs(true));
        }
    }
}
