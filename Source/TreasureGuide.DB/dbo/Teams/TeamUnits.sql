﻿CREATE TABLE [dbo].[TeamUnits]
(
    [TeamId] INT NOT NULL,
    [UnitId] INT NOT NULL,
    [Position] TINYINT NULL,
    [SpecialLevel] BIT NULL,
    CONSTRAINT [PK_dbo.TeamUnits] PRIMARY KEY CLUSTERED ([TeamId] ASC, [UnitId] ASC),
    CONSTRAINT [FK_dbo.TeamUnits_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TeamUnits_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units] ([Id]) ON DELETE CASCADE
)
