using Cassandra;
using Cassandra.Mapping;
using Cassandra.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksDataLoader.models
{
	[Table("book_by_id")]
	public class Book
	{
		[PartitionKey(0)] [Column("book_id")]
		public string Id { get; set; } = default!;
		
		[Column("book_name")]
		public string Name { get; set; } = default!;

		[Column("book_description")]
		public string Description { get; set; } = default!;

		[Column("published_date")]
		public LocalDate PublishedDate { get; set; } = default!;

		[Column("cover_ids")]
		public List<string> CoverIds { get; set; } = default!;

		[Column("author_ids")]
		public List<string> AuthorIds { get; set; } = default!;

		[Column("authors")]
		public List<string> AuthorNames { get; set; } = default!;
	}
}
