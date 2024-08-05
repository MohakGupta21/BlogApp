
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data
{
    public class AuthDbContext: IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = "9482717c-b095-4cf3-bf93-d433fc56b185";
            var superAdminRoleId = "f1ea0764-36a1-443b-bfe4-f852c3d238ad";
            var userRoleId = "d1c76220-f7e0-4fcd-b793-83e1433d589f";

            // Seed Roles(User,Admin,SuperAdmin)
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };
            
            builder.Entity<IdentityRole>().HasData(roles);
            
            // Seed SuperAdminUser
            var superAdminId = "93614adb-a5ba-49fd-a516-ffe70925071f";
            var superAdminUser = new IdentityUser
            {
                UserName="superadmin@blogapp.com",
                Email ="superadmin@blogapp.com",
                NormalizedEmail="superadmin@blogapp.com".ToUpper(),
                NormalizedUserName ="superadmin@blogapp.com".ToUpper(),
                Id = superAdminId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser,"superadmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            // Add All roles to SuperAdmin
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }

            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}