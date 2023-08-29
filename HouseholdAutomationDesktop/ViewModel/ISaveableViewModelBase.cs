using System;

namespace HouseholdAutomationDesktop.ViewModel
{
    public delegate void SavedStateChanged(Type type, bool savedState);

    public interface ISaveableViewModelBase
    {
        bool IsSaved { get; protected set; }

        event SavedStateChanged? ChangesSaved;
    }
}