CREATE TRIGGER [TeamScoreCreateTrigger]
    ON [dbo].[Teams]
    FOR INSERT
    AS
    BEGIN
        SET NOCOUNT ON
        INSERT INTO [dbo].[TeamScores]([TeamId], [Value])
            SELECT 
                I.[Id], 1
            FROM INSERTED AS I
    END
GO