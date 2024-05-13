using UniversalSearchCriteria.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace UniversalSearchCriteria.Helpers
{
    public static class SearchHelper
    {
        // Method to apply filters and perform search on a collection of items
        public static ObservableCollection<T> ApplyFiltersAndSearch<T>(MyDbContext dbContext, IEnumerable<T> items, IEnumerable<SearchFilter> selectedFilters, SearchField selectedSearchField, object selectedSearchValue)
            where T : class
        {
            var filteredItems = items.ToList();

            // Apply selected filters to the collection
            foreach (var filter in selectedFilters)
            {
                var propertyName = filter.SearchField.Name;
                var propertyValue = filter.Value.ToString().ToLower();

                var property = typeof(T).GetProperty(propertyName);

                // Filter the items based on the selected filter criteria
                filteredItems = filteredItems.Where(item =>
                {
                    var propertyStringValue = GetPropertyStringValue(item, propertyName);
                    return propertyStringValue != null && propertyStringValue.ToLower().Contains(propertyValue);
                }).ToList();
            }

            // Perform search based on selected search field and value
            if (selectedSearchField != null && selectedSearchValue != null)
            {
                var searchPropertyName = selectedSearchField.Name;
                var searchValue = selectedSearchValue.ToString().ToLower();

                // Filter the items based on the search field and value
                filteredItems = filteredItems.Where(item =>
                {
                    var propertyStringValue = GetPropertyStringValue(item, searchPropertyName);
                    return propertyStringValue != null && propertyStringValue.ToLower().Contains(searchValue);
                }).ToList();
            }

            // Convert the filtered items to an ObservableCollection and return
            return new ObservableCollection<T>(filteredItems);
        }

        // Helper method to retrieve the string value of a property from an object
        private static string GetPropertyStringValue<T>(T item, string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);
            var value = property.GetValue(item);

            return value?.ToString() ?? string.Empty;
        }
    }
}
