CREATE TABLE [dbo].[Ships]
(
    [Id] INT NOT NULL,
    [Name] NVARCHAR(500) NULL,
    [Description] NVARCHAR(1000) NULL,
    CONSTRAINT [PK_dbo.Ships] PRIMARY KEY CLUSTERED ([Id] ASC)
)
