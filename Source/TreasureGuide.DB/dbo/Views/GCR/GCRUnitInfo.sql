CREATE VIEW [dbo].[GCRUnitInfo]
    AS SELECT
        G.[UnitId],
        G.[Order],
        ISNULL(G.[Name], U.[Name]) AS [Name],
        U.[Type] AS [Color]
    FROM [dbo].[GCRUnits] AS G
        JOIN [dbo].[Units] AS U
            ON G.[UnitId] = U.[Id]
