USE LibraryManagementDB
GO

--INSERT Categories
EXEC [Library].uspInsertCategory 'Matem�tica'
GO
EXEC [Library].uspInsertCategory 'F�sica'
GO

--INSERT Subcategories
EXEC [Library].uspInsertSubCategory 1, '�lgebra'
GO
EXEC [Library].uspInsertSubCategory 1, 'Geometr�a'
GO
EXEC [Library].uspInsertSubCategory 1, 'Geometr�a Anal�tica'
GO
EXEC [Library].uspInsertSubCategory 1, 'Trigonometr�a'
GO
EXEC [Library].uspInsertSubCategory 1, 'C�lculo'
GO