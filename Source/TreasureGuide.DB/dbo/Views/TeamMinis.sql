CREATE VIEW [dbo].[TeamMinis] AS
(
    SELECT
        [Id], 
        [StageId], 
        U.[0] AS [Slot1], 
        U.[1] AS [Slot2], 
        U.[2] AS [Slot3], 
        U.[3] AS [Slot4], 
        U.[4] AS [Slot5], 
        U.[5] AS [Slot6] 
    FROM (
        SELECT
            TM.[Id], TM.[StageId], TU.[UnitId], TU.[Position], TU.[Sub]
        FROM [dbo].[Teams] AS TM
            JOIN [dbo].[TeamUnits] AS TU
                ON TM.[Id] = TU.[TeamId] AND TU.[Sub] = 0
    ) T
    PIVOT
    (
        SUM([UnitId])
        FOR [Position] IN ([0],[1],[2],[3],[4],[5])
    ) U
)
