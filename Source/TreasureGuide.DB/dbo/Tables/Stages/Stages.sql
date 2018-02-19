CREATE TABLE [dbo].[Stages]
(
    [Id] INT NOT NULL,
    [Name] NVARCHAR(128) NULL,
    [Stamina] TINYINT NULL,
    [UnitId] INT NULL,
    [Type] TINYINT NOT NULL CONSTRAINT [DF_dbo.Stages_Type] DEFAULT 0,
    [Global] BIT NOT NULL CONSTRAINT [DF_dbo.Stages_Global] DEFAULT 1,
    [OldId] INT NULL,
    CONSTRAINT [PK_dbo.Stages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK.dbo_Stages_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units]([Id]) ON DELETE SET NULL
)
