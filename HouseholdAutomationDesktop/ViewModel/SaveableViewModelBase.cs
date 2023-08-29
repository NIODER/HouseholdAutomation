using System;

namespace HouseholdAutomationDesktop.ViewModel
{
    public abstract class SaveableViewModelBase : ViewModelBase, ISaveableViewModelBase
    {
        private bool _saved = true;

        public bool IsSaved
        {
            get => _saved;
            set
            {
                _saved = value;
                OnPropertyChanged(nameof(IsSaved));
                ChangesSaved?.Invoke(GetType(), _saved);
            }
        }

        public event SavedStateChanged? ChangesSaved;
    }
}
