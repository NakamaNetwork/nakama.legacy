CREATE TABLE [dbo].[Rounds]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [StageId] INT NOT NULL,
    [Number] TINYINT NOT NULL CONSTRAINT [DF_dbo.Rounds_Number] DEFAULT 0,
    CONSTRAINT [PK_dbo.Rounds] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Rounds_dbo.Stages] FOREIGN KEY([StageId]) REFERENCES [dbo].[Stages]([Id]) ON DELETE CASCADE,
)
