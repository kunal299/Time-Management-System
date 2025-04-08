USE [UCITMSDev]
GO

/****** Object:  StoredProcedure [dbo].[GetUserManagerInfo]    Script Date: 27-10-2024 21:43:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[GetUserManagerInfo]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
		UMM.MappingID,
		U.UserID , 
		U.DisplayName AS UserName, 
		UMM.ManagerID,
		UM.DisplayName AS ManagerName,
		[isPrimary],
		[isSecondary]

    FROM 
        [dbo].[UserManagerMapping] as UMM,
		[dbo].[Users] UM,
		[dbo].[Users] U
        Where UMM.ManagerID = UM.UserID
        and  UMM.UserID = U.UserID
    
END
GO


