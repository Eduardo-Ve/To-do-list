namespace To_do_list.models
{
    public class Note
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "Nueva Nota";
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Note() { }
        public Note(string title, string content)
        {
            Title = title;
            Content = content;
            CreatedAt = DateTime.Now;
        }
    }
}