using Microsoft.EntityFrameworkCore;
using TicketTracker.Models;

namespace TicketTracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        UseLowercaseNames(modelBuilder);
        base.OnModelCreating(modelBuilder);
        
        //User
        modelBuilder.Entity<User>(e =>
        {
            e.Property(p => p.Email).IsRequired();
            e.HasIndex(u => u.Email)
                .IsUnique();

            e.Property(p => p.Role)
                .HasDefaultValue("Client");
            
            //we have two relationships from Ticket to User
            //EF Core cannot automatically guess how to map two navigation properties pointing to the same entity type (User) —
            //it gets confused, because both are to User, and it doesn't know which foreign key belongs to which navigation.
            e.HasMany(u => u.CreatedTickets)
                .WithOne(t => t.CreatedBy)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            
            e.HasMany(u => u.AssignedTickets)
                .WithOne(t => t.AssignedTo)
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);
            
            //There’s only one relationship from Comment to User, so EF Core can automatically match
        });
        
        //Ticket
        modelBuilder.Entity<Ticket>(e =>
        {
            e.Property(p => p.Status)
                .HasDefaultValue("Open");
            
            e.HasMany(t => t.Comments)
                .WithOne(c => c.Ticket)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(t => t.Attachments)
                .WithOne(a => a.Ticket)
                .OnDelete(DeleteBehavior.Cascade);

        });
        
        //Comment
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.CreatedBy)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
    
    private void UseLowercaseNames(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()!.ToLowerInvariant());

            foreach (var property in entity.GetProperties())
                property.SetColumnName(property.Name.ToLowerInvariant());

            foreach (var key in entity.GetKeys())
                key.SetName(key.GetName()!.ToLowerInvariant());

            foreach (var fk in entity.GetForeignKeys())
                fk.SetConstraintName(fk.GetConstraintName()!.ToLowerInvariant());

            foreach (var index in entity.GetIndexes())
                index.SetDatabaseName(index.GetDatabaseName()!.ToLowerInvariant());
        }
    }

}