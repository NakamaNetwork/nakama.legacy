CREATE TABLE [dbo].[Teams]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [SubmittedById] NVARCHAR(450) NOT NULL,
    [Name] NVARCHAR(250) NOT NULL,
    [Description] NVARCHAR(1000) NULL,
    [Guide] NVARCHAR(MAX) NULL,
    [Credits] NVARCHAR(250) NULL,
    [StageId] INT NULL,
    CONSTRAINT [PK_dbo.Teams] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK.dbo_Teams_dbo.Stages] FOREIGN KEY([StageId]) REFERENCES [dbo].[Stages]([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK.dbo_Teams_dbo.AspNetUsers] FOREIGN KEY([SubmittedById]) REFERENCES [dbo].[AspNetUsers]([Id])
)
