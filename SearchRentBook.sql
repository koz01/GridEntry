USE [LearnEF]
GO
/****** Object:  StoredProcedure [dbo].[SearchRentBook]    Script Date: 05/15/2017 09:26:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[SearchRentBook]
(
    @BookName        varchar(30) = null,
    @MemberName      varchar(30) = null,
    @Category		 varchar(30)  = null,
    @IsIssueBook     varchar(30)  = null,
    @IsRentBook		 varchar(30)  = null
)
AS
BEGIN
    DECLARE @query    nvarchar(1000);

    SET @query = 'SELECT b.autokey,m.MemberName as memberId,b.StartDate,b.IssueDate,a.BookName as bookId,
						c.Category_Name as CategoryId,b.NumberOfDay,b.status
					FROM Book a, BookRent b, member m, Category c
					WHERE b.BookId= a.autokey
					AND b.MemberId = m.autokey
					AND b.CategoryId = c.Category_Id
					AND b.status = 1'
    SET @query = @query + ' AND 1=1'
 
    IF @BookName != ''
        SET @query = @query + ' AND a.BookName LIKE ''' + @BookName + '%'''
 
    IF @MemberName != ''
        SET @query = @query + ' AND m.MemberName LIKE ''' + @MemberName + '%'''
        
    IF @Category !=0
        SET @query = @query + ' AND b.categoryid = '+ @Category    
  
    IF (@IsRentBook = 1 and @IsIssueBook =0)
		SET @query = @query + 'AND  convert(char(26), getdate(), 103) BETWEEN convert(char(26), b.StartDate, 103) 
							AND convert(char(26), b.IssueDate, 103)'
    ELSE IF (@IsIssueBook = 1 and @IsRentBook = 0)
		SET @query = @query + 'AND convert(char(26), b.IssueDate, 103) < convert(char(26), getdate(), 103)'
		
    EXEC (@query)
END
