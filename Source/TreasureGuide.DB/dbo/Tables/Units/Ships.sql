CREATE TABLE [dbo].[Ships]
(
    [Id] INT NOT NULL,
    [Name] NVARCHAR(128) NULL,
    [Description] NVARCHAR(1000) NULL,
    [EventShip] BIT NOT NULL CONSTRAINT [DF_dbo.Ships_EventShip] DEFAULT 0,
    [EventShipActive] BIT NOT NULL CONSTRAINT [DF_dbo.Ships_EventShipActive] DEFAULT 0,
    [EditedDate] DATETIMEOFFSET(7) NULL,
    CONSTRAINT [PK_dbo.Ships] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TRIGGER [dbo].[TRG_Ships_Updated]
ON [dbo].[Ships]
AFTER UPDATE 
AS BEGIN
   UPDATE [dbo].[Ships]
   SET [EditedDate] = SYSDATETIMEOFFSET()
   FROM INSERTED I
   WHERE [dbo].[Ships].[Id] = I.[Id]
END
GO
CREATE TRIGGER [dbo].[TRG_Ships_Deleted]
ON [dbo].[Ships]
AFTER DELETE 
AS BEGIN
   INSERT INTO [dbo].[DeletedItems]([Id], [Type], [EditedDate])
   SELECT [Id], 3, SYSDATETIMEOFFSET() FROM DELETED I
END
GO