using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Learning___Program
{
    public static class DatabaseHelper // Класс для работы с базой данных
    {
        private static readonly string connectionString = "Server=ASUSVIVOBOOK\\MSSQLSERVER3;Database=Learning;Integrated Security=True;";

        public static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(connectionString);

            // Добавляем обработку события InfoMessage для логирования SQL-сообщений
            connection.InfoMessage += (sender, e) =>
            {
                foreach (SqlError error in e.Errors)
                {
                    Console.WriteLine($"SQL Info: {error.Message}");
                }
            };

            return connection;
        }

        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = commandType;

                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    try
                    {
                        return command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        LogSqlError(ex, query);
                        throw; // Перебрасываем исключение для обработки на уровне выше
                    }
                }
            }
        }

        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            DataTable dataTable = new DataTable();

            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = commandType;

                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                    catch (SqlException ex)
                    {
                        LogSqlError(ex, query);
                        throw;
                    }
                }
            }

            return dataTable;
        }

        public static object ExecuteScalar(string query, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = commandType;

                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    try
                    {
                        return command.ExecuteScalar();
                    }
                    catch (SqlException ex)
                    {
                        LogSqlError(ex, query);
                        throw;
                    }
                }
            }
        }

        // Новый метод для выполнения хранимых процедур
        public static DataTable ExecuteStoredProcedure(string procedureName, SqlParameter[] parameters = null)
        {
            return ExecuteQuery(procedureName, parameters, CommandType.StoredProcedure);
        }

        // Метод для логирования ошибок SQL
        private static void LogSqlError(SqlException ex, string query)
        {
            StringBuilder errorMessage = new StringBuilder();
            errorMessage.AppendLine($"SQL Error executing query: {query}");

            foreach (SqlError error in ex.Errors)
            {
                errorMessage.AppendLine($"Error #{error.Number}: {error.Message}");
                errorMessage.AppendLine($"LineNumber: {error.LineNumber}");
                errorMessage.AppendLine($"Source: {error.Source}");
                errorMessage.AppendLine($"Procedure: {error.Procedure}");
            }

            // Можно заменить на запись в лог-файл или систему мониторинга
            Console.WriteLine(errorMessage.ToString());

            // Также показываем пользователю упрощенное сообщение
            MessageBox.Show($"Ошибка базы данных: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Метод для проверки соединения с базой данных
        public static bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        // Добавляем метод для выполнения хранимых процедур с выходным параметром
        public static int ExecuteStoredProcedureWithOutput(string procedureName,
            SqlParameter[] parameters, string outputParamName)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    command.ExecuteNonQuery();

                    return Convert.ToInt32(command.Parameters[outputParamName].Value);
                }
            }

        }
    }
}
