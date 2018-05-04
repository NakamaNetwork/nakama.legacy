CREATE VIEW [dbo].[GCRTable]
    AS
SELECT
    [Id],
    [LeaderId],
    [LeaderName],
    [LeaderColor],
    [StageId],
    [StageThumb],
    [StageName],
    [StageColor],
    [F2P],
    [Global],
    [Video]
FROM
    (SELECT
        *,
        ROW_NUMBER() OVER (PARTITION BY [LeaderId], [StageId] ORDER BY [Score] DESC) AS [Rank]
    FROM
        (SELECT
            *,
            CAST(
                (CASE WHEN [Global] = 1 THEN 3 ELSE 0 END) +
                (CASE WHEN [F2P] = 1 THEN 2 ELSE 0 END) +
                (CASE WHEN [Video] = 1 THEN 1 ELSE 0 END)
            AS INT) AS [Score]
        FROM 
            (SELECT
                T.[Id],
                GCRU.[UnitId] AS [LeaderId],
                GCRU.[Name] AS [LeaderName],
                GCRU.[Color] AS [LeaderColor],
                T.[StageId],
                GCRS.[Thumbnail] AS [StageThumb],
                GCRS.[Name] AS [StageName],
                GCRS.[Color] AS [StageColor],
                CAST(CASE WHEN SUM(CASE WHEN U.[Position] = 0 OR (SU.[Flags] & 4 = 0 AND SU.[Flags] & 8 = 0) THEN 1 ELSE 0 END) = COUNT(U.[UnitId]) THEN 1 ELSE 0 END AS BIT) AS [F2P],
                CAST(CASE WHEN SUM(CASE WHEN SU.[Flags] & 1 = 1 THEN 1 ELSE 0 END) = COUNT(U.[UnitId]) AND LU.[Flags] & 1 = 1 THEN 1 ELSE 0 END AS BIT) AS [Global],
                CAST(CASE WHEN COUNT(V.[Id]) > 0 THEN 1 ELSE 0 END AS BIT) AS [Video]
            FROM [dbo].[Teams] AS T
                JOIN [dbo].[GCRStageInfo] AS GCRS
                    ON GCRS.[StageId] = T.[StageId]
                JOIN [dbo].[TeamUnits] AS L
                    ON T.[Id] = L.[TeamId] AND L.[Position] = 1 AND L.[Sub] = 0
                LEFT JOIN [dbo].[Units] AS LU
                    ON L.[UnitId] = LU.[Id]
                LEFT JOIN [dbo].[GCRUnitInfo] AS GCRU
                    ON GCRU.[UnitId] = LU.[Id]
                LEFT JOIN [dbo].[TeamUnits] AS U
                    ON T.[Id] = U.[TeamId] AND U.[Position] <> 1 AND U.[Sub] = 0
                LEFT JOIN [dbo].[Units] AS SU
                    ON U.[UnitId] = SU.[Id]
                LEFT JOIN [dbo].[TeamVideos] AS V
                    ON T.[Id] = V.[TeamId]
                WHERE T.[StageId] IS NOT NULL AND T.[Draft] = 0 AND T.[Deleted] = 0 AND ISNULL(V.[Deleted], 0) = 0
                GROUP BY T.[Id], GCRU.[UnitId], GCRU.[Name], GCRU.[Color], T.[StageId], GCRS.[Name], GCRS.[Thumbnail], GCRS.[Color], LU.[Name], LU.[Type], LU.[Flags]
            ) GCR
        WHERE [LeaderId] IS NOT NULL OR [F2P] = 1
    ) Scores
    WHERE [Score] > 0
) Grouped
WHERE [Rank] = 1