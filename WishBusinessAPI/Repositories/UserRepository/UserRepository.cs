using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using WishBusinessAPI.Common;
using WishBusinessAPI.Models;
using WishBusinessAPI.Models.Response;

namespace WishBusinessAPI.Repositories.UserRepository
{
    public class UserRepository: IUserRepository
    {
        private readonly IConfiguration _configuration;

        bool isSuccess=false;
        TranCodes tranCodes = TranCodes.Exception;
        string Message =string.Empty;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public GetUserResponse GetUser()
        {
            /* ........................................................................
              PROCEDURE `PCR_USERS_GET`(
					OUT `PCODE` VARCHAR(2), 
                    OUT `PDESC` VARCHAR(1000), 
                    OUT `PMSG` VARCHAR(2)
             )
          ........................................................................ */
            DataTable dataTable = new DataTable();
            MySqlCommand command = new MySqlCommand();
            MySqlConnection connection = new MySqlConnection();

            List<User> UserList = new List<User>();

            try
            {
                using (command = new MySqlCommand("PCR_USERS_GET",
                    connection = new MySqlConnection(_configuration.GetConnectionString("DB_CONNECTION"))))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "PCODE",
                        MySqlDbType = MySqlDbType.VarChar,
                        Direction = ParameterDirection.Output,
                        Size = 1000
                    });
                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "PDESC",
                        MySqlDbType = MySqlDbType.VarChar,
                        Direction = ParameterDirection.Output,
                        Size = 1000
                    });
                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "PMSG",
                        MySqlDbType = MySqlDbType.VarChar,
                        Direction = ParameterDirection.Output,
                        Size = 1000
                    });

                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    adapter.Fill(dataTable);
                    connection.Close();

                    if (Convert.ToString(command.Parameters["PCODE"].Value) == "00" || Convert.ToString(command.Parameters["PCODE"].Value) == "0")
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            foreach (DataRow row in dataTable.Rows)
                            {
                                User user = new User();

                                user.userId = Convert.ToInt32(row["User_Id"]);
                                user.userName = Convert.ToString(row["User_Name"]);
                                user.userMobile = Convert.ToString(row["Mobile"]);
                                user.userPassword = Convert.ToString(row["uPassword"]);
                                user.code = Convert.ToString(row["Invitation_Code"]);

                                UserList.Add(user);
                            }

                            isSuccess = true;
                            tranCodes = TranCodes.Success;
                            Message = Convert.ToString(command.Parameters["PDESC"].Value);
                        }
                        else
                        {
                            Message = Convert.ToString(command.Parameters["PDESC"].Value);
                        }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                if (connection != null)
                    { connection.Close();
                    connection.Dispose();
                    }
            }

            return new GetUserResponse { isSuccess = isSuccess, Message=Message, tranCodes = tranCodes, Data = UserList };
        }
    }
}
