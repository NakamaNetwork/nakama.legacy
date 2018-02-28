CREATE TABLE [dbo].[UnitEvolutions]
(
    [FromUnitId] INT NOT NULL,
    [ToUnitId] INT NOT NULL,
    [EditedDate] DATETIMEOFFSET(7) NULL,
    CONSTRAINT [PK_dbo.UnitEvolutions] PRIMARY KEY CLUSTERED ([FromUnitId] ASC, [ToUnitId] ASC),
    CONSTRAINT [FK_dbo.UnitEvolutions_dbo.ToUnitId] FOREIGN KEY([ToUnitId]) REFERENCES [dbo].[Units] ([Id]),
    CONSTRAINT [FK_dbo.UnitEvolutions_dbo.FromUnitId] FOREIGN KEY([FromUnitId]) REFERENCES [dbo].[Units] ([Id])
)
GO
CREATE TRIGGER [dbo].[TRG_UnitEvolutions_Updated]
ON [dbo].[UnitEvolutions]
AFTER UPDATE 
AS BEGIN
   UPDATE [dbo].[UnitEvolutions]
   SET [EditedDate] = SYSDATETIMEOFFSET()
   FROM INSERTED
END
GO