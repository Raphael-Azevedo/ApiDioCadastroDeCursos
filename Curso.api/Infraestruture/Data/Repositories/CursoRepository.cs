﻿using Curso.api.Business.Entities;
using Curso.api.Business.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Curso.api.Infraestruture.Data.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly CursoDbContext _contexto;
        public CursoRepository(CursoDbContext contexto)
        {
            _contexto = contexto;
        }
        public void Adicionar(Cursos curso)
        {
            _contexto.Curso.Add(curso);
        }

        public void Commit()
        {
            _contexto.SaveChanges();
        }

        public IList<Cursos> ObterPorUsuario(int codigoUsuario)
        {
            return _contexto.Curso.Include(i => i.Usuario).Where(w => w.CodigoUsuario == codigoUsuario).ToList();
        }
    }
}
