CREATE TABLE [dbo].[TeamSockets]
(
    [TeamId] INT NOT NULL,
    [SocketType] TINYINT NOT NULL,
    [Level] TINYINT NOT NULL,
    CONSTRAINT [PK_dbo.TeamSockets] PRIMARY KEY CLUSTERED ([TeamId] ASC, [SocketType] ASC),
    CONSTRAINT [FK_dbo.TeamSockets_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [CK_dbo.TeamSockets_Level] CHECK ([Level] >= 0 AND [LEVEL] <= 5)
)
