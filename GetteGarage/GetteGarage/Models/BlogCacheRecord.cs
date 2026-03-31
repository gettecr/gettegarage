namespace GetteGarage.Models;

public class BlogCacheRecord
{
    public int Id { get; set; } = 1; // We only need one row to store the whole list
    public DateTime LastUpdated { get; set; }
    
    // store the entire List<BlogPost> as a single JSON 
    public string SerializedPosts { get; set; } = "[]"; 
}