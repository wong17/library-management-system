﻿using AutoMapper;
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
        private readonly IBookRepository _repository = repository;
        private readonly IAuthorBll _authorBll = authorBll;
        private readonly IPublisherBll _publisherBll = publisherBll;
        private readonly ICategoryBll _categoryBll = categoryBll;
        private readonly ISubCategoryBll _subCategoryBll = subCategoryBll;
        private readonly IBookAuthorBll _bookAuthorBll = bookAuthorBll;
        private readonly IBookSubCategoryBll _bookSubCategoryBll = bookSubCategoryBll;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse> Create(Book entity) => await _repository.Create(entity);

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            // Comprobar si hay libros
            var response = await _repository.GetAll();
            if (response.Result is null || response.Result is not IEnumerable<Book> books)
                return response;

            // Obtener la editorial de cada uno de los libros
            var publishersId = books.Select(b => b.PublisherId).ToList();
            // Obtener la categoria de cada uno de los libros
            var categoriesId = books.Select(b => b.CategoryId).ToList();
            // Convertir books a BookDtos
            var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books).ToList();

            #region Publisher

            // 1. Comprobar si hay editoriales
            var publishersResponse = _publisherBll.GetAll();
            if (publishersResponse.Result.Result is not null && publishersResponse.Result.Result is IEnumerable<PublisherDto> publishers)
            {
                var publishersDictionary = publishers.ToDictionary(p => p.PublisherId);

                for (int i = 0; i < bookDtos.Count; i++)
                {
                    var bookDto = bookDtos[i];
                    var publisherId = publishersId[i];

                    if (publishersDictionary.TryGetValue(publisherId, out var publisherDto)) { bookDto.Publisher = publisherDto; }
                }

            }

            #endregion Publisher

            #region Category

            // 2. Comprobar si hay categorias
            var categoriesResponse = _categoryBll.GetAll();
            if (categoriesResponse.Result.Result is not null && categoriesResponse.Result.Result is IEnumerable<CategoryDto> categories)
            {
                var categoriesDictionary = categories.ToDictionary(c => c.CategoryId);

                for (int i = 0; i < bookDtos.Count; i++)
                {
                    var bookDto = bookDtos[i];
                    var categoryId = categoriesId[i];

                    if (categoriesDictionary.TryGetValue(categoryId, out var categoryDto)) { bookDto.Category = categoryDto; }
                }
            }

            #endregion Category

            #region Authors

            // 3. Comprobar si hay autores
            var authorsResponse = _authorBll.GetAll();
            var bookAuthorsResponse = _bookAuthorBll.GetAll();
            if ((authorsResponse.Result.Result is not null && authorsResponse.Result.Result is IEnumerable<AuthorDto> authors) && 
                (bookAuthorsResponse.Result.Result is not null && bookAuthorsResponse.Result.Result is IEnumerable<BookAuthorDto> bookAuthors))
            {
                var authorsDictionary = authors.ToDictionary(a => a.AuthorId);
                var bookAuthorsDictionary = ListHelper.ListToDictionary(bookAuthors.ToList(), ba => ba.BookId, ba => ba.AuthorId);
                
                for (int i = 0; i < bookDtos.Count; i++)
                {
                    var bookDto = bookDtos[i];
                    var allAuthors = new List<AuthorDto>();

                    // Si no se puede obtener la lista con id de todos los autores del libro actual...
                    if (!bookAuthorsDictionary.TryGetValue(bookDto.BookId, out var authorList))
                        continue;

                    // Recorrer lista de id de autores para obtener el AuthorDto
                    for (int j = 0; j < authorList.Count; j++)
                    {
                        var authorId = authorList[j];
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
            var subCategoryResponse = _subCategoryBll.GetAll();
            var bookSubCategoriesResponse = _bookSubCategoryBll.GetAll();
            if ((subCategoryResponse.Result.Result is not null && subCategoryResponse.Result.Result is IEnumerable<SubCategoryDto> subCategories) &&
                (bookSubCategoriesResponse.Result.Result is not null && bookSubCategoriesResponse.Result.Result is IEnumerable<BookSubCategoryDto> bookSubCategories))
            {
                var subCategoriesDictionary = subCategories.ToDictionary(s => s.SubCategoryId);
                var bookSubCategoriesDictionary = ListHelper.ListToDictionary(bookSubCategories.ToList(), bs => bs.BookId, bs => bs.SubCategoryId);

                for (int i = 0; i < bookDtos.Count; i++)
                {
                    var bookDto = bookDtos[i];
                    var allSubCategories = new List<SubCategoryDto>();

                    // Si no se puede obtener la lista con id de todas las subcategorias del libro actual...
                    if (!bookSubCategoriesDictionary.TryGetValue(bookDto.BookId, out var subCategoriesList))
                        continue;

                    // Recorrer lista de id de subcategorias para obtener la SubCategoryDto
                    for (int j = 0; j < subCategoriesList.Count; j++)
                    {
                        var subCategoryId = subCategoriesList[j];
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
            var response = await _repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is null || response.Result is not Book book)
                return response;

            // Obtener la editorial de cada uno de los libros
            var publisherId = book.PublisherId;
            // Obtener la categoria de cada uno de los libros
            var categoryId = book.CategoryId;
            // Convertir Book a BookDto
            var bookDto = _mapper.Map<BookDto>(book);

            // 1. Comprobar si existe la editorial
            var publisherResponse = _publisherBll.GetById(publisherId);
            if (publisherResponse.Result.Result is not null && publisherResponse.Result.Result is PublisherDto publisher)
            {
                bookDto.Publisher = publisher;
            }

            // 2. Comprobar si existe la categoria
            var categoryResponse = _categoryBll.GetById(categoryId);
            if (categoryResponse.Result.Result is not null && categoryResponse.Result.Result is CategoryDto category)
            {
                bookDto.Category = category;
            }

            // 3. Comprobar si hay autores
            var authorsResponse = _authorBll.GetAll();
            var bookAuthorsResponse = _bookAuthorBll.GetAll();
            if ((authorsResponse.Result.Result is not null && authorsResponse.Result.Result is IEnumerable<AuthorDto> authors) &&
                (bookAuthorsResponse.Result.Result is not null && bookAuthorsResponse.Result.Result is IEnumerable<BookAuthorDto> bookAuthors))
            {
                var authorsDictionary = authors.ToDictionary(a => a.AuthorId);
                var bookAuthorsDictionary = ListHelper.ListToDictionary(bookAuthors.ToList(), ba => ba.BookId, ba => ba.AuthorId);

                var allAuthors = new List<AuthorDto>();

                // Si se puede obtener la lista con id de todos los autores del libro...
                if (bookAuthorsDictionary.TryGetValue(bookDto.BookId, out var authorList))
                {
                    // Recorrer lista de id de autores para obtener el AuthorDto
                    for (int j = 0; j < authorList.Count; j++)
                    {
                        var authorId = authorList[j];
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
            var subCategoryResponse = _subCategoryBll.GetAll();
            var bookSubCategoriesResponse = _bookSubCategoryBll.GetAll();
            if ((subCategoryResponse.Result.Result is not null && subCategoryResponse.Result.Result is IEnumerable<SubCategoryDto> subCategories) &&
                (bookSubCategoriesResponse.Result.Result is not null && bookSubCategoriesResponse.Result.Result is IEnumerable<BookSubCategoryDto> bookSubCategories))
            {
                var subCategoriesDictionary = subCategories.ToDictionary(s => s.SubCategoryId);
                var bookSubCategoriesDictionary = ListHelper.ListToDictionary(bookSubCategories.ToList(), bs => bs.BookId, bs => bs.SubCategoryId);

                var allSubCategories = new List<SubCategoryDto>();

                // Si se puede obtener la lista con id de todas las subcategorias del libro actual...
                if (bookSubCategoriesDictionary.TryGetValue(bookDto.BookId, out var subCategoriesList))
                {
                    // Recorrer lista de id de subcategorias para obtener la SubCategoryDto
                    for (int j = 0; j < subCategoriesList.Count; j++)
                    {
                        var subCategoryId = subCategoriesList[j];
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

        public async Task<ApiResponse> Update(Book entity) => await _repository.Update(entity);

        public async Task<ApiResponse> GetFilteredBook(FilterBookDto filterBookDto)
        {
            // Comprobar si hay libros
            var response = await _repository.GetFilteredBook(filterBookDto);
            if (response.Result is null || response.Result is not IEnumerable<Book> books)
                return response;

            // Obtener la editorial de cada uno de los libros
            var publishersId = books.Select(b => b.PublisherId).ToList();
            // Obtener la categoria de cada uno de los libros
            var categoriesId = books.Select(b => b.CategoryId).ToList();
            // Convertir books a BookDtos
            var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books).ToList();

            #region Publisher

            // 1. Comprobar si hay editoriales
            var publishersResponse = _publisherBll.GetAll();
            if (publishersResponse.Result.Result is not null && publishersResponse.Result.Result is IEnumerable<PublisherDto> publishers)
            {
                var publishersDictionary = publishers.ToDictionary(p => p.PublisherId);

                for (int i = 0; i < bookDtos.Count; i++)
                {
                    var bookDto = bookDtos[i];
                    var publisherId = publishersId[i];

                    if (publishersDictionary.TryGetValue(publisherId, out var publisherDto)) { bookDto.Publisher = publisherDto; }
                }

            }

            #endregion Publisher

            #region Category

            // 2. Comprobar si hay categorias
            var categoriesResponse = _categoryBll.GetAll();
            if (categoriesResponse.Result.Result is not null && categoriesResponse.Result.Result is IEnumerable<CategoryDto> categories)
            {
                var categoriesDictionary = categories.ToDictionary(c => c.CategoryId);

                for (int i = 0; i < bookDtos.Count; i++)
                {
                    var bookDto = bookDtos[i];
                    var categoryId = categoriesId[i];

                    if (categoriesDictionary.TryGetValue(categoryId, out var categoryDto)) { bookDto.Category = categoryDto; }
                }
            }

            #endregion Category

            #region Authors

            // 3. Comprobar si hay autores
            var authorsResponse = _authorBll.GetAll();
            var bookAuthorsResponse = _bookAuthorBll.GetAll();
            if ((authorsResponse.Result.Result is not null && authorsResponse.Result.Result is IEnumerable<AuthorDto> authors) &&
                (bookAuthorsResponse.Result.Result is not null && bookAuthorsResponse.Result.Result is IEnumerable<BookAuthorDto> bookAuthors))
            {
                var authorsDictionary = authors.ToDictionary(a => a.AuthorId);
                var bookAuthorsDictionary = ListHelper.ListToDictionary(bookAuthors.ToList(), ba => ba.BookId, ba => ba.AuthorId);

                for (int i = 0; i < bookDtos.Count; i++)
                {
                    var bookDto = bookDtos[i];
                    var allAuthors = new List<AuthorDto>();

                    // Si no se puede obtener la lista con id de todos los autores del libro actual...
                    if (!bookAuthorsDictionary.TryGetValue(bookDto.BookId, out var authorList))
                        continue;

                    // Recorrer lista de id de autores para obtener el AuthorDto
                    for (int j = 0; j < authorList.Count; j++)
                    {
                        var authorId = authorList[j];
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
            var subCategoryResponse = _subCategoryBll.GetAll();
            var bookSubCategoriesResponse = _bookSubCategoryBll.GetAll();
            if ((subCategoryResponse.Result.Result is not null && subCategoryResponse.Result.Result is IEnumerable<SubCategoryDto> subCategories) &&
                (bookSubCategoriesResponse.Result.Result is not null && bookSubCategoriesResponse.Result.Result is IEnumerable<BookSubCategoryDto> bookSubCategories))
            {
                var subCategoriesDictionary = subCategories.ToDictionary(s => s.SubCategoryId);
                var bookSubCategoriesDictionary = ListHelper.ListToDictionary(bookSubCategories.ToList(), bs => bs.BookId, bs => bs.SubCategoryId);

                for (int i = 0; i < bookDtos.Count; i++)
                {
                    var bookDto = bookDtos[i];
                    var allSubCategories = new List<SubCategoryDto>();

                    // Si no se puede obtener la lista con id de todas las subcategorias del libro actual...
                    if (!bookSubCategoriesDictionary.TryGetValue(bookDto.BookId, out var subCategoriesList))
                        continue;

                    // Recorrer lista de id de subcategorias para obtener la SubCategoryDto
                    for (int j = 0; j < subCategoriesList.Count; j++)
                    {
                        var subCategoryId = subCategoriesList[j];
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
