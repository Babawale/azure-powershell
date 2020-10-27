$loadEnvPath = Join-Path $PSScriptRoot 'loadEnv.ps1'
if (-Not (Test-Path -Path $loadEnvPath)) {
    $loadEnvPath = Join-Path $PSScriptRoot '..\loadEnv.ps1'
}
. ($loadEnvPath)
$TestRecordingFile = Join-Path $PSScriptRoot 'Start-AzCloudService.Recording.json'
$currentPath = $PSScriptRoot
while(-not $mockingPath) {
    $mockingPath = Get-ChildItem -Path $currentPath -Recurse -Include 'HttpPipelineMocking.ps1' -File
    $currentPath = Split-Path -Path $currentPath -Parent
}
. ($mockingPath | Select-Object -First 1).FullName

Describe 'Start-AzCloudService' {
    It 'Start cloud service' {
        Start-AzCloudService -ResourceGroupName $env.ResourceGroupName -CloudServiceName $env.CloudServiceName
    }

    # It 'Start cloud service via identity' {
	#     Get-AzCloudService -ResourceGroupName $env.ResourceGroupName -CloudServiceName $env.CloudServiceName | Start-AzCloudService
    # }
}
