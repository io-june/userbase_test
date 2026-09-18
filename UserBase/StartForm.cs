using System;
using System.Windows.Forms;
using UserBase.Database;
using UserBase.Database.LocalFileCreator;
using UserBase.Database.RemoteConnectionHandler;

namespace UserBase
{
    public partial class StartForm : Form
    {
        private readonly IDatabaseCreatorResolver _creators;
        private readonly IConnectionStringFactory _connStrings;
        private readonly IConnectionTester _tester;
        private bool _inProgress = false;

        public DbConnectionInfo? Result { get; private set; }
        public StartForm(
            IDatabaseCreatorResolver creators,
            IConnectionStringFactory connStrings,
            IConnectionTester tester)
        {
            _creators = creators;
            _connStrings = connStrings;
            _tester = tester;
            InitializeComponent();
            UpdateEnabledState();
        }

        private void radioLocal_CheckedChanged(object sender, EventArgs e) => UpdateEnabledState();
        private void radioRemote_CheckedChanged(object sender, EventArgs e) => UpdateEnabledState();
        private void UpdateEnabledState()
        {
            if (_inProgress) return;

            bool local = radioLocal.Checked;

            textBoxLocal.Enabled = local;
            openButton.Enabled = local;
            newButton.Enabled = local;

            textBoxRemote.Enabled = !local;
            connectButton.Enabled = !local;
        }

        private DbConnectionInfo BuildInfo()
        {
            if (radioLocal.Checked)
            {
                if (string.IsNullOrWhiteSpace(textBoxLocal.Text))
                    throw new InvalidOperationException(
                        "Выберите или создайте файл.");

                var provider = DatabaseFileTypes.FromExtension(textBoxLocal.Text)
                    ?? throw new InvalidOperationException("Неподдерживаемое расширение.");

                return new DbConnectionInfo
                {
                    Provider = provider,
                    ConnectionString = _connStrings.ForLocalFile(provider, textBoxLocal.Text)
                };
            }

            if (string.IsNullOrWhiteSpace(textBoxRemote.Text))
                throw new InvalidOperationException("Введите строку подключения к SQL Server.");

            return new DbConnectionInfo
            {
                Provider = DbProvider.SqlServer,
                ConnectionString = textBoxRemote.Text.Trim()
            };
        }

        private async Task Connect()
        {
            if (_inProgress) return;

            DbConnectionInfo info;
            try { info = BuildInfo(); }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка соединения с БД!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            setProgress(true);
            progressBar.Value = progressBar.Minimum;
            progressBarLabel.Text = "Соединение...";

            var progress = new Progress<ConnectionProgress>(p =>
            {
                progressBar.Value = Math.Clamp(p.Percent, progressBar.Minimum, progressBar.Maximum);
                progressBarLabel.Text = p.Message;
            });

            try
            {
                await _tester.TestAsync(info, progress);
                setProgress(false);
                Result = info;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                setProgress(false);
                progressBar.Value = progressBar.Minimum;
                progressBarLabel.Text = "Соединение прервано.";
                MessageBox.Show(this, $"Соединение прервано:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void newButton_Click(object sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog
            {
                Title = "Создать файл базы данных",
                Filter = DatabaseFileTypes.BuildFilter(),
                FilterIndex = 1,
                AddExtension = true,
                OverwritePrompt = true
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            var provider = DatabaseFileTypes.FromExtension(dialog.FileName)
                        ?? DatabaseFileTypes.FromFilterIndex(dialog.FilterIndex);
            if (provider is null)
            {
                MessageBox.Show(this, "Не удалось определить тип файла базы данных.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                _creators.Resolve(provider.Value).Create(dialog.FileName);
                textBoxLocal.Text = dialog.FileName;
                await Connect();
            }
            catch (Exception exc)
            {
                MessageBox.Show(this,
                    $"Ошибка при создании файла:\n{exc.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void openButton_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Открыть файл базы данных",
                Filter = DatabaseFileTypes.BuildFilter()
            };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            textBoxLocal.Text = dialog.FileName;
            await Connect();
        }

        private async void connectButton_Click(object sender, EventArgs e) => await Connect();

        private void setProgress(bool isProgress)
        {
            _inProgress = isProgress;
            radioLocal.Enabled = !isProgress;
            radioRemote.Enabled = !isProgress;
            if (isProgress)
            {
                textBoxLocal.Enabled = false;
                textBoxRemote.Enabled = false;
                newButton.Enabled = false;
                openButton.Enabled = false;
                connectButton.Enabled = false;
            }
            else
            {
                UpdateEnabledState();
            }
        }
    }
}
