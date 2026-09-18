namespace UserBase
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            searchBar = new TextBox();
            addButton = new Button();
            editButton = new Button();
            removeButton = new Button();
            employeesGrid = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)employeesGrid).BeginInit();
            SuspendLayout();
            // 
            // searchBar
            // 
            searchBar.Location = new Point(255, 415);
            searchBar.Name = "searchBar";
            searchBar.Size = new Size(533, 23);
            searchBar.TabIndex = 0;
            searchBar.TextChanged += searchBar_TextChanged;
            // 
            // addButton
            // 
            addButton.Location = new Point(12, 414);
            addButton.Name = "addButton";
            addButton.Size = new Size(75, 23);
            addButton.TabIndex = 1;
            addButton.Text = "Добавить";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += addButton_Click;
            // 
            // editButton
            // 
            editButton.Location = new Point(93, 415);
            editButton.Name = "editButton";
            editButton.Size = new Size(75, 23);
            editButton.TabIndex = 2;
            editButton.Text = "Изменить";
            editButton.UseVisualStyleBackColor = true;
            editButton.Click += editButton_Click;
            // 
            // removeButton
            // 
            removeButton.Location = new Point(174, 415);
            removeButton.Name = "removeButton";
            removeButton.Size = new Size(75, 23);
            removeButton.TabIndex = 3;
            removeButton.Text = "Удалить";
            removeButton.UseVisualStyleBackColor = true;
            removeButton.Click += removeButton_Click;
            // 
            // employeesGrid
            // 
            employeesGrid.BackgroundColor = SystemColors.Window;
            employeesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            employeesGrid.Location = new Point(12, 12);
            employeesGrid.Name = "employeesGrid";
            employeesGrid.Size = new Size(776, 396);
            employeesGrid.TabIndex = 4;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(employeesGrid);
            Controls.Add(removeButton);
            Controls.Add(editButton);
            Controls.Add(addButton);
            Controls.Add(searchBar);
            Name = "MainForm";
            Text = "Сотрудники";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)employeesGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox searchBar;
        private Button addButton;
        private Button editButton;
        private Button removeButton;
        private DataGridView employeesGrid;
    }
}
