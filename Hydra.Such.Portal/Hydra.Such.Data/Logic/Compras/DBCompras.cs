using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.ComprasML;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.ViewModel.Compras;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.ComprasML
{
    public class DBCompras
    {
        public static List<ComprasViewModel> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Compras.Select(Compras => new ComprasViewModel()
                    {
                        ID = Compras.Id,
                        CodigoProduto = Compras.CodigoProduto,
                        Descricao = Compras.Descricao,
                        Descricao2 = Compras.Descricao2,
                        CodigoUnidadeMedida = Compras.CodigoUnidadeMedida,
                        Quantidade = Compras.Quantidade,
                        NoRequisicao = Compras.NoRequisicao,
                        NoLinhaRequisicao = Compras.NoLinhaRequisicao,
                        Urgente = Compras.Urgente,
                        UrgenteTexto = Compras.Urgente == null ? "" : Compras.Urgente == false ? "Não" : "Sim",
                        RegiaoMercadoLocal = Compras.RegiaoMercadoLocal,
                        Estado = Compras.Estado,
                        //EstadoTexto = Compras.Estado == null ? "" : EnumerablesFixed.ComprasEstado.Where(y => y.Id == Compras.Estado).FirstOrDefault().Value,
                        DataCriacao = Compras.DataCriacao,
                        DataCriacaoTexto = Compras.DataCriacao == null ? "" : Compras.DataCriacao.Value.ToString("yyyy-MM-dd"),
                        HoraCriacaoTexto = Compras.DataCriacao == null ? "" : Compras.DataCriacao.Value.ToString("HH:mm:ss"),
                        UtilizadorCriacao = Compras.UtilizadorCriacao,
                        UtilizadorCriacaoTexto = Compras.UtilizadorCriacao == null ? "" : DBUserConfigurations.GetById(Compras.UtilizadorCriacao).Nome,
                        Responsaveis = Compras.Responsaveis,
                        NoProjeto = Compras.NoProjeto,
                        NoProjetoTexto = Compras.NoProjeto == null ? "" : DBProjects.GetById(Compras.NoProjeto).Descrição,
                        NoFornecedor = Compras.NoFornecedor,
                        //NoFornecedorTexto = Compras.NoFornecedor == null ? "" DBNAV2017Supplier.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, Compras.NoFornecedor).FirstOrDefault().Name,
                        NoEncomenda = Compras.NoEncomenda,
                        //NoEncomendaTexto =
                        DataEncomenda = Compras.DataEncomenda,
                        DataEncomendaTexto = Compras.DataEncomenda == null ? "" : Compras.DataEncomenda.Value.ToString("yyyy-MM-dd"),
                        HoraEncomendaTexto = Compras.DataEncomenda == null ? "" : Compras.DataEncomenda.Value.ToString("HH:mm:ss"),
                        NoConsultaMercado = Compras.NoConsultaMercado,
                        //NoConsultaMercadoTexto =
                        DataConsultaMercado = Compras.DataConsultaMercado,
                        DataConsultaMercadoTexto = Compras.DataConsultaMercado == null ? "" : Compras.DataConsultaMercado.Value.ToString("yyyy-MM-dd"),
                        HoraConsultaMercadoTexto = Compras.DataConsultaMercado == null ? "" : Compras.DataConsultaMercado.Value.ToString("HH:mm:ss"),
                        DataValidacao = Compras.DataValidacao,
                        DataValidacaoTexto = Compras.DataValidacao == null ? "" : Compras.DataValidacao.Value.ToString("yyyy-MM-dd"),
                        HoraValidacaoTexto = Compras.DataValidacao == null ? "" : Compras.DataValidacao.Value.ToString("HH:mm:ss"),
                        UtilizadorValidacao = Compras.UtilizadorValidacao,
                        UtilizadorValidacaoTexto = Compras.UtilizadorValidacao == null ? "" : DBUserConfigurations.GetById(Compras.UtilizadorValidacao).Nome,
                        DataRecusa = Compras.DataRecusa,
                        DataRecusaTexto = Compras.DataRecusa == null ? "" : Compras.DataRecusa.Value.ToString("yyyy-MM-dd"),
                        HoraRecusaTexto = Compras.DataRecusa == null ? "" : Compras.DataRecusa.Value.ToString("HH:mm:ss"),
                        UtilizadorRecusa = Compras.UtilizadorRecusa,
                        UtilizadorRecusaTexto = Compras.UtilizadorRecusa == null ? "" : DBUserConfigurations.GetById(Compras.UtilizadorRecusa).Nome,
                        DataTratado = Compras.DataTratado,
                        DataTratadoTexto = Compras.DataTratado == null ? "" : Compras.DataTratado.Value.ToString("yyyy-MM-dd"),
                        HoraTratadoTexto = Compras.DataTratado == null ? "" : Compras.DataTratado.Value.ToString("HH:mm:ss"),
                        UtilizadorTratado = Compras.UtilizadorTratado,
                        UtilizadorTratadoTexto = Compras.UtilizadorTratado == null ? "" : DBUserConfigurations.GetById(Compras.UtilizadorTratado).Nome,
                        Recusada = Compras.Recusada,
                        RecusadaTexto = Compras.Recusada == null ? "" : Compras.Recusada == false ? "Não" : "Sim",
                        DataMercadoLocal = Compras.DataMercadoLocal,
                        DataMercadoLocalTexto = Compras.DataMercadoLocal == null ? "" : Compras.DataMercadoLocal.Value.ToString("yyyy-MM-dd"),
                        HoraMercadoLocalTexto = Compras.DataMercadoLocal == null ? "" : Compras.DataMercadoLocal.Value.ToString("HH:mm:ss")
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ComprasViewModel GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Compras.Where(x =>
                        (x.Id == ID)
                    ).Select(Compras => new ComprasViewModel()
                    {
                        ID = Compras.Id,
                        CodigoProduto = Compras.CodigoProduto,
                        Descricao = Compras.Descricao,
                        Descricao2 = Compras.Descricao2,
                        CodigoUnidadeMedida = Compras.CodigoUnidadeMedida,
                        Quantidade = Compras.Quantidade,
                        NoRequisicao = Compras.NoRequisicao,
                        NoLinhaRequisicao = Compras.NoLinhaRequisicao,
                        Urgente = Compras.Urgente,
                        UrgenteTexto = Compras.Urgente == null ? "" : Compras.Urgente == false ? "Não" : "Sim",
                        RegiaoMercadoLocal = Compras.RegiaoMercadoLocal,
                        Estado = Compras.Estado,
                        //EstadoTexto = Compras.Estado == null ? "" : EnumerablesFixed.ComprasEstado.Where(y => y.Id == Compras.Estado).FirstOrDefault().Value,
                        DataCriacao = Compras.DataCriacao,
                        DataCriacaoTexto = Compras.DataCriacao == null ? "" : Compras.DataCriacao.Value.ToString("yyyy-MM-dd"),
                        HoraCriacaoTexto = Compras.DataCriacao == null ? "" : Compras.DataCriacao.Value.ToString("HH:mm:ss"),
                        UtilizadorCriacao = Compras.UtilizadorCriacao,
                        UtilizadorCriacaoTexto = Compras.UtilizadorCriacao == null ? "" : DBUserConfigurations.GetById(Compras.UtilizadorCriacao).Nome,
                        Responsaveis = Compras.Responsaveis,
                        NoProjeto = Compras.NoProjeto,
                        NoProjetoTexto = Compras.NoProjeto == null ? "" : DBProjects.GetById(Compras.NoProjeto).Descrição,
                        NoFornecedor = Compras.NoFornecedor,
                        //NoFornecedorTexto = Compras.NoFornecedor == null ? "" DBNAV2017Supplier.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, Compras.NoFornecedor).FirstOrDefault().Name,
                        NoEncomenda = Compras.NoEncomenda,
                        //NoEncomendaTexto =
                        DataEncomenda = Compras.DataEncomenda,
                        DataEncomendaTexto = Compras.DataEncomenda == null ? "" : Compras.DataEncomenda.Value.ToString("yyyy-MM-dd"),
                        HoraEncomendaTexto = Compras.DataEncomenda == null ? "" : Compras.DataEncomenda.Value.ToString("HH:mm:ss"),
                        NoConsultaMercado = Compras.NoConsultaMercado,
                        //NoConsultaMercadoTexto =
                        DataConsultaMercado = Compras.DataConsultaMercado,
                        DataConsultaMercadoTexto = Compras.DataConsultaMercado == null ? "" : Compras.DataConsultaMercado.Value.ToString("yyyy-MM-dd"),
                        HoraConsultaMercadoTexto = Compras.DataConsultaMercado == null ? "" : Compras.DataConsultaMercado.Value.ToString("HH:mm:ss"),
                        DataValidacao = Compras.DataValidacao,
                        DataValidacaoTexto = Compras.DataValidacao == null ? "" : Compras.DataValidacao.Value.ToString("yyyy-MM-dd"),
                        HoraValidacaoTexto = Compras.DataValidacao == null ? "" : Compras.DataValidacao.Value.ToString("HH:mm:ss"),
                        UtilizadorValidacao = Compras.UtilizadorValidacao,
                        UtilizadorValidacaoTexto = Compras.UtilizadorValidacao == null ? "" : DBUserConfigurations.GetById(Compras.UtilizadorValidacao).Nome,
                        DataRecusa = Compras.DataRecusa,
                        DataRecusaTexto = Compras.DataRecusa == null ? "" : Compras.DataRecusa.Value.ToString("yyyy-MM-dd"),
                        HoraRecusaTexto = Compras.DataRecusa == null ? "" : Compras.DataRecusa.Value.ToString("HH:mm:ss"),
                        UtilizadorRecusa = Compras.UtilizadorRecusa,
                        UtilizadorRecusaTexto = Compras.UtilizadorRecusa == null ? "" : DBUserConfigurations.GetById(Compras.UtilizadorRecusa).Nome,
                        DataTratado = Compras.DataTratado,
                        DataTratadoTexto = Compras.DataTratado == null ? "" : Compras.DataTratado.Value.ToString("yyyy-MM-dd"),
                        HoraTratadoTexto = Compras.DataTratado == null ? "" : Compras.DataTratado.Value.ToString("HH:mm:ss"),
                        UtilizadorTratado = Compras.UtilizadorTratado,
                        UtilizadorTratadoTexto = Compras.UtilizadorTratado == null ? "" : DBUserConfigurations.GetById(Compras.UtilizadorTratado).Nome,
                        Recusada = Compras.Recusada,
                        RecusadaTexto = Compras.Recusada == null ? "" : Compras.Recusada == false ? "Não" : "Sim",
                        DataMercadoLocal = Compras.DataMercadoLocal,
                        DataMercadoLocalTexto = Compras.DataMercadoLocal == null ? "" : Compras.DataMercadoLocal.Value.ToString("yyyy-MM-dd"),
                        HoraMercadoLocalTexto = Compras.DataMercadoLocal == null ? "" : Compras.DataMercadoLocal.Value.ToString("HH:mm:ss")
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ComprasViewModel> GetAllByEstado(int Estado)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Compras.Where(x =>
                        (x.Estado == Estado)
                    ).Select(Compras => new ComprasViewModel()
                    {
                        ID = Compras.Id,
                        CodigoProduto = Compras.CodigoProduto,
                        Descricao = Compras.Descricao,
                        Descricao2 = Compras.Descricao2,
                        CodigoUnidadeMedida = Compras.CodigoUnidadeMedida,
                        Quantidade = Compras.Quantidade,
                        NoRequisicao = Compras.NoRequisicao,
                        NoLinhaRequisicao = Compras.NoLinhaRequisicao,
                        Urgente = Compras.Urgente,
                        UrgenteTexto = Compras.Urgente == null ? "" : Compras.Urgente == false ? "Não" : "Sim",
                        RegiaoMercadoLocal = Compras.RegiaoMercadoLocal,
                        Estado = Compras.Estado,
                        DataCriacao = Compras.DataCriacao,
                        DataCriacaoTexto = Compras.DataCriacao == null ? "" : Compras.DataCriacao.Value.ToString("yyyy-MM-dd"),
                        HoraCriacaoTexto = Compras.DataCriacao == null ? "" : Compras.DataCriacao.Value.ToString("HH:mm:ss"),
                        UtilizadorCriacao = Compras.UtilizadorCriacao,
                        UtilizadorCriacaoTexto = Compras.UtilizadorCriacao == null ? "" : DBUserConfigurations.GetById(Compras.UtilizadorCriacao).Nome,
                        Responsaveis = Compras.Responsaveis,
                        NoProjeto = Compras.NoProjeto,

                        NoProjetoTexto = Compras.NoProjeto == null ? "" : DBProjects.GetById(Compras.NoProjeto) != null ? DBProjects.GetById(Compras.NoProjeto).Descrição : "",

                        NoFornecedor = Compras.NoFornecedor,
                        NoEncomenda = Compras.NoEncomenda,
                        DataEncomenda = Compras.DataEncomenda,
                        DataEncomendaTexto = Compras.DataEncomenda == null ? "" : Compras.DataEncomenda.Value.ToString("yyyy-MM-dd"),
                        HoraEncomendaTexto = Compras.DataEncomenda == null ? "" : Compras.DataEncomenda.Value.ToString("HH:mm:ss"),
                        NoConsultaMercado = Compras.NoConsultaMercado,
                        DataConsultaMercado = Compras.DataConsultaMercado,
                        DataConsultaMercadoTexto = Compras.DataConsultaMercado == null ? "" : Compras.DataConsultaMercado.Value.ToString("yyyy-MM-dd"),
                        HoraConsultaMercadoTexto = Compras.DataConsultaMercado == null ? "" : Compras.DataConsultaMercado.Value.ToString("HH:mm:ss"),
                        DataValidacao = Compras.DataValidacao,
                        DataValidacaoTexto = Compras.DataValidacao == null ? "" : Compras.DataValidacao.Value.ToString("yyyy-MM-dd"),
                        HoraValidacaoTexto = Compras.DataValidacao == null ? "" : Compras.DataValidacao.Value.ToString("HH:mm:ss"),
                        UtilizadorValidacao = Compras.UtilizadorValidacao,
                        UtilizadorValidacaoTexto = Compras.UtilizadorValidacao == null ? "" : DBUserConfigurations.GetById(Compras.UtilizadorValidacao).Nome,
                        DataRecusa = Compras.DataRecusa,
                        DataRecusaTexto = Compras.DataRecusa == null ? "" : Compras.DataRecusa.Value.ToString("yyyy-MM-dd"),
                        HoraRecusaTexto = Compras.DataRecusa == null ? "" : Compras.DataRecusa.Value.ToString("HH:mm:ss"),
                        UtilizadorRecusa = Compras.UtilizadorRecusa,
                        UtilizadorRecusaTexto = Compras.UtilizadorRecusa == null ? "" : DBUserConfigurations.GetById(Compras.UtilizadorRecusa).Nome,
                        DataTratado = Compras.DataTratado,
                        DataTratadoTexto = Compras.DataTratado == null ? "" : Compras.DataTratado.Value.ToString("yyyy-MM-dd"),
                        HoraTratadoTexto = Compras.DataTratado == null ? "" : Compras.DataTratado.Value.ToString("HH:mm:ss"),
                        UtilizadorTratado = Compras.UtilizadorTratado,
                        UtilizadorTratadoTexto = Compras.UtilizadorTratado == null ? "" : DBUserConfigurations.GetById(Compras.UtilizadorTratado).Nome,
                        Recusada = Compras.Recusada,
                        RecusadaTexto = Compras.Recusada == null ? "" : Compras.Recusada == false ? "Não" : "Sim",
                        DataMercadoLocal = Compras.DataMercadoLocal,
                        DataMercadoLocalTexto = Compras.DataMercadoLocal == null ? "" : Compras.DataMercadoLocal.Value.ToString("yyyy-MM-dd"),
                        HoraMercadoLocalTexto = Compras.DataMercadoLocal == null ? "" : Compras.DataMercadoLocal.Value.ToString("HH:mm:ss")
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Compras Create(Compras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Compras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Compras Update(Compras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Compras.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool Delete(Compras ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Compras.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static List<ComprasViewModel> ToListComprasViewModel(List<ComprasViewModel> ComprasList)
        {
            try
            {
                List<ComprasViewModel> ComprasViewModel = null;

                return ComprasViewModel;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ComprasViewModel ParseToComprasViewModel(Compras Compras)
        {
            try
            {
                if (Compras != null)
                {
                    ComprasViewModel ComprasVM = new ComprasViewModel
                    {
                        ID = Compras.Id,
                        CodigoProduto = Compras.CodigoProduto,
                        Descricao = Compras.Descricao,
                        Descricao2 = Compras.Descricao2,
                        CodigoUnidadeMedida = Compras.CodigoUnidadeMedida,
                        Quantidade = Compras.Quantidade,
                        NoRequisicao = Compras.NoRequisicao,
                        NoLinhaRequisicao = Compras.NoLinhaRequisicao,
                        Urgente = Compras.Urgente,
                        RegiaoMercadoLocal = Compras.RegiaoMercadoLocal,
                        Estado = Compras.Estado,
                        DataCriacao = Compras.DataCriacao,
                        UtilizadorCriacao = Compras.UtilizadorCriacao,
                        Responsaveis = Compras.Responsaveis,
                        NoProjeto = Compras.NoProjeto,
                        NoFornecedor = Compras.NoFornecedor,
                        NoEncomenda = Compras.NoEncomenda,
                        DataEncomenda = Compras.DataEncomenda,
                        NoConsultaMercado = Compras.NoConsultaMercado,
                        DataConsultaMercado = Compras.DataConsultaMercado,
                        DataValidacao = Compras.DataValidacao,
                        UtilizadorValidacao = Compras.UtilizadorValidacao,
                        DataRecusa = Compras.DataRecusa,
                        UtilizadorRecusa = Compras.UtilizadorRecusa,
                        DataTratado = Compras.DataTratado,
                        UtilizadorTratado = Compras.UtilizadorTratado,
                        Recusada = Compras.Recusada,
                        DataMercadoLocal = Compras.DataMercadoLocal
                    };

                    return ComprasVM;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Compras ParseToCompras(ComprasViewModel ComprasVM)
        {
            try
            {
                if (ComprasVM != null)
                {
                    Compras ComprasModel = new Compras
                    {
                        Id = ComprasVM.ID,
                        CodigoProduto = ComprasVM.CodigoProduto,
                        Descricao = ComprasVM.Descricao,
                        Descricao2 = ComprasVM.Descricao2,
                        CodigoUnidadeMedida = ComprasVM.CodigoUnidadeMedida,
                        Quantidade = ComprasVM.Quantidade,
                        NoRequisicao = ComprasVM.NoRequisicao,
                        NoLinhaRequisicao = ComprasVM.NoLinhaRequisicao,
                        Urgente = ComprasVM.Urgente,
                        RegiaoMercadoLocal = ComprasVM.RegiaoMercadoLocal,
                        Estado = ComprasVM.Estado,
                        DataCriacao = ComprasVM.DataCriacao,
                        UtilizadorCriacao = ComprasVM.UtilizadorCriacao,
                        Responsaveis = ComprasVM.Responsaveis,
                        NoProjeto = ComprasVM.NoProjeto,
                        NoFornecedor = ComprasVM.NoFornecedor,
                        NoEncomenda = ComprasVM.NoEncomenda,
                        DataEncomenda = ComprasVM.DataEncomenda,
                        NoConsultaMercado = ComprasVM.NoConsultaMercado,
                        DataConsultaMercado = ComprasVM.DataConsultaMercado,
                        DataValidacao = ComprasVM.DataValidacao,
                        UtilizadorValidacao = ComprasVM.UtilizadorValidacao,
                        DataRecusa = ComprasVM.DataRecusa,
                        UtilizadorRecusa = ComprasVM.UtilizadorRecusa,
                        DataTratado = ComprasVM.DataTratado,
                        UtilizadorTratado = ComprasVM.UtilizadorTratado,
                        Recusada = ComprasVM.Recusada,
                        DataMercadoLocal = ComprasVM.DataMercadoLocal
                    };

                    return ComprasModel;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}