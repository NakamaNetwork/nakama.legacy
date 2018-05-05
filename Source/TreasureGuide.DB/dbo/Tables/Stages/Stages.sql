CREATE TABLE [dbo].[Stages]
(
    [Id] INT NOT NULL,
    [Name] NVARCHAR(128) NULL,
    [Stamina] TINYINT NULL,
    [UnitId] INT NULL,
    [Type] TINYINT NOT NULL CONSTRAINT [DF_dbo.Stages_Type] DEFAULT 0,
    [Global] BIT NOT NULL CONSTRAINT [DF_dbo.Stages_Global] DEFAULT 1,
    [EditedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.Stages_EditedDate] DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT [PK_dbo.Stages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK.dbo_Stages_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units]([Id]) ON DELETE SET NULL
)
GO
CREATE TRIGGER [dbo].[TRG_Stages_Updated]
ON [dbo].[Stages]
AFTER UPDATE 
AS BEGIN
   UPDATE [dbo].[Stages]
   SET [EditedDate] = SYSDATETIMEOFFSET()
   FROM INSERTED I
   WHERE [dbo].[Stages].[Id] = I.[Id]
END
GO
CREATE TRIGGER [dbo].[TRG_Stages_Deleted]
ON [dbo].[Stages]
AFTER DELETE 
AS BEGIN
   INSERT INTO [dbo].[DeletedItems]([Id], [Type], [EditedDate])
   SELECT [Id], 2, SYSDATETIMEOFFSET() FROM DELETED I
END
GO