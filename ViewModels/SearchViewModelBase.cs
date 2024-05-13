using UniversalSearchCriteria.Helpers;
using UniversalSearchCriteria.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UniversalSearchCriteria.ViewModels
{
    // This is an abstract base class for view models that implement search functionality
    // The type parameter 'T' represents the type of items being searched
    public abstract class SearchViewModelBase<T> : INotifyPropertyChanged where T : class
    {
        protected readonly MyDbContext dbContext;
        protected List<SearchField> searchFields;
        protected SearchField selectedSearchField;
        protected object selectedSearchValue;
        protected UIElement selectedSearchValueControl;
        protected ObservableCollection<T> items;
        protected ObservableCollection<SearchFilter> selectedFilters;
        protected SearchFilter selectedFilter;
        protected ObservableCollection<T> filteredItems;

        public event PropertyChangedEventHandler PropertyChanged;

        public SearchViewModelBase()
        {
            dbContext = new MyDbContext();
            InitializeSearchFields();
            Items = new ObservableCollection<T>();
            selectedSearchField = searchFields.FirstOrDefault();
            selectedSearchValue = null;
            selectedSearchValueControl = null;
            SelectedFilters = new ObservableCollection<SearchFilter>();
            Items = new ObservableCollection<T>(dbContext.Set<T>().ToList());
            FilteredItems = SearchHelper.ApplyFiltersAndSearch(dbContext, Items, SelectedFilters, selectedSearchField, selectedSearchValue);
        }

        // Overloaded constructor that allows injecting a pre-initialized DbContext instance
        public SearchViewModelBase(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
            InitializeSearchFields();
            Items = new ObservableCollection<T>();
            selectedSearchField = searchFields.FirstOrDefault();
            selectedSearchValue = null;
            selectedSearchValueControl = null;
            SelectedFilters = new ObservableCollection<SearchFilter>();
            Items = new ObservableCollection<T>(dbContext.Set<T>().ToList());
            FilteredItems = SearchHelper.ApplyFiltersAndSearch(dbContext, Items, SelectedFilters, selectedSearchField, selectedSearchValue);
        }

        // Property for the list of search fields
        public List<SearchField> SearchFields
        {
            get { return searchFields; }
            set { searchFields = value; OnPropertyChanged(nameof(SearchFields)); }
        }

        // Property for selected search field
        public SearchField SelectedSearchField
        {
            get { return selectedSearchField; }
            set
            {
                selectedSearchField = value;
                SelectedSearchValue = null; // Reset the selected search value
                SelectedSearchValueControl = GetSearchValueControl(selectedSearchField);
                OnPropertyChanged(nameof(SelectedSearchField));
                FilteredItems = SearchHelper.ApplyFiltersAndSearch(dbContext, Items, SelectedFilters, selectedSearchField, selectedSearchValue);
            }
        }

        // Property for the selected search value
        public object SelectedSearchValue
        {
            get { return selectedSearchValue; }
            set
            {
                selectedSearchValue = value;
                OnPropertyChanged(nameof(SelectedSearchValue));
                FilteredItems = SearchHelper.ApplyFiltersAndSearch(dbContext, Items, SelectedFilters, selectedSearchField, selectedSearchValue);
            }
        }

        // Property for the selected search value control (UI element)
        public UIElement SelectedSearchValueControl
        {
            get
            {
                if (selectedSearchValueControl == null)
                {
                    selectedSearchValueControl = GetDefaultSearchValueControl();
                }
                return selectedSearchValueControl;
            }
            set { selectedSearchValueControl = value; OnPropertyChanged(nameof(SelectedSearchValueControl)); }
        }

        // Get the default search value control (UI element)
        private UIElement GetDefaultSearchValueControl()
        {
            if (SelectedSearchField != null)
            {
                return GetSearchValueControl(SelectedSearchField);
            }
            var defaultControl = new TextBlock();
            defaultControl.Text = "Select a search field";
            return defaultControl;
        }

        // Property for the collection of items
        public ObservableCollection<T> Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged(nameof(Items)); }
        }

        // Property for the collection of selected filters
        public ObservableCollection<SearchFilter> SelectedFilters
        {
            get { return selectedFilters; }
            set { selectedFilters = value; OnPropertyChanged(nameof(SelectedFilters)); }
        }

        // Property for the selected filter
        public SearchFilter SelectedFilter
        {
            get { return selectedFilter; }
            set
            {
                selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                OnPropertyChanged(nameof(SelectedFilterText));
            }
        }

        // Property for the text representation of the selected filter
        public string SelectedFilterText => SelectedFilter?.ToString();

        // Property for the collection of filtered items
        public ObservableCollection<T> FilteredItems
        {
            get { return filteredItems; }
            set { filteredItems = value; OnPropertyChanged(nameof(FilteredItems)); }
        }

        // Initialize the list of search fields
        protected virtual void InitializeSearchFields()
        {
            searchFields = GetSearchFields(typeof(T));
        }

        // Method to retrieve the list of search fields for a given object type
        protected virtual List<SearchField> GetSearchFields(Type objectType)
        {
            var searchFields = new List<SearchField>();
            var properties = objectType.GetProperties();

            foreach (var property in properties)
            {
                var searchField = new SearchField(property.Name, objectType, property.PropertyType);
                searchFields.Add(searchField);
            }
            return searchFields;
        }

        // Method to get the appropriate search value control (UI element) based on the selected search field
        protected virtual UIElement GetSearchValueControl(SearchField searchField)
        {
            // Create a TextBox control for string fields
            if (searchField.FieldType == typeof(string))
            {
                var textBox = new TextBox();
                textBox.TextChanged += TextBox_TextChanged;
                return textBox;
            }
            // Create a DatePicker control for DateTime fields
            else if (searchField.FieldType == typeof(DateTime))
            {
                var datePicker = new DatePicker();
                datePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;
                return datePicker;
            }
            // Create a TextBox control for integer fields
            else if (searchField.FieldType == typeof(int))
            {
                var textBox = (new TextBox());
                textBox.TextChanged += TextBox_TextChanged;
                return textBox;
            }
            // Create a TextBox control for double fields
            else if (searchField.FieldType == typeof(double))
            {
                var textBox = (new TextBox());
                textBox.TextChanged += TextBox_TextChanged;
                return textBox;
            }
            else
            {
                // Return an appropriate UI element for other field types
                return new TextBlock();
            }
        }

        // Event handler for the text changed event of the text box
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SelectedSearchValue = ((TextBox)sender).Text;
        }

        // Event handler for the selected date changed event of the date picker
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedSearchValue = ((DatePicker)sender).SelectedDate;
        }

        // Command for adding a filter
        public ICommand AddFilterCommand => new RelayCommand(AddFilter);

        // Method for adding a filter
        private void AddFilter(object parameter)
        {
            if (SelectedSearchField != null && SelectedSearchValue != null)
            {
                var filter = new SearchFilter(SelectedSearchField, SelectedSearchValue);
                SelectedFilters.Add(filter);
                FilteredItems = SearchHelper.ApplyFiltersAndSearch(dbContext, Items, SelectedFilters, selectedSearchField, selectedSearchValue);
            }
            else
            {
                MessageBox.Show("Please select a search field and value.");
            }
        }

        // Command for removing a filter
        public ICommand RemoveFilterCommand => new RelayCommand(RemoveFilter);

        // Method for removing a filter
        private void RemoveFilter(object parameter)
        {
            if (SelectedFilter != null)
            {
                SelectedFilters.Remove(SelectedFilter);
                FilteredItems = SearchHelper.ApplyFiltersAndSearch(dbContext, Items, SelectedFilters, selectedSearchField, selectedSearchValue);
                SelectedSearchValueControl = GetSearchValueControl(SelectedSearchField); // Update the search value control with the latest search field
                SelectedSearchValue = null;
            }
        }

        // Command for removing the selected search value
        public ICommand RemoveSearchValueCommand => new RelayCommand(RemoveSearchValue);

        // Method for removing the selected search value
        private void RemoveSearchValue(object parameter)
        {
            SelectedSearchValue = null;
            SelectedSearchValueControl = GetSearchValueControl(SelectedSearchField); // Update the search value control with the latest search field
        }

        // Get the string representation of a property value
        protected virtual string GetPropertyStringValue(T item, string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);
            var value = property.GetValue(item);
            return value?.ToString() ?? string.Empty;
        }

        // Notify property changed event
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}