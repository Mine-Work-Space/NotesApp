using AutoMapper;
using NotesApp.Models;
using NotesApp.Models.UI;

namespace NotesApp
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<Note, NoteUI>();
			CreateMap<NoteUI, Note>()
				// Map from NoteUI.EditedText to Note.Text
				.ForMember(note => note.Text, options => options.MapFrom(src => src.EditableText));
		}
	}
}
