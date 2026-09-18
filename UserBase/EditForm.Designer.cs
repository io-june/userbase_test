namespace UserBase
{
    partial class EditForm
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
            famTextBox = new TextBox();
            otchTextBox = new TextBox();
            nameTextBox = new TextBox();
            datePicker = new DateTimePicker();
            polPicker = new ComboBox();
            panel1 = new Panel();
            cancelButton = new Button();
            applyButton = new Button();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // famTextBox
            // 
            famTextBox.Location = new Point(132, 18);
            famTextBox.Name = "famTextBox";
            famTextBox.Size = new Size(200, 23);
            famTextBox.TabIndex = 0;
            // 
            // otchTextBox
            // 
            otchTextBox.Location = new Point(132, 79);
            otchTextBox.Name = "otchTextBox";
            otchTextBox.Size = new Size(200, 23);
            otchTextBox.TabIndex = 2;
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(132, 47);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(200, 23);
            nameTextBox.TabIndex = 1;
            // 
            // datePicker
            // 
            datePicker.Location = new Point(132, 105);
            datePicker.Name = "datePicker";
            datePicker.Size = new Size(200, 23);
            datePicker.TabIndex = 3;
            // 
            // polPicker
            // 
            polPicker.FormattingEnabled = true;
            polPicker.Location = new Point(132, 134);
            polPicker.Name = "polPicker";
            polPicker.Size = new Size(200, 23);
            polPicker.TabIndex = 4;
            // 
            // panel1
            // 
            panel1.Controls.Add(cancelButton);
            panel1.Controls.Add(applyButton);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(famTextBox);
            panel1.Controls.Add(nameTextBox);
            panel1.Controls.Add(polPicker);
            panel1.Controls.Add(otchTextBox);
            panel1.Controls.Add(datePicker);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(352, 210);
            panel1.TabIndex = 7;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(175, 170);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 6;
            cancelButton.Text = "Отмена";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // applyButton
            // 
            applyButton.Location = new Point(77, 170);
            applyButton.Name = "applyButton";
            applyButton.Size = new Size(80, 23);
            applyButton.TabIndex = 5;
            applyButton.Text = "Применить";
            applyButton.UseVisualStyleBackColor = true;
            applyButton.Click += applyButton_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(27, 137);
            label5.Name = "label5";
            label5.Size = new Size(30, 15);
            label5.TabIndex = 12;
            label5.Text = "Пол";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(27, 111);
            label4.Name = "label4";
            label4.Size = new Size(90, 15);
            label4.TabIndex = 11;
            label4.Text = "Дата рождения";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(27, 82);
            label3.Name = "label3";
            label3.Size = new Size(58, 15);
            label3.TabIndex = 10;
            label3.Text = "Отчество";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 50);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 9;
            label2.Text = "Имя";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 21);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 8;
            label1.Text = "Фамилия";
            // 
            // EditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(377, 233);
            Controls.Add(panel1);
            Name = "EditForm";
            Text = "Добавление/изменение сотрудника";
            Load += EditForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox famTextBox;
        private TextBox otchTextBox;
        private TextBox nameTextBox;
        private DateTimePicker datePicker;
        private ComboBox polPicker;
        private Panel panel1;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button cancelButton;
        private Button applyButton;
    }
}