using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimal_api.Domain.ModelViews
{
    // struct: para tipos pequenos, imutáveis e que representam valores (ex.: Point, DateTime)
    public struct Home
    {
        public string Mensagem { get => "Bem vindo a API de veículos - Minimal API"; }
        public string Doc { get => "/swagger"; }
    }
}