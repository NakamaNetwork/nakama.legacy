CREATE TABLE [dbo].[Units]
(
    [Id] INT NOT NULL, 
    [Name] NVARCHAR(500) NULL, 
    [Type] NVARCHAR(100) NULL, 
    [Stars] TINYINT NULL, 
    [Cost] TINYINT NULL, 
    [Combo] TINYINT NULL, 
    [Sockets] TINYINT NULL, 
    [Max Level] TINYINT NULL, 
    [EXP to Max] INT NULL, 
    [Min HP] SMALLINT NULL, 
    [Min ATK] SMALLINT NULL, 
    [Min RCV] SMALLINT NULL, 
    [Max HP] SMALLINT NULL, 
    [Max ATK] SMALLINT NULL, 
    [Max RCV] SMALLINT NULL, 
    [Growth Rate] INT NULL,
    CONSTRAINT [PK_dbo.Units] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Units_const.Types] FOREIGN KEY([Type]) REFERENCES [const].[Types] ([Name])
)
