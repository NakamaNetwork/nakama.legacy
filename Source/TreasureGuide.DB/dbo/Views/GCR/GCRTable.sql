CREATE VIEW [dbo].[GCRTable]
    AS
SELECT
    G.[Id],
    U.[UnitId] AS [LeaderId],
    G.[StageId],
    G.[F2P],
    G.[Global],
    G.[Video]
FROM
    [dbo].[GCRSummaries] AS G
    JOIN [dbo].[GCRStages] AS S
        ON G.[StageId] = S.[StageId]
    LEFT JOIN [dbo].[GCRUnits] AS U
        ON G.[LeaderId] = U.[UnitId]
    WHERE U.[UnitId] IS NOT NULL OR [F2P] = 1