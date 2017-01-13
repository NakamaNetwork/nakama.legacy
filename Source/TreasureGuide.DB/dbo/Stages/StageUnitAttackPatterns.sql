CREATE TABLE [dbo].[StageUnitAttackPatterns]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[StageUnitId] INT NOT NULL,
	[Condition] NVARCHAR(250) NULL,
	[Description] NVARCHAR(1000) NULL,
    CONSTRAINT [PK_dbo.StageUnitAttackPatterns] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.StageUnitAttackPatterns_dbo.StageUnits] FOREIGN KEY([StageUnitId]) REFERENCES [dbo].[StageUnits]([Id]) ON DELETE CASCADE,
)
