CREATE TABLE [dbo].[UnitFlags]
(
    [UnitId] INT NOT NULL,
    [Global] BIT NOT NULL CONSTRAINT [DF_dbo.UnitFlags_Global] DEFAULT 1,
    [RR] BIT NOT NULL CONSTRAINT [DF_dbo.UnitFlags_RR]  DEFAULT 0, 
    [ERR] BIT NOT NULL CONSTRAINT [DF_dbo.UnitFlags_ERR]  DEFAULT 0, 
    [LRR] BIT NOT NULL CONSTRAINT [DF_dbo.UnitFlags_LRR]  DEFAULT 0, 
    [Promo] BIT NOT NULL CONSTRAINT [DF_dbo.UnitFlags_PROMO]  DEFAULT 0, 
    [Shop] BIT NOT NULL CONSTRAINT [DF_dbo.UnitFlags_Shop]  DEFAULT 0,
    CONSTRAINT [PK_dbo.UnitFlags] PRIMARY KEY CLUSTERED ([UnitId] ASC),
	CONSTRAINT [FK_dbo.UnitFlags_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units]([Id]) ON DELETE CASCADE
)
