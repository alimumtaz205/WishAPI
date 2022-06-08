using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Data;
using WishBusinessAPI.Common;
using WishBusinessAPI.Models;
using WishBusinessAPI.Models.Response;

namespace WishBusinessAPI.Repositories.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _configuration;

        bool isSuccess = false;
        TranCodes tranCodes = TranCodes.Exception;
        string Message = string.Empty;

        public AccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public GetUserResponse Login(LoginDTO login)
        {
            /*.........................................................
                * PROCEDURE `PCR_USER_LOGIN`(
                            IN `P_USERNAME` VARCHAR(1000), 
                            IN `P_PASSWORD` VARCHAR(1000), 
                            OUT `PCODE` VARCHAR(20), 
                            OUT `PDESC` VARCHAR(1000), 
                            OUT `PMSG` VARCHAR(20)
            ) 
            .........................................................*/
            DataTable dt = new DataTable();
            MySqlCommand cmd = null;
            MySqlConnection conn = null;

            try
            {
                var connection = _configuration.GetConnectionString("DB_CONNECTION");
                using (cmd = new MySqlCommand("PCR_USER_LOGIN", conn = new MySqlConnection(_configuration.GetConnectionString("DB_CONNECTION"))))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_USERNAME", Value = login.userName, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 1000 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_PASSWORD", Value = login.userPassword, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });

                    //output
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "pCODE", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "pDESC", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "pMSG", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Size = 100 });

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

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
                if (conn != null) { conn.Close(); conn.Dispose(); }
            }

            return new GetUserResponse { isSuccess = isSuccess, Message = Message };
        }
    }
}
