namespace Lab1.WinForms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            txtAccountNumber = new TextBox();
            txtOwnerName = new TextBox();
            btnCreate = new Button();
            txtNewOwner = new TextBox();
            btnEditOwner = new Button();
            btnDelete = new Button();
            chkShowDeleted = new CheckBox();
            btnRestore = new Button();
            btnFreezeToggle = new Button();
            txtTargetAccount = new TextBox();
            txtAmount = new TextBox();
            btnTransfer = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Top;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(700, 225);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
           
            txtAccountNumber.Location = new Point(18, 240);
            txtAccountNumber.Margin = new Padding(3, 2, 3, 2);
            txtAccountNumber.Name = "txtAccountNumber";
            txtAccountNumber.PlaceholderText = "Номер счёта";
            txtAccountNumber.Size = new Size(176, 23);
            txtAccountNumber.TabIndex = 1;
            
            txtOwnerName.Location = new Point(210, 240);
            txtOwnerName.Margin = new Padding(3, 2, 3, 2);
            txtOwnerName.Name = "txtOwnerName";
            txtOwnerName.PlaceholderText = "ФИО владельца";
            txtOwnerName.Size = new Size(176, 23);
            txtOwnerName.TabIndex = 2;
            
            btnCreate.Location = new Point(402, 240);
            btnCreate.Margin = new Padding(3, 2, 3, 2);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(88, 23);
            btnCreate.TabIndex = 3;
            btnCreate.Text = "Создать";
            btnCreate.UseVisualStyleBackColor = true;
            
            txtNewOwner.Location = new Point(18, 270);
            txtNewOwner.Margin = new Padding(3, 2, 3, 2);
            txtNewOwner.Name = "txtNewOwner";
            txtNewOwner.PlaceholderText = "Новый владелец";
            txtNewOwner.Size = new Size(176, 23);
            txtNewOwner.TabIndex = 4;
             
            btnEditOwner.Location = new Point(210, 270);
            btnEditOwner.Margin = new Padding(3, 2, 3, 2);
            btnEditOwner.Name = "btnEditOwner";
            btnEditOwner.Size = new Size(131, 26);
            btnEditOwner.TabIndex = 5;
            btnEditOwner.Text = "Изменить владельца";
            btnEditOwner.UseVisualStyleBackColor = true;
           
            btnDelete.Location = new Point(402, 272);
            btnDelete.Margin = new Padding(3, 2, 3, 2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(88, 22);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "Удалить";
            btnDelete.UseVisualStyleBackColor = true;
           
            chkShowDeleted.Location = new Point(18, 300);
            chkShowDeleted.Margin = new Padding(3, 2, 3, 2);
            chkShowDeleted.Name = "chkShowDeleted";
            chkShowDeleted.Size = new Size(158, 20);
            chkShowDeleted.TabIndex = 7;
            chkShowDeleted.Text = "Показать удалённые";
            chkShowDeleted.UseVisualStyleBackColor = true;
           
            btnRestore.Location = new Point(210, 300);
            btnRestore.Margin = new Padding(3, 2, 3, 2);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(106, 22);
            btnRestore.TabIndex = 8;
            btnRestore.Text = "Восстановить";
            btnRestore.UseVisualStyleBackColor = true;
            
            btnFreezeToggle.Location = new Point(324, 300);
            btnFreezeToggle.Margin = new Padding(3, 2, 3, 2);
            btnFreezeToggle.Name = "btnFreezeToggle";
            btnFreezeToggle.Size = new Size(166, 22);
            btnFreezeToggle.TabIndex = 9;
            btnFreezeToggle.Text = "Заморозить/Разморозить";
            btnFreezeToggle.UseVisualStyleBackColor = true;
            btnFreezeToggle.Click += btnFreezeToggle_Click_1;
            
            txtTargetAccount.Location = new Point(18, 330);
            txtTargetAccount.Margin = new Padding(3, 2, 3, 2);
            txtTargetAccount.Name = "txtTargetAccount";
            txtTargetAccount.PlaceholderText = "Номер счёта получателя";
            txtTargetAccount.Size = new Size(176, 23);
            txtTargetAccount.TabIndex = 10;
            
            txtAmount.Location = new Point(210, 330);
            txtAmount.Margin = new Padding(3, 2, 3, 2);
            txtAmount.Name = "txtAmount";
            txtAmount.PlaceholderText = "Сумма";
            txtAmount.Size = new Size(106, 23);
            txtAmount.TabIndex = 11;
            
            btnTransfer.Location = new Point(402, 331);
            btnTransfer.Margin = new Padding(3, 2, 3, 2);
            btnTransfer.Name = "btnTransfer";
            btnTransfer.Size = new Size(88, 22);
            btnTransfer.TabIndex = 12;
            btnTransfer.Text = "Перевести";
            btnTransfer.UseVisualStyleBackColor = true;
           
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 390);
            Controls.Add(btnCreate);
            Controls.Add(txtOwnerName);
            Controls.Add(txtAccountNumber);
            Controls.Add(dataGridView1);
            Controls.Add(txtNewOwner);
            Controls.Add(btnEditOwner);
            Controls.Add(btnDelete);
            Controls.Add(chkShowDeleted);
            Controls.Add(btnRestore);
            Controls.Add(btnFreezeToggle);
            Controls.Add(txtTargetAccount);
            Controls.Add(txtAmount);
            Controls.Add(btnTransfer);
            Margin = new Padding(3, 2, 3, 2);
            Name = "MainForm";
            Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }


        #endregion

        private DataGridView dataGridView1;
        private TextBox txtAccountNumber;
        private TextBox txtOwnerName;
        private Button btnCreate;
        private TextBox txtNewOwner;
        private Button btnEditOwner;
        private Button btnDelete;
        private CheckBox chkShowDeleted;
        private Button btnRestore;
        private Button btnFreezeToggle;
        private TextBox txtTargetAccount;
        private TextBox txtAmount;
        private Button btnTransfer;
    }
}