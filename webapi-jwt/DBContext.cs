using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class DBContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public string DbPath { get; }

    public DBContext()
    {

        // This will get the current WORKING directory (i.e. \bin\Debug)
        string workingDirectory = Environment.CurrentDirectory;
        // or: Directory.GetCurrentDirectory() gives the same result

        // This will get the current PROJECT bin directory (ie ../bin/)
        // string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

        // This will get the current PROJECT directory
        // string projectDirectory = Directory.GetParent(workingDirectory).FullName;
        Console.WriteLine(workingDirectory);
        // Console.WriteLine(projectDirectory);
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(workingDirectory, "webapi-jwt.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

[Index(nameof(User.Username), IsUnique = true)]
public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}
