$modulePath =  $global:rootPath + $global:queueObject.SolutionName + "\deployment.ps1"
Write-Output "Queued item status # " + $global:queueObject.Status 

if($global:queueObject.Status -eq "2"){
	Write-Output "#####################  Deployment started ($global:queueObject) '  ####################" 
	$global:buildId = $global:queueObject.BuildId
	$global:WebAppUri = $global:queueObject.PkgURL
	$global:DeploymentName = $global:queueObject.DeploymentName
	$global:SuiteName = $global:queueObject.DeploymentName
	$global:Location = $global:queueObject.PresetAzureLocationName
	Import-Module $modulePath -ErrorAction Stop
	$global:status = 3
	$global:description = "Deployment queued"
	Write-Output $global:description
}
else{
	Write-Output "#####################  Build started ($global:queueObject) '  ####################" 
	$TFSInstanceURL = "https://manutfs.visualstudio.com"
	$ProjectCollection = "DefaultCollection"
	$TeamProject = "emoapp"
	$Uri = $TFSInstanceURL+"/"+$ProjectCollection+"/"+$TeamProject+"/_apis/build/builds?api-version=2.0"
	$User = ""
	$Password = "hoacsi7qd5hj2oyebf6m4hqwaq6c6fs37ulw7wp7tip3rd7nfoea"
	$base64authinfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $User, $Password)))
	$contentType = "application/json"   
	$headers = @{Authorization=("Basic {0}" -f $base64authinfo)};   
	$build = @{
		"definition"= @{
    		"id" = 4
  		}
	};
	$json = $build | ConvertTo-Json;
	$responseFromGet = Invoke-RestMethod -Method POST -Uri $Uri -ContentType $contentType -Headers $headers -Body $json;
	$global:buildId = $responseFromGet.id
	$global:status = 1
	$global:description = "Buid queued"
	Write-Output $global:description
}
