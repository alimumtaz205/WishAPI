using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using WishBusinessAPI.Common;
using WishBusinessAPI.Models;
using WishBusinessAPI.Models.Response;

namespace WishBusinessAPI
{
    public class FinanceRepository: IFinanceRepository
    {
        private readonly IConfiguration _configuration;

        bool isSuccess = false;
        TranCodes tranCodes = TranCodes.Exception;
        string Message = string.Empty;

        public FinanceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public FinanceResponse GetFinance()
        {
            /* ........................................................................
                        PROCEDURE `PCR_FINANCE_GET`(
		                           OUT `PCODE` VARCHAR(2), 
                                   OUT `PDESC` VARCHAR(1000), 
                                   OUT `PMSG` VARCHAR(2)
            )
          ........................................................................ */
            DataTable dataTable = new DataTable();
            MySqlCommand command = new MySqlCommand();
            MySqlConnection connection = new MySqlConnection();

            List<Finance> UserList = new List<Finance>();

            try
            {
                using (command = new MySqlCommand("PCR_FINANCE_GET",
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
                                Finance obj = new Finance();

                                obj.financeId = Convert.ToInt32(row["financeId"]);
                                obj.availabaleBalance = Convert.ToString(row["Available_Balance"]);
                                obj.accountFunds = Convert.ToString(row["Account_funds"]);
                                obj.frozenAmount = Convert.ToString(row["Frozen_Amount"]);
                                obj.financialIncome = Convert.ToString(row["Financial_Income"]);

                                UserList.Add(obj);
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
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return new FinanceResponse { isSuccess = isSuccess, Message = Message, tranCodes = tranCodes, Data = UserList };
        }

        public FinanceResponse AddFinance(Finance request)
        {
            /*.........................................................
            PROCEDURE `PCR_FINANCE_ADD`(
		            IN `P_USERID` VARCHAR(100), 
                    IN `P_FINANCEID` VARCHAR(1000), 
                    IN `P_ACCOUNT_FUNDS` VARCHAR(100), 
                    IN `P_FROZEN_AMOUNT` VARCHAR(200), 
                    IN `P_FINANCIAL_INCOME` VARCHAR(200), 
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
                using (cmd = new MySqlCommand("PCR_FINANCE_ADD", con = new MySqlConnection(_configuration.GetConnectionString("DB_CONNECTION"))))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //input
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_USERID", Value = request.userId, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_FINANCEID", Value = request.financeId, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_ACCOUNT_FUNDS", Value = request.accountFunds, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_FROZEN_AMOUNT", Value = request.frozenAmount, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });
                    cmd.Parameters.Add(new MySqlParameter { ParameterName = "P_FINANCIAL_INCOME", Value = request.financialIncome, MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Input, Size = 100 });

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

            return new FinanceResponse { isSuccess = isSuccess, Message = Message };
        }
    }
}