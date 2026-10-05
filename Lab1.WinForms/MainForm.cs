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
        /// <summary>
        /// Обрабатывает загрузку главной формы: заполняет таблицу актуальным списком счетов.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }
        /// <summary>
        /// Обновляет содержимое таблицы счетов в зависимости от выбранного режима
        /// отображения (активные или удалённые счета).
        /// </summary>
        private void RefreshGrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = chkShowDeleted.Checked
                ? logic.GetDeletedAccount()
                : logic.GetActiveAccount();
        }
        /// <summary>
        /// Обрабатывает нажатие кнопки "Создать": считывает данные из полей ввода,
        /// создаёт новый банковский счёт и добавляет его в список.
        /// </summary>
        /// <param name="sender">Источник события (кнопка)</param>
        /// <param name="e">Данные события</param>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            

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
        /// <summary>
        /// Обрабатывает нажатие кнопки "Изменить владельца": заменяет ФИО владельца
        /// у счёта, выбранного в таблице, на введённое пользователем значение.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
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
        /// <summary>
        /// Обрабатывает нажатие кнопки "Удалить": помечает выбранный в таблице счёт
        /// как удалённый, если его баланс равен нулю.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
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
        /// <summary>
        /// Обрабатывает нажатие кнопки "Восстановить": возвращает выбранный в таблице
        /// удалённый счёт в список активных.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
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

        /// <summary>
        /// Обрабатывает нажатие кнопки "Заморозить/Разморозить": переключает статус
        /// активности выбранного в таблице счёта на противоположный.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
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
        /// <summary>
        /// Обрабатывает нажатие кнопки "Перевести": выполняет перевод указанной суммы
        /// с выбранного в таблице счёта на счёт с введённым номером получателя.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
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
        /// <summary>
        /// Обрабатывает клик по ячейке таблицы счетов.
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события с информацией о выбранной ячейке</param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        /// <summary>
        /// Возвращает банковский счёт, выбранный в данный момент в таблице,
        /// или null, если ни одна строка не выбрана.
        /// </summary>
        private void btnFreezeToggle_Click_1(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Возвращает банковский счёт, выбранный в данный момент в таблице,
        /// </summary>
        private BankAccount? SelectedAccount =>
        dataGridView1.CurrentRow?.DataBoundItem as BankAccount;
    }
}