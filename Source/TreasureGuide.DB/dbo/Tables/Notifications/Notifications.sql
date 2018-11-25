CREATE TABLE [dbo].[Notifications]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [UserId] NVARCHAR(450) NOT NULL,
    [EventType] INT NOT NULL,
    [EventId] INT NULL,
    [TriggerUserId] NVARCHAR(450) NULL,
    [ReceivedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.Notifications_ReceivedDate] DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT [PK_dbo.Notifications] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK.dbo_Notifications_dbo.UserProfiles] FOREIGN KEY([UserId]) REFERENCES [dbo].[UserProfiles]([Id]),
    CONSTRAINT [FK.dbo_Notifications_dbo.UserProfiles_Trigger] FOREIGN KEY([TriggerUserId]) REFERENCES [dbo].[UserProfiles]([Id])
)
GO
-- EventType 0 = Unknown, 1 = Team Comment, 2 = Comment Reply, 3 = Video Submission
CREATE NONCLUSTERED INDEX [IX_dbo.Notifications]
    ON [dbo].[Notifications]([UserId],[EventType],[EventId])
        INCLUDE ([Id]);
GO