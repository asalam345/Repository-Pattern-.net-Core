using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Entities;
using Entities.Extensions;
using Entities.Models;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ChatServerApi.Utility;
using Microsoft.AspNetCore.Http;

namespace ChatServerApi
{
    public class UserRepository : SqlServerCommands, IUserRepository
    {
        public async Task<Result> Authenticate(AuthenticateRequest model)
        {
			Result result = DefaultResult("Success");
			try
			{
				string query = "SELECT * FROM tblUser WHERE Email = '" + model.Email + "'";
				DataTable dataTable = await GetData(query);
				var users = Converter.ConvertDataTable<User>(dataTable);
				var user = users != null && users.Count > 0 ? users[0] : null;
				if (user == null) return null;

				var token = generateJwtToken(user.UserId);
				result.Data = new AuthenticateResponse(user, token);
				result.Message = "Success";
			}
			catch (Exception ex)
			{
				result.IsSuccess = false;
				result.Message = ex.InnerException.ToString();
			}

			return result;
        }
        
		public async Task<Result> GetAllUsers()
		{
			Result result = DefaultResult("Success");

			try
			{
				IEnumerable<User> users;
				string query = "SELECT * FROM tblUser";
				DataTable dataTable = await GetData(query);
				users = Converter.ConvertDataTable<User>(dataTable);
				result.Data = users;
			}
			catch (Exception ex)
			{
				result.Message = "UnSuccess";
				result.IsSuccess = false;
				result.Data = ex.Message;
			}

			return await Task.FromResult<Result>(result);
		}

		public async Task<Result> GetUserById(Guid id)
		{
			Result result = DefaultResult("Success");

			try
			{
				IEnumerable<User> users;
				string query = "SELECT * FROM tblUser Where UserId = '" + id + "'";
				DataTable dataTable = await GetData(query);
				users = Converter.ConvertDataTable<User>(dataTable);
				result.Data = users;
			}
			catch (Exception ex)
			{
				result.Message = "UnSuccess";
				result.IsSuccess = false;
				result.Data = ex.Message;
			}
			return await Task.FromResult<Result>(result);
		}

		public async Task<Result> CreateUser(User model)
		{
			Result result = DefaultResult("Registered Successfully!");

			try
			{
				User ui = await isExists(model.Email);
				if (ui.Email != null)
				{
					result.Message = "Email Alreay Exists!";
					result.IsSuccess = false;
					return await Task.FromResult<Result>(result);
				}
				ui = await isExists(model.FirstName, model.LastName);
				if (ui.Email != null)
				{
					result.Message = "Name Alreay Exists!";
					result.IsSuccess = false;
					return await Task.FromResult<Result>(result);
				}

				string query = @"INSERT INTO tblUser(UserId, email,firstname,lastName) VALUES
								(@NEWID,@Email,@FirstName,@LastName)";

				SqlParameter[] parameters =
				{
					new SqlParameter("@NEWID", SqlDbType.UniqueIdentifier){Value = Guid.NewGuid()},
				  new SqlParameter("@Email", SqlDbType.VarChar, 50) { Value = model.Email },
				  new SqlParameter("@FirstName", SqlDbType.VarChar, 50) { Value = model.FirstName },
				  new SqlParameter("@LastName", SqlDbType.VarChar, 50) { Value = model.LastName }
				};

				result = await InsertOrUpdateOrDelete(query, parameters);
				result.Data = model;
			}
			catch (Exception ex)
			{
				result.Message = "Error:" + ex.InnerException;
				result.IsSuccess = false;
				result.Data = ex.Message;
			}
			return await Task.FromResult<Result>(result);
		}
		async Task<User> isExists(string email)
		{
			try
			{
				User user = new User();
				string query = "SELECT * FROM tblUser WHERE Email = '" + email + "'";
				DataTable dataTable = await GetData(query);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					DataRow dr = dataTable.Rows[0];
					user = Converter.GetItem<User>(dr);
				}
				return user;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		async Task<User> isExists(string fristName, string lastName)
		{
			try
			{
				User user = new User();
				string query = "SELECT * FROM tblUser WHERE FirstName = '" + fristName + "' AND LastName = '" + lastName + "'";
				DataTable dataTable = await GetData(query);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					DataRow dr = dataTable.Rows[0];
					user = Converter.GetItem<User>(dr);
				}
				return user;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		private string generateJwtToken(Guid userId)
		{
			// generate token that is valid for 7 days
			var secretKey = MySettings.GetSecretKey();
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secretKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim("id", userId.ToString()) }),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}