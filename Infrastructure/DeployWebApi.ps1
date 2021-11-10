function Get-ScriptDirectory {
    $directorypath = if ($PSScriptRoot) { $PSScriptRoot } `
        elseif ($psise) { split-path $psise.CurrentFile.FullPath } `
        elseif ($psEditor) { split-path $psEditor.GetEditorContext().CurrentFile.Path }
    return $directorypath
}

Clear-Host

$nameSpace = "webapi"
$path = Get-ScriptDirectory
kubectl create namespace $nameSpace
Set-Location $path

#===============================================
# Teardown previous containers
#===============================================
kubectl delete -f ..\WebApi\Infrastructure --namespace=$nameSpace
kubectl delete -f mongo --namespace=$nameSpace

#===============================================
# Deploy WebApi
#===============================================
Push-Location ..\WebApi
docker build -t webapi -f Dockerfile . --no-cache
kubectl apply -f Infrastructure --namespace=$nameSpace
Pop-Location

#===============================================
# Deploy Mongo
#===============================================
kubectl apply -f mongo --namespace=$nameSpace

Start-Sleep -s 5 #make sure mongo is ready
mongosh 127.0.0.1/WebApi mongo/mongo.js
mongosh 127.0.0.1/WebApi mongo/mongo-setup-user.js
