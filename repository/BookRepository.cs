using BooksDataLoader.models;
using Cassandra.Data.Linq;
using Microsoft.Extensions.Configuration;

namespace BooksDataLoader.repository
{
	public class BookRepository : CassandraRepository<Book>
	{
		public BookRepository(IConfiguration config) : base(config, "book_id")
		{

		}
	}
}
