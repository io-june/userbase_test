namespace UserBase.Model
{
    public class Gender
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Surname { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string Patronymic { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public int GenderId { get; set; }
        public Gender? Gender { get; set; }
    }

    public class EmployeeRow
    {
        public int Id { get; set; }
        public string Surname { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string Patronymic { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string GenderName { get; set; } = "";

        public string FullName => string.Join(" ",
            new[] { Surname, FirstName, Patronymic }
                .Where(s => !string.IsNullOrWhiteSpace(s)));
    }
}
