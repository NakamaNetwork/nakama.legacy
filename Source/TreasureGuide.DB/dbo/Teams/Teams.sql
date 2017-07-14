CREATE TABLE [dbo].[Teams]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [Name] NVARCHAR(250) NOT NULL,
    [Description] NVARCHAR(1000) NULL,
    [Guide] NVARCHAR(MAX) NULL,
    [Credits] NVARCHAR(250) NULL,
    [StageId] INT NULL,
    [ShipId] INT NOT NULL CONSTRAINT [DF_dbo.Teams_ShipId] DEFAULT 0, 
    [SubmittedById] NVARCHAR(450) NOT NULL,
    [SubmittedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.Teams_SubmittedDate] DEFAULT NOW(),
    [EditedById] NVARCHAR(450) NOT NULL,
    [EditedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.Teams_EditedDate] DEFAULT NOW(),
    [Version] INT NOT NULL CONSTRAINT [DF_dbo.Teams_Version] DEFAULT 0,
    CONSTRAINT [PK_dbo.Teams] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK.dbo_Teams_dbo.Stages] FOREIGN KEY([StageId]) REFERENCES [dbo].[Stages]([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK.dbo_Teams_dbo.Ships] FOREIGN KEY([ShipId]) REFERENCES [dbo].[Ships]([Id]) ON DELETE SET DEFAULT,
    CONSTRAINT [FK.dbo_Teams_SubmittedById_dbo.AspNetUsers] FOREIGN KEY([SubmittedById]) REFERENCES [dbo].[AspNetUsers]([Id]),
    CONSTRAINT [FK.dbo_Teams_EditedById_dbo.AspNetUsers] FOREIGN KEY([EditedById]) REFERENCES [dbo].[AspNetUsers]([Id])
)
