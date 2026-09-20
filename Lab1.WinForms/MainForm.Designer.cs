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
            
            // dataGridView1
            
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Top;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(800, 300);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            
            // txtAccountNumber
            
            txtAccountNumber.Location = new Point(20, 320);
            txtAccountNumber.Name = "txtAccountNumber";
            txtAccountNumber.PlaceholderText = "Номер счёта";
            txtAccountNumber.Size = new Size(200, 27);
            txtAccountNumber.TabIndex = 1;
             
            // txtOwnerName
            
            txtOwnerName.Location = new Point(240, 320);
            txtOwnerName.Name = "txtOwnerName";
            txtOwnerName.PlaceholderText = "ФИО владельца";
            txtOwnerName.Size = new Size(200, 27);
            txtOwnerName.TabIndex = 2;
            
            // btnCreate
             
            btnCreate.Location = new Point(460, 320);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(100, 27);
            btnCreate.TabIndex = 3;
            btnCreate.Text = "Создать";
            btnCreate.UseVisualStyleBackColor = true;
             
            // txtNewOwner
             
            txtNewOwner.Location = new Point(20, 360);
            txtNewOwner.Name = "txtNewOwner";
            txtNewOwner.PlaceholderText = "Новый владелец";
            txtNewOwner.Size = new Size(200, 27);
            txtNewOwner.TabIndex = 4;
             
            // btnEditOwner
             
            btnEditOwner.Location = new Point(240, 360);
            btnEditOwner.Name = "btnEditOwner";
            btnEditOwner.Size = new Size(150, 27);
            btnEditOwner.TabIndex = 5;
            btnEditOwner.Text = "Изменить владельца";
            btnEditOwner.UseVisualStyleBackColor = true;
             
            // btnDelete
             
            btnDelete.Location = new Point(460, 362);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 25);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "Удалить";
            btnDelete.UseVisualStyleBackColor = true;
             
            // chkShowDeleted
             
            chkShowDeleted.Location = new Point(20, 400);
            chkShowDeleted.Name = "chkShowDeleted";
            chkShowDeleted.Size = new Size(180, 24);
            chkShowDeleted.TabIndex = 7;
            chkShowDeleted.Text = "Показать удалённые";
            chkShowDeleted.UseVisualStyleBackColor = true;
            
            // btnRestore
            
            btnRestore.Location = new Point(210, 400);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(120, 24);
            btnRestore.TabIndex = 8;
            btnRestore.Text = "Восстановить";
            btnRestore.UseVisualStyleBackColor = true;
             
            // btnFreezeToggle
             
            btnFreezeToggle.Location = new Point(360, 400);
            btnFreezeToggle.Name = "btnFreezeToggle";
            btnFreezeToggle.Size = new Size(180, 24);
            btnFreezeToggle.TabIndex = 9;
            btnFreezeToggle.Text = "Заморозить/Разморозить";
            btnFreezeToggle.UseVisualStyleBackColor = true;
            btnFreezeToggle.Click += btnFreezeToggle_Click_1;
             
            // txtTargetAccount
             
            txtTargetAccount.Location = new Point(20, 440);
            txtTargetAccount.Name = "txtTargetAccount";
            txtTargetAccount.PlaceholderText = "Номер счёта получателя";
            txtTargetAccount.Size = new Size(200, 27);
            txtTargetAccount.TabIndex = 10;
             
            // txtAmount
             
            txtAmount.Location = new Point(240, 440);
            txtAmount.Name = "txtAmount";
            txtAmount.PlaceholderText = "Сумма";
            txtAmount.Size = new Size(120, 27);
            txtAmount.TabIndex = 11;
             
            // btnTransfer
             
            btnTransfer.Location = new Point(380, 440);
            btnTransfer.Name = "btnTransfer";
            btnTransfer.Size = new Size(100, 27);
            btnTransfer.TabIndex = 12;
            btnTransfer.Text = "Перевести";
            btnTransfer.UseVisualStyleBackColor = true;
             
            // MainForm
             
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 520);
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