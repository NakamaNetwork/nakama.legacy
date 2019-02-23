CREATE TABLE [dbo].[BoxUnits]
(
    [BoxId] INT NOT NULL,
    [UnitId] INT NOT NULL,
    [Flags] INT NULL,
    CONSTRAINT [PK.dbo_BoxUnits] PRIMARY KEY([BoxId] ASC, [UnitId] ASC),
    CONSTRAINT [FK.dbo_BoxUnits_dbo.Boxes] FOREIGN KEY([BoxId]) REFERENCES [dbo].[Boxes]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK.dbo_BoxUnits_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units]([Id]) ON DELETE CASCADE
)
