using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
	public class Result
	{
		public string Message { get; set; }
		public bool IsSuccess { get; set; }
		public object Data { get; set; }
	}
}
