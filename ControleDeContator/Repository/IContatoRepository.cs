using ControleDeContator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeContator.Repository
{
    public interface IContatoRepository
    {
        ContatoModel GetId(int? id);
        List<ContatoModel> BuscarTodos(int usuarioId);
        ContatoModel Adicionar(ContatoModel contato);
        ContatoModel Atualizar(ContatoModel contato);
        bool Remove(int? id);

    }
}
