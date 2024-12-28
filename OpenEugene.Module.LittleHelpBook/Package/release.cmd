REN  OpenEugene.Module.LittleHelpBook.nuspec.REMOVE OpenEugene.Module.LittleHelpBook.nuspec 
"..\..\oqtane.framework\oqtane.package\nuget.exe" pack OpenEugene.Module.LittleHelpBook.nuspec 
XCOPY "*.nupkg" "..\..\oqtane.framework\Oqtane.Server\Packages\" /Y

