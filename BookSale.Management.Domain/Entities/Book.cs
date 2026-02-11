namespace BookSale.Management.Domain.Entities
{
    public class Book: BaseEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public int Available { get; set; }
        public double Cost { get; set; }
        public string? Publisher { get; set; }
        public string Author { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Description { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }

    }
}
