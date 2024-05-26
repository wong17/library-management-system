-- CREATE STORED PROCEDURES FOR Student TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. Error en la bd o no paso una validación, 2. No existe el recurso)

-- CREATE STORED PROCEDURES FOR Student TABLE
USE LibraryManagementDB
GO

-- INSERT Student
IF OBJECT_ID('University.uspInsertStudent', 'P') IS NOT NULL  
    DROP PROCEDURE University.uspInsertStudent;  
GO
CREATE PROC University.uspInsertStudent (
	@FirstName VARCHAR(15),
	@SecondName VARCHAR(15),
	@FirstLastname VARCHAR(15),
	@SecondLastname VARCHAR(15),
	@Carnet CHAR(10),
	@PhoneNumber CHAR(8),
	@Sex CHAR,
	@Email VARCHAR(255),
	@Shift VARCHAR(10),
	@CareerId TINYINT
)
AS
BEGIN
	-- VALIDATIONS
	IF (@FirstName IS NULL OR @FirstName = '' OR @SecondName IS NULL OR @SecondName = '' OR 
		@FirstLastname IS NULL OR @FirstLastname = '' OR @SecondLastname IS NULL OR @SecondLastname = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombres y apellidos son campos obligatorios' AS [Message]
		RETURN
	END
	--
	IF (@FirstName LIKE '%[^a-zA-Z]%' OR @SecondName LIKE '%[^a-zA-Z]%' OR @FirstLastname LIKE '%[^a-zA-Z]%' OR	@SecondLastname LIKE '%[^a-zA-Z]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombres y apellidos solo puede tener mayúsculas y minúsculas' AS [Message]
		RETURN
	END
	--
	IF(@Carnet IS NULL OR @Carnet = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'El carnet es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Carnet NOT LIKE '[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9][A-Z]')
	BEGIN
		SELECT 1 AS IsSuccess, 'El carnet debe estar en el formato ####-####X' AS [Message]
		RETURN 
	END
	--
	IF EXISTS (SELECT 1 FROM [University].Student WHERE Carnet = @Carnet)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe un Estudiante con el mismo número de carnet' AS [Message]
		RETURN
	END
	--
	IF(@PhoneNumber IS NULL OR @PhoneNumber = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'El número de teléfono es un campo obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@PhoneNumber NOT LIKE '[5|7|8][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
	BEGIN
		SELECT 1 AS IsSuccess, 'El número de teléfono debe tener 8 digitos y empezar con 5, 7 u 8' AS [Message]
		RETURN 
	END
	--
	IF(@Sex IS NULL OR @Sex = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'El sexo es un campo obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Sex NOT IN('M', 'F'))
	BEGIN
		SELECT 1 AS IsSuccess, 'Sexo debe ser M o F' AS [Message]
		RETURN
	END
	--
	IF(@Email IS NULL OR @Email = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'El correo es un campo obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Email NOT LIKE '%[A-Z0-9][@][A-Z0-9]%[.][A-Z0-9]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'El correo debe contener un @ y un dominio' AS [Message]
		RETURN
	END
	--
	IF(@Shift IS NULL OR @Shift = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'El turno es un campo obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Shift NOT IN('Diurno', 'Sabatino'))
	BEGIN
		SELECT 1 AS IsSuccess, 'El turno no es correcto'AS [Message]
		RETURN
	END
	--
	IF (@CareerId IS NULL OR @CareerId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Carrera es un campo obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [University].Career WHERE CareerId = @CareerId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No existe una carrera con el Id ingresado' AS [Message]
		RETURN
	END
	-- INSERT STUDENT
	BEGIN TRY
		--
		INSERT INTO [University].Student (FirstName, SecondName, FirstLastname, SecondLastname, Carnet, PhoneNumber, Sex, Email, [Shift], CareerId) 
		VALUES (@FirstName, @SecondName, @FirstLastname, @SecondLastname, @Carnet, @PhoneNumber, @Sex, @Email, @Shift, @CareerId)
		-- 
		SELECT 0 AS IsSuccess, 'Estudiante registrado exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- UPDATE BorrowedBooks
IF OBJECT_ID('University.uspUpdateBorrowedBooksStudent', 'P') IS NOT NULL  
    DROP PROCEDURE University.uspUpdateBorrowedBooksStudent;  
GO
CREATE PROC University.uspUpdateBorrowedBooksStudent (
	@StudentId INT,
	@BorrowedBooks SMALLINT
)
AS
BEGIN
	-- VERIFICAR SI EXISTE EL ESTUDIANTE
	IF (@StudentId IS NULL OR @StudentId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Estudiante es un campo obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [University].Student WHERE StudentId = @StudentId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No existe un Estudiante con el Id ingresado' AS [Message]
		RETURN
	END
	-- 
	IF (@BorrowedBooks > 3)
	BEGIN
		SELECT 1 AS IsSuccess, 'Cantidad de libros prestados fuera de rango' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [University].Student SET BorrowedBooks = BorrowedBooks + @BorrowedBooks WHERE StudentId = @StudentId
		--
		SELECT 0 AS IsSuccess, 'Libros prestados actualizados exitosamente'AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- UPDATE BorrowedMonograph
IF OBJECT_ID('University.uspUpdateHasBorrowedMonographStudent', 'P') IS NOT NULL  
    DROP PROCEDURE University.uspUpdateHasBorrowedMonographStudent;  
GO
CREATE PROC University.uspUpdateHasBorrowedMonographStudent (
	@StudentId INT,
	@HasBorrowedMonograph BIT
)
AS
BEGIN
	--
	IF (@StudentId IS NULL OR @StudentId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Estudiante es un campo obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [University].Student WHERE StudentId = @StudentId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No existe un Estudiante con el Id ingresado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [University].Student SET HasBorrowedMonograph = @HasBorrowedMonograph WHERE StudentId = @StudentId
		--
		SELECT 0 AS IsSuccess, 'Monografía prestada actualizada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- UPDATE Fine
IF OBJECT_ID('University.uspUpdateFineStudent', 'P') IS NOT NULL  
    DROP PROCEDURE University.uspUpdateFineStudent;  
GO
CREATE PROC University.uspUpdateFineStudent (
	@StudentId INT,
	@Fine DECIMAL(7,2)
)
AS
BEGIN
	--
	IF (@StudentId IS NULL OR @StudentId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Estudiante es un campo obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [University].Student WHERE StudentId = @StudentId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No existe un Estudiante con el Id ingresado' AS [Message]
		RETURN
	END
	--
	IF (@Fine < 0 OR @Fine > 99999.99)
	BEGIN
		SELECT 1 AS IsSuccess, 'Costo de la multa fuera de rango' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [University].Student SET Fine = @Fine WHERE StudentId = @StudentId
		--
		SELECT 0 AS IsSuccess, 'Multa actualizada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- DELETE Student
IF OBJECT_ID('University.uspDeleteStudent', 'P') IS NOT NULL  
    DROP PROCEDURE University.uspDeleteStudent;  
GO
CREATE PROC University.uspDeleteStudent (
	@StudentId INT
)
AS
BEGIN
	--
	IF (@StudentId IS NULL OR @StudentId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Estudiante es un campo obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [University].Student WHERE StudentId = @StudentId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No existe un Estudiante con el Id ingresado' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].BookLoan WHERE StudentId = @StudentId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar el Estudiante porque hay una solicitud de prestamo de libro asociada' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].MonographLoan WHERE StudentId = @StudentId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar el Estudiante porque hay una solicitud de prestamo de monografía asociada' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [University].Student WHERE StudentId = @StudentId
		--
		SELECT 0 AS IsSuccess, 'Estudiante eliminado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- GET Student
IF OBJECT_ID('University.uspGetStudent', 'P') IS NOT NULL  
    DROP PROCEDURE University.uspGetStudent;  
GO 
CREATE PROC University.uspGetStudent (
	@StudentId INT = NULL
)
AS
BEGIN
	--
	IF (@StudentId IS NULL OR @StudentId = '')
	BEGIN
		SELECT [StudentId], [FirstName], [SecondName], [FirstLastname], [SecondLastname], [Carnet], [PhoneNumber], [Sex], [Email], 
			[Shift], [BorrowedBooks], [HasBorrowedMonograph], [Fine], [CreatedOn], [ModifiedOn], [CareerId]
		FROM [University].Student 
	END
	ELSE 
	BEGIN
		SELECT [StudentId], [FirstName], [SecondName], [FirstLastname], [SecondLastname], [Carnet], [PhoneNumber], [Sex], [Email], 
			[Shift], [BorrowedBooks], [HasBorrowedMonograph], [Fine], [CreatedOn], [ModifiedOn], [CareerId]
		FROM [University].Student 
		WHERE StudentId = @StudentId
	END
END
GO
