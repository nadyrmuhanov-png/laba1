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
            btnDelete.Click += btnDelete_Click;
            btnRestore.Click += btnRestore_Click;
            btnFreezeToggle.Click += btnFreezeToggle_Click;
            btnTransfer.Click += btnTransfer_Click;
            chkShowDeleted.CheckedChanged += (s, e) => RefreshGrid();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = chkShowDeleted.Checked
                ? logic.GetDeletedAccount()
                : logic.GetActiveAccount();
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
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (SelectedAccount == null)
            {
                MessageBox.Show("Выберите счёт в таблице.");
                return;
            }

            if (logic.RemoveAccount(SelectedAccount.AccountNumber))
            {
                RefreshGrid();
            }
            else
            {
                MessageBox.Show("Нельзя удалить счёт с ненулевым балансом.");
            }
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (SelectedAccount == null)
            {
                MessageBox.Show("Выберите удалённый счёт в таблице.");
                return;
            }

            if (logic.RestoreAccountByNumber(SelectedAccount.AccountNumber))
            {
                RefreshGrid();
            }
            else
            {
                MessageBox.Show("Не удалось восстановить счёт.");
            }
        }
        private void btnFreezeToggle_Click(object sender, EventArgs e)
        {
            if (SelectedAccount == null)
            {
                MessageBox.Show("Выберите счёт в таблице.");
                return;
            }

            if (SelectedAccount.IsActive)
                logic.FreezeAccountNumber(SelectedAccount.AccountNumber);
            else
                logic.UnfreezeAccountNumber(SelectedAccount.AccountNumber);

            RefreshGrid();
        }
        private void btnTransfer_Click(object sender, EventArgs e)
        {
            if (SelectedAccount == null)
            {
                MessageBox.Show("Выберите счёт-отправитель в таблице.");
                return;
            }

            var target = logic.GetAccount(txtTargetAccount.Text);
            if (target == null)
            {
                MessageBox.Show("Счёт-получатель не найден.");
                return;
            }

            if (target.AccountNumber == SelectedAccount.AccountNumber)
            {
                MessageBox.Show("Нельзя перевести самому себе.");
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Введите корректную сумму.");
                return;
            }

            if (logic.Transfer(SelectedAccount, target, amount))
            {
                RefreshGrid();
                txtTargetAccount.Clear();
                txtAmount.Clear();
            }
            else
            {
                MessageBox.Show("Перевод не выполнен — проверьте баланс и статус счетов.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnFreezeToggle_Click_1(object sender, EventArgs e)
        {

        }

        private BankAccount? SelectedAccount =>
        dataGridView1.CurrentRow?.DataBoundItem as BankAccount;
    }
}