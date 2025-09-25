using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<Contact> Contacts { get; set; } = null!;
    public DbSet<Email> Emails { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<CustomerEmail> CustomerEmails { get; set; }
    public DbSet<CustomerPhone> CustomerPhones { get; set; }

    // ใน DbContext
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Contact - Customer (many-to-one)
        modelBuilder.Entity<Contact>()
            .HasOne(c => c.Customer)
            .WithMany()
            .HasForeignKey(c => c.customerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Email - Contact (many-to-one)
        modelBuilder.Entity<Email>()
            .HasOne(e => e.contact)
            .WithMany(c => c.emails)
            .HasForeignKey(e => e.contactId)
            .OnDelete(DeleteBehavior.Cascade);

        // Phone - Contact (many-to-one)
        modelBuilder.Entity<Phone>()
            .HasOne(p => p.contact)
            .WithMany(c => c.phones)
            .HasForeignKey(p => p.contactId)
            .OnDelete(DeleteBehavior.Cascade);

        // CustomerEmail - Customer (many-to-one)
        modelBuilder.Entity<CustomerEmail>()
            .HasOne(e => e.customer)
            .WithMany(c => c.CustomerEmails)
            .HasForeignKey(e => e.customerId)
            .OnDelete(DeleteBehavior.Cascade);

        // CustomerPhone - Customer (many-to-one)
        modelBuilder.Entity<CustomerPhone>()
            .HasOne(p => p.customer)
            .WithMany(c => c.CustomerPhones)
            .HasForeignKey(p => p.customerId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    // Contact - Customer (many-to-one)
    //    modelBuilder.Entity<Contact>()
    //        .HasOne(c => c.Customer)
    //        .WithMany() // If Customer has a collection of Contacts, use .WithMany(c => c.Contacts)
    //        .HasForeignKey(c => c.Customerid)
    //        .OnDelete(DeleteBehavior.Restrict);

    //    // Email - Contact (many-to-one)
    //    modelBuilder.Entity<Email>()
    //        .HasOne(e => e.contact)
    //        .WithMany(c => c.emails)
    //        .HasForeignKey(e => e.contactId)
    //        .OnDelete(DeleteBehavior.Cascade);

    //    // Phone - Contact (many-to-one)
    //    modelBuilder.Entity<Phone>()
    //        .HasOne(p => p.contact)
    //        .WithMany(c => c.phones)
    //        .HasForeignKey(p => p.contactId)
    //        .OnDelete(DeleteBehavior.Cascade);

    //    // CustomerEmail - Customer (many-to-one)
    //    modelBuilder.Entity<CustomerEmail>()
    //        .HasOne(e => e.customer)
    //        .WithMany(c => c.CustomerEmails)
    //        .HasForeignKey(e => e.customerId)
    //        .OnDelete(DeleteBehavior.Cascade);

    //    // CustomerPhone - Customer (many-to-one)
    //    modelBuilder.Entity<CustomerPhone>()
    //        .HasOne(p => p.customer)
    //        .WithMany(c => c.CustomerPhones)
    //        .HasForeignKey(p => p.customerId)
    //        .OnDelete(DeleteBehavior.Cascade);

    //    base.OnModelCreating(modelBuilder);
    //}

}