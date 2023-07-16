using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Entities.Emails.Commands;
using Application.Interfaces;
using Application.Services;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Emails.Handlers
{
    public class SendUserEmailCommandHandler : IRequestHandler<SendUserEmailCommand,IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;

        public SendUserEmailCommandHandler(IUserRepository userRepository, IMailService mailService)
        {
            _userRepository = userRepository;
            _mailService = mailService;
        }

        public async Task<IActionResult> Handle(SendUserEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                return new NotFoundObjectResult("User with ID " + request.UserId + " not found");
            }

            string to = user.Name;
            string toAddress = user.Email;
            string subject = request.Subject;
            string body = request.Body;
            await _mailService.SendEmailAsync(toAddress, to, subject, body);

            return new NoContentResult();
        }
    }
}
