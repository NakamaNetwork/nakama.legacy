CREATE VIEW [dbo].[GCRStageInfo]
    AS SELECT
        G.[StageId],
        G.[Order],
        ISNULL(G.[Name], S.[Name]) AS [Name],
        S.[UnitId] AS [Thumbnail],
        U.[Type] AS [Color]
    FROM [dbo].[GCRStages] AS G
        JOIN [dbo].[Stages] AS S
            ON G.[StageId] = S.[Id]
        LEFT JOIN [dbo].[Units] AS U
            ON S.[UnitId] = U.[Id]
