insert into usuario (nome, email, senha, telefone, tipo) values
('Arthur', 'arthur@gmail.com', '123456', '41999999999', 'cliente'),
('João', 'joao@gmail.com', '123456', '41988888888', 'proprietário');

insert into imovel (titulo, descricao, preco, cidade, endereco, numero_quartos, id_proprietario) values
('Apartamento no Centro', 'bem localizado', 1500.00, 'Curitiba', 'Rua x', 2, 2);

insert into reservas (data_inicio, data_fim, id_usuario, id_imovel) values
('2026-01-01', '2026-01-07', 1, 1);

insert into avaliacoes (nota, comentario, id_usuario, id_imovel) values
(5, 'Imovel excelente', 1, 1);