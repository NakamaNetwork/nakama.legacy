CREATE TABLE [dbo].[RoundUnitBehavior]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [RoundUnitId] INT NOT NULL,
    [Condition] NVARCHAR(250) NULL,
    [Description] NVARCHAR(1000) NULL,
    CONSTRAINT [PK_dbo.RoundUnitBehavior] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.RoundUnitBehavior_dbo.RoundUnits] FOREIGN KEY([RoundUnitId]) REFERENCES [dbo].[RoundUnits]([Id]) ON DELETE CASCADE,
)
