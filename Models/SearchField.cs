using System;
using System.Collections.Generic;

namespace UniversalSearchCriteria.Models
{
    public class SearchField
    {
        public string Name { get; set; }

        // Type of the model associated with the search field
        public Type ModelType { get; set; }

        public List<SearchField> Fields { get; set; }

        // Type of the field's value
        public Type FieldType { get; set; }

        // Constructor for a basic search field with a string value type
        public SearchField(string name, Type modelType)
        {
            Name = name;
            ModelType = modelType;
            FieldType = typeof(string);
        }

        // Constructor for a search field with nested fields
        public SearchField(string name, Type modelType, List<SearchField> fields)
        {
            Name = name;
            ModelType = modelType;
            Fields = fields;
        }

        // Constructor for a search field with a specific value type
        public SearchField(string name, Type modelType, Type fieldType)
        {
            Name = name;
            ModelType = modelType;
            FieldType = fieldType;
        }
    }
}
