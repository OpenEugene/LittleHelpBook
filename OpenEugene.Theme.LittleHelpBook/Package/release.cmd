del "*.nupkg"
REN OpenEugene.Theme.LittleHelpBook.nuspec.REMOVE OpenEugene.Theme.LittleHelpBook.nuspec
"..\..\oqtane.framework\oqtane.package\nuget.exe" pack OpenEugene.Theme.LittleHelpBook.nuspec 
XCOPY "*.nupkg" "..\..\oqtane.framework\Oqtane.Server\wwwroot\Themes\" /Y
