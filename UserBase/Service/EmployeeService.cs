using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserBase.Database.Context;
using UserBase.Model;

namespace UserBase.Service
{
    public interface IEmployeeService
    {
        Task<List<EmployeeRow>> SearchAsync(string search, string sortColumn, bool ascending, CancellationToken ct = default);
        Task<Employee?> GetAsync(int id, CancellationToken ct = default);
        Task<List<Gender>> GetGendersAsync(CancellationToken ct = default);
        Task<int> AddAsync(Employee employee, CancellationToken ct = default);
        Task UpdateAsync(Employee employee, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public EmployeeService(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        private async Task<T> WithDbAsync<T>(Func<AppDbContext, Task<T>> work)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return await work(db);
        }

        private async Task WithDbAsync(Func<AppDbContext, Task> work)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await work(db);
        }

        public Task<List<EmployeeRow>> SearchAsync(
            string search, string sortColumn, bool ascending, CancellationToken ct = default)
            => WithDbAsync(async db =>
            {
                IQueryable<Employee> q = db.Employees.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var s = search.Trim();
                    q = q.Where(e =>
                        (e.Surname + " " + e.FirstName + " " + (e.Patronymic ?? "")).Contains(s));
                }

                q = (sortColumn, ascending) switch
                {
                    ("FullName", true) => q.OrderBy(e => e.Surname).ThenBy(e => e.FirstName).ThenBy(e => e.Patronymic).ThenBy(e => e.Id),
                    ("FullName", false) => q.OrderByDescending(e => e.Surname).ThenByDescending(e => e.FirstName).ThenByDescending(e => e.Patronymic).ThenBy(e => e.Id),
                    ("DateOfBirth", true) => q.OrderBy(e => e.DateOfBirth).ThenBy(e => e.Id),
                    ("DateOfBirth", false) => q.OrderByDescending(e => e.DateOfBirth).ThenBy(e => e.Id),
                    ("Gender", true) => q.OrderBy(e => e.Gender!.Name).ThenBy(e => e.Id),
                    ("Gender", false) => q.OrderByDescending(e => e.Gender!.Name).ThenBy(e => e.Id),
                    _ => q.OrderBy(e => e.Id)
                };

                return await q.Select(e => new EmployeeRow
                {
                    Id = e.Id,
                    Surname = e.Surname,
                    FirstName = e.FirstName,
                    Patronymic = e.Patronymic,
                    DateOfBirth = e.DateOfBirth,
                    GenderName = e.Gender!.Name
                }).ToListAsync(ct);
            });

        public Task<Employee?> GetAsync(int id, CancellationToken ct = default)
            => WithDbAsync(db => db.Employees.AsNoTracking()
                                          .FirstOrDefaultAsync(e => e.Id == id, ct));

        public Task<List<Gender>> GetGendersAsync(CancellationToken ct = default)
            => WithDbAsync(db => db.Genders.AsNoTracking()
                                         .OrderBy(g => g.Name)
                                         .ToListAsync(ct));

        public Task<int> AddAsync(Employee employee, CancellationToken ct = default)
            => WithDbAsync(async db =>
            {
                db.Employees.Add(employee);
                await db.SaveChangesAsync(ct);
                return employee.Id;
            });

        public Task UpdateAsync(Employee employee, CancellationToken ct = default)
            => WithDbAsync(async db =>
            {
                var existing = await db.Employees.FirstOrDefaultAsync(e => e.Id == employee.Id, ct)
                    ?? throw new InvalidOperationException($"{employee.Id} не найден.");

                existing.Surname = employee.Surname;
                existing.FirstName = employee.FirstName;
                existing.Patronymic = employee.Patronymic;
                existing.DateOfBirth = employee.DateOfBirth;
                existing.GenderId = employee.GenderId;
                await db.SaveChangesAsync(ct);
            });

        public Task DeleteAsync(int id, CancellationToken ct = default)
            => WithDbAsync(async db =>
            {
                var emp = await db.Employees.FirstOrDefaultAsync(e => e.Id == id, ct);
                if (emp is null) return;
                db.Employees.Remove(emp);
                await db.SaveChangesAsync(ct);
            });
    }
}
