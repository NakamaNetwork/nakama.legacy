CREATE TABLE [dbo].[TeamVideos]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [TeamId] INT NOT NULL,
    [VideoLink] NVARCHAR(12) NOT NULL, 
    [Deleted] BIT NOT NULL,
    [SubmittedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.TeamVideos_SubmittedDate] DEFAULT SYSDATETIMEOFFSET(),
    [UserId] NVARCHAR(450) NOT NULL,
    CONSTRAINT [PK_dbo.TeamVideos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TeamVideos_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TeamVideos_dbo.UserProfiles] FOREIGN KEY([UserId]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE CASCADE
)