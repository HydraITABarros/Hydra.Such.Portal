﻿using System;

namespace Hydra.Such.Data.Database
{
    public partial class Visitas
    {
        public int ID { get; set; }
        public string CodVisita { get; set; }
        public string Objetivo { get; set; }
        public string Local { get; set; }
        public string CodCliente { get; set; }
        public string CodFornecedor { get; set; }
        public string Entidade { get; set; }
        public string CodRegiao { get; set; }
        public string CodArea { get; set; }
        public string CodCresp { get; set; }
        public DateTime? InicioDataHora { get; set; }
        public DateTime? FimDataHora { get; set; }
        public int? CodEstado { get; set; }
        public string IniciativaCriador { get; set; }
        public string IniciativaResponsavel { get; set; }
        public string IniciativaIntervinientes { get; set; }
        public string RececaoCriador { get; set; }
        public string RececaoResponsavel { get; set; }
        public string RececaoIntervinientes { get; set; }
        public string RelatorioSimplificado { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
    }
}
