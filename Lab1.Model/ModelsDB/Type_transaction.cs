using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lab1.Model.ModelsDB
{
    public class Type_transaction
    {
        [Key]
        [Column("id_role")]
        public int Id { get; set; }
        public string name { get; set; }
    }
}
