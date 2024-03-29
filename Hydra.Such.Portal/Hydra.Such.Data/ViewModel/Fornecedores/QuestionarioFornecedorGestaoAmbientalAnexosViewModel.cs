﻿using System;

namespace Hydra.Such.Data.ViewModel.Fornecedores
{
    public class QuestionarioFornecedorGestaoAmbientalAnexosViewModel : ErrorHandler
    {
        public string Codigo { get; set; }
        public int Versao { get; set; }
        public string ID_Fornecedor { get; set; }
        public string Fornecedor { get; set; }
        public string URL_Anexo { get; set; }
        public bool? Visivel { get; set; }
        public string Visivel_Texto { get; set; }
        public DateTime? DataHora_Criacao { get; set; }
        public string DataHora_Criacao_Texto { get; set; }
        public string Utilizador_Criacao { get; set; }
        public string Utilizador_Criacao_Texto { get; set; }
        public DateTime? DataHora_Modificacao { get; set; }
        public string DataHora_Modificacao_Texto { get; set; }
        public string Utilizador_Modificacao { get; set; }
        public string Utilizador_Modificacao_Texto { get; set; }
    }
}
