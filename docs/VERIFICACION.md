# Verificación del proyecto

Fecha: 28 de octubre de 2026 (America/Lima).

## Comprobaciones realizadas

### Ampliación del 2 de noviembre de 2026: PDF de todo el trabajo guardado

- Frontend Angular compilado en producción dentro de Node 24 y backend .NET 10 compilado y publicado dentro de Docker. Compilación del backend sin errores ni advertencias.
- Se conserva el endpoint `GET /api/reports/final.pdf` y se reutiliza `ExamService` para recuperar el mismo trabajo guardado que utiliza el portal. Se permite descargar cualquier avance.
- Prueba HTTP aislada con EF Core InMemory, catálogo real y datos sintéticos: PDF válido con 0/10, 8/10 y 10/10 (`200`, adjunto `application/pdf`, `Cache-Control: no-store`), generación concurrente rechazada (`429`) e imágenes ausentes o dañadas rechazadas (`409`). El informe parcial conserva las notas e imágenes de casos pendientes o en progreso y muestra el porcentaje real; el completo se identifica como informe final.
- PDF revisado con capturas PNG/JPEG/WebP horizontales y verticales, notas extensas, Unicode español, HTML con formato y Markdown histórico. La extracción confirma el caso 10, el final de las descripciones extensas y los avisos de casos sin evidencia. Se comprobaron resolución original de las imágenes y orientación de página adaptada a las capturas.
- Prueba con Chrome sin interfaz: botón habilitado en 9/10 y 10/10, descarga real del PDF parcial, mensaje de éxito, lectura de errores ProblemDetails recibidos como Blob, posibilidad de reintentar y ausencia de errores JavaScript. Revisión de Vista general, Recursos y entrega y formato móvil.
- Las pruebas utilizaron exclusivamente una base en memoria e imágenes sintéticas en `tmp/`. No se marcaron como listos casos del alumno, no se añadieron evidencias a su base y no se recrearon los volúmenes reales.

### Verificación previa del portal

- Angular modular compilado en modo producción con comprobación estricta de plantillas; el editor visual y el formulario de edición se cargan en módulos diferidos.
- API .NET compilada y publicada dentro de Docker. Se actualizó la dependencia transitiva PKCS indicada por NuGet; el build final no mostró advertencias de vulnerabilidad.
- Stack principal con cinco servicios: frontend, frontend-demo, api, sqlserver y redis; cuatro imágenes distintas.
- Puertos del frontend 18080, 18081 y 18082 comprobados mediante HTTP. Se cambió el puerto inicial porque Windows rechazó su publicación.
- Diez componentes disponibles en `/api/exam`, con 20 puntos posibles en total, requisitos, pasos, comandos y guías de evidencia.
- SQL Server conectado y guardado/lectura de notas con caracteres Unicode.
- Redis conectado y contador incrementándose.
- Descarga del PDF, Cheat Sheet y guía de sustentación.
- Copia del PDF idéntica al original según SHA-256.
- Carga, recuperación binaria y eliminación de una imagen PNG temporal.
- Rechazo de archivo inválido, estado no permitido y enlace de repositorio no HTTPS.
- API ejecutándose como `uid=1654(app)`, sin privilegios de root.
- Refactorización a controladores, servicios, contratos y mapeos EF Core separados, manteniendo las rutas HTTP y tablas existentes.
- Angular dividido en core, features y shared, con HttpClient tipado y estado común basado en signals.
- Rediseño del portal con tema azul, iconos SVG reutilizables, resumen de avance, tarjetas por caso y navegación lateral adaptable. Estilos organizados en tokens, base, layout, componentes y estilos propios de la vista general. No requiere fuentes de servicios externos.
- Swagger UI, su inicializador y su bundle servidos por `/api/swagger`; OpenAPI con once operaciones documentadas, respuestas tipadas y archivo multipart descrito como string/binary.
- Pruebas de validación con respuestas ProblemDetails, incluido estado inválido, notas nulas y tipo de contenido incorrecto.
- Persistencia real: se guardaron una nota y un PNG temporales, se detuvieron los frontends, se eliminaron los contenedores API y SQL Server, se crearon otros con los mismos volúmenes y se verificaron la nota, los metadatos y la imagen. Luego se restauró el avance original y se eliminó el PNG temporal.
- Laboratorio del caso 03 ejecutado: tres contenedores independientes, API conectada por la IP inspeccionada de SQL y Nginx configurado con la IP inspeccionada de la API. `/api/health` y los diez casos respondieron por el puerto 18093.

- Editor visual Tiptap con negrita, cursiva, subrayado, resaltado, subtítulos, listas, citas, código y deshacer/rehacer. Las descripciones antiguas en Markdown se renderizan con formato.
- GET y PUT `/api/evidence/{id}` para detalle y edición. Las pruebas verifican formato HTML, limpieza de contenido no permitido, descripciones extensas, validaciones y conservación de la imagen al editar.
- Las dos evidencias originales conservan sus títulos, descripciones, metadatos e imágenes tras la ampliación del esquema. Se eliminaron los registros temporales de prueba.

## Verificación manual vigente

Actualización: 1 de noviembre de 2026 (America/Lima). Se retiraron los archivos de automatización del laboratorio y la prueba de integración. Las comprobaciones anteriores se conservan como registro histórico; no representan una nueva ejecución ni evidencia del alumno.

Sigue la [guía manual de los diez casos](../backend/Content/guide.md). Ejecuta individualmente los comandos y comprueba desde el navegador:

1. Portal en 18080, 18081 y 18082; salud y servicios accesibles.
2. Guardado de una nota y recuperación después de recargar.
3. Carga de una captura real, apertura de detalle, zoom y edición de título/descripción.
4. Mismos datos desde las otras instancias del frontend.
5. Laboratorio separado por IP y capturas de sus configuraciones manuales.
6. Push, pull y ejecución desde Docker Hub y ACR con cuentas reales.
7. Nueva versión al modificar código y reconstruir la API.
8. Nota e imagen conservadas al eliminar y recrear API/SQL, con IDs diferentes y los mismos volúmenes.

En esta corrección se revisan documentos, catálogo y referencias; no se ejecutan los casos, no se generan evidencias y no se modifican los datos del portal.

## Alcance y pendientes

No se realizó una inspección visual en navegador: la herramienta de control de aplicaciones detuvo la sesión al no poder determinar con confianza la URL de Chrome. La compilación de Angular y las comprobaciones HTTP sí finalizaron correctamente.

No se publicaron imágenes en registros ni se creó un repositorio remoto. La guía vigente deja comandos individuales y pasos manuales para completarlo con las cuentas del alumno. Los datos temporales de las pruebas fueron retirados; las capturas y el avance registrados por el alumno se conservan.

Este registro describe la verificación técnica de la implementación. La sustentación y la calificación de los diez componentes siguen dependiendo de la ejecución y evidencia reales que el alumno presente.

