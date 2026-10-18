using Lab1.BusinessLogic;
using Lab1.Model.ModelsDB;

namespace Lab1.WinForms
{
    public partial class MainForm : Form
    {
       /// <summary>
       /// Экземпляр Logic, используемый для выполнения логических операций.
       /// </summary>
       /// <remarks>Инициализируется при объявлении и остаётся неизменным на протяжении жизни экземпляра
       /// содержащего типа.</remarks>
        private readonly Logic _logic = new Logic();

        public MainForm()
        {
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

        /// <summary>
        /// Получает выбранный в таблице счёт (или null).
        /// </summary>
        private BankAccount? SelectedAccount =>
            dataGridView1.CurrentRow?.DataBoundItem as BankAccount;

        private void MainForm_Load(object? sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            RefreshGrid();
        }

       /// <summary>
       /// Обновляет содержимое dataGridView1 списком счетов, получаемых из _logic.GetAccounts с учётом состояния
       /// chkShowDeleted, сбрасывает DataSource и перенастраивает столбцы.
       /// </summary>
       /// <remarks>Исключения при загрузке перехватываются и отображаются пользователю через MessageBox с
       /// текстом базовой ошибки.</remarks>
        private void RefreshGrid()
        {
            try
            {
                var accounts = _logic.GetAccounts(chkShowDeleted.Checked);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = accounts;
                ConfigureColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить счета: " + ex.GetBaseException().Message);
            }
        }

        private void ConfigureColumns()
        {
            
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

            if (dataGridView1.Columns.Contains("number"))
                dataGridView1.Columns["number"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
/// <summary>
/// Отображает сообщение об ошибке при неуспехе операции и возвращает признак успеха.
/// </summary>
/// <remarks>Побочный эффект: при неуспехе отображается MessageBox.</remarks>
/// <param name="result">Результат операции. При неуспехе отображается значение свойства Message.</param>
/// <returns>true, если операция успешна; иначе false.</returns>
        private bool Report(OperationResult result)
        {
            if (!result.Success)
                MessageBox.Show(result.Message);
            return result.Success;
        }
        /// <summary>
        /// Обрабатывает нажатие кнопки создания счета: проверяет ввод идентификатора владельца, вызывает бизнес-логику
        /// для создания счета и при успехе обновляет интерфейс.
        /// </summary>
        /// <remarks>Форма лишь разбирает ввод; проверку правил выполняет бизнес-логика. При некорректном
        /// ID владельца отображается сообщение. После успешного создания очищает поля ввода и обновляет
        /// сетку.</remarks>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события клика.</param>
        private void btnCreate_Click(object? sender, EventArgs e)
        {
            // Форма отвечает только за разбор ввода; правила проверяет бизнес-логика.
            if (!int.TryParse(txtOwnerName.Text, out int ownerId))
            {
                MessageBox.Show("ID владельца должен быть числом.");
                return;
            }

            if (Report(_logic.AddAccount(txtAccountNumber.Text, ownerId, typeId: 1, initialBalance: 0m)))
            {
                txtAccountNumber.Clear();
                txtOwnerName.Clear();
                RefreshGrid();
            }
        }
        /// <summary>
        /// Изменяет владельца выбранного счёта на указанный идентификатор владельца.
        /// </summary>
        /// <remarks>Проверяет наличие выбранного счёта и корректность ввода ID; при ошибке отображает
        /// сообщение, при успешном изменении очищает поле ввода и обновляет таблицу.</remarks>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void btnEditOwner_Click(object? sender, EventArgs e)
        {
            var account = SelectedAccount;
            if (account == null)
            {
                MessageBox.Show("Выберите счёт в таблице.");
                return;
            }
            if (!int.TryParse(txtNewOwner.Text, out int newOwnerId))
            {
                MessageBox.Show("ID нового владельца должен быть числом.");
                return;
            }

            if (Report(_logic.ChangeOwner(account.Id, newOwnerId)))
            {
                txtNewOwner.Clear();
                RefreshGrid();
            }
        }
        /// <summary>
        /// Удаляет выбранный счёт и обновляет таблицу при успешном удалении.
        /// </summary>
        /// <remarks>Если счёт не выбран, отображается сообщение с просьбой выбрать счёт. Удаление
        /// выполняется через слой логики; при успешном результате выполняется обновление представления.</remarks>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void btnDelete_Click(object? sender, EventArgs e)
        {
            var account = SelectedAccount;
            if (account == null)
            {
                MessageBox.Show("Выберите счёт в таблице.");
                return;
            }

            if (Report(_logic.DeleteAccount(account.Id)))
                RefreshGrid();
        }
      /// <summary>
      /// Восстанавливает выбранный удалённый счёт; при отсутствии выбора показывает информационное сообщение; при
      /// успешном восстановлении обновляет сетку.
      /// </summary>
      /// <remarks>Вызывает _logic.RestoreAccount и проверяет результат через Report; при успехе вызывает
      /// RefreshGrid. Если SelectedAccount равен null, показывает MessageBox с инструкцией: «Отметьте «Показать
      /// удалённые» и выберите удалённый счёт.»</remarks>
      /// <param name="sender">Источник события, инициировавшего клик.</param>
      /// <param name="e">Аргументы события клика.</param>
        private void btnRestore_Click(object? sender, EventArgs e)
        {
            var account = SelectedAccount;
            if (account == null)
            {
                MessageBox.Show("Отметьте «Показать удалённые» и выберите удалённый счёт.");
                return;
            }

            if (Report(_logic.RestoreAccount(account.Id)))
                RefreshGrid();
        }
      /// <summary>
      /// Переключает состояние заморозки выбранного счёта и при успешном выполнении обновляет таблицу.
      /// </summary>
      /// <remarks>Если счёт не выбран, отображает сообщение «Выберите счёт в таблице». Иначе вызывает
      /// _logic.ToggleFreeze(account.Id) и при успешном результате обновляет представление вызовом
      /// RefreshGrid().</remarks>
      /// <param name="sender">Источник события.</param>
      /// <param name="e">Аргументы события.</param>
        private void btnFreezeToggle_Click(object? sender, EventArgs e)
        {
            var account = SelectedAccount;
            if (account == null)
            {
                MessageBox.Show("Выберите счёт в таблице.");
                return;
            }

            if (Report(_logic.ToggleFreeze(account.Id)))
                RefreshGrid();
        }
        /// <summary>
        /// Выполняет перевод средств с выбранного счёта на указанный целевой счёт и обновляет интерфейс при успешной
        /// операции.
        /// </summary>
        /// <remarks>Проверяет наличие выбранного счёта и корректность введённой суммы; вызывает
        /// _logic.Transfer; при успешной операции очищает поля ввода и обновляет таблицу.</remarks>
        /// <param name="sender">Источник события, обычно элемент управления (например, кнопка), инициировавший клик.</param>
        /// <param name="e">Аргументы события Click.</param>
        private void btnTransfer_Click(object? sender, EventArgs e)
        {
            var account = SelectedAccount;
            if (account == null)
            {
                MessageBox.Show("Выберите счёт-отправитель в таблице.");
                return;
            }
            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Введите корректную сумму.");
                return;
            }

            if (Report(_logic.Transfer(account.Id, txtTargetAccount.Text, amount)))
            {
                txtTargetAccount.Clear();
                txtAmount.Clear();
                RefreshGrid();
            }
        }
        /// <summary>
        /// Обрабатывает щелчок по содержимому ячейки в DataGridView.
        /// Обрабатывает щелчок по содержимому ячейки в DataGridView.
        /// </summary>
        /// <remarks>Проверять e.RowIndex и e.ColumnIndex на значение -1 (заголовки или некорректная
        /// ячейка) перед доступом к данным.</remarks>
        /// <param name="sender">Источник события, обычно сам DataGridView.</param>
        /// <param name="e">Аргументы события с индексами столбца (ColumnIndex) и строки (RowIndex) целевой ячейки.</param>
        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
        }
    }
}