USE LibraryManagementDB
GO

--INSERT Books
EXEC [Library].uspInsertBook '0-8400-6852-2', '978-0-8400-6852-1', 'L001', 'Álgebra y Trigonometría con Geometría Analítica', '', 2011, NULL, 5, 1, 10, 1
GO
EXEC [Library].uspInsertBook '9681811763', '978-9681811761', 'L002', 'Geometría Analítica', '', 1989, NULL, 1, 1, 2, 1
GO
EXEC [Library].uspInsertBook '6075502068', '978-6075502069', 'L003', 'Geometria y Trigonometria', '', 2019, NULL, 6, 1, 5, 1
GO
EXEC [Library].uspInsertBook '6075502092', '978-6075502090', 'L004', 'Algebra ', '', 2019, NULL, 6, 1, 5, 1
GO
EXEC [Library].uspInsertBook '6075265503', '978-6075265506', 'L005', 'Cálculo de una variable. Trascendentes tempranas', '', 2013, NULL, 5, 1, 3, 1
GO
EXEC [Library].uspInsertBook '6075191984', '978-6075191980', 'L006', 'Fisica para Ciencias e Ingenieria, Volumen 1', '', 2015, NULL, 5, 2, 3, 1
GO
EXEC [Library].uspInsertBook '6075191992', '978-6075191997', 'L007', 'Fisica para Ciencias e Ingenieria, Volumen 2', '', 2015, NULL, 5, 2, 3, 1
GO