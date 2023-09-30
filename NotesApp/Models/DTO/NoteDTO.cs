namespace NotesApp.Models.DTO
{
    public class NoteDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateOnly CreationDate { get; set; }
    }
    public class NotesListDTO
    {
        public List<NoteDTO> Notes { get; set; } = new List<NoteDTO>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
