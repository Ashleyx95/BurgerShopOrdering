using BurgerShopOrdering.core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.core.Data.Seeding
{
    public class Seeder
    {
        public static void Seed(ModelBuilder builder)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            var user1 = new ApplicationUser("Ashleys", "Fritje", "ashleysfritje@hotmail.com", true)
            {
                Id = "00000000-0000-0000-0000-000000000001"
            };

            var user2 = new ApplicationUser("Marie", "Franck", "MarieFranck@hotmail.com", true)
            {
                Id = "00000000-0000-0000-0000-000000000002"
            };

            var user3 = new ApplicationUser("Lucy", "Mol", "lucy.mol@hotmail.com", true)
            {
                Id = "00000000-0000-0000-0000-000000000003"
            };

            user1.PasswordHash = passwordHasher.HashPassword(user1, "EasyPass1");
            user2.PasswordHash = passwordHasher.HashPassword(user2, "EasyPass1");
            user3.PasswordHash = passwordHasher.HashPassword(user3, "EasyPass1");

            var roles = new IdentityRole[]
            {
                new() { Id = "00000000-0000-0000-0000-000000000004", Name = "Admin", NormalizedName = "ADMIN" },
                new() { Id = "00000000-0000-0000-0000-000000000005", Name = "Client", NormalizedName = "CLIENT" }
            };

            var identityUserRoles = new IdentityUserRole<string>[]
            {
                new() { UserId = "00000000-0000-0000-0000-000000000001", RoleId = "00000000-0000-0000-0000-000000000004" },
                new () { UserId = "00000000-0000-0000-0000-000000000002", RoleId = "00000000-0000-0000-0000-000000000005" },
                new () { UserId = "00000000-0000-0000-0000-000000000003", RoleId = "00000000-0000-0000-0000-000000000005" },
            };

            var categories = new Category[]
            {
                new( Guid.Parse("00000000-0000-0000-0000-000000000010"), "Frieten"),
                new( Guid.Parse("00000000-0000-0000-0000-000000000011"), "Burgers"),
                new( Guid.Parse("00000000-0000-0000-0000-000000000012"), "Vlees"),
                new( Guid.Parse("00000000-0000-0000-0000-000000000013"), "Vis"),
                new( Guid.Parse("00000000-0000-0000-0000-000000000014"), "Kaas"),
                new( Guid.Parse("00000000-0000-0000-0000-000000000015"), "Kinderen"),
                new( Guid.Parse("00000000-0000-0000-0000-000000000016"), "Koude Sauzen"),
                new( Guid.Parse("00000000-0000-0000-0000-000000000017"), "Warme Sauzen"),
                new( Guid.Parse("00000000-0000-0000-0000-000000000018"), "Drinken"),
                new( Guid.Parse("00000000-0000-0000-0000-000000000019"), "Vegetarisch"),
            };

            var products = new Product[]
            {
                new( Guid.Parse("00000000-0000-0000-0000-000000000025"), "Mini friet", 2.40M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000026"), "Kleine friet", 2.80M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000027"), "Medium friet", 3.40M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000028"), "Grote friet", 4.70M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000029"), "Familie pak", 8.70M),

                new( Guid.Parse("00000000-0000-0000-0000-000000000030"), "Bicky Burger", 4.40M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000031"), "Bicky Cheese", 4.70M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000032"), "Bicky Fish", 4.70M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000033"), "Bicky Chicken", 4.70M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000034"), "Bicky Vegi", 4.70M),

                new( Guid.Parse("00000000-0000-0000-0000-000000000035"), "Berenpoot", 3.70M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000036"), "Stoofvlees", 4.70M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000037"), "Bitterballen", 2.70M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000038"), "Twijfelaar", 4.70M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000039"), "Zigeunerstick", 3.20M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000040"), "Boulet", 3.00M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000041"), "Cervela", 3.20M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000042"), "Curryworst", 2.40M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000043"), "Curryworst Speciaal", 3.40M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000044"), "Goulash kroket", 3.00M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000045"), "Kipcorn", 3.20M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000046"), "Kippen nuggets", 4.00M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000047"), "Kipfingers", 4.00M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000048"), "Loempia", 4.70M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000049"), "Viandel", 3.00M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000050"), "Sate", 4.40M),

                new( Guid.Parse("00000000-0000-0000-0000-000000000051"), "Visstick", 4.70M),

                new( Guid.Parse("00000000-0000-0000-0000-000000000052"), "Kaaskroket", 2.20M),
                new(Guid.Parse("00000000-0000-0000-0000-000000000053"), "Kaassouffle", 2.80M),
                new(Guid.Parse("00000000-0000-0000-0000-000000000054"), "Mozarella vingers", 4.20M),

                new( Guid.Parse("00000000-0000-0000-0000-000000000055"), "Boy box", 6.50M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000056"), "Girl box", 6.50M),

                new( Guid.Parse("00000000-0000-0000-0000-000000000057"), "Mayonaise", 1.20M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000058"), "Tomaten ketchup", 1.20M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000059"), "Curry ketchup", 1.20M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000060"), "Andalouse", 1.40M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000061"), "Americain", 1.40M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000062"), "Tartaar", 1.40M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000063"), "Looksaus", 1.40M),

                new( Guid.Parse("00000000-0000-0000-0000-000000000064"), "Pindasaus", 2.20M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000065"), "Stoofvleessaus", 2.20M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000066"), "Currysaus", 2.20M),

                new( Guid.Parse("00000000-0000-0000-0000-000000000067"), "Coca cola", 2.00M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000068"), "Coca cola zero", 2.00M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000069"), "Sprite", 2.00M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000070"), "Fanta", 2.00M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000071"), "Ice tea", 2.00M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000072"), "Jupiler", 2.20M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000073"), "Water plat", 2.00M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000074"), "Water bruis", 2.00M),

                new( Guid.Parse("00000000-0000-0000-0000-000000000075"), "Vegi bitterballen", 3.00M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000076"), "Nasi schijf", 3.20M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000077"), "Mini loempia", 4.20M),
                new( Guid.Parse("00000000-0000-0000-0000-000000000078"), "Vegi curryworst", 3.70M),
            };

            var categoryProducts = new[]
            {
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000010"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000025")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000010"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000026")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000010"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000027")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000010"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000028")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000010"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000029")},

                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000011"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000030")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000011"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000031")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000011"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000032")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000011"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000033")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000011"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000034")},

                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000030")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000031")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000033")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000035")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000036")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000037")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000038")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000039")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000040")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000041")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000042")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000043")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000044")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000045")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000046")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000047")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000048")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000049")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000012"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000050")},

                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000013"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000032")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000013"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000051")},

                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000014"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000052")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000014"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000053")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000014"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000054")},

                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000015"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000055")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000015"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000056")},

                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000016"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000057")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000016"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000058")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000016"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000059")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000016"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000060")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000016"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000061")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000016"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000062")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000016"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000063")},

                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000017"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000064")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000017"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000065")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000017"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000066")},

                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000018"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000067")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000018"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000068")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000018"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000069")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000018"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000070")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000018"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000071")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000018"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000072")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000018"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000073")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000018"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000074")},

                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000019"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000052")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000019"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000053")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000019"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000054")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000019"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000075")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000019"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000076")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000019"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000077")},
                new{CategoriesId = Guid.Parse("00000000-0000-0000-0000-000000000019"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000078")},

            };

            builder.Entity<ApplicationUser>().HasData(user1);
            builder.Entity<ApplicationUser>().HasData(user2);
            builder.Entity<ApplicationUser>().HasData(user3);
            builder.Entity<IdentityRole>().HasData(roles);
            builder.Entity<IdentityUserRole<string>>().HasData(identityUserRoles);
            builder.Entity<Category>().HasData(categories);
            builder.Entity<Product>().HasData(products);
            builder.Entity($"{nameof(Category)}{nameof(Product)}").HasData(categoryProducts);
        }
    }
}
