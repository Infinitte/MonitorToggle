# Monitor Toggle Tray

Una aplicación de bandeja del sistema para Windows 11 que permite alternar el encendido y apagado del monitor 1 usando los scripts existentes.

## Ejecutables Simples (Paso a Paso)

Antes de la aplicación completa, creé ejecutables simples para probar:

- **MonitorOffSimple.exe**: Apaga el monitor 1.
- **MonitorOnSimple.exe**: Enciende el monitor 1.
- **MonitorToggleSimple.exe**: Alterna entre encendido y apagado del monitor 1 (usa un archivo `monitor_state.txt` para recordar el estado).

Estos están en las carpetas respectivas `\bin\Release\net8.0\win-x64\publish\`.

Ejecútalos para verificar que funcionan. El toggle recuerda el estado entre ejecuciones.

## Aplicación Completa

La aplicación de bandeja está en `MonitorToggleTray\bin\Release\net8.0-windows\win-x64\publish\MonitorToggleTray.exe`.

### Características

- Icono en el área de notificaciones (bandeja del sistema) con icono tipo información
- Menú contextual para encender/apagar/toggle monitores 1 y 2
- Atajos de teclado globales: Ctrl+Alt+F1 para toggle monitor 1, Ctrl+Alt+F2 para toggle monitor 2
- Se ejecuta en segundo plano sin ventana visible

### Instalación

1. Asegúrate de que `uvx` esté disponible en el PATH (para monitorcontrol).
2. Ejecuta `MonitorToggleTray.exe`.
3. El icono aparecerá en la bandeja del sistema.

### Uso

- **Desde el icono de la bandeja:** Haz clic derecho en el icono y selecciona las opciones para monitor 1 o 2.
- **Atajos de teclado:** 
  - Ctrl+Alt+F1: Toggle monitor 1
  - Ctrl+Alt+F2: Toggle monitor 2
- **Salir:** Haz clic derecho en el icono y selecciona "Exit".

### Personalización

- Para cambiar los atajos de teclado, modifica las líneas RegisterHotKey en Form1.cs.
- Para cambiar el icono, reemplaza SystemIcons.Information con un icono personalizado (agrega un archivo .ico al proyecto y usa Icon = new Icon("path")).
- Para agregar más monitores, copia los métodos y registra más hotkeys.

### Requisitos

- Windows 11
- .NET 8.0 (incluido en la publicación self-contained)
- uvx y monitorcontrol instalados

### Compilación

Si deseas modificar el código:

1. Instala .NET 8.0 SDK
2. Ejecuta `dotnet build` en la carpeta del proyecto
3. Para publicar: `dotnet publish -c Release -r win-x64 --self-contained`