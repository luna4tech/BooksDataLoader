using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using Microsoft.Extensions.Configuration;

namespace BooksDataLoader.repository
{
	public class CassandraRepository<T>
	{
		protected ISession Session { get; }
		protected IMapper Mapper { get; }
		protected Table<T> Table { get; }
		protected string PrimaryKeyName { get; }
		public CassandraRepository(IConfiguration config, string primaryKeyName)
		{

			Session = Cluster.Builder()
						.WithCloudSecureConnectionBundle(@"res\secure-connect-betterreads.zip")
						.WithCredentials(config.GetValue<string>("clientId"), config.GetValue<string>("clientSecret"))
						.Build()
						.Connect("main");

			Mapper = new Mapper(Session);

			Table = new Table<T>(Session);
			Table.CreateIfNotExists();

			PrimaryKeyName = primaryKeyName;
		}

		public async Task InsertAsync<T>(T row)
		{
			await Mapper.InsertAsync<T>(row);
		}
		public T? Get(string key)
		{
			try
			{
				return Mapper.Single<T>($"select * from {Table.Name} where {PrimaryKeyName}='{key}'");
			} 
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return default(T);
			}
		}
		public async Task<T?> GetAsync(string id)
		{
			try
			{
				return await Mapper.SingleAsync<T>($"select * from {Table.Name} where {PrimaryKeyName}='{id}'");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return default(T);
			}
		}
	}
}
