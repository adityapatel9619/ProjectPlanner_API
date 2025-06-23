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

        /// <summary>
        /// Get Department List
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Register New Employee In System
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<NewEmployeeModel> SaveNewEmployee(NewEmployeeModel model)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionstring))
                {
                    SqlCommand save = new SqlCommand("PP_NewEmployee_Register_INSERT", conn);
                    save.CommandType = CommandType.StoredProcedure;
                    save.Parameters.Add("@sName", SqlDbType.VarChar, 100).Value = model.sName;
                    save.Parameters.Add("@sEmail", SqlDbType.VarChar, 100).Value = model.sEmail;
                    save.Parameters.Add("@sPhone", SqlDbType.VarChar,12).Value = model.sPhone;
                    save.Parameters.Add("@nDept", SqlDbType.Int).Value = Convert.ToInt32(model.nDept);

                    await conn.OpenAsync();

                    SqlDataReader reader = await save.ExecuteReaderAsync();

                    reader.Close();
                    conn.Close();
                }

                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
