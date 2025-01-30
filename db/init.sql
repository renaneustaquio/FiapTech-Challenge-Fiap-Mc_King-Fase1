CREATE TABLE clientes
(
  codigo SERIAL PRIMARY KEY,
  cpf varchar(11) NOT NULL,
  nome varchar(150) NOT NULL,
  email varchar(200) NOT NULL,
  CONSTRAINT cpf UNIQUE(cpf),
  CONSTRAINT email UNIQUE(email)
);

CREATE TABLE produtos (
    codigo SERIAL PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    descricao TEXT,
    preco NUMERIC(10, 2) NOT NULL,
    categoria INTEGER NOT NULL,
    situacao INTEGER NOT NULL
);

CREATE TABLE pedidos (
    codigo SERIAL PRIMARY KEY,
    codigo_cliente INT REFERENCES clientes(codigo) ON DELETE SET NULL
);

CREATE TABLE pedido_combos (
    codigo SERIAL PRIMARY KEY,
    codigo_pedido INT REFERENCES pedidos(codigo) ON DELETE CASCADE NOT NULL,
    preco FLOAT NOT NULL
);

CREATE TABLE pedido_combo_items (
    codigo SERIAL PRIMARY KEY,
    pedido_combo_codigo INT REFERENCES pedido_combos(codigo) ON DELETE CASCADE NOT NULL,
    codigo_produto INT REFERENCES produtos(codigo),
    preco FLOAT NOT NULL
);

CREATE TABLE pedido_pagamento (
    codigo SERIAL PRIMARY KEY,
    codigo_pedido INT REFERENCES pedidos(codigo) ON DELETE CASCADE NOT NULL,
    valor DECIMAL(10, 2) NOT NULL,
    forma_pagamento VARCHAR(50),
    data_pagamento TIMESTAMP
);

CREATE TABLE pedido_status (
    codigo SERIAL PRIMARY KEY,
    codigo_pedido INT REFERENCES pedidos(codigo) ON DELETE CASCADE NOT NULL,
    status int NOT NULL,
    data_criacao TIMESTAMP DEFAULT NOW() NOT NULL
);

INSERT INTO "produtos" ("nome", "descricao", "preco", "categoria", "situacao")
VALUES
('Mc King Feliz', 'Hambúrguer, Queijo Prato, Ketchup e Brinquedo', 24.90, 1, 1),
('Mc King', '2 Hambúrguer, Queijo Prato, Molho MC King especial, Alface, Cebola e Picles', 31.90, 1, 1),
('Mc Cheddar Duplo', '2 Hambúrguer, 2 Queijo Prato, Molho Cheddar, Ketchup, Mostarda e Picles', 29.90, 1, 1),
('Batata pequena', 'Batata frita pequena. Aprox. 100g', 9.90, 2, 1),
('Batata média', 'Batata frita média. Aprox. 200g', 11.90, 2, 1),
('Batata grande', 'Batata frita grande. Aprox. 300g', 13.90, 2, 1),
('Mc King Free Refil', 'Um refrigerante a sua escolha. Free refil', 14.90, 3, 1),
('Suco', 'Suco de laranja ou uva', 15.90, 3, 1),
('Casquinha', 'Casquinha. Sabores: Baunilha, Chocolate ou Mista', 3.50, 4, 1),
('Sundae', 'Sundae. Sabores: Doce de leite, Chocolate ou Morango', 8.90, 4, 1),
('Milk-Shake', 'Milk-Shake. Sabores: Doce de leite, Crocante, Chocolate ou Morango', 16.90, 4, 1);


INSERT INTO "clientes" ("cpf", "nome", "email") VALUES
('39592262098',	'João Pedro',	'jp@gmail.com'),
('49783597086',	'Maria Tereza',	'm.tereza@hotmail.com');

INSERT INTO "pedidos" ("codigo_cliente") VALUES
(NULL),
(1),
(2);

INSERT INTO "pedido_status" (codigo_pedido, "status", "data_criacao") VALUES
(3,	1,	'2024-11-01 21:42:08.12827'),
(1,	1,	'2024-11-01 21:41:42.681931'),
(2,	1,	'2024-11-01 21:41:50.691848'),
(2,	2,	'2024-11-01 21:41:59.801704'),
(1, 2,	'2024-11-01 21:44:26.020703'),
(1,	4,	'2024-11-01 21:44:45.576766');

INSERT INTO "pedido_pagamento" (codigo_pedido, "valor", "forma_pagamento", "data_pagamento") VALUES
(1,	50.7,	'1',	'2024-11-01 21:45:27.806018'),
(2,	11.9,	'1',	'2024-11-01 21:45:51.00446');


INSERT INTO "pedido_combos" (codigo_pedido, "preco") VALUES
(1,	50.7),
(2,	11.9),
(3,	50.7),
(3,	11.9);


INSERT INTO "pedido_combo_items" ("pedido_combo_codigo", "codigo_produto", "preco") VALUES
(1,	1,	24.9),
(1,	4,	9.9),
(1,	8,	15.9),
(2,	5,	11.9),
(3,	5,	11.9),
(4,	3,	29.9),
(4,	5,	11.9),
(4,	10,	8.9);