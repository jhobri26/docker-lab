# Guía de sustentación · Los diez casos en 10 minutos

Alumno: JHONATAN BRICEÑO CUÑACHI CANAQUIRI · CURS-000662.
Instructor: Aristedes Novoa Arbildo · Galaxy Training.

## Cómo usar esta guía

La presentación explica **qué desarrollaste, cómo lo hiciste y con qué resultado lo comprobaste**. Utiliza el guion cronometrado como recorrido principal y el [manual técnico](#manual-técnico-de-ejecución-y-evidencias) para preparar los ejercicios o ampliar una respuesta. Puedes presentar directamente con el portal, el editor y PowerShell; cada bloque corresponde a una pantalla o diapositiva de apoyo.

Los textos entre comillas son un **guion de ensayo**. Exprésalos en pasado únicamente cuando tengas evidencia real de haber realizado esa acción. Sustituye versiones, resultados y enlaces por los de tu entrega. Si algo sigue pendiente, indícalo con precisión. La existencia del código o de esta guía no acredita la ejecución de los diez casos.

## Preparación de la exposición

Realiza estos pasos antes de iniciar el cronómetro:

1. Abre el portal original en `http://localhost:18080`, revisa el perfil, el repositorio y los estados reales. Ordena las evidencias de los casos 01 a 10 y deja visibles las comparaciones de los casos 08 y 09. Ten descargado el informe PDF como respaldo.
2. Deja abiertos `backend/Content/cheatsheet.md`, ambos Dockerfile y `.dockerignore`, `compose.yml`, los dos Compose del caso 06, `compose.caso8.yml`, `backend/Configuration/AppInfo.cs` y `compose.caso9.yml`. Abre también la configuración por IP que utilizaste en el caso 03.
3. Completa previamente los builds, publicaciones, descargas y pruebas de eliminación/recreación. Prepara sus salidas reales para explicarlas: esas operaciones pueden exceder el tiempo disponible. En los diez minutos, combina demostración en vivo del portal con evidencias de las ejecuciones anteriores. Coordina con el docente cualquier repetición completa que solicite la evaluación en vivo.
4. Mantén funcionando el portal original. Arranca previamente solo los laboratorios que mostrarás en vivo, según la memoria disponible; para los demás usa las capturas reales. Identifica cada dirección: caso 03, `18093`; caso 05, `18150`; caso 06 Docker Hub, `18160`, y ACR, `18170`; caso 08, `18180`; caso 09, `18190`.
5. En PowerShell, desde la raíz del proyecto, comprueba el portal con estas dos líneas individuales:

```powershell
docker compose -f compose.yml ps
curl.exe http://localhost:18080/api/health
```

6. Ensaya con cronómetro y letra legible. El tiempo de cada bloque incluye hablar, cambiar de pantalla y mostrar resultados. Si una consulta tarda más de cinco segundos, muestra su evidencia guardada e identifica que corresponde a una ejecución previa. Resuelve el diagnóstico fuera del recorrido cronometrado.

## Agenda de 10 minutos

| Intervalo | Duración | Bloque | Pantalla principal |
| --- | --- | --- | --- |
| 00:00–00:35 | 35 s | Apertura y solución desarrollada | Vista general y arquitectura |
| 00:35–01:00 | 25 s | Caso 01 · Cheat Sheet | Hoja personalizada |
| 01:00–01:40 | 40 s | Caso 02 · Contenedores simples | Evidencias del ciclo de vida |
| 01:40–02:35 | 55 s | Caso 03 · Tres contenedores por IP | IP, configuración e integración |
| 02:35–03:15 | 40 s | Caso 04 · Redes | Conectividad antes/después |
| 03:15–04:10 | 55 s | Caso 05 · Imágenes propias | Dockerfile, exclusiones y build |
| 04:10–05:15 | 65 s | Caso 06 · Registros | Docker Hub y ACR |
| 05:15–06:20 | 65 s | Caso 07 · Compose | Servicios, imágenes y puertos |
| 06:20–07:20 | 60 s | Caso 08 · Cambio de código | Código, imagen y respuesta antes/después |
| 07:20–08:45 | 85 s | Caso 09 · Secrets y persistencia | IDs, volúmenes y datos recuperados |
| 08:45–09:30 | 45 s | Caso 10 · Investigación | Métricas, salud y diagnóstico |
| 09:30–10:00 | 30 s | Cierre y entrega | Repositorio e informe |
| **Total** | **600 s** | **10 casos, apertura y cierre** | |

## Guion para explicar el desarrollo

### 00:00–00:35 · Apertura

**Muestra:** la vista general del portal y el flujo `Navegador → Angular/Nginx → API .NET → SQL Server`, con Redis conectado a la API y un volumen para las imágenes de evidencia.

**Qué decir:** «Soy Jhonatan Briceño Cuñachi Canaquiri. Desarrollé un portal para organizar los diez casos del examen, registrar notas y adjuntar evidencias. Angular presenta la información, Nginx dirige las peticiones a la API .NET, SQL Server guarda notas y metadatos, y Redis mantiene un contador de visitas. Voy a explicar cómo construí, conecté, publiqué y validé esta solución con Docker».

**Enlace:** «Primero organicé los comandos y comprobé el manejo básico de contenedores».

### 00:35–01:00 · Caso 01: organizar los comandos

**Muestra:** tu nombre y las secciones de `backend/Content/cheatsheet.md`, junto con las salidas reales de `docker compose ps` y `docker compose images`.

**Qué decir:** «Preparé una hoja de ayuda adaptada al proyecto y organicé los comandos por propósito. Con `ps` consulto los contenedores y con `images` identifico sus imágenes. La imagen contiene la aplicación y sus dependencias; el contenedor es una instancia creada a partir de ella. Esta hoja me permitió repetir y documentar los ejercicios».

**Resultado que debes señalar:** comandos propios del proyecto con su utilidad, respaldados por sus salidas.

### 01:00–01:40 · Caso 02: controlar el ciclo de vida

**Muestra:** las evidencias de Nginx en `18090` y la secuencia creación → inspección/logs → renombre → parada/arranque/reinicio → eliminación normal y forzada.

**Qué decir:** «Utilicé la imagen pública de Nginx para practicar el ciclo de vida. Creé el contenedor, publiqué un puerto y comprobé la página. Después inspeccioné su configuración y revisé los logs generados por la petición. Lo renombré y probé detenerlo, iniciarlo y reiniciarlo. Finalmente comparé `rm` sobre un contenedor detenido con `rm -f` sobre uno en ejecución».

**Resultado que debes señalar:** página accesible antes de eliminar y listados posteriores que confirman ambas eliminaciones. Detener conserva el contenedor; eliminar lo retira.

### 01:40–02:35 · Caso 03: integrar tres contenedores por IP

**Muestra:** las IP reales obtenidas con `inspect`, `Database__Server` con la IP de SQL y el `proxy_pass` de Nginx con la IP de la API. Termina con la nota recuperada en `18093` y su respuesta de salud.

**Qué decir:** «Separé este laboratorio del portal principal y conecté tres contenedores a una red: frontend, API y SQL Server. Obtuve las IP internas mediante `inspect` y configuré manualmente las conexiones. Nginx envía las peticiones a la IP de la API y esta consulta la IP de SQL. Guardé una nota y la recuperé después de recargar, comprobando el recorrido completo. Al recrear contenedores las IP pueden cambiar; por eso en el stack de Compose utilizo nombres de servicio».

**Resultado que debes señalar:** correspondencia entre IP inspeccionadas y configuración, más datos recuperados. La respuesta de salud comprueba API/SQL; la nota recuperada demuestra además el uso de la aplicación. Explica qué imágenes elegiste y verifica que cumplen la condición de ser distintas de las usadas en clase.

### 02:35–03:15 · Caso 04: administrar redes

**Muestra:** `exam-red-a`, `exam-red-b`, las conexiones antes/después y la respuesta HTML obtenida desde `exam-probe` hacia `exam-network`.

**Qué decir:** «Creé e inspeccioné una red y comprobé la comunicación entre dos contenedores. Para cambiar el nombre de la red, creé otra, conecté los contenedores a ella, desconecté la anterior y la eliminé. Docker no dispone de `network rename`, por lo que documenté esta migración. Por último, probé la limpieza de redes sin uso filtrando por la etiqueta del caso».

**Resultado que debes señalar:** conectividad antes y después de la migración y eliminación efectiva de la red sin uso.

### 03:15–04:10 · Caso 05: construir las imágenes propias

**Muestra:** ambos Dockerfile y `.dockerignore`; después los builds, las etiquetas `local-caso5` y la aplicación del laboratorio en `18150`.

**Qué decir:** «Creé imágenes propias para frontend y API mediante builds en varias etapas. En el frontend, Node compila Angular y Nginx sirve los archivos generados. En el backend, el SDK compila y publica .NET; la imagen final utiliza el runtime y ejecuta la API con el usuario `app`. Los archivos `.dockerignore` excluyen dependencias y artefactos locales del contexto. Construí ambas imágenes y las ejecuté en un laboratorio con red y volúmenes propios».

**Resultado que debes señalar:** builds correctos, referencias de las dos imágenes y portal/SQL funcionando. Relaciona cada etapa con el artefacto que copia a la imagen final.

### 04:10–05:15 · Caso 06: distribuir por dos registros

**Muestra:** dos repositorios de Docker Hub y dos de ACR con sus nombres reales. En cada registro presenta visibilidad/autenticación, `push`, `pull`, imágenes utilizadas y funcionamiento: Docker Hub en `18160`, ACR en `18170`.

**Qué decir:** «Reutilicé las dos imágenes del caso anterior y les asigné nombres de destino con `tag`. Publiqué frontend y API en Docker Hub público y en Azure Container Registry con autenticación: dos registros y cuatro repositorios. Luego descargué las imágenes y las ejecuté con un Compose por registro, usando `--no-build` para utilizar las imágenes publicadas. Comprobé los servicios y la conexión SQL en cada despliegue. Cada laboratorio conserva sus propios datos».

**Resultado que debes señalar:** publicación, descarga y funcionamiento en **ambos** registros. En ACR, explica que privado significa acceso autenticado y muestra la configuración de descarga anónima; no implica haber creado un endpoint privado de red. Los nombres configurados en YAML deben coincidir con tus evidencias.

### 05:15–06:20 · Caso 07: orquestar la solución con Compose

**Muestra:** `compose.yml`, las salidas de servicios/imágenes y las publicaciones de puertos. Ejecuta en vivo, una línea por vez, con el portal original ya iniciado:

```powershell
docker compose -f compose.yml ps
curl.exe http://localhost:18080/api/services
```

**Qué decir:** «Consolidé la solución en cinco servicios: frontend, frontend-demo, API, SQL Server y Redis. Hay cuatro imágenes distintas porque los dos frontend reutilizan la misma imagen. Declaré la red, los volúmenes, los secrets y dependencias condicionadas a la salud de los servicios. El contenedor frontend publica su puerto 80 en los puertos 18080 y 18081 del equipo; frontend-demo es otra instancia en 18082. Las tres entradas comparten API y datos. Aquí relaciono el estado de los contenedores con la respuesta real de los servicios».

**Resultado que debes señalar:** cinco contenedores, cuatro imágenes, dos puertos del mismo contenedor y respuesta real de API/SQL/Redis. Abre las dos direcciones preparadas o muestra su evidencia de nota compartida.

### 06:20–07:20 · Caso 08: incorporar un cambio de código

**Muestra:** `build.context` y `build.dockerfile` en `compose.caso8.yml`; la línea modificada en `AppInfo.cs`; una comparación de versión, ID de imagen e ID de contenedor antes/después; y la captura del build/recreación.

**Qué decir:** «Configuré en Compose el contexto y el Dockerfile de cada aplicación. En la API cambié `AppInfo.Version`, reconstruí la imagen y recreé el contenedor del laboratorio. Comparé la versión HTTP y los IDs de imagen y contenedor antes y después. Guardar el archivo o reiniciar el contenedor anterior no incorpora el código nuevo, porque se compila y copia durante el build. La prueba relaciona el archivo modificado con la nueva respuesta».

**Resultado que debes señalar:** código modificado → build → imagen nueva → contenedor nuevo → versión nueva en `18180`. Lee los valores reales de tus capturas. El archivo fuente actualmente indica `1.0.1`; ese valor por sí solo no demuestra la comparación con la versión anterior.

### 07:20–08:45 · Caso 09: separar configuración y datos persistentes

**Muestra:** declaración del secret en `compose.caso9.yml` y su ruta de montaje; después, la secuencia de datos antes → eliminación de contenedores → nuevos IDs → mismos volúmenes y datos recuperados en `18190`.

**Qué decir:** «Definí un secret desde un archivo local y lo monté en API y SQL. La API lee la contraseña desde `/run/secrets/db_password`; comprobé el acceso al archivo sin revelar su contenido. Separé la persistencia en tres volúmenes: SQL guarda notas y metadatos, el volumen de evidencias guarda las imágenes y Redis conserva su propia información. Guardé una nota, una imagen con título y descripción y una marca Redis. Registré los IDs, eliminé los cinco contenedores y los creé de nuevo conservando los volúmenes y la contraseña. Los IDs cambiaron y recuperé los mismos datos».

**Resultado que debes señalar:** compara lado a lado los IDs anteriores/nuevos y los nombres/fechas de volúmenes conservados. Abre la misma imagen y lee la misma nota, título, descripción y marca Redis. Un título visible sin su imagen no acredita la recuperación completa.

**Precisión técnica:** Compose local monta el secret desde archivo; no equivale al almacén cifrado de Swarm. La prueba mantiene los volúmenes y no utiliza `down -v`. Si omitiste la eliminación real de contenedores, explica que esa parte sigue pendiente.

### 08:45–09:30 · Caso 10: investigar y diagnosticar

**Muestra:** una captura legible de recursos y otra de diagnóstico. Selecciona dos valores reales e interpreta qué significan. Si el tiempo lo permite, ejecuta esta consulta del portal original:

```powershell
docker compose -f compose.yml stats --no-stream
```

**Qué decir:** «Complementé el desarrollo investigando salud, recursos, procesos, usuario de ejecución, red, puertos, logs y almacenamiento. Con `stats` observo CPU y memoria; con los healthchecks compruebo disponibilidad; con los logs y el estado de reinicio investigo fallos. También revisé el usuario de la API y las capas de la imagen. En esta salida, el valor que señalo significa [interpretación del dato real] y me serviría para investigar [problema concreto]».

**Resultado que debes señalar:** utilidad de los comandos aplicada a los resultados obtenidos. Por ejemplo, relaciona memoria con recursos disponibles o un mensaje de logs con una conexión SQL. Evita diagnosticar un fallo que no aparece en tus datos.

### 09:30–10:00 · Cierre

**Muestra:** repositorio real, resumen de los diez casos e informe descargado con el avance guardado.

**Qué decir:** «El desarrollo conecta el ciclo completo: administrar contenedores, integrar servicios, construir imágenes, distribuirlas y ejecutar la solución con Compose. Las comparaciones de código y persistencia permiten comprobar cómo actualicé la aplicación y cómo conservé sus datos al reemplazar contenedores. Entrego el código, las configuraciones, la hoja de comandos y las evidencias organizadas por caso. El portal y el informe reflejan el estado real de mi trabajo».

## Ensayo y preguntas de apoyo

Al llegar a **05:15**, debes haber terminado el caso 06; a **07:20**, empezar el caso 09; a **09:30**, iniciar el cierre. Si te retrasas, reduce la lectura de salidas y las transiciones. Conserva la explicación y la evidencia principal de cada caso. Reserva las preguntas para después de los diez minutos si el docente lo permite.

| Pregunta probable | Respuesta breve que debes poder respaldar |
| --- | --- |
| ¿Por qué cinco contenedores y cuatro imágenes? | frontend y frontend-demo crean instancias distintas de la misma imagen. |
| ¿Por qué IP en el caso 03 y nombres en Compose? | El caso 03 demuestra configuración por IP; Compose permite referirse a servicios por nombre dentro de la red. |
| ¿Qué diferencia hay entre build y restart? | Build genera una imagen desde el código; restart vuelve a iniciar el contenedor que ya existe. |
| ¿Cómo sabes que se publicó y descargó una imagen? | Muestro los repositorios y las salidas de push/pull, y las referencias usadas por los contenedores. |
| ¿Cómo sabes que los datos persistieron? | Muestro eliminación, IDs nuevos, volúmenes conservados y la recuperación de los mismos datos e imagen. |
| ¿Dónde está la contraseña? | En el archivo local montado como secret; explico la ruta y su lectura sin mostrar el valor. |
| ¿Qué demuestra healthy? | Que pasó la comprobación definida; además verifico las operaciones y respuestas que pide cada caso. |

## Manual técnico de ejecución y evidencias

Los pasos siguientes se conservan como referencia para desarrollar, verificar y repetir los ejercicios. No forman parte de la lectura del guion de diez minutos.

Ejecuta cada comando individualmente en PowerShell y revisa el resultado antes de continuar. Los bloques son referencias de comandos; no los guardes ni ejecutes como scripts. Crea y edita las configuraciones a mano. Son diez componentes de 2 puntos y una sustentación en vivo. Las anotaciones de la columna «Cumple» del PDF no son la nota de esta entrega.

## Preparación manual

1. Inicia Docker Desktop con contenedores Linux. Asigna memoria suficiente para SQL Server y los demás servicios (se recomiendan al menos 6 GB para el conjunto).
2. Abre PowerShell en `C:\Users\JCUNACHI\Documents\CURSOS\EXAMEN-DOCKER` y comprueba:

```powershell
docker version
docker info
docker compose version
```

3. En el Explorador abre `secrets`. Si existe `db_password.txt`, consérvalo: la base existente necesita esa contraseña. En una primera instalación, créalo con el editor y escribe únicamente una contraseña propia de al menos 16 caracteres ASCII, con mayúsculas, minúsculas, números y símbolos. Guarda en UTF-8 sin BOM, en una sola línea, sin espacios ni comillas. Comprueba que no tenga doble extensión `.txt`. No captures su contenido.
4. Valida y levanta el portal:

```powershell
docker compose config --quiet
docker compose up -d --build --wait --wait-timeout 300
docker compose ps
```

5. Abre `http://localhost:18080`. Revisa tu nombre y el enlace real del repositorio desde Recursos y entrega. El mismo frontend responde en 18081; la segunda instancia responde en 18082.
6. Crea manualmente subcarpetas `caso-01` a `caso-10` dentro de `EVINDENCIA`. Captura con `Win + Mayús + S`, mostrando comando y resultado. Guarda los PNG y súbelos en el caso correspondiente del portal. Describe **acción realizada, resultado observado y requisito demostrado**. Conserva los originales.

Esta guía no constituye evidencia de ejecución. Marca cada caso como listo después de comprobar sus resultados reales.

## Caso 01 · Docker Cheat Sheet personalizado (2 puntos)

**Requisito:** hoja propia con comandos de Docker y Compose empleados en los casos y otros relevantes.

1. Abre `backend/Content/cheatsheet.md` o descárgalo desde Recursos. Revisa tu nombre, comandos del proyecto y explicaciones; personaliza las notas para tu exposición.
2. Muestra las secciones de contenedores, imágenes, redes, registros, Compose, secrets y volúmenes.
3. Ejecuta dos comandos de la hoja y explica sus salidas:

```powershell
docker compose ps
docker compose images
```

**Capturas:** `01-01-hoja.png` (nombre y comandos personalizados), `01-02-comandos.png` (ambas salidas). Incluye el Markdown en el repositorio y explica la diferencia entre imagen y contenedor.

## Caso 02 · Contenedores simples (2 puntos)

**Requisito:** usar una imagen pública y demostrar creación, inspección, logs, renombre, parada, reinicio y eliminación estándar/forzada.

Ejecuta una línea a la vez:

```powershell
docker pull nginx:1.28-alpine
docker create --name exam-simple -p 127.0.0.1:18090:80 nginx:1.28-alpine
docker ps -a --filter name=exam-simple
docker start exam-simple
curl.exe http://localhost:18090
docker logs exam-simple
docker inspect exam-simple --format '{{.Name}} {{.State.Status}} {{.Config.Image}}'
docker rename exam-simple exam-nginx
docker ps --filter name=exam-nginx
docker stop exam-nginx
docker ps -a --filter name=exam-nginx
docker start exam-nginx
docker restart exam-nginx
docker stop exam-nginx
docker rm exam-nginx
docker ps -a --filter name=exam-nginx
docker run -d --name exam-force nginx:1.28-alpine
docker ps --filter name=exam-force
docker rm -f exam-force
docker ps -a --filter name=exam-force
```

Abre `http://localhost:18090` antes de eliminar el contenedor. La petición genera una entrada en los logs. `rm` elimina el contenedor detenido; `rm -f` permite eliminar el que estaba ejecutándose.

**Capturas:** `02-01-creacion-web.png`, `02-02-inspect-logs-renombre.png`, `02-03-parada-reinicio.png`, `02-04-eliminaciones.png` (ambas modalidades y listados posteriores).

## Caso 03 · Tres contenedores por IP interna (2 puntos)

**Requisito:** integrar tres contenedores con IP internas obtenidas con `inspect`, usando imágenes diferentes de las desarrolladas en clase. Confirma esa selección con el instructor: aquí se propone frontend Angular/Nginx → API .NET → SQL Server. Es un laboratorio separado del portal principal.

### A. Preparar imágenes, red y almacenamiento

```powershell
docker compose build frontend api
docker network create --label dockerexam.case03=true exam-three-net
docker volume create dockerexam-three-sql
docker volume create dockerexam-three-evidence
```

Crea con el editor `secrets/three-sql.txt`, con una única línea: `MSSQL_SA_PASSWORD=` seguida de la misma contraseña de `secrets/db_password.txt`, sin comillas. Guarda en UTF-8 sin BOM. Ambos `.txt` se excluyen de Git. No captures este contenido. Este SQL recibe una variable de entorno; la demostración de Compose secrets corresponde al caso 09.

### B. Crear SQL y obtener su IP

```powershell
docker run -d --name exam-three-sql --network exam-three-net -e ACCEPT_EULA=Y -e MSSQL_PID=Developer --env-file ./secrets/three-sql.txt -v dockerexam-three-sql:/var/opt/mssql mcr.microsoft.com/mssql/server:2022-latest
docker logs --tail 30 exam-three-sql
docker inspect exam-three-sql --format '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}'
```

Espera que los logs indiquen que SQL Server está listo para conexiones; vuelve a consultarlos manualmente si hace falta. Copia la IP y sustituye `IP_SQL_REAL` en el comando siguiente. No uses una IP de ejemplo ni el nombre DNS.

### C. Crear la API apuntando a SQL por IP

`$PWD` representa la carpeta actual de PowerShell. Ejecuta desde la raíz del proyecto.

```powershell
docker run -d --name exam-three-api --network exam-three-net -e Database__Server=IP_SQL_REAL -e Database__Name=DockerExamThree -e Database__PasswordFile=/run/secrets/db_password --mount "type=bind,source=$PWD/secrets/db_password.txt,target=/run/secrets/db_password,readonly" -v dockerexam-three-evidence:/data/evidence dockerexam-api:local
docker logs --tail 30 exam-three-api
docker exec exam-three-api dotnet Exam.Api.dll --healthcheck
$LASTEXITCODE
docker inspect exam-three-api --format '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}'
docker inspect exam-three-api --format '{{json .Config.Env}}'
```

El healthcheck debe terminar sin error y `$LASTEXITCODE` debe ser `0`. Si falla, revisa los logs y repite la comprobación; no continúes con una API que no inicia. La configuración debe mostrar `Database__Server` con la IP SQL y la ruta al secret, sin contraseña en las variables.

### D. Crear la configuración del frontend con la IP de la API

Crea la carpeta `docs/caso03` en el Explorador y el archivo `docs/caso03/nginx.conf` en el editor. Pega esta configuración, sustituye `IP_API_REAL` por la IP inspeccionada y guarda:

```nginx
server {
    listen 80;
    root /usr/share/nginx/html;
    client_max_body_size 9m;
    location /api/ { proxy_pass http://IP_API_REAL:8080; }
    location / { try_files $uri $uri/ /index.html; }
}
```

```powershell
docker run -d --name exam-three-front --network exam-three-net -p 127.0.0.1:18093:80 --mount "type=bind,source=$PWD/docs/caso03/nginx.conf,target=/etc/nginx/conf.d/default.conf,readonly" dockerexam-frontend:local
docker exec exam-three-front nginx -t
docker exec exam-three-front cat /etc/nginx/conf.d/default.conf
docker network inspect exam-three-net
curl.exe http://localhost:18093/api/health
docker ps --filter network=exam-three-net
```

Abre `http://localhost:18093`, guarda una nota identificable y recarga: debe recuperarse desde SQL. La salud debe responder `status: ok`. Los datos de este laboratorio son independientes: sube sus capturas al portal principal en 18080. Redis no participa aquí; su indicador puede aparecer desconectado.

**Capturas:** `03-01-ip-sql-api.png`, `03-02-configuracion.png` (IP SQL en API y proxy_pass con IP API), `03-03-tres-contenedores.png` (red e instancias), `03-04-integracion.png` (portal 18093, salud y nota recuperada).

Después de guardar las capturas puedes retirar únicamente los contenedores y la red del caso:

```powershell
docker rm -f exam-three-front exam-three-api exam-three-sql
docker network rm exam-three-net
```

Los volúmenes se conservan. Si repites y ya existen los contenedores, captura lo necesario y retíralos antes de crearlos de nuevo; vuelve a inspeccionar las IP. Conserva la contraseña del SQL inicializado.

## Caso 04 · Redes y conectividad (2 puntos)

**Requisito:** crear, inspeccionar, renombrar y eliminar redes; conectar/desconectar contenedores existentes, asignar red al crear y limpiar redes sin uso.

```powershell
docker network create --label dockerexam.case04=true exam-red-a
docker run -d --name exam-network --network exam-red-a nginx:1.28-alpine
docker run -d --name exam-probe --network exam-red-a nginx:1.28-alpine
docker network inspect exam-red-a
docker exec exam-probe wget -qO- http://exam-network
docker network create --label dockerexam.case04=true exam-red-b
docker network connect exam-red-b exam-network
docker network connect exam-red-b exam-probe
docker network disconnect exam-red-a exam-network
docker network disconnect exam-red-a exam-probe
docker network inspect exam-red-b
docker exec exam-probe wget -qO- http://exam-network
docker network rm exam-red-a
docker network ls
```

El HTML de Nginx demuestra conectividad antes y después de migrar. Docker no ofrece `docker network rename`: explica la alternativa de crear otra red, migrar conexiones y retirar la antigua. Confirma con el instructor cómo evaluará esa parte.

Después de capturar conectividad:

```powershell
docker rm -f exam-probe exam-network
docker network rm exam-red-b
docker network create --label dockerexam.case04=true exam-red-unused
docker network ls --filter label=dockerexam.case04=true
docker network prune --filter label=dockerexam.case04=true
docker network ls --filter label=dockerexam.case04=true
```

Lee y responde la confirmación de prune manualmente. El filtro solo considera redes sin uso etiquetadas para el caso 04; `exam-red-unused` permite evidenciar una eliminación efectiva.

**Capturas:** `04-01-red-inicial.png`, `04-02-migracion-conectividad.png` (inspect y HTML en ambas redes), `04-03-eliminacion-prune.png` (red antigua eliminada, limpieza y listado final).

## Caso 05 · Creación de imágenes propias (2 puntos)

**Requisito:** Dockerfile y Dockerignore personalizados, imágenes construidas y contenedores funcionando.

1. Abre `frontend/Dockerfile`, `frontend/.dockerignore`, `backend/Dockerfile` y `backend/.dockerignore`. Explica las etapas de compilación y ejecución, los archivos copiados y las exclusiones.
2. Utiliza `compose.caso5.yml`, ya preparado en la raíz. Define el proyecto `dockerexam-caso5`, imágenes propias con etiqueta `local-caso5`, puertos 18150/18151/18152 y red/volúmenes exclusivos. `compose.yml` corresponde al portal original y `compose_a.yml` conserva su respaldo. Las rutas de build y el secret local se mantienen.
3. Ejecuta individualmente desde la raíz del proyecto. El primer comando detiene el intento anterior del caso 05, que podía compartir almacenamiento con el portal original:

```powershell
docker compose -f compose.caso5.yml stop
docker compose -f compose.caso5.yml config --quiet
docker build -t dockerexam-frontend:local-caso5 ./frontend
docker build -t dockerexam-api:local-caso5 ./backend
docker image ls dockerexam-frontend
docker image ls dockerexam-api
docker history dockerexam-api:local-caso5
docker image inspect dockerexam-api:local-caso5 --format '{{.Config.User}}'
docker compose -f compose.caso5.yml up -d --no-build --wait --wait-timeout 300
docker compose -f compose.caso5.yml ps
docker compose -f compose.caso5.yml images
curl.exe http://localhost:18150/api/health
```

4. Abre `http://localhost:18150` y comprueba la salud e integración. Frontend y API son las imágenes propias; SQL y Redis son de terceros. El mismo frontend también responde en 18151 y la segunda instancia en 18152. Este laboratorio empieza con datos propios; guarda las capturas y súbelas al portal original en 18080.

Los volúmenes del caso son `dockerexam-caso5-sql-data`, `dockerexam-caso5-evidence-data` y `dockerexam-caso5-redis-data`. La red es `dockerexam-caso5-net`. Conserva los volúmenes originales: no uses `down -v`. Para detener solo este laboratorio al terminar, ejecuta `docker compose -f compose.caso5.yml stop`.

**Capturas:** `05-01-dockerfiles-ignore.png` (ambas aplicaciones), `05-02-build-imagenes.png` (builds exitosos y tags), `05-03-funcionamiento.png` (contenedores, portal y salud).

## Caso 06 · Publicar y ejecutar imágenes desde dos registros (2 puntos)

**Qué pide la rúbrica:** publicar imágenes propias en repositorios públicos y privados, usando Docker Hub y una nube; descargarlas, crear contenedores y verificar que funcionan.

### Qué vas a entregar

Vas a publicar las **mismas dos imágenes del caso 05** en **cuatro repositorios**, distribuidos en **dos registros**:

| Registro | Repositorio | Visibilidad | Imagen que publicas |
| --- | --- | --- | --- |
| Docker Hub | `tu_usuario/dockerexam-frontend` | Público | Frontend Angular/Nginx |
| Docker Hub | `tu_usuario/dockerexam-api` | Público | API .NET |
| Azure ACR | `dockerexam-frontend`, dentro de tu ACR | Privado, requiere autenticación | La misma imagen frontend |
| Azure ACR | `dockerexam-api`, dentro de tu ACR | Privado, requiere autenticación | La misma imagen API |

Un **registro** es el servicio que almacena imágenes; un **repositorio** agrupa las versiones de una imagen; **1.0.0** es la etiqueta de versión que usarás aquí. Publicar dos imágenes en cada registro permite mostrar repositorios públicos y privados. SQL y Redis se descargan de sus proveedores: no los publiques como imágenes propias.

Para publicar usarás `docker tag` y `docker push`. Compose se utiliza después para levantar frontend, API, SQL y Redis juntos y comprobar que las imágenes publicadas funcionan. Ya están creados **dos archivos completos**, uno por registro. Cada archivo incluye los servicios relacionados; no necesitas un Compose por cada repositorio de imagen.

| Archivo preparado | Proyecto y recursos | Dirección principal | Otros puertos |
| --- | --- | --- | --- |
| `compose.caso6.dockerhub.yml` | `dockerexam-caso6-dockerhub` | http://localhost:18160 | 18161 y 18162 |
| `compose.caso6.acr.yml` | `dockerexam-caso6-acr` | http://localhost:18170 | 18171 y 18172 |

Cada despliegue tiene red y tres volúmenes propios. Las notas del despliegue Docker Hub y las del despliegue ACR son independientes. El portal original para registrar evidencias continúa en **http://localhost:18080**. El archivo único anterior del caso 06 fue sustituido por estos dos.

### Paso 1. Preparar la terminal y comprobar tus imágenes

Abre PowerShell en la raíz del proyecto. Ejecuta una línea a la vez y comprueba su resultado antes de pasar a la siguiente:

```powershell
Set-Location 'C:\Users\JCUNACHI\Documents\CURSOS\EXAMEN-DOCKER'
docker image inspect dockerexam-frontend:local-caso5 --format '{{.Id}}'
docker image inspect dockerexam-api:local-caso5 --format '{{.Id}}'
```

Cada inspect debe devolver un identificador de imagen. Si falta una imagen, vuelve a construirla siguiendo el caso 05. Vas a reutilizar estas imágenes: no hace falta reconstruirlas para publicarlas en otro registro.

Conserva el archivo `secrets/db_password.txt` existente y su contraseña. No lo captures ni lo publiques. Si ya terminaste la evidencia del caso 05, puedes liberar memoria deteniendo su stack:

```powershell
docker compose -f compose.caso5.yml stop
```

### Paso 2. Identificar los tres datos que debes reemplazar

| Texto de ejemplo | Qué debes colocar | Dónde lo obtienes |
| --- | --- | --- |
| `jhobri` | Tu namespace/usuario real de Docker Hub, en minúsculas | Tu cuenta de Docker Hub; no el nombre visible del perfil |
| `nombre_acr` | Nombre del recurso Azure Container Registry | Azure Portal |
| `tu-registro.azurecr.io` | Servidor completo de tu ACR, sin https:// ni barra final | Consulta `loginServer` del paso 10 |

**Sustituye estos textos tanto en los comandos como en el Compose correspondiente antes de utilizarlos.** Son ejemplos y no apuntan a tus cuentas. Mantén los nombres `dockerexam-frontend`, `dockerexam-api` y la etiqueta `1.0.0` para que todo coincida.

### Paso 3. Crear los dos repositorios públicos de Docker Hub

En el navegador entra en Docker Hub con tu cuenta. Abre **Repositories → Create repository**:

1. Selecciona tu namespace.
2. Escribe `docker-exam-frontend-caso-6` y selecciona visibilidad **Public**. Crea el repositorio.
3. Repite el proceso para `docker-exam-api-caso-6`, también **Public**.

Debes ver dos repositorios diferentes. Todavía pueden estar vacíos: la etiqueta aparecerá después de publicar las imágenes. Conserva sus enlaces para la entrega.

### Paso 4. Personalizar el Compose de Docker Hub

Abre `compose.caso6.dockerhub.yml` en el editor. Sustituye `tu_usuario` por tu namespace real en sus **tres líneas image**: frontend, frontend-demo y api.

Las referencias deben quedar con esta estructura, usando tu usuario real:

```yaml
services:
  frontend:
    image: docker.io/jhobri/docker-exam-frontend-caso-6:1.0.0
  frontend-demo:
    image: docker.io/jhobri/docker-exam-frontend-caso-6:1.0.0
  api:
    image: docker.io/jhobri/docker-exam-api-caso-6:1.0.0
```

Estas líneas explican qué reemplazar; los servicios ya están definidos en el archivo. No pegues este bloque al final ni dupliques secciones. Guarda el archivo.

### Paso 5. Iniciar sesión en Docker Hub

```powershell
docker login
```

Completa la autenticación con la misma cuenta propietaria de los repositorios. Espera la confirmación de inicio de sesión. No incluyas códigos, contraseñas ni tokens en capturas.

### Paso 6. Etiquetar y publicar el frontend en Docker Hub

Reemplaza `tu_usuario` y ejecuta:

```powershell
docker tag dockerexam-frontend:local-caso5 docker.io/jhobri/docker-exam-frontend-caso-6:1.0.0
docker push docker.io/jhobri/docker-exam-frontend-caso-6:1.0.0
```

`tag` añade el nombre del destino a la imagen local. `push` la publica. El push debe terminar correctamente y mostrar un digest. Abre el repositorio frontend en Docker Hub y comprueba que aparece la etiqueta **1.0.0**.

### Paso 7. Etiquetar y publicar la API en Docker Hub

```powershell
docker tag dockerexam-api:local-caso5 docker.io/jhobri/docker-exam-api-caso-6:1.0.0
docker push docker.io/jhobri/docker-exam-api-caso-6:1.0.0
```

Comprueba también la etiqueta **1.0.0** en el repositorio API. Captura ambos repositorios públicos y los dos resultados de push. Si un push falla, corrige usuario, nombre o permisos antes de continuar.

### Paso 8. Descargar las imágenes de Docker Hub

Usa el archivo Docker Hub que personalizaste en el paso 4:

```powershell
docker compose -f compose.caso6.dockerhub.yml config --quiet
docker compose -f compose.caso6.dockerhub.yml config --images
docker compose -f compose.caso6.dockerhub.yml pull
```

`config --quiet` valida el archivo y normalmente no muestra nada si es correcto. `config --images` debe mostrar tu usuario en las referencias de frontend y API. `pull` debe finalizar sin errores. Si una capa indica `Already exists`, conserva la confirmación de descarga/digest: esa capa ya estaba en caché.

**Captura la descarga correcta.** No continúes con `denied`, `not found` u otro error pendiente.

### Paso 9. Ejecutar y capturar el despliegue de Docker Hub

```powershell
docker compose -f compose.caso6.dockerhub.yml up -d --no-build --pull never --wait --wait-timeout 300
docker compose -f compose.caso6.dockerhub.yml images
docker compose -f compose.caso6.dockerhub.yml ps
curl.exe http://localhost:18160/api/health
```

El arranque utiliza las imágenes descargadas en el paso anterior. Deben aparecer cinco servicios saludables y la API debe responder **status: ok**.

Abre **http://localhost:18160**. Guarda una nota identificable, recarga y comprueba que sigue ahí. Captura el navegador con su dirección, la salud y las referencias de imágenes Docker Hub. Esa es tu demostración de funcionamiento del primer registro.

Guarda las capturas antes de seguir. Detén este laboratorio para liberar memoria antes de levantar ACR:

```powershell
docker compose -f compose.caso6.dockerhub.yml stop
```

### Paso 10. Preparar el registro privado Azure ACR

Necesitas una suscripción Azure, Azure CLI y permisos de publicación/descarga. En Azure Portal busca **Container registries** y abre tu registro. Si todavía no tienes uno, créalo con tu suscripción, grupo de recursos, nombre único, región y plan; revisa el costo antes de confirmar. Mantén deshabilitada la descarga anónima.

Sustituye `nombre_acr` por el nombre real del recurso y ejecuta:

```powershell
az login
az acr login --name acrjhobri2026
az acr show --name acrjhobri2026 --query loginServer --output tsv
```

Copia el servidor completo devuelto por la última línea. Puede contener componentes adicionales al nombre del recurso: usa la salida exacta, no construyas la dirección por tu cuenta. A partir de aquí, ese valor reemplaza **todo el texto `tu-registro.azurecr.io`**.

### Paso 11. Personalizar el Compose de ACR

Abre `compose.caso6.acr.yml`. Sustituye `tu-registro.azurecr.io` por el servidor que copiaste, en sus **tres referencias image**: frontend, frontend-demo y api. Guarda.

```yaml
services:
  frontend:
    image: tu-registro.azurecr.io/dockerexam-frontend:1.0.0
  frontend-demo:
    image: tu-registro.azurecr.io/dockerexam-frontend:1.0.0
  api:
    image: tu-registro.azurecr.io/dockerexam-api:1.0.0
```

Los servicios ya existen: edita sus líneas, sin añadir otro bloque. Este archivo utilizará 18170/18171/18172 y almacenamiento propio, separado de Docker Hub.

### Paso 12. Etiquetar y publicar el frontend en ACR

Sustituye el servidor de ejemplo por el `loginServer` real antes de ejecutar:

```powershell
docker tag dockerexam-frontend:local-caso5 acrjhobri2026.azurecr.io/dockerexam-frontend-caso-6:1.0.0
docker push acrjhobri2026.azurecr.io/dockerexam-frontend-caso-6:1.0.0
```

Es la misma imagen local del caso 05, ahora publicada en el registro privado. Comprueba que el push termina correctamente y conserva su salida con digest.

### Paso 13. Etiquetar y publicar la API en ACR

```powershell
docker tag dockerexam-api:local-caso5  acrjhobri2026.azurecr.io/dockerexam-api-caso-6:1.0.0
docker push  acrjhobri2026.azurecr.io/dockerexam-api-caso-6:1.0.0
az acr repository list --name acrjhobri2026.azurecr.io --output table
az acr show --name acrjhobri2026.azurecr.io --query anonymousPullEnabled --output tsv
```

Tras los push deben aparecer `dockerexam-frontend` y `dockerexam-api` en Repositories de tu ACR, con etiqueta **1.0.0**. Se crean al publicar las imágenes. Captura los dos repositorios, las etiquetas y los resultados de push.

La consulta `anonymousPullEnabled` debe mostrar **false**. Si muestra true, deshabilita la descarga anónima en el registro y vuelve a comprobar. Aquí «privado» significa que requiere autenticación; no implica que hayas configurado un endpoint de red privado. Captura la configuración sin mostrar credenciales.

### Paso 14. Descargar desde ACR

Usa ahora el archivo ACR, con los nombres corregidos y la sesión autenticada:

```powershell
docker compose -f compose.caso6.acr.yml config --quiet
docker compose -f compose.caso6.acr.yml config --images
docker compose -f compose.caso6.acr.yml pull
```

Comprueba que frontend y API apuntan al **servidor ACR real**, no a Docker Hub. La descarga debe terminar correctamente. Guarda la captura de este pull por separado del pull de Docker Hub.

### Paso 15. Ejecutar y capturar el despliegue de ACR

```powershell
docker compose -f compose.caso6.acr.yml up -d --no-build --pull never --wait --wait-timeout 300
docker compose -f compose.caso6.acr.yml images
docker compose -f compose.caso6.acr.yml ps
curl.exe http://localhost:18170/api/health
```

Deben aparecer cinco servicios saludables y una respuesta **status: ok**. Abre **http://localhost:18170**, guarda una nota y recarga para comprobar integración. Este despliegue tiene datos propios: crea su nota de prueba, en lugar de esperar la del despliegue Docker Hub.

Captura referencias de imágenes ACR, servicios, salud y navegador funcionando. Al terminar puedes detenerlo:

```powershell
docker compose -f compose.caso6.acr.yml stop
```

### Paso 16. Revisar y cargar todas las evidencias

| Archivo sugerido | Debe demostrar |
| --- | --- |
| `06-01-hub-publicos.png` | Dos repositorios Docker Hub públicos y sus etiquetas 1.0.0 |
| `06-02-hub-push.png` | Push correcto del frontend y la API a Docker Hub |
| `06-03-hub-pull.png` | Referencias Docker Hub y descarga correcta |
| `06-04-hub-ejecucion.png` | Imágenes usadas, servicios saludables y portal/health en 18160 |
| `06-05-acr-privados.png` | Dos repositorios ACR, etiquetas y descarga anónima deshabilitada |
| `06-06-acr-push.png` | Push correcto del frontend y la API a ACR |
| `06-07-acr-pull.png` | Referencias ACR y descarga autenticada correcta |
| `06-08-acr-ejecucion.png` | Imágenes usadas, servicios saludables y portal/health en 18170 |

Puedes usar más de una captura por fila si no cabe toda la información. Guarda los PNG en `EVINDENCIA/caso-06` y súbelos al caso 06 del **portal original, http://localhost:18080**. Añade los enlaces reales y una explicación de qué publicaste, dónde lo descargaste y qué comprobaste al ejecutarlo.

Mostrar únicamente etiquetas locales o contenedores Healthy no prueba el caso completo. Deben verse **publicación + descarga + funcionamiento en cada uno de los dos registros**, además de la visibilidad pública/privada. Conserva los volúmenes; no utilices `down -v` para esta demostración.

### Si aparece un error

| Error o duda | Qué revisar |
| --- | --- |
| `pull access denied` / `denied` en Docker Hub | Cuenta del login, namespace, repositorio y permiso de publicación; ambos deben existir con esos nombres |
| `manifest unknown` / `not found` | Que el push de esa imagen y la etiqueta 1.0.0 haya terminado correctamente |
| `unauthorized` en ACR | Repite `az acr login --name nombre_acr` con el nombre real y verifica permisos de la cuenta |
| Puerto ocupado | Utiliza el archivo correcto: Docker Hub 18160-18162; ACR 18170-18172; revisa `docker ps` |
| SQL no inicia o el stack queda unhealthy | Consulta `docker compose -f ARCHIVO_DEL_REGISTRO logs --tail 50 sqlserver`; conserva sus volúmenes y contraseña |
| La guía del portal todavía muestra el archivo único anterior | La fuente de esta guía y el catálogo ya están actualizados; las imágenes existentes incorporan la versión que tenían al construirse. Consulta este Markdown en el editor para seguir la versión nueva |

Fuentes: [repositorios y visibilidad Docker Hub](https://docs.docker.com/docker-hub/repos/create/), [publicar y descargar en ACR](https://learn.microsoft.com/azure/container-registry/container-registry-get-started-docker-cli), [conceptos de registro y repositorio ACR](https://learn.microsoft.com/azure/container-registry/container-registry-concepts), [pull con Compose](https://docs.docker.com/reference/cli/docker/compose/pull/) y [up con Compose](https://docs.docker.com/reference/cli/docker/compose/up/).

## Caso 07 · Docker Compose (2 puntos)

**Requisito:** stack relacionado con al menos cuatro imágenes distintas, propias y de terceros, cinco contenedores y uno expuesto en diferentes puertos.

1. Abre `compose.yml`: señala los servicios, red, dependencias y dos puertos de `frontend`.
2. Ejecuta:

```powershell
docker compose up -d --build --wait --wait-timeout 300
docker compose config --services
docker compose images
docker compose ps
curl.exe http://localhost:18080/api/health
curl.exe http://localhost:18081/api/health
curl.exe http://localhost:18082/api/services
docker compose exec redis redis-cli GET dockerexam:visits
```

3. Identifica los cinco contenedores: frontend, frontend-demo, API, SQL Server y Redis. Las cuatro imágenes son frontend propia, API propia, SQL Server y Redis; frontend-demo reutiliza la primera.
4. Abre 18080 y 18081: pertenecen al mismo contenedor frontend. Abre 18082, que pertenece a frontend-demo. Guarda una nota desde 18080 y recarga las otras dos vistas: deben mostrarla porque comparten API y SQL. Consulta otra vez `/api/services` y Redis para ver el contador incrementado.

**Capturas:** `07-01-compose.png`, `07-02-contenedores-imagenes.png`, `07-03-puertos-datos.png` (barras de dirección y nota compartida), `07-04-redis.png`.

## Caso 08 · Compose con Build y cambio de código (2 puntos)

**Qué pide la rúbrica:** utilizar Dockerfile y context desde Compose, modificar el código fuente de una aplicación/API y generar los contenedores para comprobar que incorporan el cambio.

### Qué vas a demostrar

Cambiarás la versión que devuelve la API, desde `1.0.0` a `1.0.1`, editando **una línea de código C#**. Después ejecutarás el build con Compose y comprobarás la nueva respuesta. Si tu versión inicial ya es otra, anota ese valor y elige una versión diferente: no presentes como cambio dos respuestas iguales.

La evidencia debe conectar estos hechos: **código modificado → imagen reconstruida → contenedor recreado → respuesta nueva**. Mostrar solo `Healthy`, reiniciar un contenedor o cambiar una etiqueta de imagen no demuestra esa cadena completa.

Ya está creado `compose.caso8.yml` en la raíz. Incluye cinco servicios relacionados, imágenes propias `dockerexam-frontend:local-caso8` y `dockerexam-api:local-caso8`, red `dockerexam-caso8-net` y volúmenes exclusivos del caso 08. El portal del laboratorio responde en **http://localhost:18180**; el mismo frontend también en 18181 y la segunda instancia en 18182. El portal original donde registrarás las capturas sigue en **http://localhost:18080**.

La preparación de esta guía conserva `AppInfo.Version` sin modificar. El cambio de código lo harás tú en el paso 5, después de capturar el estado inicial.

### Paso 1. Preparar la carpeta y los recursos

Abre PowerShell y ejecuta una línea a la vez:

```powershell
Set-Location 'C:\Users\JCUNACHI\Documents\CURSOS\EXAMEN-DOCKER'
docker version
docker compose -f compose.caso8.yml config --quiet
```

Docker Desktop debe estar iniciado con contenedores Linux. Conserva `secrets/db_password.txt` y su contraseña; el caso 08 usa ese archivo para inicializar su propio SQL. No muestres su contenido.

Si otros laboratorios consumen memoria, detén los que ya terminaste desde Docker Desktop o con su comando `docker compose -f ARCHIVO_DEL_CASO stop`, utilizando el archivo real. Mantén disponible el portal original para registrar la evidencia.

### Paso 2. Mostrar cómo Compose construye las imágenes

Abre `compose.caso8.yml` en VS Code. Señala estas configuraciones, que ya están incluidas en el archivo:

```yaml
services:
  frontend:
    image: dockerexam-frontend:local-caso8
    build:
      context: ./frontend
      dockerfile: Dockerfile
  api:
    image: dockerexam-api:local-caso8
    build:
      context: ./backend
      dockerfile: Dockerfile
```

Este bloque muestra los campos relevantes; conserva los demás servicios, puertos y propiedades del archivo completo.

- `context` identifica la carpeta cuyos archivos recibe el build.
- `dockerfile` identifica el Dockerfile dentro de ese contexto.
- `image` asigna un nombre y etiqueta a la imagen construida.

Abre también `backend/Dockerfile`: `COPY . .` incorpora el código del contexto y `dotnet publish` compila la API. `backend/Configuration/AppInfo.cs` está dentro de ese contexto. El frontend se construye con su propio Dockerfile.

**Guarda `08-01-compose-build.png`:** Compose con ambos context/Dockerfile y el Dockerfile del backend. Explica cómo llega el archivo C# a la imagen.

### Paso 3. Construir y levantar la versión inicial

Primero construye las dos imágenes y luego levanta el stack:

```powershell
docker compose -f compose.caso8.yml build frontend api
docker compose -f compose.caso8.yml up -d --no-build --wait --wait-timeout 300
docker compose -f compose.caso8.yml ps
```

Espera que los cinco servicios aparezcan saludables. Este primer build establece la versión inicial para la comparación; todavía no demuestra el cambio pedido por la rúbrica.

Abre **http://localhost:18180** y confirma que el portal carga. Si el arranque falla, resuelve el error antes de modificar el código.

### Paso 4. Registrar el estado ANTES del cambio

Ejecuta y guarda las salidas:

```powershell
Get-Content .\backend\Configuration\AppInfo.cs
curl.exe http://localhost:18180/api/health
docker image inspect dockerexam-api:local-caso8 --format '{{.Id}}'
docker inspect dockerexam-caso8-api-1 --format '{{.Id}} {{.Image}}'
```

En una primera ejecución, el archivo y la respuesta deben mostrar `1.0.0`. La respuesta esperada tiene esta estructura:

```json
{"status":"ok","api":".NET 10","database":"SQL Server 2022","version":"1.0.0"}
```

La salida de `docker image inspect` es el ID de la imagen con etiqueta `local-caso8`. El último comando muestra, en este orden, **ID completo del contenedor** y **ID de la imagen que utiliza**. Estos dos IDs no son el mismo tipo de dato: compara cada uno con su equivalente después.

**Guarda `08-02-antes.png`** mostrando versión, ID de imagen y del contenedor. Puedes usar más capturas si las salidas no caben. Mantén esta evidencia para la comparación final.

### Paso 5. Cambiar manualmente el código fuente

En VS Code abre **`backend/Configuration/AppInfo.cs`**. Cambia únicamente esta línea:

```csharp
public const string Version = "1.0.0";
```

Por esta otra:

```csharp
public const string Version = "1.0.1";
```

Guarda con **Ctrl + S**. Si la versión inicial era otra, utiliza un valor nuevo y anota ambos. No cambies los Dockerfile, la configuración de SQL ni los nombres de imágenes para esta demostración.

**Guarda `08-03-codigo-cambiado.png`** con la ruta del archivo y la línea nueva. Si tienes Git configurado, también puedes mostrar `git diff -- backend/Configuration/AppInfo.cs`; no necesitas Git para realizar el caso.

### Paso 6. Comprobar que editar el archivo todavía no actualiza la API

```powershell
Get-Content .\backend\Configuration\AppInfo.cs
curl.exe http://localhost:18180/api/health
```

El archivo local debe mostrar `1.0.1`, pero la API que sigue ejecutándose debe devolver la versión anterior. En este laboratorio el código se copia a la imagen al compilar: guardar el archivo o usar `docker restart` no incorpora el nuevo código.

Esta comparación ayuda a explicar por qué necesitas el siguiente build. Puedes guardarla como captura adicional `08-03b-pendiente-build.png`.

### Paso 7. Reconstruir y recrear la API con Compose

Con el código nuevo guardado y SQL/Redis ya saludables desde el paso 3, ejecuta:

```powershell
docker compose -f compose.caso8.yml up -d --build --no-deps --wait --wait-timeout 300 api
```

Este comando utiliza el `build` de la API para compilar otra imagen y recrea su contenedor cuando cambia la imagen. `--no-deps` limita la operación a la API; sus dependencias ya deben estar en funcionamiento. `--wait` espera que termine saludable.

En la salida identifica la compilación/publicación .NET, la exportación de la imagen y la recreación/arranque saludable de la API. Algunos pasos del Dockerfile pueden aparecer como `CACHED`: es normal reutilizar capas que no cambiaron. El cambio de código debe incorporarse en la imagen final.

**Guarda `08-04-build-recreacion.png`** con el comando y su resultado exitoso. Si la salida es extensa, captura la compilación y el resultado final por separado. No avances si el build o el arranque terminó con error.

### Paso 8. Registrar el estado DESPUÉS y comparar

Ejecuta los mismos comandos de comprobación:

```powershell
curl.exe http://localhost:18180/api/health
docker image inspect dockerexam-api:local-caso8 --format '{{.Id}}'
docker inspect dockerexam-caso8-api-1 --format '{{.Id}} {{.Image}}'
docker compose -f compose.caso8.yml ps api
```

La respuesta ahora debe tener `version: 1.0.1` y `status: ok`. Si el proxy responde temporalmente 502 al cambiar la IP de la API, espera unos segundos y repite la consulta manualmente.

Compara con `08-02-antes.png`:

| Dato | Antes | Después |
| --- | --- | --- |
| Versión de la API | 1.0.0, o tu valor inicial | 1.0.1, o el valor nuevo que elegiste |
| ID de imagen | sha256 inicial | sha256 distinto, correspondiente al código nuevo |
| ID de contenedor | ID inicial | ID distinto, por la recreación |
| Imagen usada por el contenedor | ID de la imagen inicial | Debe coincidir con el ID nuevo de `docker image inspect` |
| Salud | ok / healthy | ok / healthy |

La etiqueta `dockerexam-api:local-caso8` puede mantenerse igual: lo que cambia es el ID de su contenido. Un ID de contenedor nuevo por sí solo no acredita que se haya compilado código nuevo; comprueba también imagen y respuesta.

**Guarda `08-05-despues.png`** con la nueva respuesta y los IDs. Si alguno de esos cambios no se observa, revisa la tabla de errores antes de marcar el caso como listo.

### Paso 9. Mostrar funcionamiento y registrar las evidencias

Abre **http://localhost:18180** y comprueba el portal. En otra pestaña abre **http://localhost:18180/api/health** para mostrar la nueva versión con la dirección visible. Si guardaste una nota antes del build, recarga y confirma que sigue disponible: la recreación de la API utiliza el mismo almacenamiento del caso.

**Guarda `08-06-portal-funcionando.png`.** Conserva los PNG en `EVINDENCIA/caso-08` y súbelos al **caso 08 del portal original en http://localhost:18080**.

| Archivo | Requisito que respalda |
| --- | --- |
| `08-01-compose-build.png` | Uso de Dockerfile y context desde Compose |
| `08-02-antes.png` | Respuesta y recursos antes de modificar código |
| `08-03-codigo-cambiado.png` | Modificación real de una línea C# |
| `08-04-build-recreacion.png` | Build con Compose y regeneración de la API |
| `08-05-despues.png` | Imagen/contenedor nuevos y versión nueva funcionando |
| `08-06-portal-funcionando.png` | Aplicación accesible después del cambio |

En la descripción escribe tus valores reales, por ejemplo: «La API devolvía [versión inicial]. Modifiqué AppInfo.cs a [versión nueva], ejecuté el build con Compose y comprobé [versión nueva], una imagen distinta y un contenedor nuevo». Sustituye los corchetes por lo que realmente observaste. La comparación entre antes y después es parte esencial de la sustentación en vivo.

### Paso 10. Cerrar o repetir la demostración

Después de guardar las capturas, puedes detener solo el laboratorio:

```powershell
docker compose -f compose.caso8.yml stop
```

Conserva los volúmenes y la contraseña; no uses `down -v`. El caso 08 tiene almacenamiento propio y sus capturas se registran en el portal original.

El archivo `AppInfo.cs` pertenece al código fuente compartido por los builds. Las imágenes que ya estaban ejecutándose conservan su código, pero un build posterior de otro caso incorporará la versión que dejes en el archivo. Si necesitas repetir la prueba desde la versión original, restaura esa línea a mano, construye/levanta de nuevo el caso 08 y vuelve a registrar la línea base antes de hacer otro cambio.

### Si el resultado no coincide

| Problema | Comprobación o corrección manual |
| --- | --- |
| Estás consultando la versión de otro laboratorio | Usa `compose.caso8.yml` y puerto 18180; 18080 es el portal original |
| La API conserva la versión anterior | Confirma que guardaste AppInfo.cs, que modificaste el archivo dentro de backend y que el build terminó correctamente |
| Solo ejecutaste restart o up sin build | Repite el comando del paso 7 después de guardar el código |
| La imagen cambió pero el contenedor usa la anterior | Comprueba el resultado de up y la imagen mostrada por inspect; termina la recreación de la API |
| La respuesta ya tenía tu versión nueva antes de comenzar | Elige una versión diferente y documenta el valor inicial real |
| La API queda unhealthy | `docker compose -f compose.caso8.yml logs --tail 50 api`; comprueba también SQL y Redis con ps |
| El arranque inicial falla por SQL | `docker compose -f compose.caso8.yml logs --tail 50 sqlserver`; conserva los volúmenes y la contraseña |
| El puerto está ocupado | Revisa `docker ps` y confirma que usas los puertos del caso 08, 18180/18181/18182 |

Si comprobaste ruta y guardado y aún necesitas descartar un problema de caché, reconstruye únicamente la API sin caché y luego aplícala; vuelve a hacer las comprobaciones del paso 8:

```powershell
docker compose -f compose.caso8.yml build --no-cache api
docker compose -f compose.caso8.yml up -d --no-deps --no-build --wait --wait-timeout 300 api
```

Fuentes: [context y Dockerfile en Compose](https://docs.docker.com/reference/compose-file/build/), [build con Compose](https://docs.docker.com/reference/cli/docker/compose/build/) y [up: build, recreación y conservación de volúmenes](https://docs.docker.com/reference/cli/docker/compose/up/).

## Caso 09 · Secrets y volúmenes: eliminar y recrear conservando datos (2 puntos)

**Qué pide la rúbrica:** crear y utilizar secrets, crear y utilizar volúmenes, eliminar los contenedores y crear otros que reutilicen esos volúmenes, demostrando que los datos persisten.

### Qué vas a demostrar

La prueba tiene dos partes que debes evidenciar:

1. **Secret utilizado:** Compose monta un archivo de contraseña y tanto API como SQL pueden leerlo; la API logra conectarse a SQL con esa configuración.
2. **Persistencia real:** guardas una nota, una imagen y una marca Redis, eliminas los cinco contenedores del laboratorio y creas otros. Los IDs de contenedor cambian, pero los volúmenes y los datos siguen siendo los mismos.

Ya está preparado **`compose.caso9.yml`**, con proyecto `dockerexam-caso9`, imágenes `local-caso9` y recursos exclusivos. El portal del laboratorio está en **http://localhost:18190**, el mismo frontend también en 18191 y la segunda instancia en 18192. El portal original de entrega permanece en **http://localhost:18080**.

| Recurso | Nombre o ruta del caso 09 | Qué conserva o entrega |
| --- | --- | --- |
| Archivo local del secret | `secrets/caso9_db_password.txt` | Contraseña privada del SQL de este laboratorio |
| Secret dentro de API y SQL | `/run/secrets/db_password` | Archivo que leen ambos servicios |
| Volumen SQL | `dockerexam-caso9-sql-data` → `/var/opt/mssql` | Notas, estados y metadatos de evidencias en SQL |
| Volumen de imágenes | `dockerexam-caso9-evidence-data` → `/data/evidence` | Archivos PNG/JPG/WebP cargados al portal |
| Volumen Redis | `dockerexam-caso9-redis-data` → `/data` | La marca de prueba y los datos AOF de Redis |
| Red | `dockerexam-caso9-net` | Comunicación de los cinco servicios del caso |

La tabla describe los recursos declarados. Las capturas del caso deben mostrar su creación/uso y los resultados reales de la prueba. La preparación de los archivos no elimina contenedores ni genera esas evidencias.

### Paso 1. Crear a mano el archivo de contraseña del caso 09

Abre la carpeta del proyecto en VS Code. Dentro de `secrets`, crea **un archivo de texto**, llamado exactamente **`caso9_db_password.txt`**. Escribe únicamente una contraseña propia de al menos 16 caracteres ASCII, con mayúsculas, minúsculas, números y símbolos, en una sola línea, sin espacios ni comillas. Guarda en UTF-8 sin BOM.

No escribas `MSSQL_SA_PASSWORD=` dentro: este archivo contiene solo el valor. Es independiente de los secretos de los otros casos. Si ya existe y el SQL del caso 09 fue inicializado, conserva ese mismo archivo y contraseña.

En PowerShell, desde la raíz:

```powershell
Set-Location 'C:\Users\JCUNACHI\Documents\CURSOS\EXAMEN-DOCKER'
Test-Path .\secrets\caso9_db_password.txt -PathType Leaf
Get-Item .\secrets\caso9_db_password.txt | Select-Object Name,Length,PSIsContainer
```

Debes obtener **True**, una longitud mayor que cero y **PSIsContainer: False**. Si aparece una carpeta, corrige el tipo antes de seguir. Revisa que Windows no haya añadido otra extensión `.txt`.

**La longitud del archivo no valida la complejidad.** Revisa tú mismo que la contraseña tenga al menos una mayúscula, una minúscula, un número y un símbolo, además de los 16 caracteres indicados. Mantén una sola línea, sin BOM ni espacios al principio/final. No muestres la contraseña para comprobarlo. Después del primer arranque correcto de SQL, conserva exactamente ese valor: editar el archivo no cambia la contraseña `sa` almacenada en el volumen.

Puedes mostrar nombre y tipo del archivo, pero no su contenido. `.gitignore` excluye `secrets/*.txt`; no agregues la contraseña al repositorio.

Si ya terminaste las capturas del caso 08, puedes liberar memoria deteniéndolo:

```powershell
docker compose -f compose.caso8.yml stop
```

### Paso 2. Mostrar la declaración y el uso de secrets/volumes

Abre `compose.caso9.yml` y señala:

- `secrets.db_password.file`, que apunta al archivo local propio del caso 09.
- `secrets: [db_password]` en los servicios API y SQL: son los que reciben el archivo.
- `Database__PasswordFile: /run/secrets/db_password` en la API.
- El `entrypoint` de SQL, que lee ese archivo y proporciona la contraseña al proceso SQL Server.
- Los montajes `sql-data`, `evidence-data` y `redis-data`, con sus rutas y nombres físicos exclusivos.
- Redis con `--appendonly yes`, que permite conservar sus datos en el volumen.

Para explicar que la API consume el archivo, abre `backend/Configuration/ServiceRegistration.cs`: `BuildConnectionString` utiliza `File.ReadAllText(passwordFile).Trim()` para obtener la contraseña. No hay un valor de contraseña escrito en el Compose.

En Compose local, el secret se entrega mediante un archivo montado dentro del contenedor. Esta demostración utiliza **Compose secrets**, no el almacenamiento cifrado de secrets de Swarm. No necesitas ejecutar `docker secret create` para este caso.

**Guarda `09-01-secrets-compose.png`** con declaración, servicios consumidores y volúmenes. Incluye el archivo de código que lee la ruta si necesitas una captura adicional; no abras el archivo de contraseña.

### Paso 3. Crear las imágenes, los volúmenes y los contenedores iniciales

Ejecuta cada línea por separado:

```powershell
docker compose -f compose.caso9.yml config --quiet
docker compose -f compose.caso9.yml build frontend api
docker compose -f compose.caso9.yml create --no-build
docker volume inspect dockerexam-caso9-sql-data dockerexam-caso9-evidence-data dockerexam-caso9-redis-data --format '{{.Name}} | {{.CreatedAt}} | {{.Driver}}'
docker compose -f compose.caso9.yml up -d --no-build --wait --wait-timeout 300
docker compose -f compose.caso9.yml ps
curl.exe http://localhost:18190/api/health
```

`create --no-build` prepara la red, los volúmenes y los contenedores sin iniciar los servicios. Así puedes capturar los tres volúmenes antes del arranque de SQL. Después `up` inicia el stack. Si repites el caso, reutiliza los recursos existentes en lugar de borrarlos.

Un SQL `unhealthy` no significa que los volúmenes no existan. Compruébalos con el inspect anterior. API/frontend pueden permanecer en `Created` mientras esperan la dependencia SQL saludable. No basta con ver `ready for client connections` en un punto del log: confirma que SQL termina healthy y que no falla después durante su configuración. Un frontend Healthy solo confirma que Nginx responde a su comprobación; verifica también SQL y `/api/health`.

Espera que los cinco servicios estén saludables y que health responda **status: ok**. Esa respuesta incluye una comprobación de conexión a SQL. Abre **http://localhost:18190** y confirma que carga el portal. La versión de la API puede ser la que dejaste en el caso 08; no es lo que se evalúa aquí.

### Si el arranque falla: distinguir complejidad de contraseña e inicio de sesión

Antes de continuar con la evidencia, consulta:

```powershell
docker compose -f compose.caso9.yml ps -a
docker compose -f compose.caso9.yml logs --tail 100 sqlserver
docker compose -f compose.caso9.yml logs --tail 40 api
docker volume inspect dockerexam-caso9-sql-data dockerexam-caso9-evidence-data dockerexam-caso9-redis-data --format '{{.Name}} | {{.CreatedAt}}'
```

| Mensaje | Qué significa |
| --- | --- |
| `Password validation failed` / `not complex enough` | La contraseña fue rechazada durante la configuración inicial; longitud suficiente no garantiza complejidad |
| `Login failed for user 'sa'`, error 18456, estado 8 en el log SQL | La conexión llega al servidor, pero la contraseña presentada no coincide con la almacenada para sa |
| `dependency failed to start` | Compose bloquea el arranque de los servicios dependientes de SQL; lee el error SQL que lo causa |
| Volúmenes visibles en inspect aunque SQL falle | Los volúmenes ya se crearon; el problema del servicio debe resolverse por separado |

API y SQL deben tener montado el mismo archivo `secrets/caso9_db_password.txt`. La API lee su contenido y el healthcheck SQL también lo utiliza. **Si cambias ese archivo después de inicializar SQL, pueden fallar ambos accesos aunque el archivo sea el mismo y tenga formato correcto.** La contraseña de sa ya está guardada dentro del volumen SQL; recrear el contenedor no la reemplaza.

**Recuperación A: conservar los datos y usar la contraseña con la que SQL se inicializó correctamente.** Si conoces esa contraseña, detén los servicios del caso 09:

```powershell
docker compose -f compose.caso9.yml stop
```

En el editor restaura **esa contraseña exacta** en `secrets/caso9_db_password.txt`, sin mostrarla. No inventes otra ni cambies la de otros casos. Vuelve a crear los contenedores para que todos monten el archivo actual, conservando los volúmenes:

```powershell
docker compose -f compose.caso9.yml up -d --no-build --force-recreate --wait --wait-timeout 300
docker compose -f compose.caso9.yml ps
curl.exe http://localhost:18190/api/health
```

La recreación reaplica el archivo correcto; **no rota** la contraseña de la base. Si no conoces la contraseña válida y existen datos que conservar, no borres el volumen ni ejecutes la recuperación B: conserva los datos para recuperar el acceso administrativo de SQL.

**Recuperación B: reiniciar solo la base de un laboratorio nuevo, sin datos que conservar.** Úsala únicamente si todavía no guardaste notas/imágenes que necesites y no puedes recuperar la contraseña de esa inicialización. Es una reparación anterior a la prueba; no forma parte de la demostración de persistencia.

Primero detén y retira los contenedores del caso 09:

```powershell
docker compose -f compose.caso9.yml stop
docker compose -f compose.caso9.yml rm -f
```

Edita a mano `secrets/caso9_db_password.txt` con una contraseña nueva que cumpla los cuatro grupos y guárdala en el formato del paso 1. Después elimina **únicamente** el volumen SQL de este laboratorio vacío:

```powershell
docker volume rm dockerexam-caso9-sql-data
```

Ese comando **elimina la base SQL del caso 09**, incluidos los metadatos de cualquier imagen que hubiera allí. No lo ejecutes si necesitas esos datos. No elimines los volúmenes del portal original ni los de otros casos.

Reconstruye la estructura e inicia con esa contraseña, sin recompilar imágenes:

```powershell
docker compose -f compose.caso9.yml create --no-build
docker compose -f compose.caso9.yml up -d --no-build --wait --wait-timeout 300
docker compose -f compose.caso9.yml ps
curl.exe http://localhost:18190/api/health
```

Solo después de quedar healthy/ok, comienza la evidencia desde el paso 4. Guarda los datos de prueba en el paso 5 y registra las nuevas fechas de volúmenes en el paso 6. **Durante la prueba de persistencia posterior no se elimina ningún volumen.**

### Paso 4. Demostrar que ambos servicios pueden leer el secret

```powershell
docker compose -f compose.caso9.yml exec api printenv Database__PasswordFile
docker compose -f compose.caso9.yml exec api ls -l /run/secrets/db_password
docker compose -f compose.caso9.yml exec api test -r /run/secrets/db_password
$LASTEXITCODE
docker compose -f compose.caso9.yml exec sqlserver test -r /run/secrets/db_password
$LASTEXITCODE
curl.exe http://localhost:18190/api/health
```

`printenv` debe mostrar la **ruta**, no la contraseña. Cada comprobación `test -r` debe terminar con **0**, mostrado por la línea `$LASTEXITCODE` que ejecutas inmediatamente después. Comprueba también `status: ok` y explica la relación entre el montaje, el código que lee el archivo y la conexión SQL funcionando.

**Guarda `09-02-secret-uso.png`.** Esta captura demuestra acceso y uso de la configuración sin mostrar el valor del secret. No ejecutes `cat` ni `Get-Content` sobre el archivo de contraseña para obtener evidencia.

### Paso 5. Guardar tres datos identificables ANTES de eliminar

En el portal del laboratorio **18190**, abre el **caso 09**:

1. En «Notas de la implementación», escribe un texto único, por ejemplo `Persistencia caso 09 -` seguido de tu fecha y hora reales. Guarda la nota.
2. Sube una captura PNG/JPG/WebP real, de hasta 8 MB, con título y descripción identificables. Pulsa «Guardar evidencia» y espera la confirmación.
3. Recarga la página. Comprueba el texto exacto de la nota y abre «Ver detalle» para confirmar que la imagen carga y conserva título/descripción.

Después crea una marca independiente en Redis:

```powershell
docker compose -f compose.caso9.yml exec redis redis-cli SET caso09:marca PRUEBA-PERSISTENCIA-CASO09
docker compose -f compose.caso9.yml exec redis redis-cli GET caso09:marca
docker compose -f compose.caso9.yml exec api ls -l /data/evidence
```

Redis debe responder **OK** al guardar y devolver **PRUEBA-PERSISTENCIA-CASO09** al leer. En `/data/evidence` debe aparecer el archivo de la imagen subida; su nombre interno es generado por la API y puede ser distinto del nombre de tu PNG original.

**Guarda `09-03-datos-antes.png`** con nota, detalle de imagen y la marca Redis. Usa capturas adicionales para que se lean título y descripción. No continúes si los datos todavía no se recuperan después de recargar.

### Paso 6. Registrar IDs, volúmenes y montajes ANTES

```powershell
docker inspect dockerexam-caso9-api-1 dockerexam-caso9-sqlserver-1 dockerexam-caso9-redis-1 --format '{{.Name}} {{.Id}}'
docker volume inspect dockerexam-caso9-sql-data dockerexam-caso9-evidence-data dockerexam-caso9-redis-data --format '{{.Name}} | {{.CreatedAt}} | {{.Driver}}'
docker inspect dockerexam-caso9-api-1 dockerexam-caso9-sqlserver-1 dockerexam-caso9-redis-1 --format '{{.Name}} {{json .Mounts}}'
```

Guarda los **IDs completos de los tres contenedores de datos**, los nombres/fechas de creación de sus volúmenes y los montajes. En el montaje de cada servicio identifica:

- API: volumen `dockerexam-caso9-evidence-data` en `/data/evidence`.
- SQL: volumen `dockerexam-caso9-sql-data` en `/var/opt/mssql`.
- Redis: volumen `dockerexam-caso9-redis-data` en `/data`.
- API y SQL: secret montado en `/run/secrets/db_password`, con `RW: false`.

`Type: volume` distingue los datos persistentes del montaje del secret. Mostrar `.Mounts` no muestra el contenido de la contraseña.

**Guarda `09-04-ids-volumenes-antes.png`**. Puedes separar las salidas en varias imágenes; deben poder compararse después.

### Paso 7. Detener los cinco contenedores del laboratorio

Verifica otra vez que guardaste la evidencia anterior. Ejecuta:

```powershell
docker compose -f compose.caso9.yml stop
docker compose -f compose.caso9.yml ps -a
```

Los servicios del caso 09 deben aparecer detenidos. Desde ahora **18190 no responderá** hasta recrear el stack; el portal original en 18080 sigue siendo otro proyecto.

Detener es un paso previo: por sí solo no demuestra persistencia tras eliminación. El siguiente paso es necesario para cubrir la rúbrica.

### Paso 8. Eliminar contenedores y comprobar que permanecen los volúmenes

```powershell
docker compose -f compose.caso9.yml rm -f
docker compose -f compose.caso9.yml ps -a
docker volume inspect dockerexam-caso9-sql-data dockerexam-caso9-evidence-data dockerexam-caso9-redis-data --format '{{.Name}} | {{.CreatedAt}} | {{.Driver}}'
```

La salida de rm debe mostrar la eliminación de los cinco contenedores. `ps -a` debe quedar sin filas de contenedores de este proyecto. La consulta de volúmenes debe seguir mostrando **los mismos nombres y fechas de creación del paso 6**.

Aquí `rm -f` elimina contenedores detenidos y omite la pregunta de confirmación. Conserva la red, los volúmenes y el archivo del secret. No uses `down -v`, `volume rm` ni `volume prune` durante esta demostración.

**Guarda `09-05-eliminacion.png`**, con eliminación, listado vacío y volúmenes conservados. El archivo Compose por sí solo no prueba que eliminaste los contenedores: captura la ejecución real.

### Paso 9. Crear contenedores nuevos con los mismos volúmenes

Mantén el mismo `compose.caso9.yml` y el mismo archivo de contraseña. Las imágenes del arranque inicial siguen disponibles:

```powershell
docker compose -f compose.caso9.yml up -d --no-build --pull never --wait --wait-timeout 300
docker compose -f compose.caso9.yml ps
docker inspect dockerexam-caso9-api-1 dockerexam-caso9-sqlserver-1 dockerexam-caso9-redis-1 --format '{{.Name}} {{.Id}}'
```

Compose debe crear contenedores nuevos y llevar los cinco servicios a estado saludable. Compara los IDs de API, SQL y Redis con los del paso 6: **deben ser diferentes**. Sus nombres pueden mantenerse iguales; eso no significa que sean los mismos contenedores.

El arranque usa las imágenes existentes y reutiliza los volúmenes declarados. No reconstruyas el código, no renombres los volúmenes y no cambies la contraseña en medio de esta prueba.

**Guarda `09-06-recreacion.png`**, con creación, estado saludable y nuevos IDs.

### Paso 10. Demostrar los mismos montajes, la marca Redis y el archivo de imagen

```powershell
docker volume inspect dockerexam-caso9-sql-data dockerexam-caso9-evidence-data dockerexam-caso9-redis-data --format '{{.Name}} | {{.CreatedAt}} | {{.Driver}}'
docker inspect dockerexam-caso9-api-1 dockerexam-caso9-sqlserver-1 dockerexam-caso9-redis-1 --format '{{.Name}} {{json .Mounts}}'
docker compose -f compose.caso9.yml exec redis redis-cli GET caso09:marca
docker compose -f compose.caso9.yml exec api ls -l /data/evidence
curl.exe http://localhost:18190/api/health
```

Comprueba nombres y fechas de los volúmenes, sus destinos en los contenedores nuevos, la marca exacta **PRUEBA-PERSISTENCIA-CASO09** y el archivo de imagen presente. La salud debe volver a responder **status: ok**.

**Guarda `09-07-montajes-redis-despues.png`** con estos resultados. El siguiente paso aporta la comparación visual de SQL y de la imagen.

### Paso 11. Recuperar la misma nota y abrir la misma imagen DESPUÉS

Abre una ventana privada/incógnito y entra a **http://localhost:18190**. Así consultas el despliegue recreado sin depender de la página o imagen que tenías abierta. Regresa al caso 09 y comprueba:

- La nota contiene exactamente el texto único que escribiste antes.
- La evidencia conserva su título y descripción.
- Al pulsar «Ver detalle», la misma imagen carga correctamente.

Puedes comprobar también desde **http://localhost:18192**, que usa la segunda instancia del frontend del laboratorio. No compares estos datos con los del portal original 18080: tienen almacenamiento distinto.

**Guarda `09-08-datos-despues.png`** mostrando la nota y la imagen abierta con la dirección del caso 09 visible. Si solo aparece el título pero la imagen no carga, todavía no has demostrado la persistencia completa.

### Paso 12. Comparar, registrar y cerrar el caso

La comparación final debe quedar así:

| Elemento | Qué debe suceder después de eliminar/recrear |
| --- | --- |
| IDs de API, SQL y Redis | Cambian: son contenedores nuevos |
| Nombres de los contenedores | Pueden mantenerse iguales |
| Nombres y fechas de creación de los volúmenes | Se mantienen: son los volúmenes anteriores |
| Montajes de API, SQL y Redis | Apuntan a esos mismos volúmenes y rutas |
| Archivo del secret | Se reutiliza, sin cambiar la contraseña |
| Nota, título y descripción | Conservan los valores exactos guardados en SQL |
| Archivo e imagen abierta | Siguen disponibles desde el volumen de evidencias |
| Marca Redis | Conserva PRUEBA-PERSISTENCIA-CASO09 |

| Captura sugerida | Qué demuestra |
| --- | --- |
| `09-01-secrets-compose.png` | Secret declarado, servicios consumidores y volúmenes |
| `09-02-secret-uso.png` | Archivo legible por API/SQL y conexión SQL correcta sin revelar la contraseña |
| `09-03-datos-antes.png` | Nota, imagen y marca Redis guardadas antes |
| `09-04-ids-volumenes-antes.png` | IDs originales, nombres/fechas de volúmenes y montajes |
| `09-05-eliminacion.png` | Contenedores eliminados y volúmenes conservados |
| `09-06-recreacion.png` | Nuevos contenedores saludables e IDs diferentes |
| `09-07-montajes-redis-despues.png` | Mismos volúmenes/montajes, archivo persistido y marca Redis conservada |
| `09-08-datos-despues.png` | Misma nota, metadatos e imagen recuperados desde el portal |

Guarda los PNG en `EVINDENCIA/caso-09` y súbelos al **caso 09 del portal original en http://localhost:18080**. En la descripción indica los datos reales que comparaste y explica: SQL guarda nota/metadatos; evidence-data guarda la imagen; Redis usa su propio volumen. No marques el caso como listo solo por obtener Healthy.

Al terminar puedes detener únicamente el laboratorio, conservando sus datos para la sustentación:

```powershell
docker compose -f compose.caso9.yml stop
```

### Si algo no coincide

| Problema | Qué revisar antes de continuar |
| --- | --- |
| El secret no existe o aparece como carpeta | Corrige `secrets/caso9_db_password.txt`; Test-Path con PathType Leaf debe ser True y PSIsContainer False |
| test -r devuelve un valor distinto de 0 | Revisa que el archivo sea legible, el montaje y el servicio correcto; no expongas su contenido para diagnosticar |
| SQL no inicia o la API no conecta | Consulta `docker compose -f compose.caso9.yml logs --tail 50 sqlserver` y los logs de api; conserva el archivo de contraseña original de esa base |
| Password validation failed / not complex enough | Revisa los cuatro grupos de caracteres del paso 1; si luego cambió a error 18456, sigue la recuperación del paso 3 |
| Login failed for sa / 18456 estado 8 | La contraseña no coincide con la guardada en SQL; restaurar el archivo válido no equivale a crear otra contraseña. Consulta las recuperaciones A/B |
| El portal muestra datos vacíos después | Confirma 18190/18192, nombres exactos de los volúmenes y montajes; no crees otra nota como reemplazo antes de revisar |
| Hay metadatos pero la imagen no carga | Revisa el volumen evidence-data, el listado de `/data/evidence` y los logs de la API |
| Los IDs de contenedor no cambiaron | Confirma que realmente ejecutaste rm y que capturaste los IDs del proyecto caso9 |
| Los volúmenes tienen nuevas fechas o faltan los datos | Revisa si se cambiaron nombres o se eliminaron volúmenes; no presentes esa ejecución como prueba exitosa de persistencia |
| Redis devuelve nil | Verifica que guardaste la marca antes, que Redis usa su volumen original y que tiene AOF habilitado |
| Puerto ocupado | Revisa `docker ps`; este laboratorio usa 18190/18191/18192 |

Fuentes: [secrets en Compose](https://docs.docker.com/compose/how-tos/use-secrets/), [ciclo de vida de volúmenes](https://docs.docker.com/engine/storage/volumes/), [preparar contenedores con Compose create](https://docs.docker.com/reference/cli/docker/compose/create/), [eliminar contenedores detenidos con Compose rm](https://docs.docker.com/reference/cli/docker/compose/rm/), [crear contenedores con Compose up](https://docs.docker.com/reference/cli/docker/compose/up/), [contraseña y volúmenes SQL existentes](https://learn.microsoft.com/sql/linux/containers/troubleshoot) y [error SQL 18456](https://learn.microsoft.com/sql/relational-databases/errors-events/mssqlserver-18456-database-engine-error).

## Caso 10 · Comandos útiles para operar y diagnosticar Docker (2 puntos)

**Qué pide la rúbrica:** presentar otros comandos, aplicaciones o usos de Docker/Compose investigados como complemento del entrenamiento. No fija una cantidad de comandos o capturas: demuestra los que ejecutes y puedas explicar con sus resultados reales.

### Qué laboratorio vas a usar

Para este caso utiliza el **portal original**, con **`compose.yml`**, proyecto **`dockerexam`** y dirección **http://localhost:18080**. No hace falta crear otro Compose: vas a observar y diagnosticar un stack que ya funciona.

Los comandos con nombres `dockerexam-api-1`, `dockerexam-frontend-1` y red `dockerexam-net` corresponden a ese proyecto. Los comandos de otros casos usan recursos diferentes. Ejecuta cada línea individualmente desde la raíz:

```powershell
Set-Location 'C:\Users\JCUNACHI\Documents\CURSOS\EXAMEN-DOCKER'
docker compose -f compose.yml config --quiet
docker compose -f compose.yml ps -a
```

El stack debe estar iniciado antes de usar exec/stats/top. Si sus contenedores existen pero están detenidos, puedes iniciarlos con `docker compose -f compose.yml start` y volver a consultar ps. Si no existen, sigue la preparación del portal original. No necesitas reconstruir las imágenes para obtener estas evidencias.

La propuesta contiene **seis grupos principales (1 a 6)** y **dos complementarios (7 y 8)**. Por cada grupo elegido guarda el comando, su salida y una explicación: **qué consultaste → qué observaste → para qué sirve**. Los títulos/descripciones siguientes son modelos para completar con tus resultados; no sustituyen una ejecución real.

La versión actual de los comandos está en este Markdown y en la hoja de ayuda. Si el portal que ya tenías iniciado todavía muestra el catálogo anterior, puedes incorporar los recursos nuevos reconstruyendo solo su API antes de obtener las capturas:

```powershell
docker compose -f compose.yml up -d --build --no-deps --wait --wait-timeout 300 api
```

Ese paso actualiza la imagen/API del portal original y requiere que SQL/Redis ya estén saludables. Conserva sus volúmenes y su archivo de contraseña. Después vuelve a consultar ps y health para empezar la observación.

### Grupo 1. Salud del stack y detalle del healthcheck

```powershell
docker compose -f compose.yml ps
docker inspect dockerexam-api-1 --format 'Estado={{.State.Status}} Salud={{.State.Health.Status}}'
docker inspect dockerexam-api-1 --format '{{json .State.Health.Log}}'
curl.exe http://localhost:18080/api/health
```

**Utilidad:** distinguir un proceso ejecutándose de un servicio disponible y consultar el resultado reciente del healthcheck. En este proyecto, el healthcheck de la API consulta `/api/health`, que verifica conexión a SQL. El healthcheck del frontend comprueba que Nginx sirve su página; no prueba por sí solo toda la integración.

**Qué explicar:** ps muestra servicios y estados; el inspect de Health.Log muestra ejecuciones recientes de la comprobación y sus ExitCode. Un ExitCode 0 indica éxito de esa comprobación. Health.Log no es el historial completo de la aplicación. La petición HTTP debe responder `status: ok` para acreditar que API y SQL están disponibles.

Abre compose.yml y muestra `depends_on` con `condition: service_healthy`, además de sus healthchecks. Explica que las dependencias condicionan el arranque; si luego aparece un fallo, hay que consultar de nuevo el estado y los logs.

**Evidencia:** `10-01-salud.png`.

**Título sugerido:** «Healthchecks y disponibilidad de API/SQL».

**Descripción modelo:** «Consulté el estado de los servicios y las últimas comprobaciones de la API. Observé [estados reales y ExitCode]. La petición /api/health devolvió [resultado]. Esto permite comprobar disponibilidad antes de diagnosticar el servicio».

### Grupo 2. Consumo de recursos y procesos

```powershell
docker compose -f compose.yml stats --no-stream
docker compose -f compose.yml top api
```

**Utilidad:** detectar qué servicios consumen CPU/memoria y comprobar qué procesos están ejecutándose en un contenedor. `--no-stream` toma una muestra y devuelve el control a la terminal; top muestra procesos, no un gráfico de carga.

| Campo de stats | Qué puedes interpretar |
| --- | --- |
| CPU % | Uso de CPU en la muestra; un valor bajo o 0 puede ser normal en reposo |
| MEM USAGE / LIMIT | Memoria consumida y límite informado |
| NET I/O | Datos recibidos y enviados |
| BLOCK I/O | Lecturas/escrituras reportadas para dispositivos de bloque |
| PIDS | Procesos y tareas/hilos contabilizados; no tiene por qué coincidir con las filas de top |

**Qué explicar:** identifica con valores reales qué servicio consume más memoria en ese momento. No inventes un consumo mínimo ni generes carga artificial para la captura. En top localiza el proceso de la API, asociado a dotnet y Exam.Api.dll.

**Evidencia:** `10-02-recursos-procesos.png`.

**Título sugerido:** «Diagnóstico de CPU, memoria y procesos».

**Descripción modelo:** «Tomé una muestra con stats e identifiqué [servicio/consumo observado]. Con top comprobé [proceso observado]. Estos comandos ayudan a localizar consumo elevado y verificar qué se ejecuta dentro del contenedor».

### Grupo 3. Usuario de ejecución y modo privilegiado

```powershell
docker compose -f compose.yml exec api id
docker inspect dockerexam-api-1 --format 'Usuario={{.Config.User}} Privileged={{.HostConfig.Privileged}}'
```

**Utilidad:** comprobar la identidad con la que se ejecuta la aplicación y si Docker activó el modo privilegiado del contenedor.

**Qué explicar:** el Dockerfile del backend termina con `USER app`. Espera que id identifique ese usuario con UID distinto de 0 y que el inspect muestre `Usuario=app` y `Privileged=false`. Son dos comprobaciones distintas: usuario del proceso y configuración de privilegios del contenedor. Ejecutar la API sin ser root reduce los permisos que recibe su proceso.

**Evidencia:** `10-03-usuario-privilegios.png`.

**Título sugerido:** «API ejecutada con usuario app».

**Descripción modelo:** «Verifiqué la identidad del proceso y el modo privilegiado. La salida mostró [usuario/UID] y Privileged=[valor real]. Lo relacioné con USER app del Dockerfile para explicar los permisos de ejecución».

### Grupo 4. Red interna, DNS y puertos publicados

```powershell
docker network inspect dockerexam-net --format '{{range .Containers}}{{.Name}} {{.IPv4Address}}{{println}}{{end}}'
docker compose -f compose.yml exec frontend wget -qO- http://api:8080/api/health
docker port dockerexam-frontend-1
docker ps --filter publish=18080 --format 'table {{.Names}}\t{{.Ports}}'
```

**Utilidad:** ver qué contenedores comparten red, probar una petición entre servicios y localizar quién tiene publicado un puerto del host.

**Qué explicar:** la petición wget se ejecuta dentro del frontend y llega a la API mediante el nombre de servicio **api** y su puerto interno **8080**. Si devuelve `status: ok`, demuestra que ese recorrido funciona. No necesita usar una IP copiada a mano.

`docker port` debe mostrar las publicaciones 18080/18081 del contenedor frontend. El filtro publish permite localizar al propietario de un puerto; para investigar otro conflicto puedes repetirlo cambiando el número, por ejemplo a 18090. SQL y Redis utilizan sus puertos internos y no tienen publicación al host en este Compose.

**Evidencia:** `10-04-red-puertos.png`.

**Título sugerido:** «Conectividad por DNS y diagnóstico de puertos».

**Descripción modelo:** «Identifiqué los servicios de la red, ejecuté una petición frontend→api por su nombre DNS y consulté las publicaciones del frontend. Observé [respuesta y puertos reales]. Esto ayuda a distinguir comunicación interna de acceso desde Windows y a localizar puertos ocupados».

### Grupo 5. Logs, reinicios y memoria insuficiente

```powershell
docker compose -f compose.yml logs --tail 40 --timestamps api
docker compose -f compose.yml logs --since 10m --tail 40 sqlserver
docker inspect dockerexam-api-1 --format 'Estado={{.State.Status}} Reinicios={{.RestartCount}} OOMKilled={{.State.OOMKilled}}'
```

**Utilidad:** consultar mensajes recientes, acotar el intervalo de diagnóstico y comprobar reinicios o una terminación registrada por falta de memoria.

**Qué explicar:** correlaciona las fechas y los mensajes con el estado del servicio. `RestartCount` corresponde al contenedor actual; no es un historial de todas las instancias anteriores. `OOMKilled=true` es una señal para investigar memoria, mientras que false no descarta otros fallos. Si no hubo mensajes de SQL en los últimos diez minutos, esa consulta puede estar vacía; consulta `docker compose -f compose.yml logs --tail 40 sqlserver` para ampliar la observación.

Puedes explicar el error real que investigaste en el caso 09: estado unhealthy y error SQL 18456 requieren revisar el log del servicio y su causa de autenticación. Si adjuntas aquella captura, identifícala como diagnóstico del **caso 09** y documenta la corrección; no la presentes como salida del portal original. No provoques otro fallo para fabricar evidencia.

Opcionalmente puedes seguir los logs en vivo:

```powershell
docker compose -f compose.yml logs --follow --tail 20 api
```

Sal de esa vista con **Ctrl + C**. En logs --follow detienes la consulta; el contenedor sigue ejecutándose.

**Evidencia:** `10-05-logs-reinicios.png`.

**Título sugerido:** «Diagnóstico con logs y estado del contenedor».

**Descripción modelo:** «Consulté logs con fechas y límites de líneas/tiempo. Observé [mensajes reales], RestartCount=[valor] y OOMKilled=[valor]. Expliqué cómo estos datos permiten distinguir errores de aplicación, reinicios y posibles problemas de memoria».

### Grupo 6. Espacio utilizado e historial de la imagen

```powershell
docker system df
docker image ls dockerexam-api
docker image history dockerexam-api:local
```

**Utilidad:** observar espacio ocupado por Docker y relacionar una imagen con sus capas de construcción. system df informa sobre **todo el motor Docker**, no solo sobre el proyecto original.

**Qué explicar:** distingue imágenes, contenedores, volúmenes y caché de construcción. El espacio indicado como recuperable es información para analizar; estos comandos no eliminan recursos. En history relaciona capas con las instrucciones del Dockerfile, como copiar la aplicación o preparar su directorio. Las imágenes pueden compartir capas; no sumes sus tamaños como si todos fueran espacio independiente.

Si deseas más detalle, puedes ejecutar `docker system df -v` y capturar solo las filas relevantes, explicando que el resultado sigue siendo global.

**Evidencia:** `10-06-almacenamiento-capas.png`.

**Título sugerido:** «Uso de disco e historial de imágenes».

**Descripción modelo:** «Revisé el espacio mostrado para [categorías observadas] y las capas de la imagen API. Esto permite analizar almacenamiento y entender cómo el Dockerfile produce una imagen antes de decidir acciones de mantenimiento».

### Grupo 7. Complementario: comprobar Redis y el panel de servicios

```powershell
docker compose -f compose.yml exec redis redis-cli PING
curl.exe http://localhost:18080/api/services
docker compose -f compose.yml exec redis redis-cli GET dockerexam:visits
curl.exe http://localhost:18080/api/services
docker compose -f compose.yml exec redis redis-cli GET dockerexam:visits
```

**Utilidad:** comprobar Redis y verificar que participa en una operación de la aplicación. PING debe responder **PONG**. `/api/services` comprueba API/SQL/Redis e incrementa el contador de consultas; este grupo sí aumenta ese dato del portal.

**Qué explicar:** compara los valores de visits con el contador consultado en Redis. Otras consultas del portal también pueden incrementarlo, de modo que no necesitas demostrar una diferencia exacta de uno. Debes observar conectividad y un valor numérico coherente. Una respuesta redis:false no acredita la integración y necesita diagnóstico.

**Evidencia opcional:** `10-07-redis-servicios.png`.

**Título sugerido:** «Redis integrado con la API».

**Descripción modelo:** «Redis respondió [resultado de PING] y el panel /api/services informó [estados y contador reales]. Consulté la clave dockerexam:visits para relacionar la petición HTTP con el servicio Redis».

### Grupo 8. Complementario: cambios de archivos del contenedor

```powershell
docker diff dockerexam-frontend-1
```

**Utilidad:** observar archivos/directorios que cambiaron desde la creación del contenedor. Interpreta los símbolos de la salida:

| Símbolo | Significado |
| --- | --- |
| A | Archivo o directorio añadido |
| C | Archivo o directorio modificado |
| D | Archivo o directorio eliminado |

Relaciona una ruta real que aparezca con la ejecución de Nginx, por ejemplo un archivo de proceso o configuración si está presente. Si la salida está vacía, registra que no reportó cambios; no crees archivos artificiales para que aparezcan resultados. Esta observación no sustituye la prueba de persistencia de los volúmenes del caso 09.

**Evidencia opcional:** `10-08-cambios-archivos.png`.

**Título sugerido:** «Inspección de cambios del sistema de archivos».

**Descripción modelo:** «Consulté diff y observé [rutas y símbolos reales, o salida vacía]. Expliqué qué cambios reporta Docker sobre ese contenedor y su utilidad para investigar modificaciones durante la ejecución».

### Cómo registrar y sustentar las evidencias

| Captura sugerida | Comando o tema principal |
| --- | --- |
| `10-01-salud.png` | ps, inspect de healthcheck y health HTTP |
| `10-02-recursos-procesos.png` | stats y top |
| `10-03-usuario-privilegios.png` | exec id e inspect de usuario/privilegios |
| `10-04-red-puertos.png` | network inspect, petición por DNS, port y filtro publish |
| `10-05-logs-reinicios.png` | logs, RestartCount y OOMKilled |
| `10-06-almacenamiento-capas.png` | system df, image ls y history |
| `10-07-redis-servicios.png` | Opcional: Redis y contador de servicios |
| `10-08-cambios-archivos.png` | Opcional: diff e interpretación de A/C/D |

Guarda las capturas elegidas en **`EVINDENCIA/caso-10`**. Sube cada imagen al **caso 10 del portal original en http://localhost:18080**, con título, descripción y los valores que realmente observaste. Si un grupo no cabe en una captura, divide sus salidas en varias.

La explicación debe aportar utilidad: qué problema podrías investigar con ese comando y qué significa su salida. Puedes utilizar los seis grupos principales y ampliar con los opcionales. Esta selección es una propuesta de investigación; la rúbrica no exige exactamente ocho capturas ni asigna puntos por cada comando.

### Fuentes oficiales para tu investigación

- [stats: métricas de recursos](https://docs.docker.com/reference/cli/docker/container/stats/)
- [top: procesos de los servicios](https://docs.docker.com/reference/cli/docker/compose/top/)
- [logs: tiempo, líneas y seguimiento](https://docs.docker.com/reference/cli/docker/compose/logs/)
- [port: publicaciones de puertos](https://docs.docker.com/reference/cli/docker/container/port/)
- [system df: uso de disco](https://docs.docker.com/reference/cli/docker/system/df/)
- [diff: cambios de archivos](https://docs.docker.com/reference/cli/docker/container/diff/)

## Repositorio y entrega final

Versiona código, comandos y configuraciones. Revisa `.gitignore`: excluye `secrets/*.txt`, `.env`, `tmp/`, dependencias y compilaciones. No añadas credenciales.

Si aún no hay repositorio Git, ejecuta a mano:

```powershell
git init
git status --short --untracked-files=all
git add .
git diff --cached --stat
git commit -m "Proyecto final Docker con guia manual"
git branch -M main
```

Crea un repositorio vacío en GitHub/GitLab desde el navegador, copia su URL real y sustituye `URL_HTTPS_REAL`:

```powershell
git remote add origin URL_HTTPS_REAL
git push -u origin main
```

Si ya hay repositorio o remoto, revisa `git status` y `git remote -v` y usa el existente. Registra el enlace en Recursos y entrega. Incluye Dockerfile, `.dockerignore`, Compose, Markdown y configuración manual del caso 03. Conserva los créditos del curso y de las imágenes base.

Revisa los diez casos y sus capturas reales. Ensaya la presentación en vivo: los pantallazos la respaldan, no la sustituyen. No marques como completada una publicación o demostración pendiente de ejecutar.

## Descargar un PDF con todo lo realizado

Este paso reúne todo el trabajo guardado en el portal. Puedes ejecutarlo durante el avance o al completar los diez casos.

1. Abre cada caso y revisa las notas de desarrollo, resultados y capturas reales. Completa el título y la descripción de cada evidencia; guarda los cambios antes de salir.
2. Mantén el estado real de cada caso: **Pendiente**, **En progreso** o **Listo para revisar**. El informe incluirá también las notas y evidencias de casos aún no terminados.
3. Revisa el nombre del alumno y el enlace real del repositorio en **Editar datos**.
4. En **Vista general** o **Recursos y entrega**, pulsa **Descargar PDF**. Está disponible con cualquier porcentaje de avance. Durante la generación espera a que empiece la descarga.
5. Abre el PDF: debe contener portada con el porcentaje real, resumen, diez casos con sus estados, requisitos y notas, y todas las imágenes adjuntas con sus títulos y descripciones. Las imágenes conservan su resolución; las descripciones extensas continúan en las páginas siguientes. Cuando haya 10 de 10 casos listos, la portada indicará **Informe final**.
6. Si un caso aparece sin notas o sin evidencias, regresa al portal, completa lo pendiente, guarda y genera otra versión. Si una imagen falta o está dañada, el portal muestra el error y debes volver a cargar esa evidencia.
7. Conserva la versión final junto con el repositorio y las capturas originales. El informe refleja los datos guardados al solicitarlo y no sustituye la sustentación ni la evaluación del docente.

Si el botón no aparece después de actualizar el código, reconstruye manualmente las aplicaciones del portal principal que ya está funcionando; ejecuta cada línea por separado desde la raíz del proyecto:

```powershell
docker compose build api frontend
docker compose up -d --no-deps --wait --wait-timeout 300 api
docker compose up -d --no-deps --wait --wait-timeout 120 frontend frontend-demo
```

Recarga `http://localhost:18080` con `Ctrl + F5`. Se reutilizan la base y las evidencias de los volúmenes actuales. La actualización no marca casos como completos; el estado lo guardas tú después de realizar y documentar cada demostración.

## Referencias oficiales

- [Docker CLI](https://docs.docker.com/reference/cli/docker/)
- [Compose up: reconstrucción y recreación](https://docs.docker.com/reference/cli/docker/compose/up/)
- [Combinar configuraciones Compose](https://docs.docker.com/compose/how-tos/multiple-compose-files/merge/)
- [Comandos de redes disponibles](https://docs.docker.com/reference/cli/docker/network/)
- [Compose secrets](https://docs.docker.com/compose/how-tos/use-secrets/)
- [Publicar y descargar en Azure ACR](https://learn.microsoft.com/azure/container-registry/container-registry-get-started-docker-cli)

Fuente de requisitos: PDF original de la rúbrica, conservado en la raíz y en `backend/Content/rubric.pdf`.
