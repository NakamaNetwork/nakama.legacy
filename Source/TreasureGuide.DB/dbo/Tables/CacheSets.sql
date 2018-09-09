CREATE TABLE [dbo].[CacheSets]
(
    [Type] INT NOT NULL,
    [JSON] NVARCHAR(MAX) NOT NULL,
    [EditedDate] DATETIMEOFFSET(7) NOT NULL,
    CONSTRAINT [PK_dbo.DeletedItems] PRIMARY KEY CLUSTERED ([Type] ASC)
)
GO
CREATE TRIGGER [dbo].[TRG_CacheSets_Updated]
ON [dbo].[CacheSets]
AFTER UPDATE 
AS BEGIN
   UPDATE [dbo].[CacheSets]
   SET [EditedDate] = SYSDATETIMEOFFSET()
   FROM INSERTED I
   WHERE [dbo].[CacheSets].[Type] = I.[Type]
END
GO