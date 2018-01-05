CREATE VIEW [dbo].[UserRoles]
    AS SELECT U.[Id],R.[Name] FROM [dbo].[UserProfiles] AS U
    JOIN [dbo].[AspNetUserRoles] AS UR
        ON U.[Id] = UR.[UserId]
    JOIN [dbo].[AspNetRoles] AS R
        ON R.[Id] = UR.[RoleId]