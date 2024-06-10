-- CREATE STORED PROCEDURES FOR BookSubCategory TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. No paso una validación, 2. No existe el recurso, 3. Error en la base de datos)

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
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--INSERT MANY BookSubCategory
IF OBJECT_ID('Library.uspInsertManyBookSubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertManyBookSubCategory;  
GO
CREATE PROC [Library].uspInsertManyBookSubCategory (
	@BookSubCategories [Library].BookSubCategoryType READONLY
)
AS
BEGIN
	-- Verificar que el Id del Libro no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @BookSubCategories AS bsc WHERE bsc.BookId IS NULL OR bsc.BookId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Libro es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todos los Libros
	IF EXISTS (SELECT 1 FROM @BookSubCategories AS bsc LEFT JOIN [Library].Book AS db ON bsc.BookId = db.BookId WHERE db.BookId IS NULL)
	BEGIN
		SELECT 2 AS IsSuccess, 'Uno o más Libros no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que el Id de la SubCategoria no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @BookSubCategories AS bsc WHERE bsc.SubCategoryId IS NULL OR bsc.SubCategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Sub Categoria es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todas las SubCategorias
	IF EXISTS (SELECT 1 FROM @BookSubCategories AS bsc LEFT JOIN [Library].SubCategory AS db ON bsc.SubCategoryId = db.SubCategoryId WHERE db.SubCategoryId IS NULL)
	BEGIN
		SELECT 2 AS IsSuccess, 'Una o más Sub Categorias no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que no vayan relaciones de Libros-SubCategorias repetidas
	IF EXISTS (SELECT 1 FROM @BookSubCategories AS bsc GROUP BY bsc.BookId, bsc.SubCategoryId HAVING COUNT(*) > 1)
    BEGIN
        SELECT 1 AS IsSuccess, 'Una o más relaciones Libro-Sub categoria están repetidos en la entrada' AS [Message];
        RETURN;
    END
	-- Verificar que no existan las relaciones de Libros-SubCategorias en la base de datos
	IF EXISTS (SELECT 1 FROM @BookSubCategories AS bsc INNER JOIN [Library].BookSubCategory AS db ON bsc.BookId = db.BookId AND bsc.SubCategoryId = db.SubCategoryId)
    BEGIN
        SELECT 1 AS IsSuccess, 'Una o más relaciones Libro-Sub categoria ya existen en la base de datos' AS [Message];
        RETURN;
    END
	--
	BEGIN TRAN
	BEGIN TRY
		-- Realizar la inserción 
		INSERT INTO [Library].BookSubCategory(BookId, SubCategoryId) SELECT bsc.BookId, bsc.SubCategoryId FROM @BookSubCategories AS bsc
		--
		SELECT 0 AS IsSuccess, 'Sub categoría del libro registrada exitosamente' AS [Message];
		--
		IF @@ERROR = 0
			IF @@TRANCOUNT > 0
				COMMIT TRAN;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;
		--
        SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message];
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
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE MANY BookSubCategory
IF OBJECT_ID('Library.uspDeleteManyBookSubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteManyBookSubCategory;  
GO
CREATE PROC [Library].uspDeleteManyBookSubCategory(
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
	BEGIN TRY
		--
		DELETE FROM [Library].BookSubCategory WHERE BookId = @BookId
		--
		SELECT 0 AS IsSuccess, 'Sub categorías del libro eliminadas exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE MANY BookSubCategory
IF OBJECT_ID('Library.uspUpdateManyBookSubCategory', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateManyBookSubCategory;  
GO
CREATE PROC [Library].uspUpdateManyBookSubCategory (
	@BookSubCategories [Library].BookSubCategoryType READONLY
)
AS
BEGIN
	-- Verificar que el Id del Libro no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @BookSubCategories AS bsc WHERE bsc.BookId IS NULL OR bsc.BookId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Libro es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todos los Libros
	IF EXISTS (SELECT 1 FROM @BookSubCategories AS bsc LEFT JOIN [Library].Book AS db ON bsc.BookId = db.BookId WHERE db.BookId IS NULL)
	BEGIN
		SELECT 2 AS IsSuccess, 'Uno o más Libros no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que el Id de la SubCategoria no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @BookSubCategories AS bsc WHERE bsc.SubCategoryId IS NULL OR bsc.SubCategoryId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Sub Categoria es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todas las SubCategorias
	IF EXISTS (SELECT 1 FROM @BookSubCategories AS bsc LEFT JOIN [Library].SubCategory AS db ON bsc.SubCategoryId = db.SubCategoryId WHERE db.SubCategoryId IS NULL)
	BEGIN
		SELECT 2 AS IsSuccess, 'Una o más Sub Categorias no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que no vayan relaciones de Libros-SubCategorias repetidas
	IF EXISTS (SELECT 1 FROM @BookSubCategories AS bsc GROUP BY bsc.BookId, bsc.SubCategoryId HAVING COUNT(*) > 1)
    BEGIN
        SELECT 1 AS IsSuccess, 'Una o más relaciones Libro-Sub categoria están repetidos en la entrada' AS [Message];
        RETURN;
    END
	-- Eliminar todas las relaciones existentes del libro
	BEGIN TRAN
	BEGIN TRY
		-- Obtener Id del libro
		DECLARE @BookId INT = (SELECT TOP 1 bsc.BookId FROM @BookSubCategories AS bsc)
		-- Para almacenar la respuesta del sp
		DECLARE @SPResult TABLE (
				IsSuccess INT,
				[Message] VARCHAR(255)
		)
		-- 1) Eliminar todas las sub categorias del libro
		INSERT INTO @SPResult EXEC [Library].uspDeleteManyBookSubCategory @BookId
		-- Verificar si todo salio bien
		IF ((SELECT IsSuccess FROM @SPResult) != 0)
		BEGIN
			--
			SELECT IsSuccess, [Message] FROM @SPResult
			--
			IF @@TRANCOUNT > 0
				ROLLBACK TRAN;
		END
		-- Limpiar resultados
		DELETE FROM @SPResult 
		-- 2) Insertar las nuevas relaciones 
		INSERT INTO @SPResult EXEC [Library].uspInsertManyBookSubCategory @BookSubCategories
		-- Verificar si todo salio bien
		IF ((SELECT IsSuccess FROM @SPResult) != 0)
		BEGIN
			--
			SELECT IsSuccess, [Message] FROM @SPResult
			--
			IF @@TRANCOUNT > 0
				ROLLBACK TRAN;
		END
		--
		SELECT 0 AS IsSuccess, 'Sub categorías del libro actualizadas exitosamente' AS [Message], @BookId AS Result
		--
		IF @@ERROR = 0
			IF @@TRANCOUNT > 0
				COMMIT TRAN;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;
		--
        SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message];
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