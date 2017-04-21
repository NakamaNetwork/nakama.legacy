CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] NVARCHAR(450) NOT NULL,
	[LoginProvider] NVARCHAR(450) NOT NULL,
	[Name] NVARCHAR(450) NOT NULL,
	[Value] NVARCHAR(MAX) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
