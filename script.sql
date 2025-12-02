CREATE DATABASE dbSistemaPitaya; 
USE dbSistemaPitaya;

-- Usuários do sistema
CREATE TABLE tbUsuario (
    CodUsu INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    NomeUsu VARCHAR(50) NOT NULL,
    SenhaUsu INT NOT NULL
);

-- Produtos cadastrados por usuários
CREATE TABLE tbProduto (
    CodProd INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    NomeProd VARCHAR(50) NOT NULL,
    DescProd VARCHAR(200),
    PrecoProd DECIMAL(10,2) NOT NULL,
    CodUsu INT NOT NULL,                   -- quem cadastrou o produto
    FOREIGN KEY (CodUsu) REFERENCES tbUsuario(CodUsu)
);

-- Pedidos registrados por usuários
CREATE TABLE tbPedido (
    CodPed INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    NomeProd VARCHAR(50) NOT NULL,         -- usado só para exibir ao usuário
    CodProd INT NOT NULL,                  -- FK para produto
    QtdeProd INT NOT NULL,
    PrecoProd DECIMAL(10,2) NOT NULL,
    CodUsu INT NOT NULL,                   -- quem fez o pedido
    FOREIGN KEY (CodProd) REFERENCES tbProduto(CodProd),
    FOREIGN KEY (CodUsu) REFERENCES tbUsuario(CodUsu)
);


