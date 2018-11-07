﻿CREATE TRIGGER [CommentReplyTrigger]
    ON [dbo].[TeamComments]
    FOR INSERT
    AS
    BEGIN
        SET NOCOUNT ON
        INSERT INTO [dbo].[Notifications]([UserId], [EventType], [EventId], [TriggerUserId])
            SELECT 
                P.[SubmittedById], 2, P.[Id], I.[SubmittedById]
            FROM INSERTED AS I
            JOIN [dbo].[TeamComments] AS P
                ON I.[ParentId] = P.[Id]
            WHERE P.[SubmittedById] != I.[SubmittedById]
    END
GO