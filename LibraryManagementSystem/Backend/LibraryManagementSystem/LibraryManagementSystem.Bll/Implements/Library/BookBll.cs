using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Helpers;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class BookBll(IBookRepository repository, IAuthorBll authorBll, IPublisherBll publisherBll, ICategoryBll categoryBll,
        ISubCategoryBll subCategoryBll, IBookAuthorBll bookAuthorBll, IBookSubCategoryBll bookSubCategoryBll, IMapper mapper) : IBookBll
    {
        public async Task<ApiResponse> Create(BookInsertDto entity) => await repository.Create(mapper.Map<Book>(entity));

        public async Task<ApiResponse> Delete(int id) => await repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            // Comprobar si hay libros
            var response = await repository.GetAll();
            if (response.Result is not IEnumerable<Book> books)
                return response;

            // Obtener la editorial de cada uno de los libros
            var bookList = books.ToList();
            var publishersId = bookList.Select(b => b.PublisherId).ToList();
            // Obtener la categoria de cada uno de los libros
            var categoriesId = bookList.Select(b => b.CategoryId).ToList();
            // Convertir books a BookDtos
            var bookDtos = mapper.Map<IEnumerable<BookDto>>(books).ToList();

            #region Publisher

            // 1. Comprobar si hay editoriales
            var publishersResponse = publisherBll.GetAll();
            if (publishersResponse.Result.Result is IEnumerable<PublisherDto> publishers)
            {
                var publishersDictionary = publishers.ToDictionary(p => p.PublisherId);

                for (var i = 0; i < bookDtos.Count; i++)
                {
                    var bookDto = bookDtos[i];
                    var publisherId = publishersId[i];

                    if (publishersDictionary.TryGetValue(publisherId, out var publisherDto)) { bookDto.Publisher = publisherDto; }
                }
            }

            #endregion Publisher

            #region Category

            // 2. Comprobar si hay categorias
            var categoriesResponse = categoryBll.GetAll();
            if (categoriesResponse.Result.Result is IEnumerable<CategoryDto> categories)
            {
                var categoriesDictionary = categories.ToDictionary(c => c.CategoryId);

                for (var i = 0; i < bookDtos.Count; i++)
                {
                    var bookDto = bookDtos[i];
                    var categoryId = categoriesId[i];

                    if (categoriesDictionary.TryGetValue(categoryId, out var categoryDto)) { bookDto.Category = categoryDto; }
                }
            }

            #endregion Category

            #region Authors

            // 3. Comprobar si hay autores
            var authorsResponse = authorBll.GetAll();
            var bookAuthorsResponse = bookAuthorBll.GetAll();
            if (authorsResponse.Result.Result is IEnumerable<AuthorDto> authors &&
                bookAuthorsResponse.Result.Result is IEnumerable<BookAuthorDto> bookAuthors)
            {
                var authorsDictionary = authors.ToDictionary(a => a.AuthorId);
                var bookAuthorsDictionary = ListHelper.ListToDictionary(bookAuthors.ToList(), ba => ba.BookId, ba => ba.AuthorId);

                foreach (var bookDto in bookDtos)
                {
                    var allAuthors = new List<AuthorDto>();

                    // Si no se puede obtener la lista con id de todos los autores del libro actual...
                    if (!bookAuthorsDictionary.TryGetValue(bookDto.BookId, out var authorList))
                        continue;

                    // Recorrer lista de id de autores para obtener el AuthorDto
                    foreach (var authorId in authorList)
                    {
                        // Obtener autor en base a su id
                        if (authorsDictionary.TryGetValue(authorId, out var authorDto))
                        {
                            allAuthors.Add(authorDto);
                        }
                    }
                    // Asignar lista
                    bookDto.Authors = allAuthors;
                }
            }

            #endregion Authors

            #region SubCategories

            // 4. Comprobar si hay subcategorias
            var subCategoryResponse = subCategoryBll.GetAll();
            var bookSubCategoriesResponse = bookSubCategoryBll.GetAll();
            if (subCategoryResponse.Result.Result is IEnumerable<SubCategoryDto> subCategories &&
                bookSubCategoriesResponse.Result.Result is IEnumerable<BookSubCategoryDto> bookSubCategories)
            {
                var subCategoriesDictionary = subCategories.ToDictionary(s => s.SubCategoryId);
                var bookSubCategoriesDictionary = ListHelper.ListToDictionary(bookSubCategories.ToList(), bs => bs.BookId, bs => bs.SubCategoryId);

                foreach (var bookDto in bookDtos)
                {
                    var allSubCategories = new List<SubCategoryDto>();

                    // Si no se puede obtener la lista con id de todas las subcategorias del libro actual...
                    if (!bookSubCategoriesDictionary.TryGetValue(bookDto.BookId, out var subCategoriesList))
                        continue;

                    // Recorrer lista de id de subcategorias para obtener la SubCategoryDto
                    foreach (var subCategoryId in subCategoriesList)
                    {
                        // Obtener subcategoria en base a su id
                        if (subCategoriesDictionary.TryGetValue(subCategoryId, out var subCategoryDto))
                        {
                            allSubCategories.Add(subCategoryDto);
                        }
                    }
                    // Asignar lista
                    bookDto.SubCategories = allSubCategories;
                }
            }

            #endregion SubCategories

            // Retornar Dtos
            response.Result = bookDtos;

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is not Book book)
                return response;

            // Obtener la editorial de cada uno de los libros
            var publisherId = book.PublisherId;
            // Obtener la categoria de cada uno de los libros
            var categoryId = book.CategoryId;
            // Convertir Book a BookDto
            var bookDto = mapper.Map<BookDto>(book);

            // 1. Comprobar si existe la editorial
            var publisherResponse = publisherBll.GetById(publisherId);
            if (publisherResponse.Result.Result is PublisherDto publisher)
            {
                bookDto.Publisher = publisher;
            }

            // 2. Comprobar si existe la categoria
            var categoryResponse = categoryBll.GetById(categoryId);
            if (categoryResponse.Result.Result is CategoryDto category)
            {
                bookDto.Category = category;
            }

            // 3. Comprobar si hay autores
            var authorsResponse = authorBll.GetAll();
            var bookAuthorsResponse = bookAuthorBll.GetAll();
            if (authorsResponse.Result.Result is IEnumerable<AuthorDto> authors &&
                bookAuthorsResponse.Result.Result is IEnumerable<BookAuthorDto> bookAuthors)
            {
                var authorsDictionary = authors.ToDictionary(a => a.AuthorId);
                var bookAuthorsDictionary = ListHelper.ListToDictionary(bookAuthors.ToList(), ba => ba.BookId, ba => ba.AuthorId);

                var allAuthors = new List<AuthorDto>();

                // Si se puede obtener la lista con id de todos los autores del libro...
                if (bookAuthorsDictionary.TryGetValue(bookDto.BookId, out var authorList))
                {
                    // Recorrer lista de id de autores para obtener el AuthorDto
                    foreach (var authorId in authorList)
                    {
                        // Obtener autor en base a su id
                        if (authorsDictionary.TryGetValue(authorId, out var authorDto))
                        {
                            allAuthors.Add(authorDto);
                        }
                    }

                    // Asignar lista
                    bookDto.Authors = allAuthors;
                }
            }

            // 4. Comprobar si hay subcategorias
            var subCategoryResponse = subCategoryBll.GetAll();
            var bookSubCategoriesResponse = bookSubCategoryBll.GetAll();
            if (subCategoryResponse.Result.Result is IEnumerable<SubCategoryDto> subCategories &&
                bookSubCategoriesResponse.Result.Result is IEnumerable<BookSubCategoryDto> bookSubCategories)
            {
                var subCategoriesDictionary = subCategories.ToDictionary(s => s.SubCategoryId);
                var bookSubCategoriesDictionary = ListHelper.ListToDictionary(bookSubCategories.ToList(), bs => bs.BookId, bs => bs.SubCategoryId);

                var allSubCategories = new List<SubCategoryDto>();

                // Si se puede obtener la lista con id de todas las subcategorias del libro actual...
                if (bookSubCategoriesDictionary.TryGetValue(bookDto.BookId, out var subCategoriesList))
                {
                    // Recorrer lista de id de subcategorias para obtener la SubCategoryDto
                    foreach (var subCategoryId in subCategoriesList)
                    {
                        // Obtener subcategoria en base a su id
                        if (subCategoriesDictionary.TryGetValue(subCategoryId, out var subCategoryDto))
                        {
                            allSubCategories.Add(subCategoryDto);
                        }
                    }

                    // Asignar lista
                    bookDto.SubCategories = allSubCategories;
                }
            }

            // Retornar Dto
            response.Result = bookDto;

            return response;
        }

        public async Task<ApiResponse> Update(BookUpdateDto entity) => await repository.Update(mapper.Map<Book>(entity));

        public async Task<ApiResponse> GetFilteredBook(FilterBookDto filterBookDto)
        {
            // Comprobar si hay libros
            var response = await repository.GetFilteredBook(filterBookDto);
            if (response.Result is not IEnumerable<Book> books)
                return response;

            // Obtener la editorial de cada uno de los libros
            var bookList = books.ToList();
            var publishersId = bookList.Select(b => b.PublisherId).ToList();
            // Obtener la categoria de cada uno de los libros
            var categoriesId = bookList.Select(b => b.CategoryId).ToList();
            // Convertir books a BookDtos
            var bookDtos = mapper.Map<IEnumerable<BookDto>>(books).ToList();

            #region Publisher

            // 1. Comprobar si hay editoriales
            var publishersResponse = publisherBll.GetAll();
            if (publishersResponse.Result.Result is IEnumerable<PublisherDto> publishers)
            {
                var publishersDictionary = publishers.ToDictionary(p => p.PublisherId);

                for (var i = 0; i < bookDtos.Count; i++)
                {
                    var bookDto = bookDtos[i];
                    var publisherId = publishersId[i];

                    if (publishersDictionary.TryGetValue(publisherId, out var publisherDto)) { bookDto.Publisher = publisherDto; }
                }
            }

            #endregion Publisher

            #region Category

            // 2. Comprobar si hay categorias
            var categoriesResponse = categoryBll.GetAll();
            if (categoriesResponse.Result.Result is IEnumerable<CategoryDto> categories)
            {
                var categoriesDictionary = categories.ToDictionary(c => c.CategoryId);

                for (var i = 0; i < bookDtos.Count; i++)
                {
                    var bookDto = bookDtos[i];
                    var categoryId = categoriesId[i];

                    if (categoriesDictionary.TryGetValue(categoryId, out var categoryDto)) { bookDto.Category = categoryDto; }
                }
            }

            #endregion Category

            #region Authors

            // 3. Comprobar si hay autores
            var authorsResponse = authorBll.GetAll();
            var bookAuthorsResponse = bookAuthorBll.GetAll();
            if (authorsResponse.Result.Result is IEnumerable<AuthorDto> authors &&
                bookAuthorsResponse.Result.Result is IEnumerable<BookAuthorDto> bookAuthors)
            {
                var authorsDictionary = authors.ToDictionary(a => a.AuthorId);
                var bookAuthorsDictionary = ListHelper.ListToDictionary(bookAuthors.ToList(), ba => ba.BookId, ba => ba.AuthorId);

                foreach (var bookDto in bookDtos)
                {
                    var allAuthors = new List<AuthorDto>();

                    // Si no se puede obtener la lista con id de todos los autores del libro actual...
                    if (!bookAuthorsDictionary.TryGetValue(bookDto.BookId, out var authorList))
                        continue;

                    // Recorrer lista de id de autores para obtener el AuthorDto
                    foreach (var authorId in authorList)
                    {
                        // Obtener autor en base a su id
                        if (authorsDictionary.TryGetValue(authorId, out var authorDto))
                        {
                            allAuthors.Add(authorDto);
                        }
                    }
                    // Asignar lista
                    bookDto.Authors = allAuthors;
                }
            }

            #endregion Authors

            #region SubCategories

            // 4. Comprobar si hay subcategorias
            var subCategoryResponse = subCategoryBll.GetAll();
            var bookSubCategoriesResponse = bookSubCategoryBll.GetAll();
            if (subCategoryResponse.Result.Result is IEnumerable<SubCategoryDto> subCategories &&
                bookSubCategoriesResponse.Result.Result is IEnumerable<BookSubCategoryDto> bookSubCategories)
            {
                var subCategoriesDictionary = subCategories.ToDictionary(s => s.SubCategoryId);
                var bookSubCategoriesDictionary = ListHelper.ListToDictionary(bookSubCategories.ToList(), bs => bs.BookId, bs => bs.SubCategoryId);

                foreach (var bookDto in bookDtos)
                {
                    var allSubCategories = new List<SubCategoryDto>();

                    // Si no se puede obtener la lista con id de todas las subcategorias del libro actual...
                    if (!bookSubCategoriesDictionary.TryGetValue(bookDto.BookId, out var subCategoriesList))
                        continue;

                    // Recorrer lista de id de subcategorias para obtener la SubCategoryDto
                    foreach (var subCategoryId in subCategoriesList)
                    {
                        // Obtener subcategoria en base a su id
                        if (subCategoriesDictionary.TryGetValue(subCategoryId, out var subCategoryDto))
                        {
                            allSubCategories.Add(subCategoryDto);
                        }
                    }
                    // Asignar lista
                    bookDto.SubCategories = allSubCategories;
                }
            }

            #endregion SubCategories

            // Retornar Dtos
            response.Result = bookDtos;

            return response;
        }
    }
}