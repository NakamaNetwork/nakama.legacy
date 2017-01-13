CREATE TABLE [dbo].[AspNetUserClaims](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] NVARCHAR(128) NOT NULL,
    [ClaimType] NVARCHAR(max) NULL,
    [ClaimValue] NVARCHAR(max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
 CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
)