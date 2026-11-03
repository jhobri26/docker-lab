# Docker Cheat Sheet · Docker Lab

Alumno: JHONATAN BRICEÑO CUÑACHI CANAQUIRI. Curso: CURS-000662.
Comandos para ejecutar desde la raíz del proyecto en PowerShell. Consultar el portal para la explicación de cada caso.


## **Docker**
### Trabajando con imágenes del container registry

- **Descargar una imagen desde Docker Hub:**
```shell 
#docker pull nombre_de_la_imagen
docker run -d -p 80:80 docker/getting-started
```
- **Listar imágenes locales:**
```shell 
docker image ls
```
- **Crear y ejecutar un contenedor a partir de una imagen:**
```shell 
#docker run [opciones] nombre_del_contenedor nombre_de_la_imagen
docker run -p 80:80 -p 7080:7080 --name containerBilling sotobotero/billingapp
```
- **crear un contenedor de postgres y una base de datos en el mismo comando:**
```shell
docker run --ulimit memlock=-1:-1 -d --name postgres -e POSTGRES_USER=sa -e POSTGRES_PASSWORD=admin -e POSTGRES_DB=product_db -p 5432:5432 postgres:13.3
```
- **Listar contenedores en ejecución:**
```shell 
docker ps
```
- **Listar todos los contenedores (incluso los detenidos):**
```shell 
docker ps -a
```
- **Detener todos los contenedores:**
```shell 
docker stop $(docker ps -q)
```
- **Iniciar todos los contenedores:**
```shell 
docker start $(docker ps -a -q)
```
- **Eliminar todos los contenedores:**
```shell 
docker rm $(docker ps -a -q)
```
### Construyendo Imágenes

- **Construir una imagen desde un Dockerfile:**
 ```shell
 #docker build -t nombre_de_la_imagen [opciones y parametros] .
 docker build -t billingapp --no-cache --build-arg JAR_FILE=target/*.jar .
```
- **Construir una imagen con un contexto específico:**
 ```shell
  docker build -t nombre_de_la_imagen -f ruta/Dockerfile .
```
### Comandos Avanzados para Imágenes

- **Etiquetar una imagen:**
 ```shell
 docker tag nombre_de_la_imagen nueva_etiqueta
```
- **Loguears en un registry:**
 ```shell
docker login # te pedirá usuario y contraseña
 ```
- **Subir una imagen a Docker Hub:**
 ```shell
  docker push nombre_de_la_imagen
```
- **Descargar una imagen con una etiqueta específica:**
 ```shell
  docker pull nombre_de_la_imagen:etiqueta
```
- **Desloguearse de un registry:**
 ```shell
docker logout
 ```
### Comandos para la Gestión Básica de Contenedores

- **Detener un contenedor en ejecución:**
```shell 
docker stop nombre_del_contenedor
```

- **Iniciar un contenedor detenido:**
```shell 
docker start nombre_del_contenedor
```
- **Eliminar un contenedor:**
```shell 
docker rm nombre_del_contenedor
```
- **Eliminar una imagen local:**
```shell
docker image rm nombre_de_la_imagen
```
### Trabajando con Redes y Volúmenes

- **Crear una red personalizada:**
 ```shell 
 docker network create nombre_de_la_red
```
- **Listar redes:**
 ```shell
 docker network ls
```
- **Crear un volumen:**
 ```shell
 docker volume create nombre_del_volumen
```
- **Listar volúmenes:**
 ```shell
 docker volume ls
```
### Interactuando con Contenedores

- **Ejecutar un comando en un contenedor en ejecución:**
 ```shell
 #docker exec -it nombre_del_contenedor comando
docker exec -it localbillingApp sh
```
- **Copiar archivos entre el host y un contenedor:**
 ```shell
  docker cp archivo.txt nombre_del_contenedor:/ruta/destino
  ```
### Docker Compose

- **Iniciar contenedores definidos en un archivo `docker-compose.yml`:**
 ```shell
  docker-compose up
```
- **Escalar servicios en Docker Compose:**
 ```shell
  docker-compose scale servicio=num_instancias
```
- **Detener y eliminar contenedores definidos en Docker Compose:**
 ```shell
  docker-compose down
```
- **Ver logs de servicios en Docker Compose:**
 ```shell
  docker-compose logs servicio
```
### Administración de Redes y Volúmenes

- **Eliminar una red:**
 ```shell
  docker network rm nombre_de_la_red
```
- **Eliminar un volumen:**
 ```shell
  docker volume rm nombre_del_volumen
```
- **Inspeccionar una red:**
 ```shell
 docker network inspect nombre_de_la_red
```
### Comandos para Información y Estadísticas

- **Ver detalles de un contenedor:**
 ```shell
  docker inspect nombre_del_contenedor
```
- **Obtener estadísticas de uso de recursos de un contenedor:**
 ```shell
  docker stats nombre_del_contenedor
```
- **Obtener estadísticas de uso de recursos de todos los contenedores:**
 ```shell
  docker stats
```
### Comandos para Seguridad y Control de Acceso

- **Crear un perfil de control de acceso (se requiere Docker EE):**
 ```shell
  docker trust key generate nombre_de_perfil
```
- **Especificar qué usuarios o equipos pueden o no usar Docker:**
 ```shell
  docker trust key load --alias nombre_de_perfil.pem --pem --root /etc/docker/pki/tls/private
```

- **Inhabilitar Docker para usuarios no autorizados (se requiere Docker EE):**
 ```shell
  docker trust signer remove nombre_de_perfil
```
### Comandos para Configuraciones Avanzadas

- **Crear un contenedor con un archivo de configuración personalizada:**
 ```shell
  docker run -v ruta/local:/ruta/contenedor -e variable_de_entorno=valor nombre_de_la_imagen
```
- **Especificar recursos de sistema para un contenedor (CPU, memoria, etc.):**
 ```shell
 docker run --cpu-shares=512 -m 256m nombre_de_la_imagen
```
- **Elimina todos los contenedores detenidos e imagenes que no esten en uso**
 ```shell
docker system prune --all
 ```
 - **Elimina todos los volumenes que no esten en uso**
 ```shell
docker volume prune
 ```
 - **Elimina todas las redes que no esten en uso, menos las por defecto (bridge, none,host)**
 ```shell
docker network prune
 ```

## Recorrido manual de los diez casos

La referencia del curso anterior se conserva. Para el proyecto final consulta [la hoja personalizada actualizada](cheatsheet.md) y [la guía manual de evidencia](guide.md): incluyen comandos individuales, configuración por IP, ejecución desde ambos registros y persistencia. Crea los archivos indicados con el editor y captura tus resultados reales.

