using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interview_WebAPI.Models
{
    public class Curso
    {
        // curso.detalhes.curso_id
        public int id { get; set; }

        // curso.detalhes.curso_nome
        public string nome { get; set; }

        // curso.cadastro.curso_desc
        public string desc { get; set; }

        // curso.cadastro.curso_desc_pequena
        public string desc_pequena { get; set; }

        // curso.detalhes.curso_carga_horaria
        public string carga_horaria { get; set; }

        // curso.detalhes.curso_dias_semana
        public string dias_semana { get; set; }

        // curso.detalhes.curso_inicio
        public string inicio { get; set; }

        // curso.cadastro.curso_video_intro_url
        public string intro_video_url { get; set; }

        // curso.cadastro.curso_cadastro_sucesso
        public string cadastro_sucesso { get; set; }
    }
}