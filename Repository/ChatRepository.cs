using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Entities;
using Entities.Extensions;
using Entities.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChatServerApi.Utility;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ChatServerApi
{
    public class ChatRepository : SqlServerCommands, IChatRepository
    {
		Result result;
		public ChatRepository()
		{
			result = DefaultResult("Successfully!");
		}
		public async Task<Result> Get(Guid senderId, Guid receiverId)
		{
			try
			{
				IEnumerable<Chat> messages;

				string query = "SELECT * FROM tblChat WHERE LOWER(SenderId) = '"
						+ senderId + "' AND LOWER(ReceiverId) = '" + receiverId +
						"' OR LOWER(ReceiverId) = '" + senderId + "' AND LOWER(SenderId) = '" + receiverId + "'";
					DataTable dataTable = await GetData(query);
					messages = Converter.ConvertDataTable<Chat>(dataTable);
					result.Data = messages;
				
			}
			catch (Exception ex)
			{
				result.Message = "UnSuccess";
				result.IsSuccess = false;
				result.Data = ex.Message;
			}

			return await Task.FromResult<Result>(result);
		}

		public async Task<Result> Update(Chat model)
		{
			Result result = DefaultResult("Successfully!");
			try
			{
				string query = "";
				if (model.IsDeleteFromReceiver)
				{
					query = "UPDATE tblChat SET IsDeleteFromReceiver = 1 WHERE ChatId= @Id";
				}
				else
				{
					query = "UPDATE tblChat SET IsDeleteFromSender = 1 WHERE ChatId=@Id";
				}
				SqlParameter[] parameters =
				{
					new SqlParameter("@Id", SqlDbType.BigInt){Value = model.ChatId},
				};

				result = await InsertOrUpdateOrDelete(query, parameters);
			}
			catch (Exception ex)
			{
				result.IsSuccess = false;
				result.Message = "Error:" + ex.InnerException;
			}

			return result;
		}

		public async Task<Result> Delete(long id)
		{
			Result result = DefaultResult("Successfully!");
			try
			{
				string query = "Delete FROM tblChat WHERE ChatId = @Id";
				SqlParameter[] parameters =
				{
					new SqlParameter("@Id", SqlDbType.BigInt){Value = id},
				};
				result.Message = (await InsertOrUpdateOrDelete(query, parameters)).IsSuccess ? "Delete Successfullly" : "Message Not Found!";
				result.Data = null;
			}
			catch (Exception ex)
			{
				result.IsSuccess = false;
				result.Message = "Error:" + ex.InnerException;
			}

			return result;
		}

		public async Task<Result> Save(Chat model)
		{
			try
			{
				DateTime dt = DateTime.Now;
				string time = dt.ToShortTimeString();
				string query = @"INSERT INTO tblChat(ChatId, SenderId,ReceiverId,Message,Date,Time)
output INSERTED.ChatId VALUES((SELECT 1 + coalesce(max(ChatId), 0) FROM tblChat),@SenderId, 
@ReceiverId, @Message,GetDate(), @Time)";

				SqlParameter[] parameters =
				{
					new SqlParameter("@SenderId", SqlDbType.UniqueIdentifier){Value = model.SenderId},
					new SqlParameter("@ReceiverId", SqlDbType.UniqueIdentifier){Value = model.ReceiverId},
				  new SqlParameter("@Message", SqlDbType.VarChar, 500) { Value = model.Message },
				  new SqlParameter("@Time", SqlDbType.VarChar, 15) { Value = time },
				};
				result = await InsertOrUpdateOrDelete(query, parameters);
				if (result.IsSuccess)
				{
					string[] message = new string[2];
					message[0] = result.Message;
					message[1] = time;
					result.Data = message;
				}
			}
			catch (Exception ex)
			{
				result.IsSuccess = false;
				result.Message = "Error:" + ex.InnerException;
			}
			return result;
		}
	}
}