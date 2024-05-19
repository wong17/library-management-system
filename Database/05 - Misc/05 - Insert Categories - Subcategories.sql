USE LibraryManagementDB
GO

--INSERT Categories
EXEC [Library].uspInsertCategory 'Matemática'
GO
EXEC [Library].uspInsertCategory 'Física'
GO

--INSERT Subcategories
EXEC [Library].uspInsertSubCategory 1, 'Álgebra'
GO
EXEC [Library].uspInsertSubCategory 1, 'Geometría'
GO
EXEC [Library].uspInsertSubCategory 1, 'Geometría Analítica'
GO
EXEC [Library].uspInsertSubCategory 1, 'Trigonometría'
GO
EXEC [Library].uspInsertSubCategory 1, 'Cálculo'
GO