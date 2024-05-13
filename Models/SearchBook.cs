using System;

namespace UniversalSearchCriteria.Models
{
    public class SearchBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Status { get; set; }
        public DateTime PublicationDate { get; set; }

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
