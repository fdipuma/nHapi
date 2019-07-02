properties {
    $projectName = "NHapi20"
    $unitTestAssembly = "NHapi.NUnit.dll"
    $projectConfig = "Release"
    $base_dir = resolve-path .\
    $test_dir = "$base_dir\NHapi.NUnit\bin\$projectConfig\net472"
    $nunitPath = "$base_dir\packages\NUnit.ConsoleRunner.3.10.0\tools"
}

Task Default -depends Build

Task Clean {
	Remove-Item *.nupkg
	Remove-Item ..\NuGet\*.dll
}

Task Build -depends Clean {
	Write-Host "Building Hl7Models.sln" -ForegroundColor Green
    Exec { dotnet msbuild "Hl7Models.sln" /t:Build /p:Configuration=$projectConfig /v:quiet}
}


task Test {
	exec {
		& $nunitPath\nunit3-console.exe $test_dir\$unitTestAssembly --result="$base_dir\TestResult.xml;format=nunit2"
	}
}

Task Package -depends Build {
	Remove-Item ..\NuGet\*.dll
	Copy-Item .\NHapi.NUnit\bin\Release\net472\*.dll ..\NuGet
    Copy-Item .\NHapi.NUnit\bin\Release\net472\*.xml ..\NuGet
	Exec { .nuget\nuget pack ..\NuGet\nHapi.v2.nuspec }
}

Task Deploy -depends Package {
	Exec { .nuget\nuget push *.nupkg -Source https://api.nuget.org/v3/index.json }
}