CREATE DATABASE ShopBridge

USE ShopBridge
CREATE TABLE [dbo].[Item](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemDocument]    Script Date: 26-09-2020 20:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemDocument](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[FileName] [nvarchar](50) NOT NULL,
	[UploadLocation] [nvarchar](50) NOT NULL,
	[DocumentUrl] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ItemDocument] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ItemDocument]  WITH CHECK ADD  CONSTRAINT [FK_ItemDocument_Item] FOREIGN KEY([ItemID])
REFERENCES [dbo].[Item] ([ID])
GO
ALTER TABLE [dbo].[ItemDocument] CHECK CONSTRAINT [FK_ItemDocument_Item]
GO
/****** Object:  StoredProcedure [dbo].[spGetAllItems]    Script Date: 26-09-2020 20:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetAllItems]
AS
BEGIN
	DECLARE @jsonResult nvarchar(max)

	SET @jsonResult = (
	SELECT TOP 5
		[Item].[ID] As ItemID, 
		[Item].[Name] AS ItemName,
		[Item].[Description] AS ItemDescription,
		[Item].[Price] AS ItemPrice,
		([ItemDocument].[UploadLocation] + [ItemDocument].[DocumentUrl]) AS ImageURL
		FROM [dbo].[Item] WITH (NOLOCK)
		LEFT JOIN [dbo].[ItemDocument] WITH (NOLOCK)
			ON [Item].[ID] = [ItemDocument].[ItemID]
		ORDER BY [Item].[ID] DESC
	FOR JSON PATH, INCLUDE_NULL_VALUES )

	SELECT @jsonResult
END
GO
/****** Object:  StoredProcedure [dbo].[spInsertItem]    Script Date: 26-09-2020 20:49:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spInsertItem]
(
@ItemName nvarchar(50),
@ItemDescription nvarchar(200),
@ItemPrice decimal(18,2),
@FileName nvarchar(50)= null,
@FileExtension nvarchar (10)= null
)
AS
BEGIN 
	BEGIN TRANSACTION [AddItem]
	BEGIN TRY
		--Step 1 : Insert item details recieved from user
		INSERT INTO [dbo].[Item]([Name], [Description], [Price], [DateCreated], [DateModified])
		VALUES(@ItemName, @ItemDescription, @ItemPrice, GETDATE(),GETDATE())
		
		DECLARE @insertedItemID INT
		--Step 2 : Get last inserted Item ID
		SET  @insertedItemID = (SELECT SCOPE_IDENTITY())

		IF(ISNULL(@FileName,'') != '' AND ISNULL(@FileExtension,'') != '')
		BEGIN
			--Step 3 : Insert itemDocument details in DB as well so application has a track about uploaded images
			INSERT INTO [dbo].[ItemDocument]([ItemID], [FileName], [UploadLocation], [DocumentUrl])
			VALUES(@insertedItemID,@FileName, '/uploads/images/',@FileName)
	
			DECLARE @insertedItemDocumentID INT
			--Step 4 : Get last inserted Item docuument ID
			SET  @insertedItemDocumentID = (SELECT SCOPE_IDENTITY())
	
			--Step 5: Update document URL, this will be helpful for actual thumbnail
			UPDATE [dbo].[ItemDocument]
				SET [ItemDocument].[DocumentUrl] = (@FileName +'_' + convert(nvarchar(max),@insertedItemID) + @FileExtension)
				WHERE [ItemDocument].[ID] = @insertedItemDocumentID
		END
		
		COMMIT TRANSACTION [AddItem]

		SELECT @insertedItemID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION [AddItem]
		select ERROR_MESSAGE()
	END CATCH
END
GO
USE [master]
GO
ALTER DATABASE [ShopBridge] SET  READ_WRITE 
GO
