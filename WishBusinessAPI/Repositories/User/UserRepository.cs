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

        public GetUserResponse RegisterUser(User user)
        {
            /*.........................................................
             PROCEDURE `PCR_USER_REGISTERATION`(
			            IN `P_USERNAME` VARCHAR(1000), 
                        IN `P_MOBILE` VARCHAR(1000), 
			            IN `P_PASSWORD` VARCHAR(1000), 
                        IN `P_INVCODE` VARCHAR(1000), 
                        OUT `PCODE` VARCHAR(20), 
                        OUT `PDESC` VARCHAR(1000), 
                        OUT `PMSG` VARCHAR(20)
            )  
            .......................................................*/
            #region variable
            DataTable dataTable = new DataTable();
            MySqlCommand cmd = null;
            MySqlConnection con = null;
            string Message = string.Empty;
            TranCodes tranCode = TranCodes.Exception;

            #endregion variable

            try
            {
                var aa = _configuration.GetConnectionString("CONN_STR");
                using (cmd = new MySqlCommand("PCR_USER_REGISTERATION", con = new MySqlConnection(_configuration.GetConnectionString("DB_CONNECTION"))))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //input
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_USERNAME", Value = user.userName, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_MOBILE", Value = user.userMobile, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_PASSWORD", Value = user.userPassword, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_INVCODE", Value = user.code, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                   

                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "PCODE", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Size = 1000 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "PDESC", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Size = 1000 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "PMSG", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Size = 1000 });


                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (Convert.ToString(cmd.Parameters["PCODE"].Value) == "00" || Convert.ToString(cmd.Parameters["PCODE"].Value) == "0")
                    {
                        isSuccess = true;
                        // tranCode = TranCodes.Success;
                        Message = Convert.ToString(cmd.Parameters["PDESC"].Value);
                    }
                    else
                    {
                        Message = Convert.ToString(cmd.Parameters["PDESC"].Value);
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con != null) { con.Close(); con.Dispose(); }
            }

            return new GetUserResponse { isSuccess = isSuccess, Message = Message};
        }
    }
}
