namespace UniversalSearchCriteria.Models
{
    public class SearchFilter
    {
        // The search field associated with the filter
        public SearchField SearchField { get; set; }

        // The value used for filtering
        public object Value { get; set; }

        public SearchFilter(SearchField searchField, object value)
        {
            SearchField = searchField;
            Value = value;
        }
        // ToString method to show data in DataGrid with filters.
        public override string ToString()
        {
            return $"{SearchField.Name}: {Value}";
        }
    }
}
