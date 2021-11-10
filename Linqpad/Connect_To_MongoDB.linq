<Query Kind="Program">
  <Output>DataGrids</Output>
  <NuGetReference>MongoDB.Bson</NuGetReference>
  <NuGetReference>MongoDB.Driver</NuGetReference>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>MongoDB.Bson</Namespace>
</Query>

void Main()
{
	try
	{
		string connectionString = "mongodb://admin:MyPassword@localhost:27017/admin?connect=replicaSet&replicaSet=rs0";
		var client = new MongoClient(connectionString);

		var db = client.GetDatabase("TestDB");
		var collection = "TestCollection1";
		if (db.ListCollections(new ListCollectionsOptions { Filter = new BsonDocument("name", collection) }).Any())
		{
			db.DropCollection(collection);
		}
		db.CreateCollection(collection);
	}
	catch (Exception ex)
	{
		throw new Exception(ex.Message);
	}
}