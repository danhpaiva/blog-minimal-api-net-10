# 📰 Blog Minimal API – ASP.NET Core .NET 10

![.NET](https://img.shields.io/badge/.NET_10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-6DB33F?style=for-the-badge)
![JWT](https://img.shields.io/badge/JWT_Auth-000000?style=for-the-badge&logo=jsonwebtokens)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

API REST para gerenciamento de um **Blog**, desenvolvida com **ASP.NET Core (.NET 10)**, utilizando **Entity Framework Core**, **JWT Authentication**, **SQL Server em Docker**, cache em memória e boas práticas de arquitetura.

Projeto com foco em **segurança, organização, performance e padronização de APIs REST**.

---

## 🚀 Tecnologias Utilizadas

- **.NET 10 / ASP.NET Core**
- **C#**
- **Entity Framework Core**
- **SQL Server 2022 (Docker)**
- **JWT Bearer Authentication**
- **Swagger / OpenAPI**
- **Injeção de Dependência**
- **Memory Cache**
- **Docker**
- **SMTP (SendGrid)**

---

## 📦 Repositório

🔗 https://github.com/danhpaiva/blog-minimal-api-net-10

---

## 🔐 Autenticação (JWT)

A API utiliza autenticação via **JWT Bearer Token**.

### 🔑 Login

```
POST /v1/accounts/login
```

#### Payload

```json
{
  "email": "pparker@balta.io",
  "password": "1234"
}
```

#### Resposta

```json
{
  "data": "<JWT_TOKEN>"
}
```

Utilize o token nas requisições protegidas:

```
Authorization: Bearer <token>
```

---

## 👤 Contas (Accounts)

### ➕ Criar Usuário

```
POST /v1/accounts
```

```json
{
  "email": "usuario@email.com",
  "name": "Nome do Usuário"
}
```

> A senha é gerada automaticamente e enviada por e-mail (SMTP).

---

### 🖼️ Upload de Imagem (Usuário Logado)

```
POST /v1/accounts/upload-image
```

```json
{
  "base64Image": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQ..."
}
```

---

## 🗂️ Categorias

### 📄 Listar Categorias

```
GET /v1/categories
```

---

### 🔍 Buscar Categoria por ID

```
GET /v1/categories/{id}
```

---

### ➕ Criar Categoria

```
POST /v1/categories
```

```json
{
  "name": "Backend",
  "slug": "backend"
}
```

---

### ✏️ Atualizar Categoria

```
PUT /v1/categories/{id}
```

---

### ❌ Excluir Categoria

```
DELETE /v1/categories/{id}
```

---

## 📝 Posts

### 📄 Listar Posts (Paginado)

```
GET /v1/posts?page=0&pageSize=25
```

---

### 🔍 Detalhes do Post

```
GET /v1/posts/{id}
```

---

### 🗂️ Posts por Categoria

```
GET /v1/posts/category/{slug}
```

---

## 🐳 Banco de Dados (SQL Server via Docker)

### 📥 Pull da Imagem

```bash
docker pull mcr.microsoft.com/mssql/server:2022-latest
```

### ▶️ Executar Container

```bash
docker run --name sqlserver   -e "ACCEPT_EULA=Y"   -e "MSSQL_SA_PASSWORD=1q2w3e4r@#$"   -p 1433:1433   -d mcr.microsoft.com/mssql/server:2022-latest
```

### 🔗 Connection String

```
Server=localhost,1433;
Database=BlogApi;
User ID=sa;
Password=1q2w3e4r@#$;
Trusted_Connection=False;
TrustServerCertificate=True;
```

---

## ⚙️ Configuração (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=BlogApi;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True;"
  },
  "JwtKey": "C040542A77BC4E4A8AFAA389BFED7D70abcd",
  "Smtp": {
    "Host": "smtp.sendgrid.net",
    "Port": "587",
    "Username": "apikey",
    "Password": "senhaGeradaNaPlataforma"
  }
}
```

---

## 📚 Seed de Dados

O projeto inclui scripts SQL para carga inicial de:

- Categorias
- Tags
- Usuários
- Roles
- Posts

Facilitando testes e aprendizado.

---

## 🌐 Swagger

Disponível em ambiente de desenvolvimento:

```
https://localhost:{porta}/swagger
```

---

## ▶️ Executar Localmente

```bash
git clone https://github.com/danhpaiva/blog-minimal-api-net-10
cd blog-minimal-api-net-10
dotnet run
```

---

## 📄 Licença

Este projeto está licenciado sob a licença MIT.

🔗 https://github.com/danhpaiva/blog-minimal-api-net-10/blob/main/LICENSE

---

## 👨‍💻 Autor

**Daniel Paiva**  
Desenvolvedor .NET

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/danhpaiva/)
