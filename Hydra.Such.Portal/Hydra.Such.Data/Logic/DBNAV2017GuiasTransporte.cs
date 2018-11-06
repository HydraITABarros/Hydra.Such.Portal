using Hydra.Such.Data;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.GuiaTransporte;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017GuiasTransporte
    {
        public static List<GuiaTransporteNavViewModel> GetListByDim(string NAVDatabase, string NAVCompany, List<AcessosDimensões> Dimensions, bool isHistoric)
        {
            try
            {
                SuchDBContextExtention _contextExt = new SuchDBContextExtention();
                List<GuiaTransporteNavViewModel> result = new List<GuiaTransporteNavViewModel>();

                var regions = Dimensions.Where(d => d.Dimensão == (int)Enumerations.Dimensions.Region).Select(s => s.ValorDimensão);
                var functionalAreas = Dimensions.Where(d => d.Dimensão == (int)Enumerations.Dimensions.FunctionalArea).Select(s => s.ValorDimensão);
                var responsabilityCenters = Dimensions.Where(d => d.Dimensão == (int)Enumerations.Dimensions.ResponsabilityCenter).Select(s => s.ValorDimensão);

                var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabase),
                        new SqlParameter("@CompanyName", NAVCompany),
                        new SqlParameter("@Regions", regions != null && regions.Count() > 0 ? string.Join(',', regions) : null),
                        new SqlParameter("@FunctionalAreas",functionalAreas != null && functionalAreas.Count() > 0 ?  string.Join(',', functionalAreas): null),
                        new SqlParameter("@RespCenters", responsabilityCenters != null && responsabilityCenters.Count() > 0 ? '\'' + string.Join("',\'",responsabilityCenters) + '\'': null),
                        new SqlParameter("@IsHistoric", isHistoric ? 1 : 0)
                };

                IEnumerable<dynamic> data = _contextExt.execStoredProcedure("exec NAV2017GuiasTransporteList @DBName, @CompanyName, @Regions, @FunctionalAreas, @RespCenters, @IsHistoric", parameters);

                foreach(dynamic temp in data)
                {
                    int hist = 0;
                     if(temp != null)
                    {
                        hist = temp.Historico.Equals(DBNull.Value)? 0 : (int)temp.Historico;
                        result.Add(new GuiaTransporteNavViewModel() {
                            NoGuiaTransporte = temp.NoGuia.Equals(DBNull.Value) ? "" : (string)temp.NoGuia,
                            Historico = hist == 0 ? false:true,
                            DataGuia = temp.DataGuia.Equals(DBNull.Value)? DateTime.Parse("1900-01-01") : (DateTime)temp.DataGuia,
                            NoProjecto = temp.NoProjecto.Equals(DBNull.Value) ? "" : (string)temp.NoProjecto,
                            Utilizador = temp.Utilizador.Equals(DBNull.Value) ? "" : (string)temp.Utilizador,
                            NoCliente = temp.NoCliente.Equals(DBNull.Value) ? "" : (string)temp.NoCliente,
                            NomeCliente = temp.NomeCliente.Equals(DBNull.Value) ? "" : (string)temp.NomeCliente,
                            NoRequisicao = temp.NoRequisicao.Equals(DBNull.Value) ? "" : (string)temp.NoRequisicao,
                            GlobalDimension1Code = temp.GlobalDimension1Code.Equals(DBNull.Value) ? "" : (string)temp.GlobalDimension1Code,
                            GlobalDimension2Code = temp.GlobalDimension2Code.Equals(DBNull.Value) ? "" : (string)temp.GlobalDimension2Code,
                            GlobalDimension3Code = temp.GlobalDimension3Code.Equals(DBNull.Value) ? "" : (string)temp.GlobalDimension3Code,
                            ResponsabilityCenter = temp.ResponsabilityCenter.Equals(DBNull.Value) ? "" : (string)temp.ResponsabilityCenter,
                            GuiaTransporteInterface = temp.GuiaTransporteInterface.Equals(DBNull.Value) ? 0 : (int)temp.GuiaTransporteInterface,
                            NoGuiaOriginalInterface = temp.NoGuiaOriginalInterface.Equals(DBNull.Value) ? "" : (string)temp.NoGuiaOriginalInterface,
                            DimensionSetId = temp.DimensionSetId.Equals(DBNull.Value) ? 0 : (int)temp.DimensionSetId

                        });
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
               return null;
            }
            
        }

        public static GuiaTransporteNavViewModel GetDetailsByNo(string NAVDatabase, string NAVCompany, List<AcessosDimensões> Dimensions, string noGuia, bool isHistoric)
        {
            try
            {
                SuchDBContextExtention _contextExt = new SuchDBContextExtention();
                
                var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabase),
                        new SqlParameter("@CompanyName", NAVCompany),
                        new SqlParameter("@No", noGuia),
                        new SqlParameter("@IsHistoric", isHistoric ? 1 : 0)
                };

                dynamic data = _contextExt.execStoredProcedure("exec NAV2017GuiaTransportDetails @DBName, @CompanyName, @No, @IsHistoric", parameters).FirstOrDefault();

                if (data == null)
                    return null;

                GuiaTransporteNavViewModel result = new GuiaTransporteNavViewModel() {
                    NoGuiaTransporte = data.NoGuiaTransporte.Equals(DBNull.Value) ? "" : (string)data.NoGuiaTransporte

                };

                List<LinhaGuiaTransporteNavViewModel> linhasGt = new List<LinhaGuiaTransporteNavViewModel>();

                dynamic gtlines = _contextExt.execStoredProcedure("exec NAV2017GuiaTransportLines @DBName, @CompanyName, @No, @IsHistoric", parameters);


                foreach(dynamic ln in gtlines)
                {
                     if(ln != null)
                    {
                        LinhaGuiaTransporteNavViewModel line = new LinhaGuiaTransporteNavViewModel()
                        {
                            NoGuiaTransporte = (string)ln.NoDocumento,
                            NoLinha = ln.NoLinha.Equals(DBNull.Value) ? 1 : (int)ln.NoLinha
                        };

                        linhasGt.Add(line);
                    }
                }
                result.LinhasGuiaTransporte = linhasGt;
                return result;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
