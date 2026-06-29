using System.Text.Json;
using System.Xml.Linq;
using GetteGarage.Data;
using GetteGarage.Models;

namespace GetteGarage.Services
{
    public class BlogService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public BlogService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<List<BlogPost>> GetPostsAsync(bool forceRefresh = false)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<GameDbContext>();

            // 1. CHECK THE CACHE 
            var cache = db.BlogCache.FirstOrDefault(c => c.Id == 1);
            
            if (cache != null && !forceRefresh && (DateTime.UtcNow - cache.LastUpdated).TotalHours < 24)
            {
                try
                {
                    var cachedPosts = JsonSerializer.Deserialize<List<BlogPost>>(cache.SerializedPosts);
                    if (cachedPosts != null && cachedPosts.Any())
                    {
                        return cachedPosts; 
                    }
                }
                catch { /* If deserialization fails, fall through to fetch fresh data */ }
            }

            // 2. FETCH FRESH DATA DIRECTLY FROM THE RSS FEED 
            string targetUrl = "https://rss-proxy.gettegarage.com/?url=https://gettegarage.substack.com/feed";
            var freshPosts = new List<BlogPost>();

            try 
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(180); // Don't hang forever
                
                // Set a real browser User-Agent so Substack doesn't reject the request
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                
                // Get the raw XML string from the RSS feed
                var xmlContent = await client.GetStringAsync(targetUrl);
                
                // Parse the XML
                var doc = XDocument.Parse(xmlContent);
                
                // Define standard namespaces used in RSS feeds
                XNamespace contentNs = "http://purl.org/rss/1.0/modules/content/";
                XNamespace mediaNs = "http://search.yahoo.com/mrss/";

                var items = doc.Descendants("item").Select(item => 
                {
                    // Map the enclosure node if it exists
                    var enclosureEl = item.Element("enclosure");
                    EnclosureObject? enclosure = null;
                    if (enclosureEl != null)
                    {
                        enclosure = new EnclosureObject
                        {
                            Link = enclosureEl.Attribute("url")?.Value ?? "",
                            Type = enclosureEl.Attribute("type")?.Value ?? ""
                        };
                    }

                    // Substack puts full HTML inside content:encoded
                    var contentElement = item.Element(contentNs + "encoded");
                    string content = contentElement?.Value ?? item.Element("description")?.Value ?? "";

                    // Attempt to locate an image in media:content or media:thumbnail
                    var mediaContentEl = item.Element(mediaNs + "content");
                    var mediaThumbnailEl = item.Element(mediaNs + "thumbnail");
                    string thumbnail = mediaContentEl?.Attribute("url")?.Value 
                                        ?? mediaThumbnailEl?.Attribute("url")?.Value 
                                        ?? "";

                    return new RssItem
                    {
                        Title = item.Element("title")?.Value ?? "",
                        Link = item.Element("link")?.Value ?? "",
                        PubDate = item.Element("pubDate")?.Value ?? "",
                        Description = item.Element("description")?.Value ?? "",
                        Content = content,
                        Thumbnail = thumbnail,
                        Enclosure = enclosure ?? new EnclosureObject()
                    };
                }).ToList();

                // Re-create the parent data container 
                var data = new RssRoot { Items = items };
                
                if (data?.Items != null)
                {
                    freshPosts = data.Items.Select(item => 
                    {
                        // --- SLUG LOGIC ---
                        string slug = "post";
                        try 
                        {
                            if (!string.IsNullOrEmpty(item.Link)) {
                                var uri = new Uri(item.Link);
                                slug = uri.Segments.Last().Trim('/');
                            }
                        }
                        catch { slug = Math.Abs(item.Title.GetHashCode()).ToString(); }

                        // --- IMAGE LOGIC ---
                        string imageUrl = "";
                        // Try the Enclosure Link first
                        if (item.Enclosure != null && !string.IsNullOrWhiteSpace(item.Enclosure.Link))
                        {
                            imageUrl = item.Enclosure.Link;
                        }
                        // Fallback to Thumbnail if Enclosure is missing
                        else if (!string.IsNullOrWhiteSpace(item.Thumbnail))
                        {
                            imageUrl = item.Thumbnail;
                        }

                        string content = item.Content ?? "";

                        // --- FIX SUBSTACK AUDIO & VIDEO EMBEDS ---

                        //1. Process Audio
                        content = ProcessMediaEmbed(
                            content, 
                            "AudioPlaceholder", 
                            "native-audio-embed", 
                            "https://gettegarage.substack.com/api/v1/audio/upload/{0}/src", 
                            "AUDIO CLIP", 
                            "<audio controls style='width: 100%; outline: none;'><source src='{0}' type='audio/mpeg'></audio>"
                        );

                        // 2. Process Video
                        content = ProcessMediaEmbed(
                            content, 
                            "VideoPlaceholder", 
                            "native-video-embed", 
                            "https://gettegarage.substack.com/api/v1/video/upload/{0}/src", 
                            "VIDEO CLIP", 
                            "<video controls style='width: 100%; border-radius: 4px; outline: none;'><source src='{0}' type='video/mp4'></video>"
                        );

                        return new BlogPost
                        {
                            Title = item.Title,
                            Link = item.Link,
                            PubDate = DateTime.TryParse(item.PubDate, out var date) ? date : DateTime.UtcNow,
                            Summary = item.Description, 
                            Content = content, // Use the fixed HTML
                            ImageUrl = imageUrl, 
                            Slug = slug
                        };
                    }).ToList();
                }

                foreach(var post in freshPosts)
                {
                    Console.WriteLine($"[DEBUG] Title: {post.Title} | ImageUrl: {post.ImageUrl}");
                }
            }
            catch (Exception ex)
            {
                if (cache != null)
                {
                    var stalePosts = JsonSerializer.Deserialize<List<BlogPost>>(cache.SerializedPosts);
                    if (stalePosts != null) return stalePosts;
                }

                return new List<BlogPost> { new BlogPost { Title = "Network Error", Summary = ex.Message, Slug = "error" } };
            }

            // 3. SAVE TO CACHE
            if (freshPosts.Any())
            {
                if (cache == null)
                {
                    cache = new BlogCacheRecord { Id = 1 };
                    db.BlogCache.Add(cache);
                }

                cache.LastUpdated = DateTime.UtcNow;
                cache.SerializedPosts = JsonSerializer.Serialize(freshPosts);
                
                await db.SaveChangesAsync();
            }

            return freshPosts;
        }

        class RssRoot { public List<RssItem>? Items { get; set; } }
        
        class RssItem 
        { 
            public string Title { get; set; } = "";
            public string Link { get; set; } = "";
            public string PubDate { get; set; } = "";
            public string Description { get; set; } = "";
            public string Content { get; set; } = "";
            public string Thumbnail { get; set; } = "";
            public EnclosureObject Enclosure { get; set; } = default!;
        }

        class EnclosureObject
        {
            public string Link { get; set; }  = "";
            public string Type { get; set; }  = "";
        }

        private string ProcessMediaEmbed(string htmlContent, string componentName, string className, string urlTemplate, string retroHeader, string html5PlayerTemplate)
        {
            try
            {
                int searchIndex = 0;
                int safetyLimit = 10; // Handle multiple clips in one post

                while (safetyLimit > 0)
                {
                    safetyLimit--;

                    // 1. Find the occurrence of the placeholder name
                    int componentIndex = htmlContent.IndexOf(componentName, searchIndex);
                    if (componentIndex == -1) break;

                    // 2. Find the bounds of the containing <div>
                    // Look backwards for the start of the tag
                    int tagStart = htmlContent.LastIndexOf("<div", componentIndex);
                    // Look forwards for the end of the closing tag
                    int tagEnd = htmlContent.IndexOf("</div>", componentIndex) + "</div>".Length;

                    if (tagStart == -1 || tagEnd == -1) break;

                    // 3. Extract the tag content to find the JSON data-attrs
                    string fullTag = htmlContent.Substring(tagStart, tagEnd - tagStart);
                    
                    // Extract JSON block (between { and })
                    int jsonStart = fullTag.IndexOf("{");
                    int jsonEnd = fullTag.LastIndexOf("}");

                    if (jsonStart != -1 && jsonEnd != -1)
                    {
                        string rawJson = fullTag.Substring(jsonStart, jsonEnd - jsonStart + 1)
                                                .Replace("&quot;", "\"");

                        using var doc = JsonDocument.Parse(rawJson);
                        if (doc.RootElement.TryGetProperty("mediaUploadId", out var idProp))
                        {
                            string uploadId = idProp.GetString() ?? "";
                            if (!string.IsNullOrEmpty(uploadId))
                            {
                                // 4. Build the custom player HTML
                                string mediaUrl = string.Format(urlTemplate, uploadId);
                                string playerHtml = string.Format(html5PlayerTemplate, mediaUrl);

                                string nativeEmbed = $@"
                                    <div class='custom-media-player my-6 pa-4' style='background: rgba(255,255,255,0.02); border: 1px dashed #f6a91f; border-radius: 4px;'>
                                        <p style='color: #f6a91f; font-family: ""Press Start 2P"", cursive; font-size: 0.6rem; margin-bottom: 12px; margin-top: 0;'>{retroHeader}</p>
                                        {playerHtml}
                                    </div>";

                                // 5. SWAP THE HTML
                                // Remove the old tag and insert the new player at the exact same spot
                                htmlContent = htmlContent.Remove(tagStart, tagEnd - tagStart);
                                htmlContent = htmlContent.Insert(tagStart, nativeEmbed);

                                // Move the searchIndex past the newly inserted content
                                searchIndex = tagStart + nativeEmbed.Length;
                                continue; // Look for next clip
                            }
                        }
                    }
                    
                    // If we found a component but failed to parse it, move index forward to avoid infinite loop
                    searchIndex = tagEnd;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($">>> MEDIA_PARSER_ERROR: {ex.Message}");
            }

            return htmlContent;
        }
    }

    public class BlogPost
    {
        public string Title { get; set; } = "";
        public string Link { get; set; } = "";
        public DateTime PubDate { get; set; }
        public string Summary { get; set; } = "";
        public string Content { get; set; } = "";
        public string Slug { get; set; } = "";
        public string ImageUrl { get; set; } = "";
    }
}