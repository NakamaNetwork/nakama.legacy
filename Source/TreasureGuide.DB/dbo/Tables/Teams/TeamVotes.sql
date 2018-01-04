CREATE TABLE [dbo].[TeamVotes]
(
    [TeamId] INT NOT NULL,
    [UserHash] NVARCHAR(256) NOT NULL,
    CONSTRAINT [PK_dbo.TeamVotes] PRIMARY KEY CLUSTERED ([TeamId] ASC, [UserHash] ASC),
    CONSTRAINT [FK_dbo.TeamVotes_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE,
)
