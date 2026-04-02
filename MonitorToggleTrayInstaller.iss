; MonitorToggleTray Installer (Inno Setup)
; https://jrsoftware.org/ishelp/

[Setup]
AppName=MonitorToggleTray
AppVersion=1.0.0
DefaultDirName={pf}\MonitorToggleTray
DefaultGroupName=MonitorToggleTray
DisableDirPage=no
DisableProgramGroupPage=no
DisableReadyPage=no
DisableStartupPrompt=yes
OutputDir=output
OutputBaseFilename=MonitorToggleTraySetup
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin
Uninstallable=yes
UninstallDisplayIcon={app}\MonitorToggleTray.exe
CompressionThreads=2
AppCopyright=© 2026
SetupIconFile=monitor.ico

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "MonitorToggleTray\bin\Release\net8.0-windows\win-x64\publish\MonitorToggleTray.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "MonitorToggleTray\bin\Release\net8.0-windows\win-x64\publish\MonitorToggleTray.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "MonitorToggleTray\monitor.ico"; DestDir: "{app}"; Flags: ignoreversion
; Si dependes de otros dlls, añade one por linea. El self-contained incluye casi todo.
; Excluir .cmd/.lnk: no los referenciamos.

[Icons]
Name: "{group}\MonitorToggleTray"; Filename: "{app}\MonitorToggleTray.exe"; IconFilename: "{app}\monitor.ico"; WorkingDir: "{app}"
Name: "{userdesktop}\MonitorToggleTray"; Filename: "{app}\MonitorToggleTray.exe"; IconFilename: "{app}\monitor.ico"; Tasks: desktopicon; WorkingDir: "{app}"

[Registry]
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; \
    ValueType: string; ValueName: "MonitorToggleTray"; ValueData: """{app}\MonitorToggleTray.exe"""; \
    Check: IsAutoStartChecked

[Tasks]
Name: "desktopicon"; Description: "Create a Desktop icon"; GroupDescription: "Additional icons:"; Flags: unchecked
Name: "autostart"; Description: "Start MonitorToggleTray on Windows logon"; GroupDescription: "Additional icons:"; Flags: unchecked

[Run]
Filename: "{app}\MonitorToggleTray.exe"; Description: "{cm:LaunchProgram,MonitorToggleTray}"; Flags: nowait postinstall skipifsilent

[Code]
function IsAutoStartChecked: Boolean;
begin
  Result := IsTaskSelected('autostart');
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssDone then
  begin
    if IsTaskSelected('autostart') then
      RegWriteStringValue(HKCU, 'Software\Microsoft\Windows\CurrentVersion\Run', 'MonitorToggleTray', '"' + ExpandConstant('{app}\MonitorToggleTray.exe') + '"')
    else
      RegDeleteKeyValue(HKCU, 'Software\Microsoft\Windows\CurrentVersion\Run', 'MonitorToggleTray');
  end;
end