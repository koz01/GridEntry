USE [GridEntry]
GO
/****** Object:  StoredProcedure [dbo].[SelectPur_List]    Script Date: 05/15/2017 09:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[SelectPur_List]
@getFDate nvarchar(50),
@getTDate nvarchar(50),
@getCardID nvarchar(10)
AS
BEGIN
	IF @getCardID != '0'
		SELECT D.DetaiID, H.PurID as PurID, D.SrNo,Cr.CardID,D.Qty,D.TotalAmount ,D.Amount
					FROM Pur_Inv_His H,Pur_Detail_His D,Card Cr
					Where H.PurID=D.PurID
					And D.CardID = Cr.CardID
					And convert(varchar(30),H.PurDate,103)  BETWEEN convert(varchar(30),@getFDate,103) 
					And convert(char(26), @getTDate , 103)
					AND D.CardID =  @getCardID
	ELSE 
		SELECT D.DetaiID, H.PurID as PurID, D.SrNo,Cr.CardID,D.Qty,D.TotalAmount ,D.Amount
					FROM Pur_Inv_His H,Pur_Detail_His D,Card Cr
					Where H.PurID=D.PurID
					And D.CardID = Cr.CardID
					And convert(varchar(30),H.PurDate,103)  BETWEEN convert(varchar(30),@getFDate,103) 
					And convert(char(26), @getTDate , 103)
	

END




