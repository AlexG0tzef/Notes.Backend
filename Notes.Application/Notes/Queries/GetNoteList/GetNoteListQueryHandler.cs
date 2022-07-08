using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;

namespace Notes.Application.Notes.Queries.GetNoteList
{
    public class GetNoteListQueryHandler : IRequestHandler<GetNoteListQuery, NoteListViewModel>
    {
        private readonly INoteDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetNoteListQueryHandler(INoteDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<NoteListViewModel> Handle(GetNoteListQuery request, CancellationToken cancellationToken)
        {
            var notesQuery = new List<NoteLookUpDTO>();
            try
            {
                notesQuery = await _dbContext.Notes
                    .Where(note => note.UserID == request.UserID)
                    .ProjectTo<NoteLookUpDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            { 
            }
            return new NoteListViewModel() { Notes = notesQuery };
        }
    }
}
