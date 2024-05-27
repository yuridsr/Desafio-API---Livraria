using Livraria.Communication.Requests;
using Livraria.Communication.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Livraria.Controllers;
[Route("bookstore/[controller]")]
[ApiController]
public class booksController : ControllerBase
{
	List<Book> books = new List<Book>();


	[HttpPost]
	[ProducesResponseType(typeof(ResponseBookRegisterJson), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	public IActionResult createBook([FromBody] RequestBookRegisterJson resquest)
	{
		//retorna o ultimo id cadastrado
		int ultId = 1;
		foreach (var book in books)
		{
			if (book.Id > ultId)
			{ ultId = book.Id; }
		}

		//Adiciona o novo livro
		books.Add(new Book
		{
			Id = ultId + 1,
			Titulo = resquest.Titulo,
			Autor = resquest.Autor,
			Genero = resquest.Genero,
			Preco = resquest.Preco,
			Quantidade = resquest.Quantidade
		});

		//Retorno do endpoint
		var response = new ResponseBookRegisterJson
		{
			Id = ultId,
			Titulo = resquest.Titulo
		};

		return Created(string.Empty, response);
	}


	[HttpGet]
	[ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	public IActionResult getBook()
	{
		return Ok(books);
	}


	[HttpPut]
	[Route("{id}")]
	[ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	public IActionResult updateBook([FromBody] RequestBookUpdateJson request, [FromRoute] int id)
	{
		int index = -1; //indice para remover da list books
		foreach (var book in books)
		{
			if (book.Id == id)
			{
				index = books.IndexOf(book); //descobre o index do livro para remover
			}
		}
		if (index >= 0) // a remoção só acontece quando o index for válido
		{
			books[index] = new Book
			{
				Id = id,
				Autor = request.Autor,
				Genero = request.Genero,
				Preco = request.Preco,
				Quantidade = request.Quantidade,
				Titulo = request.Titulo
			};

			return NoContent();
		}
		return BadRequest("Id inválido"); //Caso o usuário coloque um id inexistente

	}

	[HttpDelete]
	[Route("{id}")]
	[ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	public IActionResult deleteBook([FromRoute] int id)
	{

		int index = -1; //indice para remover da list books
		foreach (var book in books)
		{
			if (book.Id == id)
			{
				index = books.IndexOf(book); //descobre o index do livro para remover
			}
		}

		if (index >= 0) // a remoção só acontece quando o index for válido
		{
			books.RemoveAt(index); // faz a remoção do livro
			return NoContent();
		}

		return BadRequest("Id inválido"); //Caso o usuário coloque um id inexistente
	}

}
