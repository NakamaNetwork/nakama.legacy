﻿CREATE TABLE [dbo].[TeamUnits]
(
    [TeamId] INT NOT NULL,
    [UnitId] INT NOT NULL,
    [Position] TINYINT NOT NULL,
    [Flags] INT NULL,
    [Support] BIT NOT NULL CONSTRAINT [DF_dbo.TeamUnits_Support] DEFAULT 0,
    [Sub] BIT NOT NULL CONSTRAINT [DF_dbo.TeamUnits_Sub] DEFAULT 0,
    CONSTRAINT [PK_dbo.TeamUnits] PRIMARY KEY CLUSTERED ([TeamId] ASC, [Position] ASC, [UnitId] ASC),
    CONSTRAINT [CK_dbo.TeamUnits_Position] CHECK ([Position] >= 0 AND [Position] < 6),
    CONSTRAINT [FK_dbo.TeamUnits_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TeamUnits_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units] ([Id]) ON DELETE CASCADE
)
GO
CREATE NONCLUSTERED INDEX [IX_dbo.TeamUnits]
    ON [dbo].[TeamUnits] ([Position],[Sub],[Support],[Flags])
        INCLUDE ([TeamId],[UnitId])
GO
CREATE NONCLUSTERED INDEX [IX_dbo.TeamUnits_SubSupport]
    ON [dbo].[TeamUnits] ([Sub], [Support])
        INCLUDE ([TeamId],[UnitId])
GO