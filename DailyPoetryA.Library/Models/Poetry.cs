namespace DailyPoetryA.Library.Models;

[SQLite.Table("works")]
public class Poetry {
    [SQLite.Column("id")] public int Id { get; set; }

    [SQLite.Column("name")] public string Name { get; set; } = string.Empty;

    [SQLite.Column("author_name")]
    public string Author { get; set; } = string.Empty;

    [SQLite.Column("dynasty")]
    public string Dynasty { get; set; } = string.Empty;

    [SQLite.Column("content")]
    public string Content { get; set; } = string.Empty;

    private string _snippet;

    [SQLite.Ignore]
    public string Snippet =>
        _snippet ??= Content.Split('。')[0].Replace("\r\n", " ");
}