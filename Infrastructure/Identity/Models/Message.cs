﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Models
{
	public class Message
	{
		public int Id { get; set; }

		public string Content { get; set; }

		public bool IsWorker { get; set; }

		public DateTime Date { get; set; }

		//foreign
		public int CorrespondenceId { get; set; }

		//navigation
		public virtual Correspondence Correspondence { get; set; }
	}
}