CREATE TABLE [dbo].[Units]
(
    [Id] INT NOT NULL, 
    [Name] NVARCHAR(500) NULL, 
    [Type] TINYINT NOT NULL CONSTRAINT [DF_dbo.Units_Type] DEFAULT 0, 
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
    [GrowthRate] DECIMAL(2, 1) NULL,
    CONSTRAINT [PK_dbo.Units] PRIMARY KEY CLUSTERED ([Id] ASC)
)