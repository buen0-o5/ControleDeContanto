using ControleDeContator.Data;
using ControleDeContator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeContator.Repository
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly BancoContext _context;

        public ContatoRepository(BancoContext context)
        {
            _context = context;
        }
        public ContatoModel GetId(int? id)
        {
            return _context.Contatos.FirstOrDefault(x => x.Id == id);
        }
        public List<ContatoModel> BuscarTodos(int usuarioId)
        {
            return _context.Contatos.Where(x => x.UsuarioId == usuarioId).ToList();
        }
        public ContatoModel Adicionar(ContatoModel contato)
        {
            _context.Contatos.Add(contato);
            _context.SaveChanges();
            return contato;
        }
        public bool Remove(int? id)
        {
            ContatoModel contatoDb = GetId(id);
            if (contatoDb == null) throw new System.Exception("Houve um erro ao tentar excluir o contato!");
            
            _context.Contatos.Remove(contatoDb);
            _context.SaveChanges();
 
            return true;
        }

        public ContatoModel Atualizar(ContatoModel contato)
        {
            ContatoModel contatoDb = GetId(contato.Id);
            if (contatoDb == null) throw new System.Exception("Houve um erro na atualizaçao do contato!");

            contatoDb.Nome = contato.Nome;
            contatoDb.Email = contato.Email;
            contatoDb.Celular = contato.Celular;

            _context.Contatos.Update(contatoDb);
            _context.SaveChanges();

            return contatoDb;
        }
    }
}
