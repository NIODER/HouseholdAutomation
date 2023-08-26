using System;

namespace HouseholdAutomationDesktop.ViewModel
{
    public interface IDialogViewModel
    {
        event EventHandler<EventArgs>? OnDialogResult;
    }
}
