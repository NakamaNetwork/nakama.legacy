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
   FROM INSERTED
END
GO