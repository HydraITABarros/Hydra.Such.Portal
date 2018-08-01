using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Telemoveis
{
    public class TelemoveisEquipamentosView : ErrorHandler
    {
        public int Tipo { get; set; }
        public string Imei { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Estado { get; set; }
        public string Cor { get; set; }
        public string Observacoes { get; set; }
        public DateTime? DataRecepcao { get; set; }
        public string Documento { get; set; }
        public string DocumentoRecepcao { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public byte DevolvidoBk { get; set; }
        public string NumEmpregadoComprador { get; set; }
        public string NomeComprador { get; set; }
        public int? Devolvido { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }

        //Campos com descrições
        public string Tipo_Show { get; set; }
        public string Estado_Show { get; set; }
        public string Devolvido_Show { get; set; }
        public string DataRecepcao_Show { get; set; }
        public string DataAlteracao_Show { get; set; }
        public string NomeUtilizadorCartao_Show { get; set; }
        public string DataAtribuicaoUtilizadorCartao_Show { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }

    }
}
