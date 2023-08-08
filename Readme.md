# Realizador de ping

El realizador de ping es un programa hecho en C# para realizar una secuencia de pings en un intervalo de tiempo de 3hrs.

Para que este funcione primero se debe generar un archivo llamado *secrets.json* este se genera con el comando y debe ser dentro de la carpeta del proyecto:  
```
dotnet user-secrets inti
```

Despues de generar el secrets se debe modificar el archivo *appsetings.json*, siendo exactos el objeto llamado "*ApiSettings*" con valor "*ApiKey*" con el nuevo id generado en la carpeta del json secreto.  

Este se encuentra en la ruta: *%APPDATA%\Microsoft\UserSecrets\\*.


Cosas por actualizar:  
- [ ] Mejorar el *appsettings.json* para permitir el tipo de correo al que se envia
