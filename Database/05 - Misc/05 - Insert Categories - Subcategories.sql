USE LibraryManagementDB
GO

--INSERT Categories
EXEC [Library].uspInsertCategory 'Matemática'
GO
EXEC [Library].uspInsertCategory 'Física'
GO

--INSERT Subcategories Matemática
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
--INSERT Subcategories Física
EXEC [Library].uspInsertSubCategory 2, 'Dinámica'
GO
EXEC [Library].uspInsertSubCategory 2, 'Mecánica'
GO
EXEC [Library].uspInsertSubCategory 2, 'Termodinámica'
GO
EXEC [Library].uspInsertSubCategory 2, 'Electromagnetismo'
GO
EXEC [Library].uspInsertSubCategory 2, 'Óptica'
GO
EXEC [Library].uspInsertSubCategory 2, 'Física Cuántica'
GO
EXEC [Library].uspInsertSubCategory 2, 'Física Nuclear'
GO
EXEC [Library].uspInsertSubCategory 2, 'Relatividad'
GO
EXEC [Library].uspInsertSubCategory 2, 'Oscilaciones y ondas mecánicas'
GO
EXEC [Library].uspInsertSubCategory 2, 'Electricidad y magnetismo'
GO
EXEC [Library].uspInsertSubCategory 2, 'Luz y óptica'
GO