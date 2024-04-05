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
    public class AlterarSenhaController : Controller
    {
        private readonly IUsuarioRepository _usuario;
        private readonly ISessao _sessao;

        public AlterarSenhaController(IUsuarioRepository usuario, ISessao sessao)
        {
            _usuario = usuario;
            _sessao = sessao;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Alterar(AlterarSenhaModel alterarSenhaModel)
        {
            try
            {
                //Atribuindo o Id do usuario ao usuario alterado 
                UsuarioModel usuariologado =  _sessao.BuscasrSessaoDoUsuario();
                alterarSenhaModel.Id = usuariologado.Id;
                
                if(ModelState.IsValid)
                {
                    _usuario.AlterarSenha(alterarSenhaModel);
                    TempData["MensagemSucesso"] = "Senha alterada com sucesso!";
                    return View("Index", alterarSenhaModel);

                }
                return View("Index", alterarSenhaModel);
            }
            catch(Exception error)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos alterar sua senha, tente novamente, detalhe do erro: {error.Message}";
                return View("Index", alterarSenhaModel);
            }
        }
    
    }
}
