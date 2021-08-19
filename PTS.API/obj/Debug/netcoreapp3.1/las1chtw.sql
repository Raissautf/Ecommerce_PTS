IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [CarrinhoCliente] (
    [Id] uniqueidentifier NOT NULL,
    [ClienteId] uniqueidentifier NOT NULL,
    [ValorTotal] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_CarrinhoCliente] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [EnderecoPedido] (
    [Id] uniqueidentifier NOT NULL,
    [Logradouro] varchar(100) NULL,
    [Numero] varchar(100) NULL,
    [Complemento] varchar(100) NULL,
    [Bairro] varchar(100) NULL,
    [Cep] varchar(100) NULL,
    [Cidade] varchar(100) NULL,
    [Estado] varchar(100) NULL,
    [PessoaId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_EnderecoPedido] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Pessoas] (
    [Id] uniqueidentifier NOT NULL,
    [Nome] varchar(100) NULL,
    [Login] varchar(100) NULL,
    [Documento] varchar(100) NULL,
    [Email] varchar(100) NULL,
    [Senha] varchar(100) NULL,
    [SenhaConfirmacao] varchar(100) NULL,
    CONSTRAINT [PK_Pessoas] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Produtos] (
    [Id] uniqueidentifier NOT NULL,
    [Nome] varchar(100) NULL,
    [Descricao] varchar(100) NULL,
    [Ativo] bit NOT NULL,
    [Valor] decimal(18,2) NOT NULL,
    [DataCadastro] datetime2 NOT NULL,
    [Imagem] varchar(100) NULL,
    [QuantidadeEstoque] int NOT NULL,
    CONSTRAINT [PK_Produtos] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [CarrinhoItem] (
    [Id] uniqueidentifier NOT NULL,
    [ProdutoId] uniqueidentifier NOT NULL,
    [Nome] varchar(100) NULL,
    [Quantidade] int NOT NULL,
    [Valor] decimal(18,2) NOT NULL,
    [Imagem] varchar(100) NULL,
    [CarrinhoClienteId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_CarrinhoItem] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CarrinhoItem_CarrinhoCliente_CarrinhoClienteId] FOREIGN KEY ([CarrinhoClienteId]) REFERENCES [CarrinhoCliente] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Pedido] (
    [Id] uniqueidentifier NOT NULL,
    [Codigo] int NOT NULL,
    [ClienteId] uniqueidentifier NOT NULL,
    [ValorTotal] decimal(18,2) NOT NULL,
    [Data] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [EnderecoId] uniqueidentifier NULL,
    [NumeroCartao] varchar(100) NULL,
    [NomeCartao] varchar(100) NULL,
    [ExpiracaoCartao] varchar(100) NULL,
    [CvvCartao] varchar(100) NULL,
    CONSTRAINT [PK_Pedido] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Pedido_EnderecoPedido_EnderecoId] FOREIGN KEY ([EnderecoId]) REFERENCES [EnderecoPedido] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Endereco] (
    [Id] uniqueidentifier NOT NULL,
    [Logradouro] varchar(100) NULL,
    [Numero] varchar(100) NULL,
    [Complemento] varchar(100) NULL,
    [Bairro] varchar(100) NULL,
    [Cep] varchar(100) NULL,
    [Cidade] varchar(100) NULL,
    [Estado] varchar(100) NULL,
    [PessoaId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Endereco] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Endereco_Pessoas_PessoaId] FOREIGN KEY ([PessoaId]) REFERENCES [Pessoas] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [PedidoItem] (
    [Id] uniqueidentifier NOT NULL,
    [ProdutoId] uniqueidentifier NOT NULL,
    [Nome] varchar(100) NULL,
    [Quantidade] int NOT NULL,
    [Valor] decimal(18,2) NOT NULL,
    [Imagem] varchar(100) NULL,
    [PedidoId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_PedidoItem] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PedidoItem_Pedido_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [Pedido] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IDX_Cliente] ON [CarrinhoCliente] ([ClienteId]);

GO

CREATE INDEX [IX_CarrinhoItem_CarrinhoClienteId] ON [CarrinhoItem] ([CarrinhoClienteId]);

GO

CREATE INDEX [IX_Endereco_PessoaId] ON [Endereco] ([PessoaId]);

GO

CREATE INDEX [IDX_Pedido_Cliente] ON [Pedido] ([ClienteId]);

GO

CREATE INDEX [IX_Pedido_EnderecoId] ON [Pedido] ([EnderecoId]);

GO

CREATE INDEX [IX_PedidoItem_PedidoId] ON [PedidoItem] ([PedidoId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201204184410_Migration01', N'3.1.8');

GO

