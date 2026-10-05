using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab1.Model.ModelsDB
{
    public class Account : IDomainObject
    {
        [Key]
        [Column("id_account")] 
        public int Id { get; set; }
        public int id_role { get; set; }
        public string full_name { get; set; }
        public DateOnly date_birth { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public DateTime date_create { get; set; }

        [ForeignKey("id_role")]
        public Role Role { get; set; }
    }
}
