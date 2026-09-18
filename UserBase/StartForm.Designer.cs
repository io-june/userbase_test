namespace UserBase
{
    partial class StartForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            radioLocal = new RadioButton();
            radioRemote = new RadioButton();
            newButton = new Button();
            connectButton = new Button();
            openButton = new Button();
            textBoxRemote = new TextBox();
            progressBar = new ProgressBar();
            progressBarLabel = new Label();
            textBoxLocal = new TextBox();
            SuspendLayout();
            // 
            // radioLocal
            // 
            radioLocal.AutoSize = true;
            radioLocal.Checked = true;
            radioLocal.Location = new Point(18, 12);
            radioLocal.Name = "radioLocal";
            radioLocal.Size = new Size(322, 19);
            radioLocal.TabIndex = 0;
            radioLocal.TabStop = true;
            radioLocal.Text = "Файл локальной базы данных Microsoft Access/SQLite";
            radioLocal.UseVisualStyleBackColor = true;
            radioLocal.CheckedChanged += radioLocal_CheckedChanged;
            // 
            // radioRemote
            // 
            radioRemote.AutoSize = true;
            radioRemote.Location = new Point(18, 66);
            radioRemote.Name = "radioRemote";
            radioRemote.Size = new Size(364, 19);
            radioRemote.TabIndex = 1;
            radioRemote.Text = "Удаленный сервер базы данных Microsoft SQL Server/Postgres";
            radioRemote.UseVisualStyleBackColor = true;
            radioRemote.CheckedChanged += radioRemote_CheckedChanged;
            // 
            // newButton
            // 
            newButton.Location = new Point(230, 37);
            newButton.Name = "newButton";
            newButton.Size = new Size(75, 23);
            newButton.TabIndex = 2;
            newButton.Text = "Новый...";
            newButton.UseVisualStyleBackColor = true;
            newButton.Click += newButton_Click;
            // 
            // connectButton
            // 
            connectButton.Location = new Point(230, 90);
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(153, 23);
            connectButton.TabIndex = 3;
            connectButton.Text = "Присоединиться";
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += connectButton_Click;
            // 
            // openButton
            // 
            openButton.Location = new Point(308, 37);
            openButton.Name = "openButton";
            openButton.Size = new Size(75, 23);
            openButton.TabIndex = 4;
            openButton.Text = "Открыть...";
            openButton.UseVisualStyleBackColor = true;
            openButton.Click += openButton_Click;
            // 
            // textBoxRemote
            // 
            textBoxRemote.Location = new Point(18, 91);
            textBoxRemote.Name = "textBoxRemote";
            textBoxRemote.Size = new Size(206, 23);
            textBoxRemote.TabIndex = 6;
            textBoxRemote.Text = "yunusoff.ru";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(-1, 132);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(404, 23);
            progressBar.TabIndex = 7;
            // 
            // progressBarLabel
            // 
            progressBarLabel.AutoSize = true;
            progressBarLabel.BackColor = Color.Transparent;
            progressBarLabel.Location = new Point(3, 137);
            progressBarLabel.Name = "progressBarLabel";
            progressBarLabel.Size = new Size(152, 15);
            progressBarLabel.TabIndex = 8;
            progressBarLabel.Text = "В ожидании подключения";
            // 
            // textBoxLocal
            // 
            textBoxLocal.Location = new Point(18, 38);
            textBoxLocal.Name = "textBoxLocal";
            textBoxLocal.Size = new Size(206, 23);
            textBoxLocal.TabIndex = 9;
            // 
            // StartForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(401, 155);
            Controls.Add(textBoxLocal);
            Controls.Add(progressBarLabel);
            Controls.Add(progressBar);
            Controls.Add(textBoxRemote);
            Controls.Add(openButton);
            Controls.Add(connectButton);
            Controls.Add(newButton);
            Controls.Add(radioRemote);
            Controls.Add(radioLocal);
            Name = "StartForm";
            Text = "Подключение к базе данных...";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RadioButton radioLocal;
        private RadioButton radioRemote;
        private Button newButton;
        private Button connectButton;
        private Button openButton;
        private TextBox textBoxRemote;
        private ProgressBar progressBar;
        private Label progressBarLabel;
        private TextBox textBoxLocal;
    }
}