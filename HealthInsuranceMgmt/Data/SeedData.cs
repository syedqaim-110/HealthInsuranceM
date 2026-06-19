// Data/SeedData.cs
using HealthInsuranceMgmt.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceMgmt.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // 1. SEED ROLES
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "Employee", "FinanceManager" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. SEED DEFAULT USERS
            var userManager = serviceProvider.GetRequiredService<UserManager<EmpRegister>>();

            // --- A) DEFAULT ADMIN USER ---
            string adminEmail = "admin@medicare.com";
            string adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new EmpRegister
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Admin",
                    Department = "Administration",
                    Designation = "Admin",
                    Address = "Medicare HQ",
                    EmailConfirmed = true
                };

                var createAdminResult = await userManager.CreateAsync(newAdmin, adminPassword);
                if (createAdminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }

            // --- B) DEFAULT EMPLOYEE USER ---
            string empEmail = "emp@test.com";
            string empPassword = "Test@123";

            var empUser = await userManager.FindByEmailAsync(empEmail);
            if (empUser == null)
            {
                var newEmp = new EmpRegister
                {
                    UserName = empEmail,
                    Email = empEmail,
                    FirstName = "Rahul",
                    LastName = "Sharma",
                    Department = "IT",
                    Designation = "Developer",
                    Address = "Mumbai",
                    EmailConfirmed = true
                };

                var createEmpResult = await userManager.CreateAsync(newEmp, empPassword);
                if (createEmpResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(newEmp, "Employee");
                }
            }

            // --- C) DEFAULT FINANCE MANAGER USER ---
            string finEmail = "finance@medicare.com";
            string finPassword = "Finance@123";

            var finUser = await userManager.FindByEmailAsync(finEmail);
            if (finUser == null)
            {
                var newFin = new EmpRegister
                {
                    UserName = finEmail,
                    Email = finEmail,
                    FirstName = "Finance",
                    LastName = "Manager",
                    Department = "Finance",
                    Designation = "Finance Manager",
                    Address = "Medicare HQ",
                    EmailConfirmed = true
                };

                var createFinResult = await userManager.CreateAsync(newFin, finPassword);
                if (createFinResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(newFin, "FinanceManager");
                }
            }
        }
    }
}