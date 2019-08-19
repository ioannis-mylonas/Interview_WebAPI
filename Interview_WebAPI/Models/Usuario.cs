using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interview_WebAPI.Models
{
    public class Usuario
    {
        // cliente.detalhes.cliente_id
        public int id { get; set; }

        // cliente.detalhes.cliente_nome
        public string nome { get; set; }

        // cliente.detalhes.cliente_email
        public string email { get; set; }

        // cliente.detalhes.cliente_telefone
        public string telefone { get; set; }

        // cliente.detalhes.cliente_nascimento
        public DateTime nascimento { get; set; }

        // Cadastro em um curso por vez:
        // Parte da chave primária em cliente.cursos
        public int curso_id { get; set; }
    }
}