using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class ContactViewModel : ErrorHandler
    {
        public string No { get; set; }
        public string NoCliente { get; set; }
        public string ClienteNome { get; set; }
        public string ClienteNIF { get; set; }
        public string ClienteEndereco { get; set; }
        public string ClienteCodigoPostal { get; set; }
        public string ClienteCidade { get; set; }
        public string ClienteRegiao { get; set; }
        public string ClienteTelefone { get; set; }
        public string ClienteEmail { get; set; }
        public int? NoServico { get; set; }
        public string ServicoNome { get; set; }
        public int? NoFuncao { get; set; }
        public string FuncaoNome { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Telemovel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Pessoa { get; set; }
        public string Notas { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoText { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string DataAlteracaoText { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }

        public ContactViewModel()
        {
            this.No = string.Empty;
            this.NoCliente = string.Empty;
            this.ClienteNome = string.Empty;
            this.ClienteNIF = string.Empty;
            this.ClienteEndereco = string.Empty;
            this.ClienteCodigoPostal = string.Empty;
            this.ClienteCidade = string.Empty;
            this.ClienteRegiao = string.Empty;
            this.ClienteTelefone = string.Empty;
            this.ClienteEmail = string.Empty;
            this.ServicoNome = string.Empty;
            this.FuncaoNome = string.Empty;
            this.Nome = string.Empty;
            this.Telefone = string.Empty;
            this.Telemovel = string.Empty;
            this.Fax = string.Empty;
            this.Email = string.Empty;
            this.Pessoa = string.Empty;
            this.Notas = string.Empty;
            this.CriadoPor = string.Empty;
            this.DataCriacaoText = string.Empty;
            this.AlteradoPor = string.Empty;
            this.DataAlteracaoText = string.Empty;
        }
    }
}
