# Interview WebAPI  
WebAPI usando ASP Net para desafio (emprego). Usa C# para se conectar a um banco de dados SQL Server padrão. Criado no Visual Studio Community Edition 2019.  
  
# Bibliotecas  
  
Instalar com o NuGet os pacotes:  
  
```
Install-Package libphonenumber-csharp  
Install-Package Microsoft.AspNet.WebApi.Cors  
```
  
# Tabelas
```
cliente.detalhes - Contém os detalhes cadastrais do cliente registrado.  
cliente.cursos - Contém id to cliente e id do curso ao qual ele se cadastrou. Permite cadastro a mais de um curso.  
  
curso.detalhes - Contém detalhes gerais sobre o curso.  
curso.cadastro - Contém detalhes específicos sobre o cadastro no curso, como url do vídeo de introdução, entre outros.  
```
  
# Campos
```
cliente.detalhes.cliente_id - INT - Chave primária para cada cliente cadastrado.  
cliente.detalhes.cliente_nome - VARCHAR - Nome do cliente.  
cliente.detalhes.cliente_email - VARCHAR - Email do cliente.  
cliente.detalhes.cliente_nascimento - DATE - Dia do nascimento do cliente, para idade.  
cliente.detalhes.cliente_telefone - VARCHAR - Telefone do cliente (opcional).  
  
cliente.cursos.cliente_id - INT - Chave estrangeira primária composta que referencia o id do cliente.  
cliente.cursos.curso_id - INT - Chave estrangeira primária composta que referencia o id do curso.  
  
curso.detalhes.curso_id - INT - Chave primária para cada curso cadastrado.  
curso.detalhes.curso_nome - VARCHAR - Nome do curso.  
curso.detalhes.curso_carga_horaria - VARCHAR - Carga horaria do curso.  
curso.detalhes.curso_dias_semana - VARCHAR - Dias da semana do curso.  
curso.detalhes.curso_inicio - DATE - Data do início do curso.  
  
curso.cadastro.curso_id - INT - Chave estrangeira que referencia o id do curso.  
curso.cadastro.curso_desc - VARCHAR - Descrição do curso.  
curso.cadastro.curso_desc_pequena - VARCHAR - Descrição pequena do curso para uso no seletor.  
curso.cadastro.curso_video_intro_url - VARCHAR - URL do vídeo de introdução ao curso.  
curso.cadastro.curso_cadastro_sucesso - VARCHAR - Mensagem de sucesso ao se cadastrar no curso.  
