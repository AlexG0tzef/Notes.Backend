using MediatR;
using Notes.Application.Interfaces;
using Notes.Application.Common.Exceptions;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.DeleteNote
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand>
    {
        private readonly INoteDbContext _dbContext;
        public DeleteNoteCommandHandler(INoteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteNoteCommand reques, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Notes.FindAsync(new object[] { reques.Id }, cancellationToken);
            if (entity == null || entity.UserID != reques.UserID)
            {
                throw new NotFoundException(nameof(Note), reques.Id);
            }
            else
            {
                _dbContext.Notes.Remove(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}
