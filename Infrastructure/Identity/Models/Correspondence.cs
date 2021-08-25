using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Infrastructure.Identity.Models
{
    public class Correspondence
    {
        public int Id { get; set; }

        [Required]
        public string RequestNumber { get; set; }
        public string AskerNumber { get; set; }

        //Foreign
        public string WorkerId { get; set; }

        //Navigation
        public ApplicationUser Worker { get; set; }
        public List<Message> Messages { get; set; }
        public Correspondence()
        {
            Messages = new List<Message>();
        }
    }
}
