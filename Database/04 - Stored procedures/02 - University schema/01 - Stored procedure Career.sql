-- CREATE STORED PROCEDURES FOR Career TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. Error en la bd o no paso una validación, 2. No existe el recurso)

-- INSERT Career
IF OBJECT_ID('University.uspInsertCareer', 'P') IS NOT NULL  
    DROP PROCEDURE University.uspInsertCareer;  
GO
CREATE PROC University.uspInsertCareer (
	@Name VARCHAR(50)
)
AS
BEGIN
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la carrera es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Carrera solo puede tener mayúsculas, minúsculas y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [University].Career WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una Carrera con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [University].Career ([Name]) VALUES (@Name)
		--
		SELECT 0 AS IsSuccess, 'Carrera registrada exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- UPDATE Career
IF OBJECT_ID('University.uspUpdateCareer', 'P') IS NOT NULL  
    DROP PROCEDURE University.uspUpdateCareer;  
GO
CREATE PROC University.uspUpdateCareer (
	@CareerId TINYINT,
	@Name VARCHAR(50)
)
AS
BEGIN
	--
	IF (@CareerId IS NULL OR @CareerId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Carrera es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [University].Career WHERE CareerId = @CareerId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Carrera con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Carrera es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Carrera solo puede tener mayúsculas, minúsculas y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [University].Career WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una Carrera con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [University].Career SET [Name] = @Name WHERE CareerId = @CareerId
		--
		SELECT 0 AS IsSuccess, 'Carrera actualizada exitosamente'AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- DELETE Career
IF OBJECT_ID('University.uspDeleteCareer', 'P') IS NOT NULL  
    DROP PROCEDURE University.uspDeleteCareer;  
GO
CREATE PROC University.uspDeleteCareer (
	@CareerId TINYINT
)
AS
BEGIN
	--
	IF (@CareerId IS NULL OR @CareerId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Carrera es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [University].Career WHERE CareerId = @CareerId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Carrera con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [University].Student WHERE CareerId = @CareerId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar la Carrera porque hay Estudiantes en ella' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Monograph WHERE CareerId = @CareerId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar la Carrera porque hay Monografias asociadas a ella' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM University.Career WHERE CareerId = @CareerId
		--
		SELECT 0 AS IsSuccess, 'Carrera eliminada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- GET Career
IF OBJECT_ID('University.uspGetCareer', 'P') IS NOT NULL  
    DROP PROCEDURE University.uspGetCareer;  
GO
CREATE PROC University.uspGetCareer (
	@CareerId TINYINT = NULL
)
AS
BEGIN
	IF (@CareerId IS NULL OR @CareerId = '')
	BEGIN
		SELECT CareerId, [Name] FROM [University].Career
	END
	ELSE
	BEGIN
		SELECT CareerId, [Name] FROM [University].Career WHERE CareerId = @CareerId
	END
END
GO
