CREATE VIEW [dbo].[UserRoleSummaries] AS
    SELECT U.[Id] AS [UserId], R.[Name] AS [RoleName] FROM [dbo].[AspNetUserRoles] AS UR
        JOIN [dbo].[AspNetRoles] AS R
            ON UR.[RoleId] = R.[Id]
        JOIN [dbo].[UserProfiles] AS U
            ON U.[Id] = UR.[UserId]