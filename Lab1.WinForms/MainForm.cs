using laba1.Models;

namespace Lab1.WinForms
{
    public partial class MainForm : Form
    {
        private Logic logic = new Logic();

        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
            btnCreate.Click += btnCreate_Click;
            btnEditOwner.Click += btnEditOwner_Click;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = logic.GetActiveAccount();
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Кнопка нажата!"); // временная проверка

            string accountNumber = txtAccountNumber.Text;
            string ownerName = txtOwnerName.Text;

            if (string.IsNullOrWhiteSpace(accountNumber) || string.IsNullOrWhiteSpace(ownerName))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            var newAccount = new BankAccount(accountNumber, ownerName, 0m);
            logic.AddAccount(newAccount);

            RefreshGrid();

            txtAccountNumber.Clear();
            txtOwnerName.Clear();
        }
        private void btnEditOwner_Click(object sender, EventArgs e)
        {
           
            if (SelectedAccount == null)
            {
                MessageBox.Show("Выберите счёт в таблице.");
                return;
            }

            string newOwner = txtNewOwner.Text;
            if (string.IsNullOrWhiteSpace(newOwner))
            {
                MessageBox.Show("Введите новое имя владельца.");
                return;
            }

            if (logic.EditAccountOwner(SelectedAccount.AccountNumber, newOwner))
            {
                RefreshGrid();
                txtNewOwner.Clear();
            }
            else
            {
                MessageBox.Show("Не удалось изменить владельца.");
            }
        }
        private BankAccount? SelectedAccount =>
    dataGridView1.CurrentRow?.DataBoundItem as BankAccount;
    }
}