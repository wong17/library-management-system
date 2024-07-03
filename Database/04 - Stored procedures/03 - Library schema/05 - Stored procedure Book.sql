-- CREATE STORED PROCEDURES FOR Book TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. No paso una validación, 2. No existe el recurso, 3. Error en la base de datos)

--INSERT Book
IF OBJECT_ID('Library.uspInsertBook', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertBook;  
GO
CREATE PROC [Library].uspInsertBook (
	@ISBN10 VARCHAR(13),
	@ISBN13 VARCHAR(17),
	@Classification VARCHAR(25),
	@Title NVARCHAR(100),
	@Description NVARCHAR(500),
	@PublicationYear SMALLINT,
	@Image VARBINARY(MAX),
	@PublisherId INT,
	@CategoryId INT,
	@NumberOfCopies SMALLINT,
	@IsActive BIT
)
AS
BEGIN
	--
	IF (@ISBN10 IS NULL OR @ISBN10 = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'ISBN10 del libro es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@ISBN10 LIKE '%[^0-9\-]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'ISBN10 del libro solo puede tener números y guiones' AS [Message]
		RETURN
	END
	--
	IF (@ISBN13 IS NULL OR @ISBN13 = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'ISBN13 del libro es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@ISBN13 LIKE '%[^0-9\-]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'ISBN13 del libro solo puede tener números y guiones' AS [Message]
		RETURN
	END
	--
	IF (@Classification IS NULL OR @Classification = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Clasificación del libro es obligatoria' AS [Message]
		RETURN
	END
	--
	IF (@Classification LIKE '%[^a-zA-Z0-9\-\. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Clasificación del libro solo puede tener letras, números, guiones, puntos y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Book WHERE [Classification] = @Classification)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe un Libro con la misma clasificación' AS [Message]
		RETURN
	END
	--
	IF (@Title IS NULL OR @Title = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Titulo del libro es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Title LIKE '%[^a-zA-Z0-9\-\.\, ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Titulo del libro solo puede tener letras, números, guiones, puntos. comas y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Book WHERE Title = @Title)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe un Libro con el mismo titulo' AS [Message]
		RETURN
	END
	--
	IF (@PublicationYear <= 0)
	BEGIN
		SELECT 1 AS IsSuccess, 'Año de publicación del Libro fuera de rango' AS [Message]
		RETURN
	END
	--
	IF (@PublisherId IS NULL OR @PublisherId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Editorial del libro es obligatoria' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Publisher WHERE PublisherId = @PublisherId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Editorial con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@CategoryId IS NULL OR @CategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Categoría del libro es obligatoria' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Category WHERE CategoryId = @CategoryId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Categoría con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@NumberOfCopies < 0)
	BEGIN
		SELECT 1 AS IsSuccess, 'Número de copias del Libro fuera de rango' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Library].Book (ISBN10, ISBN13, [Classification], Title, [Description], PublicationYear, [Image], PublisherId, CategoryId, NumberOfCopies, IsActive)
		VALUES (@ISBN10, @ISBN13, @Classification, @Title, @Description, @PublicationYear, @Image, @PublisherId, @CategoryId, @NumberOfCopies, @IsActive)
		--
		SELECT 0 AS IsSuccess, 'Libro registrado exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE Book
IF OBJECT_ID('Library.uspUpdateBook', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateBook;  
GO
CREATE PROC [Library].uspUpdateBook (
	@BookId INT,
	@ISBN10 VARCHAR(13),
	@ISBN13 VARCHAR(17),
	@Classification VARCHAR(25),
	@Title NVARCHAR(100),
	@Description NVARCHAR(500),
	@PublicationYear SMALLINT,
	@Image VARBINARY(MAX),
	@PublisherId INT,
	@CategoryId INT,
	@NumberOfCopies SMALLINT,
	@IsActive BIT
)
AS
BEGIN
	-- BookId
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
	-- ISBN10
	IF (@ISBN10 IS NULL OR @ISBN10 = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'ISBN10 del libro es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@ISBN10 LIKE '%[^0-9\-]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'ISBN10 del libro solo puede tener números y guiones' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Book WHERE ISBN10 = @ISBN10 AND BookId != @BookId)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe un Libro con el mismo ISBN10' AS [Message]
		RETURN
	END
	-- ISBN13
	IF (@ISBN13 IS NULL OR @ISBN13 = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'ISBN13 del libro es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@ISBN13 LIKE '%[^0-9\-]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'ISBN13 del libro solo puede tener números y guiones' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Book WHERE ISBN13 = @ISBN13 AND BookId != @BookId)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe un Libro con el mismo ISBN13' AS [Message]
		RETURN
	END
	-- Classification
	IF (@Classification IS NULL OR @Classification = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Clasificación del libro es obligatoria' AS [Message]
		RETURN
	END
	--
	IF (@Classification LIKE '%[^a-zA-Z0-9\-\. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Clasificación del libro solo puede tener letras, números, guiones, puntos y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Book WHERE [Classification] = @Classification AND BookId != @BookId)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe un Libro con la misma clasificación' AS [Message]
		RETURN
	END
	-- Title
	IF (@Title IS NULL OR @Title = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Titulo del libro es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Title LIKE '%[^a-zA-Z0-9\-\.\, ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Titulo del libro solo puede tener letras, números, guiones, puntos, comas y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Book WHERE Title = @Title AND BookId != @BookId)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe un Libro con el mismo titulo' AS [Message]
		RETURN
	END
	-- PublicationYear
	IF (@PublicationYear <= 0)
	BEGIN
		SELECT 1 AS IsSuccess, 'Año de publicación del Libro fuera de rango' AS [Message]
		RETURN
	END
	-- PublisherId
	IF (@PublisherId IS NULL OR @PublisherId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Editorial del libro es obligatoria' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Publisher WHERE PublisherId = @PublisherId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Editorial con el Id proporcionado' AS [Message]
		RETURN
	END
	-- CategoryId
	IF (@CategoryId IS NULL OR @CategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Categoría del libro es obligatoria' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Category WHERE CategoryId = @CategoryId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Categoría con el Id proporcionado' AS [Message]
		RETURN
	END
	-- NumberOfCopies
	IF (@NumberOfCopies < 0)
	BEGIN
		SELECT 1 AS IsSuccess, 'Número de copias del Libro fuera de rango' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Library].Book 
			SET ISBN10 = @ISBN10, ISBN13 = @ISBN13, [Classification] = @Classification, Title = @Title, [Description] = @Description, 
				PublicationYear = @PublicationYear, [Image] = @Image, PublisherId = @PublisherId, CategoryId = @CategoryId, 
				NumberOfCopies = @NumberOfCopies, IsActive = @IsActive, IsAvailable = @IsActive
		WHERE BookId = @BookId
		--
		SELECT 0 AS IsSuccess, 'Libro actualizado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE Book
IF OBJECT_ID('Library.uspDeleteBook', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteBook;  
GO
CREATE PROC [Library].uspDeleteBook (
	@BookId INT
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
	DECLARE @IsActive BIT = (SELECT IsActive FROM [Library].Book WHERE BookId = @BookId)
	DECLARE @IsThereABookLoanPending BIT = (SELECT 1 FROM [Library].BookLoan WHERE BookId = @BookId AND [State] IN ('CREADA', 'PRESTADO'))
	
	IF (@IsActive = 1 AND @IsThereABookLoanPending IS NOT NULL)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar el Libro porque hay solicitudes de préstamo asociadas' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Library].Book SET IsActive = 0, IsAvailable = 0 WHERE BookId = @BookId
		--
		SELECT 0 AS IsSuccess, 'Libro eliminado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET Book
IF OBJECT_ID('Library.uspGetBook', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetBook;  
GO
CREATE PROC [Library].uspGetBook (
	@BookId INT = NULL
)
AS
BEGIN
	--
	IF (@BookId IS NULL OR @BookId = '')
	BEGIN
		SELECT BookId, ISBN10, ISBN13, [Classification], Title, [Description], PublicationYear, [Image], IsActive, PublisherId, CategoryId, NumberOfCopies, BorrowedCopies, IsAvailable 
		FROM [Library].Book
	END
	ELSE 
	BEGIN
		SELECT BookId, ISBN10, ISBN13, [Classification], Title, [Description], PublicationYear, [Image], IsActive, PublisherId, CategoryId, NumberOfCopies, BorrowedCopies, IsAvailable 
		FROM [Library].Book
		WHERE BookId = @BookId
	END
END
GO

--UPDATE BorrowedCopies
IF OBJECT_ID('Library.uspUpdateBorrowedBooks', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateBorrowedBooks;  
GO
CREATE PROC [Library].uspUpdateBorrowedBooks (
	@BookId INT,
	@Sign SMALLINT
)
AS
BEGIN
	-- VERIFICAR SI EXISTE EL LIBRO
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
	-- OBTENER NUMERO DE COPIAS TOTALES DEL LIBRO
	DECLARE @NumberOfCopies SMALLINT = (SELECT NumberOfCopies FROM [Library].Book WHERE BookId = @BookId)
	-- OBTENER NUMERO DE COPIAS PRESTADAS DEL LIBRO
	DECLARE @BorrowedCopies SMALLINT = (SELECT BorrowedCopies FROM [Library].Book WHERE BookId = @BookId)
	-- SUMAR/RESTAR 1 COPIA EN LIBROS PRESTADOS
	SET @BorrowedCopies = @BorrowedCopies + @Sign
	-- VERIFICAR SI LA CANTIDAD DE COPIAS PRESTADAS NO ES NEGATIVO NI MAYOR A LA CANTIDAD DE LIBROS TOTALES
	IF (@BorrowedCopies > @NumberOfCopies OR @BorrowedCopies < 0)
	BEGIN
		SELECT 1 AS IsSuccess, 'Cantidad de libros préstados fuera de rango' AS [Message]
		RETURN
	END
	-- SI ERA LA ULTIMA COPIA DISPONIBLE, HACER EL PRESTAMO Y MARCAR EL LIBRO COMO NO DISPONIBLE
	IF (@NumberOfCopies = @BorrowedCopies)
	BEGIN
		--
		UPDATE [Library].Book SET BorrowedCopies = @BorrowedCopies, IsAvailable = 0 WHERE BookId = @BookId
		--
		SELECT 0 AS IsSuccess, 'Cantidad de libros préstados actualizados correctamente, libro no disponible para préstamo' AS [Message]
	END
	ELSE
	BEGIN
		--
		UPDATE [Library].Book SET BorrowedCopies = @BorrowedCopies WHERE BookId = @BookId
		--
		SELECT 0 AS IsSuccess, 'Cantidad de libros préstados actualizados correctamente' AS [Message]
	END
END
GO

-- GET Books filtered by authors, publishers, years, categories and subcategories
IF OBJECT_ID('Library.uspGetFilteredBooks', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetFilteredBooks;  
GO
CREATE PROC [Library].uspGetFilteredBooks (
	@AuthorIds [Library].AuthorType READONLY,
	@PublisherIds [Library].PublisherType READONLY,
	@CategoryIds [Library].CategoryType READONLY,
	@SubCategoryIds [Library].SubCategoryType READONLY,
	@PublicationYear SMALLINT = NULL
)
AS
BEGIN
	--
	SELECT DISTINCT b.*
    FROM [Library].Book b
    LEFT JOIN [Library].BookAuthor ba ON b.BookId = ba.BookId
    LEFT JOIN [Library].BookSubCategory bs ON b.BookId = bs.BookId
    WHERE 
        -- Filtro por autores si @AuthorIds tiene registros
        (NOT EXISTS (SELECT 1 FROM @AuthorIds) OR ba.AuthorId IN (SELECT AuthorId FROM @AuthorIds))
        AND
        -- Filtro por editoriales si @PublisherIds tiene registros
        (NOT EXISTS (SELECT 1 FROM @PublisherIds) OR b.PublisherId IN (SELECT PublisherId FROM @PublisherIds))
        AND
        -- Filtro por categorías si @CategoryIds tiene registros
        (NOT EXISTS (SELECT 1 FROM @CategoryIds) OR b.CategoryId IN (SELECT CategoryId FROM @CategoryIds))
        AND
        -- Filtro por subcategorías si @SubCategoryIds tiene registros
        (NOT EXISTS (SELECT 1 FROM @SubCategoryIds) OR bs.SubCategoryId IN (SELECT SubCategoryId FROM @SubCategoryIds))
        AND
        -- Filtro por año de publicación si @PublicationYear no es NULL
        (@PublicationYear IS NULL OR b.PublicationYear = @PublicationYear);
END
GO