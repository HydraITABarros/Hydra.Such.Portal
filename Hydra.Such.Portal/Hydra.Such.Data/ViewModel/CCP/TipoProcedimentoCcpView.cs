using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.CCP
{
    public class TipoProcedimentoCcpView : TipoProcedimentoCcp
    {
        public string DescricaoView { get; set; }
        public TipoProcedimentoCcpView(TipoProcedimentoCcp tipo)
        {
            IdTipo = tipo.IdTipo;
            DescricaoTipo = tipo.DescricaoTipo;
            Abreviatura = tipo.Abreviatura;

            if(tipo.IdTipo == 0)
            {
                DescricaoView = "";
            }
            else
            {
                 DescricaoView = tipo.Abreviatura + "-" + tipo.DescricaoTipo;
            }
            
        }
    }
}
