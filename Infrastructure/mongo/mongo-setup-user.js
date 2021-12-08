db = (new Mongo('localhost:27017')).getDB('admin')

db.createUser(
   {
     user: "admin",
     pwd: "MyPassword",
     roles: [ "readWrite", "dbAdmin" ]
   }
);

