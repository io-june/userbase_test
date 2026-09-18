using System;
using System.Windows.Forms;
using UserBase.Model;
using UserBase.Service;

namespace UserBase
{
    public partial class EditForm : Form
    {
        private readonly IEmployeeService _employees;
        private readonly int? _id;

        public int? SavedId { get; private set; }

        public EditForm(IEmployeeService employees, int? id)
        {
            _employees = employees;
            _id = id;
            InitializeComponent();
            Text = _id is null ? "Новый сотрудник" : "Редактирование сотрудника";
            datePicker.MaxDate = DateTime.Today;
        }

        private async void EditForm_Load(object sender, EventArgs e)
        {
            var genders = await _employees.GetGendersAsync();
            if (IsDisposed) return;

            polPicker.DataSource = genders;
            polPicker.DisplayMember = nameof(Gender.Name);
            polPicker.ValueMember = nameof(Gender.Id);

            if (_id is int id)
            {
                var emp = await _employees.GetAsync(id);
                if (IsDisposed) return;
                if (emp is null) { DialogResult = DialogResult.Cancel; Close(); return; }

                famTextBox.Text = emp.Surname;
                nameTextBox.Text = emp.FirstName;
                otchTextBox.Text = emp.Patronymic;
                datePicker.Value = emp.DateOfBirth;
                polPicker.SelectedValue = emp.GenderId;
            }
            else
            {
                datePicker.Value = DateTime.Today;
                polPicker.SelectedIndex = -1;
            }
        }

        private async void applyButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(famTextBox.Text) ||
                string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show(this, "Фамилия и имя — обязательные поля.");
                return;
            }

            if (polPicker.SelectedValue is not int genderId)
            {
                MessageBox.Show(this, "Выберите пол.");
                return;
            }

            var emp = new Employee
            {
                Id = _id ?? 0,
                Surname = famTextBox.Text.Trim(),
                FirstName = nameTextBox.Text.Trim(),
                Patronymic = otchTextBox.Text.Trim(),
                DateOfBirth = datePicker.Value.Date,
                GenderId = genderId
            };

            try
            {
                if (_id is null)
                {
                    SavedId = await _employees.AddAsync(emp);
                }
                else
                {
                    await _employees.UpdateAsync(emp);
                    SavedId = emp.Id;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
