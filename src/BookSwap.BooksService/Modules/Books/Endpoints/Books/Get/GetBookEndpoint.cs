﻿using BookSwap.BooksService.Modules.Books.Entities;
using BookSwap.BooksService.Modules.Books.Interfaces;
using BookSwap.Shared.Core.Data.Specifications;
using BookSwap.Shared.Core.Endpoints;
using BookSwap.Shared.Core.Swagger;
using Microsoft.AspNetCore.Mvc;

namespace BookSwap.BooksService.Modules.Books.Endpoints.Books.Get;

public class GetBookEndpoint : IEndpoint
{
    private readonly IBooksRepository _booksRepository;
    
    public GetBookEndpoint(IBooksRepository booksRepository) => _booksRepository = booksRepository;

    public IEndpointConventionBuilder MapEndpoint(IEndpointRouteBuilder builder)
        => builder.MapGet("/api/books/{id:guid}", HandleAsync)
            .WithName("GetBook")
            .WithTags("Books")
            .WithDescription("Gets a book by id")
            .ProducesNotFound()
            .WithOpenApi()
            .CacheOutput(p => p.Tag("books").SetVaryByRouteValue("id"));

    public async Task<Book> HandleAsync([FromRoute] Guid id)
    {
        var spec = new Specification<Book>()
            .AddCriteria(x => x.Id == id)
            .AddInclude(x => x.Author)
            .AddInclude(x => x.Genre);

        return await _booksRepository.FindAsync(spec);
    }
}