-- CREATE STORED PROCEDURES FOR Author TABLE
USE LibraryManagementDB
GO

-- INSERT Author
IF OBJECT_ID('Library.uspInsertAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertAuthor;  
GO
CREATE PROC [Library].uspInsertAuthor (
	@Name VARCHAR(100),
	@IsFormerGraduated BIT
)
AS
BEGIN
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre del Autor es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre del Autor solo puede tener mayúsculas, minúsculas, puntos y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Author WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una Autor con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Library].Author ([Name], IsFormerGraduated) VALUES (@Name, @IsFormerGraduated)
		--
		SELECT 0 AS IsSuccess, 'Autor registrado exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE Author
IF OBJECT_ID('Library.uspUpdateAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateAuthor;  
GO
CREATE PROC [Library].uspUpdateAuthor (
	@AuthorId INT,
	@Name VARCHAR(100),
	@IsFormerGraduated BIT
)
AS
BEGIN
	--
	IF (@AuthorId IS NULL OR @AuthorId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Autor es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Author WHERE AuthorId = @AuthorId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un Autor con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre del Autor es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre del Autor solo puede tener mayúsculas, minúsculas, puntos y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Author WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una Autor con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Library].Author SET [Name] = @Name, IsFormerGraduated = @IsFormerGraduated WHERE AuthorId = @AuthorId
		--
		SELECT 0 AS IsSuccess, 'Autor actualizado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE Author
IF OBJECT_ID('Library.uspDeleteAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteAuthor;  
GO
CREATE PROC [Library].uspDeleteAuthor (
	@AuthorId INT
)
AS
BEGIN
	--
	IF (@AuthorId IS NULL OR @AuthorId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Autor es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Author WHERE AuthorId = @AuthorId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un Autor con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].BookAuthor WHERE AuthorId = @AuthorId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar el Autor porque hay libros asociados a él' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].MonographAuthor WHERE AuthorId = @AuthorId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar el Autor porque hay monografías asociados a él' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Library].Author WHERE AuthorId = @AuthorId
		--
		SELECT 0 AS IsSuccess, 'Autor eliminado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET Author
IF OBJECT_ID('Library.uspGetAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetAuthor;  
GO
CREATE PROC [Library].uspGetAuthor (
	@AuthorId INT = NULL
)
AS
BEGIN
	--
	IF (@AuthorId IS NULL OR @AuthorId = '')
	BEGIN
		SELECT AuthorId, [Name], IsFormerGraduated FROM [Library].Author
	END
	ELSE
	BEGIN
		SELECT AuthorId, [Name], IsFormerGraduated FROM [Library].Author WHERE AuthorId = @AuthorId
	END
END
GO