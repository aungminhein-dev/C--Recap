using System;
using System.Data;
using System.Data.SqlClient;

// Top-level statements
string dataSource = "."; // Update with your actual SQL Server instance
string initialCatalog = "ToDoList"; // Replace with your actual database name
string userId = "sa";
string password = "sa@123";

DatabaseReader data = new DatabaseReader(dataSource, initialCatalog, userId, password);
data.ReadData();
//data.WriteData("New Task", 1, "2024-07-10");

Console.ReadKey();

// Class definition follows top-level statements
public class DatabaseReader
{
    private string connectionString;

    public DatabaseReader(string dataSource, string initialCatalog, string userId, string password)
    {
        SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = dataSource,
            InitialCatalog = initialCatalog,
            UserID = userId,
            Password = password
        };

        connectionString = sqlConnectionStringBuilder.ConnectionString;
    }

    public void ReadData()
    {
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            try
            {
                sqlConnection.Open();
                Console.WriteLine("Connection Opened!");

                string query = @"SELECT [ListId]
                                    ,[ListName]
                                    ,[ListStatus]
                                    ,[ListSchedule]
                                FROM [dbo].[Lists]";

                SqlCommand command = new SqlCommand(query, sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Console.WriteLine("Task => " + dr["ListName"]);
                        if ((int)dr["ListStatus"] == 0)
                        {
                            Console.WriteLine("Status => Finished!");
                        }
                        else
                        {
                            Console.WriteLine("Status => Have not finished!");
                        }
                        Console.WriteLine("List Schedule => " + dr["ListSchedule"]);
                    }
                }
                else
                {
                    Console.WriteLine("No data found!");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                Console.WriteLine("Connection Closed");
            }
        }
    }

    public void WriteData(string name, int status, string schedule)
    {
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            try
            {
                sqlConnection.Open();
                Console.WriteLine("Connection Opened!");

                string query = @"INSERT INTO [dbo].[Lists] ([ListName], [ListStatus], [ListSchedule]) VALUES (@name, @status, @schedule)";

                SqlCommand command = new SqlCommand(query, sqlConnection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@schedule", schedule);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Data inserted successfully!");
                }
                else
                {
                    Console.WriteLine("Data insertion failed!");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                Console.WriteLine("Connection Closed");
            }
        }
    }
}
