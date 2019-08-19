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
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

using Interview_WebAPI.Models;

namespace Interview_WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UsuarioController : ApiController
    {
        private enum Erro { NONE, EXISTS, MISSING_ARG, INVALID_ARG };
        private const int IDADE_MINIMA = 18;

        // GET: api/Usuario
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
                    cmd_str.Append("SELECT cliente.detalhes.cliente_id AS id, ");
                    cmd_str.Append("cliente.detalhes.cliente_nome AS nome, ");
                    cmd_str.Append("cliente.detalhes.cliente_email AS email, ");
                    cmd_str.Append("cliente.detalhes.cliente_telefone AS telefone, ");
                    cmd_str.Append("cliente.detalhes.cliente_nascimento AS nascimento, ");
                    cmd_str.Append("(SELECT curso_id FROM cliente.cursos ");
                    cmd_str.Append("WHERE cliente.cursos.cliente_id = cliente.detalhes.cliente_id ");
                    cmd_str.Append("FOR JSON PATH) AS cursos ");
                    cmd_str.Append("FROM cliente.detalhes ");
                    cmd_str.Append("LEFT JOIN cliente.cursos ON ");
                    cmd_str.Append("cliente.cursos.cliente_id = cliente.detalhes.cliente_id ");
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

        // GET: api/Usuario/5
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
                    cmd_str.Append("SELECT cliente.detalhes.cliente_id AS id, ");
                    cmd_str.Append("cliente.detalhes.cliente_nome AS nome, ");
                    cmd_str.Append("cliente.detalhes.cliente_email AS email, ");
                    cmd_str.Append("cliente.detalhes.cliente_telefone AS telefone, ");
                    cmd_str.Append("cliente.detalhes.cliente_nascimento AS nascimento, ");
                    cmd_str.Append("(SELECT curso_id FROM cliente.cursos ");
                    cmd_str.Append("WHERE cliente.cursos.cliente_id = cliente.detalhes.cliente_id ");
                    cmd_str.Append("FOR JSON PATH) AS cursos ");
                    cmd_str.Append("FROM cliente.detalhes ");
                    cmd_str.Append("LEFT JOIN cliente.cursos ON ");
                    cmd_str.Append("cliente.cursos.cliente_id = cliente.detalhes.cliente_id ");
                    cmd_str.Append("WHERE cliente.detalhes.cliente_id = @id ");
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

        // POST: api/Usuario
        public IHttpActionResult Post([FromBody]JObject value)
        {
            var serializer = new JavaScriptSerializer();
            Erro resultado = Erro.NONE;
            Usuario usuario = valida_usuario(value, ref resultado);
            if (usuario == null) { return Ok(resultado); }

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = ".";
                builder.InitialCatalog = "interview_db";
                builder.IntegratedSecurity = true;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    StringBuilder check_str = new StringBuilder();
                    check_str.Append("SELECT cliente_email ");
                    check_str.Append("FROM cliente.detalhes ");
                    check_str.Append("WHERE cliente_email = @email ");
                    check_str.Append("FOR JSON PATH, WITHOUT_ARRAY_WRAPPER");

                    StringBuilder check_curso_str = new StringBuilder();
                    check_curso_str.Append("SELECT cliente.cursos.curso_id as curso_id ");
                    check_curso_str.Append("FROM cliente.detalhes ");
                    check_curso_str.Append("JOIN cliente.cursos ON ");
                    check_curso_str.Append("cliente.detalhes.cliente_id = cliente.cursos.cliente_id ");
                    check_curso_str.Append("WHERE cliente.detalhes.cliente_email = @email ");
                    check_curso_str.Append("FOR JSON PATH");

                    StringBuilder input_str = new StringBuilder();
                    input_str.Append("INSERT INTO cliente.detalhes (cliente_nome, cliente_email, ");
                    input_str.Append("cliente_nascimento, cliente_telefone) ");
                    input_str.Append("VALUES (@nome, @email, @nascimento, @telefone)");

                    StringBuilder input_curso_str = new StringBuilder();
                    input_curso_str.Append("INSERT INTO cliente.cursos (cliente_id, curso_id) ");
                    input_curso_str.Append("VALUES (@cliente_id, @curso_id)");

                    connection.Open();
                    SqlCommand command = new SqlCommand(check_str.ToString(), connection);
                    SqlParameter email = command.Parameters.Add("@email", System.Data.SqlDbType.VarChar);
                    email.Value = usuario.email;
                    SqlDataReader reader = command.ExecuteReader();
                    StringBuilder sb = new StringBuilder();
                    while (reader.Read())
                    {
                        sb.Append(reader.GetString(0));
                    }
                    reader.Close();

                    if (sb.Length == 0)
                    {
                        SqlCommand input_command = new SqlCommand(input_str.ToString(), connection);
                        SqlParameter input_nome = input_command.Parameters.Add("@nome", System.Data.SqlDbType.VarChar);
                        input_nome.Value = usuario.nome;
                        SqlParameter input_email = input_command.Parameters.Add("@email", System.Data.SqlDbType.VarChar);
                        input_email.Value = usuario.email;
                        SqlParameter input_telefone = input_command.Parameters.Add("@telefone", System.Data.SqlDbType.VarChar);
                        input_telefone.Value = (object)usuario.telefone ?? DBNull.Value;
                        SqlParameter input_nascimento = input_command.Parameters.Add("@nascimento", System.Data.SqlDbType.Date);
                        input_nascimento.Value = usuario.nascimento.Date;

                        input_command.ExecuteNonQuery();
                        return Ok(serializer.Serialize(resultado));
                    }

                    command = new SqlCommand(check_curso_str.ToString(), connection);
                    email = command.Parameters.Add("@email", System.Data.SqlDbType.VarChar);
                    email.Value = usuario.email;
                    reader = command.ExecuteReader();
                    sb = new StringBuilder();
                    while (reader.Read())
                    {
                        sb.Append(reader.GetString(0));
                    }
                    reader.Close();

                    if (sb.Length > 0)
                    {
                        JArray result = JArray.Parse(sb.ToString());
                        foreach (JObject result_obj in result.Children<JObject>())
                        {
                            foreach (JProperty result_property in result_obj.Properties())
                            {
                                if (result_property.Name == "curso_id" && (int)result_property.Value == usuario.curso_id) {
                                    resultado = Erro.EXISTS;
                                    return Ok(serializer.Serialize(resultado));
                                }
                            }
                        }
                    }

                    int cliente_id = get_cliente_id(connection, usuario.email);
                    command = new SqlCommand(input_curso_str.ToString(), connection);
                    SqlParameter cliente_id_param = command.Parameters.Add("@cliente_id", System.Data.SqlDbType.Int);
                    cliente_id_param.Value = cliente_id;
                    SqlParameter curso_id_param = command.Parameters.Add("@curso_id", System.Data.SqlDbType.Int);
                    curso_id_param.Value = usuario.curso_id;

                    command.ExecuteNonQuery();
                    return Ok(serializer.Serialize(resultado));
                }
            }
            catch (SqlException e)
            {
                return Content(HttpStatusCode.BadRequest, serializer.Serialize(e.ToString()));
            }
        }

        // Busca cliente em cliente.detalhes por email e retorna id. Retorna -1 se não encontrado.
        private int get_cliente_id(SqlConnection connection, string email)
        {
            StringBuilder get_cliente_id_str = new StringBuilder();
            get_cliente_id_str.Append("SELECT cliente.detalhes.cliente_id as id ");
            get_cliente_id_str.Append("FROM cliente.detalhes ");
            get_cliente_id_str.Append("WHERE cliente.detalhes.cliente_email = @email ");
            get_cliente_id_str.Append("FOR JSON PATH");

            SqlCommand input_command = new SqlCommand(get_cliente_id_str.ToString(), connection);
            SqlParameter input_nome = input_command.Parameters.Add("@email", System.Data.SqlDbType.VarChar);
            input_nome.Value = email;

            SqlDataReader reader = input_command.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            while (reader.Read())
            {
                sb.Append(reader.GetString(0));
            }
            reader.Close();

            JArray result = JArray.Parse(sb.ToString());
            foreach (JObject result_obj in result.Children<JObject>())
            {
                foreach (JProperty result_property in result_obj.Properties())
                {
                    Debug.WriteLine(result_property.Name + " | " + (string)result_property.Value);
                    if (result_property.Name == "id") { return (int)result_property.Value; }
                }
            }

            return -1;
        }

        // Valida endereço de email usando EmailAddressAttribute.
        private bool valida_email(string input)
        {
            return new EmailAddressAttribute().IsValid(input);
        }

        // Valida telefone brasileiro usando libphonenumber-csharp.
        private bool valida_telefone(string input)
        {
            if(string.IsNullOrEmpty(input)) { return true; }
            var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
            var number = phoneNumberUtil.Parse(input, "BR");
            return phoneNumberUtil.IsValidNumber(number);
        }

        // Validação básica de idade. Pela natureza do negócio, aceita
        // quem estiver fazendo aniversário no mesmo ano.
        private bool valida_idade(DateTime nascimento)
        {
            var atual = DateTime.Today;
            return (atual.Year - nascimento.Year >= IDADE_MINIMA);
        }

        // Verifica campos que estejam faltando, modifica referência para erro
        // e valida campos antes de retornar objeto Usuario.
        private Usuario valida_usuario(JObject input, ref Erro resultado)
        {
            if (input["nome"] == null) { resultado = Erro.MISSING_ARG; return null; }
            if (input["email"] == null) { resultado = Erro.MISSING_ARG; return null; }
            if (input["nascimento"] == null) { resultado = Erro.MISSING_ARG; return null; }

            Usuario usuario = input.ToObject<Usuario>();
            usuario.nome.ToUpper();

            if (!valida_email(usuario.email)) { resultado = Erro.INVALID_ARG; return null; }
            if (!valida_telefone(usuario.telefone)) { resultado = Erro.INVALID_ARG; return null; }
            if (!valida_idade(usuario.nascimento)) { resultado = Erro.INVALID_ARG; return null; }

            return usuario;
        }

        // PUT: api/Usuario/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Usuario/5
        public void Delete(int id)
        {
        }
    }
}
