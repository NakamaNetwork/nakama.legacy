CREATE TABLE [dbo].[UnitAliases]
(
    [UnitId] INT NOT NULL,
    [Name] NVARCHAR(128) NOT NULL,
    [EditedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.UnitAliases_EditedDate] DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT [PK_dbo.UnitAliases] PRIMARY KEY CLUSTERED ([UnitId] ASC, [Name] ASC),
    CONSTRAINT [FK_dbo.UnitAliasess_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units] ([Id]) ON DELETE CASCADE
)
GO
CREATE TRIGGER [dbo].[TRG_UnitAliases_Updated]
ON [dbo].[UnitAliases]
AFTER UPDATE 
AS BEGIN
   UPDATE [dbo].[UnitAliases]
   SET [EditedDate] = SYSDATETIMEOFFSET()
   FROM INSERTED I
   WHERE [dbo].[UnitAliases].[UnitId] = I.[UnitId] AND [dbo].[UnitAliases].[Name] = I.[Name]
END
GO
CREATE TRIGGER [dbo].[TRG_UnitAliases_Deleted]
ON [dbo].[UnitAliases]
AFTER DELETE 
AS BEGIN
   UPDATE [dbo].[Units]
   SET [EditedDate] = SYSDATETIMEOFFSET()
   FROM DELETED I
   WHERE [dbo].[Units].[Id] = I.[UnitId]
END
GO