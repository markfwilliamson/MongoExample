$nameSpace = "webapi"

kubectl delete -f ..\WebApi\Infrastructure --namespace=$nameSpace
kubectl delete -f mongo --namespace=$nameSpace
