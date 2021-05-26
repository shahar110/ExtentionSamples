echo start...

echo ----------------------------------
echo copying to HQ Additions
echo ----------------------------------
xcopy /Y ExtensionSamples.Server.Configuration\bin\Debug\ExtensionSamples.Server.API.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\HQ\Extension\Additions
xcopy /Y ExtensionSamples.Server.Configuration\bin\Debug\ExtensionSamples.Server.Configuration.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\HQ\Extension\Additions
xcopy /Y ExtensionSamples.Server.Configuration\bin\Debug\ExtensionSamples.Server.Contract.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\HQ\Extension\Additions
xcopy /Y ExtensionSamples.Server.Configuration\bin\Debug\ExtensionSamples.Server.DataAccess.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\HQ\Extension\Additions

echo ----------------------------------
echo copying to HQ Overrides
echo ----------------------------------
xcopy /Y ExtensionSamples.Server.Configuration\bin\Debug\ExtensionSamples.Server.API.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\HQ\Extension\Overrides
xcopy /Y ExtensionSamples.Server.Configuration.Override\bin\Debug\ExtensionSamples.Server.Configuration.Override.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\HQ\Extension\Overrides
xcopy /Y ExtensionSamples.Server.Configuration\bin\Debug\ExtensionSamples.Server.Contract.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\HQ\Extension\Overrides
xcopy /Y ExtensionSamples.Server.Configuration\bin\Debug\ExtensionSamples.Server.DataAccess.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\HQ\Extension\Overrides

echo ----------------------------------
echo copying to Store Additions
echo ----------------------------------
xcopy /Y ExtensionSamples.Server.Configuration\bin\Debug\ExtensionSamples.Server.API.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\Store\Extension\Additions
xcopy /Y ExtensionSamples.Server.Configuration\bin\Debug\ExtensionSamples.Server.Configuration.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\Store\Extension\Additions
xcopy /Y ExtensionSamples.Server.Configuration\bin\Debug\ExtensionSamples.Server.Contract.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\Store\Extension\Additions
xcopy /Y ExtensionSamples.Server.Configuration\bin\Debug\ExtensionSamples.Server.DataAccess.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\Store\Extension\Additions

echo ----------------------------------
echo copying to Store Overrides
echo ----------------------------------
xcopy /Y ExtensionSamples.Server.Configuration\bin\Debug\ExtensionSamples.Server.API.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\Store\Extension\Overrides
xcopy /Y ExtensionSamples.Server.Configuration.Override\bin\Debug\ExtensionSamples.Server.Configuration.Override.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\Store\Extension\Overrides
xcopy /Y ExtensionSamples.Server.Configuration\bin\Debug\ExtensionSamples.Server.Contract.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\Store\Extension\Overrides
xcopy /Y ExtensionSamples.Server.Configuration\bin\Debug\ExtensionSamples.Server.DataAccess.dll C:\gitrepo\storix-base\StorixServer\Storix.Host\bin\Store\Extension\Overrides

echo finished...