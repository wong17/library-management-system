-- CREATE STORED PROCEDURES FOR Monograph TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. No paso una validación, 2. No existe el recurso, 3. Error en la base de datos)

--INSERT Monograph
IF OBJECT_ID('Library.uspInsertMonograph', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertMonograph;  
GO
CREATE PROC [Library].uspInsertMonograph (
	@Classification VARCHAR(25),
	@Title NVARCHAR(250),
	@Description NVARCHAR(500),
	@Tutor NVARCHAR(100),
	@PresentationDate DATE,
	@Image VARBINARY(MAX),
	@CareerId TINYINT,
	@IsActive BIT
)
AS
BEGIN
	--
	IF (@Classification IS NULL OR @Classification = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Clasificación de la monografía es obligatoria' AS [Message]
		RETURN
	END
	--
	IF (@Classification LIKE '%[^a-zA-Z0-9\-\. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Clasificación de la monografía solo puede tener letras, guiones, puntos y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Monograph WHERE [Classification] = @Classification)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una monografía con la misma clasificación' AS [Message]
		RETURN
	END
	--
	IF (@Title IS NULL OR @Title = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Titulo de la monografía es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Title LIKE '%[^a-zA-Z0-9\-\.\, ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Titulo de la monografía solo puede tener letras, números, guiones, puntos, comas y espacios' AS [Message]
		RETURN
	END
	--
	IF (@Tutor IS NULL OR @Tutor = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Tutor de la monografía es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Tutor LIKE '%[^a-zA-Z\.\, ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Tutor de la monografía solo puede tener letras, puntos, comas y espacios' AS [Message]
		RETURN
	END
	--
	IF (@PresentationDate IS NULL)
	BEGIN
		SELECT 1 AS IsSuccess, 'Fecha de presentación de la monografía es obligatoria' AS [Message]
		RETURN
	END
	--
	IF (@CareerId IS NULL OR @CareerId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Carrera a la que pertenece es obligatoria' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [University].Career WHERE CareerId= @CareerId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Carrera con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Library].Monograph ([Classification], Title, [Description], Tutor, PresentationDate, [Image], CareerId, IsActive) 
		VALUES (@Classification, @Title, @Description, @Tutor, @PresentationDate, @Image, @CareerId, @IsActive)
		--
		SELECT 0 AS IsSuccess, 'Monografía registrada exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE Monograph
IF OBJECT_ID('Library.uspUpdateMonograph', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateMonograph;  
GO
CREATE PROC [Library].uspUpdateMonograph (
	@MonographId INT,
	@Classification VARCHAR(25),
	@Title NVARCHAR(250),
	@Description NVARCHAR(500),
	@Tutor NVARCHAR(100),
	@PresentationDate DATE,
	@Image VARBINARY(MAX),
	@CareerId TINYINT,
	@IsActive BIT
)
AS
BEGIN
	-- MonographId
	IF (@MonographId IS NULL OR @MonographId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Monografía es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Monograph WHERE MonographId= @MonographId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Monografía con el Id proporcionado' AS [Message]
		RETURN
	END
	-- Classification
	IF (@Classification IS NULL OR @Classification = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Clasificación de la monografía es obligatoria' AS [Message]
		RETURN
	END
	--
	IF (@Classification LIKE '%[^a-zA-Z0-9\-\. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Clasificación de la monografía solo puede tener letras, guiones, puntos y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Monograph WHERE [Classification] = @Classification AND MonographId != @MonographId)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una monografía con la misma clasificación' AS [Message]
		RETURN
	END
	-- Title
	IF (@Title IS NULL OR @Title = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Titulo de la monografía es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Title LIKE '%[^a-zA-Z0-9\-\.\, ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Titulo de la monografía solo puede tener letras, números, guiones, puntos y espacios' AS [Message]
		RETURN
	END
	-- Tutor
	IF (@Tutor IS NULL OR @Tutor = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Tutor de la monografía es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Tutor LIKE '%[^a-zA-Z\.\, ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Tutor de la monografía solo puede tener letras, puntos, comas y espacios' AS [Message]
		RETURN
	END
	-- PresentationDate
	IF (@PresentationDate IS NULL)
	BEGIN
		SELECT 1 AS IsSuccess, 'Fecha de presentación de la monografía es obligatoria' AS [Message]
		RETURN
	END
	-- CareerId
	IF (@CareerId IS NULL OR @CareerId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Carrera a la que pertenece es obligatoria' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [University].Career WHERE CareerId = @CareerId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Carrera con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Library].Monograph 
		SET [Classification] = @Classification, Title = @Title, [Description] = @Description, Tutor = @Tutor, PresentationDate = @PresentationDate, 
			CareerId = @CareerId, [Image] = @Image,	IsActive = @IsActive, IsAvailable = @IsActive
		WHERE MonographId = @MonographId
		--
		SELECT 0 AS IsSuccess, 'Monografía actualizada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE IsAvailableMonograph
IF OBJECT_ID('Library.uspUpdateIsAvailableMonograph', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateIsAvailableMonograph;  
GO
CREATE PROC [Library].uspUpdateIsAvailableMonograph (
	@MonographId INT,
	@IsAvailable BIT
)
AS
BEGIN
	--
	IF (@MonographId IS NULL OR @MonographId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Monografía es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Monograph WHERE MonographId= @MonographId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Monografía con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Library].Monograph SET IsAvailable = @IsAvailable WHERE MonographId = @MonographId
		--
		SELECT 0 AS IsSuccess, 'Disponibilidad de la monografía actualizada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE Monograph
IF OBJECT_ID('Library.uspDeleteMonograph', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteMonograph;  
GO
CREATE PROC [Library].uspDeleteMonograph (
	@MonographId INT
)
AS
BEGIN
	--
	IF (@MonographId IS NULL OR @MonographId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Monografía es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Monograph WHERE MonographId= @MonographId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Monografía con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Library].Monograph SET IsActive = 0, IsAvailable = 0 WHERE MonographId = @MonographId
		--
		SELECT 0 AS IsSuccess, 'Monografía eliminada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET Monograph
IF OBJECT_ID('Library.uspGetMonograph', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetMonograph;  
GO
CREATE PROC [Library].uspGetMonograph (
	@MonographId INT = NULL
)
AS
BEGIN
	--
	IF (@MonographId IS NULL OR @MonographId = '')
	BEGIN
		SELECT MonographId, [Classification], Title, [Description], Tutor, PresentationDate, [Image], CareerId, IsActive, IsAvailable FROM [Library].Monograph
	END
	ELSE 
	BEGIN
		SELECT MonographId, [Classification], Title, [Description], Tutor, PresentationDate, [Image], CareerId, IsActive, IsAvailable FROM [Library].Monograph
		WHERE MonographId = @MonographId
	END
END
GO

-- GET Monographs filtered by authors, careers and presentation date
IF OBJECT_ID('Library.uspGetFilteredMonographs', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetFilteredMonographs;  
GO
CREATE PROC [Library].uspGetFilteredMonographs (
	@AuthorIds [Library].AuthorType READONLY,
	@CareerIds [University].CareerType READONLY,
	@BeginPresentationDate DATE = NULL,
	@EndPresentationDate DATE = NULL
)
AS
BEGIN
	--
	SELECT DISTINCT m.*
	FROM [Library].Monograph AS m
	LEFT JOIN [Library].MonographAuthor ma ON m.MonographId = ma.MonographId
	WHERE 
		-- Filtro por autores si @AuthorIds tiene registros
        (NOT EXISTS (SELECT 1 FROM @AuthorIds) OR ma.AuthorId IN (SELECT AuthorId FROM @AuthorIds))
		AND
		-- Filtro por carreras si @CareerIds tiene registros
		(NOT EXISTS (SELECT 1 FROM @CareerIds) OR m.CareerId IN (SELECT CareerId FROM @CareerIds))
		AND
		-- Filtro por fechas de presentación
		(
			-- Si sólo se proporciona la fecha de inicio
			(@BeginPresentationDate IS NOT NULL AND @EndPresentationDate IS NULL AND m.PresentationDate >= @BeginPresentationDate)
			OR
			-- Si sólo se proporciona la fecha final
			(@BeginPresentationDate IS NULL AND @EndPresentationDate IS NOT NULL AND m.PresentationDate <= @EndPresentationDate)
			OR
			-- Si se proporcionan ambas fechas
			(@BeginPresentationDate IS NOT NULL AND @EndPresentationDate IS NOT NULL AND m.PresentationDate BETWEEN @BeginPresentationDate AND @EndPresentationDate)
			OR
			-- Si no se proporciona ninguna fecha
			(@BeginPresentationDate IS NULL AND @EndPresentationDate IS NULL)
		);
END
GO