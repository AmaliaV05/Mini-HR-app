using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mini_HR_app.Data
{
    internal class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "e4afbe47-571b-45a8-8019-25431dadf80d",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    ConcurrencyStamp = "9d428364-6364-4f17-8f43-3269d7e2fc50"
                },
                new IdentityRole
                {
                    Id = "1148e8f6-d753-42de-81d0-c2cf9662447b",
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = "5927e66f-c510-4c85-89c3-397a79b60421"
                },
                new IdentityRole
                {
                    Id = "371a40a5-ea5b-4019-ab99-8660cf0b4dbc",
                    Name = "Member",
                    NormalizedName = "MEMBER",
                    ConcurrencyStamp = "dce69349-7ec8-4ee9-bff4-b1c502552c7b"
                });
        }
    }
}
