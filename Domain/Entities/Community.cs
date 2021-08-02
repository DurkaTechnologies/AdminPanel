using AdminPanel.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Domain.Entities
{
	public class Community : AuditableEntity, IHasDomainEvent
    {
        public int Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(64)]
        public string Area { get; set; }

        [MaxLength(16)]
        public string Type { get; set; }

        public DateTime Date { get; set; }

        public float Square { get; set; }

        public int PeopleCount { get; set; }

        public int Count { get; set; }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
