using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Contracts
{
    public static class DBContracts
    {
        #region CRUD
        public static List<Contratos> GetByNo(string ContractNo, bool Archived)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Contratos.Where(x => x.NºContrato == ContractNo && x.Arquivado == Archived).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Contratos GetByIdAndVersion(string ContractNo, int VersionNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Contratos.Where(x => x.NºContrato == ContractNo && x.NºVersão == VersionNo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Contratos GetByIdLastVersion(string ContractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Contratos.Where(x => x.NºContrato == ContractNo).OrderByDescending(x => x.NºVersão).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<Contratos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Contratos.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Contratos Create(Contratos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.Contratos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool DeleteByContractNo(string ContractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Contratos.RemoveRange(ctx.Contratos.Where(x => x.NºContrato == ContractNo));
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static Contratos Update(Contratos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.Contratos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Contratos GetActiveContractById(string ContractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Contratos.Where(x => x.NºContrato == ContractNo && x.Arquivado == false).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Contratos> GetAllByContractNo(string ContractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Contratos.Where(x => x.NºContrato == ContractNo && x.Arquivado == false).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        public static List<Contratos> GetAllByAreaIdAndType(int AreaId, int ContractType)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Contratos.Where(x => x.Área == AreaId-1 && x.TipoContrato == ContractType).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<Contratos> GetAllFixedAndArquived(bool fixedRate, bool arquived)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Contratos.Where(x => x.ContratoAvençaFixa == fixedRate && x.Arquivado == arquived).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        public static Contratos ParseToDB(ContractViewModel x)
        {
            Contratos result = new Contratos()
            {
                TipoContrato = x.ContractType,
                NºContrato = x.ContractNo,
                NºDeContrato = x.ContractNo,
                NºVersão = x.VersionNo,
                Área = x.Area,
                Descrição = x.Description,
                Estado = x.Status - 1,
                EstadoAlteração = x.ChangeStatus - 1,
                NºCliente = x.ClientNo,
                CódigoRegião = x.CodeRegion,
                CódigoÁreaFuncional = x.CodeFunctionalArea,
                CódigoCentroResponsabilidade = x.CodeResponsabilityCenter,
                CódEndereçoEnvio = x.CodeShippingAddress,
                EnvioANome = x.ShippingName,
                EnvioAEndereço = x.ShippingAddress,
                EnvioACódPostal = x.ShippingZipCode,
                EnvioALocalidade = x.ShippingLocality,
                PeríodoFatura = x.InvocePeriod - 1,
                ÚltimaDataFatura = x.LastInvoiceDate != null ? DateTime.Parse(x.LastInvoiceDate) : (DateTime?)null,
                PróximaDataFatura = x.NextInvoiceDate != null ? DateTime.Parse(x.NextInvoiceDate) : (DateTime?)null,
                DataInicial = x.StartData != null ? DateTime.Parse(x.StartData) : (DateTime?)null,
                DataExpiração = x.DueDate != null ? DateTime.Parse(x.DueDate) : (DateTime?)null,
                JuntarFaturas = x.BatchInvoices,
                PróximoPeríodoFact = x.NextBillingPeriod,
                LinhasContratoEmFact = x.ContractLinesInBilling,
                CódTermosPagamento = x.CodePaymentTerms,
                TipoProposta = x.ProposalType,
                TipoFaturação = x.BillingType - 1,
                TipoContratoManut = x.MaintenanceContractType - 1,
                NºRequisiçãoDoCliente = x.ClientRequisitionNo,
                DataReceçãoRequisição = x.ReceiptDateRequisition != null ? DateTime.Parse(x.ReceiptDateRequisition) : (DateTime?)null,
                NºCompromisso = x.PromiseNo,
                TaxaAprovisionamento = x.ProvisioningFee,
                Mc=x.Mc,
                TaxaDeslocação = x.DisplacementFee,
                ContratoAvençaFixa = x.FixedVowsAgreement,
                ObjetoServiço = x.ServiceObject,
                ContratoAvençaVariável = x.VariableAvengeAgrement,
                Notas = x.Notes,
                DataInícioContrato = x.ContractStartDate != null ? DateTime.Parse(x.ContractStartDate) : (DateTime?)null,
                DataFimContrato = x.ContractEndDate != null ? DateTime.Parse(x.ContractEndDate) : (DateTime?)null,
                DescriçãoDuraçãoContrato = x.ContractDurationDescription,
                DataInício1ºContrato = x.StartDateFirstContract != null ? DateTime.Parse(x.StartDateFirstContract) : (DateTime?)null,
                Referência1ºContrato = x.FirstContractReference,
                DuraçãoMáxContrato = x.ContractMaxDuration != null ? DateTime.Parse(x.ContractMaxDuration) : (DateTime?)null,
                RescisãoPrazoAviso = x.TerminationTermNotice,
                CondiçõesParaRenovação =x.RenovationConditions,
                CondiçõesRenovaçãoOutra = x.RenovationConditionsAnother,
                CondiçõesPagamento = x.PaymentTerms,
                CondiçõesPagamentoOutra = x.PaymentTermsAnother,
                AssinadoPeloCliente = x.CustomerSigned,
                Juros = x.Interests,
                DataDaAssinatura = x.SignatureDate != null ? DateTime.Parse(x.SignatureDate) : (DateTime?)null,
                DataEnvioCliente = x.CustomerShipmentDate != null ? DateTime.Parse(x.CustomerShipmentDate) : (DateTime?)null,
                UnidadePrestação = x.ProvisionUnit,
                ReferênciaContrato  = x.ContractReference,
                ValorTotalProposta = x.TotalProposalValue,
                LocalArquivoFísico = x.PhysicalFileLocation,
                NºOportunidade = x.OportunityNo,
                NºProposta = x.ProposalNo,
                NºContato = x.ContactNo,
                DataEstadoProposta = x.DateProposedState != null ? DateTime.Parse(x.DateProposedState) : (DateTime?)null,
                OrigemDoPedido = x.OrderOrigin,
                DescOrigemDoPedido = x.OrdOrderSource,
                NumeraçãoInterna = x.InternalNumeration,
                DataAlteraçãoProposta = x.ProposalChangeDate != null ? DateTime.Parse(x.ProposalChangeDate) : (DateTime?)null,
                DataHoraLimiteEsclarecimentos = x.LimitClarificationDate != null ? DateTime.Parse(x.LimitClarificationDate) : (DateTime?)null,
                DataHoraErrosEOmissões = x.ErrorsOmissionsDate != null ? DateTime.Parse(x.ErrorsOmissionsDate) : (DateTime?)null,
                DataHoraRelatórioFinal = x.FinalReportDate != null ? DateTime.Parse(x.FinalReportDate) : (DateTime?)null, 
                DataHoraHabilitaçãoDocumental = x.DocumentationHabilitationDate != null ? DateTime.Parse(x.DocumentationHabilitationDate) : (DateTime?)null,
                NºComprimissoObrigatório = x.CompulsoryCompulsoryNo,
                DataHoraCriação = x.CreateDate != null ? DateTime.Parse(x.CreateDate) : (DateTime?)null,
                DataHoraModificação = x.UpdateDate != null ? DateTime.Parse(x.UpdateDate) : (DateTime?)null,
                UtilizadorCriação = x.CreateUser,
                UtilizadorModificação = x.UpdateUser,
                Arquivado = x.Filed
            };



            return result;

        }

        public static ContractViewModel ParseToViewModel(Contratos x, string NAVDatabaseName, string NAVCompanyName)
        {
            ContractViewModel result = new ContractViewModel()
            {
                ContractType = x.TipoContrato,
                ContractNo = x.NºDeContrato,
                VersionNo = x.NºVersão,
                Area = x.Área,
                Description = x.Descrição,
                Status = x.Estado + 1,
                ChangeStatus = x.EstadoAlteração + 1,
                ClientNo = x.NºCliente,
                CodeRegion = x.CódigoRegião,
                CodeFunctionalArea = x.CódigoÁreaFuncional,
                CodeResponsabilityCenter = x.CódigoCentroResponsabilidade,
                CodeShippingAddress = x.CódEndereçoEnvio,
                ShippingName = x.EnvioANome,
                ShippingAddress = x.EnvioAEndereço,
                ShippingZipCode = x.EnvioACódPostal,
                ShippingLocality = x.EnvioALocalidade,
                InvocePeriod = x.PeríodoFatura + 1,
                LastInvoiceDate = x.ÚltimaDataFatura.HasValue ? x.ÚltimaDataFatura.Value.ToString("yyyy-MM-dd") : "",
                NextInvoiceDate = x.PróximaDataFatura.HasValue ? x.PróximaDataFatura.Value.ToString("yyyy-MM-dd") : "",
                StartData = x.DataInicial.HasValue ? x.DataInicial.Value.ToString("yyyy-MM-dd") : "",
                DueDate = x.DataExpiração.HasValue ? x.DataExpiração.Value.ToString("yyyy-MM-dd") : "",
                BatchInvoices = x.JuntarFaturas,
                NextBillingPeriod = x.PróximoPeríodoFact,
                ContractLinesInBilling = x.LinhasContratoEmFact,
                CodePaymentTerms = x.CódTermosPagamento,
                ProposalType = x.TipoProposta,
                BillingType = x.TipoFaturação + 1,
                MaintenanceContractType = x.TipoContratoManut + 1,
                ClientRequisitionNo = x.NºRequisiçãoDoCliente,
                ReceiptDateRequisition = x.DataReceçãoRequisição.HasValue ? x.DataReceçãoRequisição.Value.ToString("yyyy-MM-dd") : "",
                PromiseNo = x.NºCompromisso,
                ProvisioningFee = x.TaxaAprovisionamento,
                Mc = x.Mc,
                DisplacementFee = x.TaxaDeslocação,
                FixedVowsAgreement = x.ContratoAvençaFixa,
                ServiceObject = x.ObjetoServiço,
                VariableAvengeAgrement = x.ContratoAvençaVariável,
                Notes = x.Notas,
                ContractStartDate = x.DataInícioContrato.HasValue ? x.DataInícioContrato.Value.ToString("yyyy-MM-dd") : "",
                ContractEndDate = x.DataFimContrato.HasValue ? x.DataFimContrato.Value.ToString("yyyy-MM-dd") : "",
                ContractDurationDescription = x.DescriçãoDuraçãoContrato,
                StartDateFirstContract = x.DataInício1ºContrato.HasValue ? x.DataInício1ºContrato.Value.ToString("yyyy-MM-dd") : "",
                FirstContractReference = x.Referência1ºContrato,
                ContractMaxDuration = x.DuraçãoMáxContrato.HasValue ? x.DuraçãoMáxContrato.Value.ToString("yyyy-MM-dd") : "",
                TerminationTermNotice = x.RescisãoPrazoAviso,
                RenovationConditions = x.CondiçõesParaRenovação,
                RenovationConditionsAnother = x.CondiçõesRenovaçãoOutra,
                PaymentTerms = x.CondiçõesPagamento,
                PaymentTermsAnother = x.CondiçõesPagamentoOutra,
                CustomerSigned = x.AssinadoPeloCliente,
                Interests = x.Juros,
                SignatureDate = x.DataDaAssinatura.HasValue ? x.DataDaAssinatura.Value.ToString("yyyy-MM-dd") : "",
                CustomerShipmentDate = x.DataEnvioCliente.HasValue ? x.DataEnvioCliente.Value.ToString("yyyy-MM-dd") : "",
                ProvisionUnit = x.UnidadePrestação,
                ContractReference = x.ReferênciaContrato,
                TotalProposalValue = x.ValorTotalProposta,
                PhysicalFileLocation = x.LocalArquivoFísico,
                OportunityNo = x.NºOportunidade,
                ProposalNo = x.NºProposta,
                ContactNo = x.NºContato,
                DateProposedState = x.DataEstadoProposta.HasValue ? x.DataEstadoProposta.Value.ToString("yyyy-MM-dd") : "",
                OrderOrigin = x.OrigemDoPedido,
                OrdOrderSource = x.DescOrigemDoPedido,
                InternalNumeration = x.NumeraçãoInterna,
                ProposalChangeDate = x.DataAlteraçãoProposta.HasValue ? x.DataAlteraçãoProposta.Value.ToString("yyyy-MM-dd") : "",
                LimitClarificationDate = x.DataHoraLimiteEsclarecimentos.HasValue ? x.DataHoraLimiteEsclarecimentos.Value.ToString("yyyy-MM-dd") : "",
                ErrorsOmissionsDate = x.DataHoraErrosEOmissões.HasValue ? x.DataHoraErrosEOmissões.Value.ToString("yyyy-MM-dd") : "",
                FinalReportDate = x.DataHoraRelatórioFinal.HasValue ? x.DataHoraRelatórioFinal.Value.ToString("yyyy-MM-dd") : "",
                DocumentationHabilitationDate = x.DataHoraHabilitaçãoDocumental.HasValue ? x.DataHoraHabilitaçãoDocumental.Value.ToString("yyyy-MM-dd") : "",
                CompulsoryCompulsoryNo = x.NºComprimissoObrigatório,
                CreateDate = x.DataHoraCriação.HasValue ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "",
                UpdateDate = x.DataHoraModificação.HasValue ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "",
                CreateUser = x.UtilizadorCriação,
                UpdateUser = x.UtilizadorModificação,
                Filed = x.Arquivado
            };

            result.ClientName = DBNAV2017Clients.GetClientNameByNo(x.NºCliente, NAVDatabaseName, NAVCompanyName);

            return result;

        }

    }
}
