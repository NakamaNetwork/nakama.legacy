CREATE TABLE [dbo].[TeamVotes]
(
    [TeamId] INT NOT NULL,
    [UserId] NVARCHAR(450) NOT NULL,
    [Value] SMALLINT NOT NULL CONSTRAINT [DF_dbo.TeamVotes_Value] DEFAULT 0, 
    CONSTRAINT [PK_dbo.TeamVotes] PRIMARY KEY CLUSTERED ([TeamId] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.TeamVotes_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TeamVotes_dbo.UserProfiles] FOREIGN KEY([UserId]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE CASCADE
)
