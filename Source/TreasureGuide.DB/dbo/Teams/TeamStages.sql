CREATE TABLE [dbo].[TeamStages]
(
    [TeamId] INT NOT NULL,
    [StageDifficultyId] INT NOT NULL,
    CONSTRAINT [PK_dbo.TeamStages] PRIMARY KEY CLUSTERED ([TeamId], [StageDifficultyId] ASC),
    CONSTRAINT [FK_dbo.TeamStages_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TeamStages_dbo.StageDifficulties] FOREIGN KEY([StageDifficultyId]) REFERENCES [dbo].[StageDifficulties]([Id]) ON DELETE CASCADE
)
