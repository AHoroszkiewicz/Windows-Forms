using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace firma
{
    [Table(Name = "Kontrahent")]
    internal class Kontrahent
    {
        [Column(Name = "Id", IsPrimaryKey = true, CanBeNull = false)] public int Id;
        [Column(CanBeNull = false)] public string Nazwa;
        [Column(CanBeNull = true)] public string Nip;
        [Column(CanBeNull = false)] public string Ulica;
        [Column(CanBeNull = false)] public string Miasto;
    }
}
