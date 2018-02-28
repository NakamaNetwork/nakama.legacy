CREATE TABLE [dbo].[UnitAliases]
(
    [UnitId] INT NOT NULL,
    [Name] NVARCHAR(128) NOT NULL,
    [EditedDate] DATETIMEOFFSET(7) NULL,
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
   FROM INSERTED
END
GO