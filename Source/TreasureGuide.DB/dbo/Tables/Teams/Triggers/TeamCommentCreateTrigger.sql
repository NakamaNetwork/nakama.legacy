CREATE TRIGGER [TeamCommentScoreCreateTrigger]
    ON [dbo].[TeamComments]
    FOR INSERT
    AS
    BEGIN
        SET NOCOUNT ON
        INSERT INTO [dbo].[TeamCommentScores]([TeamCommentId], [Value])
            SELECT 
                I.[Id], 1
            FROM INSERTED AS I
    END
GO