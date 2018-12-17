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
using Hydra.Such.Data.Extensions;

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

                foreach (dynamic temp in data)
                {
                    int hist = 0;
                    if (temp != null)
                    {
                        hist = temp.Historico.Equals(DBNull.Value) ? 0 : (int)temp.Historico;
                        GuiaTransporteNavViewModel guia = new GuiaTransporteNavViewModel()
                        {
                            NoGuiaTransporte = temp.NoGuia.Equals(DBNull.Value) ? "" : (string)temp.NoGuia,
                            Historico = hist == 0 ? false : true,
                            DataGuia = temp.DataGuia.Equals(DBNull.Value) ? DateTime.Parse("1900-01-01") : (DateTime)temp.DataGuia,
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
                        };

                        switch (guia.Tipo)
                        {
                            case 0:
                                guia.TipoDescription = "Cliente";
                                break;
                            case 1:
                                guia.TipoDescription = "Fornecedor";
                                break;
                            case 2:
                                guia.TipoDescription = "Armazém";
                                break;
                            case 3:
                                guia.TipoDescription = "Logística";
                                break;
                            default:
                                guia.TipoDescription = "Desconhecido";
                                break;
                        }
                        result.Add(guia);
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

                int hist = data.Historico.Equals(DBNull.Value) ? 0 : (int)data.Historico;
                
                GuiaTransporteNavViewModel result = new GuiaTransporteNavViewModel() {
                    NoGuiaTransporte = data.NoGuiaTransporte.Equals(DBNull.Value) ? "" : (string)data.NoGuiaTransporte,
                    Tipo = data.Tipo.Equals(DBNull.Value) ? 0 : (int)data.Tipo,
                    NoCliente = data.NoCliente.Equals(DBNull.Value) ? "" : (string)data.NoCliente,
                    NomeCliente = data.NomeCliente.Equals(DBNull.Value) ? "" : (string)data.NomeCliente,
                    NomeCliente2 = data.NomeCliente2.Equals(DBNull.Value) ? "" : (string)data.NomeCliente2,
                    MoradaCliente = data.MoradaCliente.Equals(DBNull.Value) ? "" : (string)data.MoradaCliente,
                    MoradaCliente2 = data.MoradaCliente2.Equals(DBNull.Value) ? "" : (string)data.MoradaCliente2,
                    Cidade = data.Cidade.Equals(DBNull.Value) ? "" : (string)data.Cidade,
                    CodPostal = data.CodPostal.Equals(DBNull.Value) ? "" : (string)data.CodPostal,
                    NifCliente = data.VatRegistrationNo.Equals(DBNull.Value) ? "" : (string)data.VatRegistrationNo,
                    SourceCode = data.SourceCode.Equals(DBNull.Value) ? "" : (string)data.SourceCode,
                    NoRequisicao = data.NoRequisicao.Equals(DBNull.Value) ? "" : (string)data.NoRequisicao,
                    DataGuia = data.DataGuia.Equals(DBNull.Value) ? DateTime.Parse("1900-01-01") : (DateTime)data.DataGuia,
                    DataSaida = data.DataSaida.Equals(DBNull.Value) ? DateTime.Parse("1900-01-01") : (DateTime)data.DataSaida,
                    
                    ShipmentStartTime = data.ShipmentStartTime.Equals(DBNull.Value) ? DateTime.Parse("1900-01-01") : TimeSpan.Parse(data.ShipmentStartTime.ToShortTimeString()),

                    Requisicao = data.Requisicao.Equals(DBNull.Value) ? "" : (string)data.Requisicao,
                    ReportedBy = data.ReportedBy.Equals(DBNull.Value) ? "" : (string)data.ReportedBy,
                    NoProjecto = data.NoProjecto.Equals(DBNull.Value) ? "" : (string)data.NoProjecto,
                    OrdemTransferencia = data.OrdemTransferencia.Equals(DBNull.Value) ? "" : (string)data.OrdemTransferencia,
                    Observacoes = data.Observacoes.Equals(DBNull.Value) ? "" : (string)data.Observacoes,
                    Origem = data.Origem.Equals(DBNull.Value) ? "" : (string)data.Origem,
                    ResponsabilityCenter = data.ResponsabilityCenter.Equals(DBNull.Value) ? "" : (string)data.ResponsabilityCenter,
                    QuantidadeTotal = data.QuantidadeTotal.Equals(DBNull.Value) ? 0 : (decimal)data.QuantidadeTotal,
                    PesoTotal = data.PesoTotal.Equals(DBNull.Value) ? 0 : (decimal)data.PesoTotal,
                    Utilizador = data.Utilizador.Equals(DBNull.Value) ? "" : (string)data.Utilizador,
                    Name = data.Name.Equals(DBNull.Value) ? "" : (string)data.Name,
                    Address = data.Address.Equals(DBNull.Value) ? "" : (string)data.Address,
                    City = data.City.Equals(DBNull.Value) ? "" : (string)data.City,
                    PostCode = data.PostCode.Equals(DBNull.Value) ? "" : (string)data.PostCode,
                    HoraCarga = data.HoraCarga.Equals(DBNull.Value) ? TimeSpan.Parse("00:00") : TimeSpan.Parse(data.HoraCarga.ToShortTimeString()),
                    DataCarga = data.DataCarga.Equals(DBNull.Value) ? DateTime.Parse("1900-01-01") : (DateTime)data.DataCarga,
                    PaisCarga = data.PaisCarga.Equals(DBNull.Value) ? "" : (string)data.PaisCarga,
                    LocalDescarga = data.LocalDescarga.Equals(DBNull.Value) ? "" : (string)data.LocalDescarga,
                    LocalDescarga1 = data.LocalDescarga1.Equals(DBNull.Value) ? "" : (string)data.LocalDescarga1,
                    CodPostalDescarga = data.CodPostalDescarga.Equals(DBNull.Value) ? "" : (string)data.CodPostalDescarga,
                    HoraDescarga = data.HoraDescarga.Equals(DBNull.Value) ? TimeSpan.Parse("00:00") : TimeSpan.Parse(data.HoraDescarga.ToShortTimeString()),
                    DataDescarga = data.DataDescarga.Equals(DBNull.Value) ? DateTime.Parse("1900-01-01") : (DateTime)data.DataDescarga,
                    Viatura = data.Viatura.Equals(DBNull.Value) ? "" : (string)data.Viatura,
                    PaisDescarga = data.PaisDescarga.Equals(DBNull.Value) ? "" : (string)data.PaisDescarga,
                    GlobalDimension1Code = data.GlobalDimension1Code.Equals(DBNull.Value) ? "" : (string)data.GlobalDimension1Code,
                    GlobalDimension2Code = data.GlobalDimension2Code.Equals(DBNull.Value) ? "" : (string)data.GlobalDimension2Code,
                    GlobalDimension3Code = data.GlobalDimension3Code.Equals(DBNull.Value) ? "" : (string)data.GlobalDimension3Code,
                    NoGuiaOriginalInterface = data.NoGuiaOriginalInterface.Equals(DBNull.Value) ? "" : (string)data.NoGuiaOriginalInterface,
                    GuiaTransporteInterface = data.GuiaTransporteInterface.Equals(DBNull.Value) ? 0 : (int)data.GuiaTransporteInterface,
                    DimensionSetId = data.DimensionSetId.Equals(DBNull.Value) ? 0 : (int)data.DimensionSetId,
                    Historico = hist == 0 ? false : true,
                    CodPais = data.CodPais.Equals(DBNull.Value) ? "" : (string)data.CodPais,
                    Telefone = data.Telefone.Equals(DBNull.Value) ? "" : (string)data.Telefone,
                    MaintenanceOrderNo = data.MaintOrderNo.Equals(DBNull.Value) ? "" : (string)data.MaintOrderNo,
                    ObservacoesAdicionais = data.ObservacoesAdicionais.Equals(DBNull.Value) ? "" : (string)data.ObservacoesAdicionais,
                    UserObservacoesAdicionai = data.UserObservacoesAdicionais.Equals(DBNull.Value) ? "" : (string)data.UserObservacoesAdicionais,
                    DataObservacoesAdicionais = data.DataObservacoesAdicionais.Equals(DBNull.Value) ? DateTime.Parse("1900-01-01") : (DateTime)data.DataObservacoesAdicionais,
                    HoraObservacoesAdicionais = data.HoraObservacoesAdicionais.Equals(DBNull.Value) ? TimeSpan.Parse("00:00") : TimeSpan.Parse(data.HoraObservacoesAdicionais.ToShortTimeString())
                };

                result.FiscalCommunicationLog = new FiscalAuthorityCommunicationLog();

                switch (result.Tipo)
                {
                    case 0:
                        result.TipoDescription = "Cliente";
                        break;
                    case 1:
                        result.TipoDescription = "Fornecedor";
                        break;
                    case 2:
                        result.TipoDescription = "Armazém";
                        break;
                    case 3:
                        result.TipoDescription = "Logística";
                        break;
                    default:
                        result.TipoDescription = "";
                        break;
                }
                List<LinhaGuiaTransporteNavViewModel> linhasGt = new List<LinhaGuiaTransporteNavViewModel>();

                dynamic gtlines = _contextExt.execStoredProcedure("exec NAV2017GuiaTransportLines @DBName, @CompanyName, @No, @IsHistoric", parameters);


                foreach(dynamic ln in gtlines)
                {
                     if(ln != null)
                    {
                        LinhaGuiaTransporteNavViewModel line = new LinhaGuiaTransporteNavViewModel()
                        {
                            ParentNo = result,
                            NoGuiaTransporte = (string)ln.NoDocumento,
                            NoLinha = ln.NoLinha.Equals(DBNull.Value) ? 1 : (int)ln.NoLinha,
                            Tipo = ln.Tipo.Equals(DBNull.Value) ? 0 : (int)ln.Tipo,
                            No = ln.No.Equals(DBNull.Value) ? "" : (string)ln.No,
                            Descricao = ln.Descricao.Equals(DBNull.Value) ? "" : (string)ln.Descricao,
                            CodUnidadeMedida = ln.CodUnidadeMedida.Equals(DBNull.Value) ? "" : (string)ln.CodUnidadeMedida,
                            Quantidade = ln.Quantidade.Equals(DBNull.Value) ? 0 : (decimal)ln.Quantidade,
                            QuantidadeEnviar = ln.QuantidadeEnviar.Equals(DBNull.Value) ? 0 : (decimal)ln.QuantidadeEnviar,
                            RefDocOrigem = ln.RefDocOrigem.Equals(DBNull.Value) ? "" : (string)ln.RefDocOrigem,
                            UnitCost = ln.UnitCost.Equals(DBNull.Value) ? 0 : (decimal)ln.UnitCost,
                            UnitPrice = ln.UnitPrice.Equals(DBNull.Value) ? 0 : (decimal)ln.UnitPrice,
                            ShortcutDimension1Code = ln.ShortcutDim1Code.Equals(DBNull.Value) ? "" : (string)ln.ShortcutDim1Code,
                            ShortcutDimension2Code = ln.ShortcutDim2Code.Equals(DBNull.Value) ? "" : (string)ln.ShortcutDim2Code,
                            FunctionalLocationNo = ln.FunctionalLocationNo.Equals(DBNull.Value) ? "" : (string)ln.FunctionalLocationNo,
                            EstadoEquipamento = ln.EstadoEquipamento.Equals(DBNull.Value) ? 0 : (int)ln.EstadoEquipamento,
                            InventoryNo = ln.InventoryNo.Equals(DBNull.Value) ? "" : (string)ln.InventoryNo
                        };


                        switch (line.Tipo)
                        {
                            case 0: line.TipoDescription = "Recurso";
                                break;
                            case 1: line.TipoDescription = "Produto";
                                break;
                            case 2: line.TipoDescription = "Conta";
                                break;
                            case 3: line.TipoDescription = "Equipamento";
                                break;
                            default: line.TipoDescription = "";
                                break;
                        }

                        switch (line.EstadoEquipamento)
                        {
                            case 0: line.EstadoEquipamentoDescription = "";
                                break;
                            case 1: line.EstadoEquipamentoDescription = "Reparado";
                                break;
                            case 2: line.EstadoEquipamentoDescription = "Não foi possível a reparação";
                                break;
                            default:  line.EstadoEquipamentoDescription = "";
                                break;
                        }

                        linhasGt.Add(line);
                    }
                }

                result.LinhasGuiaTransporte = linhasGt;

                if (isHistoric)
                {
                    dynamic flog = _contextExt.execStoredProcedure("exec NAV2017GuiaTransporteCommunicationLog @DBName, @CompanyName, @No", parameters).FirstOrDefault();

                    if (flog != null)
                    {
                        result.FiscalCommunicationLog = new FiscalAuthorityCommunicationLog()
                        {
                            SourceNo = flog.SourceNo.Equals(DBNull.Value) ? "" : (string)flog.SourceNo,
                            DocumentCodeId = flog.DocCodId.Equals(DBNull.Value) ? "" : (string)flog.DocCodId,
                            CommunicationDateTime = flog.DateTimeCommunication.Equals(DBNull.Value) ? DateTime.Parse("1900-01-01") : (DateTime)flog.DateTimeCommunication,
                            ReturnCode = flog.ReturnCode.Equals(DBNull.Value) ? "" : (string)flog.ReturnCode,
                            ReturnMessage = flog.ReturnMessage.Equals(DBNull.Value) ? "" : (string)flog.ReturnMessage
                        };
                        
                    }   
                }
                
                return result;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool UpdateGuiaTransporte(GuiaTransporteNavViewModel guia)
        {
            if(guia == null)
            {
                return false;

            }

            SuchDBContextExtention _contextExt = new SuchDBContextExtention();

            List<CabecalhoGuiaTransporteSqlModel> _cabecalhos = new List<CabecalhoGuiaTransporteSqlModel>
            {
                CastToCabecalhoType(guia)
            };


            if (!_contextExt.ExecuteTableValueProcedure<CabecalhoGuiaTransporteSqlModel>(_cabecalhos, "NAV2017CabGuiaTransporte_Update", "@CabecalhoGuia", "CabGuiaTransporteType"))
            {
                return false;
            }

            if(guia.LinhasGuiaTransporte != null)
            {
                if (!UpdateLinhasGuiaTransporte(guia.LinhasGuiaTransporte))
                {
                    return false;
                }
            }
            
            return true;
        }

        public static bool UpdateLinhasGuiaTransporte(List<LinhaGuiaTransporteNavViewModel> linhas)
        {
            if(linhas == null)
            {
                return false;
            }

            List<LinhaGuiaTransporteSqlModel> _linhasType = new List<LinhaGuiaTransporteSqlModel>();

            foreach(var l in linhas)
            {
                if(l != null)
                {
                    _linhasType.Add(CastToLinhaType(l));
                }
            }

            SuchDBContextExtention _contextExt = new SuchDBContextExtention();

            return _contextExt.ExecuteTableValueProcedure(_linhasType, "NAV2017LinGuiaTransporte_Update", "@LinhasGuia", "LinGuiaTransporteType");
        }

        public static bool UpdateLinhaGuiaTransporte(LinhaGuiaTransporteNavViewModel linha)
        {
            if (linha == null)
            {
                return false;
            }

            SuchDBContextExtention _contextExt = new SuchDBContextExtention();

            List<LinhaGuiaTransporteSqlModel> _linhas = new List<LinhaGuiaTransporteSqlModel>
            {
                CastToLinhaType(linha)
            };

            return _contextExt.ExecuteTableValueProcedure(_linhas, "NAV2017LinGuiaTransporte_Update", "@LinhasGuia", "LinGuiaTransporteType");
        }

        public static CabecalhoGuiaTransporteSqlModel CastToCabecalhoType(GuiaTransporteNavViewModel guia)
        {
            DateTime emptyDateTime = DateTime.Parse("0001-01-01 00:00:00.000");
            string dateTimeFormatString = "yyyy-MM-dd HH:mm:ss.fff";


            if (guia != null)
            {
                CabecalhoGuiaTransporteSqlModel cabecalho = new CabecalhoGuiaTransporteSqlModel()
                {
                    NoGuiaTransporte = guia.NoGuiaTransporte,
                    Address = guia.Address,
                    Cidade = guia.Cidade,
                    City = guia.City,
                    CodEnvio = guia.CodEnvio,
                    CodPais = guia.CodPais,
                    CodPostal = guia.CodPostal,
                    CodPostalDescarga = guia.CodPostalDescarga,
                    DataCarga = guia.DataCarga.CompareTo(emptyDateTime) != 0 ? guia.DataCarga.ToString(dateTimeFormatString) : null,
                    DataDescarga = guia.DataDescarga.CompareTo(emptyDateTime) != 0 ? guia.DataDescarga.ToString(dateTimeFormatString) : null,
                    DataGuia = guia.DataGuia.CompareTo(emptyDateTime) != 0 ? guia.DataGuia.ToString(dateTimeFormatString) : null,
                    DataObservacoesAdicionais = guia.DataObservacoesAdicionais.CompareTo(emptyDateTime) != 0 ? guia.DataObservacoesAdicionais.ToString(dateTimeFormatString) : null,
                    DataSaida = guia.DataSaida.CompareTo(emptyDateTime) != 0 ? guia.DataSaida.ToString(dateTimeFormatString) : null,
                    DimensionSetId = guia.DimensionSetId,
                    GlobalDimension1Code = guia.GlobalDimension1Code,
                    GlobalDimension2Code = guia.GlobalDimension2Code,
                    GlobalDimension3Code = guia.GlobalDimension3Code,
                    GuiaTransporteInterface = guia.GuiaTransporteInterface,
                    Historico = guia.Historico,
                    HoraCarga = guia.HoraCarga.ToString(),
                    HoraDescarga = guia.HoraDescarga.ToString(),
                    HoraObservacoesAdicionais = guia.HoraObservacoesAdicionais.ToString(),
                    LocalDescarga = guia.LocalDescarga,
                    LocalDescarga1 = guia.LocalDescarga1,
                    MaintenanceOrderNo = guia.MaintenanceOrderNo,
                    MoradaCliente = guia.MoradaCliente,
                    MoradaCliente2 = guia.MoradaCliente2,
                    Name = guia.Name,
                    NifCliente = guia.NifCliente,
                    NoCliente = guia.NoCliente,
                    NoGuiaOriginalInterface = guia.NoGuiaOriginalInterface,
                    NomeCliente = guia.NomeCliente,
                    NomeCliente2 = guia.NomeCliente2,
                    NoProjecto = guia.NoProjecto,
                    NoRequisicao = guia.NoRequisicao,
                    NoSolicitacao = guia.NoSolicitacao,
                    Observacoes = guia.Observacoes,
                    ObservacoesAdicionais = guia.ObservacoesAdicionais,
                    OrdemTransferencia = guia.OrdemTransferencia,
                    Origem = guia.Origem,
                    PaisCarga = guia.PaisCarga,
                    PaisDescarga = guia.PaisDescarga,
                    PesoTotal = guia.PesoTotal,
                    PostCode = guia.PostCode,
                    QuantidadeTotal = guia.QuantidadeTotal,
                    ReportedBy = guia.ReportedBy,
                    Requisicao = guia.Requisicao,
                    ResponsabilityCenter = guia.ResponsabilityCenter,
                    ShipmentStartDate = guia.ShipmentStartDate.CompareTo(emptyDateTime) != 0 ? guia.ShipmentStartDate.ToString(dateTimeFormatString) : null,
                    ShipmentStartTime = guia.ShipmentStartTime.ToString(),
                    SourceCode = guia.SourceCode,
                    Telefone = guia.Telefone,
                    Tipo = guia.Tipo,
                    TipoDescription = guia.TipoDescription,
                    UserObservacoesAdicionai = guia.UserObservacoesAdicionai,
                    Utilizador = guia.Utilizador,
                    VATRegistrationNo = guia.VATRegistrationNo,
                    Viatura = guia.Viatura
                };

                return cabecalho;
            }

            return null;
        }

        public static LinhaGuiaTransporteSqlModel CastToLinhaType(LinhaGuiaTransporteNavViewModel linha)
        {
            DateTime emptyDateTime = DateTime.Parse("0001-01-01 00:00:00.000");
            string dateTimeFormatString = "yyyy-MM-dd HH:mm:ss.fff";

            if (linha != null)
            {
                LinhaGuiaTransporteSqlModel linhaType = new LinhaGuiaTransporteSqlModel()
                {
                    NoGuiaTransporte = linha.NoGuiaTransporte,
                    NoLinha = linha.NoLinha,
                    Acessories = linha.Acessories,
                    CodUnidadeMedida = linha.CodUnidadeMedida,
                    Correction = linha.Correction,
                    DataEntrega = linha.DataEntrega.CompareTo(emptyDateTime) != 0 ? linha.DataEntrega.ToString(dateTimeFormatString) : null,
                    DataGuia = linha.DataGuia.CompareTo(emptyDateTime) != 0 ? linha.DataGuia.ToString(dateTimeFormatString) : null,
                    Descricao = linha.Descricao,
                    DimensionSetID = linha.DimensionSetID,
                    EstadoEquipamento = linha.EstadoEquipamento,
                    EstadoEquipamentoDescription = linha.EstadoEquipamentoDescription,
                    FLDescription = linha.FlDescription,
                    FunctionalLocationNo = linha.FunctionalLocationNo,
                    InventoryNo = linha.InventoryNo,
                    MaintOrderNo = linha.MaintOrderNo,
                    NoCliente = linha.NoCliente,
                    No = linha.No,
                    NoMovimentoJobLedgerEntry = linha.NoMovimentoJobLedgerEntry,
                    NoProjecto = linha.NoProjecto,
                    Quantidade = linha.Quantidade,
                    QuantidadeEnviar = linha.QuantidadeEnviar,
                    RefDocOrigem = linha.RefDocOrigem,
                    ShortcutDimension1Code = linha.ShortcutDimension1Code,
                    ShortcutDimension2Code = linha.ShortcutDimension2Code,
                    ShortcutDimension3Code = linha.ShortcutDimension3Code,
                    Tipo = linha.Tipo,
                    TipoDescription = linha.TipoDescription,
                    TipoTerceiro = linha.TipoTerceiro,
                    TotalCost = linha.TotalCost,
                    TotalPrice = linha.TotalPrice,
                    UnitCost = linha.UnitCost,
                    UnitPrice = linha.UnitPrice
                };

                return linhaType;
            }

            return null;
        }
    }
}
