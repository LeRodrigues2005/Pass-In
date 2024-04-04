using System.Net.Mail;
using PassIn.Application.UseCases.Events.Register;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.RegisterAttendee
{
    public class RegisterAttendeesOnEventUseCase
    {
        private readonly PassInDbContext _dbContext;
        public RegisterAttendeesOnEventUseCase()
        {
            _dbContext = new PassInDbContext();           
        }
        public ResponseRegisteredJson Execute(Guid eventId, RequestRegisterEventJson request)
        {
            Validate(eventId, request);

            var entity = new Infrastructure.Entities.Attendee
            {
                Email = request.Email,
                Name = request.Name,
                Event_Id = eventId,
                Created_At = DateTime.UtcNow,
            };

            _dbContext.Attendees.Add(entity);
            _dbContext.SaveChanges();

            return new ResponseRegisteredJson
            {
                Id = entity.Id,
            };
        }

        private void Validate(Guid eventId, RequestRegisterEventJson request)
        {
            var eventEntity = _dbContext.Events.Find(eventId);
            if (eventEntity is null)
                throw new NotFoundException("An event with this id don't exist.");

            if(string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ErrorOnValidationException("The name is invalid.");
            }

            var emailIsValid = EmailIsValid(request.Email);
            if(emailIsValid == false)
            {
                throw new ErrorOnValidationException("The e-mail is invalid.");
            }

            // verificar se o participante requisitou o email e id corretos do banco de dados:
            var attendeeAlreadyRegistered = _dbContext.Attendees.Any(attendee => attendee.Email.Equals(request.Email) && attendee.Event_Id == eventId);

            if (attendeeAlreadyRegistered)
            {
                throw new ConflictException("You cannot register twice on the same event.");
            }

            // retorna quantos participantes já estão registrados para esse evento:
            var attendeesForEvent = _dbContext.Attendees.Count(attendee => attendee.Event_Id == eventId); 
            if(attendeesForEvent == eventEntity.Maximum_Attendees) 
            {
                throw new ErrorOnValidationException("There is no room for this event");
            }
        }
        private bool EmailIsValid(string email)
        {
            try{
                new MailAddress(email);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}