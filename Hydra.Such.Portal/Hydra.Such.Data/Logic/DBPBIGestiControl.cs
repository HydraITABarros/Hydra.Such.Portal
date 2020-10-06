using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WSCustomerNAV;
using Hydra.Such.Data.ViewModel.Clients;
using Hydra.Such.Data.ViewModel.PBIGestiControl;

namespace Hydra.Such.Data.Logic
{
    public class DBPBIGestiControl
    {
        public static List<PBIGestiControl_AreasViewModel> Get_Areas()
        {
            try
            {
                List<PBIGestiControl_AreasViewModel> result = new List<PBIGestiControl_AreasViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@ID", "")
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec PBIGestiControl_Get_Areas @ID", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new PBIGestiControl_AreasViewModel()
                        {
                            ID = temp.ID.Equals(DBNull.Value) ? "" : (string)temp.ID,
                            Area = temp.Area.Equals(DBNull.Value) ? "" : (string)temp.Area
                        });
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PBIGestiControl_IndicadoresViewModel> Get_Indicadores(string IdArea)
        {
            try
            {
                List<PBIGestiControl_IndicadoresViewModel> result = new List<PBIGestiControl_IndicadoresViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@IdArea", IdArea)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec PBIGestiControl_Get_Indicadores @IdArea", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new PBIGestiControl_IndicadoresViewModel()
                        {
                            ID = temp.ID.Equals(DBNull.Value) ? "" : (string)temp.ID,
                            Indicador = temp.Indicador.Equals(DBNull.Value) ? "" : (string)temp.Indicador
                        });
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PBIGestiControl_MovPropostasViewModel> Get_MovPropostas()
        {
            try
            {
                List<PBIGestiControl_MovPropostasViewModel> result = new List<PBIGestiControl_MovPropostasViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@ID", "")
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec PBIGestiControl_Get_MovPropostas @ID", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new PBIGestiControl_MovPropostasViewModel()
                        {
                            ID = temp.ID.Equals(DBNull.Value) ? "" : (string)temp.ID,
                            CResp = temp.CResp.Equals(DBNull.Value) ? "" : (string)temp.CResp,
                            Data = temp.Data.Equals(DBNull.Value) ? "" : (string)temp.Data,
                            NumPropostas = temp.NumPropostas.Equals(DBNull.Value) ? "" : (string)temp.NumPropostas,
                            NumRevistas = temp.NumRevistas.Equals(DBNull.Value) ? "" : (string)temp.NumRevistas,
                            NumGanhas = temp.NumGanhas.Equals(DBNull.Value) ? "" : (string)temp.NumGanhas
                        });
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PBIGestiControl_MovProducaoCRespViewModel> Get_MovProducaoCResp()
        {
            try
            {
                List<PBIGestiControl_MovProducaoCRespViewModel> result = new List<PBIGestiControl_MovProducaoCRespViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@ID", "")
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec PBIGestiControl_Get_MovProducaoCResp @ID", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new PBIGestiControl_MovProducaoCRespViewModel()
                        {
                            ID = temp.ID.Equals(DBNull.Value) ? "" : (string)temp.ID,
                            IdArea = temp.IdArea.Equals(DBNull.Value) ? "" : (string)temp.IdArea,
                            Area = temp.Area.Equals(DBNull.Value) ? "" : (string)temp.Area,
                            IdIndicador = temp.IdIndicador.Equals(DBNull.Value) ? "" : (string)temp.IdIndicador,
                            Indicador = temp.Indicador.Equals(DBNull.Value) ? "" : (string)temp.Indicador,
                            DataPro = temp.DataPro.Equals(DBNull.Value) ? "" : (string)temp.DataPro,
                            VProdGrafico = temp.VProdGrafico.Equals(DBNull.Value) ? "" : (string)temp.VProdGrafico
                        });
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PBIGestiControl_MovProducaoViewModel> Get_MovProducao()
        {
            try
            {
                List<PBIGestiControl_MovProducaoViewModel> result = new List<PBIGestiControl_MovProducaoViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@ID", "")
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec PBIGestiControl_Get_MovProducao @ID", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new PBIGestiControl_MovProducaoViewModel()
                        {
                            ID = temp.ID.Equals(DBNull.Value) ? "" : (string)temp.ID,
                            IdArea = temp.IdArea.Equals(DBNull.Value) ? "" : (string)temp.IdArea,
                            Area = temp.Area.Equals(DBNull.Value) ? "" : (string)temp.Area,
                            IdIndicador = temp.IdIndicador.Equals(DBNull.Value) ? "" : (string)temp.IdIndicador,
                            Indicador = temp.Indicador.Equals(DBNull.Value) ? "" : (string)temp.Indicador,
                            DataPro = temp.DataPro.Equals(DBNull.Value) ? "" : (string)temp.DataPro,
                            VProducao = temp.VProducao.Equals(DBNull.Value) ? "" : (string)temp.VProducao,
                            VProdGrafico = temp.VProdGrafico.Equals(DBNull.Value) ? "" : (string)temp.VProdGrafico
                        });
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PBIGestiControl_GeralViewModel Get_Geral()
        {
            try
            {
                PBIGestiControl_GeralViewModel result = new PBIGestiControl_GeralViewModel();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@ID", 1)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec PBIGestiControl_Get_Geral @ID", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.ID = temp.ID.Equals(DBNull.Value) ? "" : (string)temp.ID;
                        result.DataFecho = (DateTime)temp.DataFecho;
                        result.DataFechoText = temp.DataFecho.Equals(DBNull.Value) ? "" : Convert.ToDateTime(temp.DataFecho).ToString("yyyy-MM-dd");
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Update_Geral(string DataFecho)
        {
            try
            {
                bool result = false;

                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@Id", "1"),
                        new SqlParameter("@DataFecho", DataFecho)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec PBIGestiControl_Update_Geral @Id, @DataFecho", parameters);

                    foreach (dynamic temp in data)
                    {
                        result = temp.OK.Equals(DBNull.Value) ? false : Convert.ToInt32(temp.OK) > 0 ? true : false;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static int Insert_MovProducao(string Area, string Indicador, string DataProducaoAno, string DataProducaoMes, string ValorProducao, string ValorProducaoGrafico)
        {
            try
            {
                int result = 99;

                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@Area", Area),
                        new SqlParameter("@Indicador", Indicador),
                        new SqlParameter("@DataProducaoAno", DataProducaoAno),
                        new SqlParameter("@DataProducaoMes", DataProducaoMes),
                        new SqlParameter("@ValorProducao", ValorProducao),
                        new SqlParameter("@ValorProducaoGrafico", ValorProducaoGrafico)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec PBIGestiControl_Insert_MovProducao @Area, @Indicador, @DataProducaoAno, @DataProducaoMes, @ValorProducao, @ValorProducaoGrafico", parameters);

                    foreach (dynamic temp in data)
                    {
                        result = temp.OK.Equals(DBNull.Value) ? 0 : Convert.ToInt32(temp.OK);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return 99;
            }
        }

        public static int Delete_MovProducao(string Id)
        {
            try
            {
                int result = 99;

                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@Id", Id)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec PBIGestiControl_Delete_MovProducao @Id", parameters);

                    foreach (dynamic temp in data)
                    {
                        result = temp.OK.Equals(DBNull.Value) ? 0 : Convert.ToInt32(temp.OK);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return 99;
            }
        }

        public static int Update_MovProducao(string Id, string ValorProducao, string ValorProducaoGrafico)
        {
            try
            {
                int result = 99;

                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@Id", Id),
                        new SqlParameter("@ValorProducao", ValorProducao),
                        new SqlParameter("@ValorProducaoGrafico", ValorProducaoGrafico)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec PBIGestiControl_Update_MovProducao @Id, @ValorProducao, @ValorProducaoGrafico", parameters);

                    foreach (dynamic temp in data)
                    {
                        result = temp.OK.Equals(DBNull.Value) ? 0 : Convert.ToInt32(temp.OK);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return 99;
            }
        }




    }
}
