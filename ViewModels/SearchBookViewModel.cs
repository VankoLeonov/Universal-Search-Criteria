using UniversalSearchCriteria.Models;

namespace UniversalSearchCriteria.ViewModels
{
    public class SearchBookViewModel : SearchViewModelBase<SearchBook>
    {
        public SearchBookViewModel() : base()
        {
            // Default constructor of the SearchBookViewModel class
        }

        public SearchBookViewModel(MyDbContext dbContext) : base(dbContext)
        {
            // Constructor of the SearchBookViewModel class that accepts a MyDbContext parameter
        }
    }
}
