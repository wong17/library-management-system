-- CREATE STORED PROCEDURES FOR BookSubCategory TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. Error en la bd o no paso una validación, 2. No existe el recurso)

--INSERT BookSubCategory
IF OBJECT_ID('Library.uspInsertBookSubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertBookSubCategory;  
GO
CREATE PROC [Library].uspInsertBookSubCategory (
	@BookId INT,
	@SubCategoryId INT
)
AS
BEGIN
	--
	IF (@BookId IS NULL OR @BookId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Libro es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Book WHERE BookId = @BookId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un Libro con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@SubCategoryId IS NULL OR @SubCategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Sub categoría es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].SubCategory WHERE SubCategoryId = @SubCategoryId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Sub categoría con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].BookSubCategory WHERE BookId = @BookId AND SubCategoryId = @SubCategoryId)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe la relación entre el libro y la sub categoría proporcionada' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Library].BookSubCategory(BookId, SubCategoryId) VALUES (@BookId, @SubCategoryId)
		--
		SELECT 0 AS IsSuccess, 'Sub categoría del libro registrada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE BookSubCategory
IF OBJECT_ID('Library.uspDeleteBookSubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteBookSubCategory;  
GO
CREATE PROC [Library].uspDeleteBookSubCategory (
	@BookId INT,
	@SubCategoryId INT
)
AS
BEGIN
	--
	IF (@BookId IS NULL OR @BookId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Libro es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Book WHERE BookId = @BookId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un Libro con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@SubCategoryId IS NULL OR @SubCategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Sub categoría es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].SubCategory WHERE SubCategoryId = @SubCategoryId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Sub categoría con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].BookSubCategory WHERE BookId = @BookId AND SubCategoryId = @SubCategoryId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No existe la relación entre el libro y la sub categoría proporcionada' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Library].BookSubCategory WHERE BookId = @BookId AND SubCategoryId = @SubCategoryId
		--
		SELECT 0 AS IsSuccess, 'Sub categoría del libro eliminada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET BookSubCategory
IF OBJECT_ID('Library.uspGetBookSubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetBookSubCategory;  
GO
CREATE PROC [Library].uspGetBookSubCategory (
	@BookId INT = NULL,
	@SubCategoryId INT = NULL
)
AS
BEGIN
	--
	IF (@BookId IS NULL OR @BookId = '' OR @SubCategoryId IS NULL OR @SubCategoryId = '')
	BEGIN
		SELECT BookId, SubCategoryId, CreatedOn, ModifiedOn FROM [Library].BookSubCategory
	END
	ELSE
	BEGIN
		SELECT BookId, SubCategoryId, CreatedOn, ModifiedOn FROM [Library].BookSubCategory WHERE BookId = @BookId AND SubCategoryId = @SubCategoryId
	END
END
GO