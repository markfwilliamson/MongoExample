db = (new Mongo('localhost:27017')).getDB('admin')

rs.status()

db.createUser(
   {
     user: "admin",
     pwd: "MyPassword",
     roles: [ "readWrite", "dbAdmin" ]
   }
);

