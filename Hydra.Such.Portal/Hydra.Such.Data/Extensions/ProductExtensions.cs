using Hydra.Such.Data.ViewModel.ProjectView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Hydra.Such.Data.Extensions
{
    public static class ProductExtensions
    {

        /// <summary>
        /// Gera filtro de por cada dimensão do utilizador
        /// </summary>
        /// <param name="items">Dimensões de um utilizador</param>
        /// <param name="rootAreaId">Área de raiz. Ex.: Nutrição</param>
        /// <param name="includeRootAreaId">Incluir a área de raiz no filtro</param>
        /// <returns>String com o filtro no formato existente no campo [Area_Filtro] tab Produtos do NAV. Ex.: -50-51-52-</returns>
        public static string GenerateNAVProductFilter(this List<NAVDimValueViewModel> items, string rootAreaId, bool includeRootAreaId = false)
        {
            //List<NAVDimValueViewModel> userDimensionValues = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name);
            //string allowedProductsFilter = userDimensionValues.GenerateNAVProductFilter(rootAreaId, true);

            List<string> foundItems;// = string.Empty;
            string rootIdToSearch = string.Empty;
            switch (rootAreaId)
            {
                case "00":
                    /* 
                    (Áreas Apoio e Suporte):
                        Orgão Sociais e Gabinetes de Apoio à Gestão
                        Direções Regionais/Internacional
                        Tesouraria
                        Contabilidade
                        Recursos Humanos
                        Aprovisionamento e Logística
                     */
                    rootIdToSearch = "0";
                    break;
                case "11":
                    // Manutenção Instalações e Equipamentos Hospitalares
                    rootIdToSearch = "11";
                    break;
                case "12":
                    // Segurança e Controlo Técnico
                    rootIdToSearch = "12";
                    break;
                case "13":
                    // Energia
                    rootIdToSearch = "13";
                    break;
                case "14":
                    // Projectos e Obras
                    rootIdToSearch = "13";
                    break;
                case "22":
                    //Gestão e Tratamento de Roupa Hospitalar (Roupa)
                    rootIdToSearch = "22";
                    break;
                case "23":
                    //Gestão e Tratamento de Resíduos Hospitalares
                    rootIdToSearch = "23";
                    break;
                case "24":
                    //Gestão e Reprocessamento de Dispositivos Médicos
                    rootIdToSearch = "24";
                    break;
                case "28":
                    //Gestão de Limpeza Hospitalar
                    rootIdToSearch = "28";
                    break;
                case "3":
                    /* 
                    Novas áreas:
                        Gestão de Serviços
                        Gestão de Serviços de Transporte
                        Gestão de Parques de Estacionamento
                        Gestão  de Arquivo  e Armazéns Centrais
                     */
                    rootIdToSearch = "3";
                    break;
                case "50":
                    //Nutrição
                    rootIdToSearch = "5";
                    break;

            }
            foundItems = items.Select(x => x.Code)
                              .Where(item => item.StartsWith(rootIdToSearch)).ToList();

            string rootAreaIdFilter = string.Empty;
            if (includeRootAreaId)
            {
                rootAreaIdFilter = rootAreaId.GenerateNAVProductFilter();
                if (!string.IsNullOrEmpty(rootAreaIdFilter))
                {
                    foundItems.Add(rootAreaId.GenerateNAVProductFilter());
                }
            }
            
            string allowedDimensionsFilter = string.Empty;
            foreach (var item in foundItems)
            {
                allowedDimensionsFilter += allowedDimensionsFilter.Length > 0 ? item + "-" : "-" + item + "-";
            }

            return allowedDimensionsFilter;
        }

        /// <summary>
        /// Gera filtro para uma determinada Área
        /// </summary>
        /// <param name="rootAreaId">Área de raiz. Ex.: Nutrição</param>
        /// <returns>String com o filtro no formato existente no campo [Area_Filtro] tab Produtos do NAV. Ex.: -5-</returns>
        public static string GenerateNAVProductFilter(this string rootAreaId)
        {
            //List<string> stringsToCheck = null;
            string allowedDimensionsFilter = string.Empty;
            string rootIdToSearch = string.Empty;
            switch (rootAreaId)
            {
                case "0":
                    /* 
                    (Áreas Apoio e Suporte):
                        Orgão Sociais e Gabinetes de Apoio à Gestão
                        Direções Regionais/Internacional
                        Tesouraria
                        Contabilidade
                        Recursos Humanos
                        Aprovisionamento e Logística
                     */
                    allowedDimensionsFilter = "-0-";
                    //stringsToCheck = new List<string>() { "01", "02", 03, 04, 05, 06 };
                    break;
                case "11":
                    // Manutenção Instalações e Equipamentos Hospitalares
                    allowedDimensionsFilter = "-11-";
                    break;
                case "12":
                    // Segurança e Controlo Técnico
                    allowedDimensionsFilter = "-12-";
                    break;
                case "13":
                    // Energia
                    allowedDimensionsFilter = "-13-";
                    break;
                case "14":
                    // Projectos e Obras
                    allowedDimensionsFilter = "-13-";
                    break;
                case "22":
                    //Gestão e Tratamento de Roupa Hospitalar (Roupa)
                    allowedDimensionsFilter = "-22-";
                    break;
                case "23":
                    //Gestão e Tratamento de Resíduos Hospitalares
                    allowedDimensionsFilter = "-23-";
                    break;
                case "24":
                    //Gestão e Reprocessamento de Dispositivos Médicos
                    allowedDimensionsFilter = "-24-";
                    break;
                case "28":
                    //Gestão de Limpeza Hospitalar
                    allowedDimensionsFilter = "-28-";
                    break;
                case "3":
                    /* 
                    Novas áreas:
                        Gestão de Serviços
                        Gestão de Serviços de Transporte
                        Gestão de Parques de Estacionamento
                        Gestão  de Arquivo  e Armazéns Centrais
                     */
                    allowedDimensionsFilter = "-3-";
                    break;
                case "50":
                    //Nutrição
                    allowedDimensionsFilter = "-5-";
                    break;

            }
            return allowedDimensionsFilter;
        }
    }
}
