using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using WishBusinessAPI.Models;
using WishBusinessAPI.Models.Requests;
using WishBusinessAPI.Models.Response;

namespace WishBusinessAPI.Repositories.Payment
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IConfiguration _configuration;
        bool isSuccess = false;
       // TranCodes tranCodes = TranCodes.Exception;
        string Message = string.Empty;

        public PaymentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public GetPaymentResponse AddPaymentMethod(AddPaymentRequest payment)
        {
            /* ........................................................................
            PROCEDURE `PCR_WITHDRAW_METHOD_ADD`(
                        IN `P_NAME` VARCHAR(1000), 
                        IN `P_EMAIL` VARCHAR(1000), 
                        IN `P_USER_ID` VARCHAR(1000), 
		                IN `P_PHONE_NO` VARCHAR(100), 
                        IN `P_ADDRESS` VARCHAR(100), 
                        IN `P_WITHDRAWL_TYPE` VARCHAR(200), 
                        IN `P_NETWORK` VARCHAR(200),
                        IN `P_USDT_ADDRESS` VARCHAR(200),
                        IN `P_TRAN_PASS` VARCHAR(200),

		                OUT `PCODE` VARCHAR(2), 
                        OUT `PDESC` VARCHAR(1000), 
                        OUT `PMSG` VARCHAR(2)
            )
          ........................................................................ */
            #region variable
            DataTable dataTable = new DataTable();
            MySqlCommand cmd = null;
            MySqlConnection con = null;
            string Message = string.Empty;
           // TranCodes tranCode = TranCodes.Exception;

            #endregion variable

            try
            {
                var aa = _configuration.GetConnectionString("CONN_STR");
                using (cmd = new MySqlCommand("PCR_WITHDRAW_METHOD_ADD", con = new MySqlConnection(_configuration.GetConnectionString("DB_CONNECTION"))))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //input

                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_NAME", Value = payment.real_Name, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_EMAIL", Value = payment.e_mail, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_USER_ID", Value = payment.user_id, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_PHONE_NO", Value = payment.phone_no, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_ADDRESS", Value = payment.address, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_WITHDRAWL_TYPE", Value = payment.withdrawl_type, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_NETWORK", Value = payment.network, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_USDT_ADDRESS", Value = payment.USDT_address, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_TRAN_PASS", Value = payment.tran_pass, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    
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

            return new GetPaymentResponse { isSuccess = isSuccess, Message = Message };
        }
    }
}
