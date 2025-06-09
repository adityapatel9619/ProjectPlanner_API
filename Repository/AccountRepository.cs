using ProjectPlanner_API.IMethod;
using ProjectPlanner_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Reflection;

namespace ProjectPlanner_API.Repository
{
    public class AccountRepository : IAccount
    {
        private readonly string _connectionstring;

        public AccountRepository(ConnectionStringProvider connectionStringProvider)
        {
            _connectionstring = connectionStringProvider.GetConnection;
        }


        /// <summary>
        /// Validate username 
        /// </summary>
        /// <param name="username">adityapatel777</param>
        /// <returns></returns>
        public async Task<List<RegistrationModel>> ValidateUsername(string username)
        {
            try
            {
                List<RegistrationModel> usernameList = new List<RegistrationModel>();

                using (SqlConnection conn = new SqlConnection(_connectionstring))
                {
                    SqlCommand validate = new SqlCommand("PP_Register_VALIDATE_Username", conn);
                    validate.CommandType = CommandType.StoredProcedure;
                    validate.Parameters.Add("@sUsername", SqlDbType.VarChar, 200).Value = username;

                    await conn.OpenAsync();

                    SqlDataReader reader = await validate.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        RegistrationModel usernamedetail = new RegistrationModel()
                        {
                            UserName = string.IsNullOrEmpty(reader["sUserName"].ToString()) ? "" : reader["sUserName"].ToString(),
                            Email = ""
                        };

                        usernameList.Add(usernamedetail);
                    }
                    reader.Close();

                    conn.Close();
                }

                return usernameList;                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DepartmentModel>> GetDepartments()
        {
            try
            {
                var departments = new List<DepartmentModel>();

                using (SqlConnection conn = new SqlConnection(_connectionstring))
                {
                    SqlCommand cmd = new SqlCommand("PP_Department_GET_API", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            departments.Add(new DepartmentModel
                            {
                                nId = Convert.ToInt32(reader["nID"]),
                                sDeptName = reader["sDeptName"].ToString()
                            });
                        }
                    }
                    return departments;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
