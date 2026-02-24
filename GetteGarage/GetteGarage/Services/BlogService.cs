using System.Xml;
using System.ServiceModel.Syndication;
using System.Text.Json;

namespace GetteGarage.Services
{
    public class BlogService
    {
        public async Task<List<BlogPost>> GetPostsAsync()
{
    // Use rss2json to proxy the request (Bypasses IP Blocks & CORS)
    string targetUrl = "https://gettegarage.substack.com/feed";
    string apiUrl = $"https://api.rss2json.com/v1/api.json?rss_url={targetUrl}";

    try 
    {
        using var client = new HttpClient();
        var json = await client.GetStringAsync(apiUrl);
        
        // Deserializing the JSON response
        var data = System.Text.Json.JsonSerializer.Deserialize<RssRoot>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        

        return data.Items.Select(item => 
        {
        
            string slug = "post";
            try 
            {
                if (!string.IsNullOrEmpty(item.Link))
                {
                    var uri = new Uri(item.Link);
                    // Get the last part of the URL path
                    slug = uri.Segments.Last().Trim('/');
                }
            }
            catch 
            {
                // Fallback: Use Hash of title if URL is weird
                slug = Math.Abs(item.Title.GetHashCode()).ToString();
            }
        return new BlogPost
        {

            
            Title = item.Title,
            Link = item.Link,
            PubDate = DateTime.Parse(item.PubDate),
            Summary = item.Description, // rss2json puts summary in description
            Content = item.Content,
            Slug = slug,
            ImageUrl = item.Thumbnail
        };
    }).ToList();
    }
    catch (Exception ex)
    {
        return new List<BlogPost> { new BlogPost { Title = "Proxy Error", Summary = ex.Message, PubDate = DateTime.Now } };
    }
}

        // Helper classes for JSON
        class RssRoot { public List<RssItem> Items { get; set; } }
        class RssItem 
        { 
            public string Title { get; set; }
            public string Link { get; set; }
            public string PubDate { get; set; }
            public string Description { get; set; }
            public string Content { get; set; }
            public string Thumbnail { get; set; }
        }
            }
}

    public class BlogPost
{
        public string Title { get; set; }
        public string Link { get; set; } // The Substack URL
        public DateTime PubDate { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; } // The full HTML content

        public string Slug { get; set; }
        public string ImageUrl { get; set; }
}