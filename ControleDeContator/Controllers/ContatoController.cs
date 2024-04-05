using ControleDeContator.Filters;
using ControleDeContator.Helper;
using ControleDeContator.Models;
using ControleDeContator.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeContator.Controllers
{
    [PaginaParaUsuarioLogado]
    public class ContatoController : Controller
    {
        private readonly IContatoRepository _contato;
        private readonly ISessao _sessao;

        public ContatoController(IContatoRepository contato, ISessao sessao)
        {
            _contato = contato;
            _sessao = sessao;
        }

        [HttpGet]
        public IActionResult Index()
        {
            UsuarioModel usuarioLogado = _sessao.BuscasrSessaoDoUsuario();
            List<ContatoModel> contatos = _contato.BuscarTodos(usuarioLogado.Id);
          
            return View(contatos);
        }


        #region Criar
        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Criar(ContatoModel contatoModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuarioLogado = _sessao.BuscasrSessaoDoUsuario();
                    contatoModel.UsuarioId = usuarioLogado.Id;
                   
                    _contato.Adicionar(contatoModel);

                    TempData["MensagemSucesso"] = "Contato cadastrado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                return View(contatoModel);
            }
            catch (Exception error)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar seu contato, tente novamente, detalhe do erro: {error.Message}";
                return RedirectToAction(nameof(Index));

            }
        }
        #endregion

        #region Eitar
        [HttpGet]
        public IActionResult Editar(int? id)
        {
            var contato = _contato.GetId(id);
            return View(contato);
        }

        [HttpPost]
        public IActionResult Alterar(ContatoModel contatoModel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuarioLogado = _sessao.BuscasrSessaoDoUsuario();
                    contatoModel.UsuarioId = usuarioLogado.Id;

                    contatoModel = _contato.Atualizar(contatoModel);
                    TempData["MensagemSucesso"] = "Contato alterado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                return View(contatoModel);
            }
            catch (Exception error)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos atualizar seu contato, tente novamente, detalhe do erro: {error.Message}";
                return RedirectToAction(nameof(Index));

            }
        }
        #endregion

        #region Excluir
        [HttpGet]
        public IActionResult ApagarConfirmacao(int? id)
        {
            var contato = _contato.GetId(id);
            return View(contato);
        }
        [HttpPost]
        public IActionResult Apagar(int? id)
        {
            try
            {
                bool apagar = _contato.Remove(id);
                if (apagar)
                {
                    TempData["MensagemSucesso"] = "Contato excluido com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não conseguimos excluir seu contato, tente novamente!";

                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception error)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos excluir seu contato, tente novamente, detalhe do erro: {error.Message}";
                return RedirectToAction(nameof(Index));

            }

        }
        #endregion








    }
}
