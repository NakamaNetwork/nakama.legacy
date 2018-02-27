CREATE TABLE [dbo].[UnitEvolutions]
(
    [FromUnitId] INT NOT NULL,
    [ToUnitId] INT NOT NULL,
    [EditedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.UnitEvolutions_EditedDate] DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT [PK_dbo.UnitEvolutions] PRIMARY KEY CLUSTERED ([FromUnitId] ASC, [ToUnitId] ASC),
    CONSTRAINT [FK_dbo.UnitEvolutions_dbo.ToUnitId] FOREIGN KEY([ToUnitId]) REFERENCES [dbo].[Units] ([Id]),
    CONSTRAINT [FK_dbo.UnitEvolutions_dbo.FromUnitId] FOREIGN KEY([FromUnitId]) REFERENCES [dbo].[Units] ([Id])
)
