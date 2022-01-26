using Curso.api.Business.Entities;
using System.Collections.Generic;

namespace Curso.api.Business.Repositories
{
    public interface ICursoRepository
    {
        void Adicionar(Cursos curso);

        void Commit();
        IList<Cursos> ObterPorUsuario(int codigoUsuario);
    }
}
