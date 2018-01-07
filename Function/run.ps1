#$triggerInput = @{"Id"="d96163db-9031-4d3d-9147-02139cf6ed23";"SolutionName"="WebApplication1";"BuildTemplate"="";"Status"=2;"BuildId"="63";"PkgURL"="https://fcfnapp966e.blob.core.windows.net/builds/WebApplication1.zip?sv=2015-04-05&sr=c&sig=43ceIR%2Bh2pFZiJ2%2FWe5g2i%2Byw7j5EfqmZxENTYlrakA%3D&se=2017-11-08T14%3A48%3A16Z&sp=r";"DeploymentName"="manupsxtttz";"PresetAzureLocationName"="WestEurope";"PresetAzureAccountName"="";"PresetAzureSubscriptionName"="";"PresetAzureDirectoryName"="";"VmAdminPassword"=""};
#$global:queueObject = $triggerInput;
#Write-Output $global:queueObject
#$global:rootPath =  "C:\Users\manu.a.pratap.singh\Source\Repos\FC-OnDemandDemo\fc-fnapp\BuildSolution\Solutions\"

$global:queueObject = Get-Content $triggerInput -Raw | ConvertFrom-Json
$global:rootPath =  "D:\home\site\wwwroot\BuildSolution\Solutions\ConnectedFactory"
$modulePath =  $global:rootPath + $global:queueObject.SolutionName + "\execute.ps1"
$global:status = 1
$global:description = "Project built successfully"
$global:buildId = ""

Try
{
	Import-Module $modulePath -ErrorAction Stop
}
Catch
{
	$errorMessage = $_.Exception.Message
	$global:status = 4
	$global:description = "ERROR - module '$modulePath' failed To load.-$errorMessage"
	Write-Output $global:description
}
Finally
{
	$Uri = "http://api-ondemanddemo.azurewebsites.net/api/builds/" + $global:queueObject.Id;
	$contentType = "application/json"   
	$headers = @{};   
	$build = @{
		"Status"= $global:status;
		"Description"= $global:description;
		"VSTSBuildId" = $global:buildId
	};
	$json = $build | ConvertTo-Json;
	Invoke-RestMethod -Method PUT -Uri $Uri -ContentType $contentType -Headers $headers -Body $json;
}