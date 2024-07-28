using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;

namespace BobyDesignWeb.Services
{
    public class PaginationService
    {
        public IQueryable<T> GetPageItems<T>(IQueryable<T> query, int pageNumber, int itemsPerPage = 20) 
        {
            return query.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage);
        }
    }
}
