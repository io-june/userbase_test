using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserBase.Model;
using UserBase.Service;

namespace UserBase
{
    public partial class MainForm : Form
    {
        private readonly IEmployeeService _employees;
        private readonly System.Windows.Forms.Timer _searchDebounce =
            new() { Interval = 300 };

        private string _sortColumn = "FullName";
        private bool _sortAscending = true;
        private List<EmployeeRow> _rows = new();
        private CancellationTokenSource? _cancelOnRefresh;

        public MainForm(IEmployeeService employees)
        {
            _employees = employees;
            InitializeComponent();
            SetupGrid();
            searchBar.PlaceholderText = "Введите данные для поиска";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            _searchDebounce.Tick += async (_, _) =>
            {
                _searchDebounce.Stop();
                await RefreshAsync(GetSelectedId());
            };
        }

        private void SetupGrid()
        {
            employeesGrid.AutoGenerateColumns = false;
            employeesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            employeesGrid.MultiSelect = false;
            employeesGrid.ReadOnly = true;
            employeesGrid.AllowUserToAddRows = false;
            employeesGrid.AllowUserToDeleteRows = false;
            employeesGrid.RowHeadersVisible = false;

            employeesGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FullName",
                HeaderText = "ФИО",
                DataPropertyName = nameof(EmployeeRow.FullName),
                SortMode = DataGridViewColumnSortMode.Programmatic,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            employeesGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DateOfBirth",
                HeaderText = "Дата рождения",
                DataPropertyName = nameof(EmployeeRow.DateOfBirth),
                SortMode = DataGridViewColumnSortMode.Programmatic,
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd-MM-yyyy" }
            });
            employeesGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Gender",
                HeaderText = "Пол",
                DataPropertyName = nameof(EmployeeRow.GenderName),
                SortMode = DataGridViewColumnSortMode.Programmatic,
                Width = 100
            });

            employeesGrid.ColumnHeaderMouseClick += Grid_ColumnHeaderMouseClick;
            employeesGrid.CellDoubleClick += Grid_CellDoubleClick;
        }

        private async void MainForm_Load(object sender, EventArgs e)
            => await RefreshAsync();

        private async Task RefreshAsync(int? selectId = null)
        {
            var cancelOnRefresh = new CancellationTokenSource();
            _cancelOnRefresh?.Cancel();
            _cancelOnRefresh = cancelOnRefresh;

            try
            {

                var rows = await _employees.SearchAsync(searchBar.Text, _sortColumn, _sortAscending, cancelOnRefresh.Token);
                if (_cancelOnRefresh.IsCancellationRequested) return;
                _rows = rows;
                employeesGrid.DataSource = _rows;
                UpdateSortGlyphs();
                RestoreSelection(selectId);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestoreSelection(int? id)
        {
            if (id is not int target) return;

            var index = _rows.FindIndex(r => r.Id == target);
            if (index < 0 || index >= employeesGrid.Rows.Count) return;

            employeesGrid.CurrentCell = employeesGrid.Rows[index].Cells[0];
        }

        private async void Grid_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0) return;

            var column = employeesGrid.Columns[e.ColumnIndex].Name;
            if (_sortColumn == column) _sortAscending = !_sortAscending;
            else { _sortColumn = column; _sortAscending = true; }

            await RefreshAsync(GetSelectedId());
        }

        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in employeesGrid.Columns)
                col.HeaderCell.SortGlyphDirection = SortOrder.None;

            if (employeesGrid.Columns[_sortColumn] is { } c)
                c.HeaderCell.SortGlyphDirection =
                    _sortAscending ? SortOrder.Ascending : SortOrder.Descending;
        }

        private void searchBar_TextChanged(object sender, EventArgs e)
        {
            _searchDebounce.Stop();
            _searchDebounce.Start();
        }

        private async void addButton_Click(object sender, EventArgs e)
        {
            using var form = new EditForm(_employees, id: null);
            if (form.ShowDialog(this) == DialogResult.OK)
                await RefreshAsync(form.SavedId);
        }

        private async void editButton_Click(object? sender, EventArgs e) => await EditSelectedAsync();

        private async Task EditSelectedAsync()
        {
            if (GetSelectedId() is not int id) return;

            using var form = new EditForm(_employees, id);
            if (form.ShowDialog(this) == DialogResult.OK)
                await RefreshAsync(form.SavedId);
        }

        private async void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            await EditSelectedAsync();
        }

        private async void removeButton_Click(object sender, EventArgs e)
        {
            var id = GetSelectedId();
            if (id is null) return;

            var row = _rows.First(r => r.Id == id.Value);
            if (MessageBox.Show(this, $"Удалить сотрудника \"{row.FullName}\"?", "Подтвердите удаление",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                await _employees.DeleteAsync(id.Value);
                await RefreshAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _searchDebounce.Stop();
            _searchDebounce.Dispose();
            _cancelOnRefresh?.Cancel();
            _cancelOnRefresh?.Dispose();
            base.OnFormClosed(e);
        }

        private int? GetSelectedId() =>
            employeesGrid.CurrentRow?.DataBoundItem is EmployeeRow r ? r.Id : (int?)null;
    }
}
