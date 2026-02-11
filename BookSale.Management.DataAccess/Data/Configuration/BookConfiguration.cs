using BookSale.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookSale.Management.DataAccess.Data.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(b => b.Code)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(b => b.Title)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(b => b.Available)
                   .IsRequired();

            builder.Property(b => b.Cost)
                   .IsRequired();

            builder.Property(b => b.Publisher)
                   .HasMaxLength(500);

            builder.Property(b => b.Author)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(b => b.CreatedOn)
                   .IsRequired();

            builder.Property(b => b.Description)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.HasOne(b => b.Genre)
                   .WithMany()
                   .HasForeignKey(b => b.GenreId)
                   .IsRequired();
        }
    }
}
