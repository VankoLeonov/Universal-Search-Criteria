using UniversalSearchCriteria.Models;

namespace UniversalSearchCriteria.ViewModels
{
    public class SearchAuthorViewModel : SearchViewModelBase<SearchAuthor>
    {
        public SearchAuthorViewModel() : base()
        {
            // Default constructor of the SearchAuthorViewModel class
        }

        public SearchAuthorViewModel(MyDbContext dbContext) : base(dbContext)
        {
            // Constructor of the SearchAuthorViewModel class that accepts a MyDbContext parameter
        }
    }
}
