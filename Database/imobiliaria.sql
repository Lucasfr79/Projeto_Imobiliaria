create database imobiliaria;
use imobiliaria;

create table usuario (
    id int auto_increment primary key,
    nome varchar(100) not null,
    email varchar(100) not null unique,
    senha varchar(100) not null,
    telefone varchar(20),
    tipo ENUM('cliente', 'proprietario') not null
);

create table imovel (
    id int auto_increment primary key,
    titulo varchar(150) not null,
    descricao text,
    preco decimal(10,2) not null,
    cidade varchar(100),
    endereco varchar(150),
    numero_quartos int,
    id_proprietario int,
    foreign key (id_proprietario) references usuario(id)
);

create table reservas (
    id int auto_increment primary key,
    data_inicio date not null,
    data_fim date not null,
    id_usuario int,
    id_imovel int,
    foreign key (id_usuario) references usuario(id),
    foreign key (id_imovel) references imovel(id)
);

create table avaliacoes (
    id int auto_increment primary key,
    nota int check (nota >= 1 and nota <=5),
    comentario text,
    id_usuario int,
    id_imovel int,
    foreign key (id_usuario) references usuario(id),
    foreign key (id_imovel) references imovel(id)
);


insert into usuario (nome, email, senha, telefone, tipo) values
('Arthur', 'arthur@gmail.com', '123456', '41999999999', 'cliente'),
('João', 'joao@gmail.com', '123456', '41988888888', 'proprietario');

insert into imovel (titulo, descricao, preco, cidade, endereco, numero_quartos, id_proprietario) values
('Apartamento no Centro', 'bem localizado', 1500.00, 'Curitiba', 'Rua x', 2, 2);

insert into reservas (data_inicio, data_fim, id_usuario, id_imovel) values
('2026-01-01', '2026-01-07', 1, 1);

insert into avaliacoes (nota, comentario, id_usuario, id_imovel) values
(5, 'Imovel excelente', 1, 1);

select * from avaliacoes;

