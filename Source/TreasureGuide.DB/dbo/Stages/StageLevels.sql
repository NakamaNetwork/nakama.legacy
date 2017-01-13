CREATE TABLE [dbo].[StageLevels]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [StageDifficultyId] INT NOT NULL,
    [Number] TINYINT NULL,
    [Secret] BIT NOT NULL CONSTRAINT [DF_dbo.StageLevels_Secret] DEFAULT 0,
    CONSTRAINT [PK_dbo.StageLevels] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.StageLevels_dbo.StageDifficulties] FOREIGN KEY([StageDifficultyId]) REFERENCES [dbo].[StageDifficulties]([Id]) ON DELETE CASCADE
)
