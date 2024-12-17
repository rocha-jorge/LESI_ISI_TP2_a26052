using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using BitcoinApp.Models;
using System;

namespace BitcoinApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BTCPrice> BtcPrice { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<User> User { get; set; }
    }
}
