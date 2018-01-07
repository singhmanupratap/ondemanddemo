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
	$TFSInstanceURL = "https://vs-adityasingh.visualstudio.com"
	$ProjectCollection = "DefaultCollection"
	$TeamProject = "ondemanddemo"
	$Uri = $TFSInstanceURL+"/"+$ProjectCollection+"/"+$TeamProject+"/_apis/build/builds?api-version=2.0"
	$User = ""
	$Password = "mjreoriblyzpdiauv4hwct7oijadwqyzq7g5njv4dskz77y5ghza"
	$base64authinfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $User, $Password)))
	$contentType = "application/json"   
	$headers = @{Authorization=("Basic {0}" -f $base64authinfo)};   
	$parameters = 
	$queueItem=$global:queueObject
	$parameters = "'BuildRequestId':'{0}','AzureAccountOwnerName':'{1}','AzureSubscriptionId':'{2}','AzureTenantId':'{3}','DeploymentName':'{4}','PresetAzureDirectoryName':'{5}','PresetAzureLocationName':'{6}','PresetAzureSubscriptionName':'{7}','ServicePrincipalId':'{8}','ServicePrincipalPassword':'{9}','VmAdminPassword':'{10}'" -f $global:queueObject.Id,$global:queueObject.AzureAccountOwnerName,$global:queueObject.AzureSubscriptionId,$global:queueObject.AzureTenantId,$global:queueObject.DeploymentName,$global:queueObject.PresetAzureDirectoryName,$global:queueObject.PresetAzureLocationName,$global:queueObject.PresetAzureSubscriptionName,$global:queueObject.ServicePrincipalId,$global:queueObject.ServicePrincipalPassword,$global:queueObject.VmAdminPassword
	$build = @{
		"definition"= @{
    		"id" = 1
  		}
		"parameters" ="{$parameters}"
	};
	$json = $build | ConvertTo-Json;
	Write-Output $json
	$responseFromGet = Invoke-RestMethod -Method POST -Uri $Uri -ContentType $contentType -Headers $headers -Body $json;
	$global:buildId = $responseFromGet.id
	$global:status = 1
	$global:description = "Buid queued"
}