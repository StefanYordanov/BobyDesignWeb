namespace BobyDesignWeb.Models
{
    public class PageViewModel<T>
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public ICollection<T> Items { get; set; } = new List<T>();
    }
}
