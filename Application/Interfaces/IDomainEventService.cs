using AdminPanel.Domain.Common.Models;
using System.Threading.Tasks;

namespace AdminPanel.Application.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
