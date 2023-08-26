using HouseholdAutomationDesktop.ViewModel;
using System;

namespace HouseholdAutomationDesktop.Utils
{
    public interface IWindowPresenter
    {
        void Show(ViewModelBase viewModel);
        IViewModel Show<IViewModel>() where IViewModel : ViewModelBase;
        IViewModel Show<IViewModel>(EventHandler<EventArgs> callback) where IViewModel : ViewModelBase, IDialogViewModel;
    }
}