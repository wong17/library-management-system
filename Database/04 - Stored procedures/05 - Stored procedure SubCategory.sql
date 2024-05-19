-- CREATE STORED PROCEDURES FOR SubCategory TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. �xito, 1. Error en la bd o no paso una validaci�n, 2. No existe el recurso)

-- INSERT SubCategory
IF OBJECT_ID('Library.uspInsertSubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertSubCategory;  
GO
CREATE PROC [Library].uspInsertSubCategory (
	@CategoryId INT,
	@Name VARCHAR(100)
)
AS
BEGIN
	--
	IF (@CategoryId IS NULL OR @CategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Categor�a es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Category WHERE CategoryId = @CategoryId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Categor�a con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la SubCategor�a es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z- ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la SubCategor�a solo puede tener may�sculas, min�sculas, guiones y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].SubCategory WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una SubCategor�a con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Library].SubCategory (CategoryId, [Name]) VALUES (@CategoryId, @Name)
		--
		SELECT 0 AS IsSuccess, 'SubCategor�a registrada exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- UPDATE SubCategory
IF OBJECT_ID('Library.uspUpdateSubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateSubCategory;  
GO
CREATE PROC [Library].uspUpdateSubCategory (
	@SubCategoryId INT,
	@CategoryId INT,
	@Name VARCHAR(100)
)
AS
BEGIN
	--
	IF (@SubCategoryId IS NULL OR @SubCategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la SubCategor�a es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].SubCategory WHERE SubCategoryId = @SubCategoryId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una SubCategor�a con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@CategoryId IS NULL OR @CategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Categor�a es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Category WHERE CategoryId = @CategoryId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Categor�a con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la SubCategor�a es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z- ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la SubCategor�a solo puede tener may�sculas, min�sculas, guiones y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].SubCategory WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una SubCategor�a con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Library].SubCategory SET CategoryId = @CategoryId, [Name] = @Name WHERE SubCategoryId = @SubCategoryId
		--
		SELECT 0 AS IsSuccess, 'SubCategor�a actualizada exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- DELETE SubCategory
IF OBJECT_ID('Library.uspDeleteSubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteSubCategory;  
GO
CREATE PROC [Library].uspDeleteSubCategory (
	@SubCategoryId INT
)
AS
BEGIN
	--
	IF (@SubCategoryId IS NULL OR @SubCategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la SubCategor�a es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].SubCategory WHERE SubCategoryId = @SubCategoryId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una SubCategor�a con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].BookSubCategory WHERE SubCategoryId = @SubCategoryId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar la SubCategor�a porque hay libros asociados a ella' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Library].SubCategory WHERE SubCategoryId = @SubCategoryId
		--
		SELECT 0 AS IsSuccess, 'SubCategor�a eliminada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET SubCategory
IF OBJECT_ID('Library.uspGetSubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetSubCategory;  
GO
CREATE PROC [Library].uspGetSubCategory (
	@SubCategoryId INT = NULL
)
AS
BEGIN
	--
	IF (@SubCategoryId IS NULL OR @SubCategoryId = '')
	BEGIN
		SELECT SubCategoryId, CategoryId, [Name] FROM [Library].SubCategory
	END
	ELSE
	BEGIN
		SELECT SubCategoryId, CategoryId, [Name] FROM [Library].SubCategory WHERE SubCategoryId = @SubCategoryId
	END
END
GO