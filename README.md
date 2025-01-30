# 9soat-g65-mc-king
Tech Challenge FIAP - 9SOAT - Grupo 65

Participantes:
Mayara Manzi - RM359734

Renan Eustaquio Claudiano Martins - RM359737

## Objetivo

Permitir a expansão de uma lanchonete de bairro. A automatização do sistema visa garantir a organização dos pedidos evitando atrasos e confusão.

## Tecnologias Utilizadas

Visual studio 2022

.NET - 8.0

Postgres - 16.4

Docker desktop


## Executando o projeto

## 1. Clonar o Repositório

  

Primeiro, clone o repositório do projeto para sua máquina local e, em seguida, abra um terminal na raiz do projeto.

  
```bash
git  clone https://github.com/renaneustaquio/FiapTech-Challenge-Fiap-Mc_King-Fase1.git
```


## 2. Build o Docker-compose
Navegar até a raiz do repositório, onde se encontra o arquivo docker-compose.yml (Ex.: c:/9soat-g65-mc-king).

Execute o comando no terminal: 

```bash
 Docker-compose up --build
```


O swagger deve abrir na porta: 8080
```bash
http://localhost:8080/index.html
```

O banco irá executar na porta: 5432
```bash
http://localhost:5432/
```

A administração do banco poderá ser feita na porta:9090
```bash
http://localhost:9090/
```

Selecionar o sistema: PostgreSQL

Servidor: postgres

Usuário: postgres

Senha: postgres

Base de dados: mc_king


Alguns registros já foram pré-imputados para facilitar os testes.
