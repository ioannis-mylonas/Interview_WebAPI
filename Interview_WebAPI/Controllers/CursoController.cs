using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using System.Data.SqlClient;

using Interview_WebAPI.Models;

namespace Interview_WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class CursoController : ApiController
    {
        // GET: api/Curso
        [ResponseType(typeof(string))]
        public string Get()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = ".";
                builder.InitialCatalog = "interview_db";
                builder.IntegratedSecurity = true;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    StringBuilder cmd_str = new StringBuilder();
                    cmd_str.Append("SELECT curso.detalhes.curso_id AS id, ");
                    cmd_str.Append("curso.detalhes.curso_nome AS nome, ");
                    cmd_str.Append("curso.detalhes.curso_carga_horaria AS carga_horaria, ");
                    cmd_str.Append("curso.detalhes.curso_dias_semana AS dias_semana, ");
                    cmd_str.Append("curso.detalhes.curso_inicio AS inicio, ");
                    cmd_str.Append("curso.cadastro.curso_desc AS 'desc', ");
                    cmd_str.Append("curso.cadastro.curso_desc_pequena AS desc_pequena, ");
                    cmd_str.Append("curso.cadastro.curso_video_intro_url AS video_intro_url, ");
                    cmd_str.Append("curso.cadastro.curso_cadastro_sucesso AS cadastro_sucesso ");
                    cmd_str.Append("FROM curso.detalhes ");
                    cmd_str.Append("JOIN curso.cadastro ON ");
                    cmd_str.Append("curso.detalhes.curso_id = curso.cadastro.curso_id ");
                    cmd_str.Append("FOR JSON PATH");

                    connection.Open();
                    SqlCommand command = new SqlCommand(cmd_str.ToString(), connection);
                    SqlDataReader reader = command.ExecuteReader();
                    StringBuilder sb = new StringBuilder();
                    while (reader.Read())
                    {
                        sb.Append(reader.GetString(0));
                    }
                    return sb.ToString();
                }
            }
            catch (SqlException e)
            {
                var serializer = new JavaScriptSerializer();
                return serializer.Serialize(e.ToString());
            }
        }

        // GET: api/Curso/5
        public IHttpActionResult Get(int id)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = ".";
                builder.InitialCatalog = "interview_db";
                builder.IntegratedSecurity = true;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    StringBuilder cmd_str = new StringBuilder();
                    cmd_str.Append("SELECT curso.detalhes.curso_id AS id, ");
                    cmd_str.Append("curso.detalhes.curso_nome AS nome, ");
                    cmd_str.Append("curso.detalhes.curso_carga_horaria AS carga_horaria, ");
                    cmd_str.Append("curso.detalhes.curso_dias_semana AS dias_semana, ");
                    cmd_str.Append("curso.detalhes.curso_inicio AS inicio, ");
                    cmd_str.Append("curso.cadastro.curso_desc AS 'desc', ");
                    cmd_str.Append("curso.cadastro.curso_desc_pequena AS desc_pequena, ");
                    cmd_str.Append("curso.cadastro.curso_video_intro_url AS video_intro_url, ");
                    cmd_str.Append("curso.cadastro.curso_cadastro_sucesso AS cadastro_sucesso ");
                    cmd_str.Append("FROM curso.detalhes ");
                    cmd_str.Append("JOIN curso.cadastro ON ");
                    cmd_str.Append("curso.detalhes.curso_id = curso.cadastro.curso_id ");
                    cmd_str.Append("WHERE curso.detalhes.curso_id = @id ");
                    cmd_str.Append("FOR JSON PATH, WITHOUT_ARRAY_WRAPPER");

                    connection.Open();
                    SqlCommand command = new SqlCommand(cmd_str.ToString(), connection);
                    SqlParameter id_param = command.Parameters.Add("@id", System.Data.SqlDbType.Int);
                    id_param.Value = id;

                    SqlDataReader reader = command.ExecuteReader();
                    StringBuilder sb = new StringBuilder();
                    while (reader.Read())
                    {
                        sb.Append(reader.GetString(0));
                    }

                    if (sb.Length == 0)
                    {
                        return NotFound();
                    }
                    return Ok(sb.ToString());
                }
            }
            catch (SqlException e)
            {
                var serializer = new JavaScriptSerializer();
                return Content(HttpStatusCode.BadRequest, serializer.Serialize(e.ToString()));
            }
        }

        // POST: api/Curso
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Curso/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Curso/5
        public void Delete(int id)
        {
        }
    }
}
