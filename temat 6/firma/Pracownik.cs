using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace firma
{
    [Table(Name = "Pracownicy")]
    internal class Pracownik
    {
        [Column(Name = "Id", IsPrimaryKey = true, CanBeNull = false)] public int Id;
        [Column(CanBeNull = false)] public string Imię;
        [Column(CanBeNull = false)] public string Nazwisko;
        [Column(CanBeNull = false)] public string Email;
        [Column(CanBeNull = false)] public string Telefon;
    }
}
