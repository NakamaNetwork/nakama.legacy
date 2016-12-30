CREATE TABLE [dbo].[Units]
(
    [Id] INT NOT NULL, 
    [Name] NVARCHAR(500) NULL, 
    [Type] NVARCHAR(8) NULL, 
    [Stars] TINYINT NULL, 
    [Cost] TINYINT NULL, 
    [Combo] TINYINT NULL, 
    [Sockets] TINYINT NULL, 
    [MaxLevel] TINYINT NULL, 
    [EXPtoMax] INT NULL, 
    [MinHP] SMALLINT NULL, 
    [MinATK] SMALLINT NULL, 
    [MinRCV] SMALLINT NULL, 
    [MaxHP] SMALLINT NULL, 
    [MaxATK] SMALLINT NULL, 
    [MaxRCV] SMALLINT NULL, 
    [GrowthRate] INT NULL,
    CONSTRAINT [PK_dbo.Units] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CHK_dbo.Units_Type] CHECK ([Type] in (N'STR',N'DEX',N'QCK',N'INT',N'PSY'))
)
