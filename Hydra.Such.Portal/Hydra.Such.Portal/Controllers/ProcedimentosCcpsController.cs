using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Portal.Configurations;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Hydra.Such.Data.ViewModel.CCP;
using Hydra.Such.Data.Logic.CCP;
using Microsoft.AspNetCore.Authorization;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class ProcedimentosCcpsController : Controller
    {
        private readonly NAVConfigurations _config;

        #region Views
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detalhes(string id)
        {
            return View();
        }
        #endregion

        [HttpPost]
        public JsonResult GetAllProcedimentos()
        {
            List<ProcedimentoCCPView> result = DBProcedimentosCCP.GetAllProcedimentosByViewToList();


            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProcedimentoDetails([FromBody] ProcedimentoCCPView data)
        {
            if(data != null)
            {
                ProcedimentosCcp proc = DBProcedimentosCCP.GetProcedimentoById(data.No);
                if(proc != null)
                {
                    ProcedimentoCCPView result = CCPFunctions.CastProcCcpToProcCcpView(proc);

                    return Json(result);
                }

                return Json(new ProcedimentoCCPView());

            }

            return Json(false);
        }
        /* zpgm. 
        private readonly SuchDBContext _context;

        public ProcedimentosCcpsController(SuchDBContext context)
        {
            _context = context;
        }

        // GET: ProcedimentosCcps
        public IActionResult Index()
        {
            var suchDBContext = _context.ProcedimentosCcp.Include(p => p.Nº1).Include(p => p.NºNavigation);
            return View(suchDBContext.ToListAsync());
        }

        // GET: ProcedimentosCcps/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procedimentosCcp = await _context.ProcedimentosCcp
                .Include(p => p.Nº1)
                .Include(p => p.NºNavigation)
                .SingleOrDefaultAsync(m => m.Nº == id);
            if (procedimentosCcp == null)
            {
                return NotFound();
            }

            return View(procedimentosCcp);
        }

        // GET: ProcedimentosCcps/Create
        public IActionResult Create()
        {
            ViewData["Nº"] = new SelectList(_context.TemposPaCcp, "NºProcedimento", "NºProcedimento");
            ViewData["Nº"] = new SelectList(_context.RegistoDeAtas, "NºProcedimento", "NºProcedimento");
            return View();
        }

        // POST: ProcedimentosCcps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nº,Tipo,Ano,Referência,CódigoRegião,CódigoÁreaFuncional,CódigoCentroResponsabilidade,Estado,DataCriação,Imobilizado,ComentárioEstado,Anexos,DataHoraEstado,UtilizadorEstado,CondiçõesDePagamento,NomeProcesso,GestorProcesso,TipoProcedimento,InformaçãoTécnica,FundamentaçãoAquisição,PreçoBase,ValorPreçoBase,Negociação,CritériosAdjudicação,Prazo,PreçoMaisBaixo,PropostaEconMaisVantajosa,PropostaVariante,AbertoFechadoAoMercado,PrazoEntrega,LocaisEntrega,ObservaçõesAdicionais,EstimativaPreço,FornecedoresSugeridos,FornecedorExclusivo,Interlocutor,DescPreçoMaisBaixo,DescPropostaEconMaisVantajosa,DescPropostaVariante,DescAbertoFechadoAoMercado,DescFornecedorExclusivo,CritérioEscolhaProcedimento,DescEscolhaProcedimento,Júri,ObjetoDoContrato,PréÁrea,SubmeterPréÁrea,ValorDecisãoContratar,ValorAdjudicaçãoAnteriro,ValorAdjudicaçãoAtual,DiferençaEuros,DiferençaPercent,WorkflowFinanceiros,WorkflowJurídicos,WorkflowFinanceirosConfirm,WorkflowJurídicosConfirm,AutorizaçãoImobCa,AutorizaçãoAberturaCa,AutorizaçãoAquisiçãoCa,RejeiçãoImobCa,RejeiçãoAberturaCa,RejeiçãoAquisiçãoCa,DataAutorizaçãoImobCa,DataAutorizaçãoAbertCa,DataAutorizaçãoAquisiCa,RatificarCaAbertura,RatificarCaAdjudicação,CaRatificar,CaDataRatificaçãoAbert,CaDataRatificaçãoAdjudic,NºAta,DataAta,ComentárioPublicação,DataPublicação,UtilizadorPublicação,DataSistemaPublicação,RecolhaComentário,DataRecolha,UtilizadorRecolha,DataSistemaRecolha,ComentárioRelatórioPreliminar,DataValidRelatórioPreliminar,UtilizadorValidRelatórioPreliminar,DataSistemaValidRelatórioPreliminar,ComentárioAudiênciaPrévia,DataAudiênciaPrévia,UtilizadorAudiênciaPrévia,DataSistemaAudiênciaPrévia,ComentárioRelatórioFinal,DataRelatórioFinal,UtilizadorRelatórioFinal,DataSistemaRelatórioFinal,ComentárioNotificação,DataNotificação,UtilizadorNotificação,DataSistemaNotificação,PrazoNotificaçãoDias,PercentExecução,Arquivado,DataFechoInicial,DataFechoPrevista,NºDiasProcesso,NºDiasAtraso,Tratado,Fornecedor,Comentário,CaSuspenso,CríticoAbertura,CríticoAdjudicação,ObjetoDecisão,RazãoNecessidade,ProtocoloContrato,AutorizaçãoAdjudicação,NãoAdjudicaçãoEEncerramento,NãoAdjudicaçãoESuspensão")] ProcedimentosCcp procedimentosCcp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(procedimentosCcp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Nº"] = new SelectList(_context.TemposPaCcp, "NºProcedimento", "NºProcedimento", procedimentosCcp.Nº);
            ViewData["Nº"] = new SelectList(_context.RegistoDeAtas, "NºProcedimento", "NºProcedimento", procedimentosCcp.Nº);
            return View(procedimentosCcp);
        }

        // GET: ProcedimentosCcps/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procedimentosCcp = await _context.ProcedimentosCcp.SingleOrDefaultAsync(m => m.Nº == id);
            if (procedimentosCcp == null)
            {
                return NotFound();
            }
            ViewData["Nº"] = new SelectList(_context.TemposPaCcp, "NºProcedimento", "NºProcedimento", procedimentosCcp.Nº);
            ViewData["Nº"] = new SelectList(_context.RegistoDeAtas, "NºProcedimento", "NºProcedimento", procedimentosCcp.Nº);
            return View(procedimentosCcp);
        }

        // POST: ProcedimentosCcps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Nº,Tipo,Ano,Referência,CódigoRegião,CódigoÁreaFuncional,CódigoCentroResponsabilidade,Estado,DataCriação,Imobilizado,ComentárioEstado,Anexos,DataHoraEstado,UtilizadorEstado,CondiçõesDePagamento,NomeProcesso,GestorProcesso,TipoProcedimento,InformaçãoTécnica,FundamentaçãoAquisição,PreçoBase,ValorPreçoBase,Negociação,CritériosAdjudicação,Prazo,PreçoMaisBaixo,PropostaEconMaisVantajosa,PropostaVariante,AbertoFechadoAoMercado,PrazoEntrega,LocaisEntrega,ObservaçõesAdicionais,EstimativaPreço,FornecedoresSugeridos,FornecedorExclusivo,Interlocutor,DescPreçoMaisBaixo,DescPropostaEconMaisVantajosa,DescPropostaVariante,DescAbertoFechadoAoMercado,DescFornecedorExclusivo,CritérioEscolhaProcedimento,DescEscolhaProcedimento,Júri,ObjetoDoContrato,PréÁrea,SubmeterPréÁrea,ValorDecisãoContratar,ValorAdjudicaçãoAnteriro,ValorAdjudicaçãoAtual,DiferençaEuros,DiferençaPercent,WorkflowFinanceiros,WorkflowJurídicos,WorkflowFinanceirosConfirm,WorkflowJurídicosConfirm,AutorizaçãoImobCa,AutorizaçãoAberturaCa,AutorizaçãoAquisiçãoCa,RejeiçãoImobCa,RejeiçãoAberturaCa,RejeiçãoAquisiçãoCa,DataAutorizaçãoImobCa,DataAutorizaçãoAbertCa,DataAutorizaçãoAquisiCa,RatificarCaAbertura,RatificarCaAdjudicação,CaRatificar,CaDataRatificaçãoAbert,CaDataRatificaçãoAdjudic,NºAta,DataAta,ComentárioPublicação,DataPublicação,UtilizadorPublicação,DataSistemaPublicação,RecolhaComentário,DataRecolha,UtilizadorRecolha,DataSistemaRecolha,ComentárioRelatórioPreliminar,DataValidRelatórioPreliminar,UtilizadorValidRelatórioPreliminar,DataSistemaValidRelatórioPreliminar,ComentárioAudiênciaPrévia,DataAudiênciaPrévia,UtilizadorAudiênciaPrévia,DataSistemaAudiênciaPrévia,ComentárioRelatórioFinal,DataRelatórioFinal,UtilizadorRelatórioFinal,DataSistemaRelatórioFinal,ComentárioNotificação,DataNotificação,UtilizadorNotificação,DataSistemaNotificação,PrazoNotificaçãoDias,PercentExecução,Arquivado,DataFechoInicial,DataFechoPrevista,NºDiasProcesso,NºDiasAtraso,Tratado,Fornecedor,Comentário,CaSuspenso,CríticoAbertura,CríticoAdjudicação,ObjetoDecisão,RazãoNecessidade,ProtocoloContrato,AutorizaçãoAdjudicação,NãoAdjudicaçãoEEncerramento,NãoAdjudicaçãoESuspensão")] ProcedimentosCcp procedimentosCcp)
        {
            if (id != procedimentosCcp.Nº)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(procedimentosCcp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcedimentosCcpExists(procedimentosCcp.Nº))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Nº"] = new SelectList(_context.TemposPaCcp, "NºProcedimento", "NºProcedimento", procedimentosCcp.Nº);
            ViewData["Nº"] = new SelectList(_context.RegistoDeAtas, "NºProcedimento", "NºProcedimento", procedimentosCcp.Nº);
            return View(procedimentosCcp);
        }

        // GET: ProcedimentosCcps/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procedimentosCcp = await _context.ProcedimentosCcp
                .Include(p => p.Nº1)
                .Include(p => p.NºNavigation)
                .SingleOrDefaultAsync(m => m.Nº == id);
            if (procedimentosCcp == null)
            {
                return NotFound();
            }

            return View(procedimentosCcp);
        }

        // POST: ProcedimentosCcps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var procedimentosCcp = await _context.ProcedimentosCcp.SingleOrDefaultAsync(m => m.Nº == id);
            _context.ProcedimentosCcp.Remove(procedimentosCcp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcedimentosCcpExists(string id)
        {
            return _context.ProcedimentosCcp.Any(e => e.Nº == id);
        }
        */
    }
}
