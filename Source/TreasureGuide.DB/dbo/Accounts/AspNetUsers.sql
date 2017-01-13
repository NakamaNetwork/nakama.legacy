CREATE TABLE [dbo].[AspNetUsers](
	[Id] NVARCHAR(128) NOT NULL,
	[Email] NVARCHAR(256) NULL,
	[EmailConfirmed] BIT NOT NULL,
	[PasswordHash] NVARCHAR(max) NULL,
	[SecurityStamp] NVARCHAR(max) NULL,
	[PhoneNumber] NVARCHAR(max) NULL,
	[PhoneNumberConfirmed] BIT NOT NULL,
	[TwoFactorEnabled] BIT NOT NULL,
	[LockoutEndDateUtc] DATETIME NULL,
	[LockoutEnabled] BIT NOT NULL,
	[AccessFailedCount] INT NOT NULL,
	[UserName] NVARCHAR(256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
)