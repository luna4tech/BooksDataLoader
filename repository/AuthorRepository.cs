using BooksDataLoader.models;
using Cassandra.Data.Linq;
using Microsoft.Extensions.Configuration;

namespace BooksDataLoader.repository
{
	public class AuthorRepository : CassandraRepository<Author>
	{
		public AuthorRepository(IConfiguration config) : base(config, "author_id")
		{

		}
	}
}
