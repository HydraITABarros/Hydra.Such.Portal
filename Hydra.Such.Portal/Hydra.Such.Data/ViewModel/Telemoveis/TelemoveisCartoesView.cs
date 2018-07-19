using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Telemoveis
{
    public class TelemoveisCartoesView : ErrorHandler
    {
        public string NumCartao { get; set; }
        public int TipoServico { get; set; }
        public string ContaSuch { get; set; }
        public string ContaUtilizador { get; set; }
        public string Barramentos { get; set; }
        public string TarifarioVoz { get; set; }
        public string TarifarioDados { get; set; }
        public string ExtensaoVpn { get; set; }
        public int PlafondFr { get; set; }
        public int PlafondExtra { get; set; }
        public DateTime? FimFidelizacao { get; set; }
        public int Gprs { get; set; }
        public int Estado { get; set; }
        public DateTime DataEstado { get; set; }
        public string Observacoes { get; set; }
        public string NumFuncionario { get; set; }
        public string Nome { get; set; }
        public string CodRegiao { get; set; }
        public string CodAreaFuncional { get; set; }
        public string CodCentroResponsabilidade { get; set; }
        public string Grupo { get; set; }
        public string Imei { get; set; }
        public DateTime? DataAtribuicao { get; set; }
        public byte ChamadasInternacionais { get; set; }
        public byte Roaming { get; set; }
        public byte Internet { get; set; }
        public byte Declaracao { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public byte Plafond100percUtilizador { get; set; }
        public string WhiteList { get; set; }
        public decimal ValorMensalidadeDados { get; set; }
        public int PlafondDados { get; set; }
        public byte EquipamentoNaoDevolvido { get; set; }

        //Campos tratados
        public string TipoServico_Show { get; set; }
        public string Estado_Show { get; set; }
        public string DataEstado_Show { get; set; }
        public string FimFidelizacao_Show { get; set; }
        public bool Plafond100percUtilizador_Show { get; set; }
        public bool ChamadasInternacionais_Show { get; set; }
        public bool Roaming_Show { get; set; }
        public string Consumos_Show { get; set; }
        public decimal Consumos { get; set; }
        public string DataAtribuicao_Show { get; set; }
        public bool EquipamentoNaoDevolvido_Show { get; set; }
        public string Marca_Show { get; set; }
        public string DataAlteracao_Show { get; set; }
    }
}
