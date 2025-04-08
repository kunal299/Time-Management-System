USE [UCITMSDev]
GO

/****** Object:  StoredProcedure [dbo].[GetUserInfo]    Script Date: 21-10-2024 10:21:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[GetUserInfo] 
	-- Add the parameters for the stored procedure here
	@Email nvarchar(256)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT UserID, DisplayName, Email
	FROM Users
	WHERE Email = @Email;
END
GO


