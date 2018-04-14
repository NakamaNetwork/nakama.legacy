INSERT INTO [dbo].[GCRUnits]
    SELECT [Id], [Id] FROM [dbo].[Units]
        WHERE [Stars] >= 6
          AND [Name] NOT LIKE '%dual%' 
          AND [Name] NOT LIKE 'Saint%' 
          AND [Class] != 512
INSERT INTO [dbo].[GCRUnits] VALUES (1387, 1387)


INSERT INTO [dbo].[GCRStages]
    SELECT [Id], [Id] FROM [dbo].[Stages]
        WHERE ([Type] = 4 AND [Name] NOT LIKE '%!?%')
        OR ([Type] = 5 AND [Id] LIKE '%1')
        OR ([Type] = 6 AND [Name] LIKE 'Invasion%')
        OR ([Type] = 7)