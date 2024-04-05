using ControleDeContator.Data;
using ControleDeContator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeContator.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly BancoContext _context;

        public UsuarioRepository(BancoContext context)
        {
            _context = context;
        }

        public UsuarioModel BuscarPorLogin(string login)
        {
            return _context.Usuarios.FirstOrDefault(x => x.Login.ToUpper() == login.ToUpper());
        }

        public UsuarioModel BuscarPorEmailELogin(string email, string login)
        {
            return _context.Usuarios.FirstOrDefault(x => x.Email.ToUpper() == email.ToUpper() &&  x.Login.ToUpper() == login.ToUpper());
        }

        public List<UsuarioModel> BuscarTodos()
        {
            return _context.Usuarios
                .Include(x => x.Contatos)
                .ToList();
        }

        public UsuarioModel GetId(int? id)
        {
            return _context.Usuarios.FirstOrDefault(x => x.Id == id);
        }


        public UsuarioModel Adicionar(UsuarioModel usuario)
        {
            usuario.DataCadastro = DateTime.Now;
            usuario.SetSenhaHash();
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return usuario;
        }

        public UsuarioModel Atualizar(UsuarioModel usuario)
        {
            UsuarioModel usuarioDb = GetId(usuario.Id);
            if (usuarioDb == null) throw new System.Exception("Houve um erro na atualizaçao do usuario!");

            usuarioDb.Nome = usuario.Nome;
            usuarioDb.Email = usuario.Email;
            usuarioDb.Login = usuario.Login;
            usuarioDb.Perfil = usuario.Perfil;
            usuarioDb.DataAlterado = DateTime.Now;

            _context.Usuarios.Update(usuarioDb);
            _context.SaveChanges();

            return usuarioDb;

        }
        public UsuarioModel AlterarSenha(AlterarSenhaModel alterarSenhaModel)
        {
            UsuarioModel usuarioDb = GetId(alterarSenhaModel.Id);

            if (usuarioDb == null) throw new System.Exception("Houve um erro ao tentar atualizar a senha, usuario não encontrado");

            if(!usuarioDb.SenhaValida(alterarSenhaModel.SenhaAtual)) throw new System.Exception("Senha atual não confere");

            if (usuarioDb.SenhaValida(alterarSenhaModel.NovaSenha)) throw new Exception("Nova senha deve ser diferente da senha atual!");


            usuarioDb.SetNovaSenha(alterarSenhaModel.NovaSenha);
            usuarioDb.DataAlterado = DateTime.Now;

            _context.Usuarios.Update(usuarioDb);
            _context.SaveChanges();

            return usuarioDb;
        }


        public bool Remove(int? id)
        {
            UsuarioModel usuarioDb = GetId(id);
            if (usuarioDb == null) throw new System.Exception("Houve um erro ao tentar excluir o usuario!");

            _context.Usuarios.Remove(usuarioDb);
            _context.SaveChanges();

            return true;
        }

 
    }
}
