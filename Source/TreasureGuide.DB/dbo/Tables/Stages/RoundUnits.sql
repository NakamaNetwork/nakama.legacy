CREATE TABLE [dbo].[RoundUnits]
(
    [Id] INT NOT NULL  IDENTITY(1,1),
    [RoundId] INT NOT NULL,
    [Position] INT NULL,
    [Level] TINYINT NOT NULL,
    [UnitId] INT NOT NULL,
    [HP] SMALLINT NULL, 
    [Defense] SMALLINT NULL,
    [ATK] SMALLINT NULL, 
    [MinCD] TINYINT NULL, 
    [MaxCD] TINYINT NULL, 
    [Interval] TINYINT NULL, 
    CONSTRAINT [PK_dbo.RoundUnits] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_dbo.RoundUnits_Position] CHECK ([Position] >= 0 AND [POSITION] < 6),
    CONSTRAINT [FK_dbo.RoundUnits_dbo.Stages] FOREIGN KEY([RoundId]) REFERENCES [dbo].[Rounds]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.RoundUnits_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units]([Id]) ON DELETE CASCADE,
)
