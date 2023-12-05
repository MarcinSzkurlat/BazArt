using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.DbConfiguration
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Domain.Models.Category>
    {
        public void Configure(EntityTypeBuilder<Domain.Models.Category> builder)
        {
            builder.Property(c => c.Name)
                .HasMaxLength(100);
            builder.Property(c => c.Description)
                .HasMaxLength(1000);
            builder.Property(c => c.ImageUrl)
                .HasMaxLength(500);

            builder.HasMany(c => c.Users)
                .WithOne(c => c.Category)
                .HasForeignKey(k => k.CategoryId);

            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(k => k.CategoryId);

            builder.HasMany(c => c.Events)
                .WithOne(e => e.Category)
                .HasForeignKey(k => k.CategoryId);

            builder.HasData(
                new Category()
                {
                    Id = (int)Categories.Painting,
                    Name = Categories.Painting.ToString(),
                    Description =
                        "Painting is a visual art form where pigments or colors are applied to a surface, typically a canvas or paper, using various techniques to create images, convey emotions, or express ideas.",
                    ImageUrl = "https://res.cloudinary.com/dgrnhoty0/image/upload/v1701185038/BazArt/Categories/Category_Painting.jpg"
                },
                new Category()
                {
                    Id = (int)Categories.Sculpture,
                    Name = Categories.Sculpture.ToString(),
                    Description =
                        "Sculpture is a three-dimensional art form in which artists manipulate materials like stone, wood, metal, or clay to create physical, often sculptural, representations of objects, people, or ideas.",
                    ImageUrl =
                        "https://res.cloudinary.com/dgrnhoty0/image/upload/v1701185123/BazArt/Categories/Category_Sculpture.jpg"
                },
                new Category()
                {
                    Id = (int)Categories.Photography,
                    Name = Categories.Photography.ToString(),
                    Description =
                        "Photography is the art and technology of capturing and recording images using light-sensitive materials or digital sensors, allowing for the creation of still or moving visual representations of scenes, subjects, and moments.",
                    ImageUrl = "https://res.cloudinary.com/dgrnhoty0/image/upload/v1701185081/BazArt/Categories/Category_Photography.jpg"
                },
                new Category()
                {
                    Id = (int)Categories.HandMade,
                    Name = Categories.HandMade.ToString(),
                    Description =
                        "Handmade art refers to creative works produced by skilled individuals using manual techniques and traditional craftsmanship, typically without the aid of automated or mass-production processes. It emphasizes the personal touch and unique qualities of each piece.",
                    ImageUrl = "https://res.cloudinary.com/dgrnhoty0/image/upload/v1701184994/BazArt/Categories/Category_HandMade.jpg"
                });
        }
    }
}
