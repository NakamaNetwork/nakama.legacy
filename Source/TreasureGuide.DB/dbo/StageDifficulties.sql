CREATE TABLE [dbo].[StageDifficulties]
(
	[Id] INT NOT NULL,
	[StageId] INT NOT NULL,
    [Name] NVARCHAR(500) NULL,
	[Stamina] TINYINT NOT NULL,
	[Global] BIT NOT NULL CONSTRAINT [DF_dbo.Difficulties_Global] DEFAULT 1,
    CONSTRAINT [PK_dbo.Difficulties] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_dbo.StageDifficulties_dbo.Stages] FOREIGN KEY([StageId]) REFERENCES [dbo].[Stages]([Id]) ON DELETE CASCADE
)
