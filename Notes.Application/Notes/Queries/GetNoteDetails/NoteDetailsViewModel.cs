using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Notes.Queries.GetNoteDetails
{
    public class NoteDetailsViewModel : IMapWith<Note>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? EditeDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Note, NoteDetailsViewModel>()
                .ForMember(noteViewModel => noteViewModel.Title, opt => opt.MapFrom(note => note.Title))
                .ForMember(noteViewModel => noteViewModel.Details, opt => opt.MapFrom(note => note.Details))
                .ForMember(noteViewModel => noteViewModel.Id, opt => opt.MapFrom(note => note.Id))
                .ForMember(noteViewModel => noteViewModel.CreationDate, opt => opt.MapFrom(note => note.CreationDate))
                .ForMember(noteViewModel => noteViewModel.EditeDate, opt => opt.MapFrom(note => note.EditDate));
        }
    }
}
