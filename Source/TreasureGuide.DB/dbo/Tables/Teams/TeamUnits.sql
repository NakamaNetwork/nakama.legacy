CREATE TABLE [dbo].[TeamUnits]
(
    [TeamId] INT NOT NULL,
    [UnitId] INT NOT NULL,
    [Position] TINYINT NOT NULL,
    [SpecialLevel] TINYINT NULL,
    [Sub] BIT NOT NULL CONSTRAINT [DF_dbo.TeamUnits_Sub] DEFAULT 0,
    CONSTRAINT [PK_dbo.TeamUnits] PRIMARY KEY CLUSTERED ([TeamId] ASC, [UnitId] ASC, [Position] ASC),
    CONSTRAINT [CK_dbo.TeamUnits_Position] CHECK ([Position] >= 0 AND [Position] < 7),
    CONSTRAINT [FK_dbo.TeamUnits_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TeamUnits_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units] ([Id]) ON DELETE CASCADE
)
