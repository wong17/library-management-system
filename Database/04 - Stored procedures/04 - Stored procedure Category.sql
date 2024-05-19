-- CREATE STORED PROCEDURES FOR Category TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. Error en la bd o no paso una validación, 2. No existe el recurso)

-- INSERT Category
IF OBJECT_ID('Library.uspInsertCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertCategory;  
GO
CREATE PROC [Library].uspInsertCategory (
	@Name VARCHAR(100)
)
AS
BEGIN
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Categoría es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z- ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Categoría solo puede tener mayúsculas, minúsculas, guiones y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Category WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una Categoría con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Library].Category ([Name]) VALUES (@Name)
		--
		SELECT 0 AS IsSuccess, 'Categoría registrada exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE Category
IF OBJECT_ID('Library.uspUpdateCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateCategory;  
GO
CREATE PROC [Library].uspUpdateCategory (
	@CategoryId INT,
	@Name VARCHAR(100)
)
AS
BEGIN
	--
	IF (@CategoryId IS NULL OR @CategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Categoría es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Category WHERE CategoryId = @CategoryId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Categoría con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Categoría es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z- ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Categoría solo puede tener mayúsculas, minúsculas, guiones y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Category WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una Categoría con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Library].Category SET [Name] = @Name WHERE CategoryId = @CategoryId
		--
		SELECT 0 AS IsSuccess, 'Categoría actualizada exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE Category
IF OBJECT_ID('Library.uspDeleteCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteCategory;  
GO
CREATE PROC [Library].uspDeleteCategory (
	@CategoryId INT
)
AS
BEGIN
	--
	IF (@CategoryId IS NULL OR @CategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Categoría es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Category WHERE CategoryId = @CategoryId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Categoría con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].SubCategory WHERE CategoryId = @CategoryId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar la Categoría porque hay Sub categorías asociados a ella' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Book WHERE CategoryId = @CategoryId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar la Categoría porque hay libros asociados a ella' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Library].Category WHERE CategoryId = @CategoryId
		--
		SELECT 0 AS IsSuccess, 'Categoría eliminada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET Category
IF OBJECT_ID('Library.uspGetCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetCategory;  
GO
CREATE PROC [Library].uspGetCategory (
	@CategoryId INT = NULL
)
AS
BEGIN
	--
	IF (@CategoryId IS NULL OR @CategoryId = '')
	BEGIN
		SELECT CategoryId, [Name] FROM [Library].Category
	END
	ELSE
	BEGIN
		SELECT CategoryId, [Name] FROM [Library].Category WHERE CategoryId = @CategoryId
	END
END
GO