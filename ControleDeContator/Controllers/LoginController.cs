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
    public class LoginController : Controller
    {
        private readonly IUsuarioRepository _usuario;
        private readonly ISessao _sessao;
        private readonly IEmail _email;

        public LoginController(IUsuarioRepository usuario, ISessao sessao, IEmail email)
        {
            _usuario = usuario;
            _sessao = sessao;
            _email = email;
        }
      
        [HttpGet]
        public IActionResult Index()
        {
            //Se o usuario estiver logado redirecionar para homer
            if(_sessao.BuscasrSessaoDoUsuario() != null) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Entrar (LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuario.BuscarPorLogin(loginModel.Login);



                    if (usuario != null )
                    {
                        if (usuario.SenhaValida(loginModel.Senha))
                        {
                            _sessao.CriarSessaoDoUsuario(usuario);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["MensagemErro"] = $"Senha inválida. Por favor tente novamente";

                        }
                    }
                    else
                    {
                        TempData["MensagemErro"] = $"Usuario  inválido. Por favor tente novamente";

                    }


                }
                return View("Index");
            }
            catch (Exception error)
            {
                TempData["MensagemErro"] = $"Ops, não foi possivel realizar seu login,tente novamente detalhe do erro: {error.Message}";
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpGet]
        public IActionResult RedefinirSenha()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EnviarLinkParaRedefiniSenha(RedefinirSenhaModel redefinirSenhaModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuario.BuscarPorEmailELogin(redefinirSenhaModel.Email, redefinirSenhaModel.Login);



                    if (usuario != null)
                    {
                        string novaSenha = usuario.GerarNovaSenha();
                        string mensagem = $"Sua nova senha é {novaSenha}";

                        bool emailEnviado = _email.Enviar(usuario.Email,"Sistemas de Contatos - Nova Senha", mensagem );
                        if (emailEnviado)
                        {
                            _usuario.Atualizar(usuario);
                            TempData["MensagemSucesso"] = $"Enviamos para seu e-mail cadastrado uma nova senha";

                        }
                        else
                        {
                            TempData["MensagemErro"] = $"Não foi possivel enviar o e-mail. Por favor, tente novamente.";

                        }

                        return RedirectToAction("Index","Login");
                    }

                    TempData["MensagemErro"] = $"Não foi possivel redefinir sua senha. Por favor verifique os dados informados.";

                }
                return View("Index");
            }
            catch (Exception error)
            {
                TempData["MensagemErro"] = $"Ops, não foi possivel redefinir sua senha. Por favor tente novamente, detalhe do erro: {error.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoDoUsuario();
            return RedirectToAction("Index", "Login");
        }
    }
}
