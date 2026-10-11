using Lab1.DataAccessLayer;
using Lab1.Model.ModelsDB;
using Microsoft.Data.SqlClient;

namespace Lab1.WinForms
{   public partial class MainForm : Form
    {
        /// <summary>Форма для работы с банковскими счетами.</summary>
        private readonly IRepository<BankAccount> _repository;
        /// <summary>
        /// Инициализирует форму с указанным репозиторием для работы с банковскими счетами.
        /// </summary>
        /// <param name="repository">Репозиторий для работы с банковскими счетами.</param>
        public MainForm(IRepository<BankAccount> repository)
        {
            _repository = repository;

            InitializeComponent();
            Load += MainForm_Load;
            btnCreate.Click += btnCreate_Click;
            btnEditOwner.Click += btnEditOwner_Click;
            btnDelete.Click += btnDelete_Click;
            btnRestore.Click += btnRestore_Click;
            btnFreezeToggle.Click += btnFreezeToggle_Click;
            btnTransfer.Click += btnTransfer_Click;
            chkShowDeleted.CheckedChanged += (s, e) => RefreshGrid();
        }

        /// <summary>Счёт, выбранный в таблице (или null).</summary>
        private BankAccount? SelectedAccount =>
            dataGridView1.CurrentRow?.DataBoundItem as BankAccount;
        /// <summary>
        /// Обработчик события загрузки формы. Настраивает таблицу и обновляет данные.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Параметры события.</param>
        private void MainForm_Load(object? sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            RefreshGrid();
        }

        /// <summary>
        /// Обновляет данные в таблице, отображая счета в соответствии с состоянием флага "Показать удалённые".
        /// </summary>
        private void RefreshGrid()
        {
            Run(() =>
            {
                var accounts = _repository.ReadAll()
                    .Where(a => a.isDeleted == chkShowDeleted.Checked)
                    .ToList();

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = accounts;
                ConfigureColumns();
            });
        }
        /// <summary>
        /// Настраивает отображение столбцов в таблице, скрывая ненужные и задавая понятные заголовки.
        /// </summary>
        private void ConfigureColumns()
        {
            // Навигационные свойства Dapper не заполняет — столбцы в таблице не нужны.
            foreach (var name in new[] { "Type_BankAccount", "Account" })
                if (dataGridView1.Columns.Contains(name))
                    dataGridView1.Columns[name]!.Visible = false;

            var headers = new Dictionary<string, string>
            {
                ["Id"] = "ID",
                ["number"] = "Номер счёта",
                ["id_type"] = "Тип счёта",
                ["id_owner"] = "ID владельца",
                ["date_create"] = "Дата открытия",
                ["balance"] = "Баланс",
                ["isActiv"] = "Активен",
                ["isDeleted"] = "Удалён",
            };
            foreach (var (name, header) in headers)
                if (dataGridView1.Columns.Contains(name))
                    dataGridView1.Columns[name]!.HeaderText = header;
        }
        /// <summary>
        /// Выполняет указанное действие с обработкой исключений, связанных с базой данных.
        /// </summary>
        /// <param name="action">Действие для выполнения.</param>
        /// <returns>True, если действие выполнено успешно; иначе — False.</returns>
        private bool Run(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (SqlException ex) when (ex.Number == 547)   // нарушение внешнего ключа
            {
                MessageBox.Show("Владельца с таким ID не существует. Допустимые ID: 1–5.");
                return false;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка базы данных: " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Обработчик события нажатия кнопки "Создать". Создает новый банковский счет с указанными номером и ID владельца.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Параметры события.</param>

        private void btnCreate_Click(object? sender, EventArgs e)
        {
            string number = txtAccountNumber.Text.Trim();

            if (string.IsNullOrWhiteSpace(number) || string.IsNullOrWhiteSpace(txtOwnerName.Text))
            {
                MessageBox.Show("Заполните номер счёта и ID владельца!");
                return;
            }
            if (number.Length > 20 || !number.All(char.IsDigit))
            {
                MessageBox.Show("Номер счёта — только цифры, не длиннее 20 символов.");
                return;
            }
            if (!int.TryParse(txtOwnerName.Text, out int ownerId) || ownerId <= 0)
            {
                MessageBox.Show("ID владельца должен быть положительным числом.");
                return;
            }

            Run(() =>
            {
            if (_repository.ReadAll().Any(a => a.number.Trim() == number))
                {
                    MessageBox.Show("Счёт с таким номером уже существует.");
                    return;
                }

                _repository.Add(new BankAccount
                {
                    number = number,
                    id_owner = ownerId,
                    id_type = 1,                 // 1 = «Текущий» из справочника Type_BankAccount
                    balance = 0m,
                    date_create = DateTime.Today,
                    isActiv = true,
                    isDeleted = false
                });

                txtAccountNumber.Clear();
                txtOwnerName.Clear();
                RefreshGrid();
            });
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Изменить владельца". Изменяет ID владельца выбранного банковского счета.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Параметры события.</param>
        private void btnEditOwner_Click(object? sender, EventArgs e)
        {
            var account = SelectedAccount;
            if (account == null)
            {
                MessageBox.Show("Выберите счёт в таблице.");
                return;
            }
            if (!int.TryParse(txtNewOwner.Text, out int newOwnerId) || newOwnerId <= 0)
            {
                MessageBox.Show("Введите корректный ID нового владельца.");
                return;
            }

            account.id_owner = newOwnerId;
            if (Run(() => _repository.Update(account)))
            {
                txtNewOwner.Clear();
                RefreshGrid();
            }
        }
        /// <summary>
        /// Обработчик события нажатия кнопки "Удалить". Выполняет мягкое удаление выбранного банковского счета, если его баланс равен нулю.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Параметры события.</param>
        private void btnDelete_Click(object? sender, EventArgs e)
        {
            var account = SelectedAccount;
            if (account == null)
            {
                MessageBox.Show("Выберите счёт в таблице.");
                return;
            }
            if (account.balance != 0)
            {
                MessageBox.Show("Нельзя удалить счёт с ненулевым балансом.");
                return;
            }

            // «Мягкое» удаление: строка остаётся в БД с флагом isDeleted
            account.isDeleted = true;
            account.isActiv = false;
            if (Run(() => _repository.Update(account)))
                RefreshGrid();
        }
        /// <summary>
        /// Обработчик события нажатия кнопки "Восстановить". Восстанавливает удалённый банковский счет, делая его активным.    
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Параметры события.</param>
        private void btnRestore_Click(object? sender, EventArgs e)
        {
            var account = SelectedAccount;
            if (account == null || !account.isDeleted)
            {
                MessageBox.Show("Отметьте «Показать удалённые» и выберите удалённый счёт.");
                return;
            }

            account.isDeleted = false;
            account.isActiv = true;
            if (Run(() => _repository.Update(account)))
                RefreshGrid();
        }
        /// <summary>
        /// Обработчик события нажатия кнопки "Заморозить/Разморозить". Переключает состояние активности выбранного банковского счета.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Параметры события.</param>
        private void btnFreezeToggle_Click(object? sender, EventArgs e)
        {
            var account = SelectedAccount;
            if (account == null)
            {
                MessageBox.Show("Выберите счёт в таблице.");
                return;
            }

            // FreezeAccount/UnfreezeAccount уже есть в сущности, репозиторий лишь сохраняет результат
            if (account.isActiv) account.FreezeAccount();
            else account.UnfreezeAccount();

            if (Run(() => _repository.Update(account)))
                RefreshGrid();
        }
        /// <summary>
        ///     Обработчик события нажатия кнопки "Перевести". Выполняет перевод средств с выбранного банковского счета на указанный счет-получатель.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Параметры события.</param>
        private void btnTransfer_Click(object? sender, EventArgs e)
        {
            var sender_ = SelectedAccount;
            if (sender_ == null)
            {
                MessageBox.Show("Выберите счёт-отправитель в таблице.");
                return;
            }
            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную положительную сумму.");
                return;
            }

            string targetNumber = txtTargetAccount.Text.Trim();

            Run(() =>
            {
            var target = _repository.ReadAll()
                .FirstOrDefault(a => a.number.Trim() == targetNumber && !a.isDeleted);

            if (target == null)
            {
                MessageBox.Show("Счёт-получатель не найден.");
                return;
                }
                if (target.Id == sender_.Id)
                {
                    MessageBox.Show("Нельзя перевести самому себе.");
                    return;
                }
                if (!sender_.isActiv || !target.isActiv)
                {
                    MessageBox.Show("Один из счетов заморожен.");
                    return;
                }
                if (!sender_.Withdraw(amount))
                {
                    MessageBox.Show("Недостаточно средств на счёте.");
                    return;
                }

                target.balance += amount;
                _repository.Update(sender_);
                _repository.Update(target);

                txtTargetAccount.Clear();
                txtAmount.Clear();
                RefreshGrid();
            });
        }
        /// <summary>
        /// Обработчик события нажатия на ячейку таблицы. В текущей реализации не выполняет никаких действий, но может быть расширен для обработки кликов по ячейкам.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Параметры события.</param>
        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
        }
    }
}