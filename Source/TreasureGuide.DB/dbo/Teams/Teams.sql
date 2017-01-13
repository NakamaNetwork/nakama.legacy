CREATE TABLE [dbo].[Teams]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [StageDifficultyId] INT NOT NULL,
    [SubmittedById] NVARCHAR(128) NOT NULL,
    [Description] NVARCHAR(1000) NULL,
    [Credits] [nvarchar](250) NULL,
    CONSTRAINT [PK_dbo.Teams] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK.dbo_Teams_dbo.AspNetUsers] FOREIGN KEY([SubmittedById]) REFERENCES [dbo].[AspNetUsers]([Id]),
    CONSTRAINT [FK_dbo.Teams_dbo.StageDifficulties] FOREIGN KEY([StageDifficultyId]) REFERENCES [dbo].[StageDifficulties]([Id]) ON DELETE CASCADE
)
