using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Viaturas;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBNAV2009Viaturas
    {
        public static NAV2009Viaturas Get(string Matricula)
        {
            try
            {
                NAV2009Viaturas ViaturaNAV2009 = new NAV2009Viaturas();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@Matricula", Matricula),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009ViaturasGet @Matricula", parameters);

                    foreach (dynamic temp in data)
                    {
                        ViaturaNAV2009.Matricula = temp.Matricula != null ? Convert.ToString(temp.Matricula) : "";
                        ViaturaNAV2009.DataMatricula = temp.DataMatricula != null ? Convert.ToDateTime(temp.DataMatricula) : DateTime.MinValue;
                        ViaturaNAV2009.NoQuadro = temp.NoQuadro != null ? Convert.ToString(temp.NoQuadro) : "";
                        ViaturaNAV2009.Estado = temp.Estado != null ? Convert.ToInt32(temp.Estado) : 0;
                        ViaturaNAV2009.PesoBruto = temp.PesoBruto != null ? Convert.ToString(temp.PesoBruto) : "";
                        ViaturaNAV2009.Tara = temp.Tara != null ? Convert.ToString(temp.Tara) : "";
                        ViaturaNAV2009.Cilindrada = temp.Cilindrada != null ? Convert.ToString(temp.Cilindrada) : "";
                        ViaturaNAV2009.Potencia = temp.Potencia != null ? Convert.ToString(temp.Potencia) : "";
                        ViaturaNAV2009.Combustivel = temp.Combustivel != null ? Convert.ToInt32(temp.Combustivel) : 0;
                        ViaturaNAV2009.NoLugares = temp.NoLugares != null ? Convert.ToString(temp.NoLugares) : "";
                        ViaturaNAV2009.Cor = temp.Cor != null ? Convert.ToString(temp.Cor) : "";
                        ViaturaNAV2009.DistanciaEntreEixos = temp.DistanciaEntreEixos != null ? Convert.ToString(temp.DistanciaEntreEixos) : "";
                        ViaturaNAV2009.PneumaticosFrente = temp.PneumaticosFrente != null ? Convert.ToString(temp.PneumaticosFrente) : "";
                        ViaturaNAV2009.PneumaticosRetaguarda = temp.PneumaticosRetaguarda != null ? Convert.ToString(temp.PneumaticosRetaguarda) : "";
                        ViaturaNAV2009.DataAquisicao = temp.DataAquisicao != null ? Convert.ToDateTime(temp.DataAquisicao) : DateTime.MinValue;
                        ViaturaNAV2009.TipoPropriedade = temp.TipoPropriedade != null ? Convert.ToInt32(temp.TipoPropriedade) : 0;
                        ViaturaNAV2009.GlobalDimension1Code = temp.GlobalDimension1Code != null ? Convert.ToString(temp.GlobalDimension1Code) : "";
                        ViaturaNAV2009.GlobalDimension2Code = temp.GlobalDimension2Code != null ? Convert.ToString(temp.GlobalDimension2Code) : "";
                        ViaturaNAV2009.ShortcutDimension3Code = temp.ShortcutDimension3Code != null ? Convert.ToString(temp.ShortcutDimension3Code) : "";
                        ViaturaNAV2009.Observacoes = temp.Observacoes != null ? Convert.ToString(temp.Observacoes) : "";
                        ViaturaNAV2009.Utilizador = temp.Utilizador != null ? Convert.ToString(temp.Utilizador) : "";
                        ViaturaNAV2009.DataAlteracao = temp.DataAlteracao != null ? Convert.ToDateTime(temp.DataAlteracao) : DateTime.MinValue;
                        ViaturaNAV2009.LocalParqueamento = temp.LocalParqueamento != null ? Convert.ToString(temp.LocalParqueamento) : "";
                        ViaturaNAV2009.IntervaloRevisoes = temp.IntervaloRevisoes != null ? Convert.ToInt32(temp.IntervaloRevisoes) : 0;
                        ViaturaNAV2009.ConsumoIndicativoViatura = temp.ConsumoIndicativoViatura != null ? Convert.ToDecimal(temp.ConsumoIndicativoViatura) : 0;
                    }
                }

                return ViaturaNAV2009;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static int Create(NAV2009Viaturas Viatura)
        {
            try
            {
                int result = 0;
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@Matricula", Viatura.Matricula),
                        new SqlParameter("@DataMatricula", Viatura.DataMatricula.ToString("yyyy-MM-dd")),
                        new SqlParameter("@NoQuadro", Viatura.NoQuadro),
                        new SqlParameter("@Estado", Viatura.Estado),
                        new SqlParameter("@PesoBruto", Viatura.PesoBruto),
                        new SqlParameter("@Tara", Viatura.Tara),
                        new SqlParameter("@Cilindrada", Viatura.Cilindrada),
                        new SqlParameter("@Potencia", Viatura.Potencia),
                        new SqlParameter("@Combustivel", Viatura.Combustivel),
                        new SqlParameter("@NoLugares", Viatura.NoLugares),
                        new SqlParameter("@Cor", Viatura.Cor),
                        new SqlParameter("@DistanciaEntreEixos", Viatura.DistanciaEntreEixos),
                        new SqlParameter("@PneumaticosFrente", Viatura.PneumaticosFrente),
                        new SqlParameter("@PneumaticosRetaguarda", Viatura.PneumaticosRetaguarda),
                        new SqlParameter("@DataAquisicao", Viatura.DataAquisicao.ToString("yyyy-MM-dd")),
                        new SqlParameter("@TipoPropriedade", Viatura.TipoPropriedade),
                        new SqlParameter("@GlobalDimension1Code", Viatura.GlobalDimension1Code),
                        new SqlParameter("@GlobalDimension2Code", Viatura.GlobalDimension2Code),
                        new SqlParameter("@ShortcutDimension3Code", Viatura.ShortcutDimension3Code),
                        new SqlParameter("@Observacoes", Viatura.Observacoes),
                        new SqlParameter("@Utilizador", Viatura.Utilizador),
                        new SqlParameter("@DataAlteracao", Viatura.DataAlteracao.ToString("yyyy-MM-dd")),
                        new SqlParameter("@LocalParqueamento", Viatura.LocalParqueamento),
                        new SqlParameter("@IntervaloRevisoes", Viatura.IntervaloRevisoes),
                        new SqlParameter("@ConsumoIndicativoViatura", Viatura.ConsumoIndicativoViatura.ToString().Replace(",", "."))
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009ViaturasCreate @Matricula, @DataMatricula, @NoQuadro, @Estado, @PesoBruto, @Tara, @Cilindrada, " +
                        "@Potencia, @Combustivel, @NoLugares, @Cor, @DistanciaEntreEixos, @PneumaticosFrente, @PneumaticosRetaguarda, @DataAquisicao, @TipoPropriedade, " +
                        "@GlobalDimension1Code, @GlobalDimension2Code, @ShortcutDimension3Code, @Observacoes, @Utilizador, @DataAlteracao, @LocalParqueamento, " +
                        "@IntervaloRevisoes, @ConsumoIndicativoViatura", parameters);

                    foreach (dynamic temp in data)
                    {
                        result = temp.Result != null ? (int)temp.Result : (int)0;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int Update(NAV2009Viaturas Viatura)
        {
            try
            {
                int result = 0;
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@Matricula", Viatura.Matricula),
                        new SqlParameter("@DataMatricula", Viatura.DataMatricula.ToString("yyyy-MM-dd")),
                        new SqlParameter("@NoQuadro", Viatura.NoQuadro),
                        new SqlParameter("@Estado", Viatura.Estado),
                        new SqlParameter("@PesoBruto", Viatura.PesoBruto),
                        new SqlParameter("@Tara", Viatura.Tara),
                        new SqlParameter("@Cilindrada", Viatura.Cilindrada),
                        new SqlParameter("@Potencia", Viatura.Potencia),
                        new SqlParameter("@Combustivel", Viatura.Combustivel),
                        new SqlParameter("@NoLugares", Viatura.NoLugares),
                        new SqlParameter("@Cor", Viatura.Cor),
                        new SqlParameter("@DistanciaEntreEixos", Viatura.DistanciaEntreEixos),
                        new SqlParameter("@PneumaticosFrente", Viatura.PneumaticosFrente),
                        new SqlParameter("@PneumaticosRetaguarda", Viatura.PneumaticosRetaguarda),
                        new SqlParameter("@DataAquisicao", Viatura.DataAquisicao.ToString("yyyy-MM-dd")),
                        new SqlParameter("@TipoPropriedade", Viatura.TipoPropriedade),
                        new SqlParameter("@GlobalDimension1Code", Viatura.GlobalDimension1Code),
                        new SqlParameter("@GlobalDimension2Code", Viatura.GlobalDimension2Code),
                        new SqlParameter("@ShortcutDimension3Code", Viatura.ShortcutDimension3Code),
                        new SqlParameter("@Observacoes", Viatura.Observacoes),
                        new SqlParameter("@Utilizador", Viatura.Utilizador),
                        new SqlParameter("@DataAlteracao", Viatura.DataAlteracao.ToString("yyyy-MM-dd")),
                        new SqlParameter("@LocalParqueamento", Viatura.LocalParqueamento),
                        new SqlParameter("@IntervaloRevisoes", Viatura.IntervaloRevisoes),
                        new SqlParameter("@ConsumoIndicativoViatura", Viatura.ConsumoIndicativoViatura.ToString().Replace(",", "."))
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009ViaturasUpdate @Matricula, @DataMatricula, @NoQuadro, @Estado, @PesoBruto, @Tara, @Cilindrada, " +
                        "@Potencia, @Combustivel, @NoLugares, @Cor, @DistanciaEntreEixos, @PneumaticosFrente, @PneumaticosRetaguarda, @DataAquisicao, @TipoPropriedade, " +
                        "@GlobalDimension1Code, @GlobalDimension2Code, @ShortcutDimension3Code, @Observacoes, @Utilizador, @DataAlteracao, @LocalParqueamento, " +
                        "@IntervaloRevisoes, @ConsumoIndicativoViatura", parameters);

                    foreach (dynamic temp in data)
                    {
                        result = temp.Result != null ? (int)temp.Result : (int)0;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
