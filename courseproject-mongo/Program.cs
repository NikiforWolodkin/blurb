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

// удаление документа
users.DeleteOne(filter);



var indexOptions = new CreateIndexOptions();

// создание индекса
var signleFieldIndexKeys = Builders<User>.IndexKeys.Ascending(user => user.RegistrationDate);
var singleFieldIndexModel = new CreateIndexModel<User>(signleFieldIndexKeys, indexOptions);
users.Indexes.CreateOne(singleFieldIndexModel);

// создание составного индекса
var compoundIndexKeys = Builders<User>.IndexKeys
    .Ascending(user => user.Status)
    .Ascending(user => user.Role);
var compoundIndexModel = new CreateIndexModel<User>(signleFieldIndexKeys, indexOptions);
users.Indexes.CreateOne(singleFieldIndexModel);

// создание индекса для нескольких ключей (коллекции)
var multikeyIndexKeys = Builders<User>.IndexKeys.Ascending(user => user.Subscriptions);
var multikeyIndexModel = new CreateIndexModel<User>(multikeyIndexKeys, indexOptions);
users.Indexes.CreateOne(multikeyIndexModel);

// создание текстового индекса
var textIndexKeys = Builders<User>.IndexKeys.Text(user => user.Username);
var textIndexModel = new CreateIndexModel<User>(textIndexKeys, indexOptions);
users.Indexes.CreateOne(textIndexModel);

// создание хэш индекса
var hashIndexKeys = Builders<User>.IndexKeys.Hashed(user => user.Username);
var hashIndexModel = new CreateIndexModel<User>(hashIndexKeys, indexOptions);
users.Indexes.CreateOne(hashIndexModel);



client.DropDatabase("BlurbDB");