#$global:SuiteName = "testwebapp122222"
#$global:WebAppUri = "https://fcfnapp966e.blob.core.windows.net/builds/WebApplication1.zip?sv=2015-04-05&sr=c&sig=nzs9Vh1LtpEzz8BFKyVQUbkaup6JO%2ByThnRm711lLrY%3D&se=2017-11-07T15%3A29%3A49Z&sp=r"
#$global:Location = "WestEurope"

Function DeleteResources()
{
    Param(
        [Parameter(Mandatory=$true, Position=0)] [string] $resourceGroup
    )
    #$rgroups = Get-AzureRmResourceGroup -Name $resourceGroup
    
    Get-AzureRmResourceGroup -Name $resourceGroup -ev notPresent -ea 0

    if ($notPresent)
    {
         Write-Output "Resource group $resourceGroup does not exist"
    }
    else
    {
        Write-Output "Resource group $resourceGroup exists"
       	Remove-AzureRmResourceGroup -Name $resourceGroup -Force -ErrorAction SilentlyContinue | Out-Null
    }
}
$script:ResourceGroupName = "rg-" + $global:SuiteName
$script:DeploymentTemplateFile = $global:rootPath + "WebApplication1\template.json"

## ARM template parameters
$script:ArmParameter = @{ `
    webAppUri = $global:WebAppUri; `
	suiteName = $global:SuiteName;`
}

##Login Azure Account
$accountName ="aditya@manuapratapsinghaccenture.onmicrosoft.com"
$password = ConvertTo-SecureString "man5480U#" -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential($accountName, $password)
Login-AzureRmAccount -Credential $credential

##Remove existing resources
DeleteResources -resourceGroup $script:ResourceGroupName

## Create a resource group.
New-AzureRmResourceGroup -Name $script:ResourceGroupName -Location $global:Location

## Deployment
$global:ArmResult = New-AzureRmResourceGroupDeployment -ResourceGroupName $script:ResourceGroupName -TemplateFile $script:DeploymentTemplateFile -TemplateParameterObject $script:ArmParameter -Verbose

if($global:ArmResult.ProvisioningState -ne "Succeeded"){
    ##Remove existing resources
    DeleteResources -resourceGroup $script:ResourceGroupName
}

Write-Output $global:ArmResult