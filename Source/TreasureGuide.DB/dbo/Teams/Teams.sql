CREATE TABLE [dbo].[Teams]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [SubmittedById] NVARCHAR(450) NOT NULL,
    [Description] NVARCHAR(1000) NULL,
    [Credits] NVARCHAR(250) NULL,
    CONSTRAINT [PK_dbo.Teams] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK.dbo_Teams_dbo.AspNetUsers] FOREIGN KEY([SubmittedById]) REFERENCES [dbo].[AspNetUsers]([Id])
)
