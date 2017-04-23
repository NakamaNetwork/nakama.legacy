CREATE TABLE [dbo].[UnitFlags]
(
    [UnitId] INT NOT NULL,
    [FlagType] TINYINT NOT NULL CONSTRAINT [DF_dbo.UnitFlags_FlagType] DEFAULT 0,
    CONSTRAINT [PK_dbo.UnitFlags] PRIMARY KEY CLUSTERED ([UnitId] ASC, [FlagType] ASC),
    CONSTRAINT [FK_dbo.UnitFlags_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units]([Id]) ON DELETE CASCADE
)
