using System.Data.SqlClient;
using ProbneKolokwium.DTO;

namespace ProbneKolokwium;

public class DbService : IDbService
{
    private string ConnString;

    public DbService(IConfiguration configuration)
    {
        ConnString = configuration.GetConnectionString("MSSQL");
    }

    public async Task<IEnumerable<Prescription>> GetPrescriptionsAsync()
    {
        var sql = "SELECT * FROM Prescription";
        using var connection = new SqlConnection(ConnString);
        using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        var prescriptions = new List<Prescription>();

        while (await reader.ReadAsync()) 
        {
            prescriptions.Add(new Prescription
            {
                IdPrescription = int.Parse(reader["IdPrescription"].ToString()),
                Date = DateTime.Parse(reader["Date"].ToString()),
                DueDate = DateTime.Parse(reader["DueDate"].ToString()),
                IdPatient = int.Parse(reader["IdPatient"].ToString()),
                IdDoctor = int.Parse(reader["IdDoctor"].ToString())
            });
        }

        return prescriptions;
    }

    public async Task<IEnumerable<Prescription>> GetPrescriptionsByPatientAsync(string lastname)
    {
        var sql = "SELECT [Prescription].*" +
        "FROM [Prescription]" +
        "JOIN [Patient] ON [Prescription].[IdPatient] = [Patient].[IdPatient]" + 
        "WHERE LastName = @LASTNAME";
        await using var connection = new SqlConnection(ConnString);
        await connection.OpenAsync();
        
        await using var command = new SqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("LASTNAME", lastname);
        
        await using var reader = await command.ExecuteReaderAsync();

        var prescriptions = new List<Prescription>();

        while (await reader.ReadAsync()) 
        {
            prescriptions.Add(new Prescription
            {
                IdPrescription = int.Parse(reader["IdPrescription"].ToString()),
                Date = DateTime.Parse(reader["Date"].ToString()),
                DueDate = DateTime.Parse(reader["DueDate"].ToString()),
                IdPatient = int.Parse(reader["IdPatient"].ToString()),
                IdDoctor = int.Parse(reader["IdDoctor"].ToString())
            });
        }

        return prescriptions;
    }

    public async Task<int> AddMedicamentsToPrescriptionsAsync(int prescriptionId,
        IEnumerable<PrescriptionMedicament> prescriptions)
    {
        var sql = "INSERT INTO [Prescription_Medicament]" +
                  "VALUES (@IdMedicament, @IdPrescription, @Dose, @Details)";

        await using var connection = new SqlConnection(ConnString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Transaction = (SqlTransaction) await connection.BeginTransactionAsync();

        var affectedRows = 0;
        
        try
        {
            foreach (var prescriptionMedicament in prescriptions)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("IdMedicament", prescriptionMedicament.IdMedicament);
                command.Parameters.AddWithValue("IdPrescription", prescriptionId);
                command.Parameters.AddWithValue("Dose", prescriptionMedicament.Dose);
                command.Parameters.AddWithValue("Details", prescriptionMedicament.Details);
                affectedRows += await command.ExecuteNonQueryAsync();
            }

            await command.Transaction.CommitAsync();
        }
        catch (SqlException e)
        {
            await command.Transaction.RollbackAsync();
            Console.WriteLine(e);
            // throw;
        }
        catch (Exception e)
        {
            await command.Transaction.RollbackAsync();
            Console.WriteLine(e);
            // throw;
        }

        return affectedRows;
    }
}