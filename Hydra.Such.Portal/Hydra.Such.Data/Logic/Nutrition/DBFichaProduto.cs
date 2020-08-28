using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBFichaProduto
    {
        public static List<FichaProduto> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FichaProduto.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static FichaProduto Create(FichaProduto ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.FichaProduto.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static FichaProduto Update(FichaProduto ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.FichaProduto.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(FichaProduto ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.FichaProduto.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static FichaProduto GetById(string No)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FichaProduto
                        .FirstOrDefault(x => x.Nº == No);
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        #region Parses

        public static FichaProdutoViewModel ParseToViewModel(FichaProduto x)
        {
            if (x != null)
            {
                return new FichaProdutoViewModel()
                {
                    No = x.Nº,
                    Code = x.Nº,
                    Descricao = x.Descrição,
                    Descricao2 = x.Descrição2,
                    ListaDeMateriais = x.ListaDeMateriais,
                    UnidadeMedidaBase = x.UnidadeMedidaBase,
                    NoPrateleira = x.NºPrateleira,
                    PrecoUnitario = x.PreçoUnitário,
                    CustoUnitario = x.CustoUnitário,
                    Inventario = x.Inventário,
                    Imagem = x.Imagem,
                    ValorEnergetico = x.ValorEnergético,
                    ValorEnergetico100g = x.ValorEnergético100g,
                    Proteinas = x.Proteínas,
                    Proteinas100g = x.Proteínas100g,
                    Glicidos = x.Glícidos,
                    Glicidos100g = x.Glícidos100g,
                    Lipidos = x.Lípidos,
                    Lipidos100g = x.Lípidos100g,
                    FibraAlimentar = x.FibraAlimentar,
                    FibraAlimentar100g = x.FibraAlimentar100g,
                    QuantUnidadeMedida = x.QuantUnidadeMedida,
                    GramasPorQuantUnidMedida = x.GramasPorQuantUnidMedida,
                    TipoRefeicao = x.TipoRefeição,
                    DescricaoRefeicao = x.DescriçãoRefeição,
                    Taras = x.Taras,
                    AcidosGordosSaturados = x.ÁcidosGordosSaturados,
                    Acucares = x.Açucares,
                    Sal = x.Sal,
                    Cereais = x.Cereais,
                    Crustaceos = x.Crustáceos,
                    Ovos = x.Ovos,
                    Peixes = x.Peixes,
                    Amendoins = x.Amendoins,
                    Soja = x.Soja,
                    Leite = x.Leite,
                    FrutasDeCascaRija = x.FrutasDeCascaRija,
                    Aipo = x.Aipo,
                    Mostarda = x.Mostarda,
                    SementesDeSesamo = x.SementesDeSésamo,
                    DioxidoDeEnxofreESulfitos = x.DióxidoDeEnxofreESulfitos,
                    Tremoco = x.Tremoço,
                    Moluscos = x.Moluscos,
                    Tipo = x.Tipo,
                    VitaminaA = x.VitaminaA,
                    VitaminaD = x.VitaminaD,
                    Colesterol = x.Colesterol,
                    Sodio = x.Sodio,
                    Potacio = x.Potacio,
                    Calcio = x.Calcio,
                    Ferro = x.Ferro,
                    Edivel = x.Edivel,
                    Alcool = x.Alcool,
                    TipoAlteracaoSISLOG = x.TipoAlteracaoSISLOG,
                    DataAlteracaoSISLOG = x.DataAlteracaoSISLOG,
                    EnviarSISLOG = x.EnviarSISLOG,
                    SISLOG = x.SISLOG,
                    DataEnvioSISLOG = x.DataEnvioSISLOG,
                    NomeCurtoSISLOG = x.NomeCurtoSISLOG,
                    NomeCurtoSISLOGOriginal = x.NomeCurtoSISLOG,
                    DataHoraCriacao = x.DataHoraCriação,
                    DataHoraModificacao = x.DataHoraModificação,
                    UtilizadorCriacao = x.UtilizadorCriação,
                    UtilizadorModificacao = x.UtilizadorModificação
                };
            }
            return null;
        }

        public static List<FichaProdutoViewModel> ParseToViewModel(this List<FichaProduto> items)
        {
            List<FichaProdutoViewModel> locations = new List<FichaProdutoViewModel>();
            if (items != null)
                items.ForEach(x =>
                    locations.Add(ParseToViewModel(x)));
            return locations;
        }

        public static FichaProduto ParseToDatabase(FichaProdutoViewModel x)
        {
            return new FichaProduto()
            {
                 Nº = x.No,
                 Descrição = x.Descricao,
                 Descrição2 = x.Descricao2,
                 ListaDeMateriais = x.ListaDeMateriais,
                 UnidadeMedidaBase = x.UnidadeMedidaBase,
                 NºPrateleira = x.NoPrateleira,
                 PreçoUnitário = x.PrecoUnitario,
                 CustoUnitário = x.CustoUnitario,
                 Inventário = x.Inventario,
                 Imagem = x.Imagem,
                 ValorEnergético = x.ValorEnergetico,
                 ValorEnergético100g = x.ValorEnergetico100g,
                 Proteínas = x.Proteinas,
                 Proteínas100g = x.Proteinas100g,
                 Glícidos = x.Glicidos,
                 Glícidos100g = x.Glicidos100g,
                 Lípidos = x.Lipidos,
                 Lípidos100g = x.Lipidos100g,
                 FibraAlimentar = x.FibraAlimentar,
                 FibraAlimentar100g = x.FibraAlimentar100g,
                 QuantUnidadeMedida = x.QuantUnidadeMedida,
                 GramasPorQuantUnidMedida = x.GramasPorQuantUnidMedida,
                 TipoRefeição = x.TipoRefeicao,
                 DescriçãoRefeição = x.DescricaoRefeicao,
                 Taras = x.Taras,
                 ÁcidosGordosSaturados = x.AcidosGordosSaturados,
                 Açucares = x.Acucares,
                 Sal = x.Sal,
                 Cereais = x.Cereais,
                 Crustáceos = x.Crustaceos,
                 Ovos = x.Ovos,
                 Peixes = x.Peixes,
                 Amendoins = x.Amendoins,
                 Soja = x.Soja,
                 Leite = x.Leite,
                 FrutasDeCascaRija = x.FrutasDeCascaRija,
                 Aipo = x.Aipo,
                 Mostarda = x.Mostarda,
                 SementesDeSésamo = x.SementesDeSesamo,
                 DióxidoDeEnxofreESulfitos = x.DioxidoDeEnxofreESulfitos,
                 Tremoço = x.Tremoco,
                 Moluscos = x.Moluscos,
                 Tipo = x.Tipo,
                 VitaminaA = x.VitaminaA,
                 VitaminaD = x.VitaminaD,
                 Colesterol = x.Colesterol,
                 Sodio = x.Sodio,
                 Potacio = x.Potacio,
                 Calcio = x.Calcio,
                 Ferro = x.Ferro,
                 Edivel = x.Edivel,
                 Alcool = x.Alcool,
                TipoAlteracaoSISLOG = x.TipoAlteracaoSISLOG,
                DataAlteracaoSISLOG = x.DataAlteracaoSISLOG,
                EnviarSISLOG = x.EnviarSISLOG,
                SISLOG = x.SISLOG,
                DataEnvioSISLOG = x.DataEnvioSISLOG,
                NomeCurtoSISLOG = x.NomeCurtoSISLOG,
                DataHoraCriação = x.DataHoraCriacao,
                 DataHoraModificação = x.DataHoraModificacao,
                 UtilizadorCriação = x.UtilizadorCriacao,
                 UtilizadorModificação = x.UtilizadorModificacao
            };
        }

        public static List<FichaProduto> ParseToDatabase(this List<FichaProdutoViewModel> items)
        {
            List<FichaProduto> locations = new List<FichaProduto>();
            if (items != null)
                items.ForEach(x =>
                    locations.Add(ParseToDatabase(x)));
            return locations;
        }

        #endregion
    }
}
