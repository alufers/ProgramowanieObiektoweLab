using System;
using System.Linq;

public class Program
{
    public static void Main()
    {
        // Hint: change `DESKTOP-123ABC\SQLEXPRESS` to your server name
        //       alternatively use `localhost` or `localhost\SQLEXPRESS`

        string connectionString = @"Data Source=localhost;Initial Catalog=blogdb;Integrated Security=True";

        using (BloggingContext db = new BloggingContext(connectionString))
        {
            Console.WriteLine($"Database ConnectionString: {db.ConnectionString}.");

            // Create
            Console.WriteLine("Inserting a new blog");

            db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
            db.SaveChanges();

            // Read
            Console.WriteLine("Querying for a blog");

            Blog blog = db.Blogs
                .OrderBy(b => b.Id)
                .First();

            // Update
            Console.WriteLine("Updating the blog and adding a post");

            blog.Url = "https://devblogs.microsoft.com/dotnet";
            blog.Posts.Add(new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
            db.SaveChanges();

            // Delete
            Console.WriteLine("Delete the blog");

            db.Remove(blog);
            db.SaveChanges();
        }

        using (TasksContext db = new TasksContext(connectionString))
        {
            Random rand = new Random();
            int rando = rand.Next() % 1000;
            string[] surnames = { "Kowalski", "Nowak", "Kowal", "Janowski", "Jackowski", "Testowy"};
            string[] firstNames = { "Jan", "Michał", "Krzysztof", "Filip", "Michalina", "Kamil", "Anna", "Stefan", "Stanisław", "Mateusz", "Łukasz" };
            User user = new User { Username = $"user-{rando}", FirstName = firstNames[rand.Next(firstNames.Length)], LastName = surnames[rand.Next(surnames.Length)] };
            
            Console.WriteLine($"Creating user '{user.Username}' ({user.FirstName} {user.LastName})");

            db.Add(user);
            db.SaveChanges();

            int rolesCount = rand.Next(4) + 1;

            Console.WriteLine($"Adding {rolesCount} roles to the user");

            for(int i = 0; i < rolesCount; i++)
            {
                string[] roleNames = { "Editor", "Viewer", "Cleaner", "Administrator", "Owner", "Maintainer", "Developer", "Guest", "Auditor", "Employee", "Subcontractor" };
                Role role = new Role { Name = roleNames[rand.Next(roleNames.Length)], User = user, UserId = user.Id };
                db.Add(role);
                db.SaveChanges();
            }

            var users = (from c in db.Users select c).ToList();
            Console.WriteLine("Id".PadRight(8) + "Name".PadRight(15) + "First name".PadRight(15) + "Last name".PadRight(15) + "Roles".PadRight(25));
            foreach(var u in users)
            {
                var roles = (from c in db.Roles where c.UserId == u.Id select c).ToList();
                string rolesStr = "";
                foreach(var r  in roles)
                {
                    rolesStr += r.Name + ", ";
                }
                Console.WriteLine(u.Id.ToString().PadRight(8) + u.Username.PadRight(15) + u.FirstName.PadRight(15) + u.LastName.PadRight(15) + rolesStr.PadRight(25));
            }

        }
    }
}