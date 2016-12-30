CREATE TABLE [dbo].[UnitClasses]
(
    [UnitId] INT NOT NULL,
    [Class] NVARCHAR(100) NOT NULL,
    CONSTRAINT [PK_dbo.UnitClasses] PRIMARY KEY CLUSTERED ([UnitId] ASC, [Class] ASC),
    CONSTRAINT [FK_dbo.UnitClasses_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units] ([Id]),
    CONSTRAINT [FK_dbo.UnitClasses_const.Classes] FOREIGN KEY([Class]) REFERENCES [const].[Classes] ([Name])
)