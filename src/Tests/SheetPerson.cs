using System.ComponentModel;
using SpreadSheetParser.Models;

namespace Tests
{
    [DisplayName("Cadastro Pessoa")]
    public class SheetPerson : SheetObject
    {
        [DisplayName("Nome Cliente")]
        public string ClientName { get; set; }

        [DisplayName("Número CPF/CNPJ")]
        public string DocumentNumber { get; set; }

        [DisplayName("Data Nascimento")]
        public DateTime BirthDay { get; set; }


        [DisplayName("Altura")]
        public Double Hight { get; set; }

        public SheetPerson(SheetHeader header, SheetRow row) : base(header, row)
        {
            if (TryBuildObject<SheetPerson>(this) is false)
                throw new ArgumentException("Could not create a sample object");
        }

    }
}