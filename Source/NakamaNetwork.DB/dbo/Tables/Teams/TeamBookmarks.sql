CREATE TABLE [dbo].[TeamBookmarks]
(
    [TeamId] INT NOT NULL,
    [UserId] NVARCHAR(450) NOT NULL,
    CONSTRAINT [PK_dbo.TeamBookmarks] PRIMARY KEY CLUSTERED ([TeamId] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.TeamBookmarks_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TeamBookmarks_dbo.UserProfiles] FOREIGN KEY([UserId]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE CASCADE
)
