using BooksDataLoader.models;
using BooksDataLoader.repository;
using Cassandra;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

IConfiguration config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables()
				.Build();

var authorRepository = new AuthorRepository(config);
var bookRepository = new BookRepository(config);


void InitAuthors()
{
	try
	{
		StreamReader reader = new StreamReader(config.GetValue<string>("authorsFile"));
		string? line;
		List<Task> dbWriteTasks = new List<Task>();
		while ((line = reader.ReadLine()) != null)
		{
			try
			{
				// parse
				line = line.Substring(line.IndexOf("{"));
				var lineJson = JObject.Parse(line);

				var key = lineJson.GetValue("key").ToString();
				var authorId = key.Replace("/authors/", "");
				var authorName = lineJson.GetValue("name").ToString();

				// construct author object
				var author = new Author
				{
					Id = authorId,
					Name = authorName
				};

				// write to repository
				Console.WriteLine("Saving author " + author.Name + "...");
				dbWriteTasks.Add(authorRepository.InsertAsync(author));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		Task.WaitAll(dbWriteTasks.ToArray());
	} catch (IOException e)
	{
		Console.WriteLine(e.Message);
	}
}

void InitWorks()
{
	try
	{
		StreamReader reader = new StreamReader(config.GetValue<string>("worksFile"));
		string? line;
		List<Task> dbWriteTasks = new List<Task>();
		while ((line = reader.ReadLine()) != null)
		{
			try
			{
				// parse
				line = line.Substring(line.IndexOf("{"));
				var lineJson = JObject.Parse(line);


				var authorIds = lineJson.GetValue("authors").Select(
									(val) => val.SelectToken("author.key").ToString().Replace("/authors/", "")
								).ToList();
				var authorNames = authorIds.Select(val => authorRepository.Get(val)?.Name ?? "Unknown author").ToList();

				var date = lineJson.SelectToken("created.value").ToObject<DateTime>();

				// construct book object
				var book = new Book
				{
					Id = lineJson.GetValue("key").ToString().Replace("/works/", ""),
					Name = lineJson.GetValue("title").ToString(),
					Description = lineJson.SelectToken("description.value")?.ToString() ?? "",
					PublishedDate = new LocalDate(date.Year, date.Month, date.Day),
					AuthorIds = authorIds,
					AuthorNames = authorNames,
					CoverIds = lineJson.GetValue("covers")?.ToList().Select(val => val.ToString()).ToList() ?? new List<string>()
				};

				// write to repository
				Console.WriteLine("Saving book " + book.Name + "...");
				dbWriteTasks.Add(bookRepository.InsertAsync(book));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		Task.WaitAll(dbWriteTasks.ToArray());
	}
	catch (IOException e)
	{
		Console.WriteLine(e.Message);
	}
}

//InitAuthors();
InitWorks();
