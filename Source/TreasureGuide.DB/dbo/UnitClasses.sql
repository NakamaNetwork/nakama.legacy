CREATE TABLE [dbo].[UnitClasses]
(
    [UnitId] INT NOT NULL,
    [Class] NVARCHAR(100) NOT NULL,
    CONSTRAINT [PK_dbo.UnitClasses] PRIMARY KEY CLUSTERED ([UnitId] ASC, [Class] ASC),
    CONSTRAINT [FK_dbo.UnitClasses_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units] ([Id]),
    CONSTRAINT [CHK_dbo.UnitClasses_Class] CHECK ([Class] in (N'Shooter',N'Fighter',N'Striker',N'Slasher',N'Cerebral',N'Driven',N'Free Spirit',N'Powerhouse',N'Evolver',N'Booster'))
)