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
