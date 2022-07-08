using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNoteList
{
    public class NoteLookUpDTO : IMapWith<Note>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Note, NoteLookUpDTO>()
                .ForMember(noteDTO => noteDTO.Id, opt => opt.MapFrom(note => note.Id))
                .ForMember(noteDTO => noteDTO.Title, opt => opt.MapFrom(note => note.Title));
        }
    }
}
