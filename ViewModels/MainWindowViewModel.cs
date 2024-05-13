using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace UniversalSearchCriteria.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private List<string> objectTypes; // List for available object types
        private string selectedObjectType;
        private object selectedViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            ObjectTypes = GetModelTypesWithViewModels(); // Get the available object types with corresponding view models
            SelectedObjectType = ObjectTypes.FirstOrDefault();
            InitializeSelectedViewModel();
        }

        public List<string> ObjectTypes
        {
            get { return objectTypes; }
            set
            {
                objectTypes = value;
                OnPropertyChanged(nameof(ObjectTypes));
            }
        }

        public string SelectedObjectType
        {
            get { return selectedObjectType; }
            set
            {
                selectedObjectType = value;
                InitializeSelectedViewModel(); // Initialize the selected view model based on the new selected object type
                OnPropertyChanged(nameof(SelectedObjectType));
            }
        }

        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set
            {
                selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        // Method to initialize the selected view model based on the selected object type
        private void InitializeSelectedViewModel()
        {
            string viewModelTypeName = "UniversalSearchCriteria.ViewModels." + SelectedObjectType + "ViewModel";
            Type viewModelType = Type.GetType(viewModelTypeName);

            if (viewModelType != null)
            {
                SelectedViewModel = Activator.CreateInstance(viewModelType); // Create an instance of the selected view model
            }
            else
            {
                // Handle the case when the ViewModel type is not found
                SelectedViewModel = null;
            }
        }

        // Method to get the available object types with corresponding view models
        private List<string> GetModelTypesWithViewModels()
        {
            var modelTypes = Assembly.GetExecutingAssembly()
                                     .GetTypes()
                                     .Where(t => t.IsClass && !t.IsAbstract && t.Namespace == "UniversalSearchCriteria.Models")
                                     .ToList();

            var modelTypesWithViewModels = new List<string>();

            foreach (var modelType in modelTypes)
            {
                var viewModelTypeName = $"UniversalSearchCriteria.ViewModels.{modelType.Name}ViewModel";

                var viewModelType = Assembly.GetExecutingAssembly().GetType(viewModelTypeName);

                if (viewModelType != null)
                {
                    modelTypesWithViewModels.Add(modelType.Name);
                }
            }

            return modelTypesWithViewModels;
        }

        // Helper method to raise the PropertyChanged event
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
