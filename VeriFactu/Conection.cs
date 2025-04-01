using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data;

namespace VeriFactu
{
    internal class Conection
    {
        private static string connectionString = "Uid=indo;Pwd=jrjr8826e2;Dsn=NOG;";
        private static OdbcConnection connection = new OdbcConnection(connectionString);

        // Método para abrir la conexión
        public static DataTable SelectRows(string consultaSQL)
        {
            DataTable sql_table = new DataTable();

            try
            {
                connection.Open();
                using (OdbcDataAdapter command = new OdbcDataAdapter(consultaSQL, connection))
                {
                    command.Fill(sql_table);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al abrir la conexión o ejecutar la consulta ODBC: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return sql_table;
        }

        public static string SelectOnce(string consultaSQL)
        {
            string resultado = string.Empty; // Variable para almacenar el resultado

            try
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand(consultaSQL, connection))
                using (OdbcDataReader reader = command.ExecuteReader()) // Asegura la liberación de recursos
                {
                    if (reader.Read() && !reader.IsDBNull(0)) // Evita errores si el valor es NULL
                        resultado = reader.GetString(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al abrir la conexión o ejecutar la consulta ODBC: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return resultado;
        }


        public static bool InsertRow(string consultaSQL)
        {
            bool resultado = false;

            try
            {
                connection.Open();

                using (OdbcCommand command = new OdbcCommand(consultaSQL, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery(); // Ejecutamos la consulta SQL
                    if (rowsAffected > 0)
                    {
                        resultado = true; // Si la inserción fue exitosa, retornamos true
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Cerramos la conexión si está abierta
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return resultado;
        }


        public static bool DeleteRow(string consultaSQL)
        {
            bool resultado = false;

            try
            {
                connection.Open();

                using (OdbcCommand command = new OdbcCommand(consultaSQL, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery(); // Ejecutamos la consulta SQL
                    if (rowsAffected > 0)
                    {
                        resultado = true; // Si la inserción fue exitosa, retornamos true
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Cerramos la conexión si está abierta
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return resultado;
        }


        public static bool UpdatetRow(string consultaSQL)
        {
            bool resultado = false;

            try
            {
                connection.Open();

                using (OdbcCommand command = new OdbcCommand(consultaSQL, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery(); // Ejecutamos la consulta SQL
                    if (rowsAffected > 0)
                    {
                        resultado = true; // Si la inserción fue exitosa, retornamos true
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Cerramos la conexión si está abierta
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return resultado;
        }




    }


}
