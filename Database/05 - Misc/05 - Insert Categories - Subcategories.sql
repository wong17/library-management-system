USE LibraryManagementDB
GO

--INSERT Categories
EXEC [Library].uspInsertCategory 'Matem�tica'
GO
EXEC [Library].uspInsertCategory 'F�sica'
GO

--INSERT Subcategories Matem�tica
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
--INSERT Subcategories F�sica
EXEC [Library].uspInsertSubCategory 2, 'Din�mica'
GO
EXEC [Library].uspInsertSubCategory 2, 'Mec�nica'
GO
EXEC [Library].uspInsertSubCategory 2, 'Termodin�mica'
GO
EXEC [Library].uspInsertSubCategory 2, 'Electromagnetismo'
GO
EXEC [Library].uspInsertSubCategory 2, '�ptica'
GO
EXEC [Library].uspInsertSubCategory 2, 'F�sica Cu�ntica'
GO
EXEC [Library].uspInsertSubCategory 2, 'F�sica Nuclear'
GO
EXEC [Library].uspInsertSubCategory 2, 'Relatividad'
GO
EXEC [Library].uspInsertSubCategory 2, 'Oscilaciones y ondas mec�nicas'
GO
EXEC [Library].uspInsertSubCategory 2, 'Electricidad y magnetismo'
GO
EXEC [Library].uspInsertSubCategory 2, 'Luz y �ptica'
GO