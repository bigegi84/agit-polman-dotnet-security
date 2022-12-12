using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class DBContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public string DbPath { get; }

    public DBContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
