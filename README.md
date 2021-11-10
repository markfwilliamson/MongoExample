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

Note: This one Fails with the following error message

A timeout occurred after 30000ms selecting a server using CompositeServerSelector{ Selectors = MongoDB.Driver.MongoClient+AreSessionsSupportedServerSelector, LatencyLimitingServerSelector{ AllowedLatencyRange = 00:00:00.0150000 }, OperationsCountServerSelector }. Client view of cluster state is { ClusterId : "1", ConnectionMode : "ReplicaSet", Type : "ReplicaSet", State : "Disconnected", Servers : [{ ServerId: "{ ClusterId : 1, EndPoint : "Unspecified/localhost:27017" }", EndPoint: "Unspecified/localhost:27017", ReasonChanged: "Heartbeat", State: "Disconnected", ServerVersion: , TopologyVersion: , Type: "Unknown", HeartbeatException: "MongoDB.Driver.MongoConnectionException: An exception occurred while opening a connection to the server.
 ---> System.Net.Sockets.SocketException (99): Cannot assign requested address
   at System.Net.Sockets.Socket.AwaitableSocketAsyncEventArgs.CreateException(SocketError error, Boolean forAsyncThrow)
   at System.Net.Sockets.Socket.AwaitableSocketAsyncEventArgs.ConnectAsync(Socket socket)
   at System.Net.Sockets.Socket.ConnectAsync(EndPoint remoteEP, CancellationToken cancellationToken)
   at System.Net.Sockets.Socket.ConnectAsync(EndPoint remoteEP)
   at MongoDB.Driver.Core.Connections.TcpStreamFactory.ConnectAsync(Socket socket, EndPoint endPoint, CancellationToken cancellationToken)
   at System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start[TStateMachine](TStateMachine& stateMachine)
   at MongoDB.Driver.Core.Connections.TcpStreamFactory.ConnectAsync(Socket socket, EndPoint endPoint, CancellationToken cancellationToken)
   at MongoDB.Driver.Core.Connections.TcpStreamFactory.CreateStreamAsync(EndPoint endPoint, CancellationToken cancellationToken)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext(Thread threadPoolThread)
   at System.Threading.Tasks.AwaitTaskContinuation.RunOrScheduleAction(IAsyncStateMachineBox box, Boolean allowInlining)
   at System.Threading.Tasks.Task.RunContinuations(Object continuationObject)
   at System.Threading.Tasks.Task.FinishSlow(Boolean userDelegateExecute)
   at System.Threading.Tasks.Task.TrySetException(Object exceptionObject)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.SetException(Exception exception, Task`1& taskField)
   at MongoDB.Driver.Core.Connections.TcpStreamFactory.ConnectAsync(Socket socket, EndPoint endPoint, CancellationToken cancellationToken)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1.AsyncStateMachineBox`1.MoveNext(Thread threadPoolThread)
   at System.Threading.Tasks.AwaitTaskContinuation.RunOrScheduleAction(IAsyncStateMachineBox box, Boolean allowInlining)
   at System.Threading.Tasks.Task.RunContinuations(Object continuationObject)
   at System.Threading.Tasks.Task.FinishSlow(Boolean userDelegateExecute)
   at System.Threading.Tasks.Task.TrySetException(Object exceptionObject)
   at System.Threading.Tasks.ValueTask.ValueTaskSourceAsTask.<>c.<.cctor>b__4_0(Object state)
   at System.Net.Sockets.Socket.AwaitableSocketAsyncEventArgs.InvokeContinuation(Action`1 continuation, Object state, Boolean forceAsync, Boolean requiresExecutionContextFlow)
   at System.Net.Sockets.SocketAsyncEngine.System.Threading.IThreadPoolWorkItem.Execute()
   at System.Threading.ThreadPoolWorkQueue.Dispatch()
--- End of stack trace from previous location ---
   at MongoDB.Driver.Core.Connections.TcpStreamFactory.ConnectAsync(Socket socket, EndPoint endPoint, CancellationToken cancellationToken)
   at MongoDB.Driver.Core.Connections.TcpStreamFactory.CreateStreamAsync(EndPoint endPoint, CancellationToken cancellationToken)
   at MongoDB.Driver.Core.Connections.BinaryConnection.OpenHelperAsync(CancellationToken cancellationToken)
   --- End of inner exception stack trace ---
   at MongoDB.Driver.Core.Connections.BinaryConnection.OpenHelperAsync(CancellationToken cancellationToken)
   at MongoDB.Driver.Core.Servers.ServerMonitor.InitializeConnectionAsync(CancellationToken cancellationToken)
   at MongoDB.Driver.Core.Servers.ServerMonitor.HeartbeatAsync(CancellationToken cancellationToken)", LastHeartbeatTimestamp: "2021-11-09T17:21:59.6525201Z", LastUpdateTimestamp: "2021-11-09T17:21:59.6525205Z" }] }.
