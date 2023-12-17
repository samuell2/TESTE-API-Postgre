using System.Data;

using System.Data.SqlClient;
using System;
using WebApplication1.Model;
using Npgsql;

namespace WebApplication1.Services
{
    public class FreteAdivisor
    {

        public string ReturnBit (bool param)
        {
            if (param) {
                return "S";
            }
            else return "N";
        }

        //string connectionStringLocalhost = @"Data Source=CARLOSRODRIGUES\SQLEXPRESS;Initial Catalog=DB_AVANCADO;User ID=USERCSHARP;Password=USERCSHARP;Integrated Security=SSPI;TrustServerCertificate=True";
        string connectionString = @"User ID=postgres;Password=Faduk@12;Host=localhost;Port=4957;Database=Teste;Pooling=true;Connection Lifetime=0;";



        public bool sendRequest(Frete data)
        {
            try
            {
                NpgsqlConnection conn = new(connectionString);

                DataTable dt1 = new DataTable();
                using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(@"INSERT INTO TESTE_FRETE (PESO, QUEBRAVEL, UF) VALUES (@PESO, @QUEBRAVEL, @UF) ", conn))
                {
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add(new NpgsqlParameter("@PESO", Convert.ToInt32(data.Peso)));
                    adp.SelectCommand.Parameters.Add(new NpgsqlParameter("@QUEBRAVEL",ReturnBit(data.Quebravel)));
                    adp.SelectCommand.Parameters.Add(new NpgsqlParameter("@UF", data.UF));
       
                    adp.Fill(dt1);

                    if (dt1.Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception("Não foi possivel criar a solicitação!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
     }   
}