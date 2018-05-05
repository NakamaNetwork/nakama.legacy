CREATE TABLE [dbo].[StageAliases]
(
    [StageId] INT NOT NULL,
    [Name] NVARCHAR(128) NOT NULL,
    [EditedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.StageAliases_EditedDate] DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT [PK_dbo.StageAliases] PRIMARY KEY CLUSTERED ([StageId] ASC, [Name] ASC),
    CONSTRAINT [FK_dbo.StageAliasess_dbo.Stages] FOREIGN KEY([StageId]) REFERENCES [dbo].[Stages] ([Id]) ON DELETE CASCADE
)
GO
CREATE TRIGGER [dbo].[TRG_StageAliases_Updated]
ON [dbo].[StageAliases]
AFTER UPDATE 
AS BEGIN
   UPDATE [dbo].[StageAliases]
   SET [EditedDate] = SYSDATETIMEOFFSET()
   FROM INSERTED I
   WHERE [dbo].[StageAliases].[StageId] = I.[StageId] AND [dbo].[StageAliases].[Name] = I.[Name]
END
GO
CREATE TRIGGER [dbo].[TRG_StageAliases_Deleted]
ON [dbo].[StageAliases]
AFTER DELETE 
AS BEGIN
   UPDATE [dbo].[Stages]
   SET [EditedDate] = SYSDATETIMEOFFSET()
   FROM DELETED I
   WHERE [dbo].[Stages].[Id] = I.[StageId]
END
GO