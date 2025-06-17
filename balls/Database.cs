using MySqlConnector;

namespace Balls;

public class DatabaseHandler
{
    public static void ConnectToDatabase(
        string name,
        string password,
        string database,
        string server
    )
    {
        string connection_string = "";
        connection_string += $"Server={server}";
        connection_string += $"Database={database}";
        connection_string += $"User={name}";
        connection_string += $"Password={password}";

        MySqlConnection connection = new MySqlConnection(connection_string);

        try
        {
            connection.Open();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            connection.Close();
        }
    }
}
