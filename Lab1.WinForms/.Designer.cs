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
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Top;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(800, 300);
            dataGridView1.TabIndex = 0;
            //
            // txtAccountNumber
            //
            txtAccountNumber.Location = new Point(20, 320);
            txtAccountNumber.Name = "txtAccountNumber";
            txtAccountNumber.Size = new Size(200, 23);
            txtAccountNumber.TabIndex = 1;
            txtAccountNumber.PlaceholderText = "Номер счёта";
            //
            // txtOwnerName
            //
            txtOwnerName.Location = new Point(240, 320);
            txtOwnerName.Name = "txtOwnerName";
            txtOwnerName.Size = new Size(200, 23);
            txtOwnerName.TabIndex = 2;
            txtOwnerName.PlaceholderText = "ФИО владельца";
            //
            // btnCreate
            //
            btnCreate.Location = new Point(460, 320);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(100, 23);
            btnCreate.TabIndex = 3;
            btnCreate.Text = "Создать";
            btnCreate.UseVisualStyleBackColor = true;
            //
            // txtNewOwner
            //
            txtNewOwner.Location = new Point(20, 360);
            txtNewOwner.Name = "txtNewOwner";
            txtNewOwner.Size = new Size(200, 23);
            txtNewOwner.TabIndex = 4;
            txtNewOwner.PlaceholderText = "Новый владелец";
            //
            // btnEditOwner
            //
            btnEditOwner.Location = new Point(240, 360);
            btnEditOwner.Name = "btnEditOwner";
            btnEditOwner.Size = new Size(150, 23);
            btnEditOwner.TabIndex = 5;
            btnEditOwner.Text = "Изменить владельца";
            btnEditOwner.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            components = new System.ComponentModel.Container();
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCreate);
            Controls.Add(txtOwnerName);
            Controls.Add(txtAccountNumber);
            Controls.Add(dataGridView1);
            Controls.Add(txtNewOwner);
            Controls.Add(btnEditOwner);
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
    }
}