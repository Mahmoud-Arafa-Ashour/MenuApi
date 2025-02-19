namespace E_Commerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImagePath { get; set; } // Store file path as string
        public ICollection<Item> items { get; set; } = [];
    }
}
