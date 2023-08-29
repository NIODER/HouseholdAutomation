using AutomationHouseholdDatabase.Models;
using CommunityToolkit.Mvvm.Input;
using HouseholdAutomationLogic.BLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HouseholdAutomationDesktop.ViewModel.DialogsViewModel
{
    public class SelectResourceViewModel : ViewModelBase, IDialogViewModel, IDataLoading
    {
		public class SelectedResourceEventArgs : EventArgs
		{
            public Resource SelectedResource { get; }
            public int Count { get; set; }

            public SelectedResourceEventArgs(Resource selectedResource, int count)
            {
                SelectedResource = selectedResource;
                Count = count;
            }
        }

		private List<Resource> dbResources = new();
		private ObservableCollection<Resource> resources = new();
		private Resource? selectedResource;
		private string? resourceSelector;
        private ProviderToResource? selectedProviderToResource;
        private int resourceCount;

        public ObservableCollection<Resource> Resources
		{
			get { return resources; }
			set
			{
				resources = value;
				OnPropertyChanged(nameof(Resources));
			}
		}

        public Resource? SelectedResource
		{
			get => selectedResource;
			set
			{
				selectedResource = value;
				OnPropertyChanged(nameof(SelectedResource));
			}
		}

		public string? ResourceSelector
		{
			get => resourceSelector;
			set
			{
				resourceSelector = value;
				OnPropertyChanged(nameof(ResourceSelector));
				if (string.IsNullOrEmpty(resourceSelector))
				{
					Resources = new(dbResources);
				}
				else
				{
					Resources = new(dbResources.Where(r => r.ResourceId.ToString().StartsWith(resourceSelector) || r.ResourceName.StartsWith(resourceSelector)));
				}
				OnPropertyChanged(nameof(Resources));
			}
		}

        public ProviderToResource? SelectedProviderToResource
        {
            get => selectedProviderToResource;
            set
            {
                selectedProviderToResource = value;
                OnPropertyChanged(nameof(SelectedProviderToResource));
            }
		}

		public int ResourceCount
		{
			get => resourceCount;
			set
			{
				resourceCount = value;
				OnPropertyChanged(nameof(ResourceCount));
			}
		}

		public RelayCommand ChoseResourceCommand { get; private set; }

        public event EventHandler<EventArgs>? OnDialogResult;

		private readonly ResourceBLL _resourceBLL;

        public SelectResourceViewModel(ResourceBLL resourceBLL)
		{
			_resourceBLL = resourceBLL;
			ChoseResourceCommand = new(OnChoseResourceCommand);
		}

        private void OnChoseResourceCommand()
        {
			if (SelectedResource == null)
			{
				MessageBox.Show("Выберите ресурс.");
				return;
			}
            OnDialogResult?.Invoke(this, new SelectedResourceEventArgs(SelectedResource, ResourceCount));
        }

        public Task LoadDataAsync() => Task.Run(() =>
        {
            dbResources = _resourceBLL.Redactor.GetAll().ToList();
            Resources = new(dbResources);
        });
    }
}
