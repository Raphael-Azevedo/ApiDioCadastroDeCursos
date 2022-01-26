using System.Collections.Generic;

namespace Curso.api.Models
{
    public class ValidaCampoViewModelOutput
    {
        public IEnumerable<string> Erros { get; private set; }

        public ValidaCampoViewModelOutput(IEnumerable<string> erro)
        {
            Erros = erro;
        }
    }
}
