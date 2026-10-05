using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lab1.Model.ModelsDB
{
    public class HistoryTransaction
    {
        [Key]
        [Column("id_transaction")]
        public int Id { get; set; }

        public int id_type { get; set; }

        public int id_recipient { get; set; }

        public int? id_sender { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal amount { get; set; }

        public DateTime datetime_transaction { get; set; } = DateTime.Now;

        [ForeignKey(nameof(id_type))]
        public virtual Type_transaction? Type_transaction { get; set; }

        [ForeignKey(nameof(id_recipient))]
        public virtual BankAccount? RecipientAccount { get; set; }

        [ForeignKey(nameof(id_sender))]
        public virtual BankAccount? SenderAccount { get; set; }
    }
}
