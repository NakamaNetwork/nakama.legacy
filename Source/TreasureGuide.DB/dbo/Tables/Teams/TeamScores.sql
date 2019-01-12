CREATE TABLE [dbo].[TeamScores]
(
    [TeamId] INT NOT NULL,
    [Value] INT NOT NULL CONSTRAINT [DF_dbo.TeamScores_Value] DEFAULT 0, 
    CONSTRAINT [PK_dbo.TeamScores] PRIMARY KEY CLUSTERED ([TeamId] ASC),
    CONSTRAINT [FK_dbo.TeamScores_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE
)
