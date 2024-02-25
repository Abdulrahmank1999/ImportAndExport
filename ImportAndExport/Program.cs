using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ImportAndExport
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GetCSV();
            ImportCsv();
        }

        private static string GetCSV()
        {
            using (SqlConnection cn = new SqlConnection(GetConnectionString()))
            {
                cn.Open();

                return CreateCSV(new SqlCommand("select * from [dbo].[Customers]", cn)
                    .ExecuteReader());

            }
        }

        private static void ImportCsv()
        {
            using (SqlConnection cn = new SqlConnection(GetConnectionString()))
            {
                cn.Open();
                using (StreamReader reader = new StreamReader(@"C:\Users\96279\Desktop\MZNTask\ImportAndExport\Import\ExportedData.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        
                            var values = line.Split(',');

                            var sql = "INSERT INTO [MZNTask].[dbo].[Customers] VALUES ('" + values[0] + "','" + values[1] + "')";

                            var cmd = new SqlCommand();
                            cmd.CommandText = sql;
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.Connection = cn;
                            cmd.ExecuteNonQuery();
                     
                    }
                }

                cn.Close();

            }
        }

        private static string CreateCSV(IDataReader reader)
        {
            string file = @"C:\\Users\\96279\\Desktop\\MZNTask\\ImportAndExport\\Export\\ExportedData.csv";
            string headerLine = "";

            List<string> lines= new List<string>();

            if (reader.Read())
            {
                string[] columns = new string[reader.FieldCount];

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columns[i] = (string)reader.GetValue(i);

                }

                headerLine = string.Join(",", columns);
                lines.Add(headerLine);

            }

                while (reader.Read())
                {
                    object[] values=new object[reader.FieldCount];
                    reader.GetValues(values);
                    lines.Add(string.Join(",", values));
                }

            

            System.IO.File.WriteAllLines(file,lines);

                return file;
            
        }

        private static string GetConnectionString()
        {
            return @"server=localhost;DataBase=MZNTask;Integrated Security=true";
        }
    }
}
