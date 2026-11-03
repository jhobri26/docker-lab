# Docker Cheat Sheet · Docker Lab

**Guía práctica, explicada y orientada a evidencias**  
**Alumno:** JHONATAN BRICEÑO CUÑACHI CANAQUIRI  
**Curso:** CURS-000662  
**Entorno de ejecución:** Windows PowerShell, Docker Desktop y Docker Compose V2  
**Fecha de edición:** 1 de noviembre de 2026

---

# PARTE I. REFERENCIA DE COMANDOS DOCKER

## A. Imágenes del registro y contenedores

### A.1. Descargar una imagen de Docker Hub

```powershell
 docker pull nginx:1.28-alpine
```

**Qué hace:** Descarga la imagen `nginx` con la etiqueta `1.28-alpine` al almacenamiento local, sin crear ni iniciar contenedores. **Resultado esperado:** Docker informa que descargó o ya tiene las capas de la imagen. **Verificación:** `docker image ls nginx`.

```powershell
 docker run -d -p 8088:80 --name ejemplo-nginx nginx:1.28-alpine
```

**Qué hace:** Si es necesario descarga la imagen, crea un contenedor llamado `ejemplo-nginx`, lo inicia en segundo plano (`-d`) y publica su puerto interno 80 en el puerto 8088 del equipo. **Verificación:** `curl.exe http://localhost:8088` y `docker ps`.

> `docker pull` y `docker run` no son equivalentes: el primero solo obtiene la imagen; el segundo crea e inicia un contenedor.

### A.2. Listar imágenes

```powershell
 docker image ls
```

**Qué hace:** Lista repositorio, etiqueta, identificador, fecha y tamaño de imágenes locales. **Evidencia:** salida donde figure la imagen descargada.

### A.3. Crear y ejecutar un contenedor de una aplicación

```powershell
 docker run -d --name jhocuxLogDozzel --restart unless-stopped -p 127.0.0.1:7080:8080 -v /var/run/docker.sock:/var/run/docker.sock:ro -v dozzle_data:/data amir20/dozzle:latest
```

$ docker run -d --name jhocuxUptimeKuma --restart unless-stopped -p 127.0.0.1:3001:3001 -v uptime_kuma_data:/app/data louislam/uptime-kuma:2


**Qué hace:** Crea e inicia `jhocuxLogDozzel` desde `amir20/dozzle:latest`, publicando dos puertos. Al no usar `-d`, el proceso queda adjunto a la terminal. **Precaución:** ambos puertos deben estar libres; la disponibilidad de la imagen y sus puertos efectivos depende de esa aplicación. **Verificación:** `docker ps` y `docker logs jhocuxLogDozzel`.

### A.4. Crear PostgreSQL con una base inicial

```powershell
 docker volume create postgres_lab_data
 docker run -d --name postgres-lab `
   -e POSTGRES_USER=labuser `
   -e POSTGRES_PASSWORD='CambiarEstaClaveDeLaboratorio' `
   -e POSTGRES_DB=product_db `
   -p 127.0.0.1:5433:5432 `
   -v postgres_lab_data:/var/lib/postgresql/data `
   postgres:17
```

**Qué hace cada opción:** `-d` ejecuta en segundo plano; `--name` identifica el contenedor; `POSTGRES_USER` crea el usuario inicial; `POSTGRES_PASSWORD` asigna la contraseña de ejemplo; `POSTGRES_DB` crea `product_db` durante la **primera inicialización**; `-p 127.0.0.1:5433:5432` expone PostgreSQL solo en localhost; `-v` conserva los datos en un volumen independiente; `postgres:17` especifica la imagen y versión mayor. El acento grave (`` ` ``) es el continuador de líneas de PowerShell: debe ser el último carácter de la línea.

```powershell
 docker logs postgres-lab
 docker exec -it postgres-lab psql -U labuser -d product_db -c 'SELECT current_database();'
```

**Resultado esperado:** la consulta muestra `product_db`. **Importante:** las variables de inicialización de PostgreSQL no cambian la contraseña ni recrean la base si el volumen ya contiene una instalación. La contraseña usada en este ejemplo es solo didáctica; en un entorno real emplear secretos.

### A.5. Listar contenedores

```powershell
 docker ps
 docker ps -a
```

`docker ps` muestra únicamente contenedores en ejecución; `docker ps -a` incluye los detenidos. **Evidencia:** comparar ambos resultados después de detener un contenedor.

### A.6. Gestionar todos los contenedores (peligroso fuera del laboratorio)

```powershell
# Primero, consultar cuáles se verían afectados
 docker ps --format 'table {{.Names}}\t{{.Status}}'

# Detener todos los contenedores en ejecución (NO ejecutar si hay otros servicios)
 docker ps -q | ForEach-Object { docker stop $_ }

# Iniciar todos los contenedores existentes
 docker ps -aq | ForEach-Object { docker start $_ }

# Eliminar todos los contenedores detenidos (destructivo)
 docker ps -aq --filter status=exited | ForEach-Object { docker rm $_ }
```

**Interpretación:** `-q` devuelve identificadores; `-a` incluye detenidos; `ForEach-Object` invoca Docker por cada ID. En PowerShell, `$(docker ps -q)` no debe asumirse como una solución segura para el caso sin resultados. **Recomendación para el laboratorio:** usar nombres explícitos o `docker compose` para no afectar proyectos ajenos.

## B. Construcción y publicación de imágenes

### B.1. Construir desde el Dockerfile de la raíz

```powershell
 docker build --no-cache --build-arg JAR_FILE='target/*.jar' -t billingapp:local .
 docker build --no-cache -t frontend-angular:local .
```

**Qué hace:** `docker build` construye una imagen, `--no-cache` omite la caché de pasos reutilizables, `--build-arg` entrega el argumento `JAR_FILE` al Dockerfile si éste lo declara mediante `ARG`, `-t` asigna el nombre/etiqueta y `.` define el contexto de construcción. **Requisito:** deben existir el Dockerfile y el artefacto JAR esperado dentro del contexto. **Verificación:** `docker image ls billingapp`.

Este es el equivalente al comando de Java que compartiste.
Parámetro	                | Descripción
docker build	            | Construye la imagen Docker
--no-cache	              | Reconstruye sin utilizar caché
-t frontend-angular:local	| Nombre y etiqueta de la imagen
.	                        | Utiliza la carpeta actual como contexto

### B.2. Usar un Dockerfile de otra ruta

```powershell
 docker build -t nombre_de_la_imagen:local -f .\ruta\Dockerfile .
```

`-f` apunta a un Dockerfile alternativo; el último `.` sigue siendo el contexto de construcción. Si el contexto excluye archivos requeridos, la construcción fallará. La existencia de `.dockerignore` permite excluir carpetas como `.git`, `node_modules` y archivos secretos.

### B.3. Etiquetar, autenticar, publicar y descargar

```powershell
 docker tag billingapp:local TU_USUARIO/billingapp:1.0.0
 docker login
 docker push TU_USUARIO/billingapp:1.0.0
 docker pull TU_USUARIO/billingapp:1.0.0
 docker logout
```

- `tag`: añade una referencia de nombre/versión a la misma imagen local; no construye de nuevo.
- `login`: inicia autenticación en un registro; preferir el mecanismo seguro/interactivo del cliente.
- `push`: envía las capas y el manifiesto al repositorio remoto para el cual se tienen permisos.
- `pull`: obtiene la versión indicada desde el registro.
- `logout`: cierra la sesión almacenada para el registro.

**Evidencia:** repositorio y etiqueta publicados, sin mostrar datos de autenticación.

## C. Gestión de contenedores

```powershell
 docker stop ejemplo-nginx
 docker start ejemplo-nginx
 docker restart ejemplo-nginx
 docker logs --tail 50 ejemplo-nginx
 docker inspect ejemplo-nginx
 docker rm -f ejemplo-nginx
 docker image rm nginx:1.28-alpine
```

**Descripción de cada ejecución:** `stop` solicita la detención ordenada; `start` arranca un contenedor existente; `restart` lo reinicia; `logs --tail 50` muestra los últimos 50 registros; `inspect` devuelve su configuración técnica y estado; `rm -f` lo detiene forzosamente si es necesario y elimina; `image rm` elimina una referencia local de imagen, siempre que Docker lo permita. **Precaución:** no uses `rm -f` sin verificar el nombre.

## D. Redes y volúmenes

```powershell
 docker network create red-lab
 docker network ls
 docker network inspect red-lab
 docker network rm red-lab

 docker volume create datos-lab
 docker volume ls
 docker volume inspect datos-lab
 docker volume rm datos-lab
```

**Descripción:** `network create` crea una red Docker definida por el usuario; `network ls` la enumera; `network inspect` enseña subred, gateway y contenedores conectados; `network rm` la elimina si no está en uso. Para volúmenes, `create` reserva almacenamiento persistente, `ls` los enumera, `inspect` muestra su ubicación y configuración, y `rm` elimina el volumen si no está en uso. **Atención:** al eliminar un volumen se pierden sus datos.

## E. Interactuar y copiar archivos

```powershell
 docker exec -it postgres-lab sh
 docker cp .\archivo.txt postgres-lab:/tmp/archivo.txt
 docker cp postgres-lab:/tmp/archivo.txt .\archivo-copia.txt
```

`exec -it` ejecuta una shell dentro de un contenedor activo (`-i` mantiene entrada interactiva y `-t` asigna terminal). `docker cp` copia archivos desde Windows hacia el contenedor o en sentido contrario. `sh` está disponible en muchas imágenes Linux, pero algunas imágenes mínimas no incluyen shell. **Verificación:** `docker exec postgres-lab ls -l /tmp/archivo.txt`.

## F. Docker Compose V2

```powershell
 docker compose config
 docker compose up -d --build
 docker compose ps
 docker compose logs --tail 50 api
 docker compose up -d --scale api=2
 docker compose down
```

- `config` valida e imprime la configuración final, incluida la resolución de variables.
- `up -d --build` construye si corresponde y levanta servicios en segundo plano.
- `ps` lista los contenedores asociados al proyecto Compose actual.
- `logs` muestra eventos de un servicio específico (`api`, si está definido).
- `up -d --scale api=2` solicita dos réplicas de `api`, **solo si la configuración admite escalar** (sin nombres fijos en conflicto, puertos host estáticos incompatibles, etc.).
- `down` detiene y elimina contenedores y redes creados por Compose; normalmente conserva los volúmenes nombrados.

**Corrección importante:** `docker-compose scale servicio=num_instancias` es sintaxis heredada. Para Compose V2 se recomienda `docker compose up -d --scale servicio=N`.

## G. Inspección, estadísticas y limpieza

```powershell
 docker inspect postgres-lab
 docker stats postgres-lab --no-stream
 docker stats --no-stream
 docker system df
```

`inspect` presenta el estado, variables, montajes, puertos y redes de un recurso; `stats --no-stream` toma una muestra puntual de CPU, memoria y E/S; `docker system df` informa espacio ocupado por recursos Docker.

```powershell
# Consultar primero el alcance: estos comandos pueden afectar otros proyectos
 docker system prune --all
 docker volume prune
 docker network prune
```

**Qué eliminan:** `docker system prune --all` puede eliminar contenedores detenidos, redes sin uso, imágenes no utilizadas y caché de compilación; **no elimina por defecto los volúmenes nombrados**. `docker volume prune` elimina volúmenes locales anónimos sin uso por defecto; con `--all` también puede afectar volúmenes nombrados sin uso. `docker network prune` elimina redes no utilizadas, respetando las redes predeterminadas. Evita ejecutarlos globalmente en equipos compartidos.

## H. Restricciones de recursos

```powershell
 docker run --rm --memory=256m --cpus=1 nginx:1.28-alpine nginx -v
```

**Qué hace:** ejecuta el comando `nginx -v` en un contenedor con límite máximo de memoria de 256 MiB y cuota equivalente a una CPU; `--rm` borra el contenedor al finalizar. La reserva y el control efectivo dependen del motor/cgroups. `--cpu-shares` expresa peso relativo bajo contención y **no equivale a un máximo fijo de CPU**.

```powershell
# Ejemplo de montaje y variable de entorno
 docker run --rm -e MODO=lab -v "${PWD}:/workspace:ro" alpine:3.20 printenv MODO
```

Monta la carpeta actual en modo de solo lectura (`:ro`) y transmite `MODO` al proceso. Docker Content Trust (`docker trust key generate`, `docker trust key load`, `docker trust signer remove`) pertenece al flujo de firma y administración de confianza de imágenes y **no sustituye un sistema general de permisos de usuarios Docker ni constituye un perfil RBAC de Docker EE**. No se recomienda ejecutar los ejemplos originales de `docker trust` como control de acceso: el argumento de `key load` debe ser un archivo PEM válido y su sintaxis no implica cargar un directorio del servidor.

---

# PARTE II. DIEZ CASOS MANUALES DE LA RÚBRICA

Ejecuta una línea a la vez. Usa la [guía de sustentación](guide.md) para la preparación, archivos que debes crear a mano, resultados esperados y nombres de capturas. No guardes estos bloques como archivos ejecutables. La sustentación es en vivo; cada caso vale 2 puntos.

## 01 · Docker Cheat Sheet personalizado

Crear una hoja de ayuda personalizada de Docker y Docker Compose con los principales comandos usados en los casos y otros relevantes para el trabajo.

### Levantar manualmente el laboratorio

```powershell
# Primero crear secrets/db_password.txt con el editor si no existe.
# Conservar la contraseña si la base ya está inicializada.
docker compose config --quiet
docker compose up -d --build --wait --wait-timeout 300
docker compose ps
```

### Consultar el stack

```powershell
docker compose logs --tail 40 api
docker compose images
docker compose config --services
```

**Evidencia que debes guardar:**

- Captura de la hoja personalizada abierta.
- Captura ejecutando dos comandos documentados.
- Archivo backend/Content/cheatsheet.md en el repositorio.

Descarga la hoja desde Recursos. commands.MD se conserva como referencia del curso; nuestra hoja se adapta a este proyecto.

## 02 · Contenedores simples

Usar imágenes públicas y demostrar creación, eliminación estándar y forzada, inspección, logs, renombre, parada y reinicio de contenedores.

### Crear, ejecutar e inspeccionar

```powershell
docker pull nginx:1.28-alpine
docker create --name exam-simple -p 127.0.0.1:18090:80 nginx:1.28-alpine
docker start exam-simple
curl.exe http://localhost:18090
docker logs exam-simple
docker inspect exam-simple
```

### Renombrar, reiniciar y eliminar

```powershell
docker rename exam-simple exam-nginx
docker stop exam-nginx
docker start exam-nginx
docker restart exam-nginx
docker stop exam-nginx
docker rm exam-nginx
docker run -d --name exam-force nginx:1.28-alpine
docker rm -f exam-force
```

**Evidencia que debes guardar:**

- Nginx visible en localhost:18090.
- Terminal con inspect, logs y rename.
- Terminal con parada, reinicio y ambas eliminaciones.

Estos contenedores son independientes del portal. Ejecuta los comandos por bloques y captura sus resultados.

## 03 · Conectando tres contenedores

Integrar tres contenedores usando sus IP internas obtenidas con inspect. Seleccionar imágenes diferentes de las usadas en clase.

### Preparar y crear SQL manualmente

```powershell
docker compose build frontend api
docker network create --label dockerexam.case03=true exam-three-net
docker volume create dockerexam-three-sql
docker volume create dockerexam-three-evidence
# Crear secrets/three-sql.txt en el editor: MSSQL_SA_PASSWORD= seguido de la misma contraseña del secret. No capturarlo.
docker run -d --name exam-three-sql --network exam-three-net -e ACCEPT_EULA=Y -e MSSQL_PID=Developer --env-file ./secrets/three-sql.txt -v dockerexam-three-sql:/var/opt/mssql mcr.microsoft.com/mssql/server:2022-latest
docker logs --tail 30 exam-three-sql
docker inspect exam-three-sql --format '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}'
```

### Crear API con la IP SQL inspeccionada

```powershell
# Esperar que SQL esté listo. Sustituir IP_SQL_REAL por la IP obtenida.
docker run -d --name exam-three-api --network exam-three-net -e "Database__Server=$ipSql" -e Database__Name=DockerExamThree -e Database__PasswordFile=/run/secrets/db_password --mount "type=bind,source=$PWD/secrets/db_password.txt,target=/run/secrets/db_password,readonly" -v dockerexam-three-evidence:/data/evidence dockerexam-api:local
docker logs --tail 30 exam-three-api
docker exec exam-three-api dotnet Exam.Api.dll --healthcheck
$LASTEXITCODE
docker inspect exam-three-api --format '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}'
docker inspect exam-three-api --format '{{json .Config.Env}}'
```

### Configurar Nginx a mano y comprobar integración

```powershell
# Crear docs/caso03/nginx.conf en el editor según la Guía de sustentación de Recursos.
# Sustituir IP_API_REAL en proxy_pass por la IP inspeccionada de la API.
docker run -d --name exam-three-front --network exam-three-net -p 127.0.0.1:18093:80 --mount "type=bind,source=$PWD/docs/caso03/nginx.conf,target=/etc/nginx/conf.d/default.conf,readonly" dockerexam-frontend:local
docker exec exam-three-front nginx -t
docker exec exam-three-front cat /etc/nginx/conf.d/default.conf
docker network inspect exam-three-net
docker ps --filter network=exam-three-net
curl.exe http://localhost:18093/api/health
# Abrir localhost:18093, guardar una nota y recargar para comprobar SQL.
```

### Retirar el laboratorio después de capturar

```powershell
docker rm -f exam-three-front exam-three-api exam-three-sql
docker network rm exam-three-net
# Se conservan los volúmenes de este caso.
```

**Evidencia que debes guardar:**

- Terminal con las IP SQL/API obtenidas mediante inspect y los tres contenedores.
- Configuración de API con IP SQL y archivo Nginx editado a mano con IP API.
- Portal, /api/health y nota recuperada después de recargar en localhost:18093. Subir capturas al portal principal en 18080.

En el stack principal usamos DNS de Docker. El laboratorio de tres contenedores usa IP explícita para cumplir este caso; las IP pueden cambiar al recrear. Confirma con el instructor qué imágenes se utilizaron en clase.

## 04 · Redes y conectividad

Gestionar redes personalizadas: creación, inspección, renombre y eliminación; conectar y desconectar contenedores existentes, asignar una red al crear y limpiar redes sin uso.

### Crear red y comprobar conectividad

```powershell
docker network create --label dockerexam.case04=true exam-red-a
docker run -d --name exam-network --network exam-red-a nginx:1.28-alpine
docker run -d --name exam-probe --network exam-red-a nginx:1.28-alpine
docker network inspect exam-red-a
docker exec exam-probe wget -qO- http://exam-network
```

### Migrar las conexiones a otra red

```powershell
docker network create --label dockerexam.case04=true exam-red-b
docker network connect exam-red-b exam-network
docker network connect exam-red-b exam-probe
docker network disconnect exam-red-a exam-network
docker network disconnect exam-red-a exam-probe
docker network inspect exam-red-b
docker exec exam-probe wget -qO- http://exam-network
docker network rm exam-red-a
```

### Eliminar y limpiar redes del caso

```powershell
docker rm -f exam-probe exam-network
docker network rm exam-red-b
docker network create --label dockerexam.case04=true exam-red-unused
docker network ls --filter label=dockerexam.case04=true
docker network prune --filter label=dockerexam.case04=true
docker network ls --filter label=dockerexam.case04=true
```

**Evidencia que debes guardar:**

- Inspect de ambas redes y contenedor conectado.
- Secuencia create → connect → disconnect → rm para migrar de nombre.
- Resultado de la limpieza filtrada por etiqueta.

Docker no tiene un comando network rename. Se documenta la alternativa real: crear otra red, migrar conexiones y eliminar la anterior. No ejecutar un comando inexistente.

## 05 · Creación de imágenes

Crear imágenes propias con Dockerfile y Dockerignore personalizados, generar contenedores y verificar que las aplicaciones funcionen.

### Construir y ejecutar imágenes propias del caso 05

```powershell
docker compose -f compose.caso5.yml stop
docker compose -f compose.caso5.yml config --quiet
docker build -t dockerexam-frontend:local-caso5 ./frontend
docker build -t dockerexam-api:local-caso5 ./backend
docker image ls dockerexam-frontend
docker image ls dockerexam-api
docker compose -f compose.caso5.yml up -d --no-build --wait --wait-timeout 300
curl.exe http://localhost:18150/api/health
```

### Inspeccionar capas, usuario y contenedores del caso 05

```powershell
docker history dockerexam-api:local-caso5
docker image inspect dockerexam-api:local-caso5 --format '{{.Config.User}}'
docker compose -f compose.caso5.yml ps
docker compose -f compose.caso5.yml images
```

**Evidencia que debes guardar:**

- Dockerfile y .dockerignore de cada aplicación.
- Build exitoso y listado de imágenes propias.
- Portal visible y endpoint de salud con SQL Server conectado.

Usar compose.caso5.yml: imágenes local-caso5, puertos 18150/18151/18152 y red/volúmenes independientes. El primer stop detiene el intento anterior del caso 05. Los datos son propios del laboratorio; subir las capturas al portal original en 18080. La API usa el usuario app.

## 06 · Publicar y ejecutar desde Docker Hub y Azure ACR

Publicar imágenes propias en dos o más repositorios públicos y privados, usando Docker Hub y una nube específica. Descargar, crear contenedores y verificar su funcionamiento.

Publica las dos imágenes del caso 05 en cuatro repositorios: frontend/API públicos en Docker Hub y frontend/API privados en ACR. Usa un Compose completo por registro. Consulta los [16 pasos de guide.md](guide.md) para crear cuentas/repositorios, sustituir nombres y obtener capturas.

### Paso 1: comprobar las dos imágenes locales del caso 05

```powershell
docker image inspect dockerexam-frontend:local-caso5 --format '{{.Id}}'
docker image inspect dockerexam-api:local-caso5 --format '{{.Id}}'
```

### Pasos 3 a 7: publicar frontend y API en Docker Hub público

```powershell
# Crear dos repositorios Public: dockerexam-frontend y dockerexam-api.
# Reemplazar tu_usuario en estos comandos y en las tres imágenes de compose.caso6.dockerhub.yml.
docker login
docker tag dockerexam-frontend:local-caso5 docker.io/tu_usuario/dockerexam-frontend:1.0.0
docker push docker.io/tu_usuario/dockerexam-frontend:1.0.0
docker tag dockerexam-api:local-caso5 docker.io/tu_usuario/dockerexam-api:1.0.0
docker push docker.io/tu_usuario/dockerexam-api:1.0.0
# Capturar los dos repositorios públicos, las etiquetas y ambos push exitosos.
```

### Paso 8: descargar desde Docker Hub

```powershell
docker compose -f compose.caso6.dockerhub.yml config --quiet
docker compose -f compose.caso6.dockerhub.yml config --images
docker compose -f compose.caso6.dockerhub.yml pull
# Continuar solo si la descarga finaliza correctamente.
```

### Paso 9: ejecutar, capturar y detener Docker Hub

```powershell
docker compose -f compose.caso6.dockerhub.yml up -d --no-build --pull never --wait --wait-timeout 300
docker compose -f compose.caso6.dockerhub.yml images
docker compose -f compose.caso6.dockerhub.yml ps
curl.exe http://localhost:18160/api/health
# Abrir localhost:18160, guardar una nota, recargar y capturar antes de detener.
docker compose -f compose.caso6.dockerhub.yml stop
```

### Pasos 10 y 11: autenticar ACR y configurar su Compose

```powershell
# Reemplazar nombre_acr por el nombre real del recurso Azure Container Registry.
az login
az acr login --name nombre_acr
az acr show --name nombre_acr --query loginServer --output tsv
# Copiar el servidor obtenido. Reemplazar tu-registro.azurecr.io en las tres imágenes de compose.caso6.acr.yml y en los comandos siguientes.
```

### Pasos 12 y 13: publicar frontend y API en ACR privado

```powershell
docker tag dockerexam-frontend:local-caso5 tu-registro.azurecr.io/dockerexam-frontend:1.0.0
docker push tu-registro.azurecr.io/dockerexam-frontend:1.0.0
docker tag dockerexam-api:local-caso5 tu-registro.azurecr.io/dockerexam-api:1.0.0
docker push tu-registro.azurecr.io/dockerexam-api:1.0.0
az acr repository list --name nombre_acr --output table
az acr show --name nombre_acr --query anonymousPullEnabled --output tsv
# Capturar ambos repositorios/etiquetas, los push y la descarga anónima deshabilitada (false).
```

### Paso 14: descargar desde ACR

```powershell
docker compose -f compose.caso6.acr.yml config --quiet
docker compose -f compose.caso6.acr.yml config --images
docker compose -f compose.caso6.acr.yml pull
# Continuar solo si la descarga autenticada finaliza correctamente.
```

### Paso 15: ejecutar, capturar y detener ACR

```powershell
docker compose -f compose.caso6.acr.yml up -d --no-build --pull never --wait --wait-timeout 300
docker compose -f compose.caso6.acr.yml images
docker compose -f compose.caso6.acr.yml ps
curl.exe http://localhost:18170/api/health
# Abrir localhost:18170, guardar una nota y recargar. Son datos separados del despliegue Docker Hub.
# Capturar antes de detener y subir todas las evidencias al caso 06 del portal original en 18080.
docker compose -f compose.caso6.acr.yml stop
```

**Evidencia que debes guardar:**

- 06-01-hub-publicos.png y 06-02-hub-push.png: ambos repositorios Public, etiquetas 1.0.0 y push correctos.
- 06-03-hub-pull.png y 06-04-hub-ejecucion.png: descarga Docker Hub, referencias usadas, servicios saludables y portal en 18160.
- 06-05-acr-privados.png y 06-06-acr-push.png: ambos repositorios ACR/etiquetas, descarga anónima deshabilitada y push correctos.
- 06-07-acr-pull.png y 06-08-acr-ejecucion.png: descarga ACR autenticada y funcionamiento en 18170. Subir todo al caso 06 del portal original en 18080.

Cuatro repositorios, dos registros y dos Compose completos. Docker Hub: compose.caso6.dockerhub.yml, puerto 18160. Azure ACR: compose.caso6.acr.yml, puerto 18170. Reemplazar los nombres de ejemplo antes de publicar/descargar. Cada despliegue tiene datos independientes. Seguir los 16 pasos de la Guía de sustentación y subir evidencias al portal original en 18080.


## 07 · Docker Compose

Crear un stack de servicios con al menos cuatro imágenes distintas propias y de terceros, cinco contenedores y uno expuesto en diferentes puertos.

### Desplegar y contar

```powershell
docker compose up -d --build
docker compose ps
docker compose images
docker compose config --services
```

### Verificar puertos e integración

```powershell
curl.exe http://localhost:18080/api/health
curl.exe http://localhost:18081/api/health
curl.exe http://localhost:18082/api/services
docker compose exec redis redis-cli GET dockerexam:visits
```

**Evidencia que debes guardar:**

- Compose con cinco servicios y cuatro imágenes distintas.
- docker compose ps con los cinco contenedores saludables.
- Navegador en puertos 18080 y 18081; contador Redis en el portal.

frontend y frontend-demo reutilizan la misma imagen propia. Redis participa como contador persistente de consultas del panel de servicios.

## 08 · Compose con Build y cambio de código

Crear un stack con Dockerfile y context en Compose, modificar código de una aplicación o API y regenerar contenedores verificando el cambio.

Utiliza compose.caso8.yml y sigue los [10 pasos de la guía](guide.md). Antes de editar, captura la versión y los IDs. Después demuestra código cambiado, build, nueva imagen, nuevo contenedor y nueva respuesta. El build del laboratorio utiliza context y Dockerfile explícitos; su portal responde en 18180.

### Pasos 1 a 3: validar, construir y levantar la versión inicial

```powershell
docker compose -f compose.caso8.yml config --quiet
docker compose -f compose.caso8.yml build frontend api
docker compose -f compose.caso8.yml up -d --no-build --wait --wait-timeout 300
docker compose -f compose.caso8.yml ps
# Capturar context/Dockerfile del Compose y esperar que el stack quede saludable.
```

### Paso 4: capturar código, respuesta, imagen y contenedor ANTES

```powershell
Get-Content .\backend\Configuration\AppInfo.cs
curl.exe http://localhost:18180/api/health
docker image inspect dockerexam-api:local-caso8 --format '{{.Id}}'
docker inspect dockerexam-caso8-api-1 --format '{{.Id}} {{.Image}}'
# Guardar las salidas para compararlas después. No modificar código antes de esta captura.
```

### Pasos 5 y 6: editar código a mano y comprobar que falta el build

```powershell
# Abrir backend/Configuration/AppInfo.cs en el editor.
# Cambiar Version de 1.0.0 a 1.0.1 y guardar con Ctrl+S.
# Si ya tiene otra versión, elegir una nueva y anotar ambos valores reales.
Get-Content .\backend\Configuration\AppInfo.cs
curl.exe http://localhost:18180/api/health
# El archivo muestra el valor nuevo, pero la API conserva el anterior hasta reconstruirla.
```

### Paso 7: reconstruir y recrear solo la API

```powershell
# SQL y Redis deben estar saludables desde el arranque inicial.
docker compose -f compose.caso8.yml up -d --build --no-deps --wait --wait-timeout 300 api
# Capturar el build, la exportación de imagen y la recreación/arranque saludable de la API.
```

### Pasos 8 y 9: comparar DESPUÉS y mostrar funcionamiento

```powershell
curl.exe http://localhost:18180/api/health
docker image inspect dockerexam-api:local-caso8 --format '{{.Id}}'
docker inspect dockerexam-caso8-api-1 --format '{{.Id}} {{.Image}}'
docker compose -f compose.caso8.yml ps api
# Comparar versión nueva, ID de imagen distinto y nuevo ID de contenedor.
# Abrir localhost:18180 y localhost:18180/api/health. Subir capturas al caso 08 del portal original en 18080.
```

### Paso 10: detener el caso 08 después de guardar las evidencias

```powershell
docker compose -f compose.caso8.yml stop
# Conservar los volúmenes y la contraseña. No usar down -v.
```

**Evidencia que debes guardar:**

- 08-01-compose-build.png: ambos context/Dockerfile y compilación de la API en el Dockerfile.
- 08-02-antes.png: versión inicial, ID de imagen e ID de contenedor.
- 08-03-codigo-cambiado.png: archivo AppInfo.cs con la línea editada y guardada.
- 08-04-build-recreacion.png: comando Compose, compilación/exportación y recreación exitosa.
- 08-05-despues.png: versión nueva, imagen distinta y nuevo contenedor usando esa imagen.
- 08-06-portal-funcionando.png: portal y health accesibles en 18180. Subir al caso 08 del portal original en 18080.

Laboratorio independiente compose.caso8.yml, imágenes local-caso8 y puertos 18180/18181/18182. La guía tiene 10 pasos. Cambiar la versión solo después de la captura inicial. Guardar el código no actualiza la imagen: demostrar código modificado, build, contenedor recreado y respuesta nueva. Los builds futuros de otros casos también incorporarán la versión que dejes en el archivo fuente.


## 09 · Secrets y persistencia tras eliminar/recrear contenedores

Crear y utilizar secrets con Compose o contenedores, usar volúmenes y demostrar persistencia después de eliminar contenedores y crear otros que reutilicen los volúmenes.

Sigue los [12 pasos de la guía](guide.md). Utiliza compose.caso9.yml y puerto 18190. Primero crea los recursos y verifica los volúmenes; después inicia los servicios. Un volumen existente no garantiza un SQL saludable. Si hay error de contraseña, utiliza el diagnóstico y las recuperaciones del paso 3 antes de guardar los datos de prueba. Ejecuta cada línea individualmente.

### Pasos 1 a 3: crear el archivo privado y levantar el caso 09

```powershell
# Crear a mano secrets/caso9_db_password.txt: solo la contraseña, una línea UTF-8 sin BOM.
# Conservarla si la base del caso 09 ya fue inicializada. No capturar su contenido.
Test-Path .\secrets\caso9_db_password.txt -PathType Leaf
Get-Item .\secrets\caso9_db_password.txt | Select-Object Name,Length,PSIsContainer
docker compose -f compose.caso9.yml config --quiet
docker compose -f compose.caso9.yml build frontend api
docker compose -f compose.caso9.yml create --no-build
docker volume inspect dockerexam-caso9-sql-data dockerexam-caso9-evidence-data dockerexam-caso9-redis-data --format '{{.Name}} | {{.CreatedAt}} | {{.Driver}}'
docker compose -f compose.caso9.yml up -d --no-build --wait --wait-timeout 300
docker compose -f compose.caso9.yml ps
curl.exe http://localhost:18190/api/health
# Si SQL queda unhealthy o aparece Login failed para sa, seguir la recuperación del paso 3 de la guía; no iniciar aún la evidencia.
```

### Paso 4: demostrar acceso y uso del secret sin mostrarlo

```powershell
docker compose -f compose.caso9.yml exec api printenv Database__PasswordFile
docker compose -f compose.caso9.yml exec api ls -l /run/secrets/db_password
docker compose -f compose.caso9.yml exec api test -r /run/secrets/db_password
$LASTEXITCODE
docker compose -f compose.caso9.yml exec sqlserver test -r /run/secrets/db_password
$LASTEXITCODE
curl.exe http://localhost:18190/api/health
# Ambas comprobaciones deben devolver 0 y la salud debe ser ok. No mostrar la contraseña.
```

### Paso 5: guardar nota, imagen y marca Redis ANTES

```powershell
# En localhost:18190, caso 09: guardar nota única y subir una captura real con título/descripción.
# Recargar y abrir Ver detalle. Capturar los datos ANTES de eliminar contenedores.
docker compose -f compose.caso9.yml exec redis redis-cli SET caso09:marca PRUEBA-PERSISTENCIA-CASO09
docker compose -f compose.caso9.yml exec redis redis-cli GET caso09:marca
docker compose -f compose.caso9.yml exec api ls -l /data/evidence
```

### Paso 6: registrar IDs, volúmenes y montajes originales

```powershell
docker inspect dockerexam-caso9-api-1 dockerexam-caso9-sqlserver-1 dockerexam-caso9-redis-1 --format '{{.Name}} {{.Id}}'
docker volume inspect dockerexam-caso9-sql-data dockerexam-caso9-evidence-data dockerexam-caso9-redis-data --format '{{.Name}} | {{.CreatedAt}} | {{.Driver}}'
docker inspect dockerexam-caso9-api-1 dockerexam-caso9-sqlserver-1 dockerexam-caso9-redis-1 --format '{{.Name}} {{json .Mounts}}'
# Guardar las salidas para comparar después.
```

### Pasos 7 y 8: detener y eliminar solo los contenedores del caso 09

```powershell
# Continuar solo después de guardar nota, imagen, marca Redis y todas las capturas anteriores.
docker compose -f compose.caso9.yml stop
docker compose -f compose.caso9.yml ps -a
docker compose -f compose.caso9.yml rm -f
docker compose -f compose.caso9.yml ps -a
docker volume inspect dockerexam-caso9-sql-data dockerexam-caso9-evidence-data dockerexam-caso9-redis-data --format '{{.Name}} | {{.CreatedAt}} | {{.Driver}}'
# Capturar eliminación, listado vacío y volúmenes conservados. No usar down -v.
```

### Paso 9: crear contenedores nuevos reutilizando los volúmenes

```powershell
docker compose -f compose.caso9.yml up -d --no-build --pull never --wait --wait-timeout 300
docker compose -f compose.caso9.yml ps
docker inspect dockerexam-caso9-api-1 dockerexam-caso9-sqlserver-1 dockerexam-caso9-redis-1 --format '{{.Name}} {{.Id}}'
# Comparar con los IDs anteriores: deben cambiar.
```

### Pasos 10 y 11: comprobar montajes y los mismos datos DESPUÉS

```powershell
docker volume inspect dockerexam-caso9-sql-data dockerexam-caso9-evidence-data dockerexam-caso9-redis-data --format '{{.Name}} | {{.CreatedAt}} | {{.Driver}}'
docker inspect dockerexam-caso9-api-1 dockerexam-caso9-sqlserver-1 dockerexam-caso9-redis-1 --format '{{.Name}} {{json .Mounts}}'
docker compose -f compose.caso9.yml exec redis redis-cli GET caso09:marca
docker compose -f compose.caso9.yml exec api ls -l /data/evidence
curl.exe http://localhost:18190/api/health
# Abrir localhost:18190 en ventana privada. Comparar nota, título/descripción e imagen abierta.
# Nombres/fechas de volúmenes y marca Redis deben conservarse. Subir capturas al portal original en 18080.
```

### Paso 12: conservar el laboratorio después de la evidencia

```powershell
docker compose -f compose.caso9.yml stop
# Mantener los volúmenes y secrets/caso9_db_password.txt para la sustentación.
```

**Evidencia que debes guardar:**

- 09-01-secrets-compose.png: archivo declarado, consumidores API/SQL y volúmenes.
- 09-02-secret-uso.png: acceso legible y conexión SQL correcta sin mostrar la contraseña.
- 09-03-datos-antes.png: nota, imagen con título/descripción y marca Redis guardadas.
- 09-04-ids-volumenes-antes.png: IDs originales, nombres/fechas de volúmenes y montajes.
- 09-05-eliminacion.png: cinco contenedores eliminados, listado vacío y volúmenes conservados.
- 09-06-recreacion.png: cinco servicios saludables e IDs nuevos.
- 09-07-montajes-redis-despues.png: mismos volúmenes/montajes, archivo de imagen y marca Redis.
- 09-08-datos-despues.png: misma nota, título/descripción e imagen abierta en 18190 después de recrear.

Usar compose.caso9.yml y puerto 18190; preparar a mano su archivo secrets/caso9_db_password.txt. La guía contiene 12 pasos. Eliminar solo los contenedores de este laboratorio después de guardar los datos de prueba. Mantener contraseña y volúmenes; no usar down -v. IDs de contenedor nuevos y datos iguales demuestran persistencia. Compose local monta secrets por archivo; no es el almacén cifrado de Swarm. Subir las capturas al portal original en 18080. Un error 18456 estado 8 indica contraseña distinta de la guardada en SQL. Editar el secret no cambia sa en una base existente. La guía incluye recuperación conservando datos y reinicio inicial solo para un laboratorio vacío.


## 10 · Comandos útiles para operar y diagnosticar Docker

Incluir otros comandos, aplicaciones o usos de Docker y Compose investigados como complemento del entrenamiento.

Usa el portal original (compose.yml, puerto 18080). La [guía del caso 10](guide.md) explica para cada grupo qué observar y ofrece títulos/descripciones para evidencias. Son seis grupos principales y dos opcionales; la cantidad de capturas es una propuesta, no un mínimo de la rúbrica.

### 1. Salud del stack y últimos healthchecks

```powershell
docker compose -f compose.yml config --quiet
docker compose -f compose.yml ps
docker inspect dockerexam-api-1 --format 'Estado={{.State.Status}} Salud={{.State.Health.Status}}'
docker inspect dockerexam-api-1 --format '{{json .State.Health.Log}}'
curl.exe http://localhost:18080/api/health
```

### 2. CPU, memoria y procesos

```powershell
docker compose -f compose.yml stats --no-stream
docker compose -f compose.yml top api
# Explicar consumo real, NET/BLOCK I/O y la diferencia entre PIDS y procesos mostrados por top.
```

### 3. Usuario de ejecución y privilegios

```powershell
docker compose -f compose.yml exec api id
docker inspect dockerexam-api-1 --format 'Usuario={{.Config.User}} Privileged={{.HostConfig.Privileged}}'
# Relacionar el usuario app/UID distinto de 0 con USER app del Dockerfile.
```

### 4. Red, DNS interno y puertos publicados

```powershell
docker network inspect dockerexam-net --format '{{range .Containers}}{{.Name}} {{.IPv4Address}}{{println}}{{end}}'
docker compose -f compose.yml exec frontend wget -qO- http://api:8080/api/health
docker port dockerexam-frontend-1
docker ps --filter publish=18080 --format 'table {{.Names}}\t{{.Ports}}'
# La petición wget sale del frontend hacia el servicio api por su nombre DNS.
```

### 5. Logs con fechas, reinicios y OOM

```powershell
docker compose -f compose.yml logs --tail 40 --timestamps api
docker compose -f compose.yml logs --since 10m --tail 40 sqlserver
docker inspect dockerexam-api-1 --format 'Estado={{.State.Status}} Reinicios={{.RestartCount}} OOMKilled={{.State.OOMKilled}}'
# Si no hay mensajes dentro del período, ampliar con logs --tail 40 sqlserver.
# Opcional: logs --follow --tail 20 api. Ctrl+C termina la consulta, no el contenedor.
```

### 6. Uso de disco e historial de capas

```powershell
docker system df
docker image ls dockerexam-api
docker image history dockerexam-api:local
# system df informa sobre todo el motor Docker. Explicar categorías y capas sin eliminar recursos.
```

### 7. Opcional: Redis y contador del panel de servicios

```powershell
docker compose -f compose.yml exec redis redis-cli PING
curl.exe http://localhost:18080/api/services
docker compose -f compose.yml exec redis redis-cli GET dockerexam:visits
curl.exe http://localhost:18080/api/services
docker compose -f compose.yml exec redis redis-cli GET dockerexam:visits
# Cada consulta /api/services incrementa el contador; otras visitas también pueden hacerlo.
```

### 8. Opcional: cambios del sistema de archivos

```powershell
docker diff dockerexam-frontend-1
# Explicar A=añadido, C=modificado y D=eliminado con las rutas reales que aparezcan.
# Una salida vacía puede ser válida; no sustituye la prueba de persistencia del caso 09.
```

**Evidencia que puedes guardar:**

- 10-01-salud.png: ps, healthcheck reciente y respuesta de API/SQL.
- 10-02-recursos-procesos.png: stats y top con interpretación de valores reales.
- 10-03-usuario-privilegios.png: id, usuario app y modo privilegiado.
- 10-04-red-puertos.png: red, petición por DNS y publicaciones del frontend.
- 10-05-logs-reinicios.png: mensajes con fecha, RestartCount y OOMKilled.
- 10-06-almacenamiento-capas.png: system df, imágenes e historial de capas.
- Opcional 10-07-redis-servicios.png: PONG, estados de servicios y contador Redis.
- Opcional 10-08-cambios-archivos.png: diff e interpretación de A/C/D o de una salida vacía.

Usar compose.yml, proyecto dockerexam y portal original en 18080. La guía incluye seis grupos principales y dos opcionales, con utilidad, interpretación, título y descripción sugeridos. La rúbrica no fija una cantidad de capturas: registrar las demostraciones realmente ejecutadas y explicarlas. No hace falta otro Compose. Las consultas /api/services incrementan el contador Redis.



