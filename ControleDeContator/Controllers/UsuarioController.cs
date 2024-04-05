using ControleDeContator.Filters;
using ControleDeContator.Models;
using ControleDeContator.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeContator.Controllers
{
    [PaginaRestritaSomenteAdmin]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuario;
        private readonly IContatoRepository _contato;
        public UsuarioController(IUsuarioRepository usuario, IContatoRepository contato)
        {
            _usuario = usuario;
            _contato = contato;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<UsuarioModel> usuarios = _usuario.BuscarTodos();
            return View(usuarios);
        }


        [HttpGet]
        public IActionResult ListarContatosPorUsuarioId(int id) 
        {
            List<ContatoModel> contatos = _contato.BuscarTodos(id);
            return PartialView("_ContatosUsuario", contatos);
        }

        #region Criar
        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Criar(UsuarioModel usuarioModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _usuario.Adicionar(usuarioModel);
                    TempData["MensagemSucesso"] = "Contato cadastrado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                return View(usuarioModel);
            }
            catch (Exception error)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar seu usuario, tente novamente, detalhe do erro: {error.Message}";
                return RedirectToAction(nameof(Index));

            }
        }
        #endregion

        #region Eitar
        [HttpGet]
        public IActionResult Editar(int? id)
        {
            var usuario = _usuario.GetId(id);
            return View(usuario);
        }

   
        public IActionResult Alterar(UsuarioSemSenhaModel  usuarioSemSenhaModel)
        {

            try
            {
                UsuarioModel usuario = null;

                if (ModelState.IsValid)
                {
                    usuario = new UsuarioModel()
                    {
                        Id = usuarioSemSenhaModel.Id,
                        Nome = usuarioSemSenhaModel.Nome,
                        Login = usuarioSemSenhaModel.Login,
                        Email = usuarioSemSenhaModel.Email,
                        Perfil = usuarioSemSenhaModel.Perfil,
                    };
                    usuario = _usuario.Atualizar(usuario);
                    TempData["MensagemSucesso"] = "Usuário alterado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                return View("Editar", usuario);
            }
            catch (Exception error)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos atualizar seu usuário, tente novamente, detalhe do erro: {error.Message}";
                return RedirectToAction(nameof(Index));

            }
        }
        #endregion

        #region Excluir
        [HttpGet]
        public IActionResult ApagarConfirmacao(int? id)
        {
            var usuario = _usuario.GetId(id);
            return View(usuario);
        }

    
        public IActionResult Apagar(int? id)
        {
            try
            {
                bool apagar = _usuario.Remove(id);
                if (apagar)
                {
                    TempData["MensagemSucesso"] = "Usuário excluido com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não conseguimos excluir seu usuário, tente novamente!";

                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception error)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos excluir seu usuário, tente novamente, detalhe do erro: {error.Message}";
                return RedirectToAction(nameof(Index));

            }

        }
        #endregion

    }
}
