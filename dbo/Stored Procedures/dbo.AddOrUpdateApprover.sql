USE [UCITMSDev]
GO

/****** Object:  StoredProcedure [dbo].[AddOrUpdateApprover]    Script Date: 28-10-2024 01:57:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[AddOrUpdateApprover]
    @UserID INT,
    @PrimaryApproverID INT = NULL,
    @SecondaryApproverID INT = NULL,
    @ModUserID INT
AS
BEGIN
    -- Check and process the Primary Approver only if @PrimaryApproverID is not NULL
    IF @PrimaryApproverID IS NOT NULL
    BEGIN
        -- Check if the user already has a primary manager
        IF EXISTS (
            SELECT 1 
            FROM dbo.UserManagerMapping 
            WHERE UserID = @UserID AND isPrimary = 1
        )
        BEGIN
            -- Update the primary manager if the user already has a primary approver
            UPDATE dbo.UserManagerMapping
            SET ManagerID = @PrimaryApproverID,
                ModifiedBy = @ModUserID,
                ModifiedOn = GETDATE()
            WHERE UserID = @UserID AND isPrimary = 1;
        END
        ELSE
        BEGIN
            -- Insert a new row for the primary manager if not already assigned
            INSERT INTO dbo.UserManagerMapping (ManagerID, UserID, isPrimary, isSecondary, CreatedBy, ModifiedBy, CreatedOn, ModifiedOn)
            VALUES (@PrimaryApproverID, @UserID, 1, 0, @ModUserID, @ModUserID, GETDATE(), GETDATE());
        END
    END

    -- Check and process the Secondary Approver only if @SecondaryApproverID is not NULL
    IF @SecondaryApproverID IS NOT NULL
    BEGIN
        -- Check if the user already has a secondary manager
        IF EXISTS (
            SELECT 1 
            FROM dbo.UserManagerMapping 
            WHERE UserID = @UserID AND isSecondary = 1
        )
        BEGIN
            -- Update the secondary manager if the user already has a secondary approver
            UPDATE dbo.UserManagerMapping
            SET ManagerID = @SecondaryApproverID,
                ModifiedBy = @ModUserID,
                ModifiedOn = GETDATE()
            WHERE UserID = @UserID AND isSecondary = 1;
        END
        ELSE
        BEGIN
            -- Insert a new row for the secondary manager if not already assigned
            INSERT INTO dbo.UserManagerMapping (ManagerID, UserID, isPrimary, isSecondary, CreatedBy, ModifiedBy, CreatedOn, ModifiedOn)
            VALUES (@SecondaryApproverID, @UserID, 0, 1, @ModUserID, @ModUserID, GETDATE(), GETDATE());
        END
    END
	ELSE
	BEGIN
		DELETE FROM dbo.UserManagerMapping WHERE UserID = @UserID AND isSecondary = 1;
	END
END;
GO
