namespace BobyDesignWeb.Data.Entities
{
    public class StoredFile
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public byte[] Content { get; set; } = null!;
        public int Size { get; set; }
        ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
