using System.Data.OleDb;
using System.Runtime.InteropServices;

namespace UserBase.Database.LocalFileCreator
{
    public class AccessDatabaseCreator : IDatabaseCreator
    {
        public DbProvider Provider => DbProvider.Access;

        public void Create(string path)
        {
            if (File.Exists(path)) File.Delete(path);

            var adoxType = Type.GetTypeFromProgID("ADOX.Catalog")
                ?? throw new InvalidOperationException(
                    "ADOX недоступен. Установите Access.");
            var connStr = new OleDbConnectionStringBuilder
            {
                Provider = "Microsoft.ACE.OLEDB.12.0",
                DataSource = path
            }.ConnectionString;
            object catalog = Activator.CreateInstance(adoxType)!;
            try
            {
                ((dynamic)catalog).Create(connStr);
                using (var conn = new OleDbConnection(connStr))
                {
                    conn.Open();

                    string createGender = @"
                    CREATE TABLE Genders (
                    Id COUNTER PRIMARY KEY,
                    Name TEXT(50) NOT NULL
                    )";
                    using (var cmd = new OleDbCommand(createGender, conn))
                        cmd.ExecuteNonQuery();

                    string[] genders = { "Мужской", "Женский" };
                    foreach (var gender in genders)
                    {
                        string insert = "INSERT INTO Genders (Name) VALUES (?)";
                        using (var cmd = new OleDbCommand(insert, conn))
                        {
                            cmd.Parameters.AddWithValue("?", gender);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    string createEmployees = @"
                    CREATE TABLE Employees (
                    Id COUNTER PRIMARY KEY,
                    Surname TEXT(100) NOT NULL,
                    FirstName TEXT(100) NOT NULL,
                    Patronymic TEXT(100),
                    DateOfBirth DATETIME NOT NULL,
                    GenderId LONG NOT NULL,
                    CONSTRAINT FK_Employees_Gender 
                    FOREIGN KEY (GenderId) REFERENCES Genders(Id)
                    )";
                    using (var cmd = new OleDbCommand(createEmployees, conn))
                        cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                Marshal.FinalReleaseComObject(catalog);
            }
        }
    }
}
