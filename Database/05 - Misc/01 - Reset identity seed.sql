-- RESET IDENTITY SEED
DELETE FROM [University].Career
GO
DBCC CHECKIDENT ('[University].Career', RESEED, 0)
GO

DELETE FROM [University].Student
GO
DBCC CHECKIDENT ('[University].Student', RESEED, 0)
GO

DELETE FROM [Library].Publisher
GO
DBCC CHECKIDENT ('[Library].Publisher', RESEED, 0)
GO

DELETE FROM [Library].Category
GO
DBCC CHECKIDENT ('[Library].Category', RESEED, 0)
GO

DELETE FROM [Library].SubCategory
GO
DBCC CHECKIDENT ('[Library].SubCategory', RESEED, 0)
GO

DELETE FROM [Library].Author
GO
DBCC CHECKIDENT ('[Library].Author', RESEED, 0)
GO

DELETE FROM [Library].Book
GO
DBCC CHECKIDENT ('[Library].Book', RESEED, 0)
GO

DELETE FROM [Library].BookAuthor
GO
DBCC CHECKIDENT ('[Library].BookAuthor', RESEED, 0)
GO

DELETE FROM [Library].BookLoan
GO
DBCC CHECKIDENT ('[Library].BookLoan', RESEED, 0)
GO

DELETE FROM [Library].Monograph
GO
DBCC CHECKIDENT ('[Library].Monograph', RESEED, 0)
GO

DELETE FROM [Library].MonographLoan
GO
DBCC CHECKIDENT ('[Library].MonographLoan', RESEED, 0)
GO

TRUNCATE TABLE [Library].MonographAuthor
GO
DBCC CHECKIDENT ('[Library].MonographAuthor', RESEED, 0)
GO