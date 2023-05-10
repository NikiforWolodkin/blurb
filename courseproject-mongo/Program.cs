using courseproject_mongo;
using MongoDB.Bson;
using MongoDB.Driver;

MongoClient client = new MongoClient("mongodb://localhost:27017");



// создание бд или подключение к существующей бд
IMongoDatabase database = client.GetDatabase("blurbDB");

// получение или создание коллекции
var users = database.GetCollection<User>("users");

// создание пользователя
User user = new User { Email = "mail@mail.com" };

// добавление пользователя в бд
users.InsertOne(user);

// определение фильтра и поиск документа
var filter = new BsonDocument { { "Email", "mail@mail.com" } };
var userFromDB = users.Find(filter).FirstOrDefault();

// изменение объекта и сохранения изменений в бд
userFromDB.Username = "user";
users.ReplaceOne(filter, userFromDB);

// удаление объекта
users.DeleteOne(filter);



client.DropDatabase("BlurbDB");