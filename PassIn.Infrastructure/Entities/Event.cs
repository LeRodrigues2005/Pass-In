using System.ComponentModel.DataAnnotations.Schema;

namespace PassIn.Infrastructure.Entities
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int Maximum_Attendees { get; set; }
        [ForeignKey("Event_Id")] // para não dar erro com o underline na tabela do banco de dados
        public List<Attendee> Attendees { get; set; } = [];
    }
}