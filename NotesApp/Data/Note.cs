namespace NotesApp.Data
{
    public class Note
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateOnly DateOnly { get; set; }
    }
}
