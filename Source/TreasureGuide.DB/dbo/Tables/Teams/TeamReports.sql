CREATE TABLE [dbo].[TeamReports]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [TeamId] INT NOT NULL,
    [Reason] NVARCHAR(100) NOT NULL,
    [SubmittedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.TeamReports_SubmittedDate] DEFAULT SYSDATETIMEOFFSET(),
    [AcknowledgedDate] DATETIMEOFFSET(7) NULL,
    CONSTRAINT [PK_dbo.TeamReports] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK.dbo_TeamReports_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams]([Id]) ON DELETE CASCADE
)