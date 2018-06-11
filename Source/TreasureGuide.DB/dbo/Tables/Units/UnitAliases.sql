CREATE TABLE [dbo].[UnitAliases]
(
    [UnitId] INT NOT NULL,
    [Name] NVARCHAR(128) NOT NULL,
    CONSTRAINT [PK_dbo.UnitAliases] PRIMARY KEY CLUSTERED ([UnitId] ASC, [Name] ASC),
    CONSTRAINT [FK_dbo.UnitAliasess_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units] ([Id]) ON DELETE CASCADE
)
GO