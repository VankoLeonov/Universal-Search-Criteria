using System;

namespace UniversalSearchCriteria.Models
{
    public class SearchAuthor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime BirthDate { get; set; }

        // Method to get the string value
        public string GetPropertyStringValue(string propertyName)
        {
            var property = GetType().GetProperty(propertyName);
            var value = property.GetValue(this);
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("dd/MM/yyyy");
            }
            return value?.ToString() ?? string.Empty;
        }
    }
}
