﻿CREATE TABLE [dbo].[Units]
(
    [Id] INT NOT NULL, 
    [Name] NVARCHAR(128) NULL, 
    [Type] SMALLINT NOT NULL CONSTRAINT [DF_dbo.Units_Type] DEFAULT 0, 
    [Class] SMALLINT NOT NULL CONSTRAINT [DF_dbo.Units_Class] DEFAULT 0, 
    [Stars] DECIMAL(2,1) NULL, 
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
    [Flags] SMALLINT NOT NULL CONSTRAINT [DF_dbo.Units_Flags] DEFAULT 0,
    CONSTRAINT [PK_dbo.Units] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO