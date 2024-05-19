-- CREATE STORED PROCEDURES FOR BookAuthor TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. Error en la bd o no paso una validación, 2. No existe el recurso)

--INSERT BookAuthor
IF OBJECT_ID('Library.uspInsertBookAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertBookAuthor;  
GO
CREATE PROC [Library].uspInsertBookAuthor (
	@BookId INT,
	@AuthorId INT
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
	IF EXISTS (SELECT 1 FROM [Library].BookAuthor WHERE BookId = @BookId AND AuthorId = @AuthorId)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe la relación entre el libro y la autor proporcionado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Library].BookAuthor (BookId, AuthorId) VALUES (@BookId, @AuthorId)
		--
		SELECT 0 AS IsSuccess, 'Autor del libro registrado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE BookAuthor
IF OBJECT_ID('Library.uspDeleteBookAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteBookAuthor;  
GO
CREATE PROC [Library].uspDeleteBookAuthor (
	@BookId INT,
	@AuthorId INT
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
	IF NOT EXISTS (SELECT 1 FROM [Library].BookAuthor WHERE BookId = @BookId AND AuthorId = @AuthorId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No existe la relación entre el libro y la autor proporcionado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Library].BookAuthor WHERE BookId = @BookId AND AuthorId = @AuthorId
		--
		SELECT 0 AS IsSuccess, 'Autor del libro eliminado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET BookAuthor
IF OBJECT_ID('Library.uspGetBookAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetBookAuthor;  
GO
CREATE PROC [Library].uspGetBookAuthor (
	@BookId INT = NULL,
	@AuthorId INT = NULL
)
AS
BEGIN
	--
	IF (@BookId IS NULL OR @BookId = '' OR @AuthorId IS NULL OR @AuthorId = '')
	BEGIN
		SELECT BookId, AuthorId, CreatedOn, ModifiedOn FROM [Library].BookAuthor
	END
	ELSE
	BEGIN
		SELECT BookId, AuthorId, CreatedOn, ModifiedOn FROM [Library].BookAuthor WHERE BookId = @BookId AND AuthorId = @AuthorId
	END
END
GO