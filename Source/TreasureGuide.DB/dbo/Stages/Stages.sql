CREATE TABLE [dbo].[Stages]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [Name] NVARCHAR(500) NULL,
    [Stamina] TINYINT NULL,
    [Type] TINYINT NOT NULL CONSTRAINT [DF_dbo.Stages_Type] DEFAULT 0,
    [Global] BIT NOT NULL CONSTRAINT [DF_dbo.Stages_Global] DEFAULT 1,
    [ParentId] INT NULL,
    CONSTRAINT [PK_dbo.Stages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Stages_dbo.Stages] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Stages]([Id])
)
