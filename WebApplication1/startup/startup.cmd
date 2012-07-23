@REM Setting up Azure Stroage Credentials
set azurestoragename=planglow
set azurestoragekey=8ColKNpn1RRMDRUwQniowKazfhmoGBJSwVlR6zks0up2Kcg/WkNH4dsKoVC1C6/FEPJcZqTP/+EAlSln2SxKPA==
set storagecontainername=vivcontainer

@REM Download Inkscape Installer
set filename=inkscape.exe
packagedownloader.exe "%azurestoragename%" "%azurestoragekey%" "%storagecontainername%" "%filename%"

@REM Install Inkscape
inkscape.exe /S
exit /b 0

