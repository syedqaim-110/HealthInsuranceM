// Data/ApplicationDbContext.cs
using HealthInsuranceMgmt.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceMgmt.Data
{
    public class ApplicationDbContext : IdentityDbContext<EmpRegister>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets - Yeh database ki tables hain
        public DbSet<CompanyDetail> CompanyDetails { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<HospitalInfo> HospitalInfos { get; set; }
        public DbSet<PolicyRequestDetail> PolicyRequestDetails { get; set; }
        public DbSet<PolicyApprovalDetail> PolicyApprovalDetails { get; set; }
        public DbSet<PolicyOnEmployee> PoliciesOnEmployees { get; set; }
        public DbSet<PolicyTotalDescription> PolicyTotalDescriptions { get; set; }

        // --- NAYA ADD HUA ---
        public DbSet<FAQ> FAQs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. PolicyApprovalDetail relationships
            builder.Entity<PolicyApprovalDetail>()
                .HasOne(a => a.PolicyRequest)
                .WithOne(r => r.ApprovalDetail)
                .HasForeignKey<PolicyApprovalDetail>(a => a.PolicyRequestDetailId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PolicyApprovalDetail>()
                .HasOne(a => a.ApprovedBy)
                .WithMany()
                .HasForeignKey(a => a.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. PolicyRequestDetail relationships
            builder.Entity<PolicyRequestDetail>()
                .HasOne(r => r.Employee)
                .WithMany(e => e.PolicyRequests)
                .HasForeignKey(r => r.EmpRegisterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PolicyRequestDetail>()
                .HasOne(r => r.Policy)
                .WithMany(p => p.PolicyRequests)
                .HasForeignKey(r => r.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. PolicyOnEmployee relationships
            builder.Entity<PolicyOnEmployee>()
                .HasOne(pe => pe.Employee)
                .WithMany(e => e.AssignedPolicies)
                .HasForeignKey(pe => pe.EmpRegisterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PolicyOnEmployee>()
                .HasOne(pe => pe.Policy)
                .WithMany(p => p.AssignedEmployees)
                .HasForeignKey(pe => pe.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}