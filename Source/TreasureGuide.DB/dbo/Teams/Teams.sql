CREATE TABLE [dbo].[Teams]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [SubmittedById] NVARCHAR(450) NOT NULL,
    [Name] NVARCHAR(250) NOT NULL,
    [Description] NVARCHAR(1000) NULL,
    [Guide] NVARCHAR(MAX) NULL,
    [Credits] NVARCHAR(250) NULL,
    [StageId] INT NULL,
    [ShipId] INT NOT NULL CONSTRAINT [DF_dbo.Teams_ShipId] DEFAULT 0, 
    CONSTRAINT [PK_dbo.Teams] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK.dbo_Teams_dbo.Stages] FOREIGN KEY([StageId]) REFERENCES [dbo].[Stages]([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK.dbo_Teams_dbo.Ships] FOREIGN KEY([ShipId]) REFERENCES [dbo].[Ships]([Id]) ON DELETE SET DEFAULT,
    CONSTRAINT [FK.dbo_Teams_dbo.AspNetUsers] FOREIGN KEY([SubmittedById]) REFERENCES [dbo].[AspNetUsers]([Id])
)
