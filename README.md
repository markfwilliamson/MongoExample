# MongoExample
Connect WebApi Container to MongoDB Container

This project was created with Visual Studio and deployed to Kubernetes Cluster in Docker Desktop. 

'=====================================================</br>
'  Run WebApi and MongoDB in Container</br>
'=====================================================</br>
Setup:
1) Open powershell in "Infrastructure" folder path
2) Run ".\DeployWebApi.ps1" to create containers
3) Run "kubectl get all -n webapi -o wide" to confirm everything deployed correctly

Cleanup:
1) Run ".\TeardownWebApi.ps1" to remove existing containers

'=====================================================</br>
'=  Linqpad => Test MongoDB connection</br>
'=====================================================</br>

1) Run Code in Linqpad/Connect_To_MongoDB.linq

Note: This one Succeeds

'=====================================================</br>
'=  Swagger => Test MongoDB connection</br>
'=====================================================</br>

1) http://localhost:4100/swagger/index.html => Execute MongoTest

Note: This one Succeeds now too!
