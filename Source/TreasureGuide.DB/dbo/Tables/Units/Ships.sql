CREATE TABLE [dbo].[Ships]
(
    [Id] INT NOT NULL,
    [Name] NVARCHAR(128) NULL,
    [Description] NVARCHAR(1000) NULL,
    [EventShip] BIT NOT NULL CONSTRAINT [DF_dbo.Ships_EventShip] DEFAULT 0,
    [EventShipActive] BIT NOT NULL CONSTRAINT [DF_dbo.Ships_EventShipActive] DEFAULT 0,
    [EditedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.Ships_EditedDate] DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT [PK_dbo.Ships] PRIMARY KEY CLUSTERED ([Id] ASC)
)
