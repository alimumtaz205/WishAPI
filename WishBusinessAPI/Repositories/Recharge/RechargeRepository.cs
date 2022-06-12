using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using WishBusinessAPI.Models;
using WishBusinessAPI.Models.Requests;
using WishBusinessAPI.Models.Response;
using WishBusinessAPI.Repositories.Recharge;

namespace WishBusinessAPI
{
    public class RechargeRepository : IRechargeRepository
    {
        private readonly IConfiguration _configuration;
        bool isSuccess = false;
        // TranCodes tranCodes = TranCodes.Exception;
        string Message = string.Empty;

        public RechargeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RechargeResponse GetRechargeDetails()
        {
            /* ........................................................................
                       PROCEDURE `PCR_RECHARGE_GET`(
		                        OUT `PCODE` VARCHAR(2), 
                                OUT `PDESC` VARCHAR(1000), 
                                OUT `PMSG` VARCHAR(2))
            )
          ........................................................................ */
            DataTable dataTable = new DataTable();
            MySqlCommand command = new MySqlCommand();
            MySqlConnection connection = new MySqlConnection();

            List<Recharge> recharges = new List<Recharge>();

            try
            {
                using (command = new MySqlCommand("PCR_RECHARGE_GET",
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
                                Recharge obj = new Recharge();

                                obj.rechargeId = Convert.ToInt32(row["recharge_id"]);
                                obj.USTDTopup = Convert.ToString(row["USTD_topup"]);
                                obj.receiverAddress = Convert.ToString(row["receiver_add"]);
                                obj.tranId = Convert.ToString(row["t_txid"]);
                                obj.paymentSS = Convert.ToString(row["payment_ss"]);

                                recharges.Add(obj);
                            }

                            isSuccess = true;
                           // tranCodes = TranCodes.Success;
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
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return new RechargeResponse { isSuccess = isSuccess, Message = Message, Data = recharges };
        }

        public RechargeResponse AddRechargeDetails(RechargeRequest request)
        {
            /*..........................................................
                    PROCEDURE `PCR_RECHARGE_ADD`(
                                    IN `p_recharge_id` VARCHAR(1000), 
		                            IN `p_USTD_topup` VARCHAR(100), 
                                    IN `p_receiver_add` VARCHAR(100), 
                                    IN `p_t_txid` VARCHAR(200), 
                                    IN `p_payment_ss` VARCHAR(200),
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
                using (cmd = new MySqlCommand("PCR_RECHARGE_ADD", conn = new MySqlConnection(_configuration.GetConnectionString("DB_CONNECTION"))))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //input

                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "p_recharge_id", Value = request.rechargeId, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "p_USTD_topup", Value = request.USTDTopup, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "p_receiver_add", Value = request.receiverAddress, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "p_t_txid", Value = request.tranId, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "p_payment_ss", Value = request.paymentSS, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });

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

            return new RechargeResponse { isSuccess = isSuccess, Message = Message };
        }
    }
}
