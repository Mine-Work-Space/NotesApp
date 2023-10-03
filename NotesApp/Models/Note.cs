﻿using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApp.Models
{
    // Guid can be replaced with simple int if db won't be merged
    public class Note
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [MinLength(1, ErrorMessage = "{0} can not be null")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.MultilineText)]
        [MaxLength(8192, ErrorMessage = "There is so much text")]
        [MinLength(2, ErrorMessage = "{0} must have at least 2 characters")]
        public string Text { get; set; } = string.Empty;
        [NotMapped]
        public string TextMinimized => Text.Length > 150 ? new string(Text.Take(150).ToArray()) : string.Empty;
		[DataType(DataType.Date)]
        public DateOnly CreationDate { get; set; }
        public NpgsqlTsVector? SearchVector { get; set; }
	}
	public class NotesList
    {
        public List<Note>? Notes { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public DisplayType DisplayType { get; set; }
    }
    public enum DisplayType { SearchingByText, Default }
}