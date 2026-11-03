# Docker Lab · Examen final

Portal de sustentación de **JHONATAN BRICEÑO CUÑACHI CANAQUIRI**, curso Docker & Docker Compose for Developers (CURS-000662).

Angular 21, API .NET 10, SQL Server 2022 Developer y Redis. El portal permite registrar notas y capturas reales de los diez casos. Cada caso se realiza manualmente con comandos individuales y configuraciones editadas por el alumno.

## Empezar manualmente

1. Inicia Docker Desktop con contenedores Linux y abre PowerShell en la raíz del proyecto.
2. Si ya existe `secrets/db_password.txt`, consérvalo. En una primera instalación créalo con el editor: una contraseña propia de al menos 16 caracteres ASCII, con mayúsculas, minúsculas, números y símbolos, sin espacios ni comillas. Guarda en UTF-8 sin BOM, en una sola línea. No publiques ese archivo.
3. Ejecuta una línea a la vez:

```powershell
docker version
docker compose version
docker compose config --quiet
docker compose up -d --build --wait --wait-timeout 300
docker compose ps
```

Abre **http://localhost:18080**. El mismo frontend responde en **18081** y una segunda instancia en **18082**. Swagger: **http://localhost:18080/api/swagger/index.html**. SQL y Redis solo utilizan puertos internos. Se recomienda asignar al motor al menos 6 GB de RAM para el stack.

## Levantar evidencia según la rúbrica

Sigue la [guía de sustentación de 10 minutos](backend/Content/guide.md): incluye una agenda de 600 segundos, un guion de apertura, los diez casos y el cierre, con qué explicar y qué evidencia mostrar en cada bloque. Prepara antes los builds, publicaciones y pruebas de recreación; durante la exposición combina el portal en vivo con tus resultados documentados. El manual técnico completo se conserva después del guion, con comandos, archivos, comprobaciones y capturas por caso. La guía también se descarga desde Recursos y entrega del portal. Usa el [Cheat Sheet personalizado](backend/Content/cheatsheet.md) para consultar los comandos durante la exposición.

Para el caso 05 utiliza [compose.caso5.yml](compose.caso5.yml), con imágenes `local-caso5`, puertos 18150/18151/18152 y red/volúmenes independientes del portal original. Sigue los comandos individuales de la guía: primero detén el intento anterior del caso 05, construye ambas imágenes y levanta ese archivo con `--no-build`. Abre `http://localhost:18150` y registra las capturas en el portal original en 18080. `compose_a.yml` conserva el respaldo original y `compose.yml` está restaurado con ese contenido.

El caso 06 tiene **16 pasos** en la guía y publica dos imágenes en **cuatro repositorios**: frontend/API públicos en Docker Hub y frontend/API privados en Azure ACR. Usa [compose.caso6.dockerhub.yml](compose.caso6.dockerhub.yml) para Docker Hub (18160/18161/18162) y [compose.caso6.acr.yml](compose.caso6.acr.yml) para ACR (18170/18171/18172). Cada archivo tiene proyecto, red y volúmenes propios. Antes de ejecutar, reemplaza `tu_usuario` en las tres referencias de imágenes Docker Hub y `tu-registro.azurecr.io` en las tres referencias ACR por el servidor real obtenido con `az acr show`. Publica, descarga, ejecuta y captura primero Docker Hub; detén ese despliegue y continúa con ACR. Los comandos son individuales y las imágenes fuente son las `local-caso5` ya construidas. Sube las capturas al portal original en 18080.

Para el caso 08 utiliza [compose.caso8.yml](compose.caso8.yml), con `context` y `dockerfile` explícitos, imágenes `local-caso8`, puertos 18180/18181/18182 y almacenamiento independiente. La guía contiene **10 pasos**: levantar la versión inicial, capturar respuesta/IDs, cambiar manualmente `AppInfo.Version`, reconstruir/recrear la API y comparar la versión, la imagen y el contenedor. Conserva la versión inicial hasta obtener su captura. El portal del laboratorio abre en `http://localhost:18180`; registra las seis evidencias en el portal original en 18080.

Para el caso 09 utiliza [compose.caso9.yml](compose.caso9.yml), con puertos 18190/18191/18192, imágenes `local-caso9`, red y volúmenes exclusivos. **Primero crea a mano `secrets/caso9_db_password.txt`** siguiendo la guía; conserva esa contraseña al reutilizar su base. El recorrido tiene **12 pasos**: demostrar el secret sin revelar su valor, guardar nota/imagen/marca Redis, registrar IDs y volúmenes, eliminar los cinco contenedores y crear otros con los mismos volúmenes. Compara IDs nuevos, volúmenes conservados y datos recuperados. Guarda las ocho evidencias en el caso 09 del portal original en 18080; el laboratorio se abre en `http://localhost:18190`.

El paso 3 del caso 09 separa `create` (preparar contenedores/volúmenes) de `up` (iniciar) y explica dos fallos diferentes: contraseña rechazada por complejidad y error SQL 18456 por contraseña que no coincide con la almacenada. Incluye recuperación conservando datos y una alternativa de reinicio inicial exclusiva para un laboratorio sin datos que conservar. Durante la demostración de persistencia se mantienen todos los volúmenes.

Para el caso 10 utiliza el portal original con `compose.yml`; no hace falta otro Compose. La guía propone seis grupos principales de comandos (salud, recursos/procesos, usuario/privilegios, red/puertos, logs/reinicios y disco/capas) y dos complementarios (Redis y diff). Cada grupo incluye utilidad, interpretación, nombre de captura, título y descripción sugeridos. Registra tus salidas reales en el caso 10 de `http://localhost:18080` y explica qué problema permite investigar cada comando.

| Caso | Evidencia que debes obtener |
| --- | --- |
| 01 · Cheat Sheet | Hoja personalizada y ejecución de comandos documentados |
| 02 · Contenedores | Creación, inspect, logs, rename, parada, reinicio, rm y rm -f |
| 03 · Tres por IP | IP SQL/API con inspect, configuración manual y flujo frontend → API → SQL en 18093 |
| 04 · Redes | Creación, conectividad, migración de conexiones, eliminación y prune filtrado |
| 05 · Imágenes | Ambos Dockerfile/.dockerignore, builds y aplicaciones funcionando |
| 06 · Registros | Repositorios públicos y privados, push, pull y ejecución desde Docker Hub y ACR |
| 07 · Compose | Cinco contenedores, cuatro imágenes y un frontend en dos puertos |
| 08 · Build | Cambio manual de AppInfo.Version, reconstrucción y versión antes/después |
| 09 · Secrets/volúmenes | Montajes, nota e imagen antes/después y nuevos IDs al recrear API/SQL |
| 10 · Investigación | Salud, recursos/procesos, usuario, red/puertos, logs y almacenamiento; Redis/diff opcionales |

Crea subcarpetas por caso dentro de `EVINDENCIA`. Captura comando y resultado con `Win + Mayús + S`, guarda el PNG y súbelo al caso correspondiente con título y descripción. No captures contraseñas ni tokens. Adjunta también el código y las configuraciones al repositorio remoto y registra su enlace en el portal.

Son diez componentes de 2 puntos (20 en total). La rúbrica exige presentación en vivo. No marques demostraciones como completas por tener instrucciones. El caso 03 exige imágenes distintas de las utilizadas en clase: confirma la selección con el instructor. Docker no ofrece `network rename`; la guía explica la migración a otra red y cómo presentar esa limitación.

## Descargar un PDF con todo el trabajo realizado

1. En cada caso guarda tus notas de desarrollo, carga las capturas reales y completa sus títulos y descripciones. Guarda el estado **Listo para revisar** al terminarlo.
2. En **Vista general** y **Recursos y entrega**, pulsa **Descargar PDF**. Puedes hacerlo en cualquier momento, aunque haya casos pendientes o en progreso. Al completar los diez casos, la portada cambia a informe final.
3. Revisa tu nombre y enlace del repositorio antes de descargar. El PDF incluye portada, resumen, los diez requisitos, notas guardadas, capturas y descripciones con formato HTML o Markdown. Las capturas verticales usan páginas verticales y las horizontales, páginas apaisadas; las descripciones extensas continúan en otras páginas.
4. Abre el archivo descargado y comprueba el contenido antes de entregarlo. El informe refleja tu avance declarado; la calificación corresponde al docente. Si un caso no tiene notas o evidencias, el documento lo indica. Si falta una imagen guardada o está dañada, corrígela y vuelve a generar el PDF.

Para aplicar esta actualización a un **portal principal que ya está funcionando**, ejecuta manualmente, una línea por vez, desde la raíz:

```powershell
docker compose build api frontend
docker compose up -d --no-deps --wait --wait-timeout 300 api
docker compose up -d --no-deps --wait --wait-timeout 120 frontend frontend-demo
```

Después recarga `http://localhost:18080` con `Ctrl + F5`. Estos comandos actualizan las aplicaciones y reutilizan los volúmenes existentes. Para una primera instalación utiliza los pasos de inicio de este README.

Se reutiliza `GET /api/reports/final.pdf`, conservando su URL, y el servicio de consulta del examen. El PDF incluye todos los casos con su estado real y todas las evidencias registradas, también las de casos pendientes o en progreso. Responde `409` si no puede incluir una imagen y `429` si hay otra generación en curso. La descarga no modifica estados ni datos. QuestPDF se configura con licencia Community por el uso educativo de este proyecto.

## Datos y configuración

- `compose.yml`: cinco servicios, cuatro imágenes, red, secrets y volúmenes.
- `compose.caso5.yml`: laboratorio independiente del caso 05, con datos propios.
- `compose.caso6.dockerhub.yml`: ejecución del caso 06 desde los repositorios públicos Docker Hub.
- `compose.caso6.acr.yml`: ejecución del caso 06 desde los repositorios privados Azure ACR.
- `compose.caso8.yml`: build con context/Dockerfile y demostración del cambio de código en un laboratorio independiente.
- `compose.caso9.yml`: secret propio y persistencia de SQL, imágenes y Redis tras eliminar/recrear los contenedores del laboratorio.
- `frontend/` y `backend/`: código y Dockerfile de las aplicaciones.
- `backend/Content/exam.json`: requisitos, comandos manuales y guía de captura del portal.
- `backend/Content/rubric.pdf`: copia del PDF original.
- [Arquitectura](docs/ARQUITECTURA.md): organización y extensión del código.
- [Verificación](docs/VERIFICACION.md): registro técnico previo y comprobaciones manuales vigentes.

SQL conserva perfil, estados, notas y metadatos. `dockerexam-evidence-data` conserva imágenes PNG/JPG/WebP de hasta 8 MB; `dockerexam-sql-data` conserva SQL y `dockerexam-redis-data` el contador. La interfaz permite consultar y editar títulos y descripciones de evidencia, con formato y zoom de la imagen.

`docker compose down` conserva los volúmenes. **`docker compose down -v` elimina los datos y las evidencias**: no se utiliza en el caso 09. Conserva el mismo archivo de contraseña al reutilizar una base existente. Compose local monta el secret desde archivo. El portal está preparado para exposición local y publica puertos solo en loopback.

Las publicaciones en Docker Hub/ACR y el repositorio remoto se completan con tus propias cuentas. La guía contiene los pasos manuales y los valores que debes sustituir. No se han publicado imágenes ni creado recursos de nube como parte de esta corrección.

## Créditos y fuentes

Rúbrica: Galaxy Training, instructor Aristedes Novoa Arbildo. `commands.MD` conserva el material de apoyo del curso. Imágenes base de Microsoft, Node.js, Nginx y Redis.

- [Docker CLI](https://docs.docker.com/reference/cli/docker/)
- [Compose up](https://docs.docker.com/reference/cli/docker/compose/up/)
- [Compose secrets](https://docs.docker.com/compose/how-tos/use-secrets/)
- [Azure Container Registry](https://learn.microsoft.com/azure/container-registry/container-registry-get-started-docker-cli)
