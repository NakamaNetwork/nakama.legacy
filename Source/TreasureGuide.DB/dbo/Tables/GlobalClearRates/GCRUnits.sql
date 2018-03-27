CREATE TABLE [dbo].[GCRUnits]
(
    [UnitId] INT NOT NULL,
    CONSTRAINT [PK_dbo.GCRUnits] PRIMARY KEY CLUSTERED ([UnitId] ASC),
    CONSTRAINT [FK_dbo.GCRUnits_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units] ([Id]) ON DELETE CASCADE,
)
