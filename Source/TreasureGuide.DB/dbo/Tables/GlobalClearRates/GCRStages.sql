CREATE TABLE [dbo].[GCRStages]
(
    [StageId] INT NOT NULL,
    [Order] INT NOT NULL,
    CONSTRAINT [PK_dbo.GCRStages] PRIMARY KEY CLUSTERED ([StageId] ASC),
    CONSTRAINT [FK_dbo.GCRStages_dbo.Units] FOREIGN KEY([StageId]) REFERENCES [dbo].[Stages] ([Id]) ON DELETE CASCADE,
)
