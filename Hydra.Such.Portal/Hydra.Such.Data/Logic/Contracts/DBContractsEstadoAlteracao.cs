﻿using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Extensions;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.Logic.Contracts
{
    public static class DBContractsEstadoAlteracao
    {
        #region CRUD
        public static List<ContratosEstadoAlteracao> GetByNo(string ContractNo, bool Archived)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ContratosEstadoAlteracao.Where(x => x.NºDeContrato == ContractNo && x.Arquivado == Archived).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ContratosEstadoAlteracao GetByIdAndVersion(string ContractNo, int VersionNo, ContractType? type = null)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    var query = ctx.ContratosEstadoAlteracao.Where(x => x.NºDeContrato == ContractNo && x.NºVersão == VersionNo);
                    if (type.HasValue)
                        query = query.Where(x => x.TipoContrato == (int)type.Value);

                    return query.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ContratosEstadoAlteracao GetByIdLastVersion(string ContractNo, ContractType? type = null)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    var query = ctx.ContratosEstadoAlteracao.Where(x => x.NºDeContrato == ContractNo);
                    if (type.HasValue)
                        query = query.Where(x => x.TipoContrato == (int)type.Value);

                    return query.OrderByDescending(x => x.NºVersão).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ContratosEstadoAlteracao> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ContratosEstadoAlteracao.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ContratosEstadoAlteracao> GetAllTESTE(string user, ContractType contractType, int Type)
        {
            try
            {
                List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(user);

                using (var ctx = new SuchDBContext())
                {
                    List<ContratosEstadoAlteracao> ListContratosEstadoAlteracao = ctx.ContratosEstadoAlteracao.Where(x => x.TipoContrato == (int)contractType && x.Tipo == Type).ToList();

                    if (userDimensions != null && userDimensions.Count > 0)
                    {
                        List<LinhasContratosEstadoAlteracao> ListLinhasContratosEstadoAlteracao = ctx.LinhasContratosEstadoAlteracao.Where(x => (userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião) ||
                            userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CódigoÁreaFuncional) ||
                            userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade)) &&
                            x.TipoContrato == (int)contractType && x.Tipo == 2).Distinct().ToList();

                        ListContratosEstadoAlteracao.RemoveAll(x => !ListLinhasContratosEstadoAlteracao.Any(y => y.NºContrato == x.NºDeContrato));
                    }

                    return ListContratosEstadoAlteracao;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        public static ContratosEstadoAlteracao GetActualContract(string ContractNo, string ClientNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ContratosEstadoAlteracao.Where(x => x.TipoContrato == 3 &&
                    x.NºDeContrato == ContractNo && x.NºCliente == ClientNo).
                    OrderByDescending(x => x.NºVersão).
                    FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ContratosEstadoAlteracao Create(ContratosEstadoAlteracao ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.ContratosEstadoAlteracao.Add(ObjectToCreate);
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
                    ctx.ContratosEstadoAlteracao.RemoveRange(ctx.ContratosEstadoAlteracao.Where(x => x.NºDeContrato == ContractNo));
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static ContratosEstadoAlteracao Update(ContratosEstadoAlteracao ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.ContratosEstadoAlteracao.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ContratosEstadoAlteracao> GetAllByContractNo(string ContractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ContratosEstadoAlteracao.Where(x => x.NºDeContrato == ContractNo && x.Arquivado == false).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ContratosEstadoAlteracao> GetAllByContractProposalsNo(string ContractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ContratosEstadoAlteracao.Where(x => x.NºContrato == ContractNo && x.TipoContrato == 2).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static ContratosEstadoAlteracao GetContractProposalsNo(string ContractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ContratosEstadoAlteracao.Where(x => x.NºContrato == ContractNo && x.TipoContrato == 2).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        public static List<ContratosEstadoAlteracao> GetAllByContractType(ContractType contractType)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {

                    return ctx.ContratosEstadoAlteracao.Where(x => x.TipoContrato == (int)contractType).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ContratosEstadoAlteracao> GetAllListContractByContractTypeAndType(List<AcessosDimensões> userDimensions, int contractType, int Type)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<ContratosEstadoAlteracao> ListContratosEstadoAlteracao = ctx.ContratosEstadoAlteracao.Where(x => x.TipoContrato == contractType && x.Tipo == Type).ToList();

                    if (userDimensions != null && userDimensions.Count > 0)
                    {
                        List<LinhasContratosEstadoAlteracao> ListLinhasContratosEstadoAlteracao = ctx.LinhasContratosEstadoAlteracao.Where(x => x.TipoContrato == (int)contractType && x.Tipo == 2).Distinct().ToList();

                        //Regions
                        if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                            ListLinhasContratosEstadoAlteracao.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));

                        //FunctionalAreas
                        if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                            ListLinhasContratosEstadoAlteracao.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CódigoÁreaFuncional));

                        //ResponsabilityCenter
                        if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                            ListLinhasContratosEstadoAlteracao.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));


                        ListContratosEstadoAlteracao.RemoveAll(x => !ListLinhasContratosEstadoAlteracao.Any(y => y.NºContrato == x.NºDeContrato));
                    }

                    return ListContratosEstadoAlteracao;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ContratosEstadoAlteracao> GetAllByContractTypeAndType(ContractType contractType, int Type)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {

                    return ctx.ContratosEstadoAlteracao.Where(x => x.TipoContrato == (int)contractType && x.Tipo == Type).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ContratosEstadoAlteracao> GetAllListContractHistoric(List<AcessosDimensões> userDimensions, int contractType)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<ContratosEstadoAlteracao> ListContratosEstadoAlteracao = ctx.ContratosEstadoAlteracao.Where(x => x.TipoContrato == contractType && x.Historico == true).ToList();

                    if (userDimensions != null && userDimensions.Count > 0)
                    {
                        List<LinhasContratosEstadoAlteracao> ListLinhasContratosEstadoAlteracao = ctx.LinhasContratosEstadoAlteracao.Where(x => (
                            userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CódigoÁreaFuncional) ||
                            userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade)) &&
                            x.TipoContrato == (int)contractType && x.Tipo == 2).Distinct().ToList();

                        ListContratosEstadoAlteracao.RemoveAll(x => !ListLinhasContratosEstadoAlteracao.Any(y => y.NºContrato == x.NºDeContrato));
                    }

                    return ListContratosEstadoAlteracao;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ContratosEstadoAlteracao> GetAllHistoric(int ContractType)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ContratosEstadoAlteracao.Where(x => x.TipoContrato == ContractType && x.Historico == true).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ContratosEstadoAlteracao GetByIdAvencaFixa(string ContractNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ContratosEstadoAlteracao
                        .Where(x => x.NºDeContrato == ContractNo &&
                            x.ContratoAvençaFixa == true &&
                            //x.Arquivado == false
                            x.Arquivado != true)
                        .OrderByDescending(x => x.NºVersão)
                        .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ContratosEstadoAlteracao> GetAllAvencaFixa()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ContratosEstadoAlteracao.Where(x =>
                    x.ContratoAvençaFixa == true &&
                    x.Arquivado != true &&
                    x.Estado == 4 && // Assinado 
                    x.EstadoAlteração == 2 && // Bloqueado
                    (x.TipoFaturação == 1 || x.TipoFaturação == 4)).ToList(); // Mensal / Mensal + Consumo
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<ContratosEstadoAlteracao> GetAllAvencaFixa2()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ContratosEstadoAlteracao.Where(x =>
                    x.ContratoAvençaFixa == true &&
                    x.Arquivado != true &&
                    x.TipoContrato == 3).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        public static ContratosEstadoAlteracao ParseToDB(ContractEstadoAlteracaoViewModel x)
        {

            ContratosEstadoAlteracao result = new ContratosEstadoAlteracao()
            {
                TipoContrato = x.ContractType,
                NºContrato = x.RelatedContract,
                NºDeContrato = x.ContractNo,
                NºVersão = x.VersionNo,
                Área = x.Area,
                Descrição = x.Description,
                Estado = x.Status,
                EstadoAnterior = x.OldStatus,
                EstadoAlteração = x.ChangeStatus,
                NºCliente = x.ClientNo,
                CódigoRegião = x.CodeRegion,
                CódigoÁreaFuncional = x.CodeFunctionalArea,
                CódigoCentroResponsabilidade = x.CodeResponsabilityCenter,
                CódEndereçoEnvio = x.CodeShippingAddress,
                EnvioANome = x.ShippingName,
                EnvioANome2 = x.ShippingName2,
                EnvioAEndereço = x.ShippingAddress,
                EnvioAEndereço2 = x.ShippingAddress2,
                EnvioACódPostal = x.ShippingZipCode,
                EnvioALocalidade = x.ShippingLocality,
                PeríodoFatura = x.InvocePeriod,
                ÚltimaDataFatura = string.IsNullOrEmpty(x.LastInvoiceDate) ? (DateTime?)null : DateTime.Parse(x.LastInvoiceDate),
                PróximaDataFatura = string.IsNullOrEmpty(x.NextInvoiceDate) ? (DateTime?)null : DateTime.Parse(x.NextInvoiceDate),
                DataInicial = string.IsNullOrEmpty(x.StartData) ? (DateTime?)null : DateTime.Parse(x.StartData),
                DataExpiração = string.IsNullOrEmpty(x.DueDate) ? (DateTime?)null : DateTime.Parse(x.DueDate),
                JuntarFaturas = x.BatchInvoices,
                PróximoPeríodoFact = x.NextBillingPeriod,
                LinhasContratoEmFact = x.ContractLinesInBilling,
                CódTermosPagamento = x.CodePaymentTerms,
                CódFormaPagamento = x.CodePaymentMethod,
                TipoProposta = x.ProposalType,
                TipoFaturação = x.BillingType,
                TipoContratoManut = x.MaintenanceContractType,
                NºRequisiçãoDoCliente = x.ClientRequisitionNo,
                DataReceçãoRequisição = string.IsNullOrEmpty(x.ReceiptDateRequisition) ? (DateTime?)null : DateTime.Parse(x.ReceiptDateRequisition),
                NºCompromisso = x.PromiseNo,
                TaxaAprovisionamento = x.ProvisioningFee,
                Mc = x.Mc,
                TaxaDeslocação = x.DisplacementFee,
                ContratoAvençaFixa = x.FixedVowsAgreement,
                ObjetoServiço = x.ServiceObject,
                ContratoAvençaVariável = x.VariableAvengeAgrement,
                Notas = x.Notes,
                DataInícioContrato = string.IsNullOrEmpty(x.ContractStartDate) ? (DateTime?)null : DateTime.Parse(x.ContractStartDate),
                DataFimContrato = string.IsNullOrEmpty(x.ContractEndDate) ? (DateTime?)null : DateTime.Parse(x.ContractEndDate),
                DescriçãoDuraçãoContrato = x.ContractDurationDescription,
                DataInício1ºContrato = string.IsNullOrEmpty(x.StartDateFirstContract) ? (DateTime?)null : DateTime.Parse(x.StartDateFirstContract),
                Referência1ºContrato = x.FirstContractReference,
                DuraçãoMáxContrato = string.IsNullOrEmpty(x.ContractMaxDuration) ? (DateTime?)null : DateTime.Parse(x.ContractMaxDuration),
                RescisãoPrazoAviso = x.TerminationTermNotice,
                CondiçõesParaRenovação = x.RenovationConditions,
                CondiçõesRenovaçãoOutra = x.RenovationConditionsAnother,
                CondiçõesPagamento = x.PaymentTerms,
                CondiçõesPagamentoOutra = x.PaymentTermsAnother,
                AssinadoPeloCliente = x.CustomerSigned,
                Juros = x.Interests,
                DataDaAssinatura = string.IsNullOrEmpty(x.SignatureDate) ? (DateTime?)null : DateTime.Parse(x.SignatureDate),
                DataEnvioCliente = string.IsNullOrEmpty(x.CustomerShipmentDate) ? (DateTime?)null : DateTime.Parse(x.CustomerShipmentDate),
                UnidadePrestação = x.ProvisionUnit,
                ReferênciaContrato = x.ContractReference,
                ValorTotalProposta = x.TotalProposalValue,
                LocalArquivoFísico = x.PhysicalFileLocation,
                NºOportunidade = x.OportunityNo,
                NºProposta = x.ProposalNo,
                NºContato = x.ContactNo,
                DataEstadoProposta = string.IsNullOrEmpty(x.DateProposedState) ? (DateTime?)null : DateTime.Parse(x.DateProposedState),
                OrigemDoPedido = x.OrderOrigin,
                DescOrigemDoPedido = x.OrdOrderSource,
                NumeraçãoInterna = x.InternalNumeration,
                DataAlteraçãoProposta = string.IsNullOrEmpty(x.ProposalChangeDate) ? (DateTime?)null : DateTime.Parse(x.ProposalChangeDate),
                DataHoraLimiteEsclarecimentos = string.IsNullOrEmpty(x.LimitClarificationDate) ? (DateTime?)null : DateTime.Parse(x.LimitClarificationDate),
                DataHoraErrosEOmissões = string.IsNullOrEmpty(x.ErrorsOmissionsDate) ? (DateTime?)null : DateTime.Parse(x.ErrorsOmissionsDate),
                DataHoraRelatórioFinal = string.IsNullOrEmpty(x.FinalReportDate) ? (DateTime?)null : DateTime.Parse(x.FinalReportDate),
                DataHoraHabilitaçãoDocumental = string.IsNullOrEmpty(x.DocumentationHabilitationDate) ? (DateTime?)null : DateTime.Parse(x.DocumentationHabilitationDate),
                DataHoraEntregaProposta = string.IsNullOrEmpty(x.ProposalDelivery) ? (DateTime?)null : DateTime.Parse(x.ProposalDelivery),
                NºComprimissoObrigatório = x.CompulsoryCompulsoryNo,
                DataHoraCriação = string.IsNullOrEmpty(x.CreateDate) ? (DateTime?)null : DateTime.Parse(x.CreateDate),
                DataHoraModificação = string.IsNullOrEmpty(x.UpdateDate) ? (DateTime?)null : DateTime.Parse(x.UpdateDate),
                UtilizadorCriação = x.CreateUser,
                UtilizadorModificação = x.UpdateUser,
                Arquivado = x.Filed,
                RazãoArquivo = x.ArchiveReason,
                ValorBaseProcedimento = x.BaseValueProcedure,
                AudiênciaPrévia = string.IsNullOrEmpty(x.PreviousHearing) ? (DateTime?)null : DateTime.Parse(x.PreviousHearing),
                Historico = x.History,
                Tipo = x.Type,
                NºVep = x.NoVEP,
                TextoFatura = x.TextoFatura,
                FaturaPrecosIvaIncluido = x.FaturaPrecosIvaIncluido,
                SomatorioLinhas = x.SomatorioLinhas
            };

            if (result.DataHoraLimiteEsclarecimentos != null)
            {
                result.DataHoraLimiteEsclarecimentos = result.DataHoraLimiteEsclarecimentos.Value.Date;
                if (!string.IsNullOrEmpty(x.LimitClarificationTime))
                    result.DataHoraLimiteEsclarecimentos = result.DataHoraLimiteEsclarecimentos.Value.Add(TimeSpan.Parse(x.LimitClarificationTime));
                Console.WriteLine(result.DataHoraLimiteEsclarecimentos.Value.ToString());
            }

            if (result.DataHoraErrosEOmissões != null)
            {
                result.DataHoraErrosEOmissões = result.DataHoraErrosEOmissões.Value.Date;
                if (!string.IsNullOrEmpty(x.ErrorsOmissionsTime))
                    result.DataHoraErrosEOmissões = result.DataHoraErrosEOmissões.Value.Add(TimeSpan.Parse(x.ErrorsOmissionsTime));
                Console.WriteLine(result.DataHoraErrosEOmissões.Value.ToString());
            }

            if (result.DataHoraRelatórioFinal != null)
            {
                result.DataHoraRelatórioFinal = result.DataHoraRelatórioFinal.Value.Date;
                if (!string.IsNullOrEmpty(x.FinalReportTime))
                    result.DataHoraRelatórioFinal = result.DataHoraRelatórioFinal.Value.Add(TimeSpan.Parse(x.FinalReportTime));
                Console.WriteLine(result.DataHoraRelatórioFinal.Value.ToString());
            }

            if (result.DataHoraHabilitaçãoDocumental != null)
            {
                result.DataHoraHabilitaçãoDocumental = result.DataHoraHabilitaçãoDocumental.Value.Date;
                if (!string.IsNullOrEmpty(x.DocumentationHabilitationTime))
                    result.DataHoraHabilitaçãoDocumental = result.DataHoraHabilitaçãoDocumental.Value.Add(TimeSpan.Parse(x.DocumentationHabilitationTime));
                Console.WriteLine(result.DataHoraHabilitaçãoDocumental.Value.ToString());
            }

            if (result.AudiênciaPrévia != null)
            {
                result.AudiênciaPrévia = result.AudiênciaPrévia.Value.Date;
                if (!string.IsNullOrEmpty(x.PreviousHearingTime))
                    result.AudiênciaPrévia = result.AudiênciaPrévia.Value.Add(TimeSpan.Parse(x.PreviousHearingTime));
                Console.WriteLine(result.AudiênciaPrévia.Value.ToString());
            }

            if (result.DataHoraEntregaProposta != null)
            {
                result.DataHoraEntregaProposta = result.DataHoraEntregaProposta.Value.Date;
                if (!string.IsNullOrEmpty(x.ProposalDeliveryTime))
                    result.DataHoraEntregaProposta = result.DataHoraEntregaProposta.Value.Add(TimeSpan.Parse(x.ProposalDeliveryTime));
            }

            return result;

        }

        public static ContractEstadoAlteracaoViewModel ParseToViewModel(ContratosEstadoAlteracao x, string NAVDatabaseName, string NAVCompanyName)
        {
            if (x == null)
                return null;
            ContractEstadoAlteracaoViewModel result = new ContractEstadoAlteracaoViewModel()
            {
                ContractType = x.TipoContrato,
                ContractNo = x.NºDeContrato,
                VersionNo = x.NºVersão,
                Area = x.Área,
                Description = x.Descrição,
                Status = x.Estado,
                OldStatus = x.EstadoAnterior,
                ChangeStatus = x.EstadoAlteração,
                ClientNo = x.NºCliente,
                CodeRegion = x.CódigoRegião,
                CodeFunctionalArea = x.CódigoÁreaFuncional,
                CodeResponsabilityCenter = x.CódigoCentroResponsabilidade,
                CodeShippingAddress = x.CódEndereçoEnvio,
                ShippingName = x.EnvioANome,
                ShippingName2 = x.EnvioANome2,
                ShippingAddress = x.EnvioAEndereço,
                ShippingAddress2 = x.EnvioAEndereço2,
                ShippingZipCode = x.EnvioACódPostal,
                ShippingLocality = x.EnvioALocalidade,
                InvocePeriod = x.PeríodoFatura,
                LastInvoiceDate = x.ÚltimaDataFatura.HasValue ? x.ÚltimaDataFatura.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.ÚltimaDataFatura.Value.ToString("yyyy-MM-dd") : "" : "",
                NextInvoiceDate = x.PróximaDataFatura.HasValue ? x.PróximaDataFatura.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.PróximaDataFatura.Value.ToString("yyyy-MM-dd") : "" : "",
                StartData = x.DataInicial.HasValue ? x.DataInicial.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataInicial.Value.ToString("yyyy-MM-dd") : "" : "",
                DueDate = x.DataExpiração.HasValue ? x.DataExpiração.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataExpiração.Value.ToString("yyyy-MM-dd") : "" : "",
                BatchInvoices = x.JuntarFaturas,
                NextBillingPeriod = x.PróximoPeríodoFact,
                ContractLinesInBilling = x.LinhasContratoEmFact,
                CodePaymentTerms = x.CódTermosPagamento,
                CodePaymentMethod = x.CódFormaPagamento,
                ProposalType = x.TipoProposta,
                BillingType = x.TipoFaturação,
                MaintenanceContractType = x.TipoContratoManut,
                ClientRequisitionNo = x.NºRequisiçãoDoCliente,
                ReceiptDateRequisition = x.DataReceçãoRequisição.HasValue ? x.DataReceçãoRequisição.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataReceçãoRequisição.Value.ToString("yyyy-MM-dd") : "" : "",
                PromiseNo = x.NºCompromisso,
                ProvisioningFee = x.TaxaAprovisionamento,
                Mc = x.Mc,
                DisplacementFee = x.TaxaDeslocação,
                FixedVowsAgreement = x.ContratoAvençaFixa,
                ServiceObject = x.ObjetoServiço,
                VariableAvengeAgrement = x.ContratoAvençaVariável,
                Notes = x.Notas,
                ContractStartDate = x.DataInícioContrato.HasValue ? x.DataInícioContrato.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataInícioContrato.Value.ToString("yyyy-MM-dd") : "" : "",
                ContractEndDate = x.DataFimContrato.HasValue ? x.DataFimContrato.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataFimContrato.Value.ToString("yyyy-MM-dd") : "" : "",
                ContractDurationDescription = x.DescriçãoDuraçãoContrato,
                StartDateFirstContract = x.DataInício1ºContrato.HasValue ? x.DataInício1ºContrato.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataInício1ºContrato.Value.ToString("yyyy-MM-dd") : "" : "",
                FirstContractReference = x.Referência1ºContrato,
                ContractMaxDuration = x.DuraçãoMáxContrato.HasValue ? x.DuraçãoMáxContrato.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DuraçãoMáxContrato.Value.ToString("yyyy-MM-dd") : "" : "",
                TerminationTermNotice = x.RescisãoPrazoAviso,
                RenovationConditions = x.CondiçõesParaRenovação,
                RenovationConditionsAnother = x.CondiçõesRenovaçãoOutra,
                PaymentTerms = x.CondiçõesPagamento,
                PaymentTermsAnother = x.CondiçõesPagamentoOutra,
                CustomerSigned = x.AssinadoPeloCliente,
                Interests = x.Juros,
                SignatureDate = x.DataDaAssinatura.HasValue ? x.DataDaAssinatura.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataDaAssinatura.Value.ToString("yyyy-MM-dd") : "" : "",
                CustomerShipmentDate = x.DataEnvioCliente.HasValue ? x.DataEnvioCliente.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataEnvioCliente.Value.ToString("yyyy-MM-dd") : "" : "",
                ProvisionUnit = x.UnidadePrestação,
                ContractReference = x.ReferênciaContrato,
                TotalProposalValue = x.ValorTotalProposta,
                PhysicalFileLocation = x.LocalArquivoFísico,
                OportunityNo = x.NºOportunidade,
                ProposalNo = x.NºProposta,
                ContactNo = x.NºContato,
                DateProposedState = x.DataEstadoProposta.HasValue ? x.DataEstadoProposta.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataEstadoProposta.Value.ToString("yyyy-MM-dd") : "" : "",
                OrderOrigin = x.OrigemDoPedido,
                OrdOrderSource = x.DescOrigemDoPedido,
                InternalNumeration = x.NumeraçãoInterna,
                ProposalChangeDate = x.DataHoraModificação.HasValue ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "" : "",
                ProposalChangeTime = x.DataHoraModificação.HasValue ? x.DataHoraModificação.Value.ToString("HH:mm") : "",
                LimitClarificationDate = x.DataHoraLimiteEsclarecimentos.HasValue ? x.DataHoraLimiteEsclarecimentos.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraLimiteEsclarecimentos.Value.ToString("yyyy-MM-dd") : "" : "",
                LimitClarificationTime = x.DataHoraLimiteEsclarecimentos.HasValue ? x.DataHoraLimiteEsclarecimentos.Value.ToString("HH:mm") : "",
                ErrorsOmissionsDate = x.DataHoraErrosEOmissões.HasValue ? x.DataHoraErrosEOmissões.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraErrosEOmissões.Value.ToString("yyyy-MM-dd") : "" : "",
                ErrorsOmissionsTime = x.DataHoraErrosEOmissões.HasValue ? x.DataHoraErrosEOmissões.Value.ToString("HH:mm") : "",
                FinalReportDate = x.DataHoraRelatórioFinal.HasValue ? x.DataHoraRelatórioFinal.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraRelatórioFinal.Value.ToString("yyyy-MM-dd") : "" : "",
                FinalReportTime = x.DataHoraRelatórioFinal.HasValue ? x.DataHoraRelatórioFinal.Value.ToString("HH:mm") : "",
                DocumentationHabilitationDate = x.DataHoraHabilitaçãoDocumental.HasValue ? x.DataHoraHabilitaçãoDocumental.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraHabilitaçãoDocumental.Value.ToString("yyyy-MM-dd") : "" : "",
                DocumentationHabilitationTime = x.DataHoraHabilitaçãoDocumental.HasValue ? x.DataHoraHabilitaçãoDocumental.Value.ToString("HH:mm") : "",
                CompulsoryCompulsoryNo = x.NºComprimissoObrigatório,
                CreateDate = x.DataHoraCriação.HasValue ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "" : "",
                UpdateDate = x.DataHoraModificação.HasValue ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "" : "",
                CreateUser = x.UtilizadorCriação,
                UpdateUser = x.UtilizadorModificação,
                Filed = x.Arquivado,
                ArchiveReason = x.RazãoArquivo,
                RelatedContract = x.NºContrato,
                BaseValueProcedure = x.ValorBaseProcedimento,
                PreviousHearing = x.AudiênciaPrévia.HasValue ? x.AudiênciaPrévia.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.AudiênciaPrévia.Value.ToString("yyyy-MM-dd") : "" : "",
                PreviousHearingTime = x.AudiênciaPrévia.HasValue ? x.AudiênciaPrévia.Value.ToString("HH:mm") : "",
                ProposalDelivery = x.DataHoraEntregaProposta.HasValue ? x.DataHoraEntregaProposta.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraEntregaProposta.Value.ToString("yyyy-MM-dd") : "" : "",
                ProposalDeliveryTime = x.DataHoraEntregaProposta.HasValue ? x.DataHoraEntregaProposta.Value.ToString("HH:mm") : "",
                History = x.Historico,
                Type = x.Tipo ?? 0,
                NoVEP = x.NºVep,
                TextoFatura = x.TextoFatura,
                FaturaPrecosIvaIncluido = x.FaturaPrecosIvaIncluido,
                SomatorioLinhas = x.SomatorioLinhas
            };
            if (x.NºCliente != null && x.NºCliente != "")
            {
                result.ClientName = DBNAV2017Clients.GetClientNameByNo(x.NºCliente, NAVDatabaseName, NAVCompanyName);
            }
            return result;

        }

        public static ContractEstadoAlteracaoViewModel ParseToViewModel(ContratosEstadoAlteracao x)
        {
            if (x == null)
                return null;
            ContractEstadoAlteracaoViewModel result = new ContractEstadoAlteracaoViewModel()
            {
                ContractType = x.TipoContrato,
                ContractNo = x.NºDeContrato,
                VersionNo = x.NºVersão,
                Area = x.Área,
                Description = x.Descrição,
                Status = x.Estado,
                OldStatus = x.EstadoAnterior,
                ChangeStatus = x.EstadoAlteração,
                ClientNo = x.NºCliente,
                CodeRegion = x.CódigoRegião,
                CodeFunctionalArea = x.CódigoÁreaFuncional,
                CodeResponsabilityCenter = x.CódigoCentroResponsabilidade,
                CodeShippingAddress = x.CódEndereçoEnvio,
                ShippingName = x.EnvioANome,
                ShippingName2 = x.EnvioANome2,
                ShippingAddress = x.EnvioAEndereço,
                ShippingAddress2 = x.EnvioAEndereço2,
                ShippingZipCode = x.EnvioACódPostal,
                ShippingLocality = x.EnvioALocalidade,
                InvocePeriod = x.PeríodoFatura,
                LastInvoiceDate = x.ÚltimaDataFatura.HasValue ? x.ÚltimaDataFatura.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.ÚltimaDataFatura.Value.ToString("yyyy-MM-dd") : "" : "",
                NextInvoiceDate = x.PróximaDataFatura.HasValue ? x.PróximaDataFatura.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.PróximaDataFatura.Value.ToString("yyyy-MM-dd") : "" : "",
                StartData = x.DataInicial.HasValue ? x.DataInicial.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataInicial.Value.ToString("yyyy-MM-dd") : "" : "",
                DueDate = x.DataExpiração.HasValue ? x.DataExpiração.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataExpiração.Value.ToString("yyyy-MM-dd") : "" : "",
                BatchInvoices = x.JuntarFaturas,
                NextBillingPeriod = x.PróximoPeríodoFact,
                ContractLinesInBilling = x.LinhasContratoEmFact,
                CodePaymentTerms = x.CódTermosPagamento,
                CodePaymentMethod = x.CódFormaPagamento,
                ProposalType = x.TipoProposta,
                BillingType = x.TipoFaturação,
                MaintenanceContractType = x.TipoContratoManut,
                ClientRequisitionNo = x.NºRequisiçãoDoCliente,
                ReceiptDateRequisition = x.DataReceçãoRequisição.HasValue ? x.DataReceçãoRequisição.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataReceçãoRequisição.Value.ToString("yyyy-MM-dd") : "" : "",
                PromiseNo = x.NºCompromisso,
                ProvisioningFee = x.TaxaAprovisionamento,
                Mc = x.Mc,
                DisplacementFee = x.TaxaDeslocação,
                FixedVowsAgreement = x.ContratoAvençaFixa,
                ServiceObject = x.ObjetoServiço,
                VariableAvengeAgrement = x.ContratoAvençaVariável,
                Notes = x.Notas,
                ContractStartDate = x.DataInícioContrato.HasValue ? x.DataInícioContrato.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataInícioContrato.Value.ToString("yyyy-MM-dd") : "" : "",
                ContractEndDate = x.DataFimContrato.HasValue ? x.DataFimContrato.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataFimContrato.Value.ToString("yyyy-MM-dd") : "" : "",
                ContractDurationDescription = x.DescriçãoDuraçãoContrato,
                StartDateFirstContract = x.DataInício1ºContrato.HasValue ? x.DataInício1ºContrato.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataInício1ºContrato.Value.ToString("yyyy-MM-dd") : "" : "",
                FirstContractReference = x.Referência1ºContrato,
                ContractMaxDuration = x.DuraçãoMáxContrato.HasValue ? x.DuraçãoMáxContrato.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DuraçãoMáxContrato.Value.ToString("yyyy-MM-dd") : "" : "",
                TerminationTermNotice = x.RescisãoPrazoAviso,
                RenovationConditions = x.CondiçõesParaRenovação,
                RenovationConditionsAnother = x.CondiçõesRenovaçãoOutra,
                PaymentTerms = x.CondiçõesPagamento,
                PaymentTermsAnother = x.CondiçõesPagamentoOutra,
                CustomerSigned = x.AssinadoPeloCliente,
                Interests = x.Juros,
                SignatureDate = x.DataDaAssinatura.HasValue ? x.DataDaAssinatura.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataDaAssinatura.Value.ToString("yyyy-MM-dd") : "" : "",
                CustomerShipmentDate = x.DataEnvioCliente.HasValue ? x.DataEnvioCliente.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataEnvioCliente.Value.ToString("yyyy-MM-dd") : "" : "",
                ProvisionUnit = x.UnidadePrestação,
                ContractReference = x.ReferênciaContrato,
                TotalProposalValue = x.ValorTotalProposta,
                PhysicalFileLocation = x.LocalArquivoFísico,
                OportunityNo = x.NºOportunidade,
                ProposalNo = x.NºProposta,
                ContactNo = x.NºContato,
                DateProposedState = x.DataEstadoProposta.HasValue ? x.DataEstadoProposta.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataEstadoProposta.Value.ToString("yyyy-MM-dd") : "" : "",
                OrderOrigin = x.OrigemDoPedido,
                OrdOrderSource = x.DescOrigemDoPedido,
                InternalNumeration = x.NumeraçãoInterna,
                ProposalChangeDate = x.DataHoraModificação.HasValue ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "" : "",
                ProposalChangeTime = x.DataHoraModificação.HasValue ? x.DataHoraModificação.Value.ToString("HH:mm") : "",
                LimitClarificationDate = x.DataHoraLimiteEsclarecimentos.HasValue ? x.DataHoraLimiteEsclarecimentos.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraLimiteEsclarecimentos.Value.ToString("yyyy-MM-dd") : "" : "",
                LimitClarificationTime = x.DataHoraLimiteEsclarecimentos.HasValue ? x.DataHoraLimiteEsclarecimentos.Value.ToString("HH:mm") : "",
                ErrorsOmissionsDate = x.DataHoraErrosEOmissões.HasValue ? x.DataHoraErrosEOmissões.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraErrosEOmissões.Value.ToString("yyyy-MM-dd") : "" : "",
                ErrorsOmissionsTime = x.DataHoraErrosEOmissões.HasValue ? x.DataHoraErrosEOmissões.Value.ToString("HH:mm") : "",
                FinalReportDate = x.DataHoraRelatórioFinal.HasValue ? x.DataHoraRelatórioFinal.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraRelatórioFinal.Value.ToString("yyyy-MM-dd") : "" : "",
                FinalReportTime = x.DataHoraRelatórioFinal.HasValue ? x.DataHoraRelatórioFinal.Value.ToString("HH:mm") : "",
                DocumentationHabilitationDate = x.DataHoraHabilitaçãoDocumental.HasValue ? x.DataHoraHabilitaçãoDocumental.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraHabilitaçãoDocumental.Value.ToString("yyyy-MM-dd") : "" : "",
                DocumentationHabilitationTime = x.DataHoraHabilitaçãoDocumental.HasValue ? x.DataHoraHabilitaçãoDocumental.Value.ToString("HH:mm") : "",
                CompulsoryCompulsoryNo = x.NºComprimissoObrigatório,
                CreateDate = x.DataHoraCriação.HasValue ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraCriação.Value.ToString("yyyy-MM-dd") : "" : "",
                UpdateDate = x.DataHoraModificação.HasValue ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraModificação.Value.ToString("yyyy-MM-dd") : "" : "",
                CreateUser = x.UtilizadorCriação,
                UpdateUser = x.UtilizadorModificação,
                Filed = x.Arquivado,
                ArchiveReason = x.RazãoArquivo,
                RelatedContract = x.NºContrato,
                BaseValueProcedure = x.ValorBaseProcedimento,
                PreviousHearing = x.AudiênciaPrévia.HasValue ? x.AudiênciaPrévia.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.AudiênciaPrévia.Value.ToString("yyyy-MM-dd") : "" : "",
                PreviousHearingTime = x.AudiênciaPrévia.HasValue ? x.AudiênciaPrévia.Value.ToString("HH:mm") : "",
                ProposalDelivery = x.DataHoraEntregaProposta.HasValue ? x.DataHoraEntregaProposta.Value.ToString("yyyy-MM-dd") != "1900-01-01" ? x.DataHoraEntregaProposta.Value.ToString("yyyy-MM-dd") : "" : "",
                ProposalDeliveryTime = x.DataHoraEntregaProposta.HasValue ? x.DataHoraEntregaProposta.Value.ToString("HH:mm") : "",
                History = x.Historico,
                Type = x.Tipo ?? 0,
                NoVEP = x.NºVep,
                TextoFatura = x.TextoFatura,
                FaturaPrecosIvaIncluido = x.FaturaPrecosIvaIncluido,
                SomatorioLinhas = x.SomatorioLinhas
            };

            return result;

        }

        public static ContratosEstadoAlteracao ParseToDB(ContractViewModel x)
        {

            ContratosEstadoAlteracao result = new ContratosEstadoAlteracao()
            {
                TipoContrato = x.ContractType,
                NºContrato = x.RelatedContract,
                NºDeContrato = x.ContractNo,
                NºVersão = x.VersionNo,
                Área = x.Area,
                Descrição = x.Description,
                Estado = x.Status,
                EstadoAnterior = x.OldStatus,
                EstadoAlteração = x.ChangeStatus,
                NºCliente = x.ClientNo,
                CódigoRegião = x.CodeRegion,
                CódigoÁreaFuncional = x.CodeFunctionalArea,
                CódigoCentroResponsabilidade = x.CodeResponsabilityCenter,
                CódEndereçoEnvio = x.CodeShippingAddress,
                EnvioANome = x.ShippingName,
                EnvioANome2 = x.ShippingName2,
                EnvioAEndereço = x.ShippingAddress,
                EnvioAEndereço2 = x.ShippingAddress2,
                EnvioACódPostal = x.ShippingZipCode,
                EnvioALocalidade = x.ShippingLocality,
                PeríodoFatura = x.InvocePeriod,
                ÚltimaDataFatura = string.IsNullOrEmpty(x.LastInvoiceDate) ? (DateTime?)null : DateTime.Parse(x.LastInvoiceDate),
                PróximaDataFatura = string.IsNullOrEmpty(x.NextInvoiceDate) ? (DateTime?)null : DateTime.Parse(x.NextInvoiceDate),
                DataInicial = string.IsNullOrEmpty(x.StartData) ? (DateTime?)null : DateTime.Parse(x.StartData),
                DataExpiração = string.IsNullOrEmpty(x.DueDate) ? (DateTime?)null : DateTime.Parse(x.DueDate),
                JuntarFaturas = x.BatchInvoices,
                PróximoPeríodoFact = x.NextBillingPeriod,
                LinhasContratoEmFact = x.ContractLinesInBilling,
                CódTermosPagamento = x.CodePaymentTerms,
                CódFormaPagamento = x.CodePaymentMethod,
                TipoProposta = x.ProposalType,
                TipoFaturação = x.BillingType,
                TipoContratoManut = x.MaintenanceContractType,
                NºRequisiçãoDoCliente = x.ClientRequisitionNo,
                DataReceçãoRequisição = string.IsNullOrEmpty(x.ReceiptDateRequisition) ? (DateTime?)null : DateTime.Parse(x.ReceiptDateRequisition),
                NºCompromisso = x.PromiseNo,
                TaxaAprovisionamento = x.ProvisioningFee,
                Mc = x.Mc,
                TaxaDeslocação = x.DisplacementFee,
                ContratoAvençaFixa = x.FixedVowsAgreement,
                ObjetoServiço = x.ServiceObject,
                ContratoAvençaVariável = x.VariableAvengeAgrement,
                Notas = x.Notes,
                DataInícioContrato = string.IsNullOrEmpty(x.ContractStartDate) ? (DateTime?)null : DateTime.Parse(x.ContractStartDate),
                DataFimContrato = string.IsNullOrEmpty(x.ContractEndDate) ? (DateTime?)null : DateTime.Parse(x.ContractEndDate),
                DescriçãoDuraçãoContrato = x.ContractDurationDescription,
                DataInício1ºContrato = string.IsNullOrEmpty(x.StartDateFirstContract) ? (DateTime?)null : DateTime.Parse(x.StartDateFirstContract),
                Referência1ºContrato = x.FirstContractReference,
                DuraçãoMáxContrato = string.IsNullOrEmpty(x.ContractMaxDuration) ? (DateTime?)null : DateTime.Parse(x.ContractMaxDuration),
                RescisãoPrazoAviso = x.TerminationTermNotice,
                CondiçõesParaRenovação = x.RenovationConditions,
                CondiçõesRenovaçãoOutra = x.RenovationConditionsAnother,
                CondiçõesPagamento = x.PaymentTerms,
                CondiçõesPagamentoOutra = x.PaymentTermsAnother,
                AssinadoPeloCliente = x.CustomerSigned,
                Juros = x.Interests,
                DataDaAssinatura = string.IsNullOrEmpty(x.SignatureDate) ? (DateTime?)null : DateTime.Parse(x.SignatureDate),
                DataEnvioCliente = string.IsNullOrEmpty(x.CustomerShipmentDate) ? (DateTime?)null : DateTime.Parse(x.CustomerShipmentDate),
                UnidadePrestação = x.ProvisionUnit,
                ReferênciaContrato = x.ContractReference,
                ValorTotalProposta = x.TotalProposalValue,
                LocalArquivoFísico = x.PhysicalFileLocation,
                NºOportunidade = x.OportunityNo,
                NºProposta = x.ProposalNo,
                NºContato = x.ContactNo,
                DataEstadoProposta = string.IsNullOrEmpty(x.DateProposedState) ? (DateTime?)null : DateTime.Parse(x.DateProposedState),
                OrigemDoPedido = x.OrderOrigin,
                DescOrigemDoPedido = x.OrdOrderSource,
                NumeraçãoInterna = x.InternalNumeration,
                DataAlteraçãoProposta = string.IsNullOrEmpty(x.ProposalChangeDate) ? (DateTime?)null : DateTime.Parse(x.ProposalChangeDate),
                DataHoraLimiteEsclarecimentos = string.IsNullOrEmpty(x.LimitClarificationDate) ? (DateTime?)null : DateTime.Parse(x.LimitClarificationDate),
                DataHoraErrosEOmissões = string.IsNullOrEmpty(x.ErrorsOmissionsDate) ? (DateTime?)null : DateTime.Parse(x.ErrorsOmissionsDate),
                DataHoraRelatórioFinal = string.IsNullOrEmpty(x.FinalReportDate) ? (DateTime?)null : DateTime.Parse(x.FinalReportDate),
                DataHoraHabilitaçãoDocumental = string.IsNullOrEmpty(x.DocumentationHabilitationDate) ? (DateTime?)null : DateTime.Parse(x.DocumentationHabilitationDate),
                DataHoraEntregaProposta = string.IsNullOrEmpty(x.ProposalDelivery) ? (DateTime?)null : DateTime.Parse(x.ProposalDelivery),
                NºComprimissoObrigatório = x.CompulsoryCompulsoryNo,
                DataHoraCriação = string.IsNullOrEmpty(x.CreateDate) ? (DateTime?)null : DateTime.Parse(x.CreateDate),
                DataHoraModificação = string.IsNullOrEmpty(x.UpdateDate) ? (DateTime?)null : DateTime.Parse(x.UpdateDate),
                UtilizadorCriação = x.CreateUser,
                UtilizadorModificação = x.UpdateUser,
                Arquivado = x.Filed,
                RazãoArquivo = x.ArchiveReason,
                ValorBaseProcedimento = x.BaseValueProcedure,
                AudiênciaPrévia = string.IsNullOrEmpty(x.PreviousHearing) ? (DateTime?)null : DateTime.Parse(x.PreviousHearing),
                Historico = x.History,
                Tipo = x.Type,
                NºVep = x.NoVEP,
                TextoFatura = x.TextoFatura,
                FaturaPrecosIvaIncluido = x.FaturaPrecosIvaIncluido,
                SomatorioLinhas = x.SomatorioLinhas
            };
            return result;
        }




    }
}
