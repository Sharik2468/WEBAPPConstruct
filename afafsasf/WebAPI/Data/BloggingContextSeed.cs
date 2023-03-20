namespace WebAPI.Models
{
    public static class BloggingContextSeed
    {
        public static async Task SeedAsync(BloggingContext context)
        {
            try
            {
                context.Database.EnsureCreated();

                if (context.Blog.Any())
                {
                    return;
                }

                var blogs = new Blog[]
                {
                new Blog{Url="https://devblogs.microsoft.com/dotnet/"},
                new Blog{Url="https://devblogs.microsoft.com/dotnet/category/aspnet/"},
                new Blog{Url="https://devblogs.microsoft.com/visualstudio/"}

                };
                foreach (Blog b in blogs)
                {
                    context.Blog.Add(b);
                }
                await context.SaveChangesAsync();

                var posts = new Post[]
                {
                new Post { BlogId=1,Content="Since .NET 6, we updated the WinForms runtime to support and improve the Visual Basic Application Framework. In Visual Studio 2022, we also modernized the related user experience. Time to take a closer look how all this works behind the scenes, lets you move from .NET Framework to .NET 6/7+ and provides a great opportunity to modernize your WinForms Visual Basic Apps!",Title="What’s new for the WinForms Visual Basic Application Framework" },
                new Post { BlogId=1,Content="February 2023 Security and Quality Rollup Updates for .NET Framework",Title=".NET Framework February 2023 Security and Quality Rollup Updates" }
                };
                foreach (Post p in posts)
                {
                    context.Post.Add(p);
                }
                await context.SaveChangesAsync();

            }
            catch
            {
                throw;
            }
        }
    }
}