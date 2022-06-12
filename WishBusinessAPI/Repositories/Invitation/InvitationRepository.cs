using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Data;
using WishBusinessAPI.Models.Requests;
using WishBusinessAPI.Models.Response;

namespace WishBusinessAPI.Repositories.Invitation
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly IConfiguration _configuration;
        bool isSuccess = false;
        // TranCodes tranCodes = TranCodes.Exception;
        string Message = string.Empty;

        public InvitationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public InvitationResponse AddInvitation(InvitationRequest request)
        {
            /*..........................................................
                    PROCEDURE `PCR_INVITATION_DETAILS_ADD`(
                            IN `P_INVITATION_ID` VARCHAR(1000), 
		                    IN `P_USERID` VARCHAR(100), 
                            IN `P_INVITATION_CODE` VARCHAR(100), 
                            IN `P_SHARE_ADDRESS` VARCHAR(200), 
        
                            OUT `PCODE` VARCHAR(20), 
                            OUT `PDESC` VARCHAR(1000), 
                            OUT `PMSG` VARCHAR(20))
            ..........................................................*/

            #region variables
            DataTable dt = new DataTable();
            MySqlCommand cmd = null;
            MySqlConnection conn = null;
            string Message = string.Empty;
            #endregion variable

            try
            {
                var aa = _configuration.GetConnectionString("CONN_STR");
                using (cmd = new MySqlCommand("PCR_INVITATION_DETAILS_ADD", conn = new MySqlConnection(_configuration.GetConnectionString("DB_CONNECTION"))))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //input

                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_INVITATION_ID", Value = request.invitationId, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_USERID", Value = request.userId, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_INVITATION_CODE", Value = request.invitationCode, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_SHARE_ADDRESS", Value = request.shareAddress, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                   
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "PCODE", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Size = 1000 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "PDESC", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Size = 1000 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "PMSG", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Size = 1000 });


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

            return new InvitationResponse { isSuccess = isSuccess, Message = Message };
        }
    }
}
