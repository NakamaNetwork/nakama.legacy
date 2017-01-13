CREATE TABLE [dbo].[StageUnits]
(
    [Id] INT NOT NULL  IDENTITY(1,1),
    [StageLevelId] INT NOT NULL,
    [UnitId] INT NOT NULL,
    [Position] INT NULL,
    [HP] SMALLINT NULL, 
    [Defense] SMALLINT NULL,
    [ATK] SMALLINT NULL, 
    [MinCD] TINYINT NULL, 
    [MaxCD] TINYINT NULL, 
    [Interval] TINYINT NULL, 
    CONSTRAINT [PK_dbo.StageUnits] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [CK_dbo.StageUnits_Position] CHECK ([Position] >= 0 AND [POSITION] < 6),
    CONSTRAINT [FK_dbo.StageUnits_dbo.StageLevels] FOREIGN KEY([StageLevelId]) REFERENCES [dbo].[StageLevels]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.StageUnits_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units]([Id]) ON DELETE CASCADE,
)
