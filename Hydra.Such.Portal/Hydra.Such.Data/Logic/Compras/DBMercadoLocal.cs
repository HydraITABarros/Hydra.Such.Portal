using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Compras;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.ViewModel.Compras;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Compras
{
    public class DBMercadoLocal
    {
        public static List<MercadoLocalViewModel> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Compras.Select(Compras => new MercadoLocalViewModel()
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
                        RecusadaTexto = Compras.Recusada == null ? "" : Compras.Recusada == false ? "Não" : "Sim"
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static MercadoLocalViewModel GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Compras.Where(x =>
                        (x.Id == ID)
                    ).Select(Compras => new MercadoLocalViewModel()
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
                        RecusadaTexto = Compras.Recusada == null ? "" : Compras.Recusada == false ? "Não" : "Sim"
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MercadoLocalViewModel> GetAllByEstado(int Estado)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Compras.Where(x =>
                        (x.Estado == Estado)
                    ).Select(Compras => new MercadoLocalViewModel()
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
                        RecusadaTexto = Compras.Recusada == null ? "" : Compras.Recusada == false ? "Não" : "Sim"
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Database.Compras Create(Database.Compras ObjectToCreate)
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

        public static Database.Compras Update(Database.Compras ObjectToUpdate)
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

        public static bool Delete(Database.Compras ObjectToDelete)
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

        public static MercadoLocalViewModel ParseToComprasViewModel(MercadoLocal Compras)
        {
            try
            {
                if (Compras != null)
                {
                    MercadoLocalViewModel ComprasVM = new MercadoLocalViewModel();

                    ComprasVM.ID = Compras.Id;
                    ComprasVM.CodigoProduto = Compras.CodigoProduto;
                    ComprasVM.Descricao = Compras.Descricao;
                    ComprasVM.Descricao2 = Compras.Descricao2;
                    ComprasVM.CodigoUnidadeMedida = Compras.CodigoUnidadeMedida;
                    ComprasVM.Quantidade = Compras.Quantidade;
                    ComprasVM.NoRequisicao = Compras.NoRequisicao;
                    ComprasVM.NoLinhaRequisicao = Compras.NoLinhaRequisicao;
                    ComprasVM.Urgente = Compras.Urgente;
                    ComprasVM.RegiaoMercadoLocal = Compras.RegiaoMercadoLocal;
                    ComprasVM.Estado = Compras.Estado;
                    ComprasVM.DataCriacao = Compras.DataCriacao;
                    ComprasVM.UtilizadorCriacao = Compras.UtilizadorCriacao;
                    ComprasVM.Responsaveis = Compras.Responsaveis;
                    ComprasVM.NoProjeto = Compras.NoProjeto;
                    ComprasVM.NoFornecedor = Compras.NoFornecedor;
                    ComprasVM.NoEncomenda = Compras.NoEncomenda;
                    ComprasVM.DataEncomenda = Compras.DataEncomenda;
                    ComprasVM.NoConsultaMercado = Compras.NoConsultaMercado;
                    ComprasVM.DataConsultaMercado = Compras.DataConsultaMercado;
                    ComprasVM.DataValidacao = Compras.DataValidacao;
                    ComprasVM.UtilizadorValidacao = Compras.UtilizadorValidacao;
                    ComprasVM.DataRecusa = Compras.DataRecusa;
                    ComprasVM.UtilizadorRecusa = Compras.UtilizadorRecusa;
                    ComprasVM.DataTratado = Compras.DataTratado;
                    ComprasVM.UtilizadorTratado = Compras.UtilizadorTratado;
                    ComprasVM.Recusada = Compras.Recusada;

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
    }
}