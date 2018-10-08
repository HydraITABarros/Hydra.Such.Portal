﻿using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.PedidoCotacao
{
    public class SeleccaoEntidadesView : ErrorHandler
    {
        public int IdSeleccaoEntidades { get; set; }
        public string NumConsultaMercado { get; set; }
        public string CodFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public string CodActividade { get; set; }
        public string CidadeFornecedor { get; set; }
        public string CodTermosPagamento { get; set; }
        public string CodFormaPagamento { get; set; }
        public bool? Selecionado { get; set; }
        public bool? Preferencial { get; set; }
        public string EmailFornecedor { get; set; }
        public DateTime? DataEnvioAoFornecedor { get; set; }
        public DateTime? DataRecepcaoProposta { get; set; }
        public string UtilizadorEnvio { get; set; }
        public string UtilizadorRecepcaoProposta { get; set; }

        //Campos tratados
        public string DataEnvioAoFornecedor_Show { get; set; }
        public string DataRecepcaoProposta_Show { get; set; }
    }
}
