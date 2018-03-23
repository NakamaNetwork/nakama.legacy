CREATE TABLE [dbo].[ScheduledEvents]
(
    [StageId] INT NOT NULL,
    [Global] BIT NOT NULL,
    [StartDate] DATE(7) NOT NULL,
    [EndDate] DATETIMEOFFSET(7) NOT NULL,
    CONSTRAINT [PK_dbo.ScheduledEvents] PRIMARY KEY CLUSTERED ([StageId] ASC, [Global] ASC, [StartDate] ASC, [EndDate] ASC),
    CONSTRAINT [FK_dbo.ScheduledEvents_dbo.Stages] FOREIGN KEY([StageId]) REFERENCES [dbo].[Stages] ([Id]) ON DELETE CASCADE
)
