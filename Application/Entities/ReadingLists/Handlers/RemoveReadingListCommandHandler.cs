using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Application.Entities.ReadingLists.Commands;
using Application.Interfaces;
using MediatR;

namespace Application.Entities.ReadingLists.Handlers
{
    public class RemoveReadingListCommandHandler : IRequestHandler<RemoveReadingListCommand, bool>
    {
        private readonly IReadingListRepository _readingListRepository;

        public RemoveReadingListCommandHandler(IReadingListRepository readingListRepository)
        {
            _readingListRepository = readingListRepository;
        }

        public async Task<bool> Handle(RemoveReadingListCommand request, CancellationToken cancellationToken)
        {
            var readingList = await _readingListRepository.GetReadingListAsync(request.ReadingListId);
            if(readingList == null)
                return false;
            if(readingList.UserId != request.UserId)
                return false;
            await _readingListRepository.RemoveReadingListAsync(readingList);
            return true;
        }
    }
}
