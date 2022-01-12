using Cassandra.Mapping;
using Cassandra.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksDataLoader.models
{
	[Table("author_by_id")]
	public class Author
	{
		[PartitionKey(0)] [Column("author_id")]
		public string Id { get; set; } = default!;
		[Column("author_name")]
		public string Name { get; set; } = default!;	
	}
}
