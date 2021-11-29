//db = (new Mongo('localhost:27017')).getDB('TestDB')

//config={"_id":"rs0","members":[{"_id":0,"host":"localhost:27017"},{"_id":1,"host":"localhost:27017"},{"_id":2,"host":"localhost:27017"}]}
//config={"_id":"rs0","members":[{"_id":0,"host":"mongoset1:27017"},{"_id":1,"host":"mongoset2:27017"},{"_id":2,"host":"mongoset3:27017"}]}
//config={"_id":"rs0","members":[{"_id":0,"host":"mongoset1:27017"},{"_id":1,"host":"mongoset2:27018"},{"_id":2,"host":"mongoset3:27019"}]}
config={"_id":"rs0","members":[{"_id":0,"host":"localhost:27017"}]}

rs.initiate(config)