using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServerApi.Utility
{
	public class Common
	{
        public Result DefaultResult(string message)
        {
            Result result = new Result();
            result.Message = message;
            result.IsSuccess = true;
            result.Data = null;

            return result;
        }
    }
}
