using Domain.Common.Models;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
