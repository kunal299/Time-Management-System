USE [UCITMSDev]
GO

/****** Object:  StoredProcedure [dbo].[AddOrUpdateEngagement]    Script Date: 21-10-2024 10:05:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddOrUpdateEngagement]
    @EngagementID INT,
    @Title NVARCHAR(200),
    @StartDate DATE,
    @EndDate DATE,
    @Description NVARCHAR(MAX),
    @CreatedBy INT,
    @ModifiedBy INT,
    @EngagementCategoryID INT,
    @TeamMembers dbo.TeamMemberType READONLY,
    @Tasks dbo.TaskType READONLY,
    @EngagementOwners dbo.EngagementOwnerType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    -- Declare variable to capture EngagementID
    DECLARE @NewEngagementID INT;

    -- Check if EngagementID is provided (for update)
    IF @EngagementID = 0
    BEGIN
        -- Insert new engagement
        INSERT INTO [dbo].[Engagements]
        (
            Title, StartDate, EndDate, Description, CreatedBy, ModifiedBy, CreatedOn, ModifiedOn, EngagementCategoryID
        )
        VALUES
        (
            @Title, @StartDate, @EndDate, @Description, @CreatedBy, @ModifiedBy, GETDATE(), GETDATE(), @EngagementCategoryID
        );

        -- Capture the new EngagementID
        SET @NewEngagementID = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        -- Update existing engagement
        UPDATE [dbo].[Engagements]
        SET Title = @Title,
            StartDate = @StartDate,
            EndDate = @EndDate,
            Description = @Description,
            ModifiedBy = @ModifiedBy,
            ModifiedOn = GETDATE(),
            EngagementCategoryID = @EngagementCategoryID
        WHERE EngagementID = @EngagementID;

        -- Use the provided EngagementID
        SET @NewEngagementID = @EngagementID;
    END

    -- Team Members: Update if exists, else insert
    MERGE INTO [dbo].[EngagementUserMapping] AS target
    USING @TeamMembers AS source
    ON target.EngagementID = @NewEngagementID AND target.UserID = source.UserID
    WHEN MATCHED THEN
        UPDATE SET
            target.StartDate = source.StartDate,
            target.EndDate = source.EndDate,
            target.MaxWeeklyHours = source.MaxWeeklyHours,
            target.ModifiedBy = @ModifiedBy,
            target.ModifiedOn = GETDATE()
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (EngagementID, UserID, StartDate, EndDate, MaxWeeklyHours, CreatedBy, ModifiedBy, CreatedOn, ModifiedOn)
        VALUES (@NewEngagementID, source.UserID, source.StartDate, source.EndDate, source.MaxWeeklyHours, @CreatedBy, @ModifiedBy, GETDATE(), GETDATE());

    -- Tasks: Update if exists, else insert
    MERGE INTO [dbo].[EngagementTaskMapping] AS target
    USING @Tasks AS source
    ON target.EngagementID = @NewEngagementID AND target.TaskID = source.TaskID
    WHEN MATCHED THEN
        UPDATE SET
            target.ModifiedBy = @ModifiedBy,
            target.ModifiedOn = GETDATE()
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (EngagementID, TaskID, CreatedBy, ModifiedBy, CreatedOn, ModifiedOn)
        VALUES (@NewEngagementID, source.TaskID, @CreatedBy, @ModifiedBy, GETDATE(), GETDATE());

    -- Owners: Update if exists, else insert
    MERGE INTO [dbo].[EngagementOwnersMapping] AS target
    USING @EngagementOwners AS source
    ON target.EngagementID = @NewEngagementID AND target.UserID = source.UserID
    WHEN MATCHED THEN
        UPDATE SET
            target.ModifiedBy = @ModifiedBy,
            target.ModifiedOn = GETDATE()
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (EngagementID, UserID, CreatedBy, ModifiedBy, CreatedOn, ModifiedOn)
        VALUES (@NewEngagementID, source.UserID, @CreatedBy, @ModifiedBy, GETDATE(), GETDATE());

    -- Return the EngagementID
    SELECT @NewEngagementID AS EngagementID;
END;
GO


